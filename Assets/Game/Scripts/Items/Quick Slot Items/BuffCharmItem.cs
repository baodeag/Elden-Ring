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

        [Header("Use VFX")]
        [SerializeField] private GameObject useItemVFX;

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

        public void SetRuntimeUseItemVFX(GameObject runtimeUseItemVFX)
        {
            useItemVFX = runtimeUseItemVFX;
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

                        if (PlayerUIManager.instance != null && PlayerUIManager.instance.playerUIHudManager != null)
                            PlayerUIManager.instance.playerUIHudManager.SetQuickSlotItemQuickSlotIcon(null);

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

        private GameObject GetBuffUseVFXPrefab()
        {
            if (useItemVFX != null)
                return useItemVFX;

            if (WorldCharacterEffectsManager.instance == null)
                return null;

            if (string.IsNullOrWhiteSpace(itemName))
                return WorldCharacterEffectsManager.instance.healingFlaskVFX;

            string normalizedName = itemName.ToLowerInvariant();

            if (normalizedName.Contains("guardian"))
                return WorldCharacterEffectsManager.instance.guardianBuffPotionVFX;

            if (normalizedName.Contains("wind"))
                return WorldCharacterEffectsManager.instance.windBuffPotionVFX;

            if (normalizedName.Contains("sage"))
                return WorldCharacterEffectsManager.instance.sageBuffPotionVFX;

            if (normalizedName.Contains("war"))
                return WorldCharacterEffectsManager.instance.warBuffPotionVFX;

            return WorldCharacterEffectsManager.instance.healingFlaskVFX;
        }

        public override void PlayUseItemFX(PlayerManager player)
        {
            if (player == null)
                return;

            GameObject buffVFX = GetBuffUseVFXPrefab();

            if (buffVFX == null && WorldCharacterEffectsManager.instance != null)
                buffVFX = WorldCharacterEffectsManager.instance.healingFlaskVFX;

            if (buffVFX != null)
                Instantiate(buffVFX, player.transform);

            if (player.characterSoundFXManager != null && WorldSoundFXManager.instance != null && WorldSoundFXManager.instance.healingFlaskSFX != null)
                player.characterSoundFXManager.PlaySoundFX(WorldSoundFXManager.instance.healingFlaskSFX);
        }
    }
}
