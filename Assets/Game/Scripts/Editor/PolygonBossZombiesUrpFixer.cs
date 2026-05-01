using UnityEditor;
using UnityEngine;

public static class PolygonBossZombiesUrpFixer
{
    private const string MaterialsFolder = GameAssetPaths.PolygonBossZombiesRoot + "/Materials";
    private const string UrpLitShaderName = "Universal Render Pipeline/Lit";

    [MenuItem("Tools/Fix/Convert Polygon Boss Zombies Materials To URP")]
    public static void ConvertMaterialsToUrp()
    {
        var shader = Shader.Find(UrpLitShaderName);
        if (shader == null)
        {
            Debug.LogError($"Could not find shader '{UrpLitShaderName}'. Make sure URP is installed.");
            return;
        }

        var materialGuids = AssetDatabase.FindAssets("t:Material", new[] { MaterialsFolder });
        var updatedCount = 0;

        foreach (var guid in materialGuids)
        {
            var path = AssetDatabase.GUIDToAssetPath(guid);
            var material = AssetDatabase.LoadAssetAtPath<Material>(path);
            if (material == null)
            {
                continue;
            }

            var baseMap = GetFirstTexture(material, "_Texture", "_MainTex");
            var emissionMap = GetFirstTexture(material, "_Emissive", "_EmissionMap");
            var baseColor = GetFirstColor(material, "_Color");
            var emissionColor = GetFirstColor(material, "_EmissiveColor", "_EmissionColor");
            var smoothness = GetFirstFloat(material, "_Smoothness", "_Glossiness");

            material.shader = shader;
            material.SetColor("_BaseColor", baseColor);

            if (baseMap != null)
            {
                material.SetTexture("_BaseMap", baseMap);
            }

            material.SetFloat("_Smoothness", smoothness);

            if (emissionMap != null)
            {
                material.EnableKeyword("_EMISSION");
                material.globalIlluminationFlags = MaterialGlobalIlluminationFlags.RealtimeEmissive;
                material.SetTexture("_EmissionMap", emissionMap);
                material.SetColor("_EmissionColor", emissionColor.maxColorComponent > 0f ? emissionColor : Color.black);
            }
            else
            {
                material.DisableKeyword("_EMISSION");
                material.SetColor("_EmissionColor", Color.black);
            }

            EditorUtility.SetDirty(material);
            updatedCount++;
        }

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        Debug.Log($"Converted {updatedCount} Polygon Boss Zombies materials to URP/Lit.");
    }

    private static Texture GetFirstTexture(Material material, params string[] propertyNames)
    {
        foreach (var propertyName in propertyNames)
        {
            if (material.HasProperty(propertyName))
            {
                var texture = material.GetTexture(propertyName);
                if (texture != null)
                {
                    return texture;
                }
            }
        }

        return null;
    }

    private static Color GetFirstColor(Material material, params string[] propertyNames)
    {
        foreach (var propertyName in propertyNames)
        {
            if (material.HasProperty(propertyName))
            {
                return material.GetColor(propertyName);
            }
        }

        return Color.white;
    }

    private static float GetFirstFloat(Material material, params string[] propertyNames)
    {
        foreach (var propertyName in propertyNames)
        {
            if (material.HasProperty(propertyName))
            {
                return material.GetFloat(propertyName);
            }
        }

        return 0f;
    }
}
