using System.Collections.Generic;
using UnityEngine;

namespace baodeag
{
    public class ShopInventory : MonoBehaviour
    {
        [Header("Shop")]
        public string shopName = "Merchant Shop";
        [SerializeField] private string merchantID = "merchant_default";
        [SerializeField] private int shopProgressionTier = 1;
        [SerializeField] private bool autoScaleShopTierFromProgression = true;
        [SerializeField] private int shopTierOffset = 0;
        [SerializeField] private bool useGlobalPurchasableItems = true;
        [SerializeField] private List<ShopStockEntry> customStock = new List<ShopStockEntry>();

        [Header("Progression Pricing")]
        [SerializeField] private float buyPriceIncreasePerTier = 0.2f;
        [SerializeField] private float sellPriceIncreasePerTier = 0.05f;

        public List<ShopStockEntry> GetStockEntries()
        {
            List<ShopStockEntry> stockEntries = useGlobalPurchasableItems ? BuildGlobalStockEntries() : customStock;
            List<ShopStockEntry> visibleEntries = new List<ShopStockEntry>();
            int playerProgressionTier = GetCurrentPlayerProgressionTier();

            for (int i = 0; i < stockEntries.Count; i++)
            {
                ShopStockEntry entry = stockEntries[i];

                if (entry == null || entry.item == null)
                    continue;

                if (entry.requiredProgressionTier > playerProgressionTier)
                    continue;

                if (entry.useLimitedQuantity && GetRemainingQuantity(entry) <= 0)
                    continue;

                visibleEntries.Add(entry.GetRuntimeCopy());
            }

            return visibleEntries;
        }

        public int GetBuyPrice(Item item)
        {
            ShopStockEntry entry = GetEntryForItem(item);

            if (entry == null)
                return 0;

            int basePrice = entry.GetBuyPrice();
            float progressionMultiplier = 1f + Mathf.Max(0, GetEffectiveShopProgressionTier() - 1) * buyPriceIncreasePerTier;
            return Mathf.Max(0, Mathf.RoundToInt(basePrice * progressionMultiplier));
        }

        public int GetSellPrice(Item item)
        {
            ShopStockEntry entry = GetEntryForItem(item);

            int basePrice = entry != null ? entry.GetSellPrice() : item != null ? item.sellPrice : 0;
            float progressionMultiplier = 1f + Mathf.Max(0, GetEffectiveShopProgressionTier() - 1) * sellPriceIncreasePerTier;
            return Mathf.Max(0, Mathf.RoundToInt(basePrice * progressionMultiplier));
        }

        public int GetEffectiveShopProgressionTier()
        {
            if (!autoScaleShopTierFromProgression)
                return Mathf.Max(1, shopProgressionTier);

            return Mathf.Max(1, GetCurrentPlayerProgressionTier() + shopTierOffset);
        }

        public int GetRemainingQuantity(Item item)
        {
            ShopStockEntry entry = GetEntryForItem(item);
            return GetRemainingQuantity(entry);
        }

        public bool TryPurchaseItem(Item item)
        {
            ShopStockEntry entry = GetEntryForItem(item);

            if (entry == null)
                return false;

            if (!entry.useLimitedQuantity)
                return true;

            int remainingQuantity = GetRemainingQuantity(entry);

            if (remainingQuantity <= 0)
                return false;

            SetRemainingQuantity(entry, remainingQuantity - 1);
            return true;
        }

        private ShopStockEntry GetEntryForItem(Item item)
        {
            if (item == null)
                return null;

            List<ShopStockEntry> stockEntries = useGlobalPurchasableItems ? BuildGlobalStockEntries() : customStock;

            for (int i = 0; i < stockEntries.Count; i++)
            {
                if (stockEntries[i] == null || stockEntries[i].item == null)
                    continue;

                if (stockEntries[i].item.itemID == item.itemID)
                    return stockEntries[i];
            }

            return null;
        }

        private List<ShopStockEntry> BuildGlobalStockEntries()
        {
            List<ShopStockEntry> globalStock = new List<ShopStockEntry>();
            List<Item> items = WorldItemDatabase.Instance.GetPurchasableItems();

            for (int i = 0; i < items.Count; i++)
            {
                ShopStockEntry entry = new ShopStockEntry();
                entry.item = items[i];
                globalStock.Add(entry);
            }

            return globalStock;
        }

        private int GetRemainingQuantity(ShopStockEntry entry)
        {
            if (entry == null || !entry.useLimitedQuantity)
                return -1;

            CharacterSaveData currentCharacterData = GetCurrentCharacterData();

            if (currentCharacterData == null)
                return Mathf.Max(0, entry.startingQuantity);

            currentCharacterData.EnsureCollectionsInitialized();
            string saveKey = GetStockSaveKey(entry.item.itemID);

            if (currentCharacterData.merchantStockRemaining.ContainsKey(saveKey))
                return currentCharacterData.merchantStockRemaining[saveKey];

            return Mathf.Max(0, entry.startingQuantity);
        }

        private void SetRemainingQuantity(ShopStockEntry entry, int remainingQuantity)
        {
            if (entry == null || !entry.useLimitedQuantity || entry.item == null)
                return;

            CharacterSaveData currentCharacterData = GetCurrentCharacterData();

            if (currentCharacterData == null)
                return;

            currentCharacterData.EnsureCollectionsInitialized();
            string saveKey = GetStockSaveKey(entry.item.itemID);
            int clampedQuantity = Mathf.Clamp(remainingQuantity, 0, Mathf.Max(0, entry.startingQuantity));

            if (currentCharacterData.merchantStockRemaining.ContainsKey(saveKey))
                currentCharacterData.merchantStockRemaining[saveKey] = clampedQuantity;
            else
                currentCharacterData.merchantStockRemaining.Add(saveKey, clampedQuantity);
        }

        private CharacterSaveData GetCurrentCharacterData()
        {
            if (WorldSaveGameManager.instance == null)
                return null;

            return WorldSaveGameManager.instance.currentCharacterData;
        }

        private string GetStockSaveKey(int itemID)
        {
            return GetResolvedMerchantID() + "_" + itemID;
        }

        private string GetResolvedMerchantID()
        {
            if (!string.IsNullOrWhiteSpace(merchantID))
                return merchantID;

            return gameObject.scene.name + "_" + gameObject.name;
        }

        private int GetCurrentPlayerProgressionTier()
        {
            if (GameProgressionManager.instance != null)
                return Mathf.Max(1, GameProgressionManager.Instance.CurrentMapIndex + 1);

            CharacterSaveData currentCharacterData = GetCurrentCharacterData();

            if (currentCharacterData == null)
                return 1;

            currentCharacterData.EnsureCollectionsInitialized();

            int defeatedBossCount = 0;

            foreach (var bossState in currentCharacterData.bossesDefeated)
            {
                if (bossState.Value)
                    defeatedBossCount += 1;
            }

            return Mathf.Max(1, defeatedBossCount + 1);
        }
    }
}
