using System;
using System.Linq;
using Unity.AI.Navigation;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace baodeag.Editor
{
    public static class GeneratedWorldCleanupUtility
    {
        private const string TargetScenePath = GameAssetPaths.ScenesRoot + "/World_02.unity";

        [MenuItem("Tools/Random Map/List World_02 Roots")]
        public static void ListWorld02Roots()
        {
            Scene scene = EditorSceneManager.OpenScene(TargetScenePath, OpenSceneMode.Single);
            
        }

        [MenuItem("Tools/Random Map/Cleanup World_02 Clone Roots")]
        public static void CleanupWorld02CloneRoots()
        {
            Scene scene = EditorSceneManager.OpenScene(TargetScenePath, OpenSceneMode.Single);
            CleanupOpenScene(scene);
        }

        [MenuItem("Tools/Random Map/Cleanup Current Generated World")]
        public static void CleanupCurrentGeneratedWorld()
        {
            CleanupOpenScene(SceneManager.GetActiveScene());
        }

        private static void CleanupOpenScene(Scene scene)
        {
            GameObject[] roots = scene.GetRootGameObjects();

            int deleted = 0;
            foreach (GameObject root in roots)
            {
                if (root == null || ShouldKeepRoot(root))
                    continue;

                
                UnityEngine.Object.DestroyImmediate(root);
                deleted++;
            }

            int cleanedNavMeshes = RemoveOldNavMeshSurfaces(scene);

            EditorSceneManager.MarkSceneDirty(scene);
            EditorSceneManager.SaveScene(scene);
            AssetDatabase.SaveAssets();
            
        }

        private static int RemoveOldNavMeshSurfaces(Scene scene)
        {
            int cleaned = 0;
            foreach (GameObject root in scene.GetRootGameObjects())
            {
                foreach (NavMeshSurface surface in root.GetComponentsInChildren<NavMeshSurface>(true))
                {
                    if (surface == null || IsGeneratedNavMeshSurface(surface))
                        continue;

                    surface.RemoveData();
                    UnityEngine.Object.DestroyImmediate(surface);
                    cleaned++;
                }
            }

            return cleaned;
        }

        private static bool IsGeneratedNavMeshSurface(NavMeshSurface surface)
        {
            Transform t = surface.transform;
            while (t != null)
            {
                if (t.name == "Floors" &&
                    t.parent != null &&
                    t.parent.name == "Structure" &&
                    t.root.name.StartsWith("[Generated]", StringComparison.OrdinalIgnoreCase))
                    return true;

                t = t.parent;
            }

            return false;
        }

        private static bool ShouldKeepRoot(GameObject root)
        {
            string name = root.name;
            if (name.StartsWith("[Generated]", StringComparison.OrdinalIgnoreCase))
                return true;

            if (root.GetComponent<RandomMapGenerator>() != null)
                return true;

            string lower = name.ToLowerInvariant();
            if (lower.Contains("manager") ||
                lower.Contains("eventsystem") ||
                lower.Contains("post processing") ||
                lower.Contains("directional light") ||
                lower.Contains("adaptive probe") ||
                lower.Contains("probevolume") ||
                lower.Contains("navmesh") ||
                lower.Contains("camera") ||
                lower.Contains("player"))
                return true;

            return false;
        }
    }
}
