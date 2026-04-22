using UnityEditor;
using UnityEngine;

public static class Stylized3DMonsterUrpFixer
{
    private const string MaterialsFolder = "Assets/Stylized3DMonster";
    private const string UrpLitShaderName = "Universal Render Pipeline/Lit";
    private const string AutoRunSessionKey = "Stylized3DMonsterUrpFixer.AutoRunAttempted.V1";

    static Stylized3DMonsterUrpFixer()
    {
        EditorApplication.delayCall += TryAutoFixOnce;
    }

    [MenuItem("Tools/Fix/Convert Stylized3DMonster Materials To URP")]
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

            var baseMap = GetFirstTexture(material, "_MainTex", "_BaseMap", "_BASE_COLOR_MAP");
            var emissionMap = GetFirstTexture(material, "_EmissionMap", "_EMISSION_COLOR_MAP");
            var baseColor = GetFirstColor(material, "_BaseColor", "_Color", "_BASE_COLOR");
            var emissionColor = GetFirstColor(material, "_EmissionColor", "_EMISSION_COLOR");

            material.shader = shader;
            material.SetColor("_BaseColor", baseColor);
            material.SetFloat("_Surface", 0f);
            material.SetFloat("_AlphaClip", 0f);
            material.SetFloat("_Cull", 2f);
            material.SetFloat("_Smoothness", 0f);

            if (baseMap != null)
            {
                material.SetTexture("_BaseMap", baseMap);
            }

            if (emissionMap != null)
            {
                material.EnableKeyword("_EMISSION");
                material.globalIlluminationFlags = MaterialGlobalIlluminationFlags.RealtimeEmissive;
                material.SetTexture("_EmissionMap", emissionMap);
                material.SetColor("_EmissionColor", emissionColor.maxColorComponent > 0f ? emissionColor : Color.white * 0.25f);
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
        Debug.Log($"Converted {updatedCount} Stylized3DMonster materials to URP/Lit.");
    }

    private static void TryAutoFixOnce()
    {
        EditorApplication.delayCall -= TryAutoFixOnce;

        if (SessionState.GetBool(AutoRunSessionKey, false))
        {
            return;
        }

        SessionState.SetBool(AutoRunSessionKey, true);
        ConvertMaterialsToUrp();
    }

    private static Texture GetFirstTexture(Material material, params string[] propertyNames)
    {
        foreach (var propertyName in propertyNames)
        {
            if (!material.HasProperty(propertyName))
            {
                continue;
            }

            var texture = material.GetTexture(propertyName);
            if (texture != null)
            {
                return texture;
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
}
