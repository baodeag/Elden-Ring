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
        protected int finalDamageDealt = 0;

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
            ApplyAttackerBuildUps(character);
            PLayDirectionalBasedDamageAnimation(character);

            PlayDamageSFX(character);
            PlayDamageVFX(character);

            CalculateStanceDamage(character);
        }

        protected void RegisterDamageDealer(CharacterManager character)
        {
            AICharacterManager aiCharacter = character as AICharacterManager;
            PlayerManager playerCausingDamage = characterCausingDamage as PlayerManager;

            if (aiCharacter != null && playerCausingDamage != null)
                aiCharacter.RegisterLastPlayerWhoDealtDamage(playerCausingDamage);
        }

        private void ApplyAttackerBuildUps(CharacterManager character)
        {
            if (!character.IsOwner)
                return;

            if (character is not PlayerManager player || player.playerEffectsManager == null)
                return;

            if (!ShouldApplyMonster33PowerUpFireBuildUp())
                return;

            player.playerEffectsManager.ApplyFireBuildUpFromHit(PlayerEffectsManager.DefaultFireBuildUpFromHit);
        }

        private bool ShouldApplyMonster33PowerUpFireBuildUp()
        {
            if (fireDamage > 0)
                return true;

            if (characterCausingDamage == null)
                return false;

            AIMonster33BossCharacterNetworkManager monster33NetworkManager = characterCausingDamage.GetComponent<AIMonster33BossCharacterNetworkManager>();
            if (monster33NetworkManager != null)
                return monster33NetworkManager.isPowerUpPhaseActive.Value;

            AIMonster33CombatManager monster33CombatManager = characterCausingDamage.GetComponent<AIMonster33CombatManager>();
            return monster33CombatManager != null && monster33CombatManager.IsPoweredUp;
        }

        protected virtual void CalculateDamage(CharacterManager character)
        {
            if (!character.IsOwner)
                return;

            RegisterDamageDealer(character);
            
            finalDamageDealt = Mathf.RoundToInt(physicalDamage + magicDamage + fireDamage + lightningDamage + holyDamage);

            if(finalDamageDealt <= 0)
            {
                finalDamageDealt = 1;
            }

            character.characterNetworkManager.currentHealth.Value -= finalDamageDealt;

            //subject poise damage from characters total
            character.characterStatsManager.totalPoiseDamage -= poiseDamage;

            //we store the previous poise damage taken for other interactions
            character.characterCombatManager.previousPoiseDamageTaken = poiseDamage;

            float remainingPoise = character.characterStatsManager.basePoiseDefense + 
                character.characterStatsManager.offensivePoiseBonus + 
                character.characterStatsManager.totalPoiseDamage;

            if (remainingPoise <= 0) 
                poiseIsBroken = true;

            //since the  character has been hit, reset the poise timer
            character.characterStatsManager.poiseResetTimer = character.characterStatsManager.defaultPoiseResetTime;
        }

        protected void CalculateStanceDamage(CharacterManager character)
        {
            AICharacterManager aiCharacter = character as AICharacterManager;

            //you can optionally give weapons their own stance damage values, or use poise damage
            int stanceDamage = Mathf.RoundToInt(poiseDamage);

            if (aiCharacter != null)
            {
                aiCharacter.aiCharacterCombatManager.DamageStance(stanceDamage);
            }
        }

        protected void PlayDamageVFX(CharacterManager character)
        {
            character.characterEffectsManager.PlayBloodSplatterVFX(contactPoint);
        }

        protected void PlayDamageSFX(CharacterManager character)
        {
            AudioClip physicalDamageSFX = WorldSoundFXManager.instance.ChooseRandomSFXFromArray(WorldSoundFXManager.instance.physicalDamageSFX);

            character.characterSoundFXManager.PlaySoundFX(physicalDamageSFX);
            character.characterSoundFXManager.PlayDamageGruntSoundFX();
        }

        protected void PLayDirectionalBasedDamageAnimation(CharacterManager character)
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
                character.characterCombatManager.DestroyAllCurrentActionFX();
            }
            else
            {
                //play normally
                character.characterAnimatorManager.PlayTargetActionAnimation(damageAnimation, false, false, true, true);
            }
                
        }
    }
}
