using NUnit.Framework;
using Unity.Netcode;
using UnityEngine;
using System.Collections.Generic;
using System.Collections;

namespace baodeag
{
    public class ElevatorInteractable : Interactable
    {
        [Header("Network Position")]
        public NetworkVariable<Vector3> networkPosition = new NetworkVariable<Vector3>(Vector3.zero, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
        public NetworkVariable<bool> elevatorIsRising = new NetworkVariable<bool>(false, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
        public NetworkVariable<bool> elevatorIsDescending = new NetworkVariable<bool>(false, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
        [SerializeField] float networkPositionSmoothTime = 0.1f;
        [SerializeField] float yMovementOffSet = 0.3f;

        [Header("Destination")]
        [SerializeField] float moveSpeed = 2;
        public Vector3 destinationHigh; //where the elevator stop when it rises
        public Vector3 destinationLow; //where the elevator stop when it descends

        [Header("Recall Locations")]
        public CallElevatorInteractable lowDestinationRecall;
        public CallElevatorInteractable highDestinationRecall;

        [Header("Characters On Elevator")]
        [SerializeField] protected List<CharacterManager> charactersOnElevator = new List<CharacterManager>();

        [Header("SFX")]
        private AudioSource elevatorAudioSource;
        [SerializeField] private AudioClip elevatorMovingSFX;
        [SerializeField] private AudioClip[] elevatorStoppingSFX;

        protected override void Awake()
        {
            base.Awake();

            elevatorAudioSource = GetComponent<AudioSource>();
        }

        public override void OnTriggerEnter(Collider other)
        {
            if (elevatorIsRising.Value || elevatorIsDescending.Value)
                return;

            base.OnTriggerEnter(other);
        }

        public override void Interact(PlayerManager player)
        {
            base.Interact(player);

            if (player.IsOwner)
                ActivateElevatorServerRpc();
        }

        public override void OnNetworkSpawn()
        {
            base.OnNetworkSpawn();

            if (!IsOwner)
            {
                transform.localPosition = networkPosition.Value;
            }
            else
            {
                networkPosition.Value = transform.localPosition;
            }

            if (elevatorIsRising.Value)
                ActivateElevator(true);

            if (elevatorIsDescending.Value)
                ActivateElevator(false);
        }

        public override void OnNetworkDespawn()
        {
            base.OnNetworkDespawn();
        }

        private void ActivateElevator(bool isRising)
        {
            StartCoroutine(MoveElevatorCoroutine(isRising));
        }

        private IEnumerator MoveElevatorCoroutine(bool isRising)
        {
            if (interactableCollider != null)
                interactableCollider.enabled = false;

            //when the elevator starts, remove it as an interactable whilist its going
            for (int i = 0; i < charactersOnElevator.Count; i++)
            {
                if (charactersOnElevator[i] == null)
                    continue;

                PlayerManager player = charactersOnElevator[i] as PlayerManager;

                if (player == null)
                    continue;

                player.playerInteractionManager.RemoveInteractionFromList(this);
            }

            //sfx
            elevatorAudioSource.clip = elevatorMovingSFX;
            elevatorAudioSource.Play();

            //decide the destination
            Vector3 destination = destinationHigh;

            if (!isRising)
                destination = destinationLow;

            lowDestinationRecall.RemoveInteractionFromPlayers();
            highDestinationRecall.RemoveInteractionFromPlayers();

            //move the elevator
            while (transform.localPosition != destination)
            {
                transform.localPosition = Vector3.MoveTowards(transform.localPosition, destination, moveSpeed * Time.deltaTime);
                Vector3 velocityOfMovement = Vector3.MoveTowards(transform.position, destination, moveSpeed * Time.deltaTime);

                if (IsOwner)
                    networkPosition.Value = transform.localPosition;

                for (int i = 0; i < charactersOnElevator.Count; i++)
                {
                    if (charactersOnElevator[i] == null)
                        continue;

                    if (!charactersOnElevator[i].gameObject.activeInHierarchy)
                        RemoveCharacterFromListOfCharactersOnElevator(charactersOnElevator[i]);

                    if (!charactersOnElevator[i].characterNetworkManager.isJumping.Value)
                        charactersOnElevator[i].transform.position = new Vector3(
                            charactersOnElevator[i].transform.position.x,
                            velocityOfMovement.y + yMovementOffSet,
                            charactersOnElevator[i].transform.position.z);
                }

                yield return null;
            }

            //stop the movement flags
            if (IsOwner)
            {
                elevatorIsRising.Value = false;
                elevatorIsDescending.Value = false;
            }

            lowDestinationRecall.ReturnInteractionToPlayers();
            highDestinationRecall.ReturnInteractionToPlayers();

            //stop the movement sfx
            elevatorAudioSource.Stop();
            //play the stopping sfx
            elevatorAudioSource.PlayOneShot(WorldSoundFXManager.instance.ChooseRandomSFXFromArray(elevatorStoppingSFX));

            //re-enable interaction with the elevator
            if (interactableCollider != null)
                interactableCollider.enabled = true;

            yield return null;
        }

        public void AddCharacterToListOfCharactersOnElevator(CharacterManager character)
        {
            if (charactersOnElevator.Contains(character))
                return;

            charactersOnElevator.Add(character);
            character.characterLocomotionManager.isRidingLift = true;
        }

        public void RemoveCharacterFromListOfCharactersOnElevator(CharacterManager character)
        {
            if (!charactersOnElevator.Contains(character))
                return;

            charactersOnElevator.Remove(character);
            character.characterLocomotionManager.isRidingLift = false;
        }

        [ServerRpc(RequireOwnership = false)]

        public void ActivateElevatorServerRpc()
        {
            if (IsServer)
                ActivateElevatorClientRpc();
        }

        [ClientRpc]

        private void ActivateElevatorClientRpc()
        {
            if (transform.localPosition == destinationHigh)
            {
                if (IsOwner)
                    elevatorIsDescending.Value = true;

                ActivateElevator(false);
            }
            else if (transform.localPosition == destinationLow)
            {
                if (IsOwner)
                    elevatorIsRising.Value = true;

                ActivateElevator(true);
            }
        }
    }
}
