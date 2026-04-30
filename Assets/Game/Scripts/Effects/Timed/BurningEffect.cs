using UnityEngine;

namespace baodeag
{
    [CreateAssetMenu(menuName = "Character Effects/Timed Effects/Burning Effect")]
    public class BurningEffect : TimedCharacterEffect
    {
        [Header("Damage")]
        [SerializeField] private int burnDamage = 10;

        public override void ProcessEffect(CharacterManager character)
        {
            timeRemainingOnEffect -= 1;

            if (timeRemainingOnEffect <= 0 || character.isDead.Value)
            {
                character.characterEffectsManager.RemoveTimedEffect(effectID);
                return;
            }

            if (!character.characterNetworkManager.isBurning.Value)
            {
                character.characterEffectsManager.RemoveTimedEffect(effectID);
                return;
            }

            if (!character.IsOwner)
                return;

            character.characterEffectsManager.ProcessEffectDamage(burnDamage);
        }

        public override void RemoveEffect(CharacterManager character)
        {
            base.RemoveEffect(character);

            if (character.IsOwner)
                character.characterNetworkManager.isBurning.Value = false;
        }
    }
}
