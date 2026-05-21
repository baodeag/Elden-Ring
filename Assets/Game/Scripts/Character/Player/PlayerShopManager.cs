using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

namespace baodeag
{
    public class PlayerShopManager : MonoBehaviour
    {
        private PlayerManager player;

        private void Awake()
        {
            player = GetComponent<PlayerManager>();
        }

        public bool TryBuyItem(ShopStockEntry entry, ShopInventory shopInventory = null)
        {
            if (player == null || entry == null || entry.item == null)
                return false;

            if (!entry.item.canBePurchased)
                return false;

            int price = shopInventory != null ? shopInventory.GetBuyPrice(entry.item) : entry.GetBuyPrice();

            if (price < 0 || player.playerStatsManager.runes < price)
                return false;

            if (shopInventory != null && !shopInventory.TryPurchaseItem(entry.item))
                return false;

            Item purchasedItem = CreatePurchasedItem(entry.item);

            if (purchasedItem == null)
                return false;

            player.playerInventoryManager.AddItemToInventory(purchasedItem);
            player.playerStatsManager.AddRunes(-price);

            if (!player.IsServer)
                SyncBuyItemServerRpc(entry.item.itemID, price);

            TryAutoSave();
            RefreshOwnedPlayerUI();
            return true;
        }

        public bool TrySellItem(Item item, ShopInventory shopInventory = null)
        {
            if (player == null || item == null)
                return false;

            if (!item.canBeSold)
                return false;

            int sellPrice = shopInventory != null ? shopInventory.GetSellPrice(item) : Mathf.Max(0, item.sellPrice);

            if (!player.playerInventoryManager.TryRemoveItemFromInventory(item))
                return false;

            player.playerStatsManager.AddRunes(sellPrice);

            if (!player.IsServer)
                SyncSellItemServerRpc(item.itemID, sellPrice);

            TryAutoSave();
            RefreshOwnedPlayerUI();
            return true;
        }

        public List<Item> GetSellableInventoryItems()
        {
            List<Item> sellableItems = new List<Item>();

            if (player == null || player.playerInventoryManager.itemsInInventory == null)
                return sellableItems;

            for (int i = 0; i < player.playerInventoryManager.itemsInInventory.Count; i++)
            {
                Item item = player.playerInventoryManager.itemsInInventory[i];

                if (item == null || !item.canBeSold)
                    continue;

                sellableItems.Add(item);
            }

            return sellableItems;
        }

        public int GetOwnedAmount(Item item)
        {
            if (item == null || player == null)
                return 0;

            return player.playerInventoryManager.GetInventoryCountByItemID(item.itemID);
        }

        public int GetCurrentRunes()
        {
            if (player == null)
                return 0;

            return player.playerStatsManager.runes;
        }

        [ServerRpc]
        private void SyncBuyItemServerRpc(int itemID, int price)
        {
            Item purchasedItem = WorldItemDatabase.Instance.CreateItemInstance(itemID);

            if (purchasedItem == null)
                return;

            player.playerInventoryManager.AddItemToInventory(purchasedItem);
            player.playerStatsManager.AddRunes(-price);
            TryAutoSave();
        }

        [ServerRpc]
        private void SyncSellItemServerRpc(int itemID, int sellPrice)
        {
            if (!player.playerInventoryManager.RemoveFirstItemByID(itemID))
                return;

            player.playerStatsManager.AddRunes(sellPrice);
            TryAutoSave();
        }

        private Item CreatePurchasedItem(Item shopItem)
        {
            if (shopItem == null)
                return null;

            if (WorldItemDatabase.Instance != null)
            {
                Item purchasedItem = WorldItemDatabase.Instance.CreateItemInstance(shopItem.itemID);

                if (purchasedItem != null)
                    return purchasedItem;
            }

            // Fall back to the exact asset the merchant is selling if the database lookup is out of sync.
            return Instantiate(shopItem);
        }

        private void RefreshOwnedPlayerUI()
        {
            if (!player.IsOwner || PlayerUIManager.instance == null)
                return;

            PlayerUIManager.instance.playerUIHudManager.SetRunesCount(0);
            PlayerUIManager.instance.playerUIEquipmentManager.RefreshMenu();

            if (PlayerUIManager.instance.playerUIShopManager != null)
                PlayerUIManager.instance.playerUIShopManager.RefreshCurrentView();
        }

        private void TryAutoSave()
        {
            if (WorldSaveGameManager.instance == null)
                return;

            if (WorldSaveGameManager.instance.currentCharacterData == null)
                return;

            if (WorldSaveGameManager.instance.currentCharacterSlotBeingUsed == CharacterSlot.NO_SLOT)
                return;

            WorldSaveGameManager.instance.SaveGame();
        }
    }
}
