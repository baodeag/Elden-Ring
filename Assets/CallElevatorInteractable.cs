using UnityEngine;
using System.Collections.Generic;
using System.Collections;

namespace baodeag
{
    public class CallElevatorInteractable : Interactable
    {
        [Header("Elevator")]
        [SerializeField] ElevatorInteractable elevator;

        [Header("Players Within Interaction Radius")]
        public List<PlayerManager> playersWithinInteractionTrigger = new List<PlayerManager>();

        [Header("Top/Bottom Call")]
        [SerializeField] bool isTopDestination = true;

        private Coroutine waitForElevatorTravelCoroutine;

        public override void OnTriggerEnter(Collider other)
        {
            PlayerManager player = other.GetComponent<PlayerManager>();

            if (player != null)
                AddCharacterToListOfCharactersOnElevator(player);
        }

        public override void OnTriggerExit(Collider other)
        {
            base.OnTriggerExit(other);

            PlayerManager player = other.GetComponent<PlayerManager>();

            if (player != null)
                RemoveCharacterFromListOfCharactersOnElevator(player);
        }

        public override void Interact(PlayerManager player)
        {
            elevator.ActivateElevatorServerRpc();
        }

        public void AddCharacterToListOfCharactersOnElevator(PlayerManager player)
        {
            //check for null incase somebody on the elevator disconnects during interaction redius check
            for (int i = 0; i < playersWithinInteractionTrigger.Count; i++)
            {
                if (playersWithinInteractionTrigger[i] == null)
                    playersWithinInteractionTrigger.RemoveAt(i);
            }

            if (playersWithinInteractionTrigger.Contains(player))
                return;

            playersWithinInteractionTrigger.Add(player);

            if (waitForElevatorTravelCoroutine != null)
                StopCoroutine(waitForElevatorTravelCoroutine);

            waitForElevatorTravelCoroutine = StartCoroutine(CheckForCharactersInTrigger());
        }

        public void RemoveCharacterFromListOfCharactersOnElevator(PlayerManager player)
        {
            if (!playersWithinInteractionTrigger.Contains(player))
                return;

            playersWithinInteractionTrigger.Remove(player);

            for (int i = 0; i < playersWithinInteractionTrigger.Count; i++)
            {
                if (playersWithinInteractionTrigger[i] == null)
                    playersWithinInteractionTrigger.RemoveAt(i);
            }
        }

        private IEnumerator CheckForCharactersInTrigger()
        {
            while (elevator.elevatorIsRising.Value || elevator.elevatorIsDescending.Value)
            {
                yield return null;
            }

            for (int i = 0; i < playersWithinInteractionTrigger.Count; i++)
            {
                if (playersWithinInteractionTrigger[i] == null)
                    continue;

                if (isTopDestination && elevator.networkPosition.Value == elevator.destinationLow)
                    playersWithinInteractionTrigger[i].playerInteractionManager.AddInteractionToList(this);

                if (!isTopDestination && elevator.networkPosition.Value == elevator.destinationHigh)
                    playersWithinInteractionTrigger[i].playerInteractionManager.AddInteractionToList(this);
            }
        }

        public void RemoveInteractionFromPlayers()
        {
            for (int i = 0; i < playersWithinInteractionTrigger.Count; i++)
            {
                if (playersWithinInteractionTrigger[i] == null)
                    continue;

                if (!playersWithinInteractionTrigger[i].IsOwner)
                    continue;

                playersWithinInteractionTrigger[i].playerInteractionManager.RemoveInteractionFromList(this);
            }
        }

        public void ReturnInteractionToPlayers()
        {
            for (int i = 0; i < playersWithinInteractionTrigger.Count; i++)
            {
                if (playersWithinInteractionTrigger[i] == null)
                    continue;

                if (!playersWithinInteractionTrigger[i].IsOwner)
                    continue;


                if (isTopDestination && elevator.networkPosition.Value == elevator.destinationLow)
                    playersWithinInteractionTrigger[i].playerInteractionManager.AddInteractionToList(this);

                if (!isTopDestination && elevator.networkPosition.Value == elevator.destinationHigh)
                    playersWithinInteractionTrigger[i].playerInteractionManager.AddInteractionToList(this);
            }
        }
    }
}
