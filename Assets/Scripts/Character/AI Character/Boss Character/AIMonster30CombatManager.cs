using UnityEngine;

namespace baodeag
{
    public class AIMonster30CombatManager : AICharacterCombatManager
    {
        [Header("Damage Colliders")]
        [SerializeField] ManualDamageCollider rightHandDamageCollider;
        [SerializeField] ManualDamageCollider leftHandDamageCollider;

        [Header("Damage Modifiers")]
        [SerializeField] float attack01DamageModifier = 1.2f;
        [SerializeField] float attack02DamageModifier = 1.65f;
        [SerializeField] float attack03DamageModifier = 1.9f;

        public void SetAttack01Damage()
        {
            ApplyDamage(rightHandDamageCollider, attack01DamageModifier);
            ApplyDamage(leftHandDamageCollider, attack01DamageModifier);
            aiCharacter.characterSoundFXManager.PlayAttackGruntSoundFX();
        }

        public void SetAttack02Damage()
        {
            ApplyDamage(rightHandDamageCollider, attack02DamageModifier);
            ApplyDamage(leftHandDamageCollider, attack02DamageModifier);
            aiCharacter.characterSoundFXManager.PlayAttackGruntSoundFX();
        }

        public void SetAttack03Damage()
        {
            ApplyDamage(rightHandDamageCollider, attack03DamageModifier);
            ApplyDamage(leftHandDamageCollider, attack03DamageModifier);
            aiCharacter.characterSoundFXManager.PlayAttackGruntSoundFX();
        }

        public void OpenRightHandDamageCollider()
        {
            rightHandDamageCollider?.EnableDamageCollider();
        }

        public void CloseRightHandDamageCollider()
        {
            rightHandDamageCollider?.DisableDamageCollider();
        }

        public void OpenLeftHandDamageCollider()
        {
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

        private void ApplyDamage(ManualDamageCollider collider, float modifier)
        {
            if (collider == null)
                return;

            collider.physicalDamage = baseDamage * modifier;
            collider.poiseDamage = basePoiseDamage * modifier;
        }
    }
}
