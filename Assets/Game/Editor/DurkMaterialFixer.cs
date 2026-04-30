using UnityEditor;
using UnityEngine;

namespace baodeag.EditorTools
{
    public static class DurkMaterialFixer
    {
        private const string TargetPrefabPath = GameAssetPaths.PrefabsRoot + "/Character/Boss/Durk_Boss_01.prefab";
        private const string BodyMaterialPath = GameAssetPaths.PolygonDungeonRoot + "/Materials/Dungeons_Material_Characters_01.mat";
        private const string ClubMaterialPath = GameAssetPaths.PolygonDungeonRoot + "/Materials/Dungeon_Material_01.mat";
        private const string AutoRunSessionKey = "DurkMaterialFixer.AutoRunAttempted.V2";

        static DurkMaterialFixer()
        {
            // Disabled autorun to avoid mutating editor state on startup.
        }

        [MenuItem("Tools/Fix/Apply Durk Boss Materials")]
        public static void ApplyDurkBossMaterials()
        {
            var bodyMaterial = AssetDatabase.LoadAssetAtPath<Material>(BodyMaterialPath);
            var clubMaterial = AssetDatabase.LoadAssetAtPath<Material>(ClubMaterialPath);

            if (bodyMaterial == null || clubMaterial == null)
            {
                Debug.LogError("Durk materials are missing.");
                return;
            }

            var prefabRoot = PrefabUtility.LoadPrefabContents(TargetPrefabPath);

            try
            {
                var renderers = prefabRoot.GetComponentsInChildren<Renderer>(true);
                var changes = 0;

                foreach (var renderer in renderers)
                {
                    if (renderer is ParticleSystemRenderer)
                    {
                        continue;
                    }

                    var material = IsClubRenderer(renderer) ? clubMaterial : bodyMaterial;
                    var sharedMaterials = renderer.sharedMaterials;

                    if (sharedMaterials == null || sharedMaterials.Length == 0)
                    {
                        renderer.sharedMaterials = new[] { material };
                        changes++;
                        continue;
                    }

                    var needsUpdate = false;
                    for (var i = 0; i < sharedMaterials.Length; i++)
                    {
                        if (sharedMaterials[i] != material)
                        {
                            sharedMaterials[i] = material;
                            needsUpdate = true;
                        }
                    }

                    if (!needsUpdate)
                    {
                        continue;
                    }

                    renderer.sharedMaterials = sharedMaterials;
                    changes++;
                }

                if (changes > 0)
                {
                    PrefabUtility.SaveAsPrefabAsset(prefabRoot, TargetPrefabPath);
                    AssetDatabase.ImportAsset(TargetPrefabPath, ImportAssetOptions.ForceUpdate);
                }

                Debug.Log($"Applied Durk boss materials to {changes} renderer(s).");
            }
            finally
            {
                PrefabUtility.UnloadPrefabContents(prefabRoot);
            }
        }

        private static bool IsClubRenderer(Renderer renderer)
        {
            var targetName = renderer.name.ToLowerInvariant();
            if (targetName.Contains("club") || targetName.Contains("trunk"))
            {
                return true;
            }

            if (renderer.TryGetComponent<MeshFilter>(out var meshFilter) &&
                meshFilter.sharedMesh != null)
            {
                var meshName = meshFilter.sharedMesh.name.ToLowerInvariant();
                return meshName.Contains("club") || meshName.Contains("trunk");
            }

            if (renderer is SkinnedMeshRenderer skinnedMeshRenderer &&
                skinnedMeshRenderer.sharedMesh != null)
            {
                var meshName = skinnedMeshRenderer.sharedMesh.name.ToLowerInvariant();
                return meshName.Contains("club") || meshName.Contains("trunk");
            }

            return false;
        }

        private static void TryAutoFixOnce()
        {
            if (SessionState.GetBool(AutoRunSessionKey, false))
            {
                return;
            }

            SessionState.SetBool(AutoRunSessionKey, true);
            ApplyDurkBossMaterials();
        }
    }
}
