using UnityEngine;

namespace baodeag
{
    public class ResetIsChugging : StateMachineBehaviour
    {
        PlayerManager player;

        // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
        override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            if (player == null) 
                player = animator.GetComponent<PlayerManager>();

            if (player == null)
                return;

            //if we are out of flasks play the empty animation & hide weapons (owner only)
            if (player.playerNetworkManager.isChugging.Value && player.IsOwner)
            {
                FlaskItem currentFlask = player.playerInventoryManager.currentQuickSlotItem as FlaskItem;

                if (currentFlask.healthFlask)
                {
                    if (player.playerNetworkManager.remainingHealthFlasks.Value <= 0)
                    {
                        player.playerAnimatorManager.PlayTargetActionAnimation(currentFlask.emptyFlaskAnimation, false, false, true, true, false);
                        player.playerNetworkManager.HideWeaponsServerRPC();
                    }
                }
                else
                {
                    if (player.playerNetworkManager.remainingFocusPointsFlasks.Value <= 0)
                    {
                        player.playerAnimatorManager.PlayTargetActionAnimation(currentFlask.emptyFlaskAnimation, false, false, true, true, false);
                        player.playerNetworkManager.HideWeaponsServerRPC();
                    }
                }
            }

            //if we are out of flasks, instantiate the empty flask
            if (player.playerNetworkManager.isChugging.Value)
            {
                FlaskItem currentFlask = player.playerInventoryManager.currentQuickSlotItem as FlaskItem;

                if (currentFlask.healthFlask)
                {
                    if (player.playerNetworkManager.remainingHealthFlasks.Value <= 0)
                    {
                        Destroy(player.playerEffectsManager.activeQuickSlotItemFX);
                        GameObject emptyFlask = Instantiate(currentFlask.emptyFlaskItem, player.playerEquipmentManager.rightHandWeaponSlot.transform);
                        player.playerEffectsManager.activeQuickSlotItemFX = emptyFlask;
                    }
                }
                else
                {
                    if (player.playerNetworkManager.remainingFocusPointsFlasks.Value <= 0)
                    {
                        Destroy(player.playerEffectsManager.activeQuickSlotItemFX);
                        GameObject emptyFlask = Instantiate(currentFlask.emptyFlaskItem, player.playerEquipmentManager.rightHandWeaponSlot.transform);
                        player.playerEffectsManager.activeQuickSlotItemFX = emptyFlask;
                    }
                }
            }

            //reset is chugging
            if (player.IsOwner)
                player.playerNetworkManager.isChugging.Value = false;
        }

        // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
        //override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        //{
        //    
        //}

        // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
        //override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        //{
        //    
        //}

        // OnStateMove is called right after Animator.OnAnimatorMove()
        //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        //{
        //    // Implement code that processes and affects root motion
        //}

        // OnStateIK is called right after Animator.OnAnimatorIK()
        //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        //{
        //    // Implement code that sets up animation IK (inverse kinematics)
        //}
    }
}
