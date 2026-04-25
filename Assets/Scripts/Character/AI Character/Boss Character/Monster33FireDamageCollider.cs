using UnityEngine;

namespace baodeag
{
    public class Monster33FireDamageCollider : ManualDamageCollider
    {
        [SerializeField] bool firePhaseActive;
        [SerializeField] GameObject burningHitVFXPrefab;

        public void SetFirePhaseActive(bool active)
        {
            firePhaseActive = active;
        }

        public void ConfigureFireHit(GameObject burningHitVFXPrefab, int fireBuildUpAmount)
        {
            this.burningHitVFXPrefab = burningHitVFXPrefab;
        }

        protected override void DamageTarget(CharacterManager damageTarget)
        {
            // Check before base call, since base will add damageTarget to charactersDamaged
            bool canSpawnFireVFX = firePhaseActive && damageTarget != null && !charactersDamaged.Contains(damageTarget);
            base.DamageTarget(damageTarget);

            // Fire build-up is already applied by ManualDamageCollider.ApplyMonster33PowerUpFireBuildUp
            // when boss is powered up. Here we only handle the visual (burning hit VFX).
            if (!canSpawnFireVFX)
                return;

            SpawnBurningHitVFX();
        }

        private void SpawnBurningHitVFX()
        {
            if (burningHitVFXPrefab == null)
                return;

            GameObject hitVFX = Instantiate(burningHitVFXPrefab, contactPoint, Quaternion.identity);
            Destroy(hitVFX, 5f);
        }
    }
}
