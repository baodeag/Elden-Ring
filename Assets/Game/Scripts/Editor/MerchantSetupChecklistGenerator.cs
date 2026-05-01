using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace baodeag
{
    public static class MerchantSetupChecklistGenerator
    {
        private const string ChecklistPath = GameAssetPaths.DocsRoot + "/MERCHANT_SETUP_CHECKLIST.md";

        private struct MerchantRecord
        {
            public string assetPath;
            public string hierarchyPath;
            public string shopName;
            public string merchantID;
            public bool autoScaleShopTierFromProgression;
            public int shopTierOffset;
            public bool useGlobalPurchasableItems;
        }

        [MenuItem("Tools/Merchants/Generate Setup Checklist")]
        public static void GenerateChecklistMenuItem()
        {
            GenerateChecklist();
        }

        public static void GenerateChecklist()
        {
            List<MerchantRecord> merchantRecords = new List<MerchantRecord>();

            GatherPrefabMerchants(merchantRecords);
            GatherSceneMerchants(merchantRecords);

            Directory.CreateDirectory(Path.GetDirectoryName(ChecklistPath));
            File.WriteAllText(ChecklistPath, BuildChecklistContent(merchantRecords));
            AssetDatabase.Refresh();

            Object checklistAsset = AssetDatabase.LoadAssetAtPath<Object>(ChecklistPath);

            if (checklistAsset != null)
                Selection.activeObject = checklistAsset;

            Debug.Log($"Generated merchant setup checklist with {merchantRecords.Count} merchant entries at '{ChecklistPath}'.");
        }

        private static void GatherPrefabMerchants(List<MerchantRecord> merchantRecords)
        {
            string[] prefabGuids = AssetDatabase.FindAssets("t:Prefab");

            for (int i = 0; i < prefabGuids.Length; i++)
            {
                string prefabPath = AssetDatabase.GUIDToAssetPath(prefabGuids[i]);
                GameObject prefabRoot = AssetDatabase.LoadAssetAtPath<GameObject>(prefabPath);

                if (prefabRoot == null)
                    continue;

                ShopInventory[] shops = prefabRoot.GetComponentsInChildren<ShopInventory>(true);

                for (int shopIndex = 0; shopIndex < shops.Length; shopIndex++)
                {
                    ShopInventory shop = shops[shopIndex];

                    if (shop == null)
                        continue;

                    merchantRecords.Add(CreateRecord(prefabPath, GetHierarchyPath(shop.transform), shop));
                }
            }
        }

        private static void GatherSceneMerchants(List<MerchantRecord> merchantRecords)
        {
            string[] sceneGuids = AssetDatabase.FindAssets("t:Scene");
            Scene originalScene = SceneManager.GetActiveScene();

            for (int i = 0; i < sceneGuids.Length; i++)
            {
                string scenePath = AssetDatabase.GUIDToAssetPath(sceneGuids[i]);
                Scene openedScene = EditorSceneManager.OpenScene(scenePath, OpenSceneMode.Additive);

                try
                {
                    GameObject[] rootObjects = openedScene.GetRootGameObjects();

                    for (int rootIndex = 0; rootIndex < rootObjects.Length; rootIndex++)
                    {
                        ShopInventory[] shops = rootObjects[rootIndex].GetComponentsInChildren<ShopInventory>(true);

                        for (int shopIndex = 0; shopIndex < shops.Length; shopIndex++)
                        {
                            ShopInventory shop = shops[shopIndex];

                            if (shop == null)
                                continue;

                            merchantRecords.Add(CreateRecord(scenePath, GetHierarchyPath(shop.transform), shop));
                        }
                    }
                }
                finally
                {
                    EditorSceneManager.CloseScene(openedScene, true);
                }
            }

            if (originalScene.IsValid() && !string.IsNullOrEmpty(originalScene.path))
                EditorSceneManager.OpenScene(originalScene.path, OpenSceneMode.Single);
        }

        private static MerchantRecord CreateRecord(string assetPath, string hierarchyPath, ShopInventory shop)
        {
            SerializedObject serializedShop = new SerializedObject(shop);

            return new MerchantRecord
            {
                assetPath = assetPath,
                hierarchyPath = hierarchyPath,
                shopName = shop.shopName,
                merchantID = serializedShop.FindProperty("merchantID")?.stringValue ?? string.Empty,
                autoScaleShopTierFromProgression = serializedShop.FindProperty("autoScaleShopTierFromProgression")?.boolValue ?? true,
                shopTierOffset = serializedShop.FindProperty("shopTierOffset")?.intValue ?? 0,
                useGlobalPurchasableItems = serializedShop.FindProperty("useGlobalPurchasableItems")?.boolValue ?? true
            };
        }

        private static string GetHierarchyPath(Transform currentTransform)
        {
            if (currentTransform == null)
                return string.Empty;

            Stack<string> segments = new Stack<string>();

            while (currentTransform != null)
            {
                segments.Push(currentTransform.name);
                currentTransform = currentTransform.parent;
            }

            return string.Join("/", segments.ToArray());
        }

        private static string BuildChecklistContent(List<MerchantRecord> merchantRecords)
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendLine("# Merchant Setup Checklist");
            builder.AppendLine();
            builder.AppendLine($"Generated automatically on {System.DateTime.Now:yyyy-MM-dd HH:mm:ss}.");
            builder.AppendLine();
            builder.AppendLine($"Detected merchant/shop entries: {merchantRecords.Count}");
            builder.AppendLine();

            if (merchantRecords.Count == 0)
            {
                builder.AppendLine("No `ShopInventory` components were found in prefabs or scenes.");
                builder.AppendLine();
            }
            else
            {
                for (int i = 0; i < merchantRecords.Count; i++)
                {
                    MerchantRecord record = merchantRecords[i];
                    builder.AppendLine($"## Merchant {i + 1}");
                    builder.AppendLine();
                    builder.AppendLine($"- Asset: `{record.assetPath}`");
                    builder.AppendLine($"- Hierarchy: `{record.hierarchyPath}`");
                    builder.AppendLine($"- shopName: `{record.shopName}`");
                    builder.AppendLine($"- merchantID: `{record.merchantID}`");
                    builder.AppendLine($"- autoScaleShopTierFromProgression: `{record.autoScaleShopTierFromProgression}`");
                    builder.AppendLine($"- shopTierOffset: `{record.shopTierOffset}`");
                    builder.AppendLine($"- useGlobalPurchasableItems: `{record.useGlobalPurchasableItems}`");
                    builder.AppendLine();
                }
            }

            builder.AppendLine("## Per Merchant Checklist");
            builder.AppendLine();
            builder.AppendLine("- Give each merchant a unique `merchantID`.");
            builder.AppendLine("- Keep `autoScaleShopTierFromProgression` enabled if the shop should follow map progression.");
            builder.AppendLine("- Use `shopTierOffset = 0` for normal scaling, `1` for stronger/later stock, `-1` for earlier/cheaper stock.");
            builder.AppendLine("- Turn off `useGlobalPurchasableItems` when you want a curated merchant inventory.");
            builder.AppendLine("- Fill `customStock` and set `requiredProgressionTier` per item.");
            builder.AppendLine();
            builder.AppendLine("## Suggested Tier Mapping");
            builder.AppendLine();
            builder.AppendLine("- Map 1 -> Tier 1");
            builder.AppendLine("- Map 2 -> Tier 2");
            builder.AppendLine("- Map 3 -> Tier 3");
            builder.AppendLine("- Map 4 -> Tier 4");
            builder.AppendLine("- Map 5 -> Tier 5");
            return builder.ToString();
        }
    }
}
