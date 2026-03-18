using UnityEngine;
using System.Collections.Generic;
using System.Collections;

namespace baodeag
{
    public class ElevatorButtonTrigger : MonoBehaviour
    {
        [Header("Characters Standing On Button")]
        private List<CharacterManager> charactersOnElevatorButton = new List<CharacterManager>();

        [Header("Animation")]
        [SerializeField] Animator animator;
        [SerializeField] string pressButtonAnimation;
        [SerializeField] string releaseButtonAnimation;

        [Header("Elevator")]
        [SerializeField] ElevatorInteractable elevator;
        [SerializeField] float minimumButtonReleaseTime = 2f;
        private bool buttonHasBeenPressed = false;
        private Coroutine elevatorButtonCoroutine;

        private void OnTriggerEnter(Collider other)
        {
            CharacterManager character = other.GetComponent<CharacterManager>();

            if (character != null)
                AddCharacterToListOfCharactersOnElevatorButton(character);
        }

        private void OnTriggerExit(Collider other)
        {
            CharacterManager character = other.GetComponent<CharacterManager>();

            if (character != null)
                RemoveCharacterFromListOfCharactersOnElevatorButton(character);
        }

        public void AddCharacterToListOfCharactersOnElevatorButton(CharacterManager character)
        {
            if (charactersOnElevatorButton.Contains(character))
                return;

            charactersOnElevatorButton.Add(character);

            for (int i = 0; i < charactersOnElevatorButton.Count; i++)
            {
                if (charactersOnElevatorButton[i] == null)
                    charactersOnElevatorButton.RemoveAt(i);
            }

            if (charactersOnElevatorButton.Count > 0 && !elevator.elevatorIsRising.Value && !elevator.elevatorIsDescending.Value)
                ActivateElevatorWithButton();
        }

        public void RemoveCharacterFromListOfCharactersOnElevatorButton(CharacterManager character)
        {
            if (!charactersOnElevatorButton.Contains(character))
                return;

            charactersOnElevatorButton.Remove(character);

            for (int i = 0; i < charactersOnElevatorButton.Count; i++)
            {
                if (charactersOnElevatorButton[i] == null)
                    charactersOnElevatorButton.RemoveAt(i);
            }
        }

        private void ActivateElevatorWithButton()
        {
            if (buttonHasBeenPressed)
                return;

            buttonHasBeenPressed = true;
            animator.Play(pressButtonAnimation);

            //start elevator
            elevator.ActivateElevatorServerRpc();

            //wait for button to be released
            if (elevatorButtonCoroutine != null)
                StopCoroutine(elevatorButtonCoroutine);

            elevatorButtonCoroutine = StartCoroutine(WaitForElevatorButtonToRelease());
        }

        private IEnumerator WaitForElevatorButtonToRelease()
        {
            while (elevator.elevatorIsDescending.Value || elevator.elevatorIsRising.Value)
            {
                yield return null;
            }

            yield return new WaitForSeconds(minimumButtonReleaseTime);

            while (charactersOnElevatorButton.Count > 0)
            {
                yield return null;
            }

            buttonHasBeenPressed = false;
            animator.Play(releaseButtonAnimation);
        }
    }
}
