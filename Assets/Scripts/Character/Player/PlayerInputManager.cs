using System;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;
using Unity.Netcode;

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
        [SerializeField] bool lockOn_Left_Input;
        [SerializeField] bool lockOn_Right_Input;
        private Coroutine lockOnCoroutine;

        [Header("Player Movement Input")]
        [SerializeField] Vector2 movement_Input;
        public float horizontal_Input;
        public float vertical_Input;
        public float moveAmount;

        [Header("Player Action Inputs")]
        [SerializeField] bool dodge_Input = false;
        [SerializeField] bool sprint_Input = false;
        [SerializeField] bool jump_Input = false;
        [SerializeField] bool switch_Right_Weapon_Input = false;
        [SerializeField] bool switch_Left_Weapon_Input = false;
        [SerializeField] bool switch_Quick_Slot_Item_Input = false;
        [SerializeField] bool interaction_Input = false;
        [SerializeField] bool use_Item_Input = false;

        [Header("Bumper Inputs")]
        [SerializeField] bool RB_Input = false;
        [SerializeField] bool hold_RB_Input = false;
        [SerializeField] bool LB_Input = false;
        [SerializeField] bool hold_LB_Input = false;

        [Header("Trigger Inputs")]
        [SerializeField] bool RT_Input = false;
        [SerializeField] bool Hold_RT_Input = false;
        [SerializeField] bool LT_Input = false;

        [Header("Two Hand Inputs")]
        [SerializeField] bool two_Hand_Input = false;
        [SerializeField] bool two_Hand_Right_Weapon_Input = false;
        [SerializeField] bool two_Hand_Left_Weapon_Input = false;

        [Header("Qued Inputs")]
        [SerializeField] private bool input_Que_Is_Active = false;
        [SerializeField] float default_Que_Input_Time = 0.35f;
        [SerializeField] float que_Input_Timer = 0;
        [SerializeField] bool que_RB_Input = false;
        [SerializeField] bool que_RT_Input = false;

        [Header("UI Inputs")]
        [SerializeField] bool openCharacterMenuInput = false;
        [SerializeField] bool closeMenuInput = false;

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

                //actions
                playerControls.PlayerActions.Dodge.performed += i => dodge_Input = true; // Get dodge input
                playerControls.PlayerActions.Jump.performed += i => jump_Input = true;
                playerControls.PlayerActions.SwitchRightWeapon.performed += i => switch_Right_Weapon_Input = true;
                playerControls.PlayerActions.SwitchLeftWeapon.performed += i => switch_Left_Weapon_Input = true;
                playerControls.PlayerActions.SwitchQuickSlotItem.performed += i => switch_Quick_Slot_Item_Input = true;
                playerControls.PlayerActions.Interact.performed += i => interaction_Input = true;
                playerControls.PlayerActions.X.performed += i => use_Item_Input = true;

                //bumper
                playerControls.PlayerActions.RB.performed += i => RB_Input = true; // Get RB input
                playerControls.PlayerActions.HoldRB.performed += i => hold_RB_Input = true;
                playerControls.PlayerActions.HoldRB.canceled += i => hold_RB_Input = false;

                playerControls.PlayerActions.LB.performed += i => LB_Input = true; // Get LB input
                playerControls.PlayerActions.LB.canceled += i => player.playerNetworkManager.isBlocking.Value = false;
                playerControls.PlayerActions.LB.canceled += i => player.playerNetworkManager.isAiming.Value = false;
                playerControls.PlayerActions.HoldLB.performed += i => hold_LB_Input = true;
                playerControls.PlayerActions.HoldLB.canceled += i => hold_LB_Input = false;


                //triggers
                playerControls.PlayerActions.RT.performed += i => RT_Input = true;
                playerControls.PlayerActions.HoldRT.performed += i => Hold_RT_Input = true;
                playerControls.PlayerActions.HoldRT.canceled += i => Hold_RT_Input = false;
                playerControls.PlayerActions.LT.performed += i => LT_Input = true;

                //two hand
                playerControls.PlayerActions.TwoHandWeapon.performed += i => two_Hand_Input = true;
                playerControls.PlayerActions.TwoHandWeapon.canceled += i => two_Hand_Input = false;
                playerControls.PlayerActions.TwoHandRightWeapon.performed += i => two_Hand_Right_Weapon_Input = true;
                playerControls.PlayerActions.TwoHandRightWeapon.canceled += i => two_Hand_Right_Weapon_Input = false;
                playerControls.PlayerActions.TwoHandLeftWeapon.performed += i => two_Hand_Left_Weapon_Input = true;
                playerControls.PlayerActions.TwoHandLeftWeapon.canceled += i => two_Hand_Left_Weapon_Input = false;

                //lock on
                playerControls.PlayerActions.LockOn.performed += i => lockOn_Input = true;
                playerControls.PlayerActions.SeekLeftLockOnTarget.performed += i => lockOn_Left_Input = true;
                playerControls.PlayerActions.SeekRightLockOnTarget.performed += i => lockOn_Right_Input = true;

                //sprint
                playerControls.PlayerActions.Sprint.performed += i => sprint_Input = true;
                playerControls.PlayerActions.Sprint.canceled += i => sprint_Input = false;

                //qued inputs
                playerControls.PlayerActions.QueRB.performed += i => QueInput(ref que_RB_Input);
                playerControls.PlayerActions.QueRT.performed += i => QueInput(ref que_RT_Input);

                //ui inputs
                playerControls.PlayerActions.Dodge.performed += i => closeMenuInput = true;
                playerControls.PlayerActions.OpenCharacterMenu.performed += i => openCharacterMenuInput = true;
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
            HandleUseItemInput();
            HandleTwoHandInput();
            HandleLockOnInput();
            HandleLockOnSwitchTargetInput();
            HandlePlayerMovementInput();
            HandleCameraMovementInput();
            HandleDodgeInput();
            HandleSprintInput();
            HandleJumpInput();
            HandleRBInput();
            HandleHoldRBInput();
            HandleLBInput();
            HandleHoldLBInput();
            HandleRTInput();
            HandleChargeRTInput();
            HandleLTInput();
            HandleSwitchRightWeaponInput();
            HandleSwitchLeftWeaponInput();
            HandleSwitchQuickSlotItemInput();
            HandleInteractionInput();
            HandleQuedInput();
            HandleCloseUIInputs();
            HandleOpenCharacterMenuInput();
        }

        //use item
        private void HandleUseItemInput()
        {
            if (use_Item_Input)
            {
                use_Item_Input = false;
                
                if (PlayerUIManager.instance.menuWindowIsOpen)
                    return;

                if (player.playerInventoryManager.currentQuickSlotItem != null)
                {
                    player.playerInventoryManager.currentQuickSlotItem.AttemptToUseItem(player);

                    //send server rpc so our player perform item action on other clients game windows
                    player.playerNetworkManager.NotifyServerOfQuickSlotItemActionServerRpc
                        (NetworkManager.Singleton.LocalClientId, player.playerInventoryManager.currentQuickSlotItem.itemID);
                }
            }
        }

        //two hand
        private void HandleTwoHandInput()
        {
            if (!two_Hand_Input)
                return;

            if (two_Hand_Right_Weapon_Input)
            {
                RB_Input = false;
                two_Hand_Right_Weapon_Input = false;
                player.playerNetworkManager.isBlocking.Value = false;

                if (player.playerNetworkManager.isTwoHandingWeapon.Value)
                {
                    //if we are two handing a weapon already, change the is twohanding bool to false which triggers an onvaluedchange function, which un-twohanded current weapon
                    player.playerNetworkManager.isTwoHandingWeapon.Value = false;
                    return;
                }
                else
                {
                    //if we are not already two handing a weapon, change the is twohanding bool to true which triggers an onvaluedchange function, which twohands the right weapon
                    player.playerNetworkManager.isTwoHandingRightWeapon.Value = true;
                    return;
                }
            }
            else if (two_Hand_Left_Weapon_Input)
            {
                LB_Input = false;
                two_Hand_Left_Weapon_Input = false;
                player.playerNetworkManager.isBlocking.Value = false;
                if (player.playerNetworkManager.isTwoHandingWeapon.Value)
                {
                    //if we are two handing a weapon already, change the is twohanding bool to false which triggers an onvaluedchange function, which un-twohanded current weapon
                    player.playerNetworkManager.isTwoHandingWeapon.Value = false;
                    return;
                }
                else
                {
                    //if we are not already two handing a weapon, change the is twohanding bool to true which triggers an onvaluedchange function, which twohands the left weapon
                    player.playerNetworkManager.isTwoHandingLeftWeapon.Value = true;
                    return;
                }
            }
        }

        //Lock On
        //private void HandleLockOnInput()
        //{
        //    if (player.playerNetworkManager.isLockedOn.Value)
        //    {
        //        if (player.playerCombatManager.currentTarget != null)
        //            return;

        //        if (player.playerCombatManager.currentTarget.isDead.Value)
        //        {
        //            //stop locking on to a dead target
        //            player.playerNetworkManager.isLockedOn.Value = false;
        //        }

        //        if (lockOnCoroutine != null)
        //            StopCoroutine(lockOnCoroutine);
        //        lockOnCoroutine = StartCoroutine(PlayerCamera.instance.WaitThenFindNewTarget());
        //    }

        //    if (lockOn_Input && player.playerNetworkManager.isLockedOn.Value)
        //    {
        //        lockOn_Input = false;
        //        PlayerCamera.instance.ClearLockOnTargets();
        //        player.playerNetworkManager.isLockedOn.Value = false;
        //        return;
        //    }

        //    if (lockOn_Input && !player.playerNetworkManager.isLockedOn.Value)
        //    {
        //        lockOn_Input = false;

        //        PlayerCamera.instance.HandleLocatingLockOnTarget();

        //        if (PlayerCamera.instance.nearestLockOnTarget != null)
        //        {
        //            player.playerCombatManager.SetTarget(PlayerCamera.instance.nearestLockOnTarget);
        //            player.playerNetworkManager.isLockedOn.Value = true;
        //        }
        //    }
        //}

        private void HandleLockOnInput()
        {
            // 1) Ưu tiên toggle theo phím R
            if (lockOn_Input)
            {
                lockOn_Input = false;

                if (player.playerNetworkManager.isLockedOn.Value)
                {
                    // TẮT lock-on
                    player.playerNetworkManager.isLockedOn.Value = false;

                    // dừng coroutine (nếu đang chạy)
                    if (lockOnCoroutine != null)
                    {
                        StopCoroutine(lockOnCoroutine);
                        lockOnCoroutine = null;
                    }

                    // dọn target & camera
                    if (player.playerCombatManager != null)
                        player.playerCombatManager.currentTarget = null;

                    if (PlayerCamera.instance != null)
                        PlayerCamera.instance.ClearLockOnTargets();

                    return;
                }
                else
                {
                    // BẬT lock-on: tìm target ngay lập tức
                    if (PlayerCamera.instance != null)
                    {
                        PlayerCamera.instance.HandleLocatingLockOnTarget();

                        if (PlayerCamera.instance.nearestLockOnTarget != null &&
                            player.playerCombatManager != null)
                        {
                            player.playerCombatManager.SetTarget(PlayerCamera.instance.nearestLockOnTarget);
                            player.playerNetworkManager.isLockedOn.Value = true;

                            // BẮT ĐẦU coroutine tìm/chuyển target sau một nhịp (nếu bạn muốn cập nhật mềm)
                            if (lockOnCoroutine != null)
                                StopCoroutine(lockOnCoroutine);
                            lockOnCoroutine = StartCoroutine(PlayerCamera.instance.WaitThenFindNewTarget());
                        }
                        else
                        {
                            // Không có target -> đảm bảo trạng thái off
                            player.playerNetworkManager.isLockedOn.Value = false;

                            if (lockOnCoroutine != null)
                            {
                                StopCoroutine(lockOnCoroutine);
                                lockOnCoroutine = null;
                            }
                        }
                    }
                    return;
                }
            }

            // 2) Nếu đang lock-on, tự thoát khi target không hợp lệ (null/chết)
            if (player.playerNetworkManager.isLockedOn.Value)
            {
                var t = player.playerCombatManager != null ? player.playerCombatManager.currentTarget : null;

                bool invalid = (t == null) || (t.isDead != null && t.isDead.Value);
                if (invalid)
                {
                    player.playerNetworkManager.isLockedOn.Value = false;

                    if (lockOnCoroutine != null)
                    {
                        StopCoroutine(lockOnCoroutine);
                        lockOnCoroutine = null;
                    }

                    if (player.playerCombatManager != null)
                        player.playerCombatManager.currentTarget = null;

                    if (PlayerCamera.instance != null)
                        PlayerCamera.instance.ClearLockOnTargets();
                }
            }
        }

        private void HandleLockOnSwitchTargetInput()
        {
            if (lockOn_Left_Input)
            {
                lockOn_Left_Input = false;
                
                if (player.playerNetworkManager.isLockedOn.Value)
                {
                    PlayerCamera.instance.HandleLocatingLockOnTarget();

                    if (PlayerCamera.instance.leftLockOnTarget != null)
                    {
                        player.playerCombatManager.SetTarget(PlayerCamera.instance.leftLockOnTarget);
                    }
                }
            }

            if (lockOn_Right_Input)
            {
                lockOn_Right_Input = false;

                if (player.playerNetworkManager.isLockedOn.Value)
                {
                    PlayerCamera.instance.HandleLocatingLockOnTarget();

                    if (PlayerCamera.instance.rightLockOnTarget != null)
                    {
                        player.playerCombatManager.SetTarget(PlayerCamera.instance.rightLockOnTarget);
                    }
                }
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

            if (moveAmount != 0)
            {
                player.playerNetworkManager.isMoving.Value = true;
            }
            else
            {
                player.playerNetworkManager.isMoving.Value = false;
            }

            if (!player.playerLocomotionManager.canRun)
            {
                if (moveAmount > 0.5f)
                    moveAmount = 0.5f;

                if (vertical_Input > 0.5f)
                    vertical_Input = 0.5f;

                if (horizontal_Input > 0.5f)
                    horizontal_Input = 0.5f;
            }


            if (player.playerNetworkManager.isLockedOn.Value && !player.playerNetworkManager.isSprinting.Value)
            {
                // if we are locked on pass the horizontal movement as well
                player.playerAnimatorManager.UpdateAnimatorMovementParameters(horizontal_Input, vertical_Input, player.playerNetworkManager.isSprinting.Value);
                return;
            }

            if (player.playerNetworkManager.isAiming.Value)
            {
                // if we are locked on pass the horizontal movement as well
                player.playerAnimatorManager.UpdateAnimatorMovementParameters(horizontal_Input, vertical_Input, player.playerNetworkManager.isSprinting.Value);
                return;
            }

            // if we are not locked on, only use the move amount
            player.playerAnimatorManager.UpdateAnimatorMovementParameters(0, moveAmount, player.playerNetworkManager.isSprinting.Value);
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

                if (PlayerUIManager.instance.menuWindowIsOpen)
                    return;

                player.playerLocomotionManager.AttemptToPerformJump();
            }
        }

        private void HandleRBInput()
        {
            if (two_Hand_Input)
                return;

            if (RB_Input)
            {
                RB_Input = false;

                player.playerNetworkManager.SetCharacterActionHand(true);

                player.playerCombatManager.PerformWeaponBasedAction(
                    player.playerInventoryManager.currentRightHandWeapon.oh_RB_Action,
                    player.playerInventoryManager.currentRightHandWeapon);
            }
        }

        private void HandleHoldRBInput()
        {
            if (hold_RB_Input)
            {
                player.playerNetworkManager.isChargingRightSpell.Value = true;
                player.playerNetworkManager.isHoldingArrow.Value = true;
            }
            else
            {
                player.playerNetworkManager.isChargingRightSpell.Value = false;
                player.playerNetworkManager.isHoldingArrow.Value = false;
            }
        }

        private void HandleLBInput()
        {
            if (two_Hand_Input)
                return;

            if (LB_Input)
            {
                LB_Input = false;

                player.playerNetworkManager.SetCharacterActionHand(false);

                if (player.playerNetworkManager.isTwoHandingRightWeapon.Value)
                {
                    player.playerCombatManager.PerformWeaponBasedAction(player.playerInventoryManager.currentRightHandWeapon.oh_LB_Action, player.playerInventoryManager.currentRightHandWeapon);
                }
                else
                {
                    player.playerCombatManager.PerformWeaponBasedAction(player.playerInventoryManager.currentLeftHandWeapon.oh_LB_Action, player.playerInventoryManager.currentLeftHandWeapon);
                }
            }
        }

        private void HandleHoldLBInput()
        {
            if (hold_LB_Input)
            {
                player.playerNetworkManager.isChargingLeftSpell.Value = true;
            }
            else
            {
                player.playerNetworkManager.isChargingLeftSpell.Value = false;
            }
        }

        private void HandleRTInput()
        {
            if (RT_Input)
            {
                RT_Input = false;
                player.playerNetworkManager.SetCharacterActionHand(true);
                player.playerCombatManager.PerformWeaponBasedAction(
                    player.playerInventoryManager.currentRightHandWeapon.oh_RT_Action,
                    player.playerInventoryManager.currentRightHandWeapon);
            }
        }

        private void HandleChargeRTInput()
        {
            if (player.isPerformingAction)
            {
                if (player.playerNetworkManager.isUsingRightHand.Value)
                {
                    player.playerNetworkManager.isChargingAttack.Value = Hold_RT_Input;
                }
            }
        }

        private void HandleLTInput()
        {
            if (LT_Input)
            {
                LT_Input = false;

                WeaponItem weaponPerformingAshOfWar = player.playerCombatManager.SelectWeaponToPerformAshOfWar();

                weaponPerformingAshOfWar.ashOfWarAction.AttemptToPerformAction(player);
            }
        }

        private void HandleSwitchRightWeaponInput()
        {
            if (switch_Right_Weapon_Input)
            {
                switch_Right_Weapon_Input = false;

                if (PlayerUIManager.instance.menuWindowIsOpen)
                    return;

                if (player.isPerformingAction)
                    return;

                if (player.playerCombatManager.isUsingItem)
                    return;

                player.playerEquipmentManager.SwitchRightWeapon();
            }
        }

        private void HandleSwitchLeftWeaponInput()
        {
            if (switch_Left_Weapon_Input)
            {
                switch_Left_Weapon_Input = false;

                if (PlayerUIManager.instance.menuWindowIsOpen)
                    return;

                if (player.isPerformingAction)
                    return;

                if (player.playerCombatManager.isUsingItem)
                    return;

                player.playerEquipmentManager.SwitchLeftWeapon();
            }
        }

        private void HandleSwitchQuickSlotItemInput()
        {
            if (switch_Quick_Slot_Item_Input)
            {
                switch_Quick_Slot_Item_Input = false;

                if (PlayerUIManager.instance.menuWindowIsOpen)
                    return;

                if (player.isPerformingAction)
                    return;

                if (player.playerCombatManager.isUsingItem)
                    return;

                player.playerEquipmentManager.SwitchQuickSlotItem();
            }
        }

        private void HandleInteractionInput()
        {
            if (interaction_Input)
            {
                interaction_Input = false;

                player.playerInteractionManager.Interact();
            }
        }

        private void QueInput(ref bool quedInput) //passing a ref we pass a specific bool, and not the value of that bool (true/false)
        {
            que_RB_Input = false;
            que_RT_Input = false;

            //check for ui window being open, if its open return
            if (player.isPerformingAction || player.playerNetworkManager.isJumping.Value)
            {
                quedInput = true;
                //attempt this new input for x amount of time
                que_Input_Timer = default_Que_Input_Time;
                input_Que_Is_Active = true;
            }
        }

        private void ProcessQuedInput()
        {
            if (player.isDead.Value)
                return;

            if (que_RB_Input)
                RB_Input = true;

            if (que_RT_Input)
                RT_Input = true;
        }

        private void HandleQuedInput()
        {
            if (input_Que_Is_Active)
            {
                //while the timer is above 0, keep attempting to press the input
                if (que_Input_Timer > 0)
                {
                    que_Input_Timer -= Time.deltaTime;
                    ProcessQuedInput();
                }
                else
                {
                    //reset the que
                    que_RB_Input = false;
                    que_RT_Input = false;
                    input_Que_Is_Active = false;
                    que_Input_Timer = 0;
                }
            }
        }

        private void HandleOpenCharacterMenuInput()
        {
            if (openCharacterMenuInput)
            {
                openCharacterMenuInput = false;

                PlayerUIManager.instance.playerUIPopUpManager.CloseAllPopUpWindows();
                PlayerUIManager.instance.CloseAllMenuWindows();
                PlayerUIManager.instance.playerUICharacterMenuManager.OpenCharacterMenu();
            }
        }

        private void HandleCloseUIInputs()
        {
            if (closeMenuInput)
            {
                closeMenuInput = false;

                if (PlayerUIManager.instance.menuWindowIsOpen)
                {
                    PlayerUIManager.instance.CloseAllMenuWindows();
                }
            }
        }
    }
}
