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

        }

        private IEnumerator PlayDialogueCoroutine(AICharacterManager aiCharacter)
        {
            yield return null;
        }

        public void OnDialogueEnded(AICharacterManager aiCharacter)
        {
            
        }

        public void OnDialogueCancelled(AICharacterManager aiCharacter)
        {

        }
    }
}
