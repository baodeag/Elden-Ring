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
        public WeaponItem[] weaponInRightHandSlots = new WeaponItem[3];
        public int rightHandWeaponIndex = 0;
        public WeaponItem[] weaponInLeftHandSlots = new WeaponItem[3];
        public int leftHandWeaponIndex = 0;

        [Header("Armor")]
        public HeadEquipmentItem headEquipment;
        public BodyEquipmentItem bodyEquipment;
        public LegEquipmentItem legEquipment;
        public HandEquipmentItem handEquipment;

        [Header("Inventory")]
        public List<Item> itemsInInventory;

        public void AddItemToInventory(Item item)
        {
            itemsInInventory.Add(item);
        }

        public void RemoveItemFromInventory()
        {

        }
    }
}
