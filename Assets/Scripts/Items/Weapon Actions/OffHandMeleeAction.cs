using UnityEngine;

namespace baodeag
{
    [CreateAssetMenu(menuName = "Character Actions/Weapon Actions/Off Hand Melee Action")]
    public class OffHandMeleeAction : WeaponItemAction
    {
        public override void AttemptToPerformAction(PlayerManager playerPerformingAction, WeaponItem weaponPerformingAction)
        {
            base.AttemptToPerformAction(playerPerformingAction, weaponPerformingAction);

            //check for power stance action (dual attack)
            if (!playerPerformingAction.playerCombatManager.canBlock)
                return;

            //check for attack
            if (playerPerformingAction.playerNetworkManager.isAttacking.Value)
            {
                //disable blocking state
                if (playerPerformingAction.IsOwner)
                    playerPerformingAction.playerNetworkManager.isBlocking.Value = false;

                return;
            }

            if (playerPerformingAction.playerNetworkManager.isBlocking.Value)
                return;

            if (playerPerformingAction.IsOwner)
                playerPerformingAction.playerNetworkManager.isAttacking.Value = true;
        }
    }
}
