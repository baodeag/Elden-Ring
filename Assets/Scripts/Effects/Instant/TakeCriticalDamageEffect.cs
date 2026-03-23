using UnityEngine;

namespace baodeag
{
    [CreateAssetMenu(menuName = "Character Effects/Instant Effects/Take Critical Damage Effect")]
    public class TakeCriticalDamageEffect : TakeDamageEffect
    {
        public override void ProcessEffect(CharacterManager character)
        {
            if (character.characterNetworkManager.isInvulnerable.Value)
                return;

            //if the character is dead, no additional damage effect should be processed
            if (character.isDead.Value)
                return;

            CalculateDamage(character);

            character.characterCombatManager.pendingCriticalDamage = finalDamageDealt;
        }

        protected override void CalculateDamage(CharacterManager character)
        {
            if (!character.IsOwner)
                return;

            RegisterDamageDealer(character);

            finalDamageDealt = Mathf.RoundToInt(physicalDamage + magicDamage + fireDamage + lightningDamage + holyDamage);

            if (finalDamageDealt <= 0)
            {
                finalDamageDealt = 1;
            }

            //subject poise damage from characters total
            character.characterStatsManager.totalPoiseDamage -= poiseDamage;

            //we store the previous poise damage taken for other interactions
            character.characterCombatManager.previousPoiseDamageTaken -= poiseDamage;

            float remainingPoise = character.characterStatsManager.basePoiseDefense +
                character.characterStatsManager.offensivePoiseBonus +
                character.characterStatsManager.totalPoiseDamage;

            if (remainingPoise <= 0)
                poiseIsBroken = true;

            //since the  character has been hit, reset the poise timer
            character.characterStatsManager.poiseResetTimer = character.characterStatsManager.defaultPoiseResetTime;
        }
    }
}
