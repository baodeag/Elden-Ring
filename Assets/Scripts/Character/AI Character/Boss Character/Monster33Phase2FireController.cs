using System.Collections;
using UnityEngine;

namespace baodeag
{
    public class Monster33Phase2FireController : MonoBehaviour
    {
        [SerializeField] GameObject bodyFireVFXPrefab;
        [SerializeField] GameObject weaponTrailPrefab;
        [SerializeField] Material weaponFireMaterial;
        [SerializeField] Transform visualRoot;
        [SerializeField] Transform rightWeaponRoot;
        [SerializeField] Transform leftWeaponRoot;
        [SerializeField] float activationDelay = 2.2f;
        [SerializeField] float bodyFireScale = 1.8f;
        [SerializeField] Vector3 bodyFireLocalPosition = new Vector3(0f, 0.9f, 0f);

        GameObject activeBodyFireVFX;
        GameObject rightWeaponTrail;
        GameObject leftWeaponTrail;
        Material runtimeFireMaterial;
        bool isActive;
        Coroutine activationCoroutine;

        public void Configure(
            GameObject bodyFireVFXPrefab,
            GameObject weaponTrailPrefab,
            Material weaponFireMaterial,
            Transform visualRoot,
            Transform rightWeaponRoot,
            Transform leftWeaponRoot)
        {
            this.bodyFireVFXPrefab = bodyFireVFXPrefab;
            this.weaponTrailPrefab = weaponTrailPrefab;
            this.weaponFireMaterial = weaponFireMaterial;
            this.visualRoot = visualRoot;
            this.rightWeaponRoot = rightWeaponRoot;
            this.leftWeaponRoot = leftWeaponRoot;
        }

        public void ActivateAfterPowerUpAnimation()
        {
            if (isActive)
                return;

            if (activationCoroutine != null)
                StopCoroutine(activationCoroutine);

            activationCoroutine = StartCoroutine(ActivateAfterDelay());
        }

        public void ActivateNow()
        {
            if (isActive)
                return;

            isActive = true;
            SpawnBodyFireVFX();
            ApplyFullBodyFireMaterial();
            SpawnWeaponTrails();
        }

        private IEnumerator ActivateAfterDelay()
        {
            yield return new WaitForSeconds(activationDelay);
            ActivateNow();
        }

        private void SpawnBodyFireVFX()
        {
            if (bodyFireVFXPrefab == null || activeBodyFireVFX != null)
                return;

            Transform parent = visualRoot != null ? visualRoot : transform;
            activeBodyFireVFX = Instantiate(bodyFireVFXPrefab, parent);
            activeBodyFireVFX.transform.localPosition = bodyFireLocalPosition;
            activeBodyFireVFX.transform.localRotation = Quaternion.identity;
            activeBodyFireVFX.transform.localScale = Vector3.one * bodyFireScale;
        }

        private void SpawnWeaponTrails()
        {
            rightWeaponTrail = SpawnWeaponTrail(rightWeaponRoot, rightWeaponTrail);
            leftWeaponTrail = SpawnWeaponTrail(leftWeaponRoot, leftWeaponTrail);
        }

        private GameObject SpawnWeaponTrail(Transform weaponRoot, GameObject existingTrail)
        {
            if (weaponRoot == null || weaponTrailPrefab == null || existingTrail != null)
                return existingTrail;

            GameObject trail = Instantiate(weaponTrailPrefab, weaponRoot);
            trail.transform.localPosition = Vector3.zero;
            trail.transform.localRotation = Quaternion.identity;
            trail.transform.localScale = Vector3.one;
            return trail;
        }

        private void ApplyFullBodyFireMaterial()
        {
            Material fireMaterial = GetFireMaterial();

            if (fireMaterial == null)
                return;

            Transform parent = visualRoot != null ? visualRoot : transform;

            foreach (var renderer in parent.GetComponentsInChildren<Renderer>(true))
            {
                if (renderer == null || renderer is ParticleSystemRenderer)
                    continue;

                renderer.material = fireMaterial;
            }
        }

        private Material GetFireMaterial()
        {
            if (weaponFireMaterial != null)
                return weaponFireMaterial;

            if (runtimeFireMaterial != null)
                return runtimeFireMaterial;

            Shader shader = Shader.Find("Universal Render Pipeline/Lit");
            if (shader == null)
                shader = Shader.Find("Standard");

            if (shader == null)
                return null;

            runtimeFireMaterial = new Material(shader)
            {
                name = "Monster33_Runtime_Fire_Orange",
                color = new Color(1f, 0.38f, 0.04f, 1f)
            };

            runtimeFireMaterial.EnableKeyword("_EMISSION");
            runtimeFireMaterial.SetColor("_EmissionColor", new Color(3.5f, 0.95f, 0.08f, 1f));
            return runtimeFireMaterial;
        }
    }
}
