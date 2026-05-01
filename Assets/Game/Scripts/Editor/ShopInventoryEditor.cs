using UnityEditor;
using UnityEngine;

namespace baodeag
{
    [CustomEditor(typeof(ShopInventory))]
    public class ShopInventoryEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            ShopInventory shopInventory = (ShopInventory)target;
            SerializedProperty iterator = serializedObject.GetIterator();
            bool expanded = true;

            while (iterator.NextVisible(expanded))
            {
                if (iterator.name == "m_Script")
                {
                    using (new EditorGUI.DisabledScope(true))
                    {
                        EditorGUILayout.PropertyField(iterator, true);
                    }
                }
                else
                {
                    EditorGUILayout.PropertyField(iterator, true);
                }

                expanded = false;
            }

            EditorGUILayout.Space(8f);
            EditorGUILayout.HelpBox(
                "Quick setup: give each merchant a unique merchantID, keep Auto Scale Shop Tier From Progression enabled for map-based scaling, use Shop Tier Offset to make a merchant earlier or later than the current map, and fill Custom Stock with Required Progression Tier for gated items.",
                MessageType.Info);

            EditorGUILayout.LabelField("Recommended Tier Flow", EditorStyles.boldLabel);
            EditorGUILayout.LabelField("Map 1 = Tier 1, Map 2 = Tier 2, Map 3 = Tier 3, Map 4 = Tier 4, Map 5 = Tier 5");

            if (GameProgressionManager.instance != null)
            {
                EditorGUILayout.LabelField("Runtime Tier Preview", shopInventory.GetEffectiveShopProgressionTier().ToString());
            }

            if (string.IsNullOrWhiteSpace(shopInventory.name) == false && GUILayout.Button("Auto Fill Merchant ID From Object Name"))
            {
                SerializedProperty merchantIDProperty = serializedObject.FindProperty("merchantID");
                merchantIDProperty.stringValue = shopInventory.name.Trim().ToLowerInvariant().Replace(' ', '_');
            }

            if (GUILayout.Button("Open Merchant Setup Checklist"))
            {
                Selection.activeObject = AssetDatabase.LoadAssetAtPath<Object>(GameAssetPaths.DocsRoot + "/MERCHANT_SETUP_CHECKLIST.md");
            }

            if (GUILayout.Button("Generate Merchant Setup Checklist"))
            {
                MerchantSetupChecklistGenerator.GenerateChecklist();
            }

            serializedObject.ApplyModifiedProperties();
            EditorUtility.SetDirty(shopInventory);
        }
    }
}
