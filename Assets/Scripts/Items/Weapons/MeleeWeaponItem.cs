using UnityEngine;

namespace baodeag
{
    [CreateAssetMenu(menuName = "Items/Weapons/Melee Weapon")]
    public class MeleeWeaponItem : WeaponItem
    {
        [Header("Attack Modifiers")]
        public float riposte_Attack_01_Modifier = 3.3f;
        public float backstab_Attack_01_Modifier = 3.3f;
    }
}
