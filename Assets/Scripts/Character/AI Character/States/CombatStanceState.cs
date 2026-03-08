using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace baodeag
{
    [CreateAssetMenu(menuName = "A.I/States/Combat Stance")]
    public class CombatStanceState : AIState
    {
        [Header("Attacks")]
        public List<AICharacterAttackAction> aiCharacterAttacks; //a list of all possible attacks this character can do
        [SerializeField] protected List<AICharacterAttackAction> potentialAttacks; //all attacks possible in this situation
        [SerializeField] private AICharacterAttackAction chosenAttack;
        [SerializeField] private AICharacterAttackAction previousAttack;
        protected bool hasAttack = false;

        [Header("Combo")]
        [SerializeField] protected bool canPerformCombo = false;
        [SerializeField] protected int percentageOfTimeWillPerformCombo = 25;
        [SerializeField] public bool onlyPerformComboIfInitialAttackHits = false;
        protected bool hasRolledForComboChance = false;

        [Header("Engagement Distance")]
        [SerializeField] public float maximumEngagementDistance = 5;

        [Header("Circling")]
        [SerializeField] bool willCircleTarget = false;
        private bool hasChoosenCirclePath = false;
        private float strafeMoveAmount;

        [Header("Blocking")]
        [SerializeField] bool canBlock = false;
        [SerializeField] int percentageOfTimeWillBlock = 75;
        private bool hasRolledForBlockChance = false;
        private bool willBlockDuringThisCombatRotation = false;

        [Header("Evasion")]
        [SerializeField] bool canEvade = false;
        [SerializeField] int percentageOfTimeWillEvade = 75;
        private bool hasEvaded = false;
        private bool hasRolledForEvasionChance = false;
        private bool willEvadeDuringThisCombatRotation = false;

        public override AIState Tick(AICharacterManager aiCharacter)
        {
            if (aiCharacter.isPerformingAction)
                return this;

            if (!aiCharacter.navMeshAgent.enabled)
                aiCharacter.navMeshAgent.enabled = true;

            if (aiCharacter.aiCharacterCombatManager.currentTarget.isDead.Value)
                aiCharacter.aiCharacterCombatManager.SetTarget(null);

            if (aiCharacter.aiCharacterCombatManager.enablePivot)
            {
                if (!aiCharacter.aiCharacterNetworkManager.isMoving.Value)
                {
                    if (aiCharacter.aiCharacterCombatManager.viewableAngle < -30 || aiCharacter.aiCharacterCombatManager.viewableAngle > 30)
                        aiCharacter.aiCharacterCombatManager.PivotTowardsTarget(aiCharacter);
                }
            }

            aiCharacter.aiCharacterCombatManager.RotateTowardsAgent(aiCharacter);

            if (aiCharacter.aiCharacterCombatManager.currentTarget == null)
                return SwitchState(aiCharacter, aiCharacter.idle);

            if (willCircleTarget)
                SetCirclePath(aiCharacter);

            //roll for block chance
            if (canBlock && !hasRolledForBlockChance)
            {
                hasRolledForBlockChance = true;
                willBlockDuringThisCombatRotation = RollForOutcomeChance(percentageOfTimeWillBlock);
            }

            //roll for evasion chance
            if (canEvade && !hasRolledForEvasionChance)
            {
                hasRolledForEvasionChance = true;
                willEvadeDuringThisCombatRotation = RollForOutcomeChance(percentageOfTimeWillEvade);
            }

            //roll for combo chance
            if (canPerformCombo && !hasRolledForComboChance)
            {
                hasRolledForComboChance = true;
                aiCharacter.attack.willPerformCombo = RollForOutcomeChance(percentageOfTimeWillPerformCombo);
            }

            if (willBlockDuringThisCombatRotation)
                aiCharacter.aiCharacterNetworkManager.isBlocking.Value = true;

            if (willEvadeDuringThisCombatRotation && aiCharacter.aiCharacterCombatManager.currentTarget.characterNetworkManager.isAttacking.Value && !hasEvaded)
            {
                hasEvaded = true;
                aiCharacter.aiCharacterCombatManager.PerformEvasion();
            }

            //if we dont have an attack, get one
            if (!hasAttack)
            {
                GetNewAttack(aiCharacter);
            }
            else
            {
                aiCharacter.attack.currentAttack = chosenAttack;
                return SwitchState(aiCharacter, aiCharacter.attack);
            }

            if (aiCharacter.aiCharacterCombatManager.distanceFromTarget > maximumEngagementDistance)
                return SwitchState(aiCharacter, aiCharacter.pursueTarget);

            NavMeshPath path = new NavMeshPath();
            aiCharacter.navMeshAgent.CalculatePath(aiCharacter.aiCharacterCombatManager.currentTarget.transform.position, path);
            aiCharacter.navMeshAgent.SetPath(path);

            return this;
        }

        protected virtual void GetNewAttack(AICharacterManager aiCharacter)
        {
            potentialAttacks = new List<AICharacterAttackAction>();

            foreach (var potentialAttack in aiCharacterAttacks)
            {
                //if we are too close for this attack, check the next
                if (potentialAttack.minimumAttackDistance > aiCharacter.aiCharacterCombatManager.distanceFromTarget)
                    continue;
                //if we are too far away for this attack, check the next
                if (potentialAttack.maximumAttackDistance < aiCharacter.aiCharacterCombatManager.distanceFromTarget)
                    continue;
                //if we are outside the minimum angle for this attack, check the next
                if (potentialAttack.minimumAttackAngle > aiCharacter.aiCharacterCombatManager.viewableAngle)
                    continue;
                //if we are outside the maximum angle for this attack, check the next
                if (potentialAttack.maximumAttackAngle < aiCharacter.aiCharacterCombatManager.viewableAngle)
                    continue;

                potentialAttacks.Add(potentialAttack);
            }

            if (potentialAttacks.Count <= 0)
                return;

            var totalWeight = 0;

            foreach (var attack in potentialAttacks)
            {
                totalWeight += attack.attackWeight;
            }

            var randomWeightValue = Random.Range(1, totalWeight + 1);
            var processedWeight = 0;

            foreach (var attack in potentialAttacks)
            {
                processedWeight += attack.attackWeight;

                if (randomWeightValue <= processedWeight)
                {
                    //this is our attack
                    chosenAttack = attack;
                    previousAttack = chosenAttack;
                    hasAttack = true;
                    return;
                }
            }

        }

        protected virtual bool RollForOutcomeChance(int outcomeChance)
        {
            bool outcomeWillBePerformed = false;

            int randomPercentage = Random.Range(0, 100);

            if (randomPercentage < outcomeChance)
                outcomeWillBePerformed = true;

            return outcomeWillBePerformed;
        }

        protected virtual void SetCirclePath(AICharacterManager aiCharacter)
        {
            if (Physics.CheckSphere(aiCharacter.aiCharacterCombatManager.lockOnTransform.position, aiCharacter.characterController.radius + 0.25f, WorldUtilityManager.Instance.GetEnviroLayers()))
            {
                //stop strafing/circling because we've hit something, instead path towards enemy
                Debug.Log("We are collidong with something, ending strafe");
                aiCharacter.characterAnimatorManager.SetAnimatorMovementParameters(0, Mathf.Abs(strafeMoveAmount));
                return;
            }

            //strafe
            Debug.Log("Strafing");
            aiCharacter.characterAnimatorManager.SetAnimatorMovementParameters(strafeMoveAmount, 0);

            if (hasChoosenCirclePath)
                return;

            hasChoosenCirclePath = true;

            int leftOrRightIndex = Random.Range(0, 100);

            if (leftOrRightIndex >= 50)
            {
                //left
                strafeMoveAmount = -0.5f;
            }
            else
            {
                //right
                strafeMoveAmount = 0.5f;
            }
        }

        protected override void ResetStateFlags(AICharacterManager aiCharacter)
        {
            base.ResetStateFlags(aiCharacter);

            hasAttack = false;
            hasEvaded = false;
            hasRolledForEvasionChance = false;
            hasRolledForComboChance = false;
            hasRolledForBlockChance = false;
            hasChoosenCirclePath = false;
            willBlockDuringThisCombatRotation = false;
            strafeMoveAmount = 0;
        }
    }
}
