using UnityEngine;

namespace baodeag
{
    public class InstantCharacterEffect : ScriptableObject
    {
        [Header("Effect ID")]
        public int instantEffectID;

        public virtual void ProcessEffect(CharacterManager character)
        {
            Debug.Log("Processing instant effect on character: " + character.name);
        }
    }
}
