using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using NUnit.Framework;
using System.Collections.Generic;
using Unity.Netcode;

namespace baodeag
{
    public class PlayerUIEquipmentManager : MonoBehaviour
    {
        [Header("Menu")]
        [SerializeField] GameObject menu;

        [Header("Weapon Slots")]
        [SerializeField] Image rightHandSlot01;
        [SerializeField] Image rightHandSlot02;
        [SerializeField] Image rightHandSlot03;
        [SerializeField] Image leftHandSlot01;
        [SerializeField] Image leftHandSlot02;
        [SerializeField] Image leftHandSlot03;

        [Header("Equiment Inventory")]
        public EquipmentType currentSelectedEquipmentSlot;
        [SerializeField] GameObject equipmentInventoryWindow;
        [SerializeField] GameObject equipmentInventorySlotPrefab;
        [SerializeField] Transform equipmentInventoryContentWindow;
        [SerializeField] Item currentSelectedItem;

        public void OpenEquipmentManagerMenu()
        {
            PlayerUIManager.instance.menuWindowIsOpen = true;
            menu.SetActive(true);
            equipmentInventoryWindow.SetActive(false);
            ClearEquipmentInventory();

            RefreshWeaponSlotIcons();
        }

        public void CloseEquipmentManagerMenu()
        {
            PlayerUIManager.instance.menuWindowIsOpen = false;
            menu.SetActive(false);

        }

        private void RefreshWeaponSlotIcons()
        {
            PlayerManager player = NetworkManager.Singleton.LocalClient.PlayerObject.GetComponent<PlayerManager>();

            // right hand 01
            WeaponItem rightHandWeapon01 = player.playerInventoryManager.weaponInRightHandSlots[0];

            if (rightHandWeapon01.itemIcon != null)
            {
                rightHandSlot01.enabled = true;
                rightHandSlot01.sprite = rightHandWeapon01.itemIcon;
            }
            else
            {
                rightHandSlot01.enabled = false;
            }

            // right hand 02
            WeaponItem rightHandWeapon02 = player.playerInventoryManager.weaponInRightHandSlots[1];
            if (rightHandWeapon02.itemIcon != null)
            {
                rightHandSlot02.enabled = true;
                rightHandSlot02.sprite = rightHandWeapon02.itemIcon;
            }
            else
            {
                rightHandSlot02.enabled = false;
            }

            // right hand 03
            WeaponItem rightHandWeapon03 = player.playerInventoryManager.weaponInRightHandSlots[2];
            if (rightHandWeapon03.itemIcon != null)
            {
                rightHandSlot03.enabled = true;
                rightHandSlot03.sprite = rightHandWeapon03.itemIcon;
            }
            else
            {
                rightHandSlot03.enabled = false;
            }

            // left hand 01
            WeaponItem leftHandWeapon01 = player.playerInventoryManager.weaponInLeftHandSlots[0];
            if (leftHandWeapon01.itemIcon != null)
            {
                leftHandSlot01.enabled = true;
                leftHandSlot01.sprite = leftHandWeapon01.itemIcon;
            }
            else
            {
                leftHandSlot01.enabled = false;
            }

            // left hand 02
            WeaponItem leftHandWeapon02 = player.playerInventoryManager.weaponInLeftHandSlots[1];
            if (leftHandWeapon02.itemIcon != null)
            {
                leftHandSlot02.enabled = true;
                leftHandSlot02.sprite = leftHandWeapon02.itemIcon;
            }
            else
            {
                leftHandSlot02.enabled = false;
            }

            // left hand 03
            WeaponItem leftHandWeapon03 = player.playerInventoryManager.weaponInLeftHandSlots[2];
            if (leftHandWeapon03.itemIcon != null)
            {
                leftHandSlot03.enabled = true;
                leftHandSlot03.sprite = leftHandWeapon03.itemIcon;
            }
            else
            {
                leftHandSlot03.enabled = false;
            }
        }

        private void ClearEquipmentInventory()
        {
            foreach (Transform item in equipmentInventoryContentWindow)
            {
                Destroy(item.gameObject);
            }
        }

        public void LoadEquipmentInventory()
        {
            equipmentInventoryWindow.SetActive(true);

            switch (currentSelectedEquipmentSlot)
            {
                case EquipmentType.RightWeapon01:
                    LoadWeaponInventory();
                    break;
                case EquipmentType.RightWeapon02:
                    LoadWeaponInventory();
                    break;
                case EquipmentType.RightWeapon03:
                    LoadWeaponInventory();
                    break;
                case EquipmentType.LeftWeapon01:
                    LoadWeaponInventory();
                    break;
                case EquipmentType.LeftWeapon02:
                    LoadWeaponInventory();
                    break;
                case EquipmentType.LeftWeapon03:
                    LoadWeaponInventory();
                    break;
                default:
                    break;
            }
        }

        private void LoadWeaponInventory()
        {
            PlayerManager player = NetworkManager.Singleton.LocalClient.PlayerObject.GetComponent<PlayerManager>();

            List<WeaponItem> weaponsInInventory = new List<WeaponItem>();

            for (int i = 0; i < player.playerInventoryManager.itemsInInventory.Count; i++)
            {
                WeaponItem weapon = player.playerInventoryManager.itemsInInventory[i] as WeaponItem;

                if (weapon != null) 
                    weaponsInInventory.Add(weapon);
            }

            if (weaponsInInventory.Count <= 0)
            {
                OpenEquipmentManagerMenu();
                return;
            }

            bool hasSelectedFirstInventorySlot = false;

            for (int i = 0; i <weaponsInInventory.Count; i++)
            {
                GameObject inventorySlotGameObject = Instantiate(equipmentInventorySlotPrefab, equipmentInventoryContentWindow);
                UI_EquipmentInventorySlot equipmentInventorySlot = inventorySlotGameObject.GetComponent<UI_EquipmentInventorySlot>();
                equipmentInventorySlot.AddItem(weaponsInInventory[i]);

                //this will select the first button on the list
                if (!hasSelectedFirstInventorySlot)
                {
                    hasSelectedFirstInventorySlot = true;
                    Button inventorySlotButton = inventorySlotGameObject.GetComponent<Button>();
                    inventorySlotButton.Select();
                    inventorySlotButton.OnSelect(null);
                }
            }
        }
    }
}
