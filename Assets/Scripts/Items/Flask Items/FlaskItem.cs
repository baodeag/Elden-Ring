using UnityEngine;

namespace baodeag
{
    [CreateAssetMenu(menuName = "Items/Consumables/Flask")]
    public class FlaskItem : QuickSlotItem
    {
        [Header("Empty Item")]
        [SerializeField] GameObject emptyFlaskItem;

        public override void AttemptToUseItem(PlayerManager player)
        {
            if (!CanIUseThisItem(player))
                return;

            player.playerEffectsManager.activeQuickSlotItemFX = Instantiate(itemModel, player.playerEquipmentManager.rightHandWeaponSlot.transform);

            player.playerAnimatorManager.PlayTargetActionAnimation(useItemAnimation, true, false, true, true, false);
        }
    }
}
