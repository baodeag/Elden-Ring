using Unity.Netcode;
using UnityEngine;

namespace baodeag
{
    public class AICharacterSoundFXManager : CharacterSoundFXManager
    {
        AICharacterManager aiCharacter;

        [Header("Blocking SFX")]
        [SerializeField] AudioClip[] blockingSFX;

        [Header("Dialogue")]
        public CharacterDialogueID characterDialogueID;
        public GameObject interactableDialogueCollider;
        public CharacterDialogue currentDialogue;
        public GameObject interactableDialogueObject;
        public bool dialogueIsPlaying = false;

        protected override void Awake()
        {
            base.Awake();

            aiCharacter = GetComponent<AICharacterManager>();
        }

        protected override void Start()
        {
            base.Start();

            if (characterDialogueID != CharacterDialogueID.NoDialogueID)
            {
                currentDialogue = WorldSaveGameManager.instance.GetCharacterDialogueByEnum(characterDialogueID);

                interactableDialogueObject = Instantiate(WorldAIManager.instance.dialogueInteractable, transform);
                NetworkObject networkObject = interactableDialogueObject.GetComponent<NetworkObject>();
                networkObject.Spawn();
                networkObject.TrySetParent(gameObject, true);
            }
        }

        public override void PlayBlockSoundFX()
        {
            if (blockingSFX.Length <= 0)
                return;

            PlaySoundFX(WorldSoundFXManager.instance.ChooseRandomSFXFromArray(blockingSFX));
        }

        //dialogue
        public void PlayCurrentDialogueEvent()
        {
            if (currentDialogue == null)
                return;

            if (!dialogueIsPlaying)
            {
                currentDialogue.PlayDialogueEvent(aiCharacter);
            }
            else
            {
                PlayerUIManager.instance.playerUIPopUpManager.SendNextDialoguePopUpInIndex(currentDialogue, aiCharacter);
            }
        }

        //cancel current dialogue event (used when player leaves trigger area, ...)
        public void CancelCurrentDialogueEvent()
        {
            if (dialogueIsPlaying)
            {
                dialogueIsPlaying = false;
                PlayerUIManager.instance.playerUIPopUpManager.CancelDialoguePopUp(aiCharacter);
            }
        }

        //used for specific calls when a dialogue is over (npc dies, shop opens, ...)
        public void OnCurrentDialogueEnded()
        {
            //get new dialogue based on stage ID
            currentDialogue = WorldSaveGameManager.instance.GetCharacterDialogueByEnum(characterDialogueID);
        }
    }
}
