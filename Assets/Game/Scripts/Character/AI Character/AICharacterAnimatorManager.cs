using UnityEngine;

namespace baodeag
{
    public class AICharacterAnimatorManager : CharacterAnimatorManager
    {
        AICharacterManager aiCharacter;

        protected override void Awake()
        {
            base.Awake();
            aiCharacter = GetComponent<AICharacterManager>();
        }

        private void OnAnimatorMove()
        {
            if (aiCharacter.isDead.Value)
                return;

            //host
            if (aiCharacter.IsOwner)
            {
                if (!aiCharacter.characterLocomotionManager.isGrounded)
                    return;

                Vector3 velocity = GetAnimationOrNavMeshDelta();

                aiCharacter.characterController.Move(velocity);
                aiCharacter.transform.rotation *= aiCharacter.animator.deltaRotation;
            }
            //client
            else
            {
                if (!aiCharacter.characterLocomotionManager.isGrounded)
                    return;

                Vector3 velocity = GetAnimationOrNavMeshDelta();

                aiCharacter.characterController.Move(velocity);
                aiCharacter.transform.position = Vector3.SmoothDamp(
                    transform.position, 
                    aiCharacter.characterNetworkManager.networkPosition.Value, 
                    ref aiCharacter.characterNetworkManager.networkPositionVelocity,
                    aiCharacter.characterNetworkManager.networkPositionSmoothTime);
                aiCharacter.transform.rotation *= aiCharacter.animator.deltaRotation;
            }
        }

        private Vector3 GetAnimationOrNavMeshDelta()
        {
            Vector3 deltaPosition = aiCharacter.animator.deltaPosition;

            if (!aiCharacter.ShouldUseNavMeshTranslationForInPlaceAnimations())
                return deltaPosition;

            if (aiCharacter.isPerformingAction)
                return deltaPosition;

            if (aiCharacter.navMeshAgent == null || !aiCharacter.navMeshAgent.enabled)
                return deltaPosition;

            if (deltaPosition.sqrMagnitude > 0.000001f)
                return deltaPosition;

            Vector3 desiredVelocity = aiCharacter.navMeshAgent.desiredVelocity;
            desiredVelocity.y = 0f;

            return desiredVelocity * Time.deltaTime;
        }
    }
}
