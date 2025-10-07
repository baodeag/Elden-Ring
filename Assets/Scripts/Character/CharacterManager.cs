using UnityEngine;
using Unity.Netcode;
using System.Collections;
using NUnit.Framework;
using System.Collections.Generic;

namespace baodeag
{
    public class CharacterManager : NetworkBehaviour
    {
        [Header("Status")]
        public NetworkVariable<bool> isDead = new NetworkVariable<bool>
            (false,
            NetworkVariableReadPermission.Everyone,
            NetworkVariableWritePermission.Owner);
        [HideInInspector] public CharacterController characterController;
        [HideInInspector] public Animator animator;

        [HideInInspector] public CharacterNetworkManager characterNetworkManager;
        [HideInInspector] public CharacterEffectsManager characterEffectsManager;
        [HideInInspector] public CharacterAnimatorManager characterAnimatorManager;
        [HideInInspector] public CharacterCombatManager characterCombatManager;
        [HideInInspector] public CharacterSoundFXManager characterSoundFXManager;
        [HideInInspector] public CharacterLocomotionManager characterLocomotionManager;

        [Header("Character Group")]
        public CharacterGroup characterGroup;

        [Header("Flags")]
        public bool isPerformingAction = false;

        protected virtual void Awake()
        {
            DontDestroyOnLoad(this);

            characterController = GetComponent<CharacterController>();
            animator = GetComponent<Animator>();
            characterNetworkManager = GetComponent<CharacterNetworkManager>();
            characterEffectsManager = GetComponent<CharacterEffectsManager>();
            characterAnimatorManager = GetComponent<CharacterAnimatorManager>();
            characterCombatManager = GetComponent<CharacterCombatManager>();
            characterSoundFXManager = GetComponent<CharacterSoundFXManager>();
            characterLocomotionManager = GetComponent<CharacterLocomotionManager>();
        }

        protected virtual void Start()
        {
            IgnoreMyOwnColliders();
        }

        protected virtual void Update()
        {
            animator.SetBool("IsGrounded", characterLocomotionManager.isGrounded);

            //if this char is being controlled by the local player, update the networked position
            if (IsOwner)
            {
                characterNetworkManager.networkPosition.Value = transform.position;
                characterNetworkManager.networkRotation.Value = transform.rotation;
            }
            //if this char is being controlled by another player, interpolate to the networked position
            else
            {
                // position
                transform.position = Vector3.SmoothDamp
                    (transform.position, 
                    characterNetworkManager.networkPosition.Value, 
                    ref characterNetworkManager.networkPositionVelocity, 
                    characterNetworkManager.networkPositionSmoothTime);
                
                // rotation
                transform.rotation = Quaternion.Slerp
                    (transform.rotation, 
                    characterNetworkManager.networkRotation.Value, 
                    characterNetworkManager.networkRotationSmoothTime);
            }
        }

        protected virtual void FixedUpdate()
        {
            
        }

        protected virtual void LateUpdate()
        {
            //if we own this char, handle the camera actions
            if (IsOwner)
            {
                PlayerCamera.instance.HandleAllCameraActions();
            }
        }

        public override void OnNetworkSpawn()
        {
            base.OnNetworkSpawn();

            characterNetworkManager.isMoving.OnValueChanged += characterNetworkManager.OnIsMovingChanged;
        }

        public override void OnNetworkDespawn()
        {
            base.OnNetworkDespawn();

            characterNetworkManager.isMoving.OnValueChanged -= characterNetworkManager.OnIsMovingChanged;
        }

        public virtual IEnumerator ProcessDeathEvent(bool manuallySelectDeathAnimation = false)
        {
            if (IsOwner)
            {
                characterNetworkManager.currentHealth.Value = 0;
                isDead.Value = true;

                //reset any flags here that need to be reset on death

                if(!manuallySelectDeathAnimation)
                {
                    characterAnimatorManager.PlayTargetActionAnimation("Dead_01", true);
                }
            }

            yield return new WaitForSeconds(5);
        }

        public virtual void ReviveCharacter()
        {

        }

        protected virtual void IgnoreMyOwnColliders()
        {
            Collider characterControllerCollider = GetComponent<Collider>();
            Collider[] damageableCharacterColliders = GetComponentsInChildren<Collider>();
            List<Collider> ignoreColliders = new List<Collider>();

            //add all of our damageable colliders to the ignore list
            foreach (var collider in damageableCharacterColliders)
            {
                ignoreColliders.Add(collider);
            }

            //add our character controller collider to the ignore list
            ignoreColliders.Add(characterControllerCollider);

            //go through every collider on the list, and ignores collisions with each other
            foreach (var collider in ignoreColliders)
            {
                foreach (var otherCollider in ignoreColliders)
                {
                    if (collider != otherCollider)
                    {
                        Physics.IgnoreCollision(collider, otherCollider, true);
                    }
                }
            }
        }
    }
}
