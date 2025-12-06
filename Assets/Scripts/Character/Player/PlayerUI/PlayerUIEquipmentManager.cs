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
        private Button rightHandSlot01Button;
        [SerializeField] Image rightHandSlot02;
        private Button rightHandSlot02Button;
        [SerializeField] Image rightHandSlot03;
        private Button rightHandSlot03Button;
        [SerializeField] Image leftHandSlot01;
        private Button leftHandSlot01Button;
        [SerializeField] Image leftHandSlot02;
        private Button leftHandSlot02Button;
        [SerializeField] Image leftHandSlot03;
        private Button leftHandSlot03Button;
        [SerializeField] Image headEquipmentSlot;
        private Button headEquipmentSlotButton;
        [SerializeField] Image bodyEquipmentSlot;
        private Button bodyEquipmentSlotButton;
        [SerializeField] Image legEquipmentSlot;
        private Button legEquipmentSlotButton;
        [SerializeField] Image handEquipmentSlot;
        private Button handEquipmentSlotButton;

        [Header("Equiment Inventory")]
        public EquipmentType currentSelectedEquipmentSlot;
        [SerializeField] GameObject equipmentInventoryWindow;
        [SerializeField] GameObject equipmentInventorySlotPrefab;
        [SerializeField] Transform equipmentInventoryContentWindow;
        [SerializeField] Item currentSelectedItem;

        private void Awake()
        {
            rightHandSlot01Button = rightHandSlot01.GetComponentInParent<Button>(true);
            rightHandSlot02Button = rightHandSlot02.GetComponentInParent<Button>(true);
            rightHandSlot03Button = rightHandSlot03.GetComponentInParent<Button>(true);

            leftHandSlot01Button = leftHandSlot01.GetComponentInParent<Button>(true);
            leftHandSlot02Button = leftHandSlot02.GetComponentInParent<Button>(true);
            leftHandSlot03Button = leftHandSlot03.GetComponentInParent<Button>(true);

            headEquipmentSlotButton = headEquipmentSlot.GetComponentInParent<Button>(true);
            bodyEquipmentSlotButton = bodyEquipmentSlot.GetComponentInParent<Button>(true);
            handEquipmentSlotButton = handEquipmentSlot.GetComponentInParent<Button>(true);
            legEquipmentSlotButton = legEquipmentSlot.GetComponentInParent<Button>(true);
        }

        public void OpenEquipmentManagerMenu()
        {
            PlayerUIManager.instance.menuWindowIsOpen = true;
            ToggleEquipmentButtons(true);
            menu.SetActive(true);
            equipmentInventoryWindow.SetActive(false);

            ClearEquipmentInventory();
            RefreshEquipmentSlotIcons();
        }

        public void RefreshMenu()
        {
            ClearEquipmentInventory();
            RefreshEquipmentSlotIcons();
        }

        private void ToggleEquipmentButtons(bool isEnabled)
        {
            rightHandSlot01Button.enabled = isEnabled;
            rightHandSlot02Button.enabled = isEnabled;
            rightHandSlot03Button.enabled = isEnabled;

            leftHandSlot01Button.enabled = isEnabled;
            leftHandSlot02Button.enabled = isEnabled;
            leftHandSlot03Button.enabled = isEnabled;

            headEquipmentSlotButton.enabled = isEnabled;
            bodyEquipmentSlotButton.enabled = isEnabled;
            legEquipmentSlotButton.enabled = isEnabled;
            handEquipmentSlotButton.enabled = isEnabled;
        }

        // this function simply return to the last selected button when you are finished equipping a new item
        public void SelectLastSelectedEquipmentSlot()
        {
            Button lastSelectedButton = null;
            ToggleEquipmentButtons(true);
            switch (currentSelectedEquipmentSlot)
            {
                case EquipmentType.RightWeapon01:
                    lastSelectedButton = rightHandSlot01Button;
                    break;
                case EquipmentType.RightWeapon02:
                    lastSelectedButton = rightHandSlot02Button;
                    break;
                case EquipmentType.RightWeapon03:
                    lastSelectedButton = rightHandSlot03Button;
                    break;
                case EquipmentType.LeftWeapon01:
                    lastSelectedButton = leftHandSlot01Button;
                    break;
                case EquipmentType.LeftWeapon02:
                    lastSelectedButton = leftHandSlot02Button;
                    break;
                case EquipmentType.LeftWeapon03:
                    lastSelectedButton = leftHandSlot03Button;
                    break;
                case EquipmentType.Head:
                    lastSelectedButton = headEquipmentSlotButton;
                    break;
                case EquipmentType.Body:
                    lastSelectedButton = bodyEquipmentSlotButton;
                    break;
                case EquipmentType.Legs:
                    lastSelectedButton = legEquipmentSlotButton;
                    break;
                case EquipmentType.Hands:
                    lastSelectedButton = handEquipmentSlotButton;
                    break;
                default:
                    break;
            }

            if (lastSelectedButton != null)
            {
                lastSelectedButton.Select();
                lastSelectedButton.OnSelect(null);
            }

            equipmentInventoryWindow.SetActive(false);
        }

        public void CloseEquipmentManagerMenu()
        {
            PlayerUIManager.instance.menuWindowIsOpen = false;
            menu.SetActive(false);

        }

        private void RefreshEquipmentSlotIcons()
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

            // head equipment
            HeadEquipmentItem headEquipment = player.playerInventoryManager.headEquipment;
            if (headEquipment != null)
            {
                headEquipmentSlot.enabled = true;
                headEquipmentSlot.sprite = headEquipment.itemIcon;
            }
            else
            {
                headEquipmentSlot.enabled = false;
            }

            // body equipment
            BodyEquipmentItem bodyEquipment = player.playerInventoryManager.bodyEquipment;
            if (bodyEquipment != null)
            {
                bodyEquipmentSlot.enabled = true;
                bodyEquipmentSlot.sprite = bodyEquipment.itemIcon;
            }
            else
            {
                bodyEquipmentSlot.enabled = false;
            }

            // leg equipment
            LegEquipmentItem legEquipment = player.playerInventoryManager.legEquipment;
            if (legEquipment != null)
            {
                legEquipmentSlot.enabled = true;
                legEquipmentSlot.sprite = legEquipment.itemIcon;
            }
            else
            {
                legEquipmentSlot.enabled = false;
            }

            // hand equipment
            EquipmentItem handEquipment = player.playerInventoryManager.handEquipment;
            if (handEquipment != null)
            {
                handEquipmentSlot.enabled = true;
                handEquipmentSlot.sprite = handEquipment.itemIcon;
            }
            else
            {
                handEquipmentSlot.enabled = false;
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
            ToggleEquipmentButtons(false);
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
                case EquipmentType.Head:
                    LoadHeadEquipmentInventory();
                    break;
                case EquipmentType.Body:
                    LoadBodyEquipmentInventory();
                    break;
                case EquipmentType.Legs:
                    LoadLegEquipmentInventory();
                    break;
                case EquipmentType.Hands:
                    LoadHandEquipmentInventory();
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
                //send a player a message that there are no weapons in the inventory
                equipmentInventoryWindow.SetActive(false);
                ToggleEquipmentButtons(true);
                RefreshMenu();
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

        private void LoadHeadEquipmentInventory()
        {
            PlayerManager player = NetworkManager.Singleton.LocalClient.PlayerObject.GetComponent<PlayerManager>();

            List<HeadEquipmentItem> headEquipmentInInventory = new List<HeadEquipmentItem>();

            for (int i = 0; i < player.playerInventoryManager.itemsInInventory.Count; i++)
            {
                HeadEquipmentItem equipment = player.playerInventoryManager.itemsInInventory[i] as HeadEquipmentItem;

                if (equipment != null)
                    headEquipmentInInventory.Add(equipment);
            }

            if (headEquipmentInInventory.Count <= 0)
            {
                //send a player a message that there are no head equipment in the inventory
                equipmentInventoryWindow.SetActive(false);
                ToggleEquipmentButtons(true);
                RefreshMenu();
                return;
            }

            bool hasSelectedFirstInventorySlot = false;

            for (int i = 0; i < headEquipmentInInventory.Count; i++)
            {
                GameObject inventorySlotGameObject = Instantiate(equipmentInventorySlotPrefab, equipmentInventoryContentWindow);
                UI_EquipmentInventorySlot equipmentInventorySlot = inventorySlotGameObject.GetComponent<UI_EquipmentInventorySlot>();
                equipmentInventorySlot.AddItem(headEquipmentInInventory[i]);

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

        private void LoadBodyEquipmentInventory()
        {
            PlayerManager player = NetworkManager.Singleton.LocalClient.PlayerObject.GetComponent<PlayerManager>();

            List<BodyEquipmentItem> BodyEquipmentInInventory = new List<BodyEquipmentItem>();

            for (int i = 0; i < player.playerInventoryManager.itemsInInventory.Count; i++)
            {
                BodyEquipmentItem equipment = player.playerInventoryManager.itemsInInventory[i] as BodyEquipmentItem;

                if (equipment != null)
                    BodyEquipmentInInventory.Add(equipment);
            }

            if (BodyEquipmentInInventory.Count <= 0)
            {
                //send a player a message that there are no body equipment in the inventory
                equipmentInventoryWindow.SetActive(false);
                ToggleEquipmentButtons(true);
                RefreshMenu();
                return;
            }

            bool hasSelectedFirstInventorySlot = false;

            for (int i = 0; i < BodyEquipmentInInventory.Count; i++)
            {
                GameObject inventorySlotGameObject = Instantiate(equipmentInventorySlotPrefab, equipmentInventoryContentWindow);
                UI_EquipmentInventorySlot equipmentInventorySlot = inventorySlotGameObject.GetComponent<UI_EquipmentInventorySlot>();
                equipmentInventorySlot.AddItem(BodyEquipmentInInventory[i]);

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

        private void LoadLegEquipmentInventory()
        {
            PlayerManager player = NetworkManager.Singleton.LocalClient.PlayerObject.GetComponent<PlayerManager>();

            List<LegEquipmentItem> legEquipmentInInventory = new List<LegEquipmentItem>();

            for (int i = 0; i < player.playerInventoryManager.itemsInInventory.Count; i++)
            {
                LegEquipmentItem equipment = player.playerInventoryManager.itemsInInventory[i] as LegEquipmentItem;

                if (equipment != null)
                    legEquipmentInInventory.Add(equipment);
            }

            if (legEquipmentInInventory.Count <= 0)
            {
                //send a player a message that there are no leg equipment in the inventory
                equipmentInventoryWindow.SetActive(false);
                ToggleEquipmentButtons(true);
                RefreshMenu();
                return;
            }

            bool hasSelectedFirstInventorySlot = false;

            for (int i = 0; i < legEquipmentInInventory.Count; i++)
            {
                GameObject inventorySlotGameObject = Instantiate(equipmentInventorySlotPrefab, equipmentInventoryContentWindow);
                UI_EquipmentInventorySlot equipmentInventorySlot = inventorySlotGameObject.GetComponent<UI_EquipmentInventorySlot>();
                equipmentInventorySlot.AddItem(legEquipmentInInventory[i]);

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

        private void LoadHandEquipmentInventory()
        {
            PlayerManager player = NetworkManager.Singleton.LocalClient.PlayerObject.GetComponent<PlayerManager>();

            List<HandEquipmentItem> handEquipmentInInventory = new List<HandEquipmentItem>();

            for (int i = 0; i < player.playerInventoryManager.itemsInInventory.Count; i++)
            {
                HandEquipmentItem equipment = player.playerInventoryManager.itemsInInventory[i] as HandEquipmentItem;

                if (equipment != null)
                    handEquipmentInInventory.Add(equipment);
            }

            if (handEquipmentInInventory.Count <= 0)
            {
                //send a player a message that there are no hand equipment in the inventory
                equipmentInventoryWindow.SetActive(false);
                ToggleEquipmentButtons(true);
                RefreshMenu();
                return;
            }

            bool hasSelectedFirstInventorySlot = false;

            for (int i = 0; i < handEquipmentInInventory.Count; i++)
            {
                GameObject inventorySlotGameObject = Instantiate(equipmentInventorySlotPrefab, equipmentInventoryContentWindow);
                UI_EquipmentInventorySlot equipmentInventorySlot = inventorySlotGameObject.GetComponent<UI_EquipmentInventorySlot>();
                equipmentInventorySlot.AddItem(handEquipmentInInventory[i]);

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

        public void SelectEquipmentSlot(int equipmentSlot)
        {
            currentSelectedEquipmentSlot = (EquipmentType)equipmentSlot;
        }

        public void UnEquipSelectedItem()
        {
            PlayerManager player = NetworkManager.Singleton.LocalClient.PlayerObject.GetComponent<PlayerManager>();
            Item unequippedItem;
            switch (currentSelectedEquipmentSlot)
            {
                case EquipmentType.RightWeapon01:
                    unequippedItem = player.playerInventoryManager.weaponInRightHandSlots[0];
                    if (unequippedItem != null)
                    {
                        player.playerInventoryManager.weaponInRightHandSlots[0] = Instantiate(WorldItemDatabase.Instance.unarmedWeapon);

                        if (unequippedItem.itemID != WorldItemDatabase.Instance.unarmedWeapon.itemID)
                            player.playerInventoryManager.AddItemToInventory(unequippedItem);
                    }

                    if (player.playerInventoryManager.rightHandWeaponIndex == 0)
                        player.playerNetworkManager.currentRightHandWeaponID.Value = WorldItemDatabase.Instance.unarmedWeapon.itemID;
                    break;
                case EquipmentType.RightWeapon02:
                    unequippedItem = player.playerInventoryManager.weaponInRightHandSlots[1];
                    if (unequippedItem != null)
                    {
                        player.playerInventoryManager.weaponInRightHandSlots[1] = Instantiate(WorldItemDatabase.Instance.unarmedWeapon);

                        if (unequippedItem.itemID != WorldItemDatabase.Instance.unarmedWeapon.itemID)
                            player.playerInventoryManager.AddItemToInventory(unequippedItem);
                    }

                    if (player.playerInventoryManager.rightHandWeaponIndex == 1)
                        player.playerNetworkManager.currentRightHandWeaponID.Value = WorldItemDatabase.Instance.unarmedWeapon.itemID;
                    break;
                case EquipmentType.RightWeapon03:
                    unequippedItem = player.playerInventoryManager.weaponInRightHandSlots[2];
                    if (unequippedItem != null)
                    {
                        player.playerInventoryManager.weaponInRightHandSlots[2] = Instantiate(WorldItemDatabase.Instance.unarmedWeapon);

                        if (unequippedItem.itemID != WorldItemDatabase.Instance.unarmedWeapon.itemID)
                            player.playerInventoryManager.AddItemToInventory(unequippedItem);
                    }

                    if (player.playerInventoryManager.rightHandWeaponIndex == 2)
                        player.playerNetworkManager.currentRightHandWeaponID.Value = WorldItemDatabase.Instance.unarmedWeapon.itemID;
                    break;
                case EquipmentType.LeftWeapon01:
                    unequippedItem = player.playerInventoryManager.weaponInLeftHandSlots[0];
                    if (unequippedItem != null)
                    {
                        player.playerInventoryManager.weaponInLeftHandSlots[0] = Instantiate(WorldItemDatabase.Instance.unarmedWeapon);

                        if (unequippedItem.itemID != WorldItemDatabase.Instance.unarmedWeapon.itemID)
                            player.playerInventoryManager.AddItemToInventory(unequippedItem);
                    }

                    if (player.playerInventoryManager.leftHandWeaponIndex == 0)
                        player.playerNetworkManager.currentLeftHandWeaponID.Value = WorldItemDatabase.Instance.unarmedWeapon.itemID;
                    break;
                case EquipmentType.LeftWeapon02:
                    unequippedItem = player.playerInventoryManager.weaponInLeftHandSlots[1];
                    if (unequippedItem != null)
                    {
                        player.playerInventoryManager.weaponInLeftHandSlots[1] = Instantiate(WorldItemDatabase.Instance.unarmedWeapon);

                        if (unequippedItem.itemID != WorldItemDatabase.Instance.unarmedWeapon.itemID)
                            player.playerInventoryManager.AddItemToInventory(unequippedItem);
                    }

                    if (player.playerInventoryManager.leftHandWeaponIndex == 1)
                        player.playerNetworkManager.currentLeftHandWeaponID.Value = WorldItemDatabase.Instance.unarmedWeapon.itemID;
                    break;
                case EquipmentType.LeftWeapon03:
                    unequippedItem = player.playerInventoryManager.weaponInLeftHandSlots[2];
                    if (unequippedItem != null)
                    {
                        player.playerInventoryManager.weaponInLeftHandSlots[2] = Instantiate(WorldItemDatabase.Instance.unarmedWeapon);

                        if (unequippedItem.itemID != WorldItemDatabase.Instance.unarmedWeapon.itemID)
                            player.playerInventoryManager.AddItemToInventory(unequippedItem);
                    }

                    if (player.playerInventoryManager.leftHandWeaponIndex == 2)
                        player.playerNetworkManager.currentLeftHandWeaponID.Value = WorldItemDatabase.Instance.unarmedWeapon.itemID;
                    break;

                case EquipmentType.Head:
                    unequippedItem = player.playerInventoryManager.headEquipment;

                    if (unequippedItem != null)
                        player.playerInventoryManager.AddItemToInventory(unequippedItem);

                    player.playerInventoryManager.headEquipment = null;
                    player.playerEquipmentManager.LoadHeadEquipment(player.playerInventoryManager.headEquipment);

                    break;
                
                case EquipmentType.Body:
                    unequippedItem = player.playerInventoryManager.bodyEquipment;

                    if (unequippedItem != null)
                        player.playerInventoryManager.AddItemToInventory(unequippedItem);

                    player.playerInventoryManager.bodyEquipment = null;
                    player.playerEquipmentManager.LoadBodyEquipment(player.playerInventoryManager.bodyEquipment);

                    break;

                case EquipmentType.Legs:
                    unequippedItem = player.playerInventoryManager.legEquipment;

                    if (unequippedItem != null)
                        player.playerInventoryManager.AddItemToInventory(unequippedItem);

                    player.playerInventoryManager.legEquipment = null;
                    player.playerEquipmentManager.LoadLegEquipment(player.playerInventoryManager.legEquipment);

                    break;

                case EquipmentType.Hands:
                    unequippedItem = player.playerInventoryManager.handEquipment;

                    if (unequippedItem != null)
                        player.playerInventoryManager.AddItemToInventory(unequippedItem);

                    player.playerInventoryManager.handEquipment = null;
                    player.playerEquipmentManager.LoadHandEquipment(player.playerInventoryManager.handEquipment);

                    break;

                default:
                    break;
            }

            //refresh menu
            RefreshMenu();
        }
    }
}
