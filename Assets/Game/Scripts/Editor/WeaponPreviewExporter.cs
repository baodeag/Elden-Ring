using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using baodeag;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace baodeag.Editor
{
    public static class WeaponPreviewExporter
    {
        private const string WeaponsDataRoot = GameAssetPaths.DataRoot + "/Items/Weapons";
        private const string ExportRelativeRoot = GameAssetPaths.DocsRoot + "/WeaponPreviewExports";
        private const string ManifestCsvFileName = "weapon_preview_manifest.csv";
        private const string ManifestJsonFileName = "weapon_preview_manifest.json";
        private const string OverrideCsvFileName = "weapon_name_overrides.csv";
        private const string PreviewFolderName = "Previews";
        private const int PreviewSize = 512;

        [MenuItem("Tools/Weapons/Export Weapon Previews + Manifest")]
        public static void ExportWeaponPreviewsAndManifest()
        {
            string exportRoot = GetExportAbsoluteRoot();
            string previewRoot = Path.Combine(exportRoot, PreviewFolderName);

            Directory.CreateDirectory(exportRoot);
            Directory.CreateDirectory(previewRoot);

            List<WeaponItem> weapons = LoadAllWeaponItems();
            List<WeaponPreviewRecord> records = new List<WeaponPreviewRecord>(weapons.Count);

            for (int i = 0; i < weapons.Count; i++)
            {
                WeaponItem weapon = weapons[i];
                string assetPath = AssetDatabase.GetAssetPath(weapon);
                string baseName = Path.GetFileNameWithoutExtension(assetPath);
                string previewFileName = SanitizeFileName(baseName) + ".png";
                string previewAbsolutePath = Path.Combine(previewRoot, previewFileName);
                string previewRelativePath = ExportRelativeRoot + "/" + PreviewFolderName + "/" + previewFileName;

                WeaponPreviewRecord record = BuildRecord(weapon, assetPath, previewRelativePath);
                records.Add(record);

                if (weapon.weaponModel != null)
                    ExportPreviewTexture(weapon.weaponModel, previewAbsolutePath);
            }

            WriteManifestCsv(records, Path.Combine(exportRoot, ManifestCsvFileName));
            WriteManifestJson(records, Path.Combine(exportRoot, ManifestJsonFileName));
            EnsureOverrideCsv(records, Path.Combine(exportRoot, OverrideCsvFileName));

            AssetDatabase.Refresh();
            
        }

        [MenuItem("Tools/Weapons/Apply Weapon Names From Override CSV")]
        public static void ApplyWeaponNamesFromOverrideCsv()
        {
            string overrideCsvPath = Path.Combine(GetExportAbsoluteRoot(), OverrideCsvFileName);

            if (!File.Exists(overrideCsvPath))
            {
                
                return;
            }

            Dictionary<string, string> overrides = ReadOverrideCsv(overrideCsvPath);
            int renameCount = 0;

            foreach (KeyValuePair<string, string> entry in overrides)
            {
                WeaponItem weapon = AssetDatabase.LoadAssetAtPath<WeaponItem>(entry.Key);

                if (weapon == null)
                    continue;

                string desiredName = entry.Value?.Trim();
                if (string.IsNullOrWhiteSpace(desiredName))
                    continue;

                if (ApplyWeaponIdentity(weapon, desiredName))
                    renameCount++;
            }

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            
        }

        [MenuItem("Tools/Weapons/Apply Auto Suggested Weapon Names")]
        public static void ApplyAutoSuggestedWeaponNames()
        {
            List<WeaponItem> weapons = LoadAllWeaponItems();
            int renameCount = 0;

            foreach (WeaponItem weapon in weapons)
            {
                string assetPath = AssetDatabase.GetAssetPath(weapon);
                WeaponPreviewRecord record = BuildRecord(weapon, assetPath, string.Empty);

                if (string.IsNullOrWhiteSpace(record.suggestedDisplayName))
                    continue;

                if (ApplyWeaponIdentity(weapon, record.suggestedDisplayName))
                    renameCount++;
            }

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            
        }

        [MenuItem("Tools/Weapons/Sync Weapon Asset File Names")]
        public static void SyncWeaponAssetFileNames()
        {
            List<WeaponItem> weapons = LoadAllWeaponItems();
            int renameCount = 0;

            foreach (WeaponItem weapon in weapons)
            {
                if (!RenameWeaponAssetToMatchObjectName(weapon))
                    continue;

                renameCount++;
            }

            AssetDatabase.Refresh();
            
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

        private static WeaponPreviewRecord BuildRecord(WeaponItem weapon, string assetPath, string previewRelativePath)
        {
            string modelPath = weapon.weaponModel != null ? AssetDatabase.GetAssetPath(weapon.weaponModel) : string.Empty;
            string prefabName = string.IsNullOrWhiteSpace(modelPath) ? string.Empty : Path.GetFileNameWithoutExtension(modelPath);
            List<string> meshNames = CollectMeshNames(weapon.weaponModel);
            WeaponNamingSuggestion suggestion = SuggestDisplayName(weapon, assetPath, prefabName, meshNames);

            return new WeaponPreviewRecord
            {
                assetPath = assetPath,
                itemType = weapon.GetType().Name,
                currentObjectName = weapon.name,
                currentDisplayName = weapon.itemName,
                suggestedDisplayName = suggestion.displayName,
                namingNotes = suggestion.notes,
                weaponClass = weapon.weaponClass.ToString(),
                weaponModelType = weapon.weaponModelType.ToString(),
                modelPath = modelPath,
                prefabName = prefabName,
                meshNames = string.Join(" | ", meshNames),
                previewPath = previewRelativePath
            };
        }

        private static bool ApplyWeaponIdentity(WeaponItem weapon, string desiredName)
        {
            string normalizedName = desiredName.Trim();
            bool changed = false;

            if (!string.Equals(weapon.itemName, normalizedName, StringComparison.Ordinal) ||
                !string.Equals(weapon.name, normalizedName, StringComparison.Ordinal))
            {
                Undo.RecordObject(weapon, "Apply Weapon Display Name");
                weapon.itemName = normalizedName;
                weapon.name = normalizedName;
                EditorUtility.SetDirty(weapon);
                changed = true;
            }

            return RenameWeaponAssetToMatchObjectName(weapon) || changed;
        }

        private static bool RenameWeaponAssetToMatchObjectName(WeaponItem weapon)
        {
            string assetPath = AssetDatabase.GetAssetPath(weapon);
            if (string.IsNullOrWhiteSpace(assetPath))
                return false;

            string desiredFileName = SanitizeFileName(weapon.name?.Trim());
            if (string.IsNullOrWhiteSpace(desiredFileName))
                return false;

            string currentFileName = Path.GetFileNameWithoutExtension(assetPath);
            if (string.Equals(currentFileName, desiredFileName, StringComparison.Ordinal))
                return false;

            string renameError = AssetDatabase.RenameAsset(assetPath, desiredFileName);
            if (!string.IsNullOrWhiteSpace(renameError))
            {
                
                return false;
            }

            return true;
        }

        private static List<string> CollectMeshNames(GameObject prefab)
        {
            List<string> meshNames = new List<string>();

            if (prefab == null)
                return meshNames;

            MeshFilter[] meshFilters = prefab.GetComponentsInChildren<MeshFilter>(true);
            foreach (MeshFilter meshFilter in meshFilters)
            {
                if (meshFilter.sharedMesh == null)
                    continue;

                meshNames.Add(meshFilter.sharedMesh.name);
            }

            SkinnedMeshRenderer[] skinnedMeshRenderers = prefab.GetComponentsInChildren<SkinnedMeshRenderer>(true);
            foreach (SkinnedMeshRenderer skinnedMeshRenderer in skinnedMeshRenderers)
            {
                if (skinnedMeshRenderer.sharedMesh == null)
                    continue;

                meshNames.Add(skinnedMeshRenderer.sharedMesh.name);
            }

            return meshNames
                .Where(name => !string.IsNullOrWhiteSpace(name))
                .Distinct(StringComparer.OrdinalIgnoreCase)
                .OrderBy(name => name, StringComparer.OrdinalIgnoreCase)
                .ToList();
        }

        private static void ExportPreviewTexture(GameObject prefab, string absoluteOutputPath)
        {
            GameObject instance = null;
            PreviewRenderUtility previewUtility = null;
            RenderTexture renderTexture = null;
            Texture2D texture = null;

            try
            {
                instance = Object.Instantiate(prefab);
                instance.hideFlags = HideFlags.HideAndDontSave;

                previewUtility = new PreviewRenderUtility();
                previewUtility.cameraFieldOfView = 30f;
                previewUtility.lights[0].intensity = 1.15f;
                previewUtility.lights[0].transform.rotation = Quaternion.Euler(35f, 35f, 0f);
                previewUtility.lights[1].intensity = 0.8f;
                previewUtility.lights[1].transform.rotation = Quaternion.Euler(340f, 220f, 0f);
                previewUtility.ambientColor = new Color(0.42f, 0.42f, 0.42f, 1f);
                previewUtility.AddSingleGO(instance);

                Bounds bounds = CalculateRenderableBounds(instance);
                SetupPreviewCamera(previewUtility.camera, bounds);

                renderTexture = new RenderTexture(PreviewSize, PreviewSize, 24, RenderTextureFormat.ARGB32);
                renderTexture.hideFlags = HideFlags.HideAndDontSave;

                previewUtility.camera.targetTexture = renderTexture;
                previewUtility.camera.Render();

                RenderTexture previous = RenderTexture.active;
                RenderTexture.active = renderTexture;

                texture = new Texture2D(PreviewSize, PreviewSize, TextureFormat.RGBA32, false);
                texture.ReadPixels(new Rect(0, 0, PreviewSize, PreviewSize), 0, 0);
                texture.Apply();
                RenderTexture.active = previous;

                byte[] pngBytes = texture.EncodeToPNG();
                File.WriteAllBytes(absoluteOutputPath, pngBytes);
            }
            catch (Exception)
            {
                
            }
            finally
            {
                if (texture != null)
                    Object.DestroyImmediate(texture);

                if (renderTexture != null)
                    Object.DestroyImmediate(renderTexture);

                if (previewUtility != null)
                    previewUtility.Cleanup();

                if (instance != null)
                    Object.DestroyImmediate(instance);
            }
        }

        private static Bounds CalculateRenderableBounds(GameObject instance)
        {
            Renderer[] renderers = instance.GetComponentsInChildren<Renderer>(true);
            Bounds bounds = new Bounds(instance.transform.position, Vector3.one * 0.5f);

            if (renderers.Length == 0)
                return bounds;

            bounds = renderers[0].bounds;
            for (int i = 1; i < renderers.Length; i++)
            {
                if (renderers[i] == null)
                    continue;

                bounds.Encapsulate(renderers[i].bounds);
            }

            return bounds;
        }

        private static void SetupPreviewCamera(Camera camera, Bounds bounds)
        {
            camera.clearFlags = CameraClearFlags.Color;
            camera.backgroundColor = new Color(0.11f, 0.11f, 0.115f, 0f);
            camera.nearClipPlane = 0.01f;

            float radius = Mathf.Max(0.2f, bounds.extents.magnitude);
            float halfFovRadians = camera.fieldOfView * 0.5f * Mathf.Deg2Rad;
            float distance = radius / Mathf.Tan(halfFovRadians);
            Vector3 viewDirection = new Vector3(-0.9f, 0.28f, -1.35f).normalized;
            Vector3 lookTarget = bounds.center + Vector3.up * (bounds.size.y * 0.04f);

            camera.transform.position = lookTarget - viewDirection * distance * 1.5f;
            camera.transform.rotation = Quaternion.LookRotation(lookTarget - camera.transform.position, Vector3.up);
            camera.farClipPlane = distance * 4f;
        }

        private static void WriteManifestCsv(List<WeaponPreviewRecord> records, string absolutePath)
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendLine("assetPath,currentObjectName,currentDisplayName,suggestedDisplayName,itemType,weaponClass,weaponModelType,prefabName,meshNames,previewPath,namingNotes");

            foreach (WeaponPreviewRecord record in records)
            {
                builder.AppendLine(string.Join(",",
                    EscapeCsv(record.assetPath),
                    EscapeCsv(record.currentObjectName),
                    EscapeCsv(record.currentDisplayName),
                    EscapeCsv(record.suggestedDisplayName),
                    EscapeCsv(record.itemType),
                    EscapeCsv(record.weaponClass),
                    EscapeCsv(record.weaponModelType),
                    EscapeCsv(record.prefabName),
                    EscapeCsv(record.meshNames),
                    EscapeCsv(record.previewPath),
                    EscapeCsv(record.namingNotes)));
            }

            File.WriteAllText(absolutePath, builder.ToString(), Encoding.UTF8);
        }

        private static void WriteManifestJson(List<WeaponPreviewRecord> records, string absolutePath)
        {
            WeaponPreviewRecordCollection collection = new WeaponPreviewRecordCollection
            {
                records = records
            };

            string json = JsonUtility.ToJson(collection, true);
            File.WriteAllText(absolutePath, json, Encoding.UTF8);
        }

        private static void EnsureOverrideCsv(List<WeaponPreviewRecord> records, string absolutePath)
        {
            if (File.Exists(absolutePath))
                return;

            StringBuilder builder = new StringBuilder();
            builder.AppendLine("assetPath,displayName");

            foreach (WeaponPreviewRecord record in records)
                builder.AppendLine($"{EscapeCsv(record.assetPath)},{EscapeCsv(record.suggestedDisplayName)}");

            File.WriteAllText(absolutePath, builder.ToString(), Encoding.UTF8);
        }

        private static Dictionary<string, string> ReadOverrideCsv(string absolutePath)
        {
            Dictionary<string, string> overrides = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
            string[] lines = File.ReadAllLines(absolutePath);

            for (int i = 1; i < lines.Length; i++)
            {
                string line = lines[i];
                if (string.IsNullOrWhiteSpace(line))
                    continue;

                string[] fields = ParseCsvLine(line);
                if (fields.Length < 2)
                    continue;

                string assetPath = fields[0].Trim();
                string displayName = fields[1].Trim();

                if (string.IsNullOrWhiteSpace(assetPath))
                    continue;

                overrides[assetPath] = displayName;
            }

            return overrides;
        }

        private static WeaponNamingSuggestion SuggestDisplayName(
            WeaponItem weapon,
            string assetPath,
            string prefabName,
            List<string> meshNames)
        {
            string baseName = Path.GetFileNameWithoutExtension(assetPath);
            string normalizedSource = NormalizeSourceText($"{baseName} {prefabName} {string.Join(" ", meshNames)}");
            string descriptor = BuildDescriptor(normalizedSource);
            string weaponType = BuildWeaponType(normalizedSource, weapon);
            int numericSuffix = ExtractNumericSuffix(baseName);
            bool shouldKeepVariantNumber = RequiresVariantNumber(baseName, weaponType);

            string displayName;
            if (string.IsNullOrWhiteSpace(descriptor))
                displayName = weaponType;
            else
                displayName = descriptor + " " + weaponType;

            if (shouldKeepVariantNumber && numericSuffix > 0)
                displayName += " " + numericSuffix.ToString("00", CultureInfo.InvariantCulture);

            displayName = displayName.Trim();

            if (string.IsNullOrWhiteSpace(displayName))
                displayName = ToTitleCase(CleanFallbackBaseName(baseName));

            string notes = $"source='{baseName}', prefab='{prefabName}', type='{weaponType}', descriptor='{descriptor}'";

            return new WeaponNamingSuggestion
            {
                displayName = displayName,
                notes = notes
            };
        }

        private static string BuildDescriptor(string source)
        {
            if (source.Contains("goblin "))
                return "Goblin";
            if (source.Contains("crystal ") || source.Contains("crysta "))
                return "Crystal";
            if (source.Contains("ornate "))
                return "Ornate";
            if (source.Contains("rune "))
                return "Runed";
            if (source.Contains("nature "))
                return "Verdant";
            if (source.Contains("bone "))
                return "Bone";
            if (source.Contains("gravestone "))
                return "Gravestone";
            if (source.Contains("skull "))
                return "Skull";
            if (source.Contains("chitin "))
                return "Chitin";
            if (source.Contains("plank "))
                return "Plank";
            if (source.Contains("heater "))
                return "Heater";
            if (source.Contains("gem "))
                return "Gem";
            if (source.Contains("doubleblade "))
                return "Double-Bladed";
            if (source.Contains("war "))
                return "War";

            return string.Empty;
        }

        private static string BuildWeaponType(string source, WeaponItem weapon)
        {
            if (source.Contains("unarmed"))
                return "Unarmed";

            if (source.Contains("scythe"))
                return "Scythe";

            if (source.Contains("halberd"))
                return "Halberd";

            if (source.Contains("joust"))
                return "Lance";

            if (source.Contains("spear"))
                return "Spear";

            if (source.Contains("rapier"))
                return source.Contains("cover") ? "Sheathed Rapier" : "Rapier";

            if (source.Contains("greatsword") || source.Contains("sword large"))
                return "Greatsword";

            if (source.Contains("straightsword"))
                return "Straight Sword";

            if (source.Contains("broadsword"))
                return "Broadsword";

            if (source.Contains("shortsword") || source.Contains("sword small"))
                return source.Contains("cover") ? "Sheathed Shortsword" : "Shortsword";

            if (source.Contains("cutlass"))
                return "Cutlass";

            if (source.Contains("machete"))
                return "Machete";

            if (source.Contains("sword cover"))
                return "Sheathed Sword";

            if (source.Contains(" sword "))
                return "Sword";

            if (source.Contains("throwingknife"))
                return "Throwing Knife";

            if (source.Contains("shiv"))
                return "Shiv";

            if (source.Contains("dagger"))
                return "Dagger";

            if (source.Contains("handaxe"))
                return "Hand Axe";

            if (source.Contains("axe"))
                return "Axe";

            if (source.Contains("mace"))
                return "Mace";

            if (source.Contains("hammer"))
                return "Hammer";

            if (source.Contains("club"))
                return "Club";

            if (source.Contains("staff"))
                return "Staff";

            if (source.Contains("bow"))
                return "Bow";

            if (source.Contains("talisman") || source.Contains("charm"))
                return "Talisman";

            if (source.Contains("banner"))
                return "Banner";

            if (source.Contains("buckler"))
                return "Buckler";

            if (source.Contains("shield long"))
                return "Long Shield";

            if (source.Contains("shield round"))
                return "Round Shield";

            if (source.Contains("shield spike"))
                return "Spiked Shield";

            if (source.Contains("shield ornate"))
                return "Ornate Shield";

            if (source.Contains("shield heater"))
                return "Heater Shield";

            if (source.Contains("shield "))
                return "Shield";

            return weapon.weaponClass switch
            {
                WeaponClass.Spear => "Spear",
                WeaponClass.MediumShield => "Shield",
                WeaponClass.LightShield => "Light Shield",
                WeaponClass.Bow => "Bow",
                _ => "Weapon"
            };
        }

        private static bool RequiresVariantNumber(string baseName, string weaponType)
        {
            return baseName.Contains("Weapon_Shield_", StringComparison.OrdinalIgnoreCase) ||
                   baseName.Contains("Weapon_Shield_Long_", StringComparison.OrdinalIgnoreCase) ||
                   baseName.Contains("Weapon_Dungeon_Banner_", StringComparison.OrdinalIgnoreCase) ||
                   baseName.Contains("Weapon_Dungeon_Hammer_", StringComparison.OrdinalIgnoreCase) ||
                   baseName.Contains("Weapon_Dungeon_Ornate_Axe_", StringComparison.OrdinalIgnoreCase) ||
                   baseName.Contains("Weapon_Dungeon_Shield_", StringComparison.OrdinalIgnoreCase) ||
                   string.Equals(weaponType, "Shield", StringComparison.OrdinalIgnoreCase) ||
                   string.Equals(weaponType, "Long Shield", StringComparison.OrdinalIgnoreCase);
        }

        private static int ExtractNumericSuffix(string baseName)
        {
            string[] parts = baseName.Split('_');
            for (int i = parts.Length - 1; i >= 0; i--)
            {
                if (int.TryParse(parts[i], NumberStyles.Integer, CultureInfo.InvariantCulture, out int suffix))
                    return suffix;
            }

            return 0;
        }

        private static string CleanFallbackBaseName(string baseName)
        {
            string cleaned = baseName
                .Replace("Weapon_", string.Empty, StringComparison.OrdinalIgnoreCase)
                .Replace("Dungeon_", string.Empty, StringComparison.OrdinalIgnoreCase)
                .Replace("Polygon_", string.Empty, StringComparison.OrdinalIgnoreCase)
                .Replace("PP_Theme_04_", string.Empty, StringComparison.OrdinalIgnoreCase)
                .Replace("_", " ");

            return cleaned.Trim();
        }

        private static string NormalizeSourceText(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return string.Empty;

            string normalized = value
                .Replace("PP_", " ")
                .Replace("Theme", " ")
                .Replace("Polygon", " ")
                .Replace("Dungeon", " ")
                .Replace("Generated", " ")
                .Replace("-", " ")
                .Replace("_", " ")
                .Replace("  ", " ");

            return " " + normalized.ToLowerInvariant().Trim() + " ";
        }

        private static string ToTitleCase(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return string.Empty;

            return CultureInfo.InvariantCulture.TextInfo.ToTitleCase(value.ToLowerInvariant());
        }

        private static string EscapeCsv(string value)
        {
            if (value == null)
                return "\"\"";

            string escaped = value.Replace("\"", "\"\"");
            return $"\"{escaped}\"";
        }

        private static string[] ParseCsvLine(string line)
        {
            List<string> fields = new List<string>();
            StringBuilder field = new StringBuilder();
            bool insideQuotes = false;

            for (int i = 0; i < line.Length; i++)
            {
                char character = line[i];

                if (character == '"')
                {
                    if (insideQuotes && i + 1 < line.Length && line[i + 1] == '"')
                    {
                        field.Append('"');
                        i++;
                    }
                    else
                    {
                        insideQuotes = !insideQuotes;
                    }

                    continue;
                }

                if (character == ',' && !insideQuotes)
                {
                    fields.Add(field.ToString());
                    field.Clear();
                    continue;
                }

                field.Append(character);
            }

            fields.Add(field.ToString());
            return fields.ToArray();
        }

        private static string SanitizeFileName(string value)
        {
            char[] invalidCharacters = Path.GetInvalidFileNameChars();
            StringBuilder builder = new StringBuilder(value.Length);

            foreach (char character in value)
            {
                if (invalidCharacters.Contains(character))
                    builder.Append('_');
                else
                    builder.Append(character);
            }

            return builder.ToString();
        }

        private static string GetExportAbsoluteRoot()
        {
            string relativePath = ExportRelativeRoot.Replace('/', Path.DirectorySeparatorChar);
            return Path.Combine(Directory.GetCurrentDirectory(), relativePath);
        }

        [Serializable]
        private class WeaponPreviewRecordCollection
        {
            public List<WeaponPreviewRecord> records = new List<WeaponPreviewRecord>();
        }

        [Serializable]
        private class WeaponPreviewRecord
        {
            public string assetPath;
            public string itemType;
            public string currentObjectName;
            public string currentDisplayName;
            public string suggestedDisplayName;
            public string namingNotes;
            public string weaponClass;
            public string weaponModelType;
            public string modelPath;
            public string prefabName;
            public string meshNames;
            public string previewPath;
        }

        private struct WeaponNamingSuggestion
        {
            public string displayName;
            public string notes;
        }
    }
}
