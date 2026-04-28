using UnityEngine;

namespace baodeag
{
    public class AITormentedSoulCombatManager : AICharacterCombatManager
    {
        [Header("Damage Colliders")]
        [SerializeField] ManualDamageCollider scytheDamageCollider;

        [Header("Damage Modifiers")]
        [SerializeField] float attack01DamageModifier = 1f;
        [SerializeField] float attack02DamageModifier = 1.4f;
        [SerializeField] float poweredUpDamageMultiplier = 1.2f;

        DeathMoonSlash deathMoonSlash;
        bool isPoweredUp;

        public ManualDamageCollider ScytheDamageCollider => scytheDamageCollider;
        public bool IsPoweredUp => isPoweredUp;

        protected override void Awake()
        {
            base.Awake();
            deathMoonSlash = GetComponent<DeathMoonSlash>();
        }

        public void SetAttack01Damage()
        {
            float damageMultiplier = GetCurrentDamageMultiplier() * attack01DamageModifier;
            scytheDamageCollider.physicalDamage = baseDamage * damageMultiplier;
            scytheDamageCollider.poiseDamage = basePoiseDamage * damageMultiplier;
        }

        public void SetAttack02Damage()
        {
            float damageMultiplier = GetCurrentDamageMultiplier() * attack02DamageModifier;
            scytheDamageCollider.physicalDamage = baseDamage * damageMultiplier;
            scytheDamageCollider.poiseDamage = basePoiseDamage * damageMultiplier;
        }

        public void OpenScytheDamageCollider()
        {
            aiCharacter.characterSoundFXManager.PlayAttackGruntSoundFX();
            scytheDamageCollider.EnableDamageCollider();
        }

        public void CloseScytheDamageCollider()
        {
            scytheDamageCollider.DisableDamageCollider();
        }

        public void OpenDamageCollider()
        {
            SetAttack01Damage();
            OpenScytheDamageCollider();
            deathMoonSlash?.FireAttackSlash();
        }

        public void CloseDamageCollider()
        {
            CloseScytheDamageCollider();
        }

        public void DrainStaminaBasedOnAttack()
        {
        }

        public void ActivatePowerUp(float damageMultiplierOverride = -1f)
        {
            isPoweredUp = true;

            if (damageMultiplierOverride > 0f)
                poweredUpDamageMultiplier = damageMultiplierOverride;
        }

        public override bool TryStartSpecialSkill()
        {
            return deathMoonSlash != null && deathMoonSlash.TryActivateSkill();
        }

        float GetCurrentDamageMultiplier()
        {
            return isPoweredUp ? poweredUpDamageMultiplier : 1f;
        }

        public override void CloseAllDamageColliders()
        {
            base.CloseAllDamageColliders();

            if (scytheDamageCollider != null)
                scytheDamageCollider.DisableDamageCollider();
        }
    }
}
