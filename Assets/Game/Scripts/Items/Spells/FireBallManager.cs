using System.Collections;
using UnityEngine;

namespace baodeag
{
    public class FireBallManager : SpellManager
    {
        [Header("Colliders")]
        public FireBallDamageCollider damageCollider;

        [Header("Instantiated FX")]
        private GameObject instantiatedDestructionFX;

        private bool hasCollided = false;
        public bool isFullyCharged = false;
        private Rigidbody fireballRigidbody;
        private Coroutine destructionFXCoroutine;

        protected override void Awake()
        {
            base.Awake();

            fireballRigidbody = GetComponent<Rigidbody>();
            
        }

        protected override void Update()
        {
            base.Update();

            if (spellTarget != null)
                transform.LookAt(spellTarget.characterCombatManager.lockOnTransform.position);
        }

        private void OnCollisionEnter(Collision collision)
        {
            //if we collide with a character, ignore this we will let the damage collider handle character collisions, this is just for impact vfx
            if (collision.gameObject.layer == 6)
                return;

            if (!hasCollided)
            {
                hasCollided = true;
                InstantiateSpellDestructionFX();
            }
        }

        public void InitializeFireBall(CharacterManager spellCaster)
        {
            damageCollider.spellCaster = spellCaster;

            damageCollider.fireDamage = 150;

            if (isFullyCharged)
                damageCollider.fireDamage *= 1.4f;
        }

        public void InstantiateSpellDestructionFX()
        {
            if (isFullyCharged)
            {
                instantiatedDestructionFX = Instantiate(impactParticleFullCharge, transform.position, Quaternion.identity);
            }
            else
            {
                instantiatedDestructionFX = Instantiate(impactParticle, transform.position, Quaternion.identity);
            }

            WorldSoundFXManager.instance.AlertNearbyCharactersToSound(transform.position, 5);

            Destroy(gameObject);
        }

        public void WaitThenInstantiateSpellDestructionFX(float timeToWait)
        {
            if (destructionFXCoroutine != null)
                StopCoroutine(destructionFXCoroutine);

            destructionFXCoroutine = StartCoroutine(WaitThenInstantiateFX(timeToWait));
            StartCoroutine(WaitThenInstantiateFX(timeToWait));
        }

        private IEnumerator WaitThenInstantiateFX(float timeToWait)
        {
            yield return new WaitForSeconds(timeToWait);

            InstantiateSpellDestructionFX();
        }
    }
}
