using UnityEngine;

namespace baodeag
{
    public class AIKnightCombatManager : AICharacterCombatManager
    {
        [Header("Damage Colliders")]
        [SerializeField] ManualDamageCollider swordDamageCollider;

        [Header("Damage Modifiers")]
        [SerializeField] float attack01DamageModifier = 1.0f;
        [SerializeField] float attack02DamageModifier = 1.4f;
        [SerializeField] float poweredUpDamageMultiplier = 1.25f;

        [Header("Power Up Frost")]
        [SerializeField] int poweredUpFrostBuildUpAmount = 35;

        TwinMoonSkill twinMoonSkill;
        bool isPoweredUp;

        public ManualDamageCollider SwordDamageCollider => swordDamageCollider;
        public bool IsPoweredUp => isPoweredUp;
        public int PoweredUpFrostBuildUpAmount => poweredUpFrostBuildUpAmount;

        protected override void Awake()
        {
            base.Awake();
            twinMoonSkill = GetComponent<TwinMoonSkill>();
        }

        public void SetAttack01Damage()
        {
            float damageMultiplier = GetCurrentDamageMultiplier() * attack01DamageModifier;
            swordDamageCollider.physicalDamage = baseDamage * damageMultiplier;
            swordDamageCollider.poiseDamage = basePoiseDamage * damageMultiplier;
        }

        public void SetAttack02Damage()
        {
            float damageMultiplier = GetCurrentDamageMultiplier() * attack02DamageModifier;
            swordDamageCollider.physicalDamage = baseDamage * damageMultiplier;
            swordDamageCollider.poiseDamage = basePoiseDamage * damageMultiplier;
        }

        public void OpenSwordDamageCollider()
        {
            aiCharacter.characterSoundFXManager.PlayAttackGruntSoundFX();
            swordDamageCollider.EnableDamageCollider();
        }

        public void CloseSwordDamageCollider()
        {
            swordDamageCollider.DisableDamageCollider();
        }

        public void DrainStaminaBasedOnAttack()
        {
        }

        public void OpenDamageCollider()
        {
            SetAttack01Damage();
            OpenSwordDamageCollider();
        }

        public void CloseDamageCollider()
        {
            CloseSwordDamageCollider();
        }

        public void ApplyPowerUpBuff(float damageMultiplierOverride = -1f)
        {
            isPoweredUp = true;

            if (damageMultiplierOverride > 0f)
                poweredUpDamageMultiplier = damageMultiplierOverride;
        }

        public override bool TryStartSpecialSkill()
        {
            return twinMoonSkill != null && twinMoonSkill.TryActivateSkill();
        }

        float GetCurrentDamageMultiplier()
        {
            return isPoweredUp ? poweredUpDamageMultiplier : 1f;
        }

        public override void CloseAllDamageColliders()
        {
            base.CloseAllDamageColliders();

            swordDamageCollider.DisableDamageCollider();
        }
    }
}
