using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;

namespace baodeag
{
    public class PlayerInventoryManager : CharacterInventoryManager
    {
        [Header("Weapons")]
        public WeaponItem currentRightHandWeapon;
        public WeaponItem currentLeftHandWeapon;
        public WeaponItem currentTwoHandWeapon;

        [Header("Quick Slots")]
        public WeaponItem[] weaponsInRightHandSlots = new WeaponItem[3];
        public int rightHandWeaponIndex = 0;
        public WeaponItem[] weaponsInLeftHandSlots = new WeaponItem[3];
        public int leftHandWeaponIndex = 0;
        public SpellItem currentSpell;
        public QuickSlotItem[] quickSlotItemsInQuickSlots = new QuickSlotItem[3];
        public int quickSlotItemIndex = 0;
        public QuickSlotItem currentQuickSlotItem;

        [Header("Armor")]
        public HeadEquipmentItem headEquipment;
        public BodyEquipmentItem bodyEquipment;
        public LegEquipmentItem legEquipment;
        public HandEquipmentItem handEquipment;

        [Header("Pojectiles")]
        public RangedProjectileItem mainProjectile;
        public RangedProjectileItem secondaryProjectile;

        [Header("Inventory")]
        public List<Item> itemsInInventory;

        protected override void Awake()
        {
            base.Awake();

            if (itemsInInventory == null)
                itemsInInventory = new List<Item>();
        }

        public void AddItemToInventory(Item item)
        {
            if (item == null)
                return;

            if (itemsInInventory == null)
                itemsInInventory = new List<Item>();

            itemsInInventory.Add(item);
        }

        public void RemoveItemFromInventory(Item item)
        {
            if (item == null || itemsInInventory == null)
                return;

            bool isStackable = false;

            if (item.maxItemAmount > 1)
                isStackable = true;

            //if the item is stackable, attempt to remove from the stack first
            if (isStackable)
            {
                for (int i = itemsInInventory.Count - 1; i > -1; i--)
                {
                    if (itemsInInventory[i].itemID == item.itemID)
                    {
                        itemsInInventory[i].currentItemAmount -= item.currentItemAmount;

                        if (itemsInInventory[i].currentItemAmount <= 0)
                            itemsInInventory.Remove(item);
                    }
                }
            }
            //otherwise simply remove it from the inventory
            else
            {
                itemsInInventory.Remove(item);
            }

            //check for null lists slot and remove them
            for (int i = itemsInInventory.Count - 1; i > -1; i--)
            {
                if (itemsInInventory[i] == null)
                {
                    itemsInInventory.RemoveAt(i);
                }
            }
        }

        public bool TryRemoveItemFromInventory(Item item)
        {
            if (item == null || itemsInInventory == null)
                return false;

            int inventoryCountBeforeRemoval = GetInventoryCountByItemID(item.itemID);

            if (inventoryCountBeforeRemoval <= 0)
                return false;

            RemoveItemFromInventory(item);

            int inventoryCountAfterRemoval = GetInventoryCountByItemID(item.itemID);
            return inventoryCountAfterRemoval < inventoryCountBeforeRemoval;
        }

        public bool RemoveFirstItemByID(int itemID)
        {
            if (itemsInInventory == null)
                return false;

            for (int i = 0; i < itemsInInventory.Count; i++)
            {
                if (itemsInInventory[i] == null)
                    continue;

                if (itemsInInventory[i].itemID == itemID)
                {
                    itemsInInventory.RemoveAt(i);
                    return true;
                }
            }

            return false;
        }

        public int GetInventoryCountByItemID(int itemID)
        {
            if (itemsInInventory == null)
                return 0;

            int count = 0;

            for (int i = 0; i < itemsInInventory.Count; i++)
            {
                if (itemsInInventory[i] == null || itemsInInventory[i].itemID != itemID)
                    continue;

                if (itemsInInventory[i] is RangedProjectileItem projectile)
                {
                    count += Mathf.Max(1, projectile.currentAmmoAmount);
                }
                else if (itemsInInventory[i] is QuickSlotItem quickSlotItem)
                {
                    count += Mathf.Max(1, quickSlotItem.itemAmount);
                }
                else
                {
                    count += Mathf.Max(1, itemsInInventory[i].currentItemAmount);
                }
            }

            return count;
        }
    }
}
