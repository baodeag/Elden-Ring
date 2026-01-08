using UnityEngine;
using Unity.Netcode;
using System.Collections;

namespace baodeag
{
    public class PickUpItemInteractable : Interactable
    {
        public ItemPickUpType pickUpType;

        [Header("Item")]
        [SerializeField] Item item;

        [Header("Creature Loot Pick Up")]
        public NetworkVariable<int> itemID = new NetworkVariable<int>(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
        public NetworkVariable<Vector3> networkPosition = new NetworkVariable<Vector3>(Vector3.zero, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
        public NetworkVariable<ulong> droppingCreatureID = new NetworkVariable<ulong>(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
        public bool trackDroppingCreaturesPosition = true;

        [Header("World Spawn Pick Up")]
        [SerializeField] int worldSpawnInteractableID; //this is unique id given to each world spawn item, so you may not loot them more than once
        [SerializeField] bool hasBeenLooted = false;

        [Header("Drop SFX")]
        [SerializeField] AudioClip itemDropSFX;
        private AudioSource audioSource;

        protected override void Awake()
        {
            base.Awake();

            audioSource = GetComponent<AudioSource>();
        }

        protected override void Start()
        {
            base.Start();

            if (pickUpType == ItemPickUpType.WorldSpawn)
                CheckIfWorldItemWasAlreadyLooted();
        }

        public override void OnNetworkSpawn()
        {
            base.OnNetworkSpawn();

            itemID.OnValueChanged += OnItemIDChanged;
            networkPosition.OnValueChanged += OnNetworkPositionChanged;
            droppingCreatureID.OnValueChanged += OnDroppingCreaturesIDChanged;

            if (pickUpType == ItemPickUpType.CharacterDrop)
                audioSource.PlayOneShot(itemDropSFX);

            if (!IsOwner)
            {
                OnItemIDChanged(0, itemID.Value);
                OnNetworkPositionChanged(Vector3.zero, networkPosition.Value);
                OnDroppingCreaturesIDChanged(0, droppingCreatureID.Value);
            }
        }

        public override void OnNetworkDespawn()
        {
            base.OnNetworkDespawn();

            itemID.OnValueChanged -= OnItemIDChanged;
            networkPosition.OnValueChanged -= OnNetworkPositionChanged;
            droppingCreatureID.OnValueChanged -= OnDroppingCreaturesIDChanged;
        }

        private void CheckIfWorldItemWasAlreadyLooted()
        {
            if (!NetworkManager.Singleton.IsHost)
            {
                gameObject.SetActive(false);
                return;
            }

            //compare itemID to the list of looted items in the current character data
            if (!WorldSaveGameManager.instance.currentCharacterData.worldItemsLooted.ContainsKey(worldSpawnInteractableID))
            {
                WorldSaveGameManager.instance.currentCharacterData.worldItemsLooted.Add(worldSpawnInteractableID, false);
            }

            hasBeenLooted = WorldSaveGameManager.instance.currentCharacterData.worldItemsLooted[worldSpawnInteractableID];

            //if it has been looted, disable the game object
            if (hasBeenLooted)
                gameObject.SetActive(false);
        }

        public override void Interact(PlayerManager player)
        {
            if (player.isPerformingAction)
                return;

            if (player.playerCombatManager.isUsingItem)
                return;

            base.Interact(player);

            player.characterSoundFXManager.PlaySoundFX(WorldSoundFXManager.instance.pickUpItemSFX);

            player.playerAnimatorManager.PlayTargetActionAnimation("Pick_Up_Item_01", true);

            player.playerInventoryManager.AddItemToInventory(item);

            //display a ui pop up 
            PlayerUIManager.instance.playerUIPopUpManager.SendItemPopUp(item, 1);

            //save loot status if its a world spawn
            if (pickUpType == ItemPickUpType.WorldSpawn)
            {
                if (WorldSaveGameManager.instance.currentCharacterData.worldItemsLooted.ContainsKey(worldSpawnInteractableID))
                {
                    WorldSaveGameManager.instance.currentCharacterData.worldItemsLooted.Remove(worldSpawnInteractableID);
                }

                WorldSaveGameManager.instance.currentCharacterData.worldItemsLooted.Add(worldSpawnInteractableID, true);
            }

            DestroyThisNetworkObjectServerRpc();
        }

        protected void OnItemIDChanged(int oldValue, int newValue)
        {
            if (pickUpType != ItemPickUpType.CharacterDrop)
                return;

            item = WorldItemDatabase.Instance.GetItemByID(itemID.Value);
        }

        protected void OnNetworkPositionChanged(Vector3 oldPosition, Vector3 newPosition)
        {
            if (pickUpType != ItemPickUpType.CharacterDrop)
                return;

            transform.position = networkPosition.Value;
        }

        protected void OnDroppingCreaturesIDChanged(ulong oldID, ulong newID)
        {
            if (pickUpType != ItemPickUpType.CharacterDrop)
                return;

            if (trackDroppingCreaturesPosition)
                StartCoroutine(TrackDroppingCreaturesPosition());
        }

        protected IEnumerator TrackDroppingCreaturesPosition()
        {
            AICharacterManager droppingCreature = NetworkManager.Singleton.SpawnManager.SpawnedObjects[droppingCreatureID.Value].gameObject.GetComponent<AICharacterManager>();
            bool trackCreature = false;

            if (droppingCreature != null)
                trackCreature = true;

            if (trackCreature)
            {
                while (gameObject.activeInHierarchy)
                {
                    transform.position = droppingCreature.characterCombatManager.lockOnTransform.position;
                    yield return null;
                }
            }

            yield return null;
        }

        [ServerRpc(RequireOwnership = false)]
        protected void DestroyThisNetworkObjectServerRpc()
        {
            if (IsServer)
            {
                GetComponent<NetworkObject>().Despawn();   
            }
        }
    }
}
