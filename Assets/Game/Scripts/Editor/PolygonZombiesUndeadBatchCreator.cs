using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

namespace baodeag.EditorTools
{
    public static class PolygonZombiesUndeadBatchCreator
    {
        private const string TemplatePrefabPath = GameAssetPaths.PrefabsRoot + "/Character/Undead/Undead_Dummy_01.prefab";
        private const string SourcePrefabsFolder = GameAssetPaths.PolygonZombiesRoot + "/Prefabs";
        private const string TargetPrefabsFolder = GameAssetPaths.PrefabsRoot + "/Character/Undead";
        private const string AutoRunSessionKey = "PolygonZombiesUndeadBatchCreator.AutoRunAttempted.V2";
        private const int DamageableCharacterLayer = 7;
        private const int DamageColliderLayer = 10;

        static PolygonZombiesUndeadBatchCreator()
        {
            // Disabled autorun to avoid mutating prefabs during editor startup.
        }

        [MenuItem("Tools/Fix/Create Undead Dummies From Polygon Zombies")]
        public static void CreateUndeadDummiesFromPolygonZombies()
        {
            var templatePrefab = AssetDatabase.LoadAssetAtPath<GameObject>(TemplatePrefabPath);
            if (templatePrefab == null)
            {
                
                return;
            }

            var sourcePrefabPaths = AssetDatabase.FindAssets("t:Prefab", new[] { SourcePrefabsFolder })
                .Select(AssetDatabase.GUIDToAssetPath)
                .Where(path => path.EndsWith(".prefab") && System.IO.Path.GetFileName(path).StartsWith("Zombie_"))
                .OrderBy(path => path)
                .ToList();

            if (sourcePrefabPaths.Count == 0)
            {
                
                return;
            }

            var generatedCount = 0;
            var targetIndex = 2;

            foreach (var sourcePrefabPath in sourcePrefabPaths)
            {
                var targetPrefabName = $"Undead_Dummy_{targetIndex:00}";
                var targetPrefabPath = $"{TargetPrefabsFolder}/{targetPrefabName}.prefab";

                EnsureTargetPrefabExists(targetPrefabPath);
                UpdateTargetPrefab(targetPrefabPath, targetPrefabName, sourcePrefabPath);

                generatedCount++;
                targetIndex++;
            }

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            
        }

        private static void TryAutoCreateOnce()
        {
            EditorApplication.delayCall -= TryAutoCreateOnce;

            if (SessionState.GetBool(AutoRunSessionKey, false))
            {
                return;
            }

            SessionState.SetBool(AutoRunSessionKey, true);
            CreateUndeadDummiesFromPolygonZombies();
        }

        private static void EnsureTargetPrefabExists(string targetPrefabPath)
        {
            if (AssetDatabase.LoadAssetAtPath<GameObject>(targetPrefabPath) != null)
            {
                AssetDatabase.DeleteAsset(targetPrefabPath);
            }

            if (!AssetDatabase.CopyAsset(TemplatePrefabPath, targetPrefabPath))
            {
                
            }
        }

        private static void UpdateTargetPrefab(string targetPrefabPath, string targetPrefabName, string sourcePrefabPath)
        {
            var sourcePrefab = AssetDatabase.LoadAssetAtPath<GameObject>(sourcePrefabPath);
            if (sourcePrefab == null)
            {
                
                return;
            }

            var prefabRoot = PrefabUtility.LoadPrefabContents(targetPrefabPath);

            try
            {
                prefabRoot.name = targetPrefabName;

                RemoveExistingVisualRoots(prefabRoot.transform);

                var sourceInstance = PrefabUtility.InstantiatePrefab(sourcePrefab, prefabRoot.scene) as GameObject;
                if (sourceInstance == null)
                {
                    
                    return;
                }

                sourceInstance.name = sourcePrefab.name;
                sourceInstance.transform.SetParent(prefabRoot.transform, false);
                sourceInstance.transform.localPosition = Vector3.zero;
                sourceInstance.transform.localRotation = Quaternion.identity;
                sourceInstance.transform.localScale = Vector3.one;

                SyncAnimatorAvatar(prefabRoot, sourceInstance);
                SetLayerRecursively(sourceInstance.transform, DamageableCharacterLayer);
                RepairGameplayHooks(prefabRoot, sourceInstance.transform);
                EnsureCharacterUiReferences(prefabRoot);

                PrefabUtility.SaveAsPrefabAsset(prefabRoot, targetPrefabPath);
            }
            finally
            {
                PrefabUtility.UnloadPrefabContents(prefabRoot);
            }
        }

        private static void RemoveExistingVisualRoots(Transform root)
        {
            var childrenToRemove = new List<GameObject>();

            foreach (Transform child in root)
            {
                if (ShouldPreserveTopLevelChild(child))
                {
                    continue;
                }

                childrenToRemove.Add(child.gameObject);
            }

            foreach (var child in childrenToRemove)
            {
                Object.DestroyImmediate(child, true);
            }
        }

        private static bool ShouldPreserveTopLevelChild(Transform child)
        {
            return child.name == "Navmesh Agent"
                || child.GetComponent<NavMeshAgent>() != null
                || child.GetComponentInChildren<baodeag.UI_Character_HP_Bar>(true) != null;
        }

        private static void SyncAnimatorAvatar(GameObject prefabRoot, GameObject visualRoot)
        {
            var rootAnimator = prefabRoot.GetComponent<Animator>();
            var visualAnimator = visualRoot.GetComponent<Animator>();

            if (rootAnimator != null && visualAnimator != null)
            {
                rootAnimator.avatar = visualAnimator.avatar;
            }

            if (visualAnimator != null)
            {
                Object.DestroyImmediate(visualAnimator, true);
            }
        }

        private static void RepairGameplayHooks(GameObject prefabRoot, Transform visualRoot)
        {
            EnsureLockOnTarget(visualRoot);
            EnsureMainHurtbox(visualRoot);
            EnsureBodyColliders(visualRoot);
            EnsureHandDamageColliders(prefabRoot, visualRoot);
        }

        private static void EnsureCharacterUiReferences(GameObject prefabRoot)
        {
            var uiManager = prefabRoot.GetComponent<baodeag.CharacterUIManager>();
            if (uiManager == null)
            {
                return;
            }

            var hpBar = prefabRoot.GetComponentInChildren<baodeag.UI_Character_HP_Bar>(true);
            if (hpBar == null)
            {
                
                return;
            }

            var serializedObject = new SerializedObject(uiManager);
            serializedObject.FindProperty("characterHPBar").objectReferenceValue = hpBar;
            serializedObject.ApplyModifiedPropertiesWithoutUndo();
            EditorUtility.SetDirty(uiManager);
        }

        private static void EnsureLockOnTarget(Transform visualRoot)
        {
            var existingLockOn = visualRoot.GetComponentInChildren<LockOnTransform>(true);
            if (existingLockOn != null)
            {
                existingLockOn.gameObject.layer = DamageableCharacterLayer;
                return;
            }

            var parent = FindDeepChild(visualRoot, "Head") ?? FindDeepChild(visualRoot, "Neck") ?? visualRoot;
            var lockOnObject = new GameObject("Lock on Target");
            lockOnObject.layer = DamageableCharacterLayer;
            lockOnObject.transform.SetParent(parent, false);
            lockOnObject.transform.localPosition = Vector3.zero;
            lockOnObject.transform.localRotation = Quaternion.identity;
            lockOnObject.transform.localScale = Vector3.one;
            lockOnObject.AddComponent<LockOnTransform>();
        }

        private static void EnsureMainHurtbox(Transform visualRoot)
        {
            var hips = FindDeepChild(visualRoot, "Hips") ?? visualRoot;
            var mainHurtbox = FindDirectChild(hips, "Undead_Main_Hurtbox");

            if (mainHurtbox == null)
            {
                var hurtboxObject = new GameObject("Undead_Main_Hurtbox");
                hurtboxObject.layer = DamageableCharacterLayer;
                hurtboxObject.transform.SetParent(hips, false);
                hurtboxObject.transform.localPosition = Vector3.zero;
                hurtboxObject.transform.localRotation = Quaternion.identity;
                hurtboxObject.transform.localScale = Vector3.one;
                mainHurtbox = hurtboxObject.transform;
            }
            else
            {
                mainHurtbox.gameObject.layer = DamageableCharacterLayer;
            }

            var collider = mainHurtbox.GetComponent<CapsuleCollider>();
            if (collider == null)
            {
                collider = mainHurtbox.gameObject.AddComponent<CapsuleCollider>();
            }

            collider.isTrigger = false;
            collider.radius = 0.32f;
            collider.height = 1.25f;
            collider.direction = 1;
            collider.center = new Vector3(0f, 0.45f, 0f);
        }

        private static void EnsureBodyColliders(Transform visualRoot)
        {
            SetDamageableLayerRecursively(visualRoot);

            var colliderDefinitions = new[]
            {
                new CapsuleDefinition("Hips", 0.18f, 0.45f, 1, Vector3.zero),
                new CapsuleDefinition("Spine_01", 0.12f, 0.33f, 2, new Vector3(0f, -0.02f, 0f)),
                new CapsuleDefinition("Spine_02", 0.12f, 0.35f, 2, Vector3.zero),
                new CapsuleDefinition("Spine_03", 0.12f, 0.32f, 2, Vector3.zero),
                new CapsuleDefinition("Shoulder_L", 0.09f, 0.38f, 0, new Vector3(-0.15f, 0f, 0f)),
                new CapsuleDefinition("Shoulder_R", 0.09f, 0.38f, 0, new Vector3(0.15f, 0f, 0f)),
                new CapsuleDefinition("Elbow_L", 0.07f, 0.35f, 0, new Vector3(-0.13f, 0f, 0f)),
                new CapsuleDefinition("Elbow_R", 0.07f, 0.35f, 0, new Vector3(0.13f, 0f, 0f)),
                new CapsuleDefinition("Hand_L", 0.07f, 0.26f, 0, new Vector3(-0.12f, 0f, 0f)),
                new CapsuleDefinition("Hand_R", 0.07f, 0.26f, 0, new Vector3(0.12f, 0f, 0f)),
                new CapsuleDefinition("UpperLeg_L", 0.07f, 0.5f, 0, new Vector3(0.12f, 0f, 0f)),
                new CapsuleDefinition("UpperLeg_R", 0.07f, 0.5f, 0, new Vector3(-0.12f, 0f, 0f)),
                new CapsuleDefinition("LowerLeg_L", 0.07f, 0.5f, 0, new Vector3(0.18f, 0f, 0f)),
                new CapsuleDefinition("LowerLeg_R", 0.07f, 0.5f, 0, new Vector3(-0.18f, 0f, 0f)),
            };

            foreach (var definition in colliderDefinitions)
            {
                var bone = FindDeepChild(visualRoot, definition.BoneName);
                if (bone == null)
                {
                    continue;
                }

                bone.gameObject.layer = DamageableCharacterLayer;

                var collider = bone.GetComponent<CapsuleCollider>();
                if (collider == null)
                {
                    collider = bone.gameObject.AddComponent<CapsuleCollider>();
                }

                collider.isTrigger = false;
                collider.radius = definition.Radius;
                collider.height = definition.Height;
                collider.direction = definition.Direction;
                collider.center = definition.Center;
            }
        }

        private static void EnsureHandDamageColliders(GameObject prefabRoot, Transform visualRoot)
        {
            var combatManager = prefabRoot.GetComponent<AIUndeadCombatManager>();
            if (combatManager == null)
            {
                return;
            }

            var rightBone = FindDeepChild(visualRoot, "Hand_R");
            var leftBone = FindDeepChild(visualRoot, "Hand_L");

            if (rightBone == null || leftBone == null)
            {
                
                return;
            }

            var rightDamageCollider = EnsureManualDamageCollider(rightBone, "Undead_RightHand_Hitbox");
            var leftDamageCollider = EnsureManualDamageCollider(leftBone, "Undead_LeftHand_Hitbox");

            var serializedObject = new SerializedObject(combatManager);
            serializedObject.FindProperty("rightHandDamageCollider").objectReferenceValue = rightDamageCollider;
            serializedObject.FindProperty("leftHandDamageCollider").objectReferenceValue = leftDamageCollider;
            serializedObject.ApplyModifiedPropertiesWithoutUndo();
            EditorUtility.SetDirty(combatManager);
        }

        private static ManualDamageCollider EnsureManualDamageCollider(Transform handBone, string hitboxName)
        {
            var hitbox = FindDirectChild(handBone, hitboxName);
            if (hitbox == null)
            {
                var hitboxObject = new GameObject(hitboxName);
                hitboxObject.layer = DamageColliderLayer;
                hitboxObject.transform.SetParent(handBone, false);
                hitboxObject.transform.localPosition = Vector3.zero;
                hitboxObject.transform.localRotation = Quaternion.identity;
                hitboxObject.transform.localScale = Vector3.one;
                hitbox = hitboxObject.transform;
            }
            else
            {
                hitbox.gameObject.layer = DamageColliderLayer;
            }

            var sphereCollider = hitbox.GetComponent<SphereCollider>();
            if (sphereCollider == null)
            {
                sphereCollider = hitbox.gameObject.AddComponent<SphereCollider>();
            }

            sphereCollider.isTrigger = true;
            sphereCollider.radius = 0.16f;
            sphereCollider.center = Vector3.zero;

            var damageCollider = hitbox.GetComponent<ManualDamageCollider>();
            if (damageCollider == null)
            {
                damageCollider = hitbox.gameObject.AddComponent<ManualDamageCollider>();
            }

            return damageCollider;
        }

        private static void SetDamageableLayerRecursively(Transform root)
        {
            foreach (Transform child in root.GetComponentsInChildren<Transform>(true))
            {
                child.gameObject.layer = child.GetComponent<ManualDamageCollider>() != null ? DamageColliderLayer : DamageableCharacterLayer;
            }
        }

        private static void SetLayerRecursively(Transform root, int layer)
        {
            root.gameObject.layer = layer;

            foreach (Transform child in root)
            {
                SetLayerRecursively(child, layer);
            }
        }

        private static Transform FindDeepChild(Transform parent, string name)
        {
            foreach (Transform child in parent.GetComponentsInChildren<Transform>(true))
            {
                if (child.name == name)
                {
                    return child;
                }
            }

            return null;
        }

        private static Transform FindDirectChild(Transform parent, string name)
        {
            foreach (Transform child in parent)
            {
                if (child.name == name)
                {
                    return child;
                }
            }

            return null;
        }

        private readonly struct CapsuleDefinition
        {
            public CapsuleDefinition(string boneName, float radius, float height, int direction, Vector3 center)
            {
                BoneName = boneName;
                Radius = radius;
                Height = height;
                Direction = direction;
                Center = center;
            }

            public string BoneName { get; }
            public float Radius { get; }
            public float Height { get; }
            public int Direction { get; }
            public Vector3 Center { get; }
        }
    }
}
