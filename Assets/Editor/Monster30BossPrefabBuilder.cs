using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.AI;

namespace baodeag.EditorTools
{
    [InitializeOnLoad]
    public static class Monster30BossPrefabBuilder
    {
        private const string SourceTemplatePath = "Assets/Prefabs/Character/Undead/Undead_Dummy_30.prefab";
        private const string BossReferencePath = "Assets/Prefabs/Character/Durk_Dummy_01.prefab";
        private const string MonsterVisualPath = "Assets/Stylized3DMonster/Monster30_FreeTrial/Prefab/Monster30_01.prefab";
        private const string MonsterModelPath = "Assets/Stylized3DMonster/Monster30_FreeTrial/Monster30_FreeTrial.fbx";
        private const string BaseAnimatorControllerPath = "Assets/Data/Animator Controllers/Undead.controller";
        private const string MonsterIdleAnimationPath = "Assets/Stylized3DMonster/Monster30_FreeTrial/Anim/Monster30_Idle.anim";
        private const string MonsterWalkAnimationPath = "Assets/Stylized3DMonster/Monster30_FreeTrial/Anim/Monster30_Walk_InPlace.anim";
        private const string MonsterRebasedIdleAnimationPath = "Assets/Stylized3DMonster/Monster30_FreeTrial/Anim/Monster30_Boss_Idle_Rebased.anim";
        private const string MonsterRebasedWalkAnimationPath = "Assets/Stylized3DMonster/Monster30_FreeTrial/Anim/Monster30_Boss_Walk_Rebased.anim";
        private const string TargetOverrideControllerPath = "Assets/Data/Animator Controllers/Monster30_Boss.overrideController";
        private const string TargetPrefabPath = "Assets/Prefabs/Character/Monster30_Boss_01.prefab";
        private const string AutoRunSessionKey = "Monster30BossPrefabBuilder.AutoRunAttempted.v4";
        private const int CharacterLayer = 6;
        private const int DamageableCharacterLayer = 7;
        private const int DamageColliderLayer = 10;
        private const string MonsterAnimationRootPrefix = "Monster30_VisualRoot/Monster30_FreeTrial/";

        static Monster30BossPrefabBuilder()
        {
            EditorApplication.delayCall += TryAutoBuildOnce;
        }

        [MenuItem("Tools/Bosses/Create Monster30 Boss")]
        public static void CreateMonster30Boss()
        {
            var sourceTemplate = AssetDatabase.LoadAssetAtPath<GameObject>(SourceTemplatePath);
            var bossReference = AssetDatabase.LoadAssetAtPath<GameObject>(BossReferencePath);
            var monsterVisualPrefab = AssetDatabase.LoadAssetAtPath<GameObject>(MonsterVisualPath);

            if (sourceTemplate == null || bossReference == null || monsterVisualPrefab == null)
            {
                Debug.LogError("Monster30 boss builder is missing one of the required prefab assets.");
                return;
            }

            var bossReferenceManager = bossReference.GetComponent<AIBossCharacterManager>();
            if (bossReferenceManager == null)
            {
                Debug.LogError("Durk_Dummy_01 is missing AIBossCharacterManager, cannot copy boss settings.");
                return;
            }

            var prefabRoot = PrefabUtility.LoadPrefabContents(SourceTemplatePath);

            try
            {
                prefabRoot.name = "Monster30_Boss_01";
                prefabRoot.layer = CharacterLayer;

                ReplaceCharacterManager(prefabRoot, bossReferenceManager);
                PrepareBossStats(prefabRoot);
                RemoveOldVisualChildren(prefabRoot.transform);

                var visualInstance = PrefabUtility.InstantiatePrefab(monsterVisualPrefab, prefabRoot.scene) as GameObject;
                if (visualInstance == null)
                {
                    Debug.LogError("Failed to instantiate Monster30 visual prefab.");
                    return;
                }

                visualInstance.name = "Monster30_VisualRoot";
                visualInstance.transform.SetParent(prefabRoot.transform, false);
                visualInstance.transform.localPosition = Vector3.zero;
                visualInstance.transform.localRotation = Quaternion.identity;
                visualInstance.transform.localScale = Vector3.one;

                var monsterAvatar = LoadMonsterAvatar();
                var sourceAnimator = visualInstance.GetComponent<Animator>();
                var rootAnimator = prefabRoot.GetComponent<Animator>();

                if (sourceAnimator == null)
                {
                    sourceAnimator = visualInstance.AddComponent<Animator>();
                }

                if (monsterAvatar != null)
                {
                    sourceAnimator.avatar = monsterAvatar;
                }

                if (rootAnimator == null || sourceAnimator == null)
                {
                    Debug.LogError($"Monster30 boss builder could not find required animator components. rootAnimatorNull={rootAnimator == null}, sourceAnimatorNull={sourceAnimator == null}, monsterAvatarNull={monsterAvatar == null}");
                    return;
                }

                if (sourceAnimator.avatar != null)
                {
                    rootAnimator.avatar = sourceAnimator.avatar;
                }

                AssignMonster30Animator(rootAnimator);

                if (rootAnimator.avatar == null)
                {
                    Debug.LogError("Monster30 boss builder could not resolve a humanoid avatar for the boss.");
                    return;
                }

                ConfigureVisualHierarchy(visualInstance.transform);
                RebuildGameplayHooks(prefabRoot, visualInstance.transform, sourceAnimator);

                Object.DestroyImmediate(sourceAnimator, true);

                PrefabUtility.SaveAsPrefabAsset(prefabRoot, TargetPrefabPath);
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();

                Debug.Log("Created Monster30_Boss_01 prefab with boss gameplay hooks.");
            }
            finally
            {
                PrefabUtility.UnloadPrefabContents(prefabRoot);
            }
        }

        private static void TryAutoBuildOnce()
        {
            if (SessionState.GetBool(AutoRunSessionKey, false))
            {
                return;
            }

            SessionState.SetBool(AutoRunSessionKey, true);
            CreateMonster30Boss();
        }

        private static void ReplaceCharacterManager(GameObject prefabRoot, AIBossCharacterManager bossReferenceManager)
        {
            ReplaceNetworkManager(prefabRoot);

            var oldManager = prefabRoot.GetComponent<AICharacterManager>();
            if (oldManager != null)
            {
                Object.DestroyImmediate(oldManager, true);
            }

            var bossManager = prefabRoot.GetComponent<AIBossCharacterManager>();
            if (bossManager == null)
            {
                bossManager = prefabRoot.AddComponent<AIBossCharacterManager>();
            }

            var serializedObject = new SerializedObject(bossManager);
            serializedObject.FindProperty("characterGroup").enumValueIndex = (int)CharacterGroup.Team02;
            serializedObject.FindProperty("characterName").stringValue = "Monster30, The Ravager";
            serializedObject.FindProperty("idle").objectReferenceValue = LoadStateAsset<IdleState>("t:IdleState", "18e987916d2ef1a44b5a64ea20b40e25");
            serializedObject.FindProperty("pursueTarget").objectReferenceValue = LoadStateAsset<PursueTargetState>("t:PursueTargetState", "858bd4aaa1b8cc54cbe09f1fe39629f3");
            serializedObject.FindProperty("combatStance").objectReferenceValue = LoadStateAsset<CombatStanceState>("t:CombatStanceState", "012f1faa7309fc14f83a2c716f22ced3");
            serializedObject.FindProperty("attack").objectReferenceValue = LoadStateAsset<AttackState>("t:AttackState", "38ea9cf74089e584aaa51ccadfea4192");
            serializedObject.FindProperty("investigateSound").objectReferenceValue = LoadStateAsset<InvestigateSoundState>("t:InvestigateSoundState", "b01869362abef4045bd92e907638d143");
            serializedObject.FindProperty("bossIntroClip").objectReferenceValue = GetSerializedReference(bossReferenceManager, "bossIntroClip");
            serializedObject.FindProperty("bossBattleLoopClip").objectReferenceValue = GetSerializedReference(bossReferenceManager, "bossBattleLoopClip");
            serializedObject.FindProperty("autoWakeOnSpawn").boolValue = false;
            serializedObject.FindProperty("useNavMeshTranslationForInPlaceAnimations").boolValue = true;
            serializedObject.FindProperty("sleepAnimation").stringValue = "Sleep_01";
            serializedObject.FindProperty("awakenAnimation").stringValue = "Wake_01";
            serializedObject.FindProperty("minimumHealthPercentageToShift").floatValue = 0f;
            serializedObject.FindProperty("phaseShiftAnimation").stringValue = "Attack_02";
            serializedObject.FindProperty("phase02CombatStanceState").objectReferenceValue = serializedObject.FindProperty("combatStance").objectReferenceValue;
            serializedObject.FindProperty("sleepState").objectReferenceValue = GetSerializedReference(bossReferenceManager, "sleepState");
            serializedObject.ApplyModifiedPropertiesWithoutUndo();
            EditorUtility.SetDirty(bossManager);
        }

        private static void ReplaceNetworkManager(GameObject prefabRoot)
        {
            var oldNetworkManager = prefabRoot.GetComponent<AICharacterNetworkManager>();
            if (oldNetworkManager != null && oldNetworkManager.GetType() != typeof(AIBossCharacterNetworkManager))
            {
                Object.DestroyImmediate(oldNetworkManager, true);
            }

            if (prefabRoot.GetComponent<AIBossCharacterNetworkManager>() == null)
            {
                prefabRoot.AddComponent<AIBossCharacterNetworkManager>();
            }
        }

        private static void PrepareBossStats(GameObject prefabRoot)
        {
            var networkManager = prefabRoot.GetComponent<AICharacterNetworkManager>();
            if (networkManager != null)
            {
                var serializedObject = new SerializedObject(networkManager);
                SetIntPropertyIfPresent(serializedObject, "currentHealth.m_InternalValue", 1600);
                SetIntPropertyIfPresent(serializedObject, "maxHealth.m_InternalValue", 1600);
                SetIntPropertyIfPresent(serializedObject, "currentStamina.m_InternalValue", 240);
                SetIntPropertyIfPresent(serializedObject, "maxStamina.m_InternalValue", 240);
                SetBoolPropertyIfPresent(serializedObject, "isAwake.m_InternalValue", false);
                serializedObject.ApplyModifiedPropertiesWithoutUndo();
                EditorUtility.SetDirty(networkManager);
            }

            var animator = prefabRoot.GetComponent<Animator>();
            if (animator != null)
            {
                animator.applyRootMotion = false;
                EditorUtility.SetDirty(animator);
            }

            var combatManager = prefabRoot.GetComponent<AIUndeadCombatManager>();
            if (combatManager != null)
            {
                var serializedObject = new SerializedObject(combatManager);
                serializedObject.FindProperty("baseDamage").intValue = 48;
                serializedObject.FindProperty("basePoiseDamage").intValue = 36;
                serializedObject.FindProperty("maxStance").floatValue = 180f;
                serializedObject.FindProperty("attack01DamageModifier").floatValue = 1.2f;
                serializedObject.FindProperty("attack02DamageModifier").floatValue = 1.65f;
                serializedObject.ApplyModifiedPropertiesWithoutUndo();
                EditorUtility.SetDirty(combatManager);
            }

            var statsManager = prefabRoot.GetComponent<CharacterStatsManager>();
            if (statsManager != null)
            {
                var serializedObject = new SerializedObject(statsManager);
                serializedObject.FindProperty("runesDroppedOnDeath").intValue = 800;
                serializedObject.FindProperty("basePoiseDefense").floatValue = 80f;
                serializedObject.ApplyModifiedPropertiesWithoutUndo();
                EditorUtility.SetDirty(statsManager);
            }
        }

        private static void RemoveOldVisualChildren(Transform root)
        {
            var childrenToDelete = new List<GameObject>();

            foreach (Transform child in root)
            {
                if (child.GetComponent<NavMeshAgent>() != null)
                {
                    continue;
                }

                if (child.GetComponentInChildren<Canvas>(true) != null)
                {
                    continue;
                }

                childrenToDelete.Add(child.gameObject);
            }

            foreach (var child in childrenToDelete)
            {
                Object.DestroyImmediate(child, true);
            }
        }

        private static void ConfigureVisualHierarchy(Transform visualRoot)
        {
            foreach (var child in visualRoot.GetComponentsInChildren<Transform>(true))
            {
                child.gameObject.layer = DamageableCharacterLayer;
            }
        }

        private static void RebuildGameplayHooks(GameObject prefabRoot, Transform visualRoot, Animator sourceAnimator)
        {
            EnsureLockOnTarget(sourceAnimator);
            EnsureMainHurtbox(sourceAnimator);
            EnsureBodyColliders(sourceAnimator);
            EnsureHandDamageColliders(prefabRoot, sourceAnimator);
        }

        private static void AssignMonster30Animator(Animator rootAnimator)
        {
            var overrideController = CreateOrUpdateMonster30OverrideController();
            if (overrideController == null)
            {
                return;
            }

            rootAnimator.runtimeAnimatorController = overrideController;
            EditorUtility.SetDirty(rootAnimator);
        }

        private static AnimatorOverrideController CreateOrUpdateMonster30OverrideController()
        {
            var baseController = AssetDatabase.LoadAssetAtPath<RuntimeAnimatorController>(BaseAnimatorControllerPath);
            var monsterIdleClip = CreateOrUpdateRebasedAnimationClip(MonsterIdleAnimationPath, MonsterRebasedIdleAnimationPath);
            var monsterWalkClip = CreateOrUpdateRebasedAnimationClip(MonsterWalkAnimationPath, MonsterRebasedWalkAnimationPath);

            if (baseController == null || monsterIdleClip == null || monsterWalkClip == null)
            {
                Debug.LogError("Monster30 boss builder could not load the base controller or Monster30 locomotion clips.");
                return null;
            }

            var overrideController = AssetDatabase.LoadAssetAtPath<AnimatorOverrideController>(TargetOverrideControllerPath);
            if (overrideController == null)
            {
                overrideController = new AnimatorOverrideController(baseController)
                {
                    name = "Monster30_Boss"
                };

                AssetDatabase.CreateAsset(overrideController, TargetOverrideControllerPath);
            }
            else
            {
                overrideController.runtimeAnimatorController = baseController;
            }

            var overrides = new List<KeyValuePair<AnimationClip, AnimationClip>>();
            overrideController.GetOverrides(overrides);

            bool replacedIdle = false;
            bool replacedWalk = false;

            for (int i = 0; i < overrides.Count; i++)
            {
                var originalClip = overrides[i].Key;
                if (originalClip == null)
                {
                    continue;
                }

                if (originalClip.name == "Zombie_Idle_01")
                {
                    overrides[i] = new KeyValuePair<AnimationClip, AnimationClip>(originalClip, monsterIdleClip);
                    replacedIdle = true;
                    continue;
                }

                if (originalClip.name == "Zombie_Walk_01_Forward"
                    || originalClip.name == "Zombie_Walk_01_Forward_InPlace"
                    || originalClip.name == "Zombie_Walk_03_Forward"
                    || originalClip.name == "Zombie_Run_01_Forward"
                    || originalClip.name == "Zombie_Sprint_01_Forward")
                {
                    overrides[i] = new KeyValuePair<AnimationClip, AnimationClip>(originalClip, monsterWalkClip);
                    replacedWalk = true;
                }
            }

            overrideController.ApplyOverrides(overrides);
            EditorUtility.SetDirty(overrideController);

            if (!replacedIdle || !replacedWalk)
            {
                Debug.LogWarning($"Monster30 boss override controller did not replace every expected locomotion clip. replacedIdle={replacedIdle}, replacedWalk={replacedWalk}");
            }

            return overrideController;
        }

        private static AnimationClip CreateOrUpdateRebasedAnimationClip(string sourceClipPath, string rebasedClipPath)
        {
            var sourceClip = AssetDatabase.LoadAssetAtPath<AnimationClip>(sourceClipPath);
            if (sourceClip == null)
            {
                Debug.LogError($"Monster30 boss builder could not load source clip at {sourceClipPath}.");
                return null;
            }

            var rebasedClip = AssetDatabase.LoadAssetAtPath<AnimationClip>(rebasedClipPath);
            if (rebasedClip == null)
            {
                rebasedClip = Object.Instantiate(sourceClip);
                rebasedClip.name = System.IO.Path.GetFileNameWithoutExtension(rebasedClipPath);
                AssetDatabase.CreateAsset(rebasedClip, rebasedClipPath);
            }
            else
            {
                EditorUtility.CopySerialized(sourceClip, rebasedClip);
            }

            RebaseAnimationClipPaths(rebasedClip, MonsterAnimationRootPrefix);
            ConfigureLoopingForLocomotionClip(rebasedClip, rebasedClipPath);
            EditorUtility.SetDirty(rebasedClip);

            return rebasedClip;
        }

        private static void ConfigureLoopingForLocomotionClip(AnimationClip clip, string clipPath)
        {
            if (clip == null)
            {
                return;
            }

            bool shouldLoop = clipPath == MonsterRebasedIdleAnimationPath
                || clipPath == MonsterRebasedWalkAnimationPath;

            if (!shouldLoop)
            {
                return;
            }

            var serializedClip = new SerializedObject(clip);
            var clipSettings = serializedClip.FindProperty("m_AnimationClipSettings");
            if (clipSettings == null)
            {
                Debug.LogWarning($"Monster30 boss builder could not locate animation clip settings for {clip.name}.");
                return;
            }

            clipSettings.FindPropertyRelative("m_LoopTime").boolValue = true;
            clipSettings.FindPropertyRelative("m_LoopBlend").boolValue = true;
            clipSettings.FindPropertyRelative("m_LoopBlendOrientation").boolValue = true;
            clipSettings.FindPropertyRelative("m_LoopBlendPositionY").boolValue = false;
            clipSettings.FindPropertyRelative("m_LoopBlendPositionXZ").boolValue = false;
            serializedClip.ApplyModifiedPropertiesWithoutUndo();
        }

        private static void RebaseAnimationClipPaths(AnimationClip clip, string prefix)
        {
            foreach (var binding in AnimationUtility.GetCurveBindings(clip))
            {
                var curve = AnimationUtility.GetEditorCurve(clip, binding);
                AnimationUtility.SetEditorCurve(clip, binding, null);

                var rebasedBinding = binding;
                rebasedBinding.path = string.IsNullOrEmpty(binding.path) ? prefix.TrimEnd('/') : $"{prefix}{binding.path}";
                AnimationUtility.SetEditorCurve(clip, rebasedBinding, curve);
            }

            foreach (var binding in AnimationUtility.GetObjectReferenceCurveBindings(clip))
            {
                var keyframes = AnimationUtility.GetObjectReferenceCurve(clip, binding);
                AnimationUtility.SetObjectReferenceCurve(clip, binding, null);

                var rebasedBinding = binding;
                rebasedBinding.path = string.IsNullOrEmpty(binding.path) ? prefix.TrimEnd('/') : $"{prefix}{binding.path}";
                AnimationUtility.SetObjectReferenceCurve(clip, rebasedBinding, keyframes);
            }
        }

        private static void EnsureLockOnTarget(Animator animator)
        {
            var head = FindBone(animator, HumanBodyBones.Head, "Head", "head")
                ?? FindBone(animator, HumanBodyBones.Neck, "Neck", "neck")
                ?? animator.transform;

            var lockOnObject = FindOrCreateDirectChild(head, "Lock on Target", DamageableCharacterLayer);

            if (lockOnObject.GetComponent<LockOnTransform>() == null)
            {
                lockOnObject.gameObject.AddComponent<LockOnTransform>();
            }

            lockOnObject.localPosition = Vector3.zero;
            lockOnObject.localRotation = Quaternion.identity;
            lockOnObject.localScale = Vector3.one;
        }

        private static void EnsureMainHurtbox(Animator animator)
        {
            var hips = FindBone(animator, HumanBodyBones.Hips, "Hips", "hips", "Pelvis", "pelvis") ?? animator.transform;
            var hurtbox = FindOrCreateDirectChild(hips, "Monster30_Main_Hurtbox", DamageableCharacterLayer);
            var collider = hurtbox.GetComponent<CapsuleCollider>();
            if (collider == null)
            {
                collider = hurtbox.gameObject.AddComponent<CapsuleCollider>();
            }

            collider.isTrigger = false;
            collider.radius = 0.4f;
            collider.height = 1.6f;
            collider.direction = 1;
            collider.center = new Vector3(0f, 0.55f, 0f);
        }

        private static void EnsureHandDamageColliders(GameObject prefabRoot, Animator animator)
        {
            var combatManager = prefabRoot.GetComponent<AIUndeadCombatManager>();
            if (combatManager == null)
            {
                return;
            }

            var rightHand = FindBone(animator, HumanBodyBones.RightHand, "Hand_R", "RightHand", "hand_r", "hand.r");
            var leftHand = FindBone(animator, HumanBodyBones.LeftHand, "Hand_L", "LeftHand", "hand_l", "hand.l");

            if (rightHand == null || leftHand == null)
            {
                Debug.LogWarning("Monster30 rig does not expose LeftHand/RightHand humanoid bones, damage colliders were skipped.");
                return;
            }

            var rightCollider = EnsureManualDamageCollider(rightHand, "Monster30_RightHand_Hitbox");
            var leftCollider = EnsureManualDamageCollider(leftHand, "Monster30_LeftHand_Hitbox");

            var serializedObject = new SerializedObject(combatManager);
            serializedObject.FindProperty("rightHandDamageCollider").objectReferenceValue = rightCollider;
            serializedObject.FindProperty("leftHandDamageCollider").objectReferenceValue = leftCollider;
            serializedObject.ApplyModifiedPropertiesWithoutUndo();
            EditorUtility.SetDirty(combatManager);
        }

        private static void EnsureBodyColliders(Animator animator)
        {
            var definitions = new[]
            {
                new BoneCapsuleDefinition(HumanBodyBones.Hips, 0.2f, 0.5f, 1, new Vector3(0f, 0f, 0f), "Hips", "hips", "Pelvis"),
                new BoneCapsuleDefinition(HumanBodyBones.Spine, 0.16f, 0.42f, 1, new Vector3(0f, 0.02f, 0f), "Spine", "spine", "Spine_01"),
                new BoneCapsuleDefinition(HumanBodyBones.Chest, 0.2f, 0.55f, 1, new Vector3(0f, 0.02f, 0f), "Chest", "chest", "Spine_02"),
                new BoneCapsuleDefinition(HumanBodyBones.UpperChest, 0.18f, 0.48f, 1, new Vector3(0f, 0.02f, 0f), "UpperChest", "upperChest"),
                new BoneCapsuleDefinition(HumanBodyBones.LeftUpperArm, 0.09f, 0.45f, 0, new Vector3(-0.14f, 0f, 0f), "UpperArm_L", "LeftArm", "Arm_L"),
                new BoneCapsuleDefinition(HumanBodyBones.RightUpperArm, 0.09f, 0.45f, 0, new Vector3(0.14f, 0f, 0f), "UpperArm_R", "RightArm", "Arm_R"),
                new BoneCapsuleDefinition(HumanBodyBones.LeftLowerArm, 0.08f, 0.42f, 0, new Vector3(-0.14f, 0f, 0f), "LowerArm_L", "ForeArm_L", "LeftForeArm"),
                new BoneCapsuleDefinition(HumanBodyBones.RightLowerArm, 0.08f, 0.42f, 0, new Vector3(0.14f, 0f, 0f), "LowerArm_R", "ForeArm_R", "RightForeArm"),
                new BoneCapsuleDefinition(HumanBodyBones.LeftHand, 0.08f, 0.26f, 0, new Vector3(-0.08f, 0f, 0f), "Hand_L", "LeftHand"),
                new BoneCapsuleDefinition(HumanBodyBones.RightHand, 0.08f, 0.26f, 0, new Vector3(0.08f, 0f, 0f), "Hand_R", "RightHand"),
                new BoneCapsuleDefinition(HumanBodyBones.LeftUpperLeg, 0.1f, 0.58f, 1, new Vector3(0f, -0.1f, 0f), "UpperLeg_L", "LeftUpLeg"),
                new BoneCapsuleDefinition(HumanBodyBones.RightUpperLeg, 0.1f, 0.58f, 1, new Vector3(0f, -0.1f, 0f), "UpperLeg_R", "RightUpLeg"),
                new BoneCapsuleDefinition(HumanBodyBones.LeftLowerLeg, 0.09f, 0.56f, 1, new Vector3(0f, -0.12f, 0f), "LowerLeg_L", "LeftLeg"),
                new BoneCapsuleDefinition(HumanBodyBones.RightLowerLeg, 0.09f, 0.56f, 1, new Vector3(0f, -0.12f, 0f), "LowerLeg_R", "RightLeg"),
            };

            foreach (var definition in definitions)
            {
                var bone = FindBone(animator, definition.Bone, definition.FallbackNames);
                if (bone == null)
                {
                    continue;
                }

                bone.gameObject.layer = DamageableCharacterLayer;
                var collider = bone.GetComponent<CapsuleCollider>() ?? bone.gameObject.AddComponent<CapsuleCollider>();
                collider.isTrigger = false;
                collider.radius = definition.Radius;
                collider.height = definition.Height;
                collider.direction = definition.Direction;
                collider.center = definition.Center;
            }
        }

        private static ManualDamageCollider EnsureManualDamageCollider(Transform handBone, string hitboxName)
        {
            var hitbox = FindOrCreateDirectChild(handBone, hitboxName, DamageColliderLayer);
            var sphereCollider = hitbox.GetComponent<SphereCollider>();
            if (sphereCollider == null)
            {
                sphereCollider = hitbox.gameObject.AddComponent<SphereCollider>();
            }
            sphereCollider.isTrigger = true;
            sphereCollider.radius = 0.32f;
            sphereCollider.center = Vector3.zero;

            var damageCollider = hitbox.GetComponent<ManualDamageCollider>();
            if (damageCollider == null)
            {
                damageCollider = hitbox.gameObject.AddComponent<ManualDamageCollider>();
            }
            return damageCollider;
        }


        private static Transform FindOrCreateDirectChild(Transform parent, string childName, int layer)
        {
            foreach (Transform child in parent)
            {
                if (child.name == childName)
                {
                    child.gameObject.layer = layer;
                    return child;
                }
            }

            var childObject = new GameObject(childName);
            childObject.layer = layer;
            childObject.transform.SetParent(parent, false);
            childObject.transform.localPosition = Vector3.zero;
            childObject.transform.localRotation = Quaternion.identity;
            childObject.transform.localScale = Vector3.one;
            return childObject.transform;
        }

        private static Object GetSerializedReference(Object target, string propertyName)
        {
            var serializedObject = new SerializedObject(target);
            return serializedObject.FindProperty(propertyName).objectReferenceValue;
        }

        private static Avatar LoadMonsterAvatar()
        {
            var modelRoot = AssetDatabase.LoadAssetAtPath<GameObject>(MonsterModelPath);
            var modelAnimator = modelRoot != null ? modelRoot.GetComponent<Animator>() : null;
            if (modelAnimator != null && modelAnimator.avatar != null)
            {
                return modelAnimator.avatar;
            }

            var directAvatar = AssetDatabase.LoadAssetAtPath<Avatar>(MonsterModelPath);
            if (directAvatar != null)
            {
                return directAvatar;
            }

            return AssetDatabase.LoadAllAssetsAtPath(MonsterModelPath).OfType<Avatar>().FirstOrDefault();
        }

        private static Transform FindBone(Animator animator, HumanBodyBones humanoidBone, params string[] fallbackNames)
        {
            if (animator != null && animator.avatar != null && animator.avatar.isHuman)
            {
                var humanoidTransform = animator.GetBoneTransform(humanoidBone);
                if (humanoidTransform != null)
                {
                    return humanoidTransform;
                }
            }

            foreach (var transform in animator.GetComponentsInChildren<Transform>(true))
            {
                foreach (var fallbackName in fallbackNames)
                {
                    if (transform.name == fallbackName)
                    {
                        return transform;
                    }
                }
            }

            return null;
        }

        private static void SetIntPropertyIfPresent(SerializedObject serializedObject, string propertyPath, int value)
        {
            var property = serializedObject.FindProperty(propertyPath);
            if (property != null && property.propertyType == SerializedPropertyType.Integer)
            {
                property.intValue = value;
            }
        }

        private static void SetBoolPropertyIfPresent(SerializedObject serializedObject, string propertyPath, bool value)
        {
            var property = serializedObject.FindProperty(propertyPath);
            if (property != null && property.propertyType == SerializedPropertyType.Boolean)
            {
                property.boolValue = value;
            }
        }

        private static T LoadStateAsset<T>(string filter, string preferredGuid) where T : Object
        {
            var preferredPath = AssetDatabase.GUIDToAssetPath(preferredGuid);
            if (!string.IsNullOrEmpty(preferredPath))
            {
                var preferredAsset = AssetDatabase.LoadAssetAtPath<T>(preferredPath);
                if (preferredAsset != null)
                {
                    return preferredAsset;
                }
            }

            foreach (var guid in AssetDatabase.FindAssets(filter))
            {
                var asset = AssetDatabase.LoadAssetAtPath<T>(AssetDatabase.GUIDToAssetPath(guid));
                if (asset != null)
                {
                    return asset;
                }
            }

            return null;
        }

        private readonly struct BoneCapsuleDefinition
        {
            public readonly HumanBodyBones Bone;
            public readonly float Radius;
            public readonly float Height;
            public readonly int Direction;
            public readonly Vector3 Center;
            public readonly string[] FallbackNames;

            public BoneCapsuleDefinition(HumanBodyBones bone, float radius, float height, int direction, Vector3 center, params string[] fallbackNames)
            {
                Bone = bone;
                Radius = radius;
                Height = height;
                Direction = direction;
                Center = center;
                FallbackNames = fallbackNames;
            }
        }
    }
}
