using UnityEngine;

namespace baodeag
{
    public class PlayerStatBuffTimedEffect : TimedCharacterEffect
    {
        [Header("Source")]
        public int sourceItemID = -1;

        [Header("Bonuses")]
        public int maxHealthBonus;
        public int maxStaminaBonus;
        public int maxFocusPointsBonus;
        public float staminaRegenerationBonusPercentage;
        public float outgoingDamageBonusPercentage;

        [Header("Runtime")]
        [SerializeField] private bool effectHasBeenInitialized = false;

        public override void ProcessEffect(CharacterManager character)
        {
            if (!effectHasBeenInitialized)
                ApplyEffect(character);

            base.ProcessEffect(character);
        }

        public override void RemoveEffect(CharacterManager character)
        {
            base.RemoveEffect(character);

            if (!effectHasBeenInitialized)
                return;

            if (!character.IsOwner)
                return;

            PlayerManager player = character as PlayerManager;

            if (player == null)
                return;

            player.playerStatsManager.maxHealthBuff += -maxHealthBonus;
            player.playerStatsManager.maxStaminaBuff += -maxStaminaBonus;
            player.playerStatsManager.maxFocusPointsBuff += -maxFocusPointsBonus;
            player.playerStatsManager.outgoingDamageBonusPercentage += -outgoingDamageBonusPercentage;
            player.playerNetworkManager.staminaRegenerationModifier.Value -= staminaRegenerationBonusPercentage;
            player.playerStatsManager.RefreshDerivedStats();

            if (PlayerUIManager.instance != null && PlayerUIManager.instance.localPlayer == player)
                PlayerUIManager.instance.playerUIHudManager?.HideActiveBuff(sourceItemID);

            effectHasBeenInitialized = false;
        }

        private void ApplyEffect(CharacterManager character)
        {
            if (!character.IsOwner)
                return;

            PlayerManager player = character as PlayerManager;

            if (player == null)
                return;

            effectHasBeenInitialized = true;

            player.playerStatsManager.maxHealthBuff += maxHealthBonus;
            player.playerStatsManager.maxStaminaBuff += maxStaminaBonus;
            player.playerStatsManager.maxFocusPointsBuff += maxFocusPointsBonus;
            player.playerStatsManager.outgoingDamageBonusPercentage += outgoingDamageBonusPercentage;
            player.playerNetworkManager.staminaRegenerationModifier.Value += staminaRegenerationBonusPercentage;
            player.playerStatsManager.RefreshDerivedStats();
        }
    }
}
