using UnityEngine;

namespace baodeag
{
    public class IsOnElevatorTrigger : MonoBehaviour
    {
        [SerializeField] ElevatorInteractable elevator;

        private void OnTriggerEnter(Collider other)
        {
            CharacterManager character = other.GetComponent<CharacterManager>();

            if (character != null ) 
                elevator.AddCharacterToListOfCharactersOnElevator(character);
        }

        private void OnTriggerExit(Collider other)
        {
            CharacterManager character = other.GetComponent<CharacterManager>();

            if (character != null)
                elevator.RemoveCharacterFromListOfCharactersOnElevator(character);
        }
    }
}
