using UnityEngine;

namespace baodeag
{
    [System.Serializable]
    public class ShopStockEntry
    {
        public Item item;
        public int buyPriceOverride = -1;
        public int sellPriceOverride = -1;
        public int requiredProgressionTier = 1;
        public bool useLimitedQuantity = false;
        public int startingQuantity = 1;

        public int GetBuyPrice()
        {
            if (item == null)
                return 0;

            if (buyPriceOverride >= 0)
                return buyPriceOverride;

            return item.purchasePrice;
        }

        public int GetSellPrice()
        {
            if (item == null)
                return 0;

            if (sellPriceOverride >= 0)
                return sellPriceOverride;

            return item.sellPrice;
        }

        public ShopStockEntry GetRuntimeCopy()
        {
            return new ShopStockEntry
            {
                item = item,
                buyPriceOverride = buyPriceOverride,
                sellPriceOverride = sellPriceOverride,
                requiredProgressionTier = requiredProgressionTier,
                useLimitedQuantity = useLimitedQuantity,
                startingQuantity = startingQuantity
            };
        }
    }
}
