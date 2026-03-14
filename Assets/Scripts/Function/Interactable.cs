using UnityEngine;
using Unity.Netcode;

namespace baodeag
{
    public class Interactable : NetworkBehaviour
    {
        public string interactableText;
        [SerializeField] protected Collider interactableCollider;
        [SerializeField] protected bool hostOnlyInteractable = true;

        protected virtual void Awake()
        {
            if (interactableCollider == null)
                interactableCollider = GetComponent<Collider>();
        }

        protected virtual void Start()
        {
            
        }

        public virtual void Interact(PlayerManager player)
        {
            if (!player.IsOwner)
                return;

            //remove the interactable from the player
            interactableCollider.enabled = false;
            player.playerInteractionManager.RemoveInteractionFromList(this);
            PlayerUIManager.instance.playerUIPopUpManager.CloseAllPopUpWindows();

            //save the game after interaction
            WorldSaveGameManager.instance.SaveGame();
        }

        public virtual void OnTriggerEnter(Collider other)
        {
            PlayerManager player = other.GetComponent<PlayerManager>();

            if (player != null)
            {
                if (!player.playerNetworkManager.IsHost && hostOnlyInteractable)
                    return;

                if (!player.IsOwner)
                    return;

                //pass the interactable to the player
                player.playerInteractionManager.AddInteractionToList(this);
            }
        }

        public virtual void OnTriggerExit(Collider other)
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
            }
        }
    }
}
