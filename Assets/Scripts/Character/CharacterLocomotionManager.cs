using UnityEngine;

namespace baodeag
{
    public class CharacterLocomotionManager : MonoBehaviour
    {
        CharacterManager character;

        [Header("Ground Check & Jumping")]
        [SerializeField] protected float gravityForce = -5.55f; 
        [SerializeField] LayerMask groundLayer;
        [SerializeField] float groundCheckSphereRadius = 1; 
        [SerializeField] protected Vector3 yVelocity; //the force at which our character is pulled up or down (jumping/falling)
        [SerializeField] protected float groundedYVelocity = -20; //the force at which our character is sticking to the ground whilist they arr grounded
        [SerializeField] protected float fallStartYVelocity = -5; //the force at which our character starts to fall when they become ungrounded
        protected bool fallingVelocityHasBeenSet = false;
        [SerializeField] protected float inAirTimer = 0;


        protected virtual void Awake()
        {
            character = GetComponent<CharacterManager>();
        }

        protected virtual void Update()
        {
            HandleGroundCheck();

            if (character.isGrounded)
            {
                //if we are not attempting to jump or move around
                if(yVelocity.y < 0)
                {
                    inAirTimer = 0;
                    fallingVelocityHasBeenSet = false;
                    yVelocity.y = groundedYVelocity;
                }
            }
            else
            {
                //if we are not jumping and our falling velocity has not been set
                if(!character.isJumping && !fallingVelocityHasBeenSet)
                {
                    fallingVelocityHasBeenSet = true;
                    yVelocity.y = fallStartYVelocity;
                }

                inAirTimer += Time.deltaTime;
                character.animator.SetFloat("InAirTimer", inAirTimer);

                yVelocity.y += gravityForce * Time.deltaTime;
            }
            character.characterController.Move(yVelocity * Time.deltaTime);
        }

        protected void HandleGroundCheck()
        {
            character.isGrounded = Physics.CheckSphere(character.transform.position, groundCheckSphereRadius, groundLayer);
        }

        //draws our ground check sphere in editor
        protected void OnDrawGizmosSelected()
        {
            Gizmos.DrawSphere(character.transform.position, groundCheckSphereRadius);
        }
    }
}