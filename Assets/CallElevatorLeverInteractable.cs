using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using Unity.Netcode;

namespace baodeag
{
    public class CallElevatorLeverInteractable : CallElevatorInteractable
    {
        [Header("Animator")]
        [SerializeField] Animator animator;
        [SerializeField] string pullLeverAnimation;
        [SerializeField] string releaseLeverAnimation;

        [Header("Elevator")]
        [SerializeField] float minimumButtonReleaseTime = 2f;
        public NetworkVariable<bool> leverHasBeenPulled = new NetworkVariable<bool>(false, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
        private Coroutine elevatorLeverCoroutine;
        [SerializeField] float timeToWaitAfterPullingLeverToMoveElevator = 1f;

        public override void Interact(PlayerManager player)
        {
            ActivateElevatorWithLever();
        }

        private void ActivateElevatorWithLever()
        {
            //if the low destination lever has been pulled, dont allow this one to be pulled
            if (elevator.lowDestinationRecall is CallElevatorLeverInteractable)
            {
                CallElevatorLeverInteractable lever = elevator.lowDestinationRecall as CallElevatorLeverInteractable;

                if (lever.leverHasBeenPulled.Value)
                    return;
            }

            //if the high destination lever has been pulled, dont allow this one to be pulled
            if (elevator.highDestinationRecall is CallElevatorLeverInteractable)
            {
                CallElevatorLeverInteractable lever = elevator.highDestinationRecall as CallElevatorLeverInteractable;

                if (lever.leverHasBeenPulled.Value)
                    return;
            }

            //if the elevator is already moving, dont allow the lever to be pulled
            if (elevator.elevatorIsDescending.Value || elevator.elevatorIsRising.Value)
                return;

            PullLeverServerRpc();
        }

        [ServerRpc(RequireOwnership = false)]
        private void PullLeverServerRpc()
        {
            if (IsServer)
                PullLeverClientRpc();
        }

        [ClientRpc]
        private void PullLeverClientRpc()
        {
            StartCoroutine(WaitForLeverAnimationThenMoveElevator());
        }

        private IEnumerator WaitForLeverAnimationThenMoveElevator()
        {
            if (IsOwner)
                leverHasBeenPulled.Value = true;

            RemoveInteractionFromPlayers();
            animator.Play(pullLeverAnimation);

            yield return new WaitForSeconds(timeToWaitAfterPullingLeverToMoveElevator);

            if (IsOwner)
                elevator.ActivateElevatorServerRpc();

            //wait for button to be released
            if (elevatorLeverCoroutine != null)
                StopCoroutine(elevatorLeverCoroutine);

            elevatorLeverCoroutine = StartCoroutine(WaitForElevatorLeverToRelease());
        }

        private IEnumerator WaitForElevatorLeverToRelease()
        {
            while (elevator.elevatorIsDescending.Value || elevator.elevatorIsRising.Value)
            {
                yield return null;
            }

            yield return new WaitForSeconds(minimumButtonReleaseTime);

            if (IsOwner)
                leverHasBeenPulled.Value = false;

            animator.Play(releaseLeverAnimation);
        }
    }
}
