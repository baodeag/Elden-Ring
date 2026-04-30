using UnityEngine;

namespace baodeag
{
    [CreateAssetMenu(menuName = "Character Effects/Instant Effects/Blood Loss")]
    public class BloodLossEffect : InstantCharacterEffect
    {
        [Header("Percentage of Max HP Damage")]
        [SerializeField] float percentageOfMaxHealthAsDamageDealt = 15;
        private float damage = 0;
        public override void ProcessEffect(CharacterManager character)
        {
            base.ProcessEffect(character);

            CalculateDamage(character);
            CheckForDeath(character);
        }

        private void CalculateDamage(CharacterManager character)
        {
            damage = character.characterNetworkManager.maxHealth.Value * (percentageOfMaxHealthAsDamageDealt / 100);
            damage += 100;

            if (damage < 0)
                damage = 1;

            character.characterNetworkManager.currentHealth.Value -= Mathf.RoundToInt(damage);
        }

        private void CheckForDeath(CharacterManager character)
        {
            if (!character.IsOwner)
                return;

            if (!character.isDead.Value)
                return;
            
            if (character.characterNetworkManager.isBeingCriticallyDamaged.Value)
                return;

            character.characterAnimatorManager.PlayTargetActionAnimation("Dead_01", true);
        }
    }
}
