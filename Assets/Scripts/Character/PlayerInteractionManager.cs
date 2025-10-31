using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;

namespace baodeag
{
    public class PlayerInteractionManager : MonoBehaviour
    {
        PlayerManager player;

        private List<Interactable> currentInteractableActions;

        private void Awake()
        {
            player = GetComponent<PlayerManager>();
        }

        private void Start()
        {
            currentInteractableActions = new List<Interactable>();
        }

        private void FixedUpdate()
        {
            if (!player.IsOwner)
                return;

            //if our ui menu not open, and we dont have a pop up, check for interactables
            if (!PlayerUIManager.instance.menuWindowIsOpen && !PlayerUIManager.instance.popUpWindowIsOpen)
                CheckForInteractable();
        }

        private void CheckForInteractable()
        {
            if (currentInteractableActions.Count == 0)
                return;

            if (currentInteractableActions[0] == null)
            {
                currentInteractableActions.RemoveAt(0);
                return;
            }

            if (currentInteractableActions[0] != null)
                PlayerUIManager.instance.playerUIPopUpManager.SendPlayerMessagePopUp(currentInteractableActions[0].interactableText);
        }

        private void RefreshInteractableList()
        {
            for (int i = currentInteractableActions.Count - 1; i > -1; i--)
            {
                if (currentInteractableActions[i] == null)
                    currentInteractableActions.RemoveAt(i);
            }
        }

        public void AddInteractionToList(Interactable interactableObject)
        {
            RefreshInteractableList();

            if (!currentInteractableActions.Contains(interactableObject))
                currentInteractableActions.Add(interactableObject);
        }

        public void RemoveInteractionFromList(Interactable interactableObject)
        {
            if (currentInteractableActions.Contains(interactableObject))
                currentInteractableActions.Remove(interactableObject);

            RefreshInteractableList();
        }

        public void Interact()
        {
            if (currentInteractableActions.Count == 0)
                return;

            if (currentInteractableActions[0] != null)
            {
                currentInteractableActions[0].Interact(player);
                RefreshInteractableList();
            }
        }
    }
}
