using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace baodeag
{
    public class PlayerInputManager : MonoBehaviour
    {
        public static PlayerInputManager instance;
        public PlayerManager player;

        PlayerControls playerControls;

        [Header("Camera Input")]
        [SerializeField] Vector2 camera_Input;
        public float cameraHorizontal_Input;
        public float cameraVertical_Input;

        [Header("Lock On Input")]
        [SerializeField] bool lockOn_Input;

        [Header("Player Movement Input")]
        [SerializeField] Vector2 movement_Input;
        public float horizontal_Input;
        public float vertical_Input;
        public float moveAmount;

        [Header("Player Action Input")]
        [SerializeField] bool dodge_Input = false;
        [SerializeField] bool sprint_Input = false;
        [SerializeField] bool jump_Input = false;
        [SerializeField] bool RB_Input = false;


        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }
            else
            {
                Destroy(gameObject);
            }

        }

        private void Start()
        {
            DontDestroyOnLoad(gameObject);

            //when scene changes, call OnSceneChange
            SceneManager.activeSceneChanged += OnSceneChange;

            instance.enabled = false; //disable player controls by default

            if (playerControls != null)
            {
                playerControls.Disable();
            }

        }

        private void OnSceneChange(Scene oldScene, Scene newScene)
        {
            //if we are loading into our world scene, enable our player controls
            if (newScene.buildIndex == WorldSaveGameManager.instance.GetWorldSceneIndex())
            {
                instance.enabled = true;

                if (playerControls != null)
                {
                    playerControls.Enable();
                }
            }
            //otherwise disable them
            //this is so our player cant move around in menus or other scenes
            else
            {
                instance.enabled = false;

                if (playerControls != null)
                {
                    playerControls.Disable();
                }
            }
        }

        private void OnEnable()
        {
            if (playerControls == null)
            {
                playerControls = new PlayerControls();

                playerControls.PlayerMovement.Movement.performed += i => movement_Input = i.ReadValue<Vector2>(); // Get movement input
                playerControls.PlayerCamera.Movement.performed += i => camera_Input = i.ReadValue<Vector2>(); // Get camera input
                playerControls.PlayerActions.Dodge.performed += i => dodge_Input = true; // Get dodge input
                playerControls.PlayerActions.Jump.performed += i => jump_Input = true;
                playerControls.PlayerActions.RB.performed += i => RB_Input = true; // Get RB input

                playerControls.PlayerActions.LockOn.performed += i => lockOn_Input = true;


                playerControls.PlayerActions.Sprint.performed += i => sprint_Input = true; // Get sprint input
                playerControls.PlayerActions.Sprint.canceled += i => sprint_Input = false; // Get sprint input
            }

            playerControls.Enable();
        }

        private void OnDestroy()
        {
            //when this object is destroyed, unsubscribe from the event
            SceneManager.activeSceneChanged -= OnSceneChange;
        }

        // if we minimize or lower the window, stop adjusting input
        private void OnApplicationFocus(bool focus)
        {
            if (enabled)
            {
                if (focus)
                {
                    playerControls.Enable();
                }
                else
                {
                    playerControls.Disable();
                }
            }
        }

        private void Update()
        {
            HandleAllInputs();
        }

        private void HandleAllInputs()
        {
            HandleLockOnInput();
            HandlePlayerMovementInput();
            HandleCameraMovementInput();
            HandleDodgeInput();
            HandleSprintInput();
            HandleJumpInput();
            HandleRBInput();
        }

        //Lock On
        private void HandleLockOnInput()
        {
            if (player.playerNetworkManager.isLockedOn.Value)
            {
                if (player.playerCombatManager.currentTarget != null)
                    return;

                if (player.playerCombatManager.currentTarget.isDead.Value)
                {
                    //stop locking on to a dead target
                    player.playerNetworkManager.isLockedOn.Value = false;
                }
            }

            if (lockOn_Input && player.playerNetworkManager.isLockedOn.Value)
            {
                lockOn_Input = false;
                return;
            }

            if (lockOn_Input && !player.playerNetworkManager.isLockedOn.Value)
            {
                lockOn_Input = false;

                PlayerCamera.instance.HandleLocatingLockOnTarget();
            }
        }

        //Movement

        private void HandlePlayerMovementInput()
        {
            vertical_Input = movement_Input.y;
            horizontal_Input = movement_Input.x;

            //returns a value between 0 and 1
            moveAmount = Mathf.Clamp01(Mathf.Abs(horizontal_Input) + Mathf.Abs(vertical_Input));

            //snap moveAmount to either 0, 0.5, or 1
            if (moveAmount <= 0.5 && moveAmount > 0)
            {
                moveAmount = 0.5f; //walk
            }
            else if (moveAmount > 0.5 && moveAmount <= 1)
            {
                moveAmount = 1; //run
            }

            // why do we pass 0 on the horizontal? because we only want non-strafing movement
            // we  use horizontal when we are strafing or locked on
            if (player == null)
                return;
            // if we are not locked on, only use the move amount
            player.playerAnimatorManager.UpdateAnimatorMovementParameters(
                0,
                moveAmount,
                player.playerNetworkManager.isSprinting.Value);

            // if we are locked on pass the horizontal movement as well

        }

        private void HandleCameraMovementInput()
        {
            cameraHorizontal_Input = camera_Input.x;
            cameraVertical_Input = camera_Input.y;
        }

        //Action

        private void HandleDodgeInput()
        {
            if (dodge_Input)
            {
                dodge_Input = false;
                player.playerLocomotionManager.AttemptToPerformDodge();
            }
        }

        private void HandleSprintInput()
        {
            if (sprint_Input)
            {
                player.playerLocomotionManager.HandleSprinting();
            }
            else
            {
                player.playerNetworkManager.isSprinting.Value = false;
            }
        }

        private void HandleJumpInput()
        {
            if (jump_Input)
            {
                jump_Input = false;

                player.playerLocomotionManager.AttemptToPerformJump();
            }
        }

        private void HandleRBInput()
        {
            if (RB_Input)
            {
                RB_Input = false;

                player.playerNetworkManager.SetCharacterActionHand(true);

                player.playerCombatManager.PerformWeaponBasedAction(
                    player.playerInventoryManager.currentRightHandWeapon.oh_RB_Action,
                    player.playerInventoryManager.currentRightHandWeapon);
            }
        }
    }
}
