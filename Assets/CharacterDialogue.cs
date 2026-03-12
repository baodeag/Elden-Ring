using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace baodeag
{
    [CreateAssetMenu(menuName = "A.I/Dialogue")]
    public class CharacterDialogue : ScriptableObject
    {
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

            while (dialogueIndex < dialogueAudio.Count)
            {
                PlayerUIManager.instance.playerUIPopUpManager.SetDialoguePopUpSubtitles(dialogueString[dialogueIndex]);
                aiCharacter.aiCharacterSoundFXManager.PlaySoundFX(dialogueAudio[dialogueIndex]);
                dialogueIndex++;
                yield return new WaitForSeconds(dialogueAudio[dialogueIndex].length + 1);
            }

            OnDialogueEnded(aiCharacter);
            PlayerUIManager.instance.playerUIPopUpManager.EndDialoguePopUp();

            yield return null;
        }

        public void OnDialogueEnded(AICharacterManager aiCharacter)
        { 
            greetingHasPlayed = false;
            dialogueIndex = 0;

            //if (setStageIndex)

            aiCharacter.aiCharacterSoundFXManager.OnCurrentDialogueEnded();
        }

        public void OnDialogueCancelled(AICharacterManager aiCharacter)
        {

        }
    }
}
