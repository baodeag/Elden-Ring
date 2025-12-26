using UnityEngine;

namespace baodeag
{
    [CreateAssetMenu(menuName = "Character Actions/Weapon Actions/Aim Action")]
    public class AimAction : WeaponItemAction
    {
        public override void AttemptToPerformAction(PlayerManager playerPerformingAction, WeaponItem weaponPerformingAction)
        {
            base.AttemptToPerformAction(playerPerformingAction, weaponPerformingAction);

            //if we are not grounded, do not proceed
            if (!playerPerformingAction.playerLocomotionManager.isGrounded)
                return;

            //if we are jumping, do not proceed
            if (playerPerformingAction.playerNetworkManager.isJumping.Value)
                return;

            //if we are rolling, do not proceed
            if (playerPerformingAction.playerLocomotionManager.isRolling)
                return;

            //if we are locked on, do not proceed
            if (playerPerformingAction.playerNetworkManager.isLockedOn.Value)
                return;

            if (playerPerformingAction.IsOwner)
            {
                //two hand the weapon(bow) before we aim
                if (!playerPerformingAction.playerNetworkManager.isTwoHandingWeapon.Value)
                {
                    if (playerPerformingAction.playerNetworkManager.isUsingRightHand.Value)
                    {
                        playerPerformingAction.playerNetworkManager.isTwoHandingRightWeapon.Value = true;
                    }
                    else if (playerPerformingAction.playerNetworkManager.isUsingLeftHand.Value)
                    {
                        playerPerformingAction.playerNetworkManager.isTwoHandingLeftWeapon.Value = true;
                    }
                }

                playerPerformingAction.playerNetworkManager.isAiming.Value = true;
            }
        }
    }
}
