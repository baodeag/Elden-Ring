using UnityEngine;

namespace baodeag
{
    [CreateAssetMenu(menuName = "Items/Ashes Of War/Parry")]
    public class ParryAshOfWar : AshOfWar
    {
        public override void AttemptToPerformAction(PlayerManager playerPerformingAction)
        {
            base.AttemptToPerformAction(playerPerformingAction);

            if (!CanIUseThisAbility(playerPerformingAction))
                return;

            DeductStaminaCost(playerPerformingAction);
            DeductFocusPointCost(playerPerformingAction);
            PerformParryTypeBasedOnWeapon(playerPerformingAction);
        }

        public override bool CanIUseThisAbility(PlayerManager playerPerformingAction)
        {
            if (playerPerformingAction.isPerformingAction)
            {
                Debug.Log("Cannot Perform Ash Of War: You Are Already Performing An Action");
                return false;
            }

            if (playerPerformingAction.playerNetworkManager.isJumping.Value)
            {
                Debug.Log("Cannot Perform Ash Of War: You Are Jumping");
                return false;
            }

            if (!playerPerformingAction.playerLocomotionManager.isGrounded)
            {
                Debug.Log("Cannot Perform Ash Of War: You Are Not Grounded");
                return false;
            }

            if (playerPerformingAction.playerNetworkManager.currentStamina.Value <= 0)
            {
                Debug.Log("Cannot Perform Ash Of War: Out Of Stamina");
                return false;
            }

            return true;
        }

        //smaller weapon perform faster parries
        private void PerformParryTypeBasedOnWeapon(PlayerManager playerPerformingAction)
        {
            WeaponItem weaponBeingUsed = playerPerformingAction.playerCombatManager.currentWeaponBeingUsed;

            switch (weaponBeingUsed.weaponClass)
            {
                case WeaponClass.StraightSword:
                    break;
                case WeaponClass.Spear:
                    break;
                case WeaponClass.MediumShield:
                    playerPerformingAction.playerAnimatorManager.PlayTargetActionAnimation("Slow_Parry_01", true);
                    break;
                case WeaponClass.Fist:
                    break;
                case WeaponClass.LightShield:
                    playerPerformingAction.playerAnimatorManager.PlayTargetActionAnimation("Fast_Parry_01", true);
                    break;
                default:
                    break;
            }
        }
    }
}
