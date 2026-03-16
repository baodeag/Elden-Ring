using UnityEngine;

namespace baodeag
{
    [CreateAssetMenu(menuName = "Character Effects/Timed Effects/Frostbite Effect")]
    public class FrostBiteEffect : TimedCharacterEffect
    {
        [Header("HP Percentage Damage")]
        [SerializeField] float percentageOfLifeLost = 10;

        [Header("Effect Processed")]
        private bool effectHasBeenInitialized = false;

        public override void ProcessEffect(CharacterManager character)
        {
            base.ProcessEffect(character);

            if (!effectHasBeenInitialized)
            {
                effectHasBeenInitialized = true;
                InflictStaminaRegenerationDebuff(character);
            }
        }

        public override void RemoveEffect(CharacterManager character)
        {
            base.RemoveEffect(character);

            if (character.IsOwner)
                character.characterNetworkManager.isFrozen.Value = false;
        }

        private void InflictStaminaRegenerationDebuff(CharacterManager character)
        {
            ModifyStaminaRegenerationForATimeEffect staminaDebuff = Instantiate(WorldCharacterEffectsManager.instance.frostBiteStaminaRegenerationEffect);
            character.characterEffectsManager.AddTimedEffect(staminaDebuff);

            if (!character.IsOwner)
                return;

            float damage = character.characterNetworkManager.maxHealth.Value * (percentageOfLifeLost / 100);

            if (damage < 0)
                damage = 1;

            character.characterNetworkManager.currentStamina.Value = 0;
            character.characterEffectsManager.ProcessEffectDamage(Mathf.RoundToInt(damage));
            character.characterNetworkManager.isFrozen.Value = true;
        }
    }
}
