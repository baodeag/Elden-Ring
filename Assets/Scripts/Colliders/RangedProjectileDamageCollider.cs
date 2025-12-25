using UnityEngine;

namespace baodeag
{
    public class RangedProjectileDamageCollider : DamageCollider
    {
        [Header("Marksmen")]
        public CharacterManager characterShootingProjectile;

        [Header("Collision")]
        private bool hasCollided = false;
        public Rigidbody rigidbody;

        protected override void Awake()
        {
            base.Awake();

            rigidbody = GetComponent<Rigidbody>();
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
            if (!hasCollided)
            {
                //hasCollided = true;

                CharacterManager potentialTarget = collision.transform.gameObject.GetComponent<CharacterManager>();

                if (characterShootingProjectile == null)
                    return;

                if (potentialTarget == null)
                    return;

                if (WorldUtilityManager.Instance.CanIDamageThisTarget(characterShootingProjectile.characterGroup, potentialTarget.characterGroup))
                {
                    DamageTarget(potentialTarget);
                }

                Destroy(gameObject);
            }
        }
    }
}
