using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace baodeag.EditorTools
{
    public static class UndeadDummyGameplayFixer
    {
        private const string TargetPrefabPath = GameAssetPaths.PrefabsRoot + "/Character/Undead/Undead_Dummy_01.prefab";
        private const string VisualRootName = "SM_Chr_ZombieBoss_Wretch_01";
        private const string AutoRunSessionKey = "UndeadDummyGameplayFixer.AutoRunAttempted";
        private const int DamageableCharacterLayer = 7;
        private const int DamageColliderLayer = 10;

        static UndeadDummyGameplayFixer()
        {
            // Disabled autorun to avoid mutating editor state on startup.
        }

        [MenuItem("Tools/Fix/Repair Undead Dummy Gameplay Hooks")]
        public static void FixUndeadDummyGameplayHooks()
        {
            var prefabRoot = PrefabUtility.LoadPrefabContents(TargetPrefabPath);

            try
            {
                var visualRoot = FindDeepChild(prefabRoot.transform, VisualRootName);
                if (visualRoot == null)
                {
                    Debug.LogError("Could not find the Wretch visual root inside Undead_Dummy_01.");
                    return;
                }

                EnsureLockOnTarget(visualRoot);
                EnsureMainHurtbox(visualRoot);
                EnsureBodyColliders(visualRoot);
                EnsureHandDamageColliders(prefabRoot, visualRoot);

                PrefabUtility.SaveAsPrefabAsset(prefabRoot, TargetPrefabPath);
                Debug.Log("Repaired Undead_Dummy_01 lock-on, body colliders, and hand damage colliders.");
            }
            finally
            {
                PrefabUtility.UnloadPrefabContents(prefabRoot);
            }
        }

        private static void TryAutoFixOnce()
        {
            if (SessionState.GetBool(AutoRunSessionKey, false))
            {
                return;
            }

            SessionState.SetBool(AutoRunSessionKey, true);
            FixUndeadDummyGameplayHooks();
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
                Debug.LogWarning("Could not find Hand_L/Hand_R on Wretch rig, skipped damage collider repair.");
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

        private static void EnsureBodyColliders(Transform visualRoot)
        {
            SetDamageableLayerRecursively(visualRoot);

            var colliderDefinitions = new[]
            {
                new CapsuleDefinition("Hips", 0.18f, 0.45f, 1, new Vector3(0f, 0f, 0f)),
                new CapsuleDefinition("Spine_01", 0.12f, 0.33f, 2, new Vector3(0f, -0.02f, 0f)),
                new CapsuleDefinition("Spine_02", 0.12f, 0.35f, 2, Vector3.zero),
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

        private static void SetDamageableLayerRecursively(Transform root)
        {
            foreach (Transform child in root.GetComponentsInChildren<Transform>(true))
            {
                if (child.GetComponent<ManualDamageCollider>() != null)
                {
                    child.gameObject.layer = DamageColliderLayer;
                    continue;
                }

                child.gameObject.layer = DamageableCharacterLayer;
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
