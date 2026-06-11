using UnityEditor;
using UnityEngine;

public static class UndeadDummyModelReplacer
{
    private const string TargetPrefabPath = GameAssetPaths.PrefabsRoot + "/Character/Undead/Undead_Dummy_01.prefab";
    private const string ReplacementPrefabPath = GameAssetPaths.PolygonBossZombiesRoot + "/Prefabs/SM_Chr_ZombieBoss_Wretch_01.prefab";
    private const string OldMeshObjectName = "Md_Char_Low_Poly_Man";
    private const string OldRigRootName = "Root";
    private const string AutoRunSessionKey = "UndeadDummyModelReplacer.AutoRunAttempted";

    static UndeadDummyModelReplacer()
    {
        // Disabled autorun to avoid mutating editor state on startup.
    }

    [MenuItem("Tools/Fix/Replace Undead Dummy Model With Wretch")]
    public static void ReplaceUndeadDummyModel()
    {
        var targetPrefab = AssetDatabase.LoadAssetAtPath<GameObject>(TargetPrefabPath);
        var replacementPrefab = AssetDatabase.LoadAssetAtPath<GameObject>(ReplacementPrefabPath);

        if (targetPrefab == null || replacementPrefab == null)
        {
            
            return;
        }

        var prefabRoot = PrefabUtility.LoadPrefabContents(TargetPrefabPath);

        try
        {
            if (HasReplacementAlready(prefabRoot.transform, replacementPrefab.name))
            {
                return;
            }

            DestroyImmediateChild(prefabRoot.transform, OldMeshObjectName);
            DestroyImmediateChild(prefabRoot.transform, OldRigRootName);

            var replacementInstance = PrefabUtility.InstantiatePrefab(replacementPrefab, prefabRoot.scene) as GameObject;
            if (replacementInstance == null)
            {
                
                return;
            }

            replacementInstance.name = replacementPrefab.name;
            replacementInstance.transform.SetParent(prefabRoot.transform, false);
            replacementInstance.transform.localPosition = Vector3.zero;
            replacementInstance.transform.localRotation = Quaternion.identity;
            replacementInstance.transform.localScale = Vector3.one;

            var replacementAnimator = replacementInstance.GetComponent<Animator>();
            var rootAnimator = prefabRoot.GetComponent<Animator>();

            if (rootAnimator != null && replacementAnimator != null)
            {
                rootAnimator.avatar = replacementAnimator.avatar;
            }

            if (replacementAnimator != null)
            {
                Object.DestroyImmediate(replacementAnimator, true);
            }

            SetLayerRecursively(replacementInstance.transform, prefabRoot.layer);

            PrefabUtility.SaveAsPrefabAsset(prefabRoot, TargetPrefabPath);
            
        }
        finally
        {
            PrefabUtility.UnloadPrefabContents(prefabRoot);
        }

        baodeag.EditorTools.UndeadDummyGameplayFixer.FixUndeadDummyGameplayHooks();
    }

    private static void TryAutoReplaceOnce()
    {
        if (SessionState.GetBool(AutoRunSessionKey, false))
        {
            return;
        }

        SessionState.SetBool(AutoRunSessionKey, true);
        ReplaceUndeadDummyModel();
    }

    private static bool HasReplacementAlready(Transform root, string replacementName)
    {
        return root.Find(replacementName) != null && root.Find(OldMeshObjectName) == null && root.Find(OldRigRootName) == null;
    }

    private static void DestroyImmediateChild(Transform root, string childName)
    {
        var child = root.Find(childName);
        if (child != null)
        {
            Object.DestroyImmediate(child.gameObject, true);
        }
    }

    private static void SetLayerRecursively(Transform root, int layer)
    {
        root.gameObject.layer = layer;

        foreach (Transform child in root)
        {
            SetLayerRecursively(child, layer);
        }
    }
}
