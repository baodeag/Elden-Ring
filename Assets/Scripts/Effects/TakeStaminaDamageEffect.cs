using UnityEngine;

namespace baodeag
{
    [CreateAssetMenu(menuName = "Character Effects/Instant Effects/Take Stamina Damage")]
    public class TakeStaminaDamageEffect : InstantCharacterEffect
    {
        public float staminaDamage;

        public override void ProcessEffect(CharacterManager character)
        {
            CalculateStaminaDamage(character);
        }

        private void CalculateStaminaDamage(CharacterManager character)
        {
            if (character.IsOwner)
            {
                Debug.Log("Character is taking: " + staminaDamage + " stamina damage");
                character.characterNetworkManager.currentStamina.Value -= staminaDamage;
            }
        }
    }
}
