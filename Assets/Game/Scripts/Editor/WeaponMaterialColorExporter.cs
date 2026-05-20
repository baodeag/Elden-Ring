using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using baodeag;
using UnityEditor;
using UnityEngine;

namespace baodeag.Editor
{
    public static class WeaponMaterialColorExporter
    {
        private const string WeaponsDataRoot = GameAssetPaths.DataRoot + "/Items/Weapons";
        private const string ExportRelativeRoot = GameAssetPaths.DocsRoot + "/WeaponPreviewExports";
        private const string MaterialManifestCsvFileName = "weapon_material_color_manifest.csv";
        private const string MaterialManifestJsonFileName = "weapon_material_color_manifest.json";
        private const string PromptCsvFileName = "weapon_render_prompts_material_locked.csv";
        private const string PromptJsonFileName = "weapon_render_prompts_material_locked.json";

        [MenuItem("Tools/Weapons/Export Weapon Material Colors + Render Prompts")]
        public static void ExportWeaponMaterialColorsAndPrompts()
        {
            string exportRoot = GetExportAbsoluteRoot();
            Directory.CreateDirectory(exportRoot);

            List<WeaponItem> weapons = LoadAllWeaponItems();
            List<WeaponMaterialRecord> materialRecords = new List<WeaponMaterialRecord>();
            List<WeaponPromptRecord> promptRecords = new List<WeaponPromptRecord>();

            foreach (WeaponItem weapon in weapons)
            {
                string assetPath = AssetDatabase.GetAssetPath(weapon);
                WeaponCategory category = ClassifyWeapon(weapon);
                List<MaterialPartRecord> parts = CollectMaterialParts(weapon);
                string prefabName = GetSafeObjectName(weapon.weaponModel);

                foreach (MaterialPartRecord part in parts)
                {
                    materialRecords.Add(new WeaponMaterialRecord
                    {
                        assetPath = assetPath,
                        displayName = weapon.itemName,
                        prefabName = prefabName,
                        category = category.ToString(),
                        role = part.role,
                        rendererName = part.rendererName,
                        materialName = part.materialName,
                        colorHex = part.colorHex,
                        colorName = part.colorName
                    });
                }

                promptRecords.Add(new WeaponPromptRecord
                {
                    assetPath = assetPath,
                    displayName = weapon.itemName,
                    prefabName = prefabName,
                    category = category.ToString(),
                    colorNotes = BuildColorNotes(category, parts),
                    prompt = BuildPrompt(weapon.itemName, category, BuildColorNotes(category, parts))
                });
            }

            WriteMaterialManifestCsv(materialRecords, Path.Combine(exportRoot, MaterialManifestCsvFileName));
            WriteMaterialManifestJson(materialRecords, Path.Combine(exportRoot, MaterialManifestJsonFileName));
            WritePromptCsv(promptRecords, Path.Combine(exportRoot, PromptCsvFileName));
            WritePromptJson(promptRecords, Path.Combine(exportRoot, PromptJsonFileName));

            AssetDatabase.Refresh();
            Debug.Log($"Exported {materialRecords.Count} material color rows and {promptRecords.Count} prompt rows to {ExportRelativeRoot}");
        }

        private static List<WeaponItem> LoadAllWeaponItems()
        {
            string[] guids = AssetDatabase.FindAssets("t:ScriptableObject", new[] { WeaponsDataRoot });
            List<WeaponItem> weapons = new List<WeaponItem>();

            foreach (string guid in guids)
            {
                string assetPath = AssetDatabase.GUIDToAssetPath(guid);
                WeaponItem weapon = AssetDatabase.LoadAssetAtPath<WeaponItem>(assetPath);
                if (weapon != null)
                    weapons.Add(weapon);
            }

            return weapons
                .OrderBy(weapon => AssetDatabase.GetAssetPath(weapon), StringComparer.OrdinalIgnoreCase)
                .ToList();
        }

        private static List<MaterialPartRecord> CollectMaterialParts(WeaponItem weapon)
        {
            List<MaterialPartRecord> records = new List<MaterialPartRecord>();
            if (weapon.weaponModel == null)
                return records;

            Renderer[] renderers = weapon.weaponModel.GetComponentsInChildren<Renderer>(true);
            foreach (Renderer renderer in renderers)
            {
                if (IsIgnorableRenderer(renderer))
                    continue;

                Mesh mesh = GetRendererMesh(renderer);
                Material[] materials = renderer.sharedMaterials;
                for (int i = 0; i < materials.Length; i++)
                {
                    Material material = materials[i];
                    if (material == null || IsIgnorableMaterial(material))
                        continue;

                    ColorAnalysis colorAnalysis = AnalyzeMaterialColor(material, mesh, i);
                    string role = GuessRole(weapon, renderer.gameObject.name, material.name);

                    records.Add(new MaterialPartRecord
                    {
                        role = role,
                        rendererName = renderer.gameObject.name,
                        materialName = material.name,
                        colorHex = ColorUtility.ToHtmlStringRGB(colorAnalysis.primaryColor),
                        colorName = colorAnalysis.description
                    });
                }
            }

            return records
                .GroupBy(record => $"{record.role}|{record.colorHex}|{record.materialName}", StringComparer.OrdinalIgnoreCase)
                .Select(group => group.First())
                .ToList();
        }

        private static Mesh GetRendererMesh(Renderer renderer)
        {
            if (renderer is SkinnedMeshRenderer skinnedMeshRenderer)
                return skinnedMeshRenderer.sharedMesh;

            MeshFilter meshFilter = renderer.GetComponent<MeshFilter>();
            return meshFilter != null ? meshFilter.sharedMesh : null;
        }

        private static bool IsIgnorableRenderer(Renderer renderer)
        {
            string rendererName = renderer.gameObject.name.ToLowerInvariant();
            return rendererName.Contains("trail") ||
                   rendererName.Contains("effect") ||
                   rendererName.Contains("particle");
        }

        private static bool IsIgnorableMaterial(Material material)
        {
            string materialName = material.name.ToLowerInvariant();
            return materialName.Contains("particle") ||
                   materialName.Contains("trail");
        }

        private static ColorAnalysis AnalyzeMaterialColor(Material material, Mesh mesh, int subMeshIndex)
        {
            Texture2D sourceTexture = GetMainTexture(material);
            if (sourceTexture != null)
            {
                List<Color> sampledColors = SampleTextureColors(sourceTexture, mesh, subMeshIndex);
                if (sampledColors.Count > 0)
                    return BuildPaletteDescription(sampledColors);
            }

            Color fallbackColor = ExtractRepresentativeFlatColor(material);
            return new ColorAnalysis
            {
                primaryColor = fallbackColor,
                description = DescribeColor(fallbackColor)
            };
        }

        private static Texture2D GetMainTexture(Material material)
        {
            string[] propertyNames = { "_BaseMap", "_MainTex", "_BaseColorMap", "_Texture" };
            foreach (string propertyName in propertyNames)
            {
                if (!material.HasProperty(propertyName))
                    continue;

                Texture texture = material.GetTexture(propertyName);
                if (texture is Texture2D texture2D)
                    return texture2D;
            }

            return null;
        }

        private static List<Color> SampleTextureColors(Texture2D sourceTexture, Mesh mesh, int subMeshIndex)
        {
            List<Color> sampledColors = new List<Color>();
            RenderTexture temporaryRenderTexture = RenderTexture.GetTemporary(64, 64, 0, RenderTextureFormat.ARGB32, RenderTextureReadWrite.Linear);
            Graphics.Blit(sourceTexture, temporaryRenderTexture);

            RenderTexture previous = RenderTexture.active;
            RenderTexture.active = temporaryRenderTexture;

            Texture2D readableTexture = new Texture2D(64, 64, TextureFormat.RGBA32, false, true);
            readableTexture.ReadPixels(new Rect(0, 0, 64, 64), 0, 0);
            readableTexture.Apply();

            RenderTexture.active = previous;
            RenderTexture.ReleaseTemporary(temporaryRenderTexture);

            try
            {
                if (mesh != null && mesh.uv != null && mesh.uv.Length > 0 && subMeshIndex < mesh.subMeshCount)
                {
                    SampleMeshUvColors(readableTexture, mesh, subMeshIndex, sampledColors);
                }

                if (sampledColors.Count == 0)
                {
                    Color[] pixels = readableTexture.GetPixels();
                    for (int i = 0; i < pixels.Length; i += 2)
                    {
                        Color color = pixels[i];
                        if (color.a < 0.2f)
                            continue;

                        if (color.maxColorComponent < 0.08f)
                            continue;

                        sampledColors.Add(color);
                    }
                }
            }
            finally
            {
                UnityEngine.Object.DestroyImmediate(readableTexture);
            }

            return sampledColors;
        }

        private static void SampleMeshUvColors(Texture2D readableTexture, Mesh mesh, int subMeshIndex, List<Color> sampledColors)
        {
            int[] triangles = mesh.GetTriangles(subMeshIndex);
            Vector2[] uvs = mesh.uv;
            if (triangles == null || triangles.Length < 3 || uvs == null || uvs.Length == 0)
                return;

            const int maxTriangleSamples = 256;
            int triangleCount = triangles.Length / 3;
            int triangleStep = Mathf.Max(1, triangleCount / maxTriangleSamples);

            for (int triangleIndex = 0; triangleIndex < triangles.Length; triangleIndex += 3 * triangleStep)
            {
                int index0 = triangles[triangleIndex];
                int index1 = triangles[Mathf.Min(triangleIndex + 1, triangles.Length - 1)];
                int index2 = triangles[Mathf.Min(triangleIndex + 2, triangles.Length - 1)];

                if (index0 >= uvs.Length || index1 >= uvs.Length || index2 >= uvs.Length)
                    continue;

                Vector2 uv0 = uvs[index0];
                Vector2 uv1 = uvs[index1];
                Vector2 uv2 = uvs[index2];

                SampleUv(readableTexture, uv0, sampledColors);
                SampleUv(readableTexture, uv1, sampledColors);
                SampleUv(readableTexture, uv2, sampledColors);
                SampleUv(readableTexture, (uv0 + uv1 + uv2) / 3f, sampledColors);
            }
        }

        private static void SampleUv(Texture2D texture, Vector2 uv, List<Color> sampledColors)
        {
            float wrappedU = uv.x - Mathf.Floor(uv.x);
            float wrappedV = uv.y - Mathf.Floor(uv.y);

            int x = Mathf.Clamp(Mathf.RoundToInt(wrappedU * (texture.width - 1)), 0, texture.width - 1);
            int y = Mathf.Clamp(Mathf.RoundToInt(wrappedV * (texture.height - 1)), 0, texture.height - 1);
            Color color = texture.GetPixel(x, y);

            if (color.a < 0.2f)
                return;

            if (color.maxColorComponent < 0.08f)
                return;

            sampledColors.Add(color);
        }

        private static ColorAnalysis BuildPaletteDescription(List<Color> sampledColors)
        {
            Dictionary<string, List<Color>> buckets = new Dictionary<string, List<Color>>(StringComparer.OrdinalIgnoreCase);
            foreach (Color color in sampledColors)
            {
                string colorName = DescribeColor(color);
                if (!buckets.TryGetValue(colorName, out List<Color> values))
                {
                    values = new List<Color>();
                    buckets[colorName] = values;
                }

                values.Add(color);
            }

            List<KeyValuePair<string, List<Color>>> rankedBuckets = buckets
                .OrderByDescending(pair => pair.Value.Count)
                .ToList();

            Color primaryColor = rankedBuckets[0].Value
                .Aggregate(Color.black, (accumulator, color) => accumulator + color) / rankedBuckets[0].Value.Count;

            List<string> palette = rankedBuckets
                .Take(3)
                .Select(pair => pair.Key)
                .ToList();

            return new ColorAnalysis
            {
                primaryColor = primaryColor,
                description = palette.Count <= 1
                    ? palette[0]
                    : "palette of " + string.Join(", ", palette)
            };
        }

        private static Color ExtractRepresentativeFlatColor(Material material)
        {
            string[] propertyNames = { "_BaseColor", "_Color", "_TintColor", "_EmissionColor" };
            foreach (string propertyName in propertyNames)
            {
                if (material.HasProperty(propertyName))
                {
                    Color color = material.GetColor(propertyName);
                    if (color.a > 0.001f)
                        return color;
                }
            }

            return Color.gray;
        }

        private static string GuessRole(WeaponItem weapon, string rendererName, string materialName)
        {
            string text = $"{weapon.itemName} {rendererName} {materialName}".ToLowerInvariant();

            if (text.Contains("blade"))
                return "blade";
            if (text.Contains("guard") || text.Contains("cross"))
                return "guard";
            if (text.Contains("grip") || text.Contains("handle"))
                return "grip";
            if (text.Contains("pommel") || text.Contains("end"))
                return "pommel";
            if (text.Contains("shaft"))
                return "shaft";
            if (text.Contains("head"))
                return "head";
            if (text.Contains("wrap"))
                return "wrap";
            if (text.Contains("shield"))
                return "shield face";
            if (text.Contains("rim"))
                return "rim";
            if (text.Contains("string"))
                return "string";
            if (text.Contains("bow"))
                return "limb";
            if (text.Contains("gem") || text.Contains("crystal"))
                return "accent";

            WeaponCategory category = ClassifyWeapon(weapon);
            switch (category)
            {
                case WeaponCategory.Shield:
                    return "shield face";
                case WeaponCategory.Bow:
                    return "limb";
                case WeaponCategory.Focus:
                    return "body";
                case WeaponCategory.Axe:
                case WeaponCategory.Blunt:
                    return "head";
                case WeaponCategory.Polearm:
                case WeaponCategory.Scythe:
                    return "shaft";
                default:
                    return "blade";
            }
        }

        private static string BuildColorNotes(WeaponCategory category, List<MaterialPartRecord> parts)
        {
            if (parts.Count == 0)
                return "Match the exact color blocking visible in the prefab reference.";

            if (parts.Select(part => part.role).Distinct(StringComparer.OrdinalIgnoreCase).Count() == 1)
            {
                string paletteDescription = parts
                    .Select(part => part.colorName)
                    .Distinct(StringComparer.OrdinalIgnoreCase)
                    .FirstOrDefault();

                if (!string.IsNullOrWhiteSpace(paletteDescription))
                    return $"Overall palette reads as {paletteDescription}. Keep these same flat color families in the same visible places as the prefab reference.";
            }

            List<MaterialPartRecord> orderedParts = parts
                .OrderBy(part => GetRolePriority(category, part.role))
                .ThenBy(part => part.role, StringComparer.OrdinalIgnoreCase)
                .ToList();

            List<string> phrases = new List<string>();
            foreach (MaterialPartRecord part in orderedParts)
                phrases.Add($"{part.role} is {part.colorName}");

            return "Color map: " + string.Join("; ", phrases) + ".";
        }

        private static int GetRolePriority(WeaponCategory category, string role)
        {
            switch (category)
            {
                case WeaponCategory.Sword:
                case WeaponCategory.Greatsword:
                case WeaponCategory.Rapier:
                case WeaponCategory.Dagger:
                    if (role == "blade") return 0;
                    if (role == "guard") return 1;
                    if (role == "grip" || role == "wrap") return 2;
                    if (role == "pommel") return 3;
                    break;
                case WeaponCategory.Axe:
                case WeaponCategory.Blunt:
                    if (role == "head") return 0;
                    if (role == "shaft") return 1;
                    if (role == "grip" || role == "wrap") return 2;
                    if (role == "pommel") return 3;
                    break;
                case WeaponCategory.Polearm:
                case WeaponCategory.Scythe:
                    if (role == "head" || role == "blade") return 0;
                    if (role == "shaft") return 1;
                    if (role == "wrap") return 2;
                    if (role == "pommel") return 3;
                    break;
                case WeaponCategory.Shield:
                    if (role == "shield face") return 0;
                    if (role == "rim") return 1;
                    if (role == "accent") return 2;
                    break;
                case WeaponCategory.Bow:
                    if (role == "limb") return 0;
                    if (role == "grip") return 1;
                    if (role == "string") return 2;
                    break;
            }

            return 10;
        }

        private static string BuildPrompt(string displayName, WeaponCategory category, string colorNotes)
        {
            return
                $"Create a single game inventory icon PNG of '{displayName}'. " +
                "Use the attached prefab preview as a strict reference. " +
                "Recreate the exact same weapon shape and silhouette as closely as possible. " +
                "Match the exact silhouette, proportions, wrapping placement, head size, handle length, guard shape, tip shape, and color placement from the prefab reference. " +
                colorNotes + " " +
                "Keep the weapon in an extremely simple low-poly style, with very few large planes, very little micro detail, minimal surface noise, and broad flat color areas only. " +
                "One isolated weapon only, transparent background, no backdrop, no frame, no environment, no character, no hand, centered in the canvas, full object visible, diagonal layout like the sample icon. " +
                "Do not redesign the weapon. Do not invent a new color palette. Do not add realism, painterly brushwork, glow, extra ornaments, text, or watermark.";
        }

        private static WeaponCategory ClassifyWeapon(WeaponItem weapon)
        {
            string text = $"{weapon.itemName} {weapon.weaponModelType} {weapon.weaponClass} {GetSafeObjectName(weapon.weaponModel)}".ToLowerInvariant();
            if (text.Contains("shield"))
                return WeaponCategory.Shield;
            if (text.Contains("bow"))
                return WeaponCategory.Bow;
            if (text.Contains("staff") || text.Contains("talisman") || text.Contains("charm"))
                return WeaponCategory.Focus;
            if (text.Contains("scythe"))
                return WeaponCategory.Scythe;
            if (text.Contains("halberd") || text.Contains("glaive") || text.Contains("spear") || text.Contains("pike") || text.Contains("lance") || text.Contains("banner"))
                return WeaponCategory.Polearm;
            if (text.Contains("hammer") || text.Contains("mace") || text.Contains("club"))
                return WeaponCategory.Blunt;
            if (text.Contains("axe") || text.Contains("hatchet"))
                return WeaponCategory.Axe;
            if (text.Contains("dagger") || text.Contains("shiv") || text.Contains("knife"))
                return WeaponCategory.Dagger;
            if (text.Contains("rapier"))
                return WeaponCategory.Rapier;
            if (text.Contains("greatsword"))
                return WeaponCategory.Greatsword;
            return WeaponCategory.Sword;
        }

        private static string DescribeColor(Color color)
        {
            Color.RGBToHSV(color, out float hue, out float saturation, out float value);

            if (saturation < 0.08f)
            {
                if (value < 0.15f) return "near-black";
                if (value < 0.35f) return "dark gray";
                if (value < 0.75f) return "gray";
                return "off-white";
            }

            string baseColor;
            float hueDegrees = hue * 360f;
            if (hueDegrees < 15f || hueDegrees >= 330f) baseColor = "red";
            else if (hueDegrees < 35f) baseColor = "orange";
            else if (hueDegrees < 50f) baseColor = "gold";
            else if (hueDegrees < 70f) baseColor = "yellow";
            else if (hueDegrees < 95f) baseColor = "olive";
            else if (hueDegrees < 145f) baseColor = "green";
            else if (hueDegrees < 180f) baseColor = "teal";
            else if (hueDegrees < 210f) baseColor = "cyan";
            else if (hueDegrees < 250f) baseColor = "blue";
            else if (hueDegrees < 285f) baseColor = "purple";
            else baseColor = "magenta";

            if ((baseColor == "orange" || baseColor == "gold" || baseColor == "yellow") && value > 0.55f && saturation < 0.45f)
                return "beige-gold";
            if ((baseColor == "orange" || baseColor == "red") && value < 0.45f && saturation < 0.5f)
                return "brown";
            if (baseColor == "red" && value > 0.45f)
                return "red-orange";

            if (value < 0.25f) return "dark " + baseColor;
            if (value > 0.75f) return "pale " + baseColor;
            if (saturation < 0.35f) return "muted " + baseColor;
            return baseColor;
        }

        private static string GetSafeObjectName(UnityEngine.Object unityObject)
        {
            return unityObject == null ? string.Empty : unityObject.name;
        }

        private static void WriteMaterialManifestCsv(List<WeaponMaterialRecord> records, string outputPath)
        {
            List<string> lines = new List<string>
            {
                "assetPath,displayName,prefabName,category,role,rendererName,materialName,colorHex,colorName"
            };

            foreach (WeaponMaterialRecord record in records)
            {
                lines.Add(string.Join(",",
                    EscapeCsv(record.assetPath),
                    EscapeCsv(record.displayName),
                    EscapeCsv(record.prefabName),
                    EscapeCsv(record.category),
                    EscapeCsv(record.role),
                    EscapeCsv(record.rendererName),
                    EscapeCsv(record.materialName),
                    EscapeCsv(record.colorHex),
                    EscapeCsv(record.colorName)));
            }

            File.WriteAllLines(outputPath, lines, Encoding.UTF8);
        }

        private static void WriteMaterialManifestJson(List<WeaponMaterialRecord> records, string outputPath)
        {
            File.WriteAllText(outputPath, JsonUtility.ToJson(new WeaponMaterialRecordCollection { records = records }, true), Encoding.UTF8);
        }

        private static void WritePromptCsv(List<WeaponPromptRecord> records, string outputPath)
        {
            List<string> lines = new List<string>
            {
                "assetPath,displayName,prefabName,category,colorNotes,prompt"
            };

            foreach (WeaponPromptRecord record in records)
            {
                lines.Add(string.Join(",",
                    EscapeCsv(record.assetPath),
                    EscapeCsv(record.displayName),
                    EscapeCsv(record.prefabName),
                    EscapeCsv(record.category),
                    EscapeCsv(record.colorNotes),
                    EscapeCsv(record.prompt)));
            }

            File.WriteAllLines(outputPath, lines, Encoding.UTF8);
        }

        private static void WritePromptJson(List<WeaponPromptRecord> records, string outputPath)
        {
            File.WriteAllText(outputPath, JsonUtility.ToJson(new WeaponPromptRecordCollection { records = records }, true), Encoding.UTF8);
        }

        private static string EscapeCsv(string value)
        {
            string escaped = (value ?? string.Empty).Replace("\"", "\"\"");
            return $"\"{escaped}\"";
        }

        private static string GetExportAbsoluteRoot()
        {
            string relativePath = ExportRelativeRoot.Replace('/', Path.DirectorySeparatorChar);
            return Path.Combine(Directory.GetCurrentDirectory(), relativePath);
        }

        [Serializable]
        private class WeaponMaterialRecordCollection
        {
            public List<WeaponMaterialRecord> records = new List<WeaponMaterialRecord>();
        }

        [Serializable]
        private class WeaponPromptRecordCollection
        {
            public List<WeaponPromptRecord> records = new List<WeaponPromptRecord>();
        }

        [Serializable]
        private class WeaponMaterialRecord
        {
            public string assetPath;
            public string displayName;
            public string prefabName;
            public string category;
            public string role;
            public string rendererName;
            public string materialName;
            public string colorHex;
            public string colorName;
        }

        [Serializable]
        private class WeaponPromptRecord
        {
            public string assetPath;
            public string displayName;
            public string prefabName;
            public string category;
            public string colorNotes;
            public string prompt;
        }

        private class MaterialPartRecord
        {
            public string role;
            public string rendererName;
            public string materialName;
            public string colorHex;
            public string colorName;
        }

        private struct ColorAnalysis
        {
            public Color primaryColor;
            public string description;
        }

        private enum WeaponCategory
        {
            Sword,
            Greatsword,
            Rapier,
            Dagger,
            Axe,
            Blunt,
            Polearm,
            Scythe,
            Shield,
            Bow,
            Focus
        }
    }
}
