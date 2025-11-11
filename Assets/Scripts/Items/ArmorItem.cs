using UnityEngine;
using UnityEngine.Rendering;

namespace baodeag
{
    public class ArmorItem : EquipmentItem
    {
        [Header("Equipment Absorption Bonus")]
        public float physicalDamageAbsorption;
        public float magicDamageAbsorption;
        public float fireDamageAbsorption;
        public float lightningDamageAbsorption;
        public float holyDamageAbsorption;

        [Header("Equipment Resistance Bonus")]
        public float immunity; //resistance to rot and poison
        public float robustness; // bleed and frost
        public float focus; // madness and sleep
        public float vitality; // death curse

        [Header("Poise")]
        public float poise;

        public EquipmentModel[] equipmentModels;
    }
}
