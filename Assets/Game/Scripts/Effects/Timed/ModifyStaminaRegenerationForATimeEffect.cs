using UnityEngine;

namespace baodeag
{
    [CreateAssetMenu(menuName = "Character Effects/Timed Effects/Modify Stat/Stamina Regeneration")]
    public class ModifyStaminaRegenerationForATimeEffect : TimedCharacterEffect
    {
        [Header("Regeneration")]
        [SerializeField] float staminaRegenerationPercentageModifier = 15;

        [Header("Effect Processed")]
        private bool effectHasBeenInitialized = false;

        public override void ProcessEffect(CharacterManager character)
        {
            base.ProcessEffect(character);

            if (!effectHasBeenInitialized)
            {
                if (!character.IsOwner)
                    return;

                effectHasBeenInitialized = true;
                character.characterNetworkManager.staminaRegenerationModifier.Value += staminaRegenerationPercentageModifier;
            }
        }

        public override void RemoveEffect(CharacterManager character)
        {
            base.RemoveEffect(character);

            if (effectHasBeenInitialized)
            {
                if (!character.IsOwner)
                    return;

                character.characterNetworkManager.staminaRegenerationModifier.Value -= staminaRegenerationPercentageModifier;
            }
        }
    }
}
