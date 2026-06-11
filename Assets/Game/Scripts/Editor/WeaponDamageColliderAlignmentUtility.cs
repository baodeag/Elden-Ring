using System.IO;
using baodeag;
using UnityEditor;
using UnityEngine;

namespace baodeag.Editor
{
    public static class WeaponDamageColliderAlignmentUtility
    {
        private const string WeaponsFolder = GameAssetPaths.PrefabsRoot + "/Items/Weapons";

        [MenuItem("Tools/Weapons/Align Damage Colliders To Weapon Pivot")]
        public static void AlignDamageCollidersToWeaponPivot()
        {
            int changedCount = 0;
            string[] prefabGuids = AssetDatabase.FindAssets("t:Prefab", new[] { WeaponsFolder });

            foreach (string prefabGuid in prefabGuids)
            {
                string prefabPath = AssetDatabase.GUIDToAssetPath(prefabGuid);

                if (ShouldSkip(prefabPath))
                    continue;

                GameObject prefabRoot = PrefabUtility.LoadPrefabContents(prefabPath);

                try
                {
                    Transform weaponPivot = FindChildByName(prefabRoot.transform, "Weapon Pivot");
                    MeleeWeaponDamageCollider damageCollider = prefabRoot.GetComponentInChildren<MeleeWeaponDamageCollider>(true);

                    if (weaponPivot == null || damageCollider == null)
                        continue;

                    Transform damageTransform = damageCollider.transform;
                    bool changed = damageTransform.parent != weaponPivot ||
                                   damageTransform.localPosition != Vector3.zero ||
                                   damageTransform.localRotation != Quaternion.identity ||
                                   damageTransform.localScale != Vector3.one;

                    damageTransform.SetParent(weaponPivot, false);
                    damageTransform.localPosition = Vector3.zero;
                    damageTransform.localRotation = Quaternion.identity;
                    damageTransform.localScale = Vector3.one;

                    WeaponManager weaponManager = prefabRoot.GetComponent<WeaponManager>();
                    if (weaponManager != null)
                        weaponManager.meleeDamageCollider = damageCollider;

                    if (changed)
                    {
                        PrefabUtility.SaveAsPrefabAsset(prefabRoot, prefabPath);
                        changedCount++;
                    }
                }
                finally
                {
                    PrefabUtility.UnloadPrefabContents(prefabRoot);
                }
            }

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            
        }

        private static bool ShouldSkip(string prefabPath)
        {
            string fileName = Path.GetFileNameWithoutExtension(prefabPath);
            return fileName.Contains("Shield") || fileName.Contains("Bow") || fileName.Contains("Unarmed");
        }

        private static Transform FindChildByName(Transform parent, string childName)
        {
            foreach (Transform child in parent)
            {
                if (child.name == childName)
                    return child;

                Transform match = FindChildByName(child, childName);
                if (match != null)
                    return match;
            }

            return null;
        }
    }
}
