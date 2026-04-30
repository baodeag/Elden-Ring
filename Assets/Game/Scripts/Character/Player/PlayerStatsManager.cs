using Unity.Netcode;
using UnityEngine;

namespace baodeag
{
    public class PlayerStatsManager : CharacterStatsManager
    {
        PlayerManager player;

        [Header("Runes")]
        public int runes = 0;

        [Header("Buffs")]
        public int maxHealthBuff = 0;
        public int maxStaminaBuff = 0;
        public int maxFocusPointsBuff = 0;
        public float outgoingDamageBonusPercentage = 0f;

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
            CalculateHealthBasedOnVitalityLevel(player.playerNetworkManager.vigor.Value);
            CalculateStaminaBasedOnEnduranceLevel(player.playerNetworkManager.endurance.Value);
            CalculateFocusPointsBasedOnMindLevel(player.playerNetworkManager.mind.Value);
        }

        public int CalculateModifiedMaxHealth()
        {
            return Mathf.Max(1, CalculateHealthBasedOnVitalityLevel(player.playerNetworkManager.vigor.Value) + maxHealthBuff);
        }

        public int CalculateModifiedMaxStamina()
        {
            return Mathf.Max(1, CalculateStaminaBasedOnEnduranceLevel(player.playerNetworkManager.endurance.Value) + maxStaminaBuff);
        }

        public int CalculateModifiedMaxFocusPoints()
        {
            return Mathf.Max(0, CalculateFocusPointsBasedOnMindLevel(player.playerNetworkManager.mind.Value) + maxFocusPointsBuff);
        }

        public float GetOutgoingDamageMultiplier()
        {
            return Mathf.Max(0.1f, 1f + (outgoingDamageBonusPercentage / 100f));
        }

        public void RefreshDerivedStats()
        {
            if (!player.IsOwner)
                return;

            int oldMaxHealth = player.playerNetworkManager.maxHealth.Value;
            int oldMaxStamina = player.playerNetworkManager.maxStamina.Value;
            int oldMaxFocusPoints = player.playerNetworkManager.maxFocusPoints.Value;

            int newMaxHealth = CalculateModifiedMaxHealth();
            int newMaxStamina = CalculateModifiedMaxStamina();
            int newMaxFocusPoints = CalculateModifiedMaxFocusPoints();

            player.playerNetworkManager.maxHealth.Value = newMaxHealth;
            player.playerNetworkManager.maxStamina.Value = newMaxStamina;
            player.playerNetworkManager.maxFocusPoints.Value = newMaxFocusPoints;

            player.playerNetworkManager.currentHealth.Value = Mathf.Clamp(player.playerNetworkManager.currentHealth.Value + (newMaxHealth - oldMaxHealth), 0, newMaxHealth);
            player.playerNetworkManager.currentStamina.Value = Mathf.Clamp(player.playerNetworkManager.currentStamina.Value + (newMaxStamina - oldMaxStamina), 0, newMaxStamina);
            player.playerNetworkManager.currentFocusPoints.Value = Mathf.Clamp(player.playerNetworkManager.currentFocusPoints.Value + (newMaxFocusPoints - oldMaxFocusPoints), 0, newMaxFocusPoints);

            if (player.IsOwner && PlayerUIManager.instance != null)
            {
                PlayerUIManager.instance.playerUIHudManager.SetMaxHealthValue(newMaxHealth);
                PlayerUIManager.instance.playerUIHudManager.SetMaxStaminaValue(newMaxStamina);
                PlayerUIManager.instance.playerUIHudManager.SetMaxFocusPointValue(newMaxFocusPoints);
            }

            if (player.playerEquipmentManager != null)
                player.playerEquipmentManager.RefreshWeaponDamage();
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

        public void AddRunes(int runesToAdd)
        {
            runes += runesToAdd;

            if (player != null && player.IsOwner && PlayerUIManager.instance != null)
                PlayerUIManager.instance.playerUIHudManager.SetRunesCount(runesToAdd);
        }
    }
}
