using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace baodeag.Editor
{
    public static class PolygonArmorPreviewExporter
    {
        private const string PlayerPrefabPath = "Assets/Game/Prefabs/Character/Player.prefab";
        private const string ArmorRoot = "Assets/Game/Data/Items/Armor/Polygon Armor Collection";
        private const int PreviewSize = 256;
        private const int Padding = 16;
        private const int Columns = 4;

        [MenuItem("Tools/Export Polygon Armor Previews")]
        public static void ExportMenu()
        {
            ExportAll();
        }

        public static void ExportAll()
        {
            var exportRoot = Path.Combine(Application.dataPath, "..", "Temp", "ArmorPreviews");
            Directory.CreateDirectory(exportRoot);

            ExportCategory("Body", exportRoot);
            ExportCategory("Head", exportRoot);
            ExportCategory("Hands", exportRoot);
            ExportCategory("Legs", exportRoot);

            AssetDatabase.Refresh();
            Debug.Log($"Polygon armor previews exported to: {exportRoot}");
        }

        private static void ExportCategory(string category, string exportRoot)
        {
            var categoryPath = Path.Combine(ArmorRoot, category).Replace("\\", "/");
            var guids = AssetDatabase.FindAssets("t:ArmorItem", new[] { categoryPath });
            if (guids.Length == 0)
                return;

            var outputPath = Path.Combine(exportRoot, $"{category}.png");
            var rows = Mathf.CeilToInt(guids.Length / (float)Columns);
            var sheet = new Texture2D(
                Columns * (PreviewSize + Padding) + Padding,
                rows * (PreviewSize + Padding) + Padding,
                TextureFormat.RGBA32,
                false);

            var background = new Color(0.11f, 0.12f, 0.14f, 1f);
            var pixels = Enumerable.Repeat(background, sheet.width * sheet.height).ToArray();
            sheet.SetPixels(pixels);

            for (var i = 0; i < guids.Length; i++)
            {
                var assetPath = AssetDatabase.GUIDToAssetPath(guids[i]);
                var armorItem = AssetDatabase.LoadAssetAtPath<ArmorItem>(assetPath);
                if (armorItem == null)
                    continue;

                var preview = RenderArmorPreview(armorItem);
                if (preview == null)
                    continue;

                var col = i % Columns;
                var row = rows - 1 - (i / Columns);
                var x = Padding + col * (PreviewSize + Padding);
                var y = Padding + row * (PreviewSize + Padding);
                sheet.SetPixels(x, y, preview.width, preview.height, preview.GetPixels());
            }

            sheet.Apply();
            File.WriteAllBytes(outputPath, sheet.EncodeToPNG());
        }

        private static Texture2D RenderArmorPreview(ArmorItem armorItem)
        {
            var playerPrefab = AssetDatabase.LoadAssetAtPath<GameObject>(PlayerPrefabPath);
            if (playerPrefab == null)
                return null;

            var instance = PrefabUtility.InstantiatePrefab(playerPrefab) as GameObject;
            if (instance == null)
                return null;

            try
            {
                instance.hideFlags = HideFlags.HideAndDontSave;

                foreach (var renderer in instance.GetComponentsInChildren<Renderer>(true))
                {
                    renderer.enabled = false;
                }

                foreach (var model in armorItem.equipmentModels)
                {
                    if (model == null || string.IsNullOrWhiteSpace(model.maleEquipmentName))
                        continue;

                    var target = instance.GetComponentsInChildren<Transform>(true)
                        .FirstOrDefault(t => t.name == model.maleEquipmentName);

                    if (target == null)
                        continue;

                    ActivateHierarchy(target);
                    foreach (var renderer in target.GetComponentsInChildren<Renderer>(true))
                    {
                        renderer.enabled = true;
                    }
                }

                var activeRenderers = instance.GetComponentsInChildren<Renderer>(true)
                    .Where(r => r.enabled && r.gameObject.activeInHierarchy)
                    .ToArray();

                if (activeRenderers.Length == 0)
                    return null;

                var bounds = activeRenderers[0].bounds;
                for (var i = 1; i < activeRenderers.Length; i++)
                {
                    bounds.Encapsulate(activeRenderers[i].bounds);
                }

                var previewUtility = new PreviewRenderUtility();
                previewUtility.cameraFieldOfView = 25f;
                previewUtility.lights[0].intensity = 1.2f;
                previewUtility.lights[0].transform.rotation = Quaternion.Euler(30f, 30f, 0f);
                previewUtility.lights[1].intensity = 1.1f;
                previewUtility.lights[1].transform.rotation = Quaternion.Euler(340f, 218f, 177f);
                previewUtility.ambientColor = new Color(0.5f, 0.5f, 0.5f, 1f);

                previewUtility.AddSingleGO(instance);

                var center = bounds.center;
                var extents = bounds.extents.magnitude;
                var distance = Mathf.Max(2f, extents * 2.6f);
                previewUtility.camera.transform.position = center + new Vector3(0f, extents * 0.2f, -distance);
                previewUtility.camera.transform.LookAt(center + new Vector3(0f, extents * 0.1f, 0f));
                previewUtility.camera.nearClipPlane = 0.01f;
                previewUtility.camera.farClipPlane = 100f;

                previewUtility.BeginPreview(new Rect(0, 0, PreviewSize, PreviewSize), GUIStyle.none);
                previewUtility.camera.Render();
                var result = previewUtility.EndPreview();
                if (result is not RenderTexture renderTexture)
                {
                    previewUtility.Cleanup();
                    return null;
                }

                var previousActive = RenderTexture.active;
                RenderTexture.active = renderTexture;

                try
                {
                    var texture = new Texture2D(PreviewSize, PreviewSize, TextureFormat.RGBA32, false);
                    texture.ReadPixels(new Rect(0, 0, PreviewSize, PreviewSize), 0, 0);
                    texture.Apply();
                    return texture;
                }
                finally
                {
                    RenderTexture.active = previousActive;
                    previewUtility.Cleanup();
                }
            }
            finally
            {
                Object.DestroyImmediate(instance);
            }
        }

        private static void ActivateHierarchy(Transform target)
        {
            var current = target;
            while (current != null)
            {
                current.gameObject.SetActive(true);
                current = current.parent;
            }
        }
    }
}
