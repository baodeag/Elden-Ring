using UnityEngine;

namespace baodeag
{
    public class RangedProjectileDamageCollider : DamageCollider
    {
        [Header("Marksmen")]
        public CharacterManager characterShootingProjectile;

        [Header("Collision")]
        private bool hasPenetratedSurface = false;
        public Rigidbody rigidbody;
        private CapsuleCollider capsuleCollider;

        protected override void Awake()
        {
            base.Awake();

            rigidbody = GetComponent<Rigidbody>();
            capsuleCollider = GetComponent<CapsuleCollider>();
        }

        private void FixedUpdate()
        {
            if (rigidbody.linearVelocity != Vector3.zero)
            {
                rigidbody.rotation = Quaternion.LookRotation(rigidbody.linearVelocity);
            }
        }

        private void OnCollisionEnter(Collision collision)
        {
            CreatePenetrationIntoObject(collision);

            WorldSoundFXManager.instance.AlertNearbyCharactersToSound(transform.position, 3);

            CharacterManager potentialTarget = collision.transform.gameObject.GetComponent<CharacterManager>();

            if (characterShootingProjectile == null)
                return;

            Collider contactCollider = collision.gameObject.GetComponent<Collider>();

            if (contactCollider != null)
                contactPoint = contactCollider.ClosestPointOnBounds(transform.position);

            if (potentialTarget == null)
                return;

            if (WorldUtilityManager.Instance.CanIDamageThisTarget(characterShootingProjectile.characterGroup, potentialTarget.characterGroup))
            {
                CheckForBlock(potentialTarget);
                DamageTarget(potentialTarget);
            }
        }

        protected override void CheckForBlock(CharacterManager damageTarget)
        {
            if (charactersDamaged.Contains(damageTarget))
                return;

            float angle = Vector3.Angle(damageTarget.transform.forward, transform.forward);

            if (damageTarget.characterNetworkManager.isBlocking.Value && angle > 145)
            {
                charactersDamaged.Add(damageTarget);
                TakeBlockedDamageEffect blockedDamageEffect = Instantiate(WorldCharacterEffectsManager.instance.takeBlockedDamageEffect);

                if (characterShootingProjectile != null)
                    blockedDamageEffect.characterCausingDamage = characterShootingProjectile;

                blockedDamageEffect.physicalDamage = physicalDamage;
                blockedDamageEffect.magicDamage = magicDamage;
                blockedDamageEffect.fireDamage = fireDamage;
                blockedDamageEffect.lightningDamage = lightningDamage;
                blockedDamageEffect.holyDamage = holyDamage;
                blockedDamageEffect.poiseDamage = poiseDamage;
                blockedDamageEffect.staminaDamage = poiseDamage;
                blockedDamageEffect.contactPoint = contactPoint;

                damageTarget.characterEffectsManager.ProcessInstantEffect(blockedDamageEffect);
            }
        }

        private void CreatePenetrationIntoObject(Collision hit)
        {
            if (!hasPenetratedSurface)
            {
                hasPenetratedSurface = true;

                //get the contact point
                gameObject.transform.position = hit.GetContact(0).point;

                //stop our arrow from scaling in size with scaled up or down objects
                var emptyObject = new GameObject();
                emptyObject.transform.parent = hit.collider.transform;
                gameObject.transform.SetParent(emptyObject.transform, true);

                //how far the arrow penetrates into the surface
                transform.position += transform.forward * (Random.Range(0.1f, 0.3f));

                //disable colliders and rigidbody
                rigidbody.isKinematic = true;
                capsuleCollider.enabled = false;

                //destroy damage collider, and destroy arrow after a time
                Destroy(GetComponent<RangedProjectileDamageCollider>());
                Destroy(gameObject, 20);
            }
        }
    }
}
