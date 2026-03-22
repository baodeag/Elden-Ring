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
            NetworkVariableWritePermission.Owner);

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

            if (IsOwner)
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

            //if our save file contains this site of grace, we remove
            if (WorldSaveGameManager.instance.currentCharacterData.sitesOfGrace.ContainsKey(siteOfGraceID))
                WorldSaveGameManager.instance.currentCharacterData.sitesOfGrace.Remove(siteOfGraceID);

            //then re-add it with the value of true
            WorldSaveGameManager.instance.currentCharacterData.sitesOfGrace.Add(siteOfGraceID, true);

            player.playerAnimatorManager.PlayTargetActionAnimation("Activate_Site_Of_Grace_01", true);

            PlayerUIManager.instance.playerUIPopUpManager.SendGraceRestoredPopUp("Site Of Grace Restored");

            StartCoroutine(WaitForAnimationAndPopUpThenRestoreCollider());
        }

        private void RestAtSiteOfGrace(PlayerManager player)
        {
            PlayerUIManager.instance.playerUISiteOfGraceManager.OpenMenu();

            interactableCollider.enabled = true; //temporarily re-enabling the collider here until we add the menu so you can respawn monsters indefinitely
            player.playerNetworkManager.currentHealth.Value = player.playerNetworkManager.maxHealth.Value;
            player.playerNetworkManager.currentStamina.Value = player.playerNetworkManager.maxStamina.Value;

            WorldAIManager.instance.ResetAllCharacters();
        }

        private IEnumerator WaitForAnimationAndPopUpThenRestoreCollider()
        {
            yield return new WaitForSeconds(2);
            interactableCollider.enabled = true;
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
            base.Interact(player);

            if (player.isPerformingAction)
                return;

            if (player.playerCombatManager.isUsingItem)
                return;

            WorldSaveGameManager.instance.currentCharacterData.lastSiteOfGraceRestedAt = siteOfGraceID;

            player.playerNetworkManager.lastSiteOfGraceUsed.Value = siteOfGraceID;

            if (!isActivated.Value)
            {
                RestoreSiteOfGrace(player);
            }
            else
            {
                RestAtSiteOfGrace(player);
            }
        }

        public void TeleportToSiteOfGrace()
        {
            //the player is only able to teleport when not in a co-op game, so we can grab the local player from the network manager
            PlayerManager player = NetworkManager.Singleton.LocalClient.PlayerObject.GetComponent<PlayerManager>();
            TeleportPlayerToSiteOfGrace(player);
        }

        public void TeleportPlayerToSiteOfGrace(PlayerManager player)
        {
            if (player == null)
                return;

            //enable loading screen
            if (player.IsOwner)
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
            if (player.IsOwner)
            {
                PlayerUIManager.instance.playerUILoadingScreenManager.DeactivateLoadingScreen(1);
            }
        }
    }
}
