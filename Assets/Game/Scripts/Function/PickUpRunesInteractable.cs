using UnityEngine;
using Unity.Netcode;

namespace baodeag
{
    public class PickUpRunesInteractable : Interactable
    {
        public NetworkVariable<int> runeCount = new NetworkVariable<int>(
            0,
            NetworkVariableReadPermission.Everyone,
            NetworkVariableWritePermission.Server);
        public NetworkVariable<ulong> runeOwnerClientId = new NetworkVariable<ulong>(
            0,
            NetworkVariableReadPermission.Everyone,
            NetworkVariableWritePermission.Server);

        public override void OnTriggerEnter(Collider other)
        {
            PlayerManager player = other.GetComponent<PlayerManager>();

            if (player == null || !player.IsOwner)
                return;

            if (player.OwnerClientId != runeOwnerClientId.Value)
                return;

            base.OnTriggerEnter(other);
        }

        public override void OnTriggerExit(Collider other)
        {
            PlayerManager player = other.GetComponent<PlayerManager>();

            if (player == null || !player.IsOwner)
                return;

            if (player.OwnerClientId != runeOwnerClientId.Value)
                return;

            base.OnTriggerExit(other);
        }

        public override void Interact(PlayerManager player)
        {
            if (!player.IsOwner)
                return;

            if (player.OwnerClientId != runeOwnerClientId.Value)
                return;

            player.playerInteractionManager.RemoveInteractionFromList(this);
            PlayerUIManager.instance.playerUIPopUpManager.CloseAllPopUpWindows();

            if (IsServer)
            {
                CompleteRunePickupOnServer(player.OwnerClientId);
            }
            else
            {
                RequestRunePickupServerRpc();
            }
        }

        [ServerRpc(RequireOwnership = false)]
        private void RequestRunePickupServerRpc(ServerRpcParams serverRpcParams = default)
        {
            CompleteRunePickupOnServer(serverRpcParams.Receive.SenderClientId);
        }

        private void CompleteRunePickupOnServer(ulong looterClientId)
        {
            if (!IsServer || looterClientId != runeOwnerClientId.Value)
                return;

            ClientRpcParams clientRpcParams = new ClientRpcParams
            {
                Send = new ClientRpcSendParams
                {
                    TargetClientIds = new[] { looterClientId }
                }
            };

            GrantRunesClientRpc(runeCount.Value, clientRpcParams);
            GetComponent<NetworkObject>().Despawn();
        }

        [ClientRpc]
        private void GrantRunesClientRpc(int grantedRunes, ClientRpcParams clientRpcParams = default)
        {
            if (NetworkManager.Singleton == null || NetworkManager.Singleton.LocalClient == null || NetworkManager.Singleton.LocalClient.PlayerObject == null)
                return;

            PlayerManager localPlayer = NetworkManager.Singleton.LocalClient.PlayerObject.GetComponent<PlayerManager>();

            if (localPlayer == null || !localPlayer.IsOwner)
                return;

            if (WorldSaveGameManager.instance != null && WorldSaveGameManager.instance.currentCharacterData != null)
            {
                WorldSaveGameManager.instance.currentCharacterData.hasDeadSpot = false;
            }

            localPlayer.playerStatsManager.AddRunes(grantedRunes);

            if (WorldSaveGameManager.instance != null &&
                WorldSaveGameManager.instance.currentCharacterData != null &&
                WorldSaveGameManager.instance.currentCharacterSlotBeingUsed != CharacterSlot.NO_SLOT)
            {
                WorldSaveGameManager.instance.SaveGame();
            }
        }
    }
}
