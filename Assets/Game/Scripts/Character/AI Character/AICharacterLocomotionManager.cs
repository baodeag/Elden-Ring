using UnityEngine;

namespace baodeag
{
    public class AICharacterLocomotionManager : CharacterLocomotionManager
    {
        const float MinimumMovementThreshold = 0.01f;

        AICharacterManager aiCharacter;

        protected override void Awake()
        {
            base.Awake();

            aiCharacter = GetComponent<AICharacterManager>();
        }

        public void RotateTowardsAgent(AICharacterManager aiCharacter)
        {
            if (aiCharacter.aiCharacterNetworkManager.isMoving.Value)
            {
                aiCharacter.transform.rotation = aiCharacter.navMeshAgent.transform.rotation;
            }
        }

        protected override void Update()
        {
            base.Update();

            if (aiCharacter.IsOwner)
            {
                UpdateOwnerAnimatorMovementParameters();
            }
            else
            {
                aiCharacter.animator.SetFloat("Vertical", aiCharacter.aiCharacterNetworkManager.verticalMovement.Value, 0.1f, Time.deltaTime);
                aiCharacter.animator.SetFloat("Horizontal", aiCharacter.aiCharacterNetworkManager.horizontalMovement.Value, 0.1f, Time.deltaTime);
            }
        }

        void UpdateOwnerAnimatorMovementParameters()
        {
            float horizontalMovement = 0f;
            float verticalMovement = 0f;

            if (aiCharacter.navMeshAgent != null && aiCharacter.navMeshAgent.enabled)
            {
                Vector3 desiredVelocity = aiCharacter.navMeshAgent.desiredVelocity;
                desiredVelocity.y = 0f;

                if (desiredVelocity.sqrMagnitude > MinimumMovementThreshold * MinimumMovementThreshold)
                {
                    Vector3 localDesiredVelocity = aiCharacter.transform.InverseTransformDirection(desiredVelocity.normalized);
                    horizontalMovement = Mathf.Clamp(localDesiredVelocity.x, -1f, 1f);
                    verticalMovement = Mathf.Clamp(localDesiredVelocity.z, -1f, 1f);

                    // In-place packs like Monster30 need a forward locomotion signal even when
                    // navigation is translating the character instead of root motion.
                    if (Mathf.Abs(verticalMovement) < MinimumMovementThreshold)
                    {
                        verticalMovement = 1f;
                    }
                }
            }

            aiCharacter.animator.SetFloat("Vertical", verticalMovement, 0.1f, Time.deltaTime);
            aiCharacter.animator.SetFloat("Horizontal", horizontalMovement, 0.1f, Time.deltaTime);
            aiCharacter.characterNetworkManager.verticalMovement.Value = verticalMovement;
            aiCharacter.characterNetworkManager.horizontalMovement.Value = horizontalMovement;
        }
    }
}
