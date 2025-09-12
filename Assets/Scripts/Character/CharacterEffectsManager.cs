using UnityEngine;

namespace baodeag
{
    public class CharacterEffectsManager : MonoBehaviour
    {
        //process instant effect (take dmg, heal)

        //process timed effects (poison, burn, buff)

        //process static effects (add/remove buffs)

        CharacterManager character;

        protected virtual void Awake()
        {
            character = GetComponent<CharacterManager>();
        }

        public void ProcessInstantEffect(InstantCharacterEffect effect)
        {
            effect.ProcessEffect(character);
        }
    }
}
