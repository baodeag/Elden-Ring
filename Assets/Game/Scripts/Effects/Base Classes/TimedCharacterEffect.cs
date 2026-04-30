using UnityEngine;

namespace baodeag
{
    public class TimedCharacterEffect : ScriptableObject
    {
        [Header("Effect ID")]
        public int effectID;

        [Header("Time")]
        public float defaultLengthOfEffect;
        public float timeRemainingOnEffect;

        public virtual void ProcessEffect(CharacterManager character)
        {
            timeRemainingOnEffect -= 1;

            if (timeRemainingOnEffect <= 0) 
                character.characterEffectsManager.RemoveTimedEffect(effectID);
        }

        public virtual void RemoveEffect(CharacterManager character)
        {

        }
    }
}
