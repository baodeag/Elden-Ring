using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
using System;

namespace baodeag
{
    public class PlayerInteractionManager : MonoBehaviour
    {
        PlayerManager player;
        private const float MaxInteractableDistance = 3f;

        [HideInInspector] public List<Interactable> currentInteractableActions;

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

            if (player.isDead.Value || PlayerUIManager.instance.playerUILoadingScreenManager.LoadingScreenIsActive())
            {
                ClearInteractionList();
                return;
            }

            //if our ui menu not open, and we dont have a pop up, check for interactables
            if (!PlayerUIManager.instance.menuWindowIsOpen && !PlayerUIManager.instance.popUpWindowIsOpen)
                CheckForInteractable();
        }

        private void CheckForInteractable()
        {
            RefreshInteractableList();

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
                if (!IsValidInteractable(currentInteractableActions[i]))
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
            //if we press the interact button with or without an interactable, it will clear the pop up windows (item, message, etc)
            PlayerUIManager.instance.playerUIPopUpManager.CloseAllPopUpWindows();

            if (player.isDead.Value || PlayerUIManager.instance.playerUILoadingScreenManager.LoadingScreenIsActive())
            {
                ClearInteractionList();
                return;
            }

            RefreshInteractableList();

            if (currentInteractableActions.Count == 0)
                return;

            if (currentInteractableActions[0] != null)
            {
                currentInteractableActions[0].Interact(player);
                RefreshInteractableList();
            }
        }

        public void ClearInteractionList()
        {
            currentInteractableActions.Clear();
            PlayerUIManager.instance.playerUIPopUpManager.CloseAllPopUpWindows();
        }

        private bool IsValidInteractable(Interactable interactableObject)
        {
            if (interactableObject == null)
                return false;

            if (!interactableObject.gameObject.activeInHierarchy)
                return false;

            Collider interactableCollider = interactableObject.GetComponent<Collider>();

            if (interactableCollider == null || !interactableCollider.enabled)
                return false;

            Vector3 closestPoint = interactableCollider.ClosestPoint(player.transform.position);
            float distanceToInteractable = Vector3.Distance(player.transform.position, closestPoint);

            return distanceToInteractable <= MaxInteractableDistance;
        }
    }
}
