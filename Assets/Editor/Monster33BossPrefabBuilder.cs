using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Animations;
using UnityEngine.SceneManagement;

namespace baodeag.EditorTools
{
    public static class Monster33BossPrefabBuilder
    {
        private const string SourceTemplatePath = "Assets/Prefabs/Character/Undead/Undead_Dummy_33.prefab";
        private const string BossReferencePath = "Assets/Prefabs/Character/Durk_Dummy_01.prefab";
        private const string MonsterVisualPath = "Assets/Stylized3DMonster/Monster33_FreeTrial/Prefab/Monster33_01.prefab";
        private const string MonsterModelPath = "Assets/Stylized3DMonster/Monster33_FreeTrial/Monster33_FreeTrial.fbx";
        private const string MonsterPrimaryMaterialPath = "Assets/Stylized3DMonster/Monster33_FreeTrial/ShaderTexture/Texture/Materials/Monster33_Color06.mat";
        private const string DurkAnimatorControllerPath = "Assets/Data/Animator Controllers/Durk.controller";
        private const string MonsterIdleAnimationPath = "Assets/Stylized3DMonster/Monster33_FreeTrial/Anim/Monster33_Idle.anim";
        private const string MonsterWalkAnimationPath = "Assets/Stylized3DMonster/Monster33_FreeTrial/Anim/Monster33_Walk_InPlace.anim";
        private const string MonsterRebasedIdleAnimationPath = "Assets/Stylized3DMonster/Monster33_FreeTrial/Anim/Monster33_Boss_Idle_Rebased.anim";
        private const string MonsterRebasedWalkAnimationPath = "Assets/Stylized3DMonster/Monster33_FreeTrial/Anim/Monster33_Boss_Walk_Rebased.anim";
        private const string TargetAnimatorControllerPath = "Assets/Data/Animator Controllers/Monster33_Boss_Clean.controller";
        private const string LegacyAnimatorControllerPath = "Assets/Data/Animator Controllers/Monster33_Boss.controller";
        private const string AttackStateTemplatePath = "Assets/Data/AI States/Undead/Undead Attack State.asset";
        private const string CombatStanceTemplatePath = "Assets/Data/AI States/Undead/Undead Combat Stance State.asset";
        private const string TargetAttackStatePath = "Assets/Data/AI States/Monster33/Monster33 Attack State.asset";
        private const string TargetCombatStanceStatePath = "Assets/Data/AI States/Monster33/Monster33 Combat Stance State.asset";
        private const string TargetPhase02CombatStanceStatePath = "Assets/Data/AI States/Monster33/Monster33 Combat Stance State Phase02.asset";
        private const string Attack01TemplatePath = "Assets/Data/AI Attack Actions/Undead/Attack 01.asset";
        private const string Attack01ComboTemplatePath = "Assets/Data/AI Attack Actions/Undead/Attack 01 Combo.asset";
        private const string Attack02TemplatePath = "Assets/Data/AI Attack Actions/Undead/Attack 02.asset";
        private const string TargetAttack01Path = "Assets/Data/AI Attack Actions/Monster33/Monster33 Attack 01.asset";
        private const string TargetAttack01ComboPath = "Assets/Data/AI Attack Actions/Monster33/Monster33 Attack 01 Combo.asset";
        private const string TargetAttack02Path = "Assets/Data/AI Attack Actions/Monster33/Monster33 Attack 02 Charged.asset";
        private const string TargetAttack03Path = "Assets/Data/AI Attack Actions/Monster33/Monster33 Attack 03 Charged.asset";
        private const string HellFireKnightAttackPath = "Assets/Art/Animations/Humanoid/Hell Fire Knight/Sword And Shield Attack.fbx";
        private const string HellFireKnightAttackRebasedPath = "Assets/Art/Animations/Humanoid/Hell Fire Knight/Monster33_Sword And Shield Attack.anim";
        private const string HellFireKnightSlashPath = "Assets/Art/Animations/Humanoid/Hell Fire Knight/Sword And Shield Slash.fbx";
        private const string HellFireKnightSlashRebasedPath = "Assets/Art/Animations/Humanoid/Hell Fire Knight/Monster33_Sword And Shield Slash.anim";
        private const string HellFireKnightSlashAltPath = "Assets/Art/Animations/Humanoid/Hell Fire Knight/Sword And Shield Slash (1).fbx";
        private const string HellFireKnightSlashAltRebasedPath = "Assets/Art/Animations/Humanoid/Hell Fire Knight/Monster33_Sword And Shield Slash (1).anim";
        private const string HellFireKnightComboPath = "Assets/Art/Animations/Humanoid/Hell Fire Knight/Sword And Shield Slash (1).fbx";
        private const string HellFireKnightComboRebasedPath = "Assets/Art/Animations/Humanoid/Hell Fire Knight/Monster33_Sword And Shield Combo.anim";
        private const string HellFireKnightPowerUpPath = "Assets/Art/Animations/Humanoid/Hell Fire Knight/Sword And Shield Power Up.fbx";
        private const string HellFireKnightPowerUpRebasedPath = "Assets/Art/Animations/Humanoid/Hell Fire Knight/Monster33_Sword And Shield Power Up.anim";
        private const string TargetPrefabPath = "Assets/Prefabs/Character/Monster33_Boss_01.prefab";
        private const int CharacterLayer = 6;
        private const int DamageableCharacterLayer = 7;
        private const int DamageColliderLayer = 10;
        private const string MonsterAnimationRootPrefix = "Monster33_VisualRoot/Monster33_FreeTrial/";

        [MenuItem("Tools/Bosses/Create Monster33 Boss")]
        public static void CreateMonster33Boss()
        {
            var sourceTemplate = AssetDatabase.LoadAssetAtPath<GameObject>(SourceTemplatePath);
            var bossReference = AssetDatabase.LoadAssetAtPath<GameObject>(BossReferencePath);
            bool targetPrefabExists = AssetDatabase.LoadAssetAtPath<GameObject>(TargetPrefabPath) != null;

            if (sourceTemplate == null || bossReference == null)
            {
                Debug.LogError("Monster33 boss builder is missing one of the required prefab assets.");
                return;
            }

            var bossReferenceManager = bossReference.GetComponent<AIBossCharacterManager>();
            if (bossReferenceManager == null)
            {
                Debug.LogError("Durk_Dummy_01 is missing AIBossCharacterManager, cannot copy boss settings.");
                return;
            }

            var prefabRoot = PrefabUtility.LoadPrefabContents(targetPrefabExists ? TargetPrefabPath : SourceTemplatePath);

            try
            {
                prefabRoot.name = "Monster33_Boss_01";
                prefabRoot.layer = CharacterLayer;

                ReplaceCharacterManager(prefabRoot, bossReferenceManager);
                ReplaceSoundFXManager(prefabRoot, bossReference);
                PrepareBossStats(prefabRoot);

                var visualTransform = prefabRoot.transform.Find("Monster33_VisualRoot");
                bool createdVisualThisRun = false;
                GameObject visualInstance;

                if (visualTransform != null)
                {
                    visualInstance = visualTransform.gameObject;
                }
                else
                {
                    RemoveOldVisualChildren(prefabRoot.transform);

                    visualInstance = CreateEmbeddedMonster33Visual(prefabRoot.scene);
                    if (visualInstance == null)
                    {
                        Debug.LogError("Failed to instantiate Monster33 visual prefab.");
                        return;
                    }

                    visualInstance.name = "Monster33_VisualRoot";
                    visualInstance.transform.SetParent(prefabRoot.transform, false);
                    visualInstance.transform.localPosition = Vector3.zero;
                    visualInstance.transform.localRotation = Quaternion.identity;
                    visualInstance.transform.localScale = Vector3.one;
                    createdVisualThisRun = true;
                }

                var monsterAvatar = LoadMonsterAvatar();
                var sourceAnimator = visualInstance.GetComponent<Animator>();
                var rootAnimator = prefabRoot.GetComponent<Animator>();

                if (createdVisualThisRun && monsterAvatar != null)
                {
                    if (sourceAnimator != null)
                    {
                        sourceAnimator.avatar = monsterAvatar;
                    }
                }

                if (rootAnimator == null)
                {
                    Debug.LogError($"Monster33 boss builder could not find required animator components. rootAnimatorNull={rootAnimator == null}, monsterAvatarNull={monsterAvatar == null}");
                    return;
                }

                if (monsterAvatar != null)
                {
                    rootAnimator.avatar = monsterAvatar;
                }
                else if (sourceAnimator != null && sourceAnimator.avatar != null)
                {
                    rootAnimator.avatar = sourceAnimator.avatar;
                }

                AssignMonster33Animator(rootAnimator);
                EnsureMonster33Materials(visualInstance);

                if (rootAnimator.avatar == null)
                {
                    Debug.LogError("Monster33 boss builder could not resolve a humanoid avatar for the boss.");
                    return;
                }

                if (createdVisualThisRun)
                {
                    ConfigureVisualHierarchy(visualInstance.transform);
                    EnsureWeaponConstraintBootstrap(visualInstance);
                    RebuildGameplayHooks(prefabRoot, visualInstance.transform, rootAnimator);
                }

                if (createdVisualThisRun && sourceAnimator != null)
                {
                    UnityEngine.Object.DestroyImmediate(sourceAnimator, true);
                }

                PrefabUtility.SaveAsPrefabAsset(prefabRoot, TargetPrefabPath);
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();

                Debug.Log(targetPrefabExists
                    ? "Updated Monster33_Boss_01 root logic/components only. Existing visual hierarchy/model edits were left untouched."
                    : "Created Monster33_Boss_01 prefab with boss gameplay hooks.");
            }
            finally
            {
                PrefabUtility.UnloadPrefabContents(prefabRoot);
            }
        }

        [MenuItem("Tools/Bosses/Make Monster33 Boss Prefab Editable")]
        public static void MakeMonster33BossPrefabEditable()
        {
            var prefabRoot = PrefabUtility.LoadPrefabContents(TargetPrefabPath);
            try
            {
                var visualRoot = prefabRoot.transform.Find("Monster33_VisualRoot");
                if (visualRoot == null)
                {
                    Debug.LogWarning("Monster33_Boss_01 does not contain Monster33_VisualRoot.");
                    return;
                }

                UnpackVisualHierarchyForEditing(visualRoot.gameObject);
                PrefabUtility.SaveAsPrefabAsset(prefabRoot, TargetPrefabPath);
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();
                Debug.Log("Monster33_Boss_01 visual hierarchy was unpacked for editing.");
            }
            finally
            {
                PrefabUtility.UnloadPrefabContents(prefabRoot);
            }
        }

        private static void ReplaceCharacterManager(GameObject prefabRoot, AIBossCharacterManager bossReferenceManager)
        {
            ReplaceNetworkManager(prefabRoot);
            var aiAssets = CreateOrUpdateMonster33AIAssets();

            ReplaceCombatManager(prefabRoot);

            var oldManager = prefabRoot.GetComponent<AICharacterManager>();
            if (oldManager != null)
            {
                UnityEngine.Object.DestroyImmediate(oldManager, true);
            }

            var bossManager = prefabRoot.GetComponent<AIMonster33CharacterManager>();
            if (bossManager == null)
            {
                bossManager = prefabRoot.AddComponent<AIMonster33CharacterManager>();
            }

            var serializedObject = new SerializedObject(bossManager);
            serializedObject.FindProperty("characterGroup").enumValueIndex = (int)CharacterGroup.Team02;
            serializedObject.FindProperty("characterName").stringValue = "Monster33, Hell Fire Knight";
            serializedObject.FindProperty("idle").objectReferenceValue = LoadStateAsset<IdleState>("t:IdleState", "18e987916d2ef1a44b5a64ea20b40e25");
            serializedObject.FindProperty("pursueTarget").objectReferenceValue = LoadStateAsset<PursueTargetState>("t:PursueTargetState", "858bd4aaa1b8cc54cbe09f1fe39629f3");
            serializedObject.FindProperty("combatStance").objectReferenceValue = aiAssets.CombatStanceState;
            serializedObject.FindProperty("attack").objectReferenceValue = aiAssets.AttackState;
            serializedObject.FindProperty("investigateSound").objectReferenceValue = LoadStateAsset<InvestigateSoundState>("t:InvestigateSoundState", "b01869362abef4045bd92e907638d143");
            serializedObject.FindProperty("bossIntroClip").objectReferenceValue = GetSerializedReference(bossReferenceManager, "bossIntroClip");
            serializedObject.FindProperty("bossBattleLoopClip").objectReferenceValue = GetSerializedReference(bossReferenceManager, "bossBattleLoopClip");
            serializedObject.FindProperty("autoWakeOnSpawn").boolValue = false;
            serializedObject.FindProperty("useNavMeshTranslationForInPlaceAnimations").boolValue = true;
            serializedObject.FindProperty("sleepAnimation").stringValue = "Sleep_01";
            serializedObject.FindProperty("awakenAnimation").stringValue = "Awaken_01";
            serializedObject.FindProperty("minimumHealthPercentageToShift").floatValue = 50f;
            serializedObject.FindProperty("phaseShiftAnimation").stringValue = "Phase_Change_01";
            serializedObject.FindProperty("phase02CombatStanceState").objectReferenceValue = aiAssets.Phase02CombatStanceState;
            serializedObject.FindProperty("sleepState").objectReferenceValue = GetSerializedReference(bossReferenceManager, "sleepState");
            serializedObject.ApplyModifiedPropertiesWithoutUndo();
            EditorUtility.SetDirty(bossManager);
        }

        private static void ReplaceCombatManager(GameObject prefabRoot)
        {
            var oldCombatManager = prefabRoot.GetComponent<AICharacterCombatManager>();
            if (oldCombatManager != null)
            {
                UnityEngine.Object.DestroyImmediate(oldCombatManager, true);
            }

            if (prefabRoot.GetComponent<AIMonster33CombatManager>() == null)
            {
                prefabRoot.AddComponent<AIMonster33CombatManager>();
            }
        }

        private static void ReplaceNetworkManager(GameObject prefabRoot)
        {
            var oldNetworkManager = prefabRoot.GetComponent<AICharacterNetworkManager>();
            if (oldNetworkManager != null && oldNetworkManager.GetType() != typeof(AIMonster33BossCharacterNetworkManager))
            {
                UnityEngine.Object.DestroyImmediate(oldNetworkManager, true);
            }

            if (prefabRoot.GetComponent<AIMonster33BossCharacterNetworkManager>() == null)
            {
                prefabRoot.AddComponent<AIMonster33BossCharacterNetworkManager>();
            }
        }

        private static void ReplaceSoundFXManager(GameObject prefabRoot, GameObject bossReference)
        {
            var referenceSoundFX = bossReference != null ? bossReference.GetComponent<CharacterSoundFXManager>() : null;

            var existingSoundManagers = prefabRoot.GetComponents<CharacterSoundFXManager>();
            foreach (var existingSoundManager in existingSoundManagers)
            {
                if (existingSoundManager != null)
                {
                    UnityEngine.Object.DestroyImmediate(existingSoundManager, true);
                }
            }

            var Monster33SoundFX = prefabRoot.AddComponent<AICharacterSoundFXManager>();
            if (referenceSoundFX != null)
            {
                var sourceSerializedObject = new SerializedObject(referenceSoundFX);
                var targetSerializedObject = new SerializedObject(Monster33SoundFX);

                CopyObjectArrayProperty(sourceSerializedObject.FindProperty("damageGrunts"), targetSerializedObject.FindProperty("damageGrunts"));
                CopyObjectArrayProperty(sourceSerializedObject.FindProperty("attackGrunts"), targetSerializedObject.FindProperty("attackGrunts"));
                CopyObjectArrayProperty(sourceSerializedObject.FindProperty("footSteps"), targetSerializedObject.FindProperty("footSteps"));
                targetSerializedObject.ApplyModifiedPropertiesWithoutUndo();
            }

            EditorUtility.SetDirty(Monster33SoundFX);
        }

        private static void PrepareBossStats(GameObject prefabRoot)
        {
            var networkManager = prefabRoot.GetComponent<AICharacterNetworkManager>();
            if (networkManager != null)
            {
                var serializedObject = new SerializedObject(networkManager);
                SetIntPropertyIfPresent(serializedObject, "currentHealth.m_InternalValue", 1800);
                SetIntPropertyIfPresent(serializedObject, "maxHealth.m_InternalValue", 1800);
                SetIntPropertyIfPresent(serializedObject, "currentStamina.m_InternalValue", 260);
                SetIntPropertyIfPresent(serializedObject, "maxStamina.m_InternalValue", 260);
                SetBoolPropertyIfPresent(serializedObject, "isAwake.m_InternalValue", true);
                serializedObject.ApplyModifiedPropertiesWithoutUndo();
                EditorUtility.SetDirty(networkManager);
            }

            var animator = prefabRoot.GetComponent<Animator>();
            if (animator != null)
            {
                animator.applyRootMotion = false;
                EditorUtility.SetDirty(animator);
            }

            var combatManager = prefabRoot.GetComponent<AIMonster33CombatManager>();
            if (combatManager != null)
            {
                var serializedObject = new SerializedObject(combatManager);
                serializedObject.FindProperty("baseDamage").intValue = 46;
                serializedObject.FindProperty("basePoiseDamage").intValue = 34;
                serializedObject.FindProperty("maxStance").floatValue = 165f;
                serializedObject.FindProperty("attack01DamageModifier").floatValue = 1.2f;
                serializedObject.FindProperty("attack02DamageModifier").floatValue = 1.55f;
                serializedObject.FindProperty("attack03DamageModifier").floatValue = 1.8f;
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
                UnityEngine.Object.DestroyImmediate(child, true);
            }
        }

        private static void ConfigureVisualHierarchy(Transform visualRoot)
        {
            foreach (var child in visualRoot.GetComponentsInChildren<Transform>(true))
            {
                child.gameObject.layer = DamageableCharacterLayer;
            }
        }

        private static void EnsureMonster33Materials(GameObject visualRoot)
        {
            if (visualRoot == null)
            {
                return;
            }

            var primaryMaterial = AssetDatabase.LoadAssetAtPath<Material>(MonsterPrimaryMaterialPath);
            if (primaryMaterial == null)
            {
                Debug.LogWarning("Monster33 builder could not load the primary color material.");
                return;
            }

            foreach (var renderer in visualRoot.GetComponentsInChildren<SkinnedMeshRenderer>(true))
            {
                if (renderer == null || renderer.sharedMaterials == null || renderer.sharedMaterials.Length == 0)
                {
                    continue;
                }

                var materials = renderer.sharedMaterials;
                var changed = false;

                for (var i = 0; i < materials.Length; i++)
                {
                    if (materials[i] == primaryMaterial)
                    {
                        continue;
                    }

                    materials[i] = primaryMaterial;
                    changed = true;
                }

                if (!changed)
                {
                    continue;
                }

                renderer.sharedMaterials = materials;
                EditorUtility.SetDirty(renderer);
            }
        }

        private static void RebuildGameplayHooks(GameObject prefabRoot, Transform visualRoot, Animator sourceAnimator)
        {
            EnsureLockOnTarget(sourceAnimator);
            EnsureMainHurtbox(sourceAnimator);
            EnsureBodyColliders(sourceAnimator);
            EnsureWeaponConstraints(sourceAnimator);
            EnsureWeaponDamageColliders(prefabRoot, sourceAnimator);
        }

        private static GameObject CreateEmbeddedMonster33Visual(Scene targetScene)
        {
            var visualRoot = PrefabUtility.LoadPrefabContents(MonsterVisualPath);
            try
            {
                UnpackVisualHierarchyForEditing(visualRoot);

                var embeddedVisual = UnityEngine.Object.Instantiate(visualRoot);
                embeddedVisual.name = "Monster33_VisualRoot";
                SceneManager.MoveGameObjectToScene(embeddedVisual, targetScene);
                return embeddedVisual;
            }
            finally
            {
                PrefabUtility.UnloadPrefabContents(visualRoot);
            }
        }

        private static void UnpackVisualHierarchyForEditing(GameObject visualRoot)
        {
            if (visualRoot == null)
            {
                return;
            }

            if (PrefabUtility.IsAnyPrefabInstanceRoot(visualRoot))
            {
                PrefabUtility.UnpackPrefabInstance(visualRoot, PrefabUnpackMode.Completely, InteractionMode.AutomatedAction);
            }

            foreach (var childTransform in visualRoot.GetComponentsInChildren<Transform>(true).ToArray())
            {
                var childObject = childTransform.gameObject;
                if (!PrefabUtility.IsAnyPrefabInstanceRoot(childObject))
                {
                    continue;
                }

                PrefabUtility.UnpackPrefabInstance(childObject, PrefabUnpackMode.Completely, InteractionMode.AutomatedAction);
            }
        }

        private static void AssignMonster33Animator(Animator rootAnimator)
        {
            var controller = CreateOrUpdateMonster33AnimatorController();
            if (controller == null)
            {
                return;
            }

            rootAnimator.runtimeAnimatorController = controller;
            EditorUtility.SetDirty(rootAnimator);
        }

        private static AnimatorController CreateOrUpdateMonster33AnimatorController()
        {
            var monsterIdleClip = CreateOrUpdateRebasedAnimationClip(MonsterIdleAnimationPath, MonsterRebasedIdleAnimationPath);
            var monsterWalkClip = CreateOrUpdateRebasedAnimationClip(MonsterWalkAnimationPath, MonsterRebasedWalkAnimationPath);
            var attack01Clip = CreateOrUpdateMappedAttackClip(
                HellFireKnightAttackPath,
                HellFireKnightAttackRebasedPath,
                "SetAttack01Damage",
                0.30f,
                0.56f,
                true);
            var attack01ComboClip = CreateOrUpdateMappedAttackClip(
                HellFireKnightComboPath,
                HellFireKnightComboRebasedPath,
                "SetAttack01Damage",
                0.26f,
                0.66f,
                false);
            var attack02Clip = CreateOrUpdateMappedAttackClip(
                HellFireKnightSlashPath,
                HellFireKnightSlashRebasedPath,
                "SetAttack02Damage",
                0.28f,
                0.62f,
                false);
            var attack03Clip = CreateOrUpdateMappedAttackClip(
                HellFireKnightSlashAltPath,
                HellFireKnightSlashAltRebasedPath,
                "SetAttack03Damage",
                0.34f,
                0.7f,
                false);
            var phaseChangeClip = CreateOrUpdateMappedAttackClip(
                HellFireKnightPowerUpPath,
                HellFireKnightPowerUpRebasedPath,
                null,
                0f,
                0f,
                false);

            if (monsterIdleClip == null
                || monsterWalkClip == null
                || attack01Clip == null
                || attack01ComboClip == null
                || attack02Clip == null
                || attack03Clip == null
                || phaseChangeClip == null)
            {
                Debug.LogError("Monster33 boss builder could not create the Monster33 boss animation clips.");
                return null;
            }

            EnsureFolderExistsForAsset(TargetAnimatorControllerPath);

            if (AssetDatabase.LoadAssetAtPath<AnimatorController>(TargetAnimatorControllerPath) != null)
            {
                AssetDatabase.DeleteAsset(TargetAnimatorControllerPath);
                AssetDatabase.SaveAssets();
            }

            if (AssetDatabase.LoadAssetAtPath<AnimatorController>(TargetAnimatorControllerPath) == null)
            {
                if (!AssetDatabase.CopyAsset(DurkAnimatorControllerPath, TargetAnimatorControllerPath))
                {
                    Debug.LogError("Monster33 boss builder could not duplicate Durk.controller.");
                    return null;
                }
            }

            var controller = AssetDatabase.LoadAssetAtPath<AnimatorController>(TargetAnimatorControllerPath);
            if (controller == null)
            {
                Debug.LogError("Monster33 boss builder could not load Monster33_Boss.controller after duplication.");
                return null;
            }

            controller.name = "Monster33_Boss";
            ApplyStateMotion(controller, "Idle", monsterIdleClip);
            ApplyStateMotion(controller, "Locomotion", monsterWalkClip);
            ApplyStateMotion(controller, "Attack_01", attack01Clip);
            ApplyStateMotion(controller, "Attack_02", attack02Clip);
            ApplyStateMotion(controller, "Attack_03", attack03Clip);
            ApplyStateMotion(controller, "Phase_Change_01", phaseChangeClip);
            EnsureComboState(controller, attack01ComboClip);

            EditorUtility.SetDirty(controller);
            AssetDatabase.SaveAssets();
            AssetDatabase.ImportAsset(TargetAnimatorControllerPath, ImportAssetOptions.ForceUpdate);
            DeleteLegacyAnimatorController();

            return AssetDatabase.LoadAssetAtPath<AnimatorController>(TargetAnimatorControllerPath);
        }

        private static AnimationClip CreateOrUpdateRebasedAnimationClip(string sourceClipPath, string rebasedClipPath)
        {
            var sourceClip = AssetDatabase.LoadAssetAtPath<AnimationClip>(sourceClipPath);
            if (sourceClip == null)
            {
                Debug.LogError($"Monster33 boss builder could not load source clip at {sourceClipPath}.");
                return null;
            }

            var rebasedClip = AssetDatabase.LoadAssetAtPath<AnimationClip>(rebasedClipPath);
            if (rebasedClip == null)
            {
                rebasedClip = UnityEngine.Object.Instantiate(sourceClip);
                rebasedClip.name = System.IO.Path.GetFileNameWithoutExtension(rebasedClipPath);
                AssetDatabase.CreateAsset(rebasedClip, rebasedClipPath);
            }
            else
            {
                EditorUtility.CopySerialized(sourceClip, rebasedClip);
            }

            rebasedClip.name = System.IO.Path.GetFileNameWithoutExtension(rebasedClipPath);
            RebaseAnimationClipPaths(rebasedClip, MonsterAnimationRootPrefix);
            ConfigureLoopingForLocomotionClip(rebasedClip, rebasedClipPath);
            EditorUtility.SetDirty(rebasedClip);
            AssetDatabase.SaveAssets();

            return rebasedClip;
        }

        private static AnimationClip LoadPrimaryAnimationClip(string assetPath)
        {
            return AssetDatabase.LoadAllAssetsAtPath(assetPath)
                .OfType<AnimationClip>()
                .FirstOrDefault(clip => !clip.name.StartsWith("__preview__", StringComparison.Ordinal));
        }

        private static AnimationClip CreateOrUpdateMappedAttackClip(
            string sourceAssetPath,
            string targetClipPath,
            string setDamageFunctionName,
            float openNormalizedTime,
            float closeNormalizedTime,
            bool enablesCombo)
        {
            var sourceClip = LoadPrimaryAnimationClip(sourceAssetPath);
            if (sourceClip == null)
            {
                Debug.LogError($"Monster33 boss builder could not load source clip at {sourceAssetPath}.");
                return null;
            }

            EnsureFolderExistsForAsset(targetClipPath);

            var sourceBonePaths = BuildHumanoidBonePathMapWithFallback(
                sourceAssetPath,
                null,
                sourceAssetPath);
            var targetBonePaths = BuildHumanoidBonePathMapWithFallback(
                MonsterVisualPath,
                "Monster33_VisualRoot",
                MonsterVisualPath,
                MonsterModelPath);

            if (sourceBonePaths.Count == 0 || targetBonePaths.Count == 0)
            {
                Debug.LogWarning(
                    $"Monster33 boss builder could not build humanoid bone path maps for attack retargeting. " +
                    $"source='{sourceAssetPath}' ({sourceBonePaths.Count} bones), target='{MonsterVisualPath}'/'{MonsterModelPath}' ({targetBonePaths.Count} bones). " +
                    "Falling back to direct skeleton path remapping.");
            }

            var targetClip = AssetDatabase.LoadAssetAtPath<AnimationClip>(targetClipPath);
            if (targetClip == null)
            {
                targetClip = new AnimationClip
                {
                    name = System.IO.Path.GetFileNameWithoutExtension(targetClipPath)
                };
                AssetDatabase.CreateAsset(targetClip, targetClipPath);
            }

            ClearClipCurves(targetClip);

            int mappedFloatCurveCount = 0;
            foreach (var binding in AnimationUtility.GetCurveBindings(sourceClip))
            {
                if (!TryMapBindingPath(binding.path, sourceBonePaths, targetBonePaths, out var mappedPath))
                {
                    continue;
                }

                var curve = AnimationUtility.GetEditorCurve(sourceClip, binding);
                var mappedBinding = binding;
                mappedBinding.path = mappedPath;
                AnimationUtility.SetEditorCurve(targetClip, mappedBinding, curve);
                mappedFloatCurveCount++;
            }

            int mappedObjectCurveCount = 0;
            foreach (var binding in AnimationUtility.GetObjectReferenceCurveBindings(sourceClip))
            {
                if (!TryMapBindingPath(binding.path, sourceBonePaths, targetBonePaths, out var mappedPath))
                {
                    continue;
                }

                var curve = AnimationUtility.GetObjectReferenceCurve(sourceClip, binding);
                var mappedBinding = binding;
                mappedBinding.path = mappedPath;
                AnimationUtility.SetObjectReferenceCurve(targetClip, mappedBinding, curve);
                mappedObjectCurveCount++;
            }

            if (mappedFloatCurveCount == 0 && mappedObjectCurveCount == 0)
            {
                Debug.LogError(
                    $"Monster33 boss builder mapped 0 curves for '{sourceClip.name}' into '{targetClipPath}'. " +
                    "This usually means the source skeleton paths do not match the humanoid bone map.");
                return null;
            }

            targetClip.name = System.IO.Path.GetFileNameWithoutExtension(targetClipPath);
            ConfigureLoopingForOneShotClip(targetClip);
            AnimationUtility.SetAnimationEvents(
                targetClip,
                string.IsNullOrEmpty(setDamageFunctionName)
                    ? Array.Empty<AnimationEvent>()
                    : BuildAttackEvents(sourceClip.length, setDamageFunctionName, openNormalizedTime, closeNormalizedTime, enablesCombo));
            EditorUtility.SetDirty(targetClip);
            AssetDatabase.SaveAssets();

            return targetClip;
        }

        private static Dictionary<HumanBodyBones, string> BuildHumanoidBonePathMapWithFallback(
            string primaryAssetPath,
            string rootPrefix,
            params string[] candidateAssetPaths)
        {
            foreach (var candidateAssetPath in candidateAssetPaths.Where(path => !string.IsNullOrEmpty(path)).Distinct())
            {
                string candidatePrefix = GetBonePathRootPrefix(candidateAssetPath, rootPrefix);
                var bonePaths = BuildHumanoidBonePathMap(candidateAssetPath, candidatePrefix);
                if (bonePaths.Count > 0)
                {
                    return bonePaths;
                }
            }

            return new Dictionary<HumanBodyBones, string>();
        }

        private static Dictionary<HumanBodyBones, string> BuildHumanoidBonePathMap(string assetPath, string rootPrefix)
        {
            var result = new Dictionary<HumanBodyBones, string>();
            var asset = AssetDatabase.LoadAssetAtPath<GameObject>(assetPath);
            if (asset == null)
            {
                return result;
            }

            var instance = InstantiateHumanoidAsset(asset);
            if (instance == null)
            {
                Debug.LogWarning($"Monster33 boss builder could not instantiate humanoid asset at {assetPath}.");
                return result;
            }

            try
            {
                var animator = instance.GetComponent<Animator>() ?? instance.GetComponentInChildren<Animator>();
                if (animator == null || animator.avatar == null || !animator.avatar.isHuman)
                {
                    Debug.LogWarning($"Monster33 boss builder found no humanoid animator/avatar at {assetPath}.");
                    return result;
                }

                foreach (HumanBodyBones bone in Enum.GetValues(typeof(HumanBodyBones)))
                {
                    if (bone == HumanBodyBones.LastBone)
                    {
                        continue;
                    }

                    var transform = animator.GetBoneTransform(bone);
                    if (transform == null)
                    {
                        continue;
                    }

                    string path = AnimationUtility.CalculateTransformPath(transform, instance.transform);
                    if (!string.IsNullOrEmpty(rootPrefix))
                    {
                        path = string.IsNullOrEmpty(path) ? rootPrefix : $"{rootPrefix}/{path}";
                    }

                    result[bone] = path;
                }
            }
            finally
            {
                UnityEngine.Object.DestroyImmediate(instance);
            }

            return result;
        }

        private static GameObject InstantiateHumanoidAsset(GameObject asset)
        {
            if (asset == null)
            {
                return null;
            }

            var instance = PrefabUtility.InstantiatePrefab(asset) as GameObject;
            if (instance != null)
            {
                return instance;
            }

            instance = UnityEngine.Object.Instantiate(asset);
            if (instance != null)
            {
                instance.hideFlags = HideFlags.HideAndDontSave;
            }

            return instance;
        }

        private static string GetBonePathRootPrefix(string assetPath, string requestedRootPrefix)
        {
            if (string.IsNullOrEmpty(requestedRootPrefix))
            {
                return null;
            }

            if (string.Equals(assetPath, MonsterModelPath, StringComparison.Ordinal))
            {
                return MonsterAnimationRootPrefix.TrimEnd('/');
            }

            return requestedRootPrefix;
        }

        private static bool TryMapHumanoidBindingPath(
            string sourcePath,
            Dictionary<HumanBodyBones, string> sourceBonePaths,
            Dictionary<HumanBodyBones, string> targetBonePaths,
            out string mappedPath)
        {
            foreach (var pair in sourceBonePaths)
            {
                if (!targetBonePaths.TryGetValue(pair.Key, out var targetPath))
                {
                    continue;
                }

                if (sourcePath == pair.Value)
                {
                    mappedPath = targetPath;
                    return true;
                }

                if (!string.IsNullOrEmpty(pair.Value) && sourcePath.StartsWith(pair.Value + "/", StringComparison.Ordinal))
                {
                    string suffix = sourcePath.Substring(pair.Value.Length + 1);
                    mappedPath = string.IsNullOrEmpty(targetPath) ? suffix : $"{targetPath}/{suffix}";
                    return true;
                }
            }

            mappedPath = null;
            return false;
        }

        private static bool TryMapBindingPath(
            string sourcePath,
            Dictionary<HumanBodyBones, string> sourceBonePaths,
            Dictionary<HumanBodyBones, string> targetBonePaths,
            out string mappedPath)
        {
            if (TryMapHumanoidBindingPath(sourcePath, sourceBonePaths, targetBonePaths, out mappedPath))
            {
                return true;
            }

            return TryMapMonster33BindingPath(sourcePath, out mappedPath);
        }

        private static bool TryMapMonster33BindingPath(string sourcePath, out string mappedPath)
        {
            foreach (var pair in GetMonster33SourceToTargetPathMap().OrderByDescending(entry => entry.Key.Length))
            {
                if (!string.Equals(sourcePath, pair.Key, StringComparison.Ordinal)
                    && !sourcePath.StartsWith(pair.Key + "/", StringComparison.Ordinal))
                {
                    continue;
                }

                string suffix = sourcePath.Length == pair.Key.Length
                    ? string.Empty
                    : sourcePath.Substring(pair.Key.Length + 1);

                mappedPath = string.IsNullOrEmpty(suffix)
                    ? pair.Value
                    : $"{pair.Value}/{suffix}";
                return true;
            }

            mappedPath = null;
            return false;
        }

        private static Dictionary<string, string> GetMonster33SourceToTargetPathMap()
        {
            string prefix = MonsterAnimationRootPrefix.TrimEnd('/');
            return new Dictionary<string, string>
            {
                ["mixamorig:Hips"] = $"{prefix}/root/root.x",
                ["mixamorig:Hips/mixamorig:Spine"] = $"{prefix}/root/root.x/spine_01.x",
                ["mixamorig:Hips/mixamorig:Spine/mixamorig:Spine1"] = $"{prefix}/root/root.x/spine_01.x/spine_02.x",
                ["mixamorig:Hips/mixamorig:Spine/mixamorig:Spine1/mixamorig:Spine2"] = $"{prefix}/root/root.x/spine_01.x/spine_02.x/spine_03.x",
                ["mixamorig:Hips/mixamorig:Spine/mixamorig:Spine1/mixamorig:Spine2/mixamorig:Neck"] = $"{prefix}/root/root.x/spine_01.x/spine_02.x/spine_03.x/neck.x",
                ["mixamorig:Hips/mixamorig:Spine/mixamorig:Spine1/mixamorig:Spine2/mixamorig:Neck/mixamorig:Head"] = $"{prefix}/root/root.x/spine_01.x/spine_02.x/spine_03.x/neck.x/head.x",
                ["mixamorig:Hips/mixamorig:Spine/mixamorig:Spine1/mixamorig:Spine2/mixamorig:LeftShoulder"] = $"{prefix}/root/root.x/spine_01.x/spine_02.x/spine_03.x/shoulder.l",
                ["mixamorig:Hips/mixamorig:Spine/mixamorig:Spine1/mixamorig:Spine2/mixamorig:LeftShoulder/mixamorig:LeftArm"] = $"{prefix}/root/root.x/spine_01.x/spine_02.x/spine_03.x/shoulder.l/arm_stretch.l",
                ["mixamorig:Hips/mixamorig:Spine/mixamorig:Spine1/mixamorig:Spine2/mixamorig:LeftShoulder/mixamorig:LeftArm/mixamorig:LeftForeArm"] = $"{prefix}/root/root.x/spine_01.x/spine_02.x/spine_03.x/shoulder.l/arm_stretch.l/forearm_stretch.l",
                ["mixamorig:Hips/mixamorig:Spine/mixamorig:Spine1/mixamorig:Spine2/mixamorig:LeftShoulder/mixamorig:LeftArm/mixamorig:LeftForeArm/mixamorig:LeftHand"] = $"{prefix}/root/root.x/spine_01.x/spine_02.x/spine_03.x/shoulder.l/arm_stretch.l/forearm_stretch.l/hand.l",
                ["mixamorig:Hips/mixamorig:Spine/mixamorig:Spine1/mixamorig:Spine2/mixamorig:LeftShoulder/mixamorig:LeftArm/mixamorig:LeftForeArm/mixamorig:LeftHand/mixamorig:LeftHandThumb1"] = $"{prefix}/root/root.x/spine_01.x/spine_02.x/spine_03.x/shoulder.l/arm_stretch.l/forearm_stretch.l/hand.l/thumb1.l",
                ["mixamorig:Hips/mixamorig:Spine/mixamorig:Spine1/mixamorig:Spine2/mixamorig:LeftShoulder/mixamorig:LeftArm/mixamorig:LeftForeArm/mixamorig:LeftHand/mixamorig:LeftHandThumb1/mixamorig:LeftHandThumb2"] = $"{prefix}/root/root.x/spine_01.x/spine_02.x/spine_03.x/shoulder.l/arm_stretch.l/forearm_stretch.l/hand.l/thumb1.l/thumb2.l",
                ["mixamorig:Hips/mixamorig:Spine/mixamorig:Spine1/mixamorig:Spine2/mixamorig:LeftShoulder/mixamorig:LeftArm/mixamorig:LeftForeArm/mixamorig:LeftHand/mixamorig:LeftHandThumb1/mixamorig:LeftHandThumb2/mixamorig:LeftHandThumb3"] = $"{prefix}/root/root.x/spine_01.x/spine_02.x/spine_03.x/shoulder.l/arm_stretch.l/forearm_stretch.l/hand.l/thumb1.l/thumb2.l/thumb3.l",
                ["mixamorig:Hips/mixamorig:Spine/mixamorig:Spine1/mixamorig:Spine2/mixamorig:LeftShoulder/mixamorig:LeftArm/mixamorig:LeftForeArm/mixamorig:LeftHand/mixamorig:LeftHandIndex1"] = $"{prefix}/root/root.x/spine_01.x/spine_02.x/spine_03.x/shoulder.l/arm_stretch.l/forearm_stretch.l/hand.l/index1_base.l",
                ["mixamorig:Hips/mixamorig:Spine/mixamorig:Spine1/mixamorig:Spine2/mixamorig:LeftShoulder/mixamorig:LeftArm/mixamorig:LeftForeArm/mixamorig:LeftHand/mixamorig:LeftHandIndex1/mixamorig:LeftHandIndex2"] = $"{prefix}/root/root.x/spine_01.x/spine_02.x/spine_03.x/shoulder.l/arm_stretch.l/forearm_stretch.l/hand.l/index1_base.l/index1.l",
                ["mixamorig:Hips/mixamorig:Spine/mixamorig:Spine1/mixamorig:Spine2/mixamorig:LeftShoulder/mixamorig:LeftArm/mixamorig:LeftForeArm/mixamorig:LeftHand/mixamorig:LeftHandIndex1/mixamorig:LeftHandIndex2/mixamorig:LeftHandIndex3"] = $"{prefix}/root/root.x/spine_01.x/spine_02.x/spine_03.x/shoulder.l/arm_stretch.l/forearm_stretch.l/hand.l/index1_base.l/index1.l/index2.l",
                ["mixamorig:Hips/mixamorig:Spine/mixamorig:Spine1/mixamorig:Spine2/mixamorig:LeftShoulder/mixamorig:LeftArm/mixamorig:LeftForeArm/mixamorig:LeftHand/mixamorig:LeftHandMiddle1"] = $"{prefix}/root/root.x/spine_01.x/spine_02.x/spine_03.x/shoulder.l/arm_stretch.l/forearm_stretch.l/hand.l/middle1_base.l",
                ["mixamorig:Hips/mixamorig:Spine/mixamorig:Spine1/mixamorig:Spine2/mixamorig:LeftShoulder/mixamorig:LeftArm/mixamorig:LeftForeArm/mixamorig:LeftHand/mixamorig:LeftHandMiddle1/mixamorig:LeftHandMiddle2"] = $"{prefix}/root/root.x/spine_01.x/spine_02.x/spine_03.x/shoulder.l/arm_stretch.l/forearm_stretch.l/hand.l/middle1_base.l/middle1.l",
                ["mixamorig:Hips/mixamorig:Spine/mixamorig:Spine1/mixamorig:Spine2/mixamorig:LeftShoulder/mixamorig:LeftArm/mixamorig:LeftForeArm/mixamorig:LeftHand/mixamorig:LeftHandMiddle1/mixamorig:LeftHandMiddle2/mixamorig:LeftHandMiddle3"] = $"{prefix}/root/root.x/spine_01.x/spine_02.x/spine_03.x/shoulder.l/arm_stretch.l/forearm_stretch.l/hand.l/middle1_base.l/middle1.l/middle2.l",
                ["mixamorig:Hips/mixamorig:Spine/mixamorig:Spine1/mixamorig:Spine2/mixamorig:RightShoulder"] = $"{prefix}/root/root.x/spine_01.x/spine_02.x/spine_03.x/shoulder.r",
                ["mixamorig:Hips/mixamorig:Spine/mixamorig:Spine1/mixamorig:Spine2/mixamorig:RightShoulder/mixamorig:RightArm"] = $"{prefix}/root/root.x/spine_01.x/spine_02.x/spine_03.x/shoulder.r/arm_stretch.r",
                ["mixamorig:Hips/mixamorig:Spine/mixamorig:Spine1/mixamorig:Spine2/mixamorig:RightShoulder/mixamorig:RightArm/mixamorig:RightForeArm"] = $"{prefix}/root/root.x/spine_01.x/spine_02.x/spine_03.x/shoulder.r/arm_stretch.r/forearm_stretch.r",
                ["mixamorig:Hips/mixamorig:Spine/mixamorig:Spine1/mixamorig:Spine2/mixamorig:RightShoulder/mixamorig:RightArm/mixamorig:RightForeArm/mixamorig:RightHand"] = $"{prefix}/root/root.x/spine_01.x/spine_02.x/spine_03.x/shoulder.r/arm_stretch.r/forearm_stretch.r/hand.r",
                ["mixamorig:Hips/mixamorig:Spine/mixamorig:Spine1/mixamorig:Spine2/mixamorig:RightShoulder/mixamorig:RightArm/mixamorig:RightForeArm/mixamorig:RightHand/mixamorig:RightHandThumb1"] = $"{prefix}/root/root.x/spine_01.x/spine_02.x/spine_03.x/shoulder.r/arm_stretch.r/forearm_stretch.r/hand.r/thumb1.r",
                ["mixamorig:Hips/mixamorig:Spine/mixamorig:Spine1/mixamorig:Spine2/mixamorig:RightShoulder/mixamorig:RightArm/mixamorig:RightForeArm/mixamorig:RightHand/mixamorig:RightHandThumb1/mixamorig:RightHandThumb2"] = $"{prefix}/root/root.x/spine_01.x/spine_02.x/spine_03.x/shoulder.r/arm_stretch.r/forearm_stretch.r/hand.r/thumb1.r/thumb2.r",
                ["mixamorig:Hips/mixamorig:Spine/mixamorig:Spine1/mixamorig:Spine2/mixamorig:RightShoulder/mixamorig:RightArm/mixamorig:RightForeArm/mixamorig:RightHand/mixamorig:RightHandThumb1/mixamorig:RightHandThumb2/mixamorig:RightHandThumb3"] = $"{prefix}/root/root.x/spine_01.x/spine_02.x/spine_03.x/shoulder.r/arm_stretch.r/forearm_stretch.r/hand.r/thumb1.r/thumb2.r/thumb3.r",
                ["mixamorig:Hips/mixamorig:Spine/mixamorig:Spine1/mixamorig:Spine2/mixamorig:RightShoulder/mixamorig:RightArm/mixamorig:RightForeArm/mixamorig:RightHand/mixamorig:RightHandIndex1"] = $"{prefix}/root/root.x/spine_01.x/spine_02.x/spine_03.x/shoulder.r/arm_stretch.r/forearm_stretch.r/hand.r/index1_base.r",
                ["mixamorig:Hips/mixamorig:Spine/mixamorig:Spine1/mixamorig:Spine2/mixamorig:RightShoulder/mixamorig:RightArm/mixamorig:RightForeArm/mixamorig:RightHand/mixamorig:RightHandIndex1/mixamorig:RightHandIndex2"] = $"{prefix}/root/root.x/spine_01.x/spine_02.x/spine_03.x/shoulder.r/arm_stretch.r/forearm_stretch.r/hand.r/index1_base.r/index1.r",
                ["mixamorig:Hips/mixamorig:Spine/mixamorig:Spine1/mixamorig:Spine2/mixamorig:RightShoulder/mixamorig:RightArm/mixamorig:RightForeArm/mixamorig:RightHand/mixamorig:RightHandIndex1/mixamorig:RightHandIndex2/mixamorig:RightHandIndex3"] = $"{prefix}/root/root.x/spine_01.x/spine_02.x/spine_03.x/shoulder.r/arm_stretch.r/forearm_stretch.r/hand.r/index1_base.r/index1.r/index2.r",
                ["mixamorig:Hips/mixamorig:Spine/mixamorig:Spine1/mixamorig:Spine2/mixamorig:RightShoulder/mixamorig:RightArm/mixamorig:RightForeArm/mixamorig:RightHand/mixamorig:RightHandMiddle1"] = $"{prefix}/root/root.x/spine_01.x/spine_02.x/spine_03.x/shoulder.r/arm_stretch.r/forearm_stretch.r/hand.r/middle1_base.r",
                ["mixamorig:Hips/mixamorig:Spine/mixamorig:Spine1/mixamorig:Spine2/mixamorig:RightShoulder/mixamorig:RightArm/mixamorig:RightForeArm/mixamorig:RightHand/mixamorig:RightHandMiddle1/mixamorig:RightHandMiddle2"] = $"{prefix}/root/root.x/spine_01.x/spine_02.x/spine_03.x/shoulder.r/arm_stretch.r/forearm_stretch.r/hand.r/middle1_base.r/middle1.r",
                ["mixamorig:Hips/mixamorig:Spine/mixamorig:Spine1/mixamorig:Spine2/mixamorig:RightShoulder/mixamorig:RightArm/mixamorig:RightForeArm/mixamorig:RightHand/mixamorig:RightHandMiddle1/mixamorig:RightHandMiddle2/mixamorig:RightHandMiddle3"] = $"{prefix}/root/root.x/spine_01.x/spine_02.x/spine_03.x/shoulder.r/arm_stretch.r/forearm_stretch.r/hand.r/middle1_base.r/middle1.r/middle2.r",
                ["mixamorig:Hips/mixamorig:LeftUpLeg"] = $"{prefix}/root/root.x/thigh_stretch.l",
                ["mixamorig:Hips/mixamorig:LeftUpLeg/mixamorig:LeftLeg"] = $"{prefix}/root/root.x/thigh_stretch.l/leg_stretch.l",
                ["mixamorig:Hips/mixamorig:LeftUpLeg/mixamorig:LeftLeg/mixamorig:LeftFoot"] = $"{prefix}/root/root.x/thigh_stretch.l/leg_stretch.l/foot.l",
                ["mixamorig:Hips/mixamorig:LeftUpLeg/mixamorig:LeftLeg/mixamorig:LeftFoot/mixamorig:LeftToeBase"] = $"{prefix}/root/root.x/thigh_stretch.l/leg_stretch.l/foot.l/toes_01.l",
                ["mixamorig:Hips/mixamorig:RightUpLeg"] = $"{prefix}/root/root.x/thigh_stretch.r",
                ["mixamorig:Hips/mixamorig:RightUpLeg/mixamorig:RightLeg"] = $"{prefix}/root/root.x/thigh_stretch.r/leg_stretch.r",
                ["mixamorig:Hips/mixamorig:RightUpLeg/mixamorig:RightLeg/mixamorig:RightFoot"] = $"{prefix}/root/root.x/thigh_stretch.r/leg_stretch.r/foot.r",
                ["mixamorig:Hips/mixamorig:RightUpLeg/mixamorig:RightLeg/mixamorig:RightFoot/mixamorig:RightToeBase"] = $"{prefix}/root/root.x/thigh_stretch.r/leg_stretch.r/foot.r/toes_01.r"
            };
        }

        private static void ClearClipCurves(AnimationClip clip)
        {
            foreach (var binding in AnimationUtility.GetCurveBindings(clip))
            {
                AnimationUtility.SetEditorCurve(clip, binding, null);
            }

            foreach (var binding in AnimationUtility.GetObjectReferenceCurveBindings(clip))
            {
                AnimationUtility.SetObjectReferenceCurve(clip, binding, null);
            }

            AnimationUtility.SetAnimationEvents(clip, Array.Empty<AnimationEvent>());
        }

        private static void ConfigureLoopingForOneShotClip(AnimationClip clip)
        {
            if (clip == null)
            {
                return;
            }

            var serializedClip = new SerializedObject(clip);
            var clipSettings = serializedClip.FindProperty("m_AnimationClipSettings");
            if (clipSettings == null)
            {
                return;
            }

            clipSettings.FindPropertyRelative("m_LoopTime").boolValue = false;
            clipSettings.FindPropertyRelative("m_LoopBlend").boolValue = false;
            clipSettings.FindPropertyRelative("m_LoopBlendOrientation").boolValue = false;
            clipSettings.FindPropertyRelative("m_LoopBlendPositionY").boolValue = false;
            clipSettings.FindPropertyRelative("m_LoopBlendPositionXZ").boolValue = false;
            serializedClip.ApplyModifiedPropertiesWithoutUndo();
        }

        private static void CopyObjectArrayProperty(SerializedProperty source, SerializedProperty target)
        {
            if (source == null || target == null || !source.isArray || !target.isArray)
            {
                return;
            }

            target.arraySize = source.arraySize;
            for (int i = 0; i < source.arraySize; i++)
            {
                target.GetArrayElementAtIndex(i).objectReferenceValue = source.GetArrayElementAtIndex(i).objectReferenceValue;
            }
        }

        private static AnimationEvent[] BuildAttackEvents(
            float clipLength,
            string setDamageFunctionName,
            float openNormalizedTime,
            float closeNormalizedTime,
            bool enablesCombo)
        {
            float openTime = Mathf.Clamp01(openNormalizedTime) * clipLength;
            float closeTime = Mathf.Clamp(Mathf.Max(closeNormalizedTime, openNormalizedTime + 0.05f), 0f, 0.98f) * clipLength;
            float rotateEnableTime = Mathf.Min(openTime * 0.5f, clipLength * 0.18f);
            float rotateDisableTime = Mathf.Min(closeTime + (clipLength * 0.08f), clipLength * 0.92f);

            var events = new List<AnimationEvent>
            {
                CreateAnimationEvent(clipLength * 0.02f, setDamageFunctionName),
                CreateAnimationEvent(rotateEnableTime, "EnableCanRotate"),
                CreateAnimationEvent(openTime, "OpenRightHandDamageCollider"),
                CreateAnimationEvent(openTime, "OpenLeftHandDamageCollider"),
                CreateAnimationEvent(closeTime, "CloseRightHandDamageCollider"),
                CreateAnimationEvent(closeTime, "CloseLeftHandDamageCollider"),
                CreateAnimationEvent(rotateDisableTime, "DisableCanRotate"),
                CreateAnimationEvent(Mathf.Min(clipLength * 0.98f, clipLength - 0.01f), "ForceEndCurrentAction")
            };

            if (enablesCombo)
            {
                events.Add(CreateAnimationEvent(Mathf.Min(clipLength * 0.78f, clipLength - 0.01f), "EnableCanDoCombo"));
            }

            return events.OrderBy(evt => evt.time).ToArray();
        }

        private static AnimationEvent CreateAnimationEvent(float time, string functionName)
        {
            return new AnimationEvent
            {
                time = Mathf.Max(0f, time),
                functionName = functionName
            };
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
                Debug.LogWarning($"Monster33 boss builder could not locate animation clip settings for {clip.name}.");
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

        private static Monster33AIAssetSet CreateOrUpdateMonster33AIAssets()
        {
            var attackState = EnsureCopiedAsset<AttackState>(AttackStateTemplatePath, TargetAttackStatePath);
            var combatStanceState = EnsureCopiedAsset<CombatStanceState>(CombatStanceTemplatePath, TargetCombatStanceStatePath);
            var phase02CombatStanceState = EnsureCopiedAsset<CombatStanceState>(CombatStanceTemplatePath, TargetPhase02CombatStanceStatePath);
            var attack01 = EnsureCopiedAsset<AICharacterAttackAction>(Attack01TemplatePath, TargetAttack01Path);
            var attack01Combo = EnsureCopiedAsset<AICharacterAttackAction>(Attack01ComboTemplatePath, TargetAttack01ComboPath);
            var attack02 = EnsureCopiedAsset<AICharacterAttackAction>(Attack02TemplatePath, TargetAttack02Path);
            var attack03 = EnsureCopiedAsset<AICharacterAttackAction>(Attack02TemplatePath, TargetAttack03Path);

            if (attackState == null || combatStanceState == null || phase02CombatStanceState == null || attack01 == null || attack01Combo == null || attack02 == null || attack03 == null)
            {
                throw new InvalidOperationException("Monster33 boss builder could not create the Monster33 AI assets.");
            }

            attackState.name = "Monster33 Attack State";
            ConfigureAttackAction(
                attack01,
                "Monster33 Attack 01",
                "Attack_01",
                true,
                AttackType.LightAttack01,
                attack01Combo,
                60,
                0f,
                3.25f);
            ConfigureAttackAction(
                attack01Combo,
                "Monster33 Attack 01 Combo",
                "Attack_01_Combo",
                true,
                AttackType.LightAttack02,
                null,
                100,
                0f,
                3.5f);
            ConfigureAttackAction(
                attack02,
                "Monster33 Attack 02 Slash",
                "Attack_02",
                false,
                AttackType.ChargedAttack01,
                null,
                30,
                0.75f,
                5.5f);
            ConfigureAttackAction(
                attack03,
                "Monster33 Attack 03 Slash Alt",
                "Attack_03",
                false,
                AttackType.ChargedAttack02,
                null,
                30,
                1.1f,
                5.75f);

            ConfigureCombatStance(combatStanceState, attack01, attack02, attack03, false, 70, 5.75f);
            ConfigureCombatStance(phase02CombatStanceState, attack01, attack02, attack03, true, 100, 6.5f);
            EditorUtility.SetDirty(attackState);
            EditorUtility.SetDirty(combatStanceState);
            EditorUtility.SetDirty(phase02CombatStanceState);

            AssetDatabase.SaveAssets();

            return new Monster33AIAssetSet(attackState, combatStanceState, phase02CombatStanceState);
        }

        private static T EnsureCopiedAsset<T>(string templatePath, string targetPath) where T : UnityEngine.Object
        {
            EnsureFolderExistsForAsset(targetPath);

            var asset = AssetDatabase.LoadAssetAtPath<T>(targetPath);
            if (asset != null)
            {
                return asset;
            }

            if (!AssetDatabase.CopyAsset(templatePath, targetPath))
            {
                Debug.LogError($"Monster33 boss builder could not copy asset from {templatePath} to {targetPath}.");
                return null;
            }

            return AssetDatabase.LoadAssetAtPath<T>(targetPath);
        }

        private static void ConfigureAttackAction(
            AICharacterAttackAction attackAction,
            string assetName,
            string animationName,
            bool isParryable,
            AttackType attackType,
            AICharacterAttackAction comboAction,
            int attackWeight,
            float minimumDistance,
            float maximumDistance)
        {
            attackAction.name = assetName;

            var serializedObject = new SerializedObject(attackAction);
            serializedObject.FindProperty("attackAnimation").stringValue = animationName;
            serializedObject.FindProperty("isParryable").boolValue = isParryable;
            serializedObject.FindProperty("comboAction").objectReferenceValue = comboAction;
            serializedObject.FindProperty("attackType").enumValueIndex = (int)attackType;
            serializedObject.FindProperty("attackWeight").intValue = attackWeight;
            serializedObject.FindProperty("actionRecoveryTime").floatValue = attackType == AttackType.LightAttack01 || attackType == AttackType.LightAttack02 ? 1.35f : 1.6f;
            serializedObject.FindProperty("minimumAttackAngle").floatValue = -40f;
            serializedObject.FindProperty("maximumAttackAngle").floatValue = 40f;
            serializedObject.FindProperty("minimumAttackDistance").floatValue = minimumDistance;
            serializedObject.FindProperty("maximumAttackDistance").floatValue = maximumDistance;
            serializedObject.ApplyModifiedPropertiesWithoutUndo();
            EditorUtility.SetDirty(attackAction);
        }

        private static void ConfigureCombatStance(
            CombatStanceState combatStanceState,
            AICharacterAttackAction attack01,
            AICharacterAttackAction attack02,
            AICharacterAttackAction attack03,
            bool isPhase02,
            int comboChance,
            float maximumEngagementDistance)
        {
            combatStanceState.name = isPhase02
                ? "Monster33 Combat Stance State Phase02"
                : "Monster33 Combat Stance State";

            var serializedObject = new SerializedObject(combatStanceState);
            SetObjectArrayProperty(serializedObject.FindProperty("aiCharacterAttacks"), attack01, attack02, attack03);
            SetObjectArrayProperty(serializedObject.FindProperty("potentialAttacks"), attack01, attack02, attack03);
            serializedObject.FindProperty("chosenAttack").objectReferenceValue = null;
            serializedObject.FindProperty("previousAttack").objectReferenceValue = null;
            serializedObject.FindProperty("canPerformCombo").boolValue = true;
            serializedObject.FindProperty("percentageOfTimeWillPerformCombo").intValue = comboChance;
            serializedObject.FindProperty("onlyPerformComboIfInitialAttackHits").boolValue = true;
            serializedObject.FindProperty("maximumEngagementDistance").floatValue = maximumEngagementDistance;
            serializedObject.FindProperty("willCircleTarget").boolValue = false;
            serializedObject.FindProperty("canBlock").boolValue = false;
            serializedObject.ApplyModifiedPropertiesWithoutUndo();
            EditorUtility.SetDirty(combatStanceState);
        }

        private static void SetObjectArrayProperty(SerializedProperty arrayProperty, params UnityEngine.Object[] objects)
        {
            if (arrayProperty == null || !arrayProperty.isArray)
            {
                return;
            }

            arrayProperty.arraySize = objects.Length;
            for (int i = 0; i < objects.Length; i++)
            {
                arrayProperty.GetArrayElementAtIndex(i).objectReferenceValue = objects[i];
            }
        }

        private static void ApplyStateMotion(AnimatorController controller, string stateName, Motion motion)
        {
            var state = FindState(controller, stateName);
            if (state == null)
            {
                Debug.LogWarning($"Monster33 boss builder could not find animator state '{stateName}' inside {controller.name}.");
                return;
            }

            state.motion = motion;
        }

        private static void EnsureComboState(AnimatorController controller, AnimationClip comboClip)
        {
            var baseStateMachine = controller.layers[0].stateMachine;
            var actionOverdriveStateMachine = FindStateMachine(controller, "Action Overdrive") ?? baseStateMachine;

            RemoveStateIfExists(baseStateMachine, "Attack_01_Combo");

            var comboState = FindStateInStateMachine(actionOverdriveStateMachine, "Attack_01_Combo");
            if (comboState == null)
            {
                comboState = actionOverdriveStateMachine.AddState("Attack_01_Combo", new Vector3(870f, -190f, 0f));
            }

            comboState.motion = comboClip;
            comboState.speed = 1f;
            comboState.writeDefaultValues = true;

            foreach (var transition in comboState.transitions.ToArray())
            {
                comboState.RemoveTransition(transition);
            }

            var destinationState = FindStateInStateMachine(actionOverdriveStateMachine, "Empty")
                ?? FindState(controller, "Idle")
                ?? FindState(controller, "Locomotion");

            if (destinationState == null)
            {
                Debug.LogWarning("Monster33 boss builder could not find a destination state for Attack_01_Combo.");
                return;
            }

            var comboTransition = comboState.AddTransition(destinationState);
            comboTransition.hasExitTime = true;
            comboTransition.exitTime = 0.95f;
            comboTransition.duration = 0.08f;
            comboTransition.offset = 0f;
            comboTransition.interruptionSource = TransitionInterruptionSource.None;
        }

        private static AnimatorState FindState(AnimatorController controller, string stateName)
        {
            foreach (var layer in controller.layers)
            {
                var state = FindStateRecursive(layer.stateMachine, stateName);
                if (state != null)
                {
                    return state;
                }
            }

            return null;
        }

        private static AnimatorStateMachine FindStateMachine(AnimatorController controller, string stateMachineName)
        {
            foreach (var layer in controller.layers)
            {
                var stateMachine = FindStateMachineRecursive(layer.stateMachine, stateMachineName);
                if (stateMachine != null)
                {
                    return stateMachine;
                }
            }

            return null;
        }

        private static AnimatorState FindStateRecursive(AnimatorStateMachine stateMachine, string stateName)
        {
            foreach (var childState in stateMachine.states)
            {
                if (childState.state != null && childState.state.name == stateName)
                {
                    return childState.state;
                }
            }

            foreach (var childStateMachine in stateMachine.stateMachines)
            {
                var state = FindStateRecursive(childStateMachine.stateMachine, stateName);
                if (state != null)
                {
                    return state;
                }
            }

            return null;
        }

        private static AnimatorStateMachine FindStateMachineRecursive(AnimatorStateMachine stateMachine, string stateMachineName)
        {
            if (stateMachine != null && stateMachine.name == stateMachineName)
            {
                return stateMachine;
            }

            foreach (var childStateMachine in stateMachine.stateMachines)
            {
                var foundStateMachine = FindStateMachineRecursive(childStateMachine.stateMachine, stateMachineName);
                if (foundStateMachine != null)
                {
                    return foundStateMachine;
                }
            }

            return null;
        }

        private static AnimatorState FindStateInStateMachine(AnimatorStateMachine stateMachine, string stateName)
        {
            if (stateMachine == null)
            {
                return null;
            }

            foreach (var childState in stateMachine.states)
            {
                if (childState.state != null && childState.state.name == stateName)
                {
                    return childState.state;
                }
            }

            return null;
        }

        private static void RemoveStateIfExists(AnimatorStateMachine stateMachine, string stateName)
        {
            if (stateMachine == null)
            {
                return;
            }

            foreach (var childState in stateMachine.states.ToArray())
            {
                if (childState.state == null || childState.state.name != stateName)
                {
                    continue;
                }

                stateMachine.RemoveState(childState.state);
            }
        }

        private static void DeleteLegacyAnimatorController()
        {
            if (!AssetDatabase.LoadAssetAtPath<AnimatorController>(LegacyAnimatorControllerPath))
            {
                return;
            }

            AssetDatabase.DeleteAsset(LegacyAnimatorControllerPath);
            AssetDatabase.SaveAssets();
        }

        private static void EnsureFolderExistsForAsset(string assetPath)
        {
            string directory = System.IO.Path.GetDirectoryName(assetPath)?.Replace('\\', '/');
            if (string.IsNullOrEmpty(directory) || AssetDatabase.IsValidFolder(directory))
            {
                return;
            }

            string[] parts = directory.Split('/');
            string current = parts[0];

            for (int i = 1; i < parts.Length; i++)
            {
                string next = $"{current}/{parts[i]}";
                if (!AssetDatabase.IsValidFolder(next))
                {
                    AssetDatabase.CreateFolder(current, parts[i]);
                }

                current = next;
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
            var hurtbox = FindOrCreateDirectChild(hips, "Monster33_Main_Hurtbox", DamageableCharacterLayer);
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

        private static void EnsureWeaponDamageColliders(GameObject prefabRoot, Animator animator)
        {
            var combatManager = prefabRoot.GetComponent<AIMonster33CombatManager>();
            if (combatManager == null)
            {
                return;
            }

            var rightWeapon = FindTransformByName(animator.transform, "root_dupli_001.x");
            var leftWeapon = FindTransformByName(animator.transform, "root_dupli_002.x");

            if (rightWeapon == null || leftWeapon == null)
            {
                Debug.LogWarning("Monster33 rig does not expose root_dupli_001.x/root_dupli_002.x weapon bones, damage colliders were skipped.");
                return;
            }

            var rightCollider = EnsureWeaponDamageCollider(rightWeapon, "Monster33_Weapon_01_Hitbox");
            var leftCollider = EnsureWeaponDamageCollider(leftWeapon, "Monster33_Weapon_02_Hitbox");

            var serializedObject = new SerializedObject(combatManager);
            serializedObject.FindProperty("rightHandDamageCollider").objectReferenceValue = rightCollider;
            serializedObject.FindProperty("leftHandDamageCollider").objectReferenceValue = leftCollider;
            serializedObject.ApplyModifiedPropertiesWithoutUndo();
            EditorUtility.SetDirty(combatManager);
        }

        private static void EnsureWeaponConstraints(Animator animator)
        {
            var rightHand = FindBone(animator, HumanBodyBones.RightHand, "Hand_R", "RightHand", "hand_r", "hand.r");
            var leftHand = FindBone(animator, HumanBodyBones.LeftHand, "Hand_L", "LeftHand", "hand_l", "hand.l");
            var rightWeapon = FindTransformByName(animator.transform, "root_dupli_001.x");
            var leftWeapon = FindTransformByName(animator.transform, "root_dupli_002.x");

            if (rightHand == null || leftHand == null || rightWeapon == null || leftWeapon == null)
            {
                Debug.LogWarning("Monster33 builder could not constrain weapon roots to hands because one or more transforms are missing.");
                return;
            }

            ConfigureWeaponConstraint(rightWeapon, rightHand);
            ConfigureWeaponConstraint(leftWeapon, leftHand);
        }

        private static void ConfigureWeaponConstraint(Transform weaponRoot, Transform hand)
        {
            var constraint = weaponRoot.GetComponent<ParentConstraint>();
            if (constraint == null)
            {
                constraint = weaponRoot.gameObject.AddComponent<ParentConstraint>();
            }

            constraint.constraintActive = false;
            constraint.locked = false;
            constraint.translationAtRest = Vector3.zero;
            constraint.rotationAtRest = Vector3.zero;

            while (constraint.sourceCount > 0)
            {
                constraint.RemoveSource(0);
            }

            constraint.AddSource(new ConstraintSource
            {
                sourceTransform = hand,
                weight = 1f
            });

            weaponRoot.position = hand.position;
            weaponRoot.rotation = hand.rotation;

            constraint.SetTranslationOffset(0, Vector3.zero);
            constraint.SetRotationOffset(0, Vector3.zero);
            constraint.translationAxis = Axis.X | Axis.Y | Axis.Z;
            constraint.rotationAxis = Axis.X | Axis.Y | Axis.Z;
            constraint.constraintActive = false;
            constraint.locked = false;
        }

        private static void EnsureWeaponConstraintBootstrap(GameObject visualRoot)
        {
            if (visualRoot == null)
            {
                return;
            }

            var bootstrapType = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(assembly => assembly.GetTypes())
                .FirstOrDefault(type => type.Name == "Monster33WeaponConstraintBootstrap" && typeof(MonoBehaviour).IsAssignableFrom(type));

            if (bootstrapType == null)
            {
                Debug.LogWarning("Monster33 builder could not find Monster33WeaponConstraintBootstrap type.");
                return;
            }

            if (visualRoot.GetComponent(bootstrapType) == null)
            {
                visualRoot.AddComponent(bootstrapType);
            }
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

        private static ManualDamageCollider EnsureWeaponDamageCollider(Transform weaponBone, string hitboxName)
        {
            var hitbox = FindOrCreateDirectChild(weaponBone, hitboxName, DamageColliderLayer);
            var capsuleCollider = hitbox.GetComponent<CapsuleCollider>();
            if (capsuleCollider == null)
            {
                capsuleCollider = hitbox.gameObject.AddComponent<CapsuleCollider>();
            }

            capsuleCollider.isTrigger = true;
            capsuleCollider.radius = 0.28f;
            capsuleCollider.height = 1.35f;
            capsuleCollider.direction = 2;
            capsuleCollider.center = new Vector3(0f, 0f, -0.42f);

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

        private static UnityEngine.Object GetSerializedReference(UnityEngine.Object target, string propertyName)
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

        private static Transform FindTransformByName(Transform root, string transformName)
        {
            foreach (var transform in root.GetComponentsInChildren<Transform>(true))
            {
                if (transform.name == transformName)
                {
                    return transform;
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

        private static T LoadStateAsset<T>(string filter, string preferredGuid) where T : UnityEngine.Object
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

        private enum AttackDamageProfile
        {
            Light,
            Charged
        }

        private readonly struct Monster33AIAssetSet
        {
            public Monster33AIAssetSet(AttackState attackState, CombatStanceState combatStanceState, CombatStanceState phase02CombatStanceState)
            {
                AttackState = attackState;
                CombatStanceState = combatStanceState;
                Phase02CombatStanceState = phase02CombatStanceState;
            }

            public AttackState AttackState { get; }
            public CombatStanceState CombatStanceState { get; }
            public CombatStanceState Phase02CombatStanceState { get; }
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

