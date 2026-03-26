using System.Collections.Generic;
using UnityEngine;

namespace baodeag
{
    public class ShopInventory : MonoBehaviour
    {
        [Header("Shop")]
        public string shopName = "Merchant Shop";
        [SerializeField] private bool useGlobalPurchasableItems = true;
        [SerializeField] private List<ShopStockEntry> customStock = new List<ShopStockEntry>();

        public List<ShopStockEntry> GetStockEntries()
        {
            if (!useGlobalPurchasableItems)
                return customStock;

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
    }
}
