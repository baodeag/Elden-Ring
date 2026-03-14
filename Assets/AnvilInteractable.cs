using UnityEngine;

namespace baodeag
{
    public class AnvilInteractable : Interactable
    {
        public override void Interact(PlayerManager player)
        {
            if (!player.IsOwner)
                return;

            //remove the interactable from the player
            player.playerInteractionManager.RemoveInteractionFromList(this);
            PlayerUIManager.instance.playerUIPopUpManager.CloseAllPopUpWindows();

            //save the game after interaction
            WorldSaveGameManager.instance.SaveGame();

            if (player.IsOwner)
                PlayerUIManager.instance.playerUIWeaponUpgradeManager.OpenMenu();
        }

        public override void OnTriggerExit(Collider other)
        {
            PlayerManager player = other.GetComponent<PlayerManager>();

            if (player != null)
            {
                if (!player.playerNetworkManager.IsHost && hostOnlyInteractable)
                    return;

                if (!player.IsOwner)
                    return;

                //remove the interactable from the player
                player.playerInteractionManager.RemoveInteractionFromList(this);
                PlayerUIManager.instance.playerUIPopUpManager.CloseAllPopUpWindows();
                PlayerUIManager.instance.playerUIWeaponUpgradeManager.CloseMenu();
            }
        }
    }
}
