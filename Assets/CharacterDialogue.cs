using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace baodeag
{
    [CreateAssetMenu(menuName = "A.I/Dialogue")]
    public class CharacterDialogue : ScriptableObject
    {
        [Header("Dialogue Requirements")]
        public int requiredStageID = 0;

        [Header("Greeting Dialogue")]
        [TextArea] public List<string> greetingDialogueString = new List<string>();
        public List<AudioClip> greetingDialogueAudio = new List<AudioClip>();
        private bool greetingHasPlayed = false;

        [Header("Core Dialogue")]
        [TextArea] public List<string> dialogueString = new List<string>();
        public List<AudioClip> dialogueAudio = new List<AudioClip>();
        public int dialogueIndex = 0;

        [Header("End Triggers")]
        [SerializeField] bool setStageIndex = false;
        [SerializeField] int stageID = 0;

        [Header("Farewell Dialogue")]
        [TextArea] public List<string> farewellDialogueString = new List<string>();
        public List<AudioClip> farewellDialogueAudio = new List<AudioClip>();
        private bool farewellHasPlayed = false;

        public void PlayDialogueEvent(AICharacterManager aiCharacter)
        {
            if (dialogueString.Count != dialogueAudio.Count)
            {
                Debug.LogError("Audio Clip Count Does Not Match Subtitle Count, Missing Files");
                return;
            }

            aiCharacter.aiCharacterSoundFXManager.dialogueIsPlaying = true;
            PlayerUIManager.instance.playerUIPopUpManager.SendDialoguePopUp(this, aiCharacter);
        }

        public IEnumerator PlayDialogueCoroutine(AICharacterManager aiCharacter)
        {
            //play a random greeting dialogue, then wait the length of that audio clip + a second
            if (greetingDialogueAudio.Count != 0 && !greetingHasPlayed)
            {
                greetingHasPlayed = true;
                int randomGreetingDialogueIndex = Random.Range(0, greetingDialogueAudio.Count);
                PlayerUIManager.instance.playerUIPopUpManager.SetDialoguePopUpSubtitles(greetingDialogueString[randomGreetingDialogueIndex]);
                aiCharacter.aiCharacterSoundFXManager.PlaySoundFX(greetingDialogueAudio[randomGreetingDialogueIndex]);
                yield return new WaitForSeconds(greetingDialogueAudio[randomGreetingDialogueIndex].length);
            } 

            while (dialogueIndex < dialogueString.Count)
            {
                PlayerUIManager.instance.playerUIPopUpManager.SetDialoguePopUpSubtitles(dialogueString[dialogueIndex]);
                aiCharacter.aiCharacterSoundFXManager.PlaySoundFX(dialogueAudio[dialogueIndex]);
                yield return new WaitForSeconds(dialogueAudio[dialogueIndex].length + 1);
                dialogueIndex++;
            }

            //play a random farewell dialogue, then wait the length of that audio clip + a second
            if (farewellDialogueAudio.Count != 0 && !greetingHasPlayed)
            {
                farewellHasPlayed = true;
                int randomFarewellDialogueIndex = Random.Range(0, farewellDialogueAudio.Count);
                PlayerUIManager.instance.playerUIPopUpManager.SetDialoguePopUpSubtitles(farewellDialogueString[randomFarewellDialogueIndex]);
                aiCharacter.aiCharacterSoundFXManager.PlaySoundFX(farewellDialogueAudio[randomFarewellDialogueIndex]);
                yield return new WaitForSeconds(farewellDialogueAudio[randomFarewellDialogueIndex].length + 1);
            }

            OnDialogueEnded(aiCharacter);
            PlayerUIManager.instance.playerUIPopUpManager.EndDialoguePopUp();

            yield return null;
        }

        public void OnDialogueEnded(AICharacterManager aiCharacter)
        { 
            greetingHasPlayed = false;
            dialogueIndex = 0;

            if (setStageIndex)
                WorldSaveGameManager.instance.SetStageOfDialogue(aiCharacter.aiCharacterSoundFXManager.characterDialogueID, stageID);

            aiCharacter.aiCharacterSoundFXManager.OnCurrentDialogueEnded();
        }

        public void OnDialogueCancelled(AICharacterManager aiCharacter)
        {

        }
    }
}
