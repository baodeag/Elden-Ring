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

            base.Interact(player);

            WorldSaveGameManager.instance.currentCharacterData.hasDeadSpot = false;
            player.playerStatsManager.AddRunes(runeCount.Value);

            DespawnRunesServerRpc();
        }

        [ServerRpc(RequireOwnership = false)]
        private void DespawnRunesServerRpc()
        {
            if (IsServer)
            {
                GetComponent<NetworkObject>().Despawn();
            }
        }
    }
}
