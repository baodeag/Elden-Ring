using UnityEngine;
using Unity.Netcode;

namespace baodeag
{
    public class DialogueInteractable : Interactable
    {
        AICharacterManager aiCharacter;

        protected override void Awake()
        {
            base.Awake();

            aiCharacter = GetComponentInParent<AICharacterManager>();
        }

        public override void Interact(PlayerManager player)
        {
            if (PlayerUIManager.instance.menuWindowIsOpen)
                return;

            if (aiCharacter.isDead.Value)
            {
                interactableCollider.enabled = false;
                return;
            }

            if (NetworkManager.Singleton.IsServer)
                WorldSaveGameManager.instance.SaveGame();

            aiCharacter.aiCharacterSoundFXManager.PlayCurrentDialogueEvent();
        }

        public override void OnTriggerEnter(Collider other)
        {
            if (aiCharacter.isDead.Value)
            {
                interactableCollider.enabled = false;

                //if there is an active dialogue with this character and the player end it
                PlayerManager player = other.GetComponent<PlayerManager>();

                if (player != null && player.IsOwner)
                    aiCharacter.aiCharacterSoundFXManager.CancelCurrentDialogueEvent();
            }

            base.OnTriggerEnter(other);
        }

        public override void OnTriggerExit(Collider other)
        {
            base.OnTriggerExit(other);

            PlayerManager player = other.GetComponent<PlayerManager>();

            if (player == null)
                return;

            if (!player.IsOwner)
                return;

            //cancel current dialogue with this character when the player leaves interaction radius
            aiCharacter.aiCharacterSoundFXManager.CancelCurrentDialogueEvent();
        }
    }
}
