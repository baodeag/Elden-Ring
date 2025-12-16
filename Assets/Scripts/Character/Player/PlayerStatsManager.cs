using UnityEngine;

namespace baodeag
{
    public class PlayerStatsManager : CharacterStatsManager
    {
        PlayerManager player;
        protected override void Awake()
        {
            base.Awake();

            player = GetComponent<PlayerManager>();
        }

        protected override void Start()
        {
            base.Start();

            //when we make a character creation menu, and set the stas depending on the class, this will be calculated there
            //until then however, stats are never calculated, so we do it here
            CalculateHealthBasedOnVitalityLevel(player.playerNetworkManager.vitality.Value);
            CalculateStaminaBasedOnEnduranceLevel(player.playerNetworkManager.endurance.Value);
            CalculateFocusPointsBasedOnMindLevel(player.playerNetworkManager.mind.Value);
        }

        public void CalculateTotalArmorAbsorption()
        {
            //reset all values to 0
            armorPhysicalDamageAbsorption = 0;
            armorMagicDamageAbsorption = 0;
            armorFireDamageAbsorption = 0;
            armorLightningDamageAbsorption = 0;
            armorHolyDamageAbsorption = 0;

            armorImmunity = 0;
            armorRobustness = 0;
            armorFocus = 0;
            armorVitality = 0;

            basePoiseDefense = 0;

            //head equipment
            if (player.playerInventoryManager.headEquipment != null)
            {
                //damage resistances
                armorPhysicalDamageAbsorption += player.playerInventoryManager.headEquipment.physicalDamageAbsorption;
                armorMagicDamageAbsorption += player.playerInventoryManager.headEquipment.magicDamageAbsorption;
                armorFireDamageAbsorption += player.playerInventoryManager.headEquipment.fireDamageAbsorption;
                armorLightningDamageAbsorption += player.playerInventoryManager.headEquipment.lightningDamageAbsorption;
                armorHolyDamageAbsorption += player.playerInventoryManager.headEquipment.holyDamageAbsorption;

                //status effect resistances
                armorImmunity += player.playerInventoryManager.headEquipment.immunity;
                armorRobustness += player.playerInventoryManager.headEquipment.robustness;
                armorFocus += player.playerInventoryManager.headEquipment.focus;
                armorVitality += player.playerInventoryManager.headEquipment.vitality;

                //poise
                basePoiseDefense += player.playerInventoryManager.headEquipment.poise;
            }

            //body equipment
            if (player.playerInventoryManager.bodyEquipment != null)
            {
                //damage resistances
                armorPhysicalDamageAbsorption += player.playerInventoryManager.bodyEquipment.physicalDamageAbsorption;
                armorMagicDamageAbsorption += player.playerInventoryManager.bodyEquipment.magicDamageAbsorption;
                armorFireDamageAbsorption += player.playerInventoryManager.bodyEquipment.fireDamageAbsorption;
                armorLightningDamageAbsorption += player.playerInventoryManager.bodyEquipment.lightningDamageAbsorption;
                armorHolyDamageAbsorption += player.playerInventoryManager.bodyEquipment.holyDamageAbsorption;

                //status effect resistances
                armorImmunity += player.playerInventoryManager.bodyEquipment.immunity;
                armorRobustness += player.playerInventoryManager.bodyEquipment.robustness;
                armorFocus += player.playerInventoryManager.bodyEquipment.focus;
                armorVitality += player.playerInventoryManager.bodyEquipment.vitality;

                //poise
                basePoiseDefense += player.playerInventoryManager.bodyEquipment.poise;
            }

            //leg equipment
            if (player.playerInventoryManager.legEquipment != null)
            {
                //damage resistances
                armorPhysicalDamageAbsorption += player.playerInventoryManager.legEquipment.physicalDamageAbsorption;
                armorMagicDamageAbsorption += player.playerInventoryManager.legEquipment.magicDamageAbsorption;
                armorFireDamageAbsorption += player.playerInventoryManager.legEquipment.fireDamageAbsorption;
                armorLightningDamageAbsorption += player.playerInventoryManager.legEquipment.lightningDamageAbsorption;
                armorHolyDamageAbsorption += player.playerInventoryManager.legEquipment.holyDamageAbsorption;

                //status effect resistances
                armorImmunity += player.playerInventoryManager.legEquipment.immunity;
                armorRobustness += player.playerInventoryManager.legEquipment.robustness;
                armorFocus += player.playerInventoryManager.legEquipment.focus;
                armorVitality += player.playerInventoryManager.legEquipment.vitality;

                //poise
                basePoiseDefense += player.playerInventoryManager.legEquipment.poise;
            }

            //hand equipment
            if (player.playerInventoryManager.handEquipment != null)
            {
                //damage resistances
                armorPhysicalDamageAbsorption += player.playerInventoryManager.handEquipment.physicalDamageAbsorption;
                armorMagicDamageAbsorption += player.playerInventoryManager.handEquipment.magicDamageAbsorption;
                armorFireDamageAbsorption += player.playerInventoryManager.handEquipment.fireDamageAbsorption;
                armorLightningDamageAbsorption += player.playerInventoryManager.handEquipment.lightningDamageAbsorption;
                armorHolyDamageAbsorption += player.playerInventoryManager.handEquipment.holyDamageAbsorption;

                //status effect resistances
                armorImmunity += player.playerInventoryManager.handEquipment.immunity;
                armorRobustness += player.playerInventoryManager.handEquipment.robustness;
                armorFocus += player.playerInventoryManager.handEquipment.focus;
                armorVitality += player.playerInventoryManager.handEquipment.vitality;

                //poise
                basePoiseDefense += player.playerInventoryManager.handEquipment.poise;
            }
        }
    }
}
