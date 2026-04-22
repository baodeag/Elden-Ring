using UnityEngine;
using UnityEditor;
using System.IO;
using System.Collections.Generic;

public class PixeliusVita_Tool : EditorWindow
{
    // --- NESTED CLASS FOR MULTI-MODEL DATA ---
    [System.Serializable]
    public class InjectionObject
    {
        public GameObject model;
        public Vector3 position = Vector3.zero;
        public Vector3 rotation = Vector3.zero;
        public Vector3 scale = Vector3.one;
    }

    private int toolbarIndex = 0;
    private string[] toolbarLabels = { "Mat/Shader", "Texture", "Color Proc", "Prefab Injector" };

    // Targeted search folders to prevent naming conflicts in large projects
    public DefaultAsset matSearchFolder;
    public DefaultAsset texSearchFolder;

    // Data for Color Processor
    public List<Texture2D> sourceTextures = new List<Texture2D>();
    public List<Color> targetColors = new List<Color>();
    public bool forceAllPixelsToTargetColor = false;

    // Data for Prefab Injector
    public List<InjectionObject> injectionObjects = new List<InjectionObject>();
    public bool clearExistingChildren = true;

    private SerializedObject so;
    private int lastListCount = 0;

    [MenuItem("Tools/PixeliusVita Tool")]
    public static void ShowWindow()
    {
        PixeliusVita_Tool window = GetWindow<PixeliusVita_Tool>("PixeliusVita Workspace");
        window.minSize = new Vector2(500, 650);
    }

    private void OnEnable()
    {
        so = new SerializedObject(this);
        lastListCount = injectionObjects.Count;

        // Initialize default target colors if empty
        if (targetColors.Count == 0)
        {
            targetColors.Add(Color.red);
            targetColors.Add(Color.yellow);
            targetColors.Add(Color.green);
            targetColors.Add(new Color(0.7f, 0.9f, 1f));
            targetColors.Add(new Color(0.6f, 0.2f, 0.8f));
        }
    }

    private void OnGUI()
    {
        GUILayout.Space(10);
        EditorGUILayout.LabelField("PIXELIUS VITA - ALL IN ONE TOOL", EditorStyles.boldLabel);

        toolbarIndex = GUILayout.Toolbar(toolbarIndex, toolbarLabels, GUILayout.Height(30));
        GUILayout.Space(15);

        so.Update();

        // Handle default scale for new list elements
        if (injectionObjects.Count > lastListCount)
        {
            for (int i = lastListCount; i < injectionObjects.Count; i++)
            {
                if (injectionObjects[i] != null)
                    injectionObjects[i].scale = Vector3.one;
            }
        }
        lastListCount = injectionObjects.Count;

        switch (toolbarIndex)
        {
            case 0: DrawAssignMatShaderTab(); break;
            case 1: DrawAssignTextureTab(); break;
            case 2: DrawColorProcessorTab(); break;
            case 3: DrawPrefabInjectorTab(); break;
        }

        so.ApplyModifiedProperties();
    }

    #region TAB 01: AUTO ASSIGN MATERIAL OR SHADER
    private void DrawAssignMatShaderTab()
    {
        EditorGUILayout.HelpBox("Specify the folder containing Materials. The tool will perform a recursive search within this path.", MessageType.Info);

        matSearchFolder = (DefaultAsset)EditorGUILayout.ObjectField("Material Root Folder", matSearchFolder, typeof(DefaultAsset), false);

        if (GUILayout.Button("Execute: Auto Assign Mat/Shader", GUILayout.Height(40)))
        {
            ExecuteAssignMatOrShader();
        }
    }

    private void ExecuteAssignMatOrShader()
    {
        if (matSearchFolder == null)
        {
            EditorUtility.DisplayDialog("Source Missing", "Please assign a Material Root Folder first!", "OK");
            return;
        }

        string folderPath = AssetDatabase.GetAssetPath(matSearchFolder);
        GameObject[] selectedObjects = Selection.gameObjects;

        if (selectedObjects.Length == 0)
        {
            EditorUtility.DisplayDialog("Selection Required", "Please select GameObjects or Prefabs!", "OK");
            return;
        }

        int success = 0; int fail = 0;
        foreach (GameObject go in selectedObjects)
        {
            string targetName = go.name.Trim();
            bool assigned = false;

            Material foundMat = FindAssetInFolder<Material>(targetName, "t:Material", folderPath);
            Renderer[] renderers = go.GetComponentsInChildren<Renderer>(true);

            if (foundMat != null)
            {
                foreach (Renderer ren in renderers)
                {
                    Undo.RecordObject(ren, "Assign Material");
                    Material[] newMats = new Material[ren.sharedMaterials.Length];
                    for (int i = 0; i < newMats.Length; i++) newMats[i] = foundMat;
                    ren.sharedMaterials = newMats;
                }
                assigned = true;
            }

            if (assigned) success++; else fail++;
        }

        AssetDatabase.SaveAssets();
        EditorUtility.DisplayDialog("Process Complete", $"Success: {success} | Failed: {fail}", "OK");
    }
    #endregion

    #region TAB 02: AUTO ASSIGN TEXTURE TO MATERIAL
    private void DrawAssignTextureTab()
    {
        EditorGUILayout.HelpBox("Specify the folder containing Textures to avoid conflicts with assets having identical names in other directories.", MessageType.Info);

        texSearchFolder = (DefaultAsset)EditorGUILayout.ObjectField("Texture Root Folder", texSearchFolder, typeof(DefaultAsset), false);

        if (GUILayout.Button("Execute: Assign Texture by Name", GUILayout.Height(40)))
        {
            ExecuteAssignTextureToMaterial();
        }
    }

    private void ExecuteAssignTextureToMaterial()
    {
        if (texSearchFolder == null)
        {
            EditorUtility.DisplayDialog("Source Missing", "Please assign a Texture Root Folder!", "OK");
            return;
        }

        string folderPath = AssetDatabase.GetAssetPath(texSearchFolder);
        Object[] selectedAssets = Selection.GetFiltered<Material>(SelectionMode.Assets);
        int success = 0;

        foreach (Material mat in selectedAssets)
        {
            Texture2D foundTex = FindAssetInFolder<Texture2D>(mat.name, "t:Texture2D", folderPath);
            if (foundTex != null)
            {
                Undo.RecordObject(mat, "Assign Texture");
                // Check common shader property names
                if (mat.HasProperty("_BaseMap")) mat.SetTexture("_BaseMap", foundTex);
                else if (mat.HasProperty("_MainTex")) mat.SetTexture("_MainTex", foundTex);
                else if (mat.HasProperty("_BaseTexture")) mat.SetTexture("_BaseTexture", foundTex);

                EditorUtility.SetDirty(mat);
                success++;
            }
        }

        AssetDatabase.SaveAssets();
        EditorUtility.DisplayDialog("Process Complete", $"Successfully assigned: {success} textures.", "OK");
    }
    #endregion

    #region TAB 03: COLOR PROCESSOR PRO
    private void DrawColorProcessorTab()
    {
        EditorGUILayout.PropertyField(so.FindProperty("sourceTextures"), true);
        forceAllPixelsToTargetColor = EditorGUILayout.ToggleLeft("Force Color Mode (Monochrome)", forceAllPixelsToTargetColor);
        EditorGUILayout.PropertyField(so.FindProperty("targetColors"), true);

        if (GUILayout.Button("Execute Color Processing", GUILayout.Height(40)))
        {
            if (sourceTextures.Count > 0) ProcessColorTextures();
        }
    }

    private void ProcessColorTextures()
    {
        foreach (Texture2D tex in sourceTextures)
        {
            if (tex == null) continue;
            string assetPath = AssetDatabase.GetAssetPath(tex);
            TextureImporter importer = AssetImporter.GetAtPath(assetPath) as TextureImporter;

            // Ensure texture is readable for pixel manipulation
            if (importer != null && !importer.isReadable)
            {
                importer.isReadable = true;
                importer.SaveAndReimport();
            }

            float originalBaseHue = GetAverageHue(tex);
            for (int i = 0; i < targetColors.Count; i++)
            {
                float targetH, s, v;
                Color.RGBToHSV(targetColors[i], out targetH, out s, out v);

                Texture2D processed = forceAllPixelsToTargetColor ?
                    ApplyForceColor(tex, targetH) :
                    RotateHue(tex, targetH - originalBaseHue);

                string fullPath = Path.Combine(Path.GetDirectoryName(assetPath),
                    $"{Path.GetFileNameWithoutExtension(assetPath)}{(char)('a' + i)}.png");

                File.WriteAllBytes(fullPath, processed.EncodeToPNG());
            }
        }
        AssetDatabase.Refresh();
    }
    #endregion

    #region TAB 04: PREFAB INJECTOR
    private void DrawPrefabInjectorTab()
    {
        EditorGUILayout.HelpBox("1. Configure models and their specific transforms.\n2. Select target Prefabs in Project window.\n3. Click Execute.", MessageType.Info);

        EditorGUILayout.PropertyField(so.FindProperty("injectionObjects"), new GUIContent("Injection List"), true);

        GUILayout.Space(5);
        clearExistingChildren = EditorGUILayout.ToggleLeft("Clear all existing children in Prefabs", clearExistingChildren);

        GUILayout.Space(10);
        if (GUILayout.Button("Execute Multi-Injection", GUILayout.Height(45)))
        {
            ExecutePrefabInjection();
        }
    }

    private void ExecutePrefabInjection()
    {
        if (injectionObjects == null || injectionObjects.Count == 0)
        {
            EditorUtility.DisplayDialog("Error", "The Injection list is empty!", "OK");
            return;
        }

        GameObject[] selectedPrefabs = Selection.GetFiltered<GameObject>(SelectionMode.Assets);
        if (selectedPrefabs.Length == 0)
        {
            EditorUtility.DisplayDialog("Selection Required", "Please select target Prefabs in the Project window!", "OK");
            return;
        }

        int count = 0;
        foreach (GameObject prefabAsset in selectedPrefabs)
        {
            string assetPath = AssetDatabase.GetAssetPath(prefabAsset);
            if (!assetPath.ToLower().EndsWith(".prefab")) continue;

            GameObject root = PrefabUtility.LoadPrefabContents(assetPath);

            if (clearExistingChildren)
            {
                for (int i = root.transform.childCount - 1; i >= 0; i--)
                {
                    DestroyImmediate(root.transform.GetChild(i).gameObject);
                }
            }

            foreach (var item in injectionObjects)
            {
                if (item.model == null) continue;

                GameObject newInstance = (GameObject)PrefabUtility.InstantiatePrefab(item.model, root.transform);
                newInstance.transform.localPosition = item.position;
                newInstance.transform.localRotation = Quaternion.Euler(item.rotation);
                newInstance.transform.localScale = item.scale;
            }

            PrefabUtility.SaveAsPrefabAsset(root, assetPath);
            PrefabUtility.UnloadPrefabContents(root);
            count++;
        }

        AssetDatabase.Refresh();
        EditorUtility.DisplayDialog("Success", $"Injected objects into {count} prefab(s) successfully!", "OK");
    }
    #endregion

    // --- SHARED UTILITIES ---

    private T FindAssetInFolder<T>(string name, string filter, string folderPath) where T : Object
    {
        // Limit search scope to the specified directory for safety and performance
        string[] searchInFolders = new string[] { folderPath };
        string[] guids = AssetDatabase.FindAssets(name + " " + filter, searchInFolders);

        foreach (string guid in guids)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            // Strict name comparison to avoid partial matches
            if (Path.GetFileNameWithoutExtension(path).Equals(name, System.StringComparison.OrdinalIgnoreCase))
                return AssetDatabase.LoadAssetAtPath<T>(path);
        }
        return null;
    }

    private float GetAverageHue(Texture2D tex)
    {
        Color[] pixels = tex.GetPixels();
        float totalH = 0; int count = 0;
        foreach (Color c in pixels)
        {
            if (c.a < 0.1f) continue;
            float h, s, v;
            Color.RGBToHSV(c, out h, out s, out v);
            if (s < 0.1f) continue;
            totalH += h; count++;
        }
        return count > 0 ? totalH / count : 0f;
    }

    private Texture2D RotateHue(Texture2D source, float offset)
    {
        Texture2D res = new Texture2D(source.width, source.height, TextureFormat.RGBA32, false);
        Color[] p = source.GetPixels();
        for (int i = 0; i < p.Length; i++)
        {
            if (p[i].a <= 0.01f) continue;
            float h, s, v;
            Color.RGBToHSV(p[i], out h, out s, out v);
            p[i] = Color.HSVToRGB(Mathf.Repeat(h + offset, 1f), s, v);
            p[i].a = p[i].a;
        }
        res.SetPixels(p); res.Apply(); return res;
    }

    private Texture2D ApplyForceColor(Texture2D source, float targetH)
    {
        Texture2D res = new Texture2D(source.width, source.height, TextureFormat.RGBA32, false);
        Color[] p = source.GetPixels();
        for (int i = 0; i < p.Length; i++)
        {
            if (p[i].a <= 0.01f) continue;
            float h, s, v;
            Color.RGBToHSV(p[i], out h, out s, out v);
            Color n = Color.HSVToRGB(targetH, s, v);
            n.a = p[i].a; p[i] = n;
        }
        res.SetPixels(p); res.Apply(); return res;
    }
}