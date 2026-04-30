using Unity.Netcode;
using UnityEngine;

namespace baodeag
{
    public class ShopInteractable : Interactable
    {
        [SerializeField] private ShopInventory shopInventory;

        protected override void Awake()
        {
            base.Awake();

            if (shopInventory == null)
                shopInventory = GetComponent<ShopInventory>();

            if (string.IsNullOrWhiteSpace(interactableText))
                interactableText = "Browse wares";
        }

        public override void Interact(PlayerManager player)
        {
            if (!player.IsOwner)
                return;

            if (hostOnlyInteractable && !NetworkManager.Singleton.IsHost)
            {
                PlayerUIManager.instance.PlayUnableToContinueSFX();
                return;
            }

            if (shopInventory == null)
            {
                PlayerUIManager.instance.PlayUnableToContinueSFX();
                return;
            }

            player.playerInteractionManager.RemoveInteractionFromList(this);
            PlayerUIManager.instance.playerUIPopUpManager.CloseAllPopUpWindows();
            PlayerUIManager.instance.playerUIShopManager.OpenShop(shopInventory);
        }
    }
}
