using Unity.Netcode;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace baodeag.Editor
{
    public static class WorldMapTransitionSetupUtility
    {
        [MenuItem("Tools/Random Map/Create World_01 To World_02 Connector")]
        public static void CreateWorld01ToWorld02Connector()
        {
            Scene scene = EditorSceneManager.OpenScene("Assets/Scenes/World_01.unity", OpenSceneMode.Single);
            CreateConnectorInScene(scene);
            EditorSceneManager.SaveScene(scene);
            AssetDatabase.SaveAssets();
        }

        [MenuItem("Tools/Random Map/Create Connector In Current Scene")]
        public static void CreateConnectorInCurrentScene()
        {
            CreateConnectorInScene(SceneManager.GetActiveScene());
        }

        private static void CreateConnectorInScene(Scene scene)
        {
            GameObject existing = GameObject.Find("Travel To World_02");
            if (existing != null)
            {
                Selection.activeGameObject = existing;
                Debug.Log("[WorldMapTransitionSetup] Connector already exists: Travel To World_02");
                return;
            }

            GameObject connector = new GameObject("Travel To World_02");
            SceneManager.MoveGameObjectToScene(connector, scene);
            connector.transform.position = new Vector3(0f, 1f, 35f);

            BoxCollider collider = connector.AddComponent<BoxCollider>();
            collider.isTrigger = true;
            collider.size = new Vector3(4f, 3f, 4f);

            connector.AddComponent<NetworkObject>();
            WorldMapTransitionInteractable transition = connector.AddComponent<WorldMapTransitionInteractable>();
            transition.interactableText = "Travel to Map 2";

            EditorSceneManager.MarkSceneDirty(scene);
            Selection.activeGameObject = connector;
            Debug.Log("[WorldMapTransitionSetup] Created Travel To World_02 connector at (0, 1, 35). Move it to the desired door/exit if needed.");
        }
    }
}
