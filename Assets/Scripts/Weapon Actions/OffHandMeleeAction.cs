using UnityEngine;

namespace baodeag
{
    [CreateAssetMenu(menuName = "Character Actions/Weapon Actions/Off Hand Melee Action")]
    public class OffHandMeleeAction : WeaponItemAction
    {
        [Header("Attack Animations")]
        [SerializeField] string dw_Attack_01 = "DW_Attack_01";
        [SerializeField] string dw_Attack_02 = "DW_Attack_02";
        [SerializeField] string dw_Jump_Attack_01 = "DW_Jump_Attack_01";
        [SerializeField] string dw_Roll_Attack_01 = "DW_Roll_Attack_01";
        [SerializeField] string dw_Backstep_Attack_01 = "DW_Backstep_Attack_01";
        [SerializeField] string dw_Run_Attack_01 = "DW_Run_Attack_01";

        public override void AttemptToPerformAction(PlayerManager playerPerformingAction, WeaponItem weaponPerformingAction)
        {
            base.AttemptToPerformAction(playerPerformingAction, weaponPerformingAction);

            //check for power stance action (dual attack)
            if (playerPerformingAction.playerNetworkManager.isUsingLeftHand.Value && !playerPerformingAction.playerNetworkManager.isTwoHandingWeapon.Value)
            {
                if (playerPerformingAction.playerInventoryManager.currentRightHandWeapon.weaponClass == 
                    playerPerformingAction.playerInventoryManager.currentLeftHandWeapon.weaponClass)
                {
                    //perform a power stance action
                    PerformPowerStanceLeftHandAction(playerPerformingAction, weaponPerformingAction);
                    return;
                }
            }

            //check for power stance action (dual attack)
            if (!playerPerformingAction.playerCombatManager.canBlock)
                return;

            //if we are using item, do not proceed
            if (playerPerformingAction.playerCombatManager.isUsingItem)
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
                playerPerformingAction.playerNetworkManager.isBlocking.Value = true;
        }

        private void PerformPowerStanceLeftHandAction(PlayerManager playerPerformingAction, WeaponItem weaponPerformingAction)
        {
            if (playerPerformingAction.playerNetworkManager.currentStamina.Value <= 0)
                return;

            //perform dual wield jumping attack
            if (!playerPerformingAction.playerLocomotionManager.isGrounded)
            {
                if (playerPerformingAction.isPerformingAction)
                    return;

                if (playerPerformingAction.IsOwner)
                    playerPerformingAction.playerAnimatorManager.PlayTargetAttackActionAnimation(weaponPerformingAction, AttackType.DualJumpAttack, dw_Jump_Attack_01, true);

                return;
            }

            if (playerPerformingAction.playerNetworkManager.isJumping.Value)
                return;

            if (playerPerformingAction.playerCombatManager.canPerformRollingAttack)
            {
                playerPerformingAction.playerCombatManager.canPerformRollingAttack = false;

                if (playerPerformingAction.IsOwner)
                    playerPerformingAction.playerAnimatorManager.PlayTargetAttackActionAnimation(weaponPerformingAction, AttackType.DualRollAttack, dw_Roll_Attack_01, true);
                
                return;
            }

            if (playerPerformingAction.playerCombatManager.canPerformBackstepAttack)
            {
                playerPerformingAction.playerCombatManager.canPerformBackstepAttack = false;

                if (playerPerformingAction.IsOwner)
                    playerPerformingAction.playerAnimatorManager.PlayTargetAttackActionAnimation(weaponPerformingAction, AttackType.DualBackstepAttack, dw_Backstep_Attack_01, true);
                
                return;
            }

            if (playerPerformingAction.playerNetworkManager.isSprinting.Value)
            {
                if (playerPerformingAction.IsOwner)
                    playerPerformingAction.playerAnimatorManager.PlayTargetAttackActionAnimation(weaponPerformingAction, AttackType.DualRunAttack, dw_Run_Attack_01, true);

                return;
            }

            if (playerPerformingAction.playerCombatManager.canComboWithOffHandWeapon && playerPerformingAction.isPerformingAction)
            {
                playerPerformingAction.playerCombatManager.canComboWithOffHandWeapon = false;

                if (playerPerformingAction.playerCombatManager.lastAttackAnimationPerformed == dw_Attack_01)
                {
                    playerPerformingAction.playerAnimatorManager.PlayTargetAttackActionAnimation(weaponPerformingAction, AttackType.DualAttack02, dw_Attack_02, true);
                }
                else if (playerPerformingAction.playerCombatManager.lastAttackAnimationPerformed == dw_Attack_02)
                {
                    playerPerformingAction.playerAnimatorManager.PlayTargetAttackActionAnimation(weaponPerformingAction, AttackType.DualAttack01, dw_Attack_01, true);
                }
                else
                {
                    playerPerformingAction.playerAnimatorManager.PlayTargetAttackActionAnimation(weaponPerformingAction, AttackType.DualAttack01, dw_Attack_01, true);
                }
            }
            else if (!playerPerformingAction.playerCombatManager.canComboWithOffHandWeapon && !playerPerformingAction.isPerformingAction)
            {
                playerPerformingAction.playerAnimatorManager.PlayTargetAttackActionAnimation(weaponPerformingAction, AttackType.DualAttack01, dw_Attack_01, true);
            }
        }
    }
}
