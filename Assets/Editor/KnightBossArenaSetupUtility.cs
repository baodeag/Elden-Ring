using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace baodeag.EditorTools
{
    public static class KnightBossArenaSetupUtility
    {
        const string KnightBossSpawnerPrefabPath = "Assets/Prefabs/Boss/Knight Boss Spawner.prefab";
        const string BossFightTriggerPrefabPath = "Assets/Prefabs/Boss/Boss Fight Trigger.prefab";
        const string FogWallPrefabPath = "Assets/Prefabs/Interactable Objects/Fog Wall Interactable.prefab";

        const float DefaultTriggerForwardOffset = -8f;
        const float DefaultFogWallForwardOffset = -6f;

        [MenuItem("Tools/Bosses/Setup Knight Boss Arena")]
        public static void SetupKnightBossArena()
        {
            Transform anchor = Selection.activeTransform;
            Vector3 origin = anchor != null ? anchor.position : Vector3.zero;
            Quaternion rotation = anchor != null ? anchor.rotation : Quaternion.identity;

            GameObject spawnerPrefab = AssetDatabase.LoadAssetAtPath<GameObject>(KnightBossSpawnerPrefabPath);
            GameObject triggerPrefab = AssetDatabase.LoadAssetAtPath<GameObject>(BossFightTriggerPrefabPath);
            GameObject fogWallPrefab = AssetDatabase.LoadAssetAtPath<GameObject>(FogWallPrefabPath);

            if (spawnerPrefab == null || triggerPrefab == null || fogWallPrefab == null)
            {
                Debug.LogError("Knight boss arena setup failed because one or more required prefabs could not be found.");
                return;
            }

            GameObject arenaRoot = new GameObject("Knight Boss Arena Setup");
            Undo.RegisterCreatedObjectUndo(arenaRoot, "Create Knight Boss Arena");
            arenaRoot.transform.position = origin;
            arenaRoot.transform.rotation = rotation;

            GameObject spawnerInstance = (GameObject)PrefabUtility.InstantiatePrefab(spawnerPrefab);
            GameObject triggerInstance = (GameObject)PrefabUtility.InstantiatePrefab(triggerPrefab);
            GameObject fogWallInstance = (GameObject)PrefabUtility.InstantiatePrefab(fogWallPrefab);

            if (spawnerInstance == null || triggerInstance == null || fogWallInstance == null)
            {
                Debug.LogError("Knight boss arena setup failed while instantiating prefabs.");
                Object.DestroyImmediate(arenaRoot);
                return;
            }

            Undo.RegisterCreatedObjectUndo(spawnerInstance, "Create Knight Boss Spawner");
            Undo.RegisterCreatedObjectUndo(triggerInstance, "Create Knight Boss Trigger");
            Undo.RegisterCreatedObjectUndo(fogWallInstance, "Create Knight Boss Fog Wall");

            spawnerInstance.name = "Knight Boss Spawner";
            triggerInstance.name = "Knight Boss Trigger";
            fogWallInstance.name = "Knight Boss Fog Wall";

            spawnerInstance.transform.SetParent(arenaRoot.transform);
            triggerInstance.transform.SetParent(arenaRoot.transform);
            fogWallInstance.transform.SetParent(arenaRoot.transform);

            spawnerInstance.transform.position = origin;
            spawnerInstance.transform.rotation = rotation;

            Vector3 forward = rotation * Vector3.forward;

            triggerInstance.transform.position = origin + forward * DefaultTriggerForwardOffset;
            triggerInstance.transform.rotation = rotation;

            fogWallInstance.transform.position = origin + forward * DefaultFogWallForwardOffset;
            fogWallInstance.transform.rotation = rotation;

            BoxCollider triggerCollider = triggerInstance.GetComponent<BoxCollider>();
            if (triggerCollider != null)
            {
                triggerCollider.isTrigger = true;
                triggerCollider.size = new Vector3(14f, 4f, 8f);
                triggerCollider.center = new Vector3(0f, 1.5f, 0f);
            }

            Selection.activeGameObject = arenaRoot;
            EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());

            Debug.Log("Knight boss arena setup created. Review trigger/fog wall positions in scene, then save the scene.");
        }
    }
}
