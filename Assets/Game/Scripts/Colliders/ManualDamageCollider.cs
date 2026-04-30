using UnityEngine;

namespace baodeag
{
    public class ManualDamageCollider : DamageCollider
    {
        [SerializeField] AICharacterManager characterCausingDamage;

        protected override void Awake()
        {
            base.Awake();

            damageCollider = GetComponent<Collider>();
            characterCausingDamage = GetComponentInParent<AICharacterManager>();
        }

        protected override void GetBlockingDotValues(CharacterManager damageTarget)
        {
            directionFromAttackToDamageTarget = characterCausingDamage.transform.position - damageTarget.transform.position;
            dotValueFromAttackToDamageTarget = Vector3.Dot(directionFromAttackToDamageTarget, damageTarget.transform.forward);
        }

        protected override void DamageTarget(CharacterManager damageTarget)
        {
            if (charactersDamaged.Contains(damageTarget))
                return;

            characterCausingDamage.aiCharacterCombatManager.hasHitTargetDuringCombo = true;

            charactersDamaged.Add(damageTarget);
            ApplyMonster33PowerUpFireBuildUp(damageTarget);
            ApplyKnightPowerUpFrostBuildUp(damageTarget);

            TakeDamageEffect damageEffect = Instantiate(WorldCharacterEffectsManager.instance.takeDamageEffect);
            damageEffect.physicalDamage = physicalDamage;
            damageEffect.magicDamage = magicDamage;
            damageEffect.fireDamage = fireDamage;
            damageEffect.lightningDamage = lightningDamage;
            damageEffect.holyDamage = holyDamage;
            damageEffect.poiseDamage = poiseDamage;
            damageEffect.contactPoint = contactPoint;
            damageEffect.angleHitFrom = Vector3.SignedAngle(characterCausingDamage.transform.forward, damageTarget.transform.forward, Vector3.up);

            if (damageTarget.IsOwner)
            {
                //send a damage request to the server
                damageTarget.characterNetworkManager.NotifyTheServerOfCharacterDamageServerRpc(
                    damageTarget.NetworkObjectId,
                    characterCausingDamage.NetworkObjectId,
                    damageEffect.physicalDamage,
                    damageEffect.magicDamage,
                    damageEffect.fireDamage,
                    damageEffect.lightningDamage,
                    damageEffect.holyDamage,
                    damageEffect.poiseDamage,
                    damageEffect.angleHitFrom,
                    damageEffect.contactPoint.x,
                    damageEffect.contactPoint.y,
                    damageEffect.contactPoint.z);
            }
        }

        private void ApplyMonster33PowerUpFireBuildUp(CharacterManager damageTarget)
        {
            if (!damageTarget.IsOwner)
                return;

            if (damageTarget is not PlayerManager player || player.playerEffectsManager == null)
                return;

            var monster33CombatManager = characterCausingDamage != null
                ? characterCausingDamage.GetComponent<AIMonster33CombatManager>()
                : null;

            if (monster33CombatManager == null || !monster33CombatManager.IsPoweredUp)
                return;

            player.playerEffectsManager.ApplyFireBuildUpFromHit(PlayerEffectsManager.DefaultFireBuildUpFromHit);
        }

        private void ApplyKnightPowerUpFrostBuildUp(CharacterManager damageTarget)
        {
            if (!damageTarget.IsOwner)
                return;

            if (damageTarget is not PlayerManager player || player.playerEffectsManager == null)
                return;

            var knightCombatManager = characterCausingDamage != null
                ? characterCausingDamage.GetComponent<AIKnightCombatManager>()
                : null;

            if (knightCombatManager == null || !knightCombatManager.IsPoweredUp)
                return;

            player.playerEffectsManager.ApplyFrostBuildUpFromHit(knightCombatManager.PoweredUpFrostBuildUpAmount);
        }

        protected override void CheckForParry(CharacterManager damageTarget)
        {
            if (charactersDamaged.Contains(damageTarget))
                return;

            if (!characterCausingDamage.characterNetworkManager.isParryable.Value)
                return;

            if (!damageTarget.IsOwner)
                return;

            if (damageTarget.characterNetworkManager.isParrying.Value)
            {
                charactersDamaged.Add(damageTarget);
                damageTarget.characterNetworkManager.NotifyServerOfParryServerRpc(characterCausingDamage.NetworkObjectId);
                damageTarget.characterAnimatorManager.PlayTargetActionAnimationInstantly("Parry_Land_01", true);
            }
        }
    }
}
