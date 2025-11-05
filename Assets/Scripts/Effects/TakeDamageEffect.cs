using UnityEngine;

namespace baodeag
{
    [CreateAssetMenu(menuName = "Character Effects/Instant Effects/Take Damage")]
    public class TakeDamageEffect : InstantCharacterEffect
    {
        [Header("Character Causing Damage")]
        public CharacterManager characterCausingDamage;

        [Header("Damage")]
        public float physicalDamage = 0;
        public float magicDamage = 0;
        public float fireDamage = 0;
        public float lightningDamage = 0;
        public float holyDamage = 0;

        [Header("Final Damage")]
        private int finalDamageDealt = 0;

        [Header("Poise")]
        public float poiseDamage = 0;
        public bool poiseIsBroken = false;

        [Header("Animation")]
        public bool playDamageAnimation = true;
        public bool manuallySelectDamageAnimation = false;
        public string damageAnimation;

        [Header("Sound FX")]
        public bool willPlayDamageSFX = true;
        public AudioClip elementalDamageSoundFX;

        [Header("Direction Damage Taken From")]
        public float angleHitFrom;
        public Vector3 contactPoint;


        public override void ProcessEffect(CharacterManager character)
        {
            if (character.characterNetworkManager.isInvulnerable.Value)
                return;

            base.ProcessEffect(character);

            //if the character is dead, no additional damage effect should be processed
            if (character.isDead.Value)
                return;

            CalculateDamage(character);
            PLayDirectionalBasedDamageAnimation(character);

            PlayDamageSFX(character);
            PlayDamageVFX(character);
        }

        private void CalculateDamage(CharacterManager character)
        {
            if (!character.IsOwner)
                return;

            if(characterCausingDamage != null)
            {
                
            }
            
            finalDamageDealt = Mathf.RoundToInt(physicalDamage + magicDamage + fireDamage + lightningDamage + holyDamage);

            if(finalDamageDealt <= 0)
            {
                finalDamageDealt = 1;
            }

            character.characterNetworkManager.currentHealth.Value -= finalDamageDealt;

            //subject poise damage from characters total
            character.characterStatsManager.totalPoiseDamage -= poiseDamage;

            float remainingPoise = character.characterStatsManager.basePoiseDefense + 
                character.characterStatsManager.offensivePoiseBonus + 
                character.characterStatsManager.totalPoiseDamage;

            if (remainingPoise <= 0) 
                poiseIsBroken = true;

            //since the  character has been hit, reset the poise timer
            character.characterStatsManager.poiseResetTimer = character.characterStatsManager.defaultPoiseResetTime;
        }

        private void PlayDamageVFX(CharacterManager character)
        {
            character.characterEffectsManager.PlayBloodSplatterVFX(contactPoint);
        }

        private void PlayDamageSFX(CharacterManager character)
        {
            AudioClip physicalDamageSFX = WorldSoundFXManager.instance.ChooseRandomSFXFromArray(WorldSoundFXManager.instance.physicalDamageSFX);

            character.characterSoundFXManager.PlaySoundFX(physicalDamageSFX);
            character.characterSoundFXManager.PlayDamageGruntSoundFX();
        }

        private void PLayDirectionalBasedDamageAnimation(CharacterManager character)
        {
            if (!character.IsOwner)
                return;

            if (character.isDead.Value)
                return;

            if (poiseIsBroken)
            {
                if (angleHitFrom >= 145 && angleHitFrom <= 180)
                {
                    //play front animation
                    damageAnimation = character.characterAnimatorManager.GetRandomAnimationFromList(character.characterAnimatorManager.forward_Medium_Damage);
                }
                else if (angleHitFrom <= -145 && angleHitFrom >= -180)
                {
                    //play front animation
                    damageAnimation = character.characterAnimatorManager.GetRandomAnimationFromList(character.characterAnimatorManager.forward_Medium_Damage);
                }
                else if (angleHitFrom >= -45 && angleHitFrom <= 45)
                {
                    //play back animation
                    damageAnimation = character.characterAnimatorManager.GetRandomAnimationFromList(character.characterAnimatorManager.backward_Medium_Damage);
                }
                else if (angleHitFrom >= -144 && angleHitFrom <= -45)
                {
                    //play left animation
                    damageAnimation = character.characterAnimatorManager.GetRandomAnimationFromList(character.characterAnimatorManager.left_Medium_Damage);
                }
                else if (angleHitFrom >= 45 && angleHitFrom <= 144)
                {
                    //play right animation
                    damageAnimation = character.characterAnimatorManager.GetRandomAnimationFromList(character.characterAnimatorManager.right_Medium_Damage);
                }
            }
            else
            {
                if (angleHitFrom >= 145 && angleHitFrom <= 180)
                {
                    //play front animation
                    damageAnimation = character.characterAnimatorManager.GetRandomAnimationFromList(character.characterAnimatorManager.forward_Ping_Damage);
                }
                else if (angleHitFrom <= -145 && angleHitFrom >= -180)
                {
                    //play front animation
                    damageAnimation = character.characterAnimatorManager.GetRandomAnimationFromList(character.characterAnimatorManager.forward_Ping_Damage);
                }
                else if (angleHitFrom >= -45 && angleHitFrom <= 45)
                {
                    //play back animation
                    damageAnimation = character.characterAnimatorManager.GetRandomAnimationFromList(character.characterAnimatorManager.backward_Ping_Damage);
                }
                else if (angleHitFrom >= -144 && angleHitFrom <= -45)
                {
                    //play left animation
                    damageAnimation = character.characterAnimatorManager.GetRandomAnimationFromList(character.characterAnimatorManager.left_Ping_Damage);
                }
                else if (angleHitFrom >= 45 && angleHitFrom <= 144)
                {
                    //play right animation
                    damageAnimation = character.characterAnimatorManager.GetRandomAnimationFromList(character.characterAnimatorManager.right_Ping_Damage);
                }
            }

            character.characterAnimatorManager.lastDamageAnimationPlayed = damageAnimation;

            if (poiseIsBroken)
            {
                //if we are poise broken, we want to force the damage animation to play
                character.characterAnimatorManager.PlayTargetActionAnimation(damageAnimation, true);
            }
            else
            {
                //play normally
                character.characterAnimatorManager.PlayTargetActionAnimation(damageAnimation, false, false, true, true);
            }
                
        }
    }
}
