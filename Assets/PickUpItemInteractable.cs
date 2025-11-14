using UnityEngine;
using Unity.Netcode;

namespace baodeag
{
    public class PickUpItemInteractable : Interactable
    {
        public ItemPickUpType pickUpType;

        [Header("Item")]
        [SerializeField] Item item;

        [Header("World Spawn Pick Up")]
        [SerializeField] int itemID;
        [SerializeField] bool hasBeenLooted = false;

        protected override void Start()
        {
            base.Start();

            if (pickUpType == ItemPickUpType.WorldSpawn)
                CheckIfWorldItemWasAlreadyLooted();
        }

        private void CheckIfWorldItemWasAlreadyLooted()
        {
            if (!NetworkManager.Singleton.IsHost)
            {
                gameObject.SetActive(false);
                return;
            }

            //compare itemID to the list of looted items in the current character data
            if (!WorldSaveGameManager.instance.currentCharacterData.worldItemsLooted.ContainsKey(itemID))
            {
                WorldSaveGameManager.instance.currentCharacterData.worldItemsLooted.Add(itemID, false);
            }

            hasBeenLooted = WorldSaveGameManager.instance.currentCharacterData.worldItemsLooted[itemID];

            //if it has been looted, disable the game object
            if (hasBeenLooted)
                gameObject.SetActive(false);
        }

        public override void Interact(PlayerManager player)
        {
            base.Interact(player);

            player.characterSoundFXManager.PlaySoundFX(WorldSoundFXManager.instance.pickUpItemSFX);

            player.playerInventoryManager.AddItemToInventory(item);

            //display a ui pop up 
            PlayerUIManager.instance.playerUIPopUpManager.SendItemPopUp(item, 1);

            //save loot status if its a world spawn
            if (pickUpType == ItemPickUpType.WorldSpawn)
            {
                if (WorldSaveGameManager.instance.currentCharacterData.worldItemsLooted.ContainsKey(itemID))
                {
                    WorldSaveGameManager.instance.currentCharacterData.worldItemsLooted.Remove(itemID);
                }

                WorldSaveGameManager.instance.currentCharacterData.worldItemsLooted.Add(itemID, true);
            }

            Destroy(gameObject);
        }
    }
}
