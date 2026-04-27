using System.Collections.Generic;
using UnityEngine;

namespace baodeag
{
    [RequireComponent(typeof(SphereCollider))]
    [RequireComponent(typeof(Rigidbody))]
    public class TwinMoonShockwaveHitbox : MonoBehaviour
    {
        SphereCollider triggerCollider;
        TwinMoonSkill ownerSkill;
        AICharacterManager sourceCharacter;
        readonly HashSet<ulong> hitTargets = new HashSet<ulong>();

        float maxRadius;
        float duration;
        float damage;
        float poiseDamage;
        float knockbackForce;
        float shellThickness;
        float verticalTolerance;
        float elapsed;
        float previousRadius;
        float currentRadius;
        bool initialized;

        void Awake()
        {
            triggerCollider = GetComponent<SphereCollider>();
            triggerCollider.isTrigger = true;

            Rigidbody body = GetComponent<Rigidbody>();
            body.isKinematic = true;
            body.useGravity = false;
            body.constraints = RigidbodyConstraints.FreezeAll;
        }

        public void Initialize(
            TwinMoonSkill skill,
            AICharacterManager source,
            float targetRadius,
            float lifetime,
            float waveDamage,
            float wavePoiseDamage,
            float waveKnockbackForce,
            float hitShellThickness,
            float hitVerticalTolerance)
        {
            ownerSkill = skill;
            sourceCharacter = source;
            maxRadius = Mathf.Max(0.1f, targetRadius);
            duration = Mathf.Max(0.05f, lifetime);
            damage = waveDamage;
            poiseDamage = wavePoiseDamage;
            knockbackForce = waveKnockbackForce;
            shellThickness = Mathf.Max(0.05f, hitShellThickness);
            verticalTolerance = Mathf.Max(0.05f, hitVerticalTolerance);
            previousRadius = 0f;
            currentRadius = 0f;
            elapsed = 0f;
            initialized = true;

            UpdateColliderRadius();
            Destroy(gameObject, duration + 0.25f);
        }

        void Update()
        {
            if (!initialized)
                return;

            elapsed += Time.deltaTime;
            previousRadius = currentRadius;
            currentRadius = Mathf.Lerp(0f, maxRadius, Mathf.Clamp01(elapsed / duration));
            UpdateColliderRadius();
        }

        void OnTriggerEnter(Collider other)
        {
            TryDamageTarget(other);
        }

        void OnTriggerStay(Collider other)
        {
            TryDamageTarget(other);
        }

        void UpdateColliderRadius()
        {
            if (triggerCollider == null)
                return;

            triggerCollider.radius = currentRadius + (shellThickness * 0.5f);
        }

        void TryDamageTarget(Collider other)
        {
            if (!initialized || ownerSkill == null || sourceCharacter == null || other == null)
                return;

            CharacterManager target = other.GetComponentInParent<CharacterManager>();

            if (target == null || target == sourceCharacter || target.isDead.Value)
                return;

            if (!WorldUtilityManager.Instance.CanIDamageThisTarget(sourceCharacter.characterGroup, target.characterGroup))
                return;

            if (target.characterLocomotionManager != null && !target.characterLocomotionManager.isGrounded)
                return;

            if (target.characterNetworkManager != null)
            {
                if (target.characterNetworkManager.isJumping.Value || target.characterNetworkManager.isRolling.Value)
                    return;
            }

            if (hitTargets.Contains(target.NetworkObjectId))
                return;

            Vector3 closestPoint = other.ClosestPoint(transform.position);
            Vector3 offset = closestPoint - transform.position;
            float verticalOffset = Mathf.Abs(offset.y);
            offset.y = 0f;
            float planarDistance = offset.magnitude;
            float shellHalfThickness = shellThickness * 0.5f;
            float shellStart = Mathf.Max(0f, previousRadius - shellHalfThickness);
            float shellEnd = currentRadius + shellHalfThickness;

            if (verticalOffset > verticalTolerance)
                return;

            if (planarDistance < shellStart || planarDistance > shellEnd)
                return;

            hitTargets.Add(target.NetworkObjectId);
            ownerSkill.ApplyShockwaveHit(target, damage, poiseDamage, knockbackForce);
        }
    }
}
