using UnityEngine;
using Unity.Netcode;

namespace baodeag
{
    public class AICharacterInventoryManager : CharacterInventoryManager
    {
        AICharacterManager aiCharacter;
        [Header("Loot Chance")]
        public int dropItemChance = 10;
        [SerializeField] Item[] droppableItems;

        protected override void Awake()
        {
            base.Awake();

            aiCharacter = GetComponent<AICharacterManager>();
        }

        public void DropItem()
        {
            if (!aiCharacter.IsOwner)
                return;

            //the status of if this character will drop an item
            bool willDropItem = false;

            //random number rolled from 0 - 100
            int itemChanceRoll = Random.Range(0, 100);

            //if the random number is less than or equal to the drop item chance, the character will drop an item
            if (itemChanceRoll <= dropItemChance)
                willDropItem = true;

            if (!willDropItem)
                return;

            Item generatedItem = droppableItems[Random.Range(0, droppableItems.Length)];

            if (generatedItem == null)
                return;

            GameObject itemPickUpInteractableGameObject = Instantiate(WorldItemDatabase.Instance.pickUpItemPrefab);
            PickUpItemInteractable pickUpItemInteractable = itemPickUpInteractableGameObject.GetComponent<PickUpItemInteractable>();
            bool isBossLoot = aiCharacter is AIBossCharacterManager;
            ulong allowedLooterClientId = aiCharacter.GetLastPlayerWhoDealtDamageClientId();
            bool shouldShareLoot = isBossLoot || allowedLooterClientId == ulong.MaxValue;

            itemPickUpInteractableGameObject.GetComponent<NetworkObject>().Spawn();
            pickUpItemInteractable.itemID.Value = generatedItem.itemID;
            pickUpItemInteractable.networkPosition.Value = transform.position;
            pickUpItemInteractable.droppingCreatureID.Value = aiCharacter.NetworkObjectId;
            pickUpItemInteractable.allowedLooterClientId.Value = allowedLooterClientId;
            pickUpItemInteractable.isSharedLoot.Value = shouldShareLoot;
        }
    }
}
