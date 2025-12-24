using UnityEngine;

namespace baodeag
{
    [CreateAssetMenu(menuName = "Character Actions/Weapon Actions/Fie Projectile Action")]
    public class FireProjectileAction : WeaponItemAction
    {
        [SerializeField] ProjectileSlot projectileSlot;

        public override void AttemptToPerformAction(PlayerManager playerPerformingAction, WeaponItem weaponPerformingAction)
        {
            base.AttemptToPerformAction(playerPerformingAction, weaponPerformingAction);

            if (!playerPerformingAction.IsOwner)
                return;

            if (playerPerformingAction.playerNetworkManager.currentStamina.Value <= 0)
                return;

            RangedProjectileItem projectileItem = null;

            switch (projectileSlot)
            {
                case ProjectileSlot.Main:
                    projectileItem = playerPerformingAction.playerInventoryManager.mainProjectile;
                    break;
                case ProjectileSlot.Secondary:
                    projectileItem = playerPerformingAction.playerInventoryManager.secondaryProjectile;
                    break;
                default:
                    break;
            }

            if (projectileItem == null)
                return;

            if (!playerPerformingAction.IsOwner)
                return;

            //if the player is not two handing the weapon, set the two handing bool based on which hand is using the weapon
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

            //if the player does not have an arrow notched, do so now
            if (!playerPerformingAction.playerNetworkManager.hasArrowNotched.Value)
            {
                bool canIDrawAProjectile = CanIFireThisProjectile(weaponPerformingAction, projectileItem);

                if (!canIDrawAProjectile)
                    return;

                if (projectileItem.currentAmmoAmount <= 0)
                {
                    playerPerformingAction.playerAnimatorManager.PlayTargetActionAnimation("Out_Of_Ammo_01", true);
                    return;
                }

                playerPerformingAction.playerAnimatorManager.PlayTargetActionAnimation("Bow_Draw_01", true);
                playerPerformingAction.playerNetworkManager.NotifyServerOfDrawnProjectileServerRpc(projectileItem.itemID);
            }
        }

        private bool CanIFireThisProjectile(WeaponItem weaponPerrformingAction, RangedProjectileItem projectileItem)
        {
            //check for crossbow, bow, etc requirements here
            return true;
        }
    }
}
