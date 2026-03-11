using UnityEngine;

namespace baodeag
{
    public class AICharacterSoundFXManager : CharacterSoundFXManager
    {
        [Header("Blocking SFX")]
        [SerializeField] AudioClip[] blockingSFX;

        [Header("Dialogue")]
        public GameObject interactableDialogueCollider;
        public bool dialogueIsPlaying = false;

        public override void PlayBlockSoundFX()
        {
            if (blockingSFX.Length <= 0)
                return;

            PlaySoundFX(WorldSoundFXManager.instance.ChooseRandomSFXFromArray(blockingSFX));
        }

        //dialogue
        public void PlayCurrentDialogueEvent()
        {

        }

        public void PlayFarewellDialogueEvent()
        {

        }

        //cancel current dialogue event (used when player leaves trigger area, ...)
        public void CancelCurrentDialogueEvent()
        {

        }

        //used for specific calls when a dialogue is over (npc dies, shop opens, ...)
        public void OnCurrentDialogueEnded()
        {

        }
    }
}
