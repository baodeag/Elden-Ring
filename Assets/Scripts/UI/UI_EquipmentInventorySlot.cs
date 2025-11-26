using UnityEngine;
using UnityEngine.UI;
using Unity.Netcode;

namespace baodeag
{
    public class UI_EquipmentInventorySlot : MonoBehaviour
    {
        public Image itemIcon;
        public Image highlightedIcon;
        [SerializeField] public Item currentItem;

        public void AddItem(Item item)
        {
            if (item == null)
            {
                itemIcon.enabled = false;
                return;
            }

            itemIcon.enabled = true;

            currentItem = item;
            itemIcon.sprite = item.itemIcon;
        }

        public void SelectSlot()
        {
            highlightedIcon.enabled = true;
        }

        public void DeselectSlot()
        {
            highlightedIcon.enabled = false;
        }

        public void EquipItem()
        {
            PlayerManager player = NetworkManager.Singleton.LocalClient.PlayerObject.GetComponent<PlayerManager>();
            Item equippedItem;
            switch (PlayerUIManager.instance.playerUIEquipmentManager.currentSelectedEquipmentSlot)
            {
                case EquipmentType.RightWeapon01:

                    //if our current weapon in this slot, is not an unarmed item, add it to inventory
                    equippedItem = player.playerInventoryManager.weaponInRightHandSlots[0];

                    if (equippedItem.itemID != WorldItemDatabase.Instance.unarmedWeapon.itemID)
                    {
                        player.playerInventoryManager.AddItemToInventory(equippedItem);
                    }
                    //then replace the weapon in that slot with our new weapon
                    player.playerInventoryManager.weaponInRightHandSlots[0] = currentItem as WeaponItem;

                    //then remove the newly equipped weapon from inventory
                    player.playerInventoryManager.RemoveItemFromInventory(currentItem);

                    //re-equip the new weapon if wea are holding the current weapon in this slot
                    if (player.playerInventoryManager.rightHandWeaponIndex == 0)
                        player.playerNetworkManager.currentRightHandWeaponID.Value = currentItem.itemID;

                    //refresh the equipment window
                    PlayerUIManager.instance.playerUIEquipmentManager.RefreshMenu();

                    break;
                case EquipmentType.RightWeapon02:
                    //if our current weapon in this slot, is not an unarmed item, add it to inventory
                    equippedItem = player.playerInventoryManager.weaponInRightHandSlots[1];

                    if (equippedItem.itemID != WorldItemDatabase.Instance.unarmedWeapon.itemID)
                    {
                        player.playerInventoryManager.AddItemToInventory(equippedItem);
                    }
                    //then replace the weapon in that slot with our new weapon
                    player.playerInventoryManager.weaponInRightHandSlots[1] = currentItem as WeaponItem;

                    //then remove the newly equipped weapon from inventory
                    player.playerInventoryManager.RemoveItemFromInventory(currentItem);

                    //re-equip the new weapon if wea are holding the current weapon in this slot
                    if (player.playerInventoryManager.rightHandWeaponIndex == 1)
                        player.playerNetworkManager.currentRightHandWeaponID.Value = currentItem.itemID;

                    //refresh the equipment window
                    PlayerUIManager.instance.playerUIEquipmentManager.RefreshMenu();

                    break;
                case EquipmentType.RightWeapon03:
                    //if our current weapon in this slot, is not an unarmed item, add it to inventory
                    equippedItem = player.playerInventoryManager.weaponInRightHandSlots[2];

                    if (equippedItem.itemID != WorldItemDatabase.Instance.unarmedWeapon.itemID)
                    {
                        player.playerInventoryManager.AddItemToInventory(equippedItem);
                    }
                    //then replace the weapon in that slot with our new weapon
                    player.playerInventoryManager.weaponInRightHandSlots[2] = currentItem as WeaponItem;

                    //then remove the newly equipped weapon from inventory
                    player.playerInventoryManager.RemoveItemFromInventory(currentItem);

                    //re-equip the new weapon if wea are holding the current weapon in this slot
                    if (player.playerInventoryManager.rightHandWeaponIndex == 2)
                        player.playerNetworkManager.currentRightHandWeaponID.Value = currentItem.itemID;

                    //refresh the equipment window
                    PlayerUIManager.instance.playerUIEquipmentManager.RefreshMenu();

                    break;
                case EquipmentType.LeftWeapon01:
                    //if our current weapon in this slot, is not an unarmed item, add it to inventory
                    equippedItem = player.playerInventoryManager.weaponInLeftHandSlots[0];

                    if (equippedItem.itemID != WorldItemDatabase.Instance.unarmedWeapon.itemID)
                    {
                        player.playerInventoryManager.AddItemToInventory(equippedItem);
                    }
                    //then replace the weapon in that slot with our new weapon
                    player.playerInventoryManager.weaponInLeftHandSlots[0] = currentItem as WeaponItem;

                    //then remove the newly equipped weapon from inventory
                    player.playerInventoryManager.RemoveItemFromInventory(currentItem);

                    //re-equip the new weapon if wea are holding the current weapon in this slot
                    if (player.playerInventoryManager.leftHandWeaponIndex == 0)
                        player.playerNetworkManager.currentLeftHandWeaponID.Value = currentItem.itemID;

                    //refresh the equipment window
                    PlayerUIManager.instance.playerUIEquipmentManager.RefreshMenu();

                    break;
                case EquipmentType.LeftWeapon02:
                    //if our current weapon in this slot, is not an unarmed item, add it to inventory
                    equippedItem = player.playerInventoryManager.weaponInLeftHandSlots[1];

                    if (equippedItem.itemID != WorldItemDatabase.Instance.unarmedWeapon.itemID)
                    {
                        player.playerInventoryManager.AddItemToInventory(equippedItem);
                    }
                    //then replace the weapon in that slot with our new weapon
                    player.playerInventoryManager.weaponInLeftHandSlots[1] = currentItem as WeaponItem;

                    //then remove the newly equipped weapon from inventory
                    player.playerInventoryManager.RemoveItemFromInventory(currentItem);

                    //re-equip the new weapon if wea are holding the current weapon in this slot
                    if (player.playerInventoryManager.leftHandWeaponIndex == 1)
                        player.playerNetworkManager.currentLeftHandWeaponID.Value = currentItem.itemID;

                    //refresh the equipment window
                    PlayerUIManager.instance.playerUIEquipmentManager.RefreshMenu();

                    break;
                case EquipmentType.LeftWeapon03:
                    //if our current weapon in this slot, is not an unarmed item, add it to inventory
                    equippedItem = player.playerInventoryManager.weaponInLeftHandSlots[2];

                    if (equippedItem.itemID != WorldItemDatabase.Instance.unarmedWeapon.itemID)
                    {
                        player.playerInventoryManager.AddItemToInventory(equippedItem);
                    }
                    //then replace the weapon in that slot with our new weapon
                    player.playerInventoryManager.weaponInLeftHandSlots[2] = currentItem as WeaponItem;

                    //then remove the newly equipped weapon from inventory
                    player.playerInventoryManager.RemoveItemFromInventory(currentItem);

                    //re-equip the new weapon if wea are holding the current weapon in this slot
                    if (player.playerInventoryManager.leftHandWeaponIndex == 2)
                        player.playerNetworkManager.currentLeftHandWeaponID.Value = currentItem.itemID;

                    //refresh the equipment window
                    PlayerUIManager.instance.playerUIEquipmentManager.RefreshMenu();

                    break;

                case EquipmentType.Head:
                    //if our current weapon in this slot, is not a null item, add it to inventory
                    equippedItem = player.playerInventoryManager.headEquipment;

                    if (equippedItem != null)
                    {
                        player.playerInventoryManager.AddItemToInventory(equippedItem);
                    }
                    //then replace the item in that slot with our new item
                    player.playerInventoryManager.headEquipment = currentItem as HeadEquipmentItem;

                    //then remove the new item from inventory
                    player.playerInventoryManager.RemoveItemFromInventory(currentItem);

                    //re-equip the new item if wea are holding the current item in this slot
                    player.playerEquipmentManager.LoadHeadEquipment(player.playerInventoryManager.headEquipment);

                    //refresh the equipment window
                    PlayerUIManager.instance.playerUIEquipmentManager.RefreshMenu();

                    break;

                case EquipmentType.Body:
                    //if our current weapon in this slot, is not a null item, add it to inventory
                    equippedItem = player.playerInventoryManager.bodyEquipment;

                    if (equippedItem != null)
                    {
                        player.playerInventoryManager.AddItemToInventory(equippedItem);
                    }

                    //then replace the item in that slot with our new item
                    player.playerInventoryManager.bodyEquipment = currentItem as BodyEquipmentItem;

                    //then remove the new item from inventory
                    player.playerInventoryManager.RemoveItemFromInventory(currentItem);

                    //re-equip the new item if wea are holding the current item in this slot
                    player.playerEquipmentManager.LoadBodyEquipment(player.playerInventoryManager.bodyEquipment);

                    //refresh the equipment window
                    PlayerUIManager.instance.playerUIEquipmentManager.RefreshMenu();

                    break;

                case EquipmentType.Legs:
                    //if our current weapon in this slot, is not a null item, add it to inventory
                    equippedItem = player.playerInventoryManager.legEquipment;

                    if (equippedItem != null)
                    {
                        player.playerInventoryManager.AddItemToInventory(equippedItem);
                    }

                    //then replace the item in that slot with our new item
                    player.playerInventoryManager.legEquipment = currentItem as LegEquipmentItem;

                    //then remove the new item from inventory
                    player.playerInventoryManager.RemoveItemFromInventory(currentItem);

                    //re-equip the new item if wea are holding the current item in this slot
                    player.playerEquipmentManager.LoadLegEquipment(player.playerInventoryManager.legEquipment);

                    //refresh the equipment window
                    PlayerUIManager.instance.playerUIEquipmentManager.RefreshMenu();

                    break;

                case EquipmentType.Hands:
                    //if our current weapon in this slot, is not a null item, add it to inventory
                    equippedItem = player.playerInventoryManager.handEquipment;

                    if (equippedItem != null)
                    {
                        player.playerInventoryManager.AddItemToInventory(equippedItem);
                    }
                    //then replace the item in that slot with our new item
                    player.playerInventoryManager.handEquipment = currentItem as HandEquipmentItem;

                    //then remove the new item from inventory
                    player.playerInventoryManager.RemoveItemFromInventory(currentItem);

                    //re-equip the new item if wea are holding the current item in this slot
                    player.playerEquipmentManager.LoadHandEquipment(player.playerInventoryManager.handEquipment);

                    //refresh the equipment window
                    PlayerUIManager.instance.playerUIEquipmentManager.RefreshMenu();

                    break;

                default:
                    break;
            } 

            PlayerUIManager.instance.playerUIEquipmentManager.SelectLastSelectedEquipmentSlot();
        }
    }
}
