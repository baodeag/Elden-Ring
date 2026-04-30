using UnityEngine;

namespace baodeag
{
    [CreateAssetMenu(menuName = "Character Effects/Timed Effects/Poison Effect")]
    public class PoisonedEffect : TimedCharacterEffect
    {
        private int poisonDamage = 1;
        private bool poisonDamageHasBeenCalculated = false;

        public override void ProcessEffect(CharacterManager character)
        {
            timeRemainingOnEffect -= 1;

            if (timeRemainingOnEffect <= 0 || character.isDead.Value)
                character.characterEffectsManager.RemoveTimedEffect(effectID);

            if (!poisonDamageHasBeenCalculated)
            {
                poisonDamageHasBeenCalculated = true;
                CalculatePoisonDamage(character);
            }

            if (!character.characterNetworkManager.isPoisoned.Value)
                character.characterEffectsManager.RemoveTimedEffect(effectID);

            ProcessPoisonDamage(character);
        }

        private void CalculatePoisonDamage(CharacterManager character)
        {
            poisonDamage = 10;
        }

        private void ProcessPoisonDamage(CharacterManager character)
        {
            character.characterEffectsManager.ProcessEffectDamage(poisonDamage);
        }

        public override void RemoveEffect(CharacterManager character)
        {
            base.RemoveEffect(character);

            if (character.IsOwner)
                character.characterNetworkManager.isPoisoned.Value = false;
        }
    }
}
