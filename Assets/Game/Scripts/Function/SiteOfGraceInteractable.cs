using System.Collections;
using Unity.Netcode;
using UnityEngine;

namespace baodeag
{
    public class SiteOfGraceInteractable : Interactable
    {
        [Header("Site of Grace Info")]
        public int siteOfGraceID;
        public NetworkVariable<bool> isActivated = new NetworkVariable<bool>
            (false,
            NetworkVariableReadPermission.Everyone,
            NetworkVariableWritePermission.Server);

        [Header("VFX")]
        [SerializeField] GameObject activatedParticles;

        [Header("Interaction Text")]
        [SerializeField] string unactivatedInteractionText = "Restore Site of Grace";
        [SerializeField] string activatedInteractionText = "Rest";

        [Header("Teleport Transform")]
        [SerializeField] Transform teleportTransform;

        protected override void Start()
        {
            base.Start();

            if (IsServer && WorldSaveGameManager.instance != null && WorldSaveGameManager.instance.currentCharacterData != null)
            {
                if (WorldSaveGameManager.instance.currentCharacterData.sitesOfGrace.ContainsKey(siteOfGraceID))
                {
                    isActivated.Value = WorldSaveGameManager.instance.currentCharacterData.sitesOfGrace[siteOfGraceID];
                }
                else
                {
                    isActivated.Value = false;
                }
            }   
            
            if (isActivated.Value)
            {
                interactableText = activatedInteractionText;
            }
            else
            {
                interactableText = unactivatedInteractionText;
            }
        }

        public override void OnNetworkSpawn()
        {
            base.OnNetworkSpawn();

            //if we join when the status has already changed, we force the onchange function to run here upon joining
            if (!IsOwner)
                OnIsActivatedChanged(false, isActivated.Value);

            isActivated.OnValueChanged += OnIsActivatedChanged;

            WorldObjectManager.instance.AddSiteOfGraceToList(this);
        }

        public override void OnNetworkDespawn()
        {
            base.OnNetworkDespawn();

            isActivated.OnValueChanged -= OnIsActivatedChanged;
        }

        private void RestoreSiteOfGrace(PlayerManager player)
        {
            isActivated.Value = true;

            if (WorldSaveGameManager.instance != null && WorldSaveGameManager.instance.currentCharacterData != null)
            {
                WorldSaveGameManager.instance.currentCharacterData.sitesOfGrace[siteOfGraceID] = true;
                WorldSaveGameManager.instance.SaveGame();
            }

            CompleteGraceActivationLocally(player);
        }

        private void RestAtSiteOfGrace(PlayerManager player)
        {
            if (WorldAIManager.instance != null)
                WorldAIManager.instance.ResetAllCharacters();

            CompleteRestAtSiteOfGraceLocally(player);
        }

        private void CompleteGraceActivationLocally(PlayerManager player)
        {
            if (player == null || !player.IsOwner)
                return;

            if (WorldSaveGameManager.instance != null && WorldSaveGameManager.instance.currentCharacterData != null)
            {
                WorldSaveGameManager.instance.currentCharacterData.sitesOfGrace[siteOfGraceID] = true;
                WorldSaveGameManager.instance.currentCharacterData.lastSiteOfGraceRestedAt = siteOfGraceID;
            }

            player.playerAnimatorManager.PlayTargetActionAnimation("Activate_Site_Of_Grace_01", true);
            PlayerUIManager.instance.playerUIPopUpManager.SendGraceRestoredPopUp("Site Of Grace Restored");
            StartCoroutine(WaitForAnimationAndPopUpThenRestoreCollider());

            if (WorldSaveGameManager.instance != null &&
                WorldSaveGameManager.instance.currentCharacterData != null &&
                WorldSaveGameManager.instance.currentCharacterSlotBeingUsed != CharacterSlot.NO_SLOT)
            {
                WorldSaveGameManager.instance.SaveGame();
            }
        }

        private IEnumerator WaitForAnimationAndPopUpThenRestoreCollider()
        {
            yield return new WaitForSeconds(2);
            interactableCollider.enabled = true;
        }

        private void CompleteRestAtSiteOfGraceLocally(PlayerManager player)
        {
            if (player == null || !player.IsOwner)
                return;

            if (WorldSaveGameManager.instance != null && WorldSaveGameManager.instance.currentCharacterData != null)
            {
                WorldSaveGameManager.instance.currentCharacterData.sitesOfGrace[siteOfGraceID] = true;
                WorldSaveGameManager.instance.currentCharacterData.lastSiteOfGraceRestedAt = siteOfGraceID;
            }

            PlayerUIManager.instance.OpenMenuAsRoot(PlayerUIManager.instance.playerUISiteOfGraceManager);
            interactableCollider.enabled = true;
            player.playerNetworkManager.currentHealth.Value = player.playerNetworkManager.maxHealth.Value;
            player.playerNetworkManager.currentStamina.Value = player.playerNetworkManager.maxStamina.Value;

            if (WorldSaveGameManager.instance != null &&
                WorldSaveGameManager.instance.currentCharacterData != null &&
                WorldSaveGameManager.instance.currentCharacterSlotBeingUsed != CharacterSlot.NO_SLOT)
            {
                WorldSaveGameManager.instance.SaveGame();
            }
        }

        private void OnIsActivatedChanged(bool oldStatus, bool newStatus)
        {
            if (isActivated.Value)
            {
                activatedParticles.SetActive(true);

                interactableText = activatedInteractionText;
            }
            else
            {
                interactableText = unactivatedInteractionText;
            }
        }

        public override void Interact(PlayerManager player)
        {
            if (player.isPerformingAction)
                return;

            if (player.playerCombatManager.isUsingItem)
                return;

            player.playerInteractionManager.RemoveInteractionFromList(this);
            PlayerUIManager.instance.playerUIPopUpManager.CloseAllPopUpWindows();
            player.playerNetworkManager.lastSiteOfGraceUsed.Value = siteOfGraceID;
            
            if (WorldSaveGameManager.instance != null && WorldSaveGameManager.instance.currentCharacterData != null)
                WorldSaveGameManager.instance.currentCharacterData.lastSiteOfGraceRestedAt = siteOfGraceID;

            if (IsServer)
            {
                ProcessGraceInteractionOnServer(player.OwnerClientId);
            }
            else
            {
                ProcessGraceInteractionServerRpc();
            }
        }

        [ServerRpc(RequireOwnership = false)]
        private void ProcessGraceInteractionServerRpc(ServerRpcParams serverRpcParams = default)
        {
            ProcessGraceInteractionOnServer(serverRpcParams.Receive.SenderClientId);
        }

        private void ProcessGraceInteractionOnServer(ulong playerClientId)
        {
            if (!IsServer)
                return;

            PlayerManager player = WorldGameSessionManager.instance != null
                ? WorldGameSessionManager.instance.GetPlayerByClientId(playerClientId)
                : null;

            if (player == null)
                return;

            ClientRpcParams clientRpcParams = new ClientRpcParams
            {
                Send = new ClientRpcSendParams
                {
                    TargetClientIds = new[] { playerClientId }
                }
            };

            if (!isActivated.Value)
            {
                isActivated.Value = true;

                if (WorldSaveGameManager.instance != null && WorldSaveGameManager.instance.currentCharacterData != null)
                {
                    WorldSaveGameManager.instance.currentCharacterData.sitesOfGrace[siteOfGraceID] = true;
                    WorldSaveGameManager.instance.SaveGame();
                }

                CompleteGraceActivationClientRpc(siteOfGraceID, clientRpcParams);
            }
            else
            {
                if (WorldAIManager.instance != null)
                    WorldAIManager.instance.ResetAllCharacters();

                CompleteGraceRestClientRpc(siteOfGraceID, clientRpcParams);
            }
        }

        [ClientRpc]
        private void CompleteGraceActivationClientRpc(int activatedSiteOfGraceID, ClientRpcParams clientRpcParams = default)
        {
            PlayerManager localPlayer = NetworkManager.Singleton != null &&
                NetworkManager.Singleton.LocalClient != null &&
                NetworkManager.Singleton.LocalClient.PlayerObject != null
                ? NetworkManager.Singleton.LocalClient.PlayerObject.GetComponent<PlayerManager>()
                : null;

            if (localPlayer == null)
                return;

            playerNetworkManagerLastGrace(localPlayer, activatedSiteOfGraceID);
            CompleteGraceActivationLocally(localPlayer);
        }

        [ClientRpc]
        private void CompleteGraceRestClientRpc(int targetSiteOfGraceID, ClientRpcParams clientRpcParams = default)
        {
            PlayerManager localPlayer = NetworkManager.Singleton != null &&
                NetworkManager.Singleton.LocalClient != null &&
                NetworkManager.Singleton.LocalClient.PlayerObject != null
                ? NetworkManager.Singleton.LocalClient.PlayerObject.GetComponent<PlayerManager>()
                : null;

            if (localPlayer == null)
                return;

            playerNetworkManagerLastGrace(localPlayer, targetSiteOfGraceID);
            CompleteRestAtSiteOfGraceLocally(localPlayer);
        }

        private void playerNetworkManagerLastGrace(PlayerManager player, int targetSiteOfGraceID)
        {
            player.playerNetworkManager.lastSiteOfGraceUsed.Value = targetSiteOfGraceID;

            if (WorldSaveGameManager.instance != null && WorldSaveGameManager.instance.currentCharacterData != null)
                WorldSaveGameManager.instance.currentCharacterData.lastSiteOfGraceRestedAt = targetSiteOfGraceID;
        }

        public void TeleportToSiteOfGrace()
        {
            //the player is only able to teleport when not in a co-op game, so we can grab the local player from the network manager
            PlayerManager player = NetworkManager.Singleton.LocalClient.PlayerObject.GetComponent<PlayerManager>();
            TeleportPlayerToSiteOfGrace(player);
        }

        public void TeleportPlayerToSiteOfGrace(PlayerManager player, bool handleLoadingScreen = true)
        {
            if (player == null)
                return;

            //enable loading screen
            if (handleLoadingScreen && player.IsOwner)
            {
                player.playerInteractionManager.ClearInteractionList();
                PlayerUIManager.instance.playerUILoadingScreenManager.ActivateLoadingScreen();
            }

            //teleport player
            if (player.characterController != null)
            {
                player.characterController.enabled = false;
            }

            player.transform.position = teleportTransform.position;

            if (player.IsOwner)
            {
                player.characterNetworkManager.networkPosition.Value = teleportTransform.position;
            }

            if (player.characterController != null)
            {
                player.characterController.enabled = true;
            }

            //disable loading screen
            if (handleLoadingScreen && player.IsOwner)
            {
                PlayerUIManager.instance.playerUILoadingScreenManager.DeactivateLoadingScreen(1);
            }
        }
    }
}
