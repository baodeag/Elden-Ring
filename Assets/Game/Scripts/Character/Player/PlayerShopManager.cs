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
            BuildRuntimeLogger.Log($"[InventoryTrace] TryBuyItem begin owner={player != null && player.IsOwner} server={player != null && player.IsServer} item={(entry != null && entry.item != null ? entry.item.itemName : "null")} id={(entry != null && entry.item != null ? entry.item.itemID : -1)} type={(entry != null && entry.item != null ? entry.item.GetType().Name : "null")}");

            if (player == null || entry == null || entry.item == null)
                return false;

            if (!entry.item.canBePurchased)
                return false;

            if (WorldItemDatabase.Instance != null)
                WorldItemDatabase.Instance.RegisterItemTemplate(entry.item);

            int price = shopInventory != null ? shopInventory.GetBuyPrice(entry.item) : entry.GetBuyPrice();

            if (price < 0 || player.playerStatsManager.runes < price)
                return false;

            if (shopInventory != null && !shopInventory.TryPurchaseItem(entry.item))
                return false;

            Item purchasedItem = CreatePurchasedItem(entry.item);

            if (purchasedItem == null)
                return false;

            player.playerInventoryManager.AddItemToInventory(purchasedItem);
            BuildRuntimeLogger.Log($"[InventoryTrace] TryBuyItem added item={purchasedItem.itemName} id={purchasedItem.itemID} type={purchasedItem.GetType().Name} inventoryCount={player.playerInventoryManager.itemsInInventory?.Count ?? -1} ownedAmount={player.playerInventoryManager.GetInventoryCountByItemID(purchasedItem.itemID)}");
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
                WorldItemDatabase.Instance.RegisterItemTemplate(shopItem);
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
            if (player == null || !player.IsOwner)
            {
                BuildRuntimeLogger.Log($"[InventoryTrace] Shop TryAutoSave skipped owner={player != null && player.IsOwner} playerNull={player == null}");
                return;
            }

            if (WorldSaveGameManager.instance == null)
            {
                BuildRuntimeLogger.Log("[InventoryTrace] Shop TryAutoSave skipped WorldSaveGameManager null");
                return;
            }

            if (WorldSaveGameManager.instance.currentCharacterData == null)
            {
                BuildRuntimeLogger.Log("[InventoryTrace] Shop TryAutoSave skipped currentCharacterData null");
                return;
            }

            if (WorldSaveGameManager.instance.currentCharacterSlotBeingUsed == CharacterSlot.NO_SLOT)
            {
                BuildRuntimeLogger.Log("[InventoryTrace] Shop TryAutoSave skipped NO_SLOT");
                return;
            }

            BuildRuntimeLogger.Log($"[InventoryTrace] Shop TryAutoSave saving player={player.name} inventoryCount={player.playerInventoryManager.itemsInInventory?.Count ?? -1}");
            WorldSaveGameManager.instance.SaveGame(player);
        }
    }
}
