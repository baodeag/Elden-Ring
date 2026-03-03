using UnityEngine;
using UnityEngine.TextCore.Text;

namespace baodeag
{
    [CreateAssetMenu(menuName = "A.I/States/Attack")]
    public class AttackState : AIState
    {
        [Header("Current Attack")]
        [HideInInspector] public AICharacterAttackAction currentAttack;
        [HideInInspector] public bool willPerformCombo = false;

        [Header("State Flags")]
        protected bool hasPerformedAttack = false;
        protected bool hasPerformedCombo = false;

        [Header("Pivot After Attack")]
        [SerializeField] protected bool pivotAfterAttack = false;

        public override AIState Tick(AICharacterManager aiCharacter)
        {
            if (aiCharacter.aiCharacterCombatManager.currentTarget == null)
                return SwitchState(aiCharacter, aiCharacter.idle);

            if (aiCharacter.aiCharacterCombatManager.currentTarget.isDead.Value)
                return SwitchState(aiCharacter, aiCharacter.idle);

            aiCharacter.aiCharacterCombatManager.RotateTowardsTargetWhilstAttacking(aiCharacter);

            aiCharacter.characterAnimatorManager.UpdateAnimatorMovementParameters(0, 0, false);

            //perform a combo
            PerformCombo(aiCharacter);

            if (aiCharacter.isPerformingAction)
                return this;

            if (!hasPerformedAttack)
            {
                if (aiCharacter.aiCharacterCombatManager.actionRecoveryTimer > 0)
                    return this;

                PerformAttack(aiCharacter);

                return this;
            }

            if (pivotAfterAttack)
                aiCharacter.aiCharacterCombatManager.PivotTowardsTarget(aiCharacter);

            return SwitchState(aiCharacter, aiCharacter.combatStance);
        }

        protected void PerformAttack(AICharacterManager aiCharacter)
        {
            hasPerformedAttack = true;
            currentAttack.AttemptToPerformAction(aiCharacter);
            aiCharacter.aiCharacterCombatManager.actionRecoveryTimer = currentAttack.actionRecoveryTime;
        }

        protected override void ResetStateFlags(AICharacterManager aiCharacter)
        {
            base.ResetStateFlags(aiCharacter);

            hasPerformedAttack = false;
            hasPerformedCombo = false;
            willPerformCombo = false;
        }

        protected virtual void PerformCombo(AICharacterManager aiCharacter)
        {
            bool canPerformTheCombo = false;

            if (!willPerformCombo)
                return;

            if (hasPerformedCombo)
                return;

            if (currentAttack.comboAction == null)
                return;

            //if we dont need to hit our current target, we will perform the combo anyway
            if (aiCharacter.aiCharacterCombatManager.canPerformCombo
                && !aiCharacter.combatStance.onlyPerformComboIfInitialAttackHits)
                canPerformTheCombo = true;

            //if we do need to hit the target, and we have hit the target, perform the combo
            if (aiCharacter.aiCharacterCombatManager.canPerformCombo
                && aiCharacter.combatStance.onlyPerformComboIfInitialAttackHits
                && aiCharacter.aiCharacterCombatManager.hasHitTargetDuringCombo)
                canPerformTheCombo = true;

            if (canPerformTheCombo)
            {
                hasPerformedCombo = true;
                currentAttack.comboAction.AttemptToPerformAction(aiCharacter);
            }
        }
    }
}
