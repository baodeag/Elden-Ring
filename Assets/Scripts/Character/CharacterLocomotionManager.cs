using System.Collections;
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

        [Header("Flags")]
        public bool isRolling = false;
        public bool canRotate = true;
        public bool canMove = true;
        public bool canRun = true;
        public bool canRoll = true;
        public bool isGrounded = true;

        [Header("Slope Sliding")]
        [SerializeField] float slopeSlideStartPositionYOffset = 1;
        [SerializeField] float slopeSlideSphereCastMaxDistance = 2;
        private Vector3 slopeSlideVelocity;
        [SerializeField] float slopeSlideSpeed = -11;
        [SerializeField] float slopeSlideSpeedMultiplier = 3;
        [SerializeField] float slipperySurfaceMaxAngle = 15;
        private bool isSliding = false;
        private bool isSlidingOffCharacter = false;
        private Coroutine slideOffCharacterCoroutine;
        private bool slideUntilGrounded = false;
        [SerializeField] float characterSlideOffHeadCollisionMaxDistanceCheck = 5;
        [SerializeField] float characterCollisionCheckSphereMultiplier = 1.5f;

        protected virtual void Awake()
        {
            character = GetComponent<CharacterManager>();
        }

        protected virtual void Update()
        {
            HandleGroundCheck();
            SetGroundedVelocity();
            HandleSlopeSlideCheck();

            if (character.characterLocomotionManager.isGrounded)
            {
                //if we are not attempting to jump or move around
                if (yVelocity.y < 0)
                {
                    inAirTimer = 0;
                    fallingVelocityHasBeenSet = false;
                    yVelocity.y = groundedYVelocity;
                }
            }
            else
            {
                //if we are not jumping and our falling velocity has not been set
                if (!character.characterNetworkManager.isJumping.Value && !fallingVelocityHasBeenSet)
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

        protected void OnControllerColliderHit(ControllerColliderHit hit)
        {
            //if player hit a collider whilist in the air, it will slide until grounded on any step colliders
            if (!isGrounded)
                slideUntilGrounded = true;
        }

        protected void HandleGroundCheck()
        {
            if (isGrounded)
            {
                isGrounded = Physics.CheckSphere(character.transform.position, groundCheckSphereRadius, groundLayer, QueryTriggerInteraction.Ignore);

                if (!isGrounded)
                    OnIsNotGrounded();
            }
            else
            {
                isGrounded = Physics.CheckSphere(character.transform.position, groundCheckSphereRadius, groundLayer, QueryTriggerInteraction.Ignore);

                //if player are jumping or gaining height, player is not grounded
                if (yVelocity.y > 0)
                {
                    isGrounded = false;
                    return;
                }

                if (isGrounded)
                    OnIsGrounded();
            }

        }

        //draws our ground check sphere in editor
        protected void OnDrawGizmosSelected()
        {
            //Gizmos.DrawSphere(character.transform.position, groundCheckSphereRadius);
        }

        public void EnableCanRotate()
        {
            canRotate = true;
        }

        public void DisableCanRotate()
        {
            canRotate = false;
        }

        //slope & sliding

        private void HandleSlopeSlideCheck()
        {
            if (slopeSlideVelocity == Vector3.zero)
                isSliding = false;

            if (!isGrounded && slideUntilGrounded)
            {
                SetSlopeSlideVelocity(WorldUtilityManager.Instance.GetEnviroLayers());
                return;
            }

            if (!isGrounded)
                return;

            SetSlopeSlideVelocity(WorldUtilityManager.Instance.GetSlipperyEnviroLayers());
        }

        //this function determines what our slope slide velocity will be when not grounded
        private void SetSlopeSlideVelocity(LayerMask layers)
        {
            Vector3 startPosition = new Vector3(transform.position.x, transform.position.y + slopeSlideStartPositionYOffset, transform.position.z);

            //use a spherecast to determine the angle of whats below us, and if the angle is too great, we adjust slope slide velocity
            if (Physics.SphereCast
                (startPosition, groundCheckSphereRadius, Vector3.down, out RaycastHit hitinfo, slopeSlideSphereCastMaxDistance, layers))
            {
                float angle = Vector3.Angle(hitinfo.normal, Vector3.up);
                slopeSlideVelocity = Vector3.ProjectOnPlane(new Vector3(0, slopeSlideSpeed, 0), hitinfo.normal);

                if (angle >= slipperySurfaceMaxAngle)
                {
                    slopeSlideVelocity = Vector3.ProjectOnPlane(new Vector3(0, slopeSlideSpeed, 0), hitinfo.normal);
                    return;
                }
            }

            //otherwise, set slope slide velocity to 0
            else
            {
                slopeSlideVelocity = Vector3.zero;
            }

            if (isSliding)
            {
                slopeSlideVelocity -= slopeSlideVelocity * Time.deltaTime * slopeSlideSpeedMultiplier;

                if (slopeSlideVelocity.magnitude > 1)
                    return;
            }

            slopeSlideVelocity = Vector3.zero;
        }

        private void SetGroundedVelocity()
        {
            if (slopeSlideVelocity != Vector3.zero)
            {
                //if player is in processing of jumping, and jump is still gaining height , do not slide off surface
                if (character.characterNetworkManager.isJumping.Value && yVelocity.y > 0)
                {
                    isSliding = false;
                }
                else
                {
                    isSliding = true;
                }
            }

            if (isSliding)
            {
                yVelocity.y += WorldUtilityManager.Instance.slopeSlideForce * Time.deltaTime;
                Vector3 slideVelocity = slopeSlideVelocity;

                if (character.characterController.enabled)
                    character.characterController.Move(slideVelocity * Time.deltaTime);
            }

            if (isGrounded)
            {
                if (yVelocity.y <= 0 && !isSliding)
                    yVelocity.y = groundedYVelocity;
            }
            else if (!isGrounded && !isSlidingOffCharacter)
            {
                Collider[] characterColliders = 
                    Physics.OverlapSphere(transform.position, 
                    groundCheckSphereRadius * characterCollisionCheckSphereMultiplier, 
                    WorldUtilityManager.Instance.GetCharacterLayers());

                for (int i = 0; i < characterColliders.Length; i++)
                {
                    if (characterColliders[i].gameObject.transform.root == character.gameObject.transform.root)
                        continue;

                    CharacterController controller = characterColliders[i].GetComponent<CharacterController>();

                    if (controller == null)
                        continue;

                    if ((controller.collisionFlags & CollisionFlags.CollidedBelow) != 0)
                    {
                        isSlidingOffCharacter = true;
                        SlideOffCharacter();
                    }
                }
            }

            if (!character.characterController.enabled)
                return;

            //this is a desync prevention measure
            if (!character.IsOwner)
            {
                float distance = Vector3.Distance(transform.position, character.characterNetworkManager.networkPosition.Value);

                if (distance > 2.5f)
                {
                    yVelocity = Vector3.zero;
                    character.transform.position = character.characterNetworkManager.networkPosition.Value;
                }
            }
        }

        //character sliding
        protected virtual void SlideOffCharacter()
        {
            if (slideOffCharacterCoroutine != null) 
                StopCoroutine(slideOffCharacterCoroutine);

            slideOffCharacterCoroutine = StartCoroutine(SlideOffCharacterCoroutine());
        }

        protected virtual IEnumerator SlideOffCharacterCoroutine()
        {
            while (!isGrounded)
            {
                if (Physics.SphereCast(character.transform.position, 
                    groundCheckSphereRadius, Vector3.down, out RaycastHit hitInfo, 
                    characterSlideOffHeadCollisionMaxDistanceCheck,
                    WorldUtilityManager.Instance.GetCharacterLayers()))
                {
                    Vector3 characterSlideVelocity = Vector3.ProjectOnPlane(new Vector3(0, yVelocity.y, 0), hitInfo.normal);
                    yVelocity.y += WorldUtilityManager.Instance.slopeSlideForce * Time.deltaTime;
                    Vector3 slideVelocity = characterSlideVelocity;

                    if (character.characterController.enabled)
                        character.characterController.Move(slideVelocity * Time.deltaTime);

                    yield return null;
                }

                yield return null;
            }

            isSlidingOffCharacter = false;

            yield return null;
        }

        //on is/not grounded
        protected virtual void OnIsGrounded()
        {
            slideUntilGrounded = false;
        }

        protected virtual void OnIsNotGrounded()
        {

        }
    }
}