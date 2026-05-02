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
        public NetworkVariable<ulong> allowedLooterClientId = new NetworkVariable<ulong>(ulong.MaxValue, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
        public NetworkVariable<bool> isSharedLoot = new NetworkVariable<bool>(true, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
        public NetworkVariable<bool> isLooted = new NetworkVariable<bool>(false, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);
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

            if (pickUpType == ItemPickUpType.WorldSpawn && IsServer)
                InitializeWorldSpawnLootState();
        }

        public override void OnNetworkSpawn()
        {
            base.OnNetworkSpawn();

            itemID.OnValueChanged += OnItemIDChanged;
            networkPosition.OnValueChanged += OnNetworkPositionChanged;
            droppingCreatureID.OnValueChanged += OnDroppingCreaturesIDChanged;
            isLooted.OnValueChanged += OnIsLootedChanged;

            if (pickUpType == ItemPickUpType.CharacterDrop)
                audioSource.PlayOneShot(itemDropSFX);

            if (!IsOwner)
            {
                OnItemIDChanged(0, itemID.Value);
                OnNetworkPositionChanged(Vector3.zero, networkPosition.Value);
                OnDroppingCreaturesIDChanged(0, droppingCreatureID.Value);
            }

            OnIsLootedChanged(false, isLooted.Value);
        }

        public override void OnNetworkDespawn()
        {
            base.OnNetworkDespawn();

            itemID.OnValueChanged -= OnItemIDChanged;
            networkPosition.OnValueChanged -= OnNetworkPositionChanged;
            droppingCreatureID.OnValueChanged -= OnDroppingCreaturesIDChanged;
            isLooted.OnValueChanged -= OnIsLootedChanged;
        }

        private void InitializeWorldSpawnLootState()
        {
            if (WorldSaveGameManager.instance == null || WorldSaveGameManager.instance.currentCharacterData == null)
                return;

            if (!WorldSaveGameManager.instance.currentCharacterData.worldItemsLooted.ContainsKey(worldSpawnInteractableID))
            {
                WorldSaveGameManager.instance.currentCharacterData.worldItemsLooted.Add(worldSpawnInteractableID, false);
            }

            hasBeenLooted = WorldSaveGameManager.instance.currentCharacterData.worldItemsLooted[worldSpawnInteractableID];
            isLooted.Value = hasBeenLooted;
        }

        public override void Interact(PlayerManager player)
        {
            if (!CanBeLootedBy(player))
                return;

            if (player.isPerformingAction)
                return;

            if (player.playerCombatManager.isUsingItem)
                return;

            player.playerInteractionManager.RemoveInteractionFromList(this);
            PlayerUIManager.instance.playerUIPopUpManager.CloseAllPopUpWindows();

            if (IsServer)
            {
                CompletePickupOnServer(player.OwnerClientId);
            }
            else
            {
                RequestPickupServerRpc();
            }
        }

        public override void OnTriggerEnter(Collider other)
        {
            PlayerManager player = other.GetComponent<PlayerManager>();

            if (player == null || !player.IsOwner)
                return;

            if (!CanBeLootedBy(player))
                return;

            player.playerInteractionManager.AddInteractionToList(this);
        }

        public override void OnTriggerExit(Collider other)
        {
            PlayerManager player = other.GetComponent<PlayerManager>();

            if (player == null || !player.IsOwner)
                return;

            player.playerInteractionManager.RemoveInteractionFromList(this);
            PlayerUIManager.instance.playerUIPopUpManager.CloseAllPopUpWindows();
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

        private bool CanBeLootedBy(PlayerManager player)
        {
            if (player == null)
                return false;

            if (pickUpType != ItemPickUpType.CharacterDrop)
                return true;

            if (isSharedLoot.Value)
                return true;

            return player.OwnerClientId == allowedLooterClientId.Value;
        }

        private void OnIsLootedChanged(bool oldValue, bool newValue)
        {
            if (pickUpType != ItemPickUpType.WorldSpawn)
                return;

            gameObject.SetActive(!newValue);
        }

        [ServerRpc(RequireOwnership = false)]
        private void RequestPickupServerRpc(ServerRpcParams serverRpcParams = default)
        {
            CompletePickupOnServer(serverRpcParams.Receive.SenderClientId);
        }

        private void CompletePickupOnServer(ulong looterClientId)
        {
            if (!IsServer)
                return;

            PlayerManager player = WorldGameSessionManager.instance != null
                ? WorldGameSessionManager.instance.GetPlayerByClientId(looterClientId)
                : null;

            if (player == null || !CanBeLootedBy(player))
                return;

            if (pickUpType == ItemPickUpType.WorldSpawn)
            {
                isLooted.Value = true;

                if (WorldSaveGameManager.instance != null && WorldSaveGameManager.instance.currentCharacterData != null)
                {
                    WorldSaveGameManager.instance.currentCharacterData.worldItemsLooted[worldSpawnInteractableID] = true;
                    WorldSaveGameManager.instance.SaveGame();
                }
            }

            ClientRpcParams clientRpcParams = new ClientRpcParams
            {
                Send = new ClientRpcSendParams
                {
                    TargetClientIds = new[] { looterClientId }
                }
            };

            GrantPickedUpItemClientRpc(itemID.Value != 0 ? itemID.Value : (item != null ? item.itemID : -1), clientRpcParams);

            GetComponent<NetworkObject>().Despawn();
        }

        [ClientRpc]
        private void GrantPickedUpItemClientRpc(int grantedItemID, ClientRpcParams clientRpcParams = default)
        {
            if (NetworkManager.Singleton == null || NetworkManager.Singleton.LocalClient == null || NetworkManager.Singleton.LocalClient.PlayerObject == null)
                return;

            PlayerManager localPlayer = NetworkManager.Singleton.LocalClient.PlayerObject.GetComponent<PlayerManager>();

            if (localPlayer == null || !localPlayer.IsOwner)
                return;

            Item grantedItem = WorldItemDatabase.Instance.CreateItemInstance(grantedItemID);

            if (grantedItem == null)
                grantedItem = WorldItemDatabase.Instance.GetItemByID(grantedItemID);

            if (grantedItem == null)
                return;

            localPlayer.characterSoundFXManager.PlaySoundFX(WorldSoundFXManager.instance.pickUpItemSFX);
            localPlayer.playerAnimatorManager.PlayTargetActionAnimation("Pick_Up_Item_01", true);
            localPlayer.playerInventoryManager.AddItemToInventory(grantedItem);
            PlayerUIManager.instance.playerUIPopUpManager.SendItemPopUp(grantedItem, 1);

            if (pickUpType == ItemPickUpType.WorldSpawn &&
                WorldSaveGameManager.instance != null &&
                WorldSaveGameManager.instance.currentCharacterData != null)
            {
                WorldSaveGameManager.instance.currentCharacterData.worldItemsLooted[worldSpawnInteractableID] = true;
            }

            if (WorldSaveGameManager.instance != null &&
                WorldSaveGameManager.instance.currentCharacterData != null &&
                WorldSaveGameManager.instance.currentCharacterSlotBeingUsed != CharacterSlot.NO_SLOT)
            {
                WorldSaveGameManager.instance.SaveGame();
            }
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
