using UnityEngine;

namespace baodeag
{
    public class BuffCharmItem : QuickSlotItem
    {
        [Header("Buff")]
        [SerializeField] private float buffDurationSeconds = 30f;
        [SerializeField] private int maxHealthBonus = 0;
        [SerializeField] private int maxStaminaBonus = 0;
        [SerializeField] private int maxFocusPointsBonus = 0;
        [SerializeField] private float staminaRegenerationBonusPercentage = 0f;
        [SerializeField] private float outgoingDamageBonusPercentage = 0f;

        public void InitializeRuntimeBuff(
            string runtimeItemName,
            string runtimeItemDescription,
            Sprite runtimeIcon,
            float durationSeconds,
            int healthBonus,
            int staminaBonus,
            int focusBonus,
            float staminaRegenBonusPercent,
            float damageBonusPercent,
            int startingAmount,
            int maxAmount,
            int runtimePurchasePrice,
            int runtimeSellPrice,
            string runtimeAnimation = "Item_Flask_Drink_Start_01")
        {
            itemName = runtimeItemName;
            itemDescription = runtimeItemDescription;
            itemIcon = runtimeIcon;
            maxItemAmount = maxAmount;
            currentItemAmount = startingAmount;
            itemAmount = startingAmount;
            canBePurchased = true;
            canBeSold = true;
            purchasePrice = runtimePurchasePrice;
            sellPrice = runtimeSellPrice;
            isConsumable = true;
            useItemAnimation = runtimeAnimation;

            buffDurationSeconds = durationSeconds;
            maxHealthBonus = healthBonus;
            maxStaminaBonus = staminaBonus;
            maxFocusPointsBonus = focusBonus;
            staminaRegenerationBonusPercentage = staminaRegenBonusPercent;
            outgoingDamageBonusPercentage = damageBonusPercent;
        }

        public override bool CanIUseThisItem(PlayerManager player)
        {
            if (!player.playerCombatManager.isUsingItem && player.isPerformingAction)
                return false;

            if (player.playerNetworkManager.isAttacking.Value)
                return false;

            return itemAmount > 0;
        }

        public override void AttemptToUseItem(PlayerManager player)
        {
            if (!CanIUseThisItem(player))
                return;

            if (player.playerCombatManager.isUsingItem)
                return;

            player.playerCombatManager.isUsingItem = true;

            if (player.playerEffectsManager.activeQuickSlotItemFX != null)
                Destroy(player.playerEffectsManager.activeQuickSlotItemFX);

            if (itemModel != null)
                player.playerEffectsManager.activeQuickSlotItemFX = Instantiate(itemModel, player.playerEquipmentManager.rightHandWeaponSlot.transform);

            if (player.IsOwner)
            {
                string animationToPlay = string.IsNullOrWhiteSpace(useItemAnimation) ? "Item_Flask_Drink_Start_01" : useItemAnimation;
                player.playerAnimatorManager.PlayTargetActionAnimation(animationToPlay, false, false, true, true, false);
                player.playerNetworkManager.HideWeaponsServerRPC();
            }
        }

        public override void SuccessfullyUseItem(PlayerManager player)
        {
            base.SuccessfullyUseItem(player);

            if (player.IsOwner)
            {
                PlayerStatBuffTimedEffect effect = CreateEffectInstance();
                player.playerEffectsManager.AddTimedEffect(effect);

                if (PlayerUIManager.instance != null)
                {
                    if (PlayerUIManager.instance.playerUIPopUpManager != null)
                        PlayerUIManager.instance.playerUIPopUpManager.SendBuffPopUp(this);

                    if (PlayerUIManager.instance.playerUIHudManager != null)
                        PlayerUIManager.instance.playerUIHudManager.ShowActiveBuff(this);
                }

                if (isConsumable)
                {
                    itemAmount = Mathf.Max(0, itemAmount - 1);

                    if (itemAmount <= 0)
                    {
                        int currentIndex = player.playerInventoryManager.quickSlotItemIndex;

                        if (currentIndex >= 0 && currentIndex < player.playerInventoryManager.quickSlotItemsInQuickSlots.Length)
                            player.playerInventoryManager.quickSlotItemsInQuickSlots[currentIndex] = null;

                        player.playerEquipmentManager.RefreshCurrentQuickSlotSelection();
                    }
                    else
                    {
                        PlayerUIManager.instance.playerUIHudManager.SetQuickSlotItemQuickSlotIcon(player.playerInventoryManager.currentQuickSlotItem);
                    }
                }
                else
                {
                    PlayerUIManager.instance.playerUIHudManager.SetQuickSlotItemQuickSlotIcon(player.playerInventoryManager.currentQuickSlotItem);
                }
            }

            if (WorldCharacterEffectsManager.instance != null && WorldCharacterEffectsManager.instance.healingFlaskVFX != null)
                Instantiate(WorldCharacterEffectsManager.instance.healingFlaskVFX, player.transform);

            if (player.characterSoundFXManager != null && WorldSoundFXManager.instance != null && WorldSoundFXManager.instance.healingFlaskSFX != null)
                player.characterSoundFXManager.PlaySoundFX(WorldSoundFXManager.instance.healingFlaskSFX);
        }

        public PlayerStatBuffTimedEffect CreateEffectInstance()
        {
            PlayerStatBuffTimedEffect effect = ScriptableObject.CreateInstance<PlayerStatBuffTimedEffect>();
            effect.sourceItemID = itemID;
            effect.effectID = 100000 + itemID;
            effect.defaultLengthOfEffect = Mathf.Max(1f, buffDurationSeconds);
            effect.timeRemainingOnEffect = effect.defaultLengthOfEffect;
            effect.maxHealthBonus = maxHealthBonus;
            effect.maxStaminaBonus = maxStaminaBonus;
            effect.maxFocusPointsBonus = maxFocusPointsBonus;
            effect.staminaRegenerationBonusPercentage = staminaRegenerationBonusPercentage;
            effect.outgoingDamageBonusPercentage = outgoingDamageBonusPercentage;
            return effect;
        }
    }
}
