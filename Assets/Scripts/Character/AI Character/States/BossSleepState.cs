using UnityEngine;

namespace baodeag
{
    [CreateAssetMenu(menuName = "A.I/States/Boss Sleep")]
    public class BossSleepState : AIState
    {
        private bool sleepAnimationSet = false;
        [SerializeField] string sleepAnimation = "Sleep_01";
        [SerializeField] string wakingAnimation = "Wake_01";

        public bool hasBeenAwakened = false;

        public override AIState Tick(AICharacterManager aiCharacter)
        {
            //if the boss has already been initially "woken up", we dont want to replay the first time waking animation
            aiCharacter.navMeshAgent.enabled = false;

            if (!hasBeenAwakened)
            {
                return HasNotBeenAwakened(aiCharacter);
            }
            else
            {
                return HasBeenAwakened(aiCharacter);
            }
        }

        private AIState HasBeenAwakened(AICharacterManager aiCharacter)
        {
            if (aiCharacter.characterCombatManager.currentTarget != null && !aiCharacter.aiCharacterNetworkManager.isAwake.Value)
            {
                aiCharacter.aiCharacterNetworkManager.isAwake.Value = true;
                return SwitchState(aiCharacter, aiCharacter.pursueTarget);
            }

            return this;
        }

        private AIState HasNotBeenAwakened(AICharacterManager aiCharacter)
        {
            aiCharacter.navMeshAgent.enabled = false;

            if (!sleepAnimationSet && !aiCharacter.aiCharacterNetworkManager.isAwake.Value)
            {
                sleepAnimationSet = true;
                aiCharacter.aiCharacterNetworkManager.sleepingAnimation.Value = sleepAnimation;
                aiCharacter.aiCharacterNetworkManager.wakingAnimation.Value = wakingAnimation;
                aiCharacter.characterAnimatorManager.PlayTargetActionAnimation(aiCharacter.aiCharacterNetworkManager.sleepingAnimation.Value.ToString(), true);
            }

            if (aiCharacter.characterCombatManager.currentTarget != null && !aiCharacter.aiCharacterNetworkManager.isAwake.Value)
            {
                aiCharacter.aiCharacterNetworkManager.isAwake.Value = true;

                if (!aiCharacter.isPerformingAction && !aiCharacter.isDead.Value)
                    aiCharacter.characterAnimatorManager.PlayTargetActionAnimation(aiCharacter.aiCharacterNetworkManager.wakingAnimation.Value.ToString(), true);

                return SwitchState(aiCharacter, aiCharacter.pursueTarget);
            }

            return this;
        }
    }
}
