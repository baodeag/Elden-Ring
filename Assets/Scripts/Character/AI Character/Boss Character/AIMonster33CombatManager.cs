using UnityEngine;

namespace baodeag
{
    public class AIMonster33CombatManager : AICharacterCombatManager
    {
        [Header("Damage Colliders")]
        [SerializeField] ManualDamageCollider rightHandDamageCollider;
        [SerializeField] ManualDamageCollider leftHandDamageCollider;

        [Header("Damage Modifiers")]
        [SerializeField] float attack01DamageModifier = 1.2f;
        [SerializeField] float attack02DamageModifier = 1.65f;
        [SerializeField] float attack03DamageModifier = 1.9f;
        [SerializeField] float poweredUpDamageMultiplier = 1.3f;
        [SerializeField] float poweredUpFireDamageMultiplier = 0.45f;

        bool isPoweredUp;
        public bool IsPoweredUp => isPoweredUp;

        protected override void Awake()
        {
            base.Awake();
            ResolveDamageColliders();
        }

#if UNITY_EDITOR
        void OnValidate()
        {
            ResolveDamageColliders();
        }
#endif

        public void SetAttack01Damage()
        {
            ResolveDamageColliders();
            ApplyDamage(rightHandDamageCollider, attack01DamageModifier);
            ApplyDamage(leftHandDamageCollider, attack01DamageModifier);
            aiCharacter.characterSoundFXManager.PlayAttackGruntSoundFX();
        }

        public void SetAttack02Damage()
        {
            ResolveDamageColliders();
            ApplyDamage(rightHandDamageCollider, attack02DamageModifier);
            ApplyDamage(leftHandDamageCollider, attack02DamageModifier);
            aiCharacter.characterSoundFXManager.PlayAttackGruntSoundFX();
        }

        public void SetAttack03Damage()
        {
            ResolveDamageColliders();
            ApplyDamage(rightHandDamageCollider, attack03DamageModifier);
            ApplyDamage(leftHandDamageCollider, attack03DamageModifier);
            aiCharacter.characterSoundFXManager.PlayAttackGruntSoundFX();
        }

        public void OpenRightHandDamageCollider()
        {
            ResolveDamageColliders();
            rightHandDamageCollider?.EnableDamageCollider();
        }

        public void CloseRightHandDamageCollider()
        {
            rightHandDamageCollider?.DisableDamageCollider();
        }

        public void OpenLeftHandDamageCollider()
        {
            ResolveDamageColliders();
            leftHandDamageCollider?.EnableDamageCollider();
        }

        public void CloseLeftHandDamageCollider()
        {
            leftHandDamageCollider?.DisableDamageCollider();
        }

        public override void CloseAllDamageColliders()
        {
            base.CloseAllDamageColliders();

            rightHandDamageCollider?.DisableDamageCollider();
            leftHandDamageCollider?.DisableDamageCollider();
        }

        public override void PivotTowardsTarget(AICharacterManager aiCharacter)
        {
            if (aiCharacter.isPerformingAction)
                return;

            if (viewableAngle >= 61 && viewableAngle <= 110)
            {
                aiCharacter.characterAnimatorManager.PlayTargetActionAnimation("Turn_Right_90", true);
            }
            else if (viewableAngle <= -61 && viewableAngle >= -110)
            {
                aiCharacter.characterAnimatorManager.PlayTargetActionAnimation("Turn_Left_90", true);
            }
            else if (viewableAngle > 146 || viewableAngle < -146)
            {
                aiCharacter.characterAnimatorManager.PlayTargetActionAnimation("Turn_Right_180", true);
            }
        }

        public void ApplyPowerUpBuff()
        {
            isPoweredUp = true;
            ImbueFireDamage(rightHandDamageCollider);
            ImbueFireDamage(leftHandDamageCollider);
        }

        private void ApplyDamage(ManualDamageCollider collider, float modifier)
        {
            if (collider == null)
                return;

            float totalModifier = modifier * (isPoweredUp ? poweredUpDamageMultiplier : 1f);
            collider.physicalDamage = baseDamage * totalModifier;
            collider.fireDamage = isPoweredUp ? baseDamage * modifier * poweredUpFireDamageMultiplier : 0f;
            collider.poiseDamage = basePoiseDamage * totalModifier;
        }

        private void SetFirePhaseOnCollider(ManualDamageCollider collider)
        {
            if (collider is Monster33FireDamageCollider fireDamageCollider)
                fireDamageCollider.SetFirePhaseActive(isPoweredUp);
        }

        private void ImbueFireDamage(ManualDamageCollider collider)
        {
            if (collider == null)
                return;

            SetFirePhaseOnCollider(collider);
            collider.fireDamage = baseDamage * poweredUpFireDamageMultiplier;
        }

        private void ResolveDamageColliders()
        {
            if (rightHandDamageCollider == null)
                rightHandDamageCollider = FindColliderByObjectName("Monster33_Weapon_01_Hitbox")
                    ?? FindColliderByObjectName("root_dupli_001.x");

            if (leftHandDamageCollider == null)
                leftHandDamageCollider = FindColliderByObjectName("Monster33_Weapon_02_Hitbox")
                    ?? FindColliderByObjectName("root_dupli_002.x");
        }

        private ManualDamageCollider FindColliderByObjectName(string objectName)
        {
            foreach (var collider in GetComponentsInChildren<ManualDamageCollider>(true))
            {
                if (collider == null)
                    continue;

                if (collider.gameObject.name == objectName)
                    return collider;

                if (collider.transform.parent != null && collider.transform.parent.name == objectName)
                    return collider;
            }

            return null;
        }
    }
}

