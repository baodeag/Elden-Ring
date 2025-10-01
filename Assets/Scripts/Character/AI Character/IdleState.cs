using UnityEngine;

namespace baodeag
{
    [CreateAssetMenu(menuName = "A.I/States/Idle")]
    public class IdleState : AIState
    {
        public override AIState Tick(AICharacterManager aiCharacter)
        { 
            if (aiCharacter.characterCombatManager.currentTarget != null)
            {
                Debug.Log("We have a target");
                return this;
            }
            else
            {
                aiCharacter.aiCharacterCombatManager.FindATargetViaLineOfSight(aiCharacter);
                Debug.Log("Searching for a target");
                return this;
            }

        }
    }
}
