using UnityEngine;

namespace baodeag
{
    public class AICharacterSoundFXManager : CharacterSoundFXManager
    {
        AICharacterManager aiCharacter;

        [Header("Blocking SFX")]
        [SerializeField] AudioClip[] blockingSFX;

        [Header("Dialogue")]
        public GameObject interactableDialogueCollider;
        public CharacterDialogue currentDialogue;
        public CharacterDialogue farewellDialogue;
        public bool dialogueIsPlaying = false;

        protected override void Awake()
        {
            base.Awake();

            aiCharacter = GetComponent<AICharacterManager>();
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
                PlayerUIManager.instance.playerUIPopUpManager.SendNextDialoguePopUpIndex(currentDialogue, aiCharacter);
            }
        }

        public void PlayFarewellDialogueEvent()
        {
            if (farewellDialogue == null)
                return;

            if (!dialogueIsPlaying)
            {
                farewellDialogue.PlayDialogueEvent(aiCharacter);
            }
            else
            {
                PlayerUIManager.instance.playerUIPopUpManager.SendNextDialoguePopUpIndex(farewellDialogue, aiCharacter);
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
        }
    }
}
