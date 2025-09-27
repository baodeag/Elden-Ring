using baodeag;
using UnityEngine;

namespace baodeag
{
    [CreateAssetMenu(menuName = "Character Actions/Weapon Actions/Heavy Attack Action")]
    public class HeavyAttackWeaponItemAction : WeaponItemAction
    {
        [SerializeField] string heavy_Attack_01 = "Main_Heavy_Attack_01";

        public override void AttemptToPerformAction(PlayerManager playerPerformingAction, WeaponItem weaponPerformingAction)
        {
            base.AttemptToPerformAction(playerPerformingAction, weaponPerformingAction);

            if (!playerPerformingAction.IsOwner)
                return;

            if (playerPerformingAction.playerNetworkManager.currentStamina.Value <= 0)
                return;

            if (!playerPerformingAction.isGrounded)
                return;

            PerformHeavyAttack(playerPerformingAction, weaponPerformingAction);
        }

        private void PerformHeavyAttack(PlayerManager playPerformingAction, WeaponItem weaponPerformingAction)
        {

            if (playPerformingAction.playerNetworkManager.isUsingRightHand.Value)
            {
                playPerformingAction.playerAnimatorManager.PlayTargetAttackActionAnimation(AttackType.HeavyAttack01, heavy_Attack_01, true);
            }
            if (playPerformingAction.playerNetworkManager.isUsingLeftHand.Value)
            {

            }
        }
    }
}
