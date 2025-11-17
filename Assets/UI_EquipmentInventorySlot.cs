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
            switch (PlayerUIManager.instance.playerUIEquipmentManager.currentSelectedEquipmentSlot)
            {
                case EquipmentType.RightWeapon01:

                    //if our current weapon in this slot, is not an unarmed item, add it to inventory
                    WeaponItem currentWeapon = player.playerInventoryManager.weaponInRightHandSlots[0];

                    if (currentWeapon.itemID != WorldItemDatabase.Instance.unarmedWeapon.itemID)
                    {
                        player.playerInventoryManager.AddItemToInventory(currentWeapon);
                    }
                    //then replace the weapon in that slot with our new weapon
                    player.playerInventoryManager.weaponInRightHandSlots[0] = currentItem as WeaponItem;

                    //then remove the newly equipped weapon from inventory
                    player.playerInventoryManager.RemoveItemFromInventory(currentItem);

                    //re-equip the new weapon if wea are holding the current weapon in this slot
                    if (player.playerInventoryManager.rightHandWeaponIndex == 0)
                        player.playerNetworkManager.currentRightHandWeaponID.Value = currentItem.itemID;

                    //refresh the equipment window
                    PlayerUIManager.instance.playerUIEquipmentManager.OpenEquipmentManagerMenu();

                    break;
                case EquipmentType.RightWeapon02:
                    break;
                case EquipmentType.RightWeapon03:
                    break;
                case EquipmentType.LeftWeapon01:
                    break;
                case EquipmentType.LeftWeapon02:
                    break;
                case EquipmentType.LeftWeapon03:
                    break;
                default:
                    break;
            } 
        }
    }
}
