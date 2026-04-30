using System.Collections;
using Unity.Netcode;
using UnityEngine;

namespace baodeag
{
    public class DeathMoonSlash : NetworkBehaviour
    {
        [Header("Activation")]
        [SerializeField] bool onlyAvailableWhenPoweredUp = true;
        [SerializeField] bool activatePowerUpAtHalfHealth = true;
        [SerializeField, Range(0f, 1f)] float powerUpHealthThreshold = 0.5f;
        [SerializeField] float minimumActivationDistance = 2.5f;
        [SerializeField] float maximumActivationDistance = 14f;
        [SerializeField, Range(0, 100)] int activationChance = 100;
        [SerializeField] float cooldown = 10f;
        [SerializeField] float decisionLockout = 1f;
        [SerializeField] float poweredUpDamageMultiplier = 1.2f;

        [Header("Charge")]
        [SerializeField] float minChargeTime = 0.3f;
        [SerializeField] float maxChargeTime = 0.6f;
        [SerializeField] string chargeAnimation = "Main_Charge_01";
        [SerializeField] string slashAnimation = "TH_Light_Attack_01";
        [SerializeField] string chargeAnimatorTrigger = "";
        [SerializeField] string slashAnimatorTrigger = "";
        [SerializeField] float slashReleaseDelay = 0.18f;

        [Header("Projectile Setup")]
        [SerializeField] MoonSlashProjectile projectilePrefab;
        [SerializeField] Transform projectileSpawnPoint;
        [SerializeField] GameObject chargeVFXPrefab;
        [SerializeField] GameObject slashCastVFXPrefab;
        [SerializeField] GameObject auraVFXPrefab;
        [SerializeField] GameObject projectileImpactVFXPrefab;
        [SerializeField] LayerMask targetLayers;
        [SerializeField] float damage = 38f;
        [SerializeField] float speed = 19f;
        [SerializeField] int numberOfSlashes = 1;
        [SerializeField] float spreadAngle = 12f;
        [SerializeField] float projectileLifetime = 3f;
        [SerializeField] float projectilePoiseDamage = 16f;
        [SerializeField] float attackSlashDamageMultiplier = 0.65f;
        [SerializeField] float attackSlashPoiseMultiplier = 0.65f;
        [SerializeField] float attackSlashCooldown = 0.15f;

        [Header("Phase 2 Combo")]
        [SerializeField] int phaseTwoNumberOfSlashes = 3;
        [SerializeField] float phaseTwoSpreadAngle = 16f;
        [SerializeField] float comboDelay = 0.28f;

        [Header("Feedback")]
        [SerializeField] bool enableCameraShake = true;
        [SerializeField] float cameraShakeDuration = 0.12f;
        [SerializeField] float cameraShakeStrength = 0.12f;
        [SerializeField] float cameraShakeRange = 14f;
        [SerializeField] Color soulAuraColor = new Color(0.48f, 0.15f, 0.78f, 1f);
        [SerializeField] Color soulCoreColor = new Color(0.2f, 0.36f, 0.78f, 1f);
        [SerializeField] Vector3 auraLocalOffset = new Vector3(0f, 0.95f, 0f);

        AICharacterManager aiCharacter;
        AITormentedSoulCombatManager tormentedSoulCombatManager;
        AITormentedSoulBossCharacterManager bossCharacterManager;
        Rigidbody attachedRigidbody;

        Coroutine activeSkillRoutine;
        Coroutine activeShakeRoutine;
        GameObject activeChargeVFX;
        GameObject activeAuraVFX;
        Transform cachedTarget;
        float nextReadyTime;
        float nextDecisionTime;
        float nextAttackSlashReadyTime;
        bool isPoweredUp;

        public bool TryActivateSkill()
        {
            if (!IsOwner || aiCharacter == null || tormentedSoulCombatManager == null)
                return false;

            if (projectilePrefab == null)
                return false;

            if (activeSkillRoutine != null || aiCharacter.isDead.Value || aiCharacter.isPerformingAction)
                return false;

            if (onlyAvailableWhenPoweredUp && !isPoweredUp)
                return false;

            if (Time.time < nextReadyTime || Time.time < nextDecisionTime)
                return false;

            CharacterManager target = tormentedSoulCombatManager.currentTarget;

            if (target == null || target.isDead.Value)
                return false;

            float distanceToTarget = Vector3.Distance(transform.position, target.transform.position);

            if (distanceToTarget < minimumActivationDistance || distanceToTarget > maximumActivationDistance)
                return false;

            nextDecisionTime = Time.time + decisionLockout;

            if (Random.Range(0, 100) >= activationChance)
                return false;

            nextReadyTime = Time.time + cooldown;
            cachedTarget = target.transform;
            activeSkillRoutine = StartCoroutine(PerformDeathMoonSlash());
            return true;
        }

        void Awake()
        {
            aiCharacter = GetComponent<AICharacterManager>();
            tormentedSoulCombatManager = GetComponent<AITormentedSoulCombatManager>();
            bossCharacterManager = GetComponent<AITormentedSoulBossCharacterManager>();
            attachedRigidbody = GetComponent<Rigidbody>();
            TryAutoAssignProjectileSpawnPoint();
        }

        void OnEnable()
        {
            TryAutoAssignProjectileSpawnPoint();
            EvaluatePowerUpState();
            RefreshAuraState();
        }

        void OnDisable()
        {
            DestroyChargeVFX();
            DestroyAuraVFX();
        }

        void Update()
        {
            EvaluatePowerUpState();
            RefreshAuraState();
        }

        public override void OnNetworkSpawn()
        {
            base.OnNetworkSpawn();
            TryAutoAssignProjectileSpawnPoint();
            EvaluatePowerUpState();
            RefreshAuraState();
        }

        IEnumerator PerformDeathMoonSlash()
        {
            SetSkillState(true);
            StopNavigation();

            float chargeTime = Random.Range(minChargeTime, maxChargeTime);
            Vector3 lockedForward = GetLockedForward();

            PlayAnimation(chargeAnimation, chargeAnimatorTrigger);
            SpawnChargeVFX(chargeTime);

            float elapsedCharge = 0f;

            while (elapsedCharge < chargeTime)
            {
                elapsedCharge += Time.deltaTime;
                FaceTowards(lockedForward, 18f);
                HaltCharacterMomentum();
                yield return null;
            }

            DestroyChargeVFX();
            PlayAnimation(slashAnimation, slashAnimatorTrigger);

            yield return new WaitForSeconds(slashReleaseDelay);

            int slashCount = GetSlashCount();
            float totalSpread = GetSpreadAngle();

            for (int i = 0; i < slashCount; i++)
            {
                Vector3 shotDirection = GetSpreadDirection(lockedForward, i, slashCount, totalSpread);
                SpawnMoonSlashProjectile(shotDirection);

                if (IsServer)
                    BroadcastSlashFeedbackClientRpc(GetProjectileOrigin(), shotDirection);

                if (i < slashCount - 1)
                    yield return new WaitForSeconds(comboDelay);
            }

            yield return new WaitForSeconds(0.25f);
            ResetAfterSkill();
            activeSkillRoutine = null;
        }

        void StopNavigation()
        {
            if (aiCharacter != null && aiCharacter.navMeshAgent != null && aiCharacter.navMeshAgent.enabled)
            {
                aiCharacter.navMeshAgent.isStopped = true;
                aiCharacter.navMeshAgent.ResetPath();
                aiCharacter.navMeshAgent.enabled = false;
            }
        }

        void HaltCharacterMomentum()
        {
            if (attachedRigidbody != null)
            {
                if (!attachedRigidbody.isKinematic)
                    attachedRigidbody.linearVelocity = Vector3.zero;

                if (!attachedRigidbody.isKinematic)
                    attachedRigidbody.angularVelocity = Vector3.zero;
            }
        }

        public void FireAttackSlash()
        {
            if (!IsOwner || projectilePrefab == null || aiCharacter == null || aiCharacter.isDead.Value)
                return;

            if (!isPoweredUp)
                return;

            if (Time.time < nextAttackSlashReadyTime)
                return;

            nextAttackSlashReadyTime = Time.time + attackSlashCooldown;

            Vector3 direction = GetLockedForward();

            if (direction.sqrMagnitude <= 0.0001f)
                direction = transform.forward;

            SpawnMoonSlashProjectile(
                direction,
                damage * attackSlashDamageMultiplier,
                projectilePoiseDamage * attackSlashPoiseMultiplier);

            if (IsServer)
                BroadcastSlashFeedbackClientRpc(GetProjectileOrigin(), direction);
        }

        void SpawnMoonSlashProjectile(Vector3 direction)
        {
            SpawnMoonSlashProjectile(direction, damage, projectilePoiseDamage);
        }

        void SpawnMoonSlashProjectile(Vector3 direction, float projectileDamage, float projectilePoise)
        {
            Vector3 spawnPosition = GetProjectileOrigin();
            Quaternion spawnRotation = Quaternion.LookRotation(direction);
            MoonSlashProjectile projectile = Instantiate(projectilePrefab, spawnPosition, spawnRotation);

            projectile.Initialize(
                aiCharacter,
                direction,
                projectileDamage,
                speed,
                projectileLifetime,
                projectilePoise,
                targetLayers,
                projectileImpactVFXPrefab,
                soulAuraColor,
                soulCoreColor);

            NetworkObject networkObject = projectile.GetComponent<NetworkObject>();

            if (networkObject != null && IsServer && !networkObject.IsSpawned)
                networkObject.Spawn(true);
        }

        void SetSkillState(bool skillIsActive)
        {
            aiCharacter.isPerformingAction = skillIsActive;
            aiCharacter.characterLocomotionManager.canMove = !skillIsActive;
            aiCharacter.characterLocomotionManager.canRotate = !skillIsActive;
            aiCharacter.characterLocomotionManager.canRun = !skillIsActive;
            aiCharacter.characterLocomotionManager.canRoll = !skillIsActive;

            if (IsOwner)
            {
                aiCharacter.characterNetworkManager.isAttacking.Value = skillIsActive;
                aiCharacter.characterNetworkManager.isInvulnerable.Value = skillIsActive;
            }
        }

        void ResetAfterSkill()
        {
            DestroyChargeVFX();
            SetSkillState(false);
            aiCharacter.characterCombatManager.DisableCanDoCombo();
            aiCharacter.characterCombatManager.DisableCanDoRollingAttack();
            aiCharacter.characterCombatManager.DisableCanDoBackstepAttack();

            if (aiCharacter != null && aiCharacter.navMeshAgent != null && !aiCharacter.navMeshAgent.enabled)
                aiCharacter.navMeshAgent.enabled = true;
        }

        int GetSlashCount()
        {
            return Mathf.Max(1, IsInPhaseTwo() ? phaseTwoNumberOfSlashes : numberOfSlashes);
        }

        float GetSpreadAngle()
        {
            return Mathf.Max(0f, IsInPhaseTwo() ? phaseTwoSpreadAngle : spreadAngle);
        }

        bool IsInPhaseTwo()
        {
            if (bossCharacterManager == null || bossCharacterManager.characterNetworkManager == null)
                return isPoweredUp;

            int maxHealth = bossCharacterManager.characterNetworkManager.maxHealth.Value;

            if (maxHealth <= 0)
                return isPoweredUp;

            float currentHealthPercent = (bossCharacterManager.characterNetworkManager.currentHealth.Value / (float)maxHealth) * 100f;
            return currentHealthPercent <= bossCharacterManager.minimumHealthPercentageToShift || isPoweredUp;
        }

        void EvaluatePowerUpState()
        {
            if (!activatePowerUpAtHalfHealth || isPoweredUp || bossCharacterManager == null || bossCharacterManager.characterNetworkManager == null)
                return;

            int maxHealth = bossCharacterManager.characterNetworkManager.maxHealth.Value;

            if (maxHealth <= 0)
                return;

            float healthRatio = bossCharacterManager.characterNetworkManager.currentHealth.Value / (float)maxHealth;

            if (healthRatio <= powerUpHealthThreshold)
                ActivatePowerUp();
        }

        public void EvaluatePowerUpStateFromBossNetwork()
        {
            EvaluatePowerUpState();
        }

        void ActivatePowerUp()
        {
            if (isPoweredUp)
                return;

            isPoweredUp = true;
            tormentedSoulCombatManager?.ActivatePowerUp(poweredUpDamageMultiplier);
            RefreshAuraState();
        }

        Vector3 GetLockedForward()
        {
            if (cachedTarget != null)
            {
                Vector3 direction = cachedTarget.position - transform.position;
                direction.y = 0f;

                if (direction.sqrMagnitude > 0.0001f)
                    return direction.normalized;
            }

            return transform.forward;
        }

        Vector3 GetSpreadDirection(Vector3 baseDirection, int shotIndex, int shotCount, float totalSpread)
        {
            if (shotCount <= 1 || totalSpread <= 0.01f)
                return baseDirection;

            float normalizedIndex = shotCount == 1 ? 0f : shotIndex / (float)(shotCount - 1);
            float currentAngle = Mathf.Lerp(-totalSpread, totalSpread, normalizedIndex);
            return Quaternion.AngleAxis(currentAngle, Vector3.up) * baseDirection;
        }

        void FaceTowards(Vector3 direction, float rotationSpeed)
        {
            if (direction.sqrMagnitude <= 0.0001f)
                return;

            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }

        Vector3 GetProjectileOrigin()
        {
            if (projectileSpawnPoint == null)
                TryAutoAssignProjectileSpawnPoint();

            if (projectileSpawnPoint != null)
                return projectileSpawnPoint.position;

            return transform.position + transform.forward * 1.1f + Vector3.up * 1.2f;
        }

        void TryAutoAssignProjectileSpawnPoint()
        {
            if (projectileSpawnPoint != null)
                return;

            Transform[] childTransforms = GetComponentsInChildren<Transform>(true);

            for (int i = 0; i < childTransforms.Length; i++)
            {
                Transform child = childTransforms[i];

                if (child == null)
                    continue;

                if (child.name == "Weapon Instantiate Slot")
                {
                    projectileSpawnPoint = child;
                    return;
                }
            }
        }

        void PlayAnimation(string animationName, string triggerName)
        {
            if (!string.IsNullOrWhiteSpace(triggerName) && aiCharacter != null && aiCharacter.animator != null)
            {
                aiCharacter.animator.ResetTrigger(triggerName);
                aiCharacter.animator.SetTrigger(triggerName);
            }

            if (string.IsNullOrWhiteSpace(animationName))
                return;

            aiCharacter.characterAnimatorManager.PlayTargetActionAnimation(
                animationName,
                true,
                false,
                false,
                false,
                false,
                false);
        }

        void SpawnChargeVFX(float chargeDuration)
        {
            if (activeChargeVFX != null)
                return;

            if (chargeVFXPrefab != null)
            {
                activeChargeVFX = Instantiate(chargeVFXPrefab, transform);
                activeChargeVFX.transform.localPosition = auraLocalOffset;
                activeChargeVFX.transform.localRotation = Quaternion.identity;
                return;
            }

            activeChargeVFX = TwinMoonVFXFactory.CreateChargeVFX(transform, auraLocalOffset, soulAuraColor, chargeDuration);
        }

        void DestroyChargeVFX()
        {
            if (activeChargeVFX == null)
                return;

            Destroy(activeChargeVFX);
            activeChargeVFX = null;
        }

        void RefreshAuraState()
        {
            bool shouldShowAura = isPoweredUp && aiCharacter != null && !aiCharacter.isDead.Value;

            if (shouldShowAura)
            {
                if (activeAuraVFX == null)
                    activeAuraVFX = CreateAuraVFX();

                return;
            }

            DestroyAuraVFX();
        }

        GameObject CreateAuraVFX()
        {
            if (auraVFXPrefab != null)
            {
                GameObject prefabAura = Instantiate(auraVFXPrefab, transform);
                prefabAura.transform.localPosition = auraLocalOffset;
                prefabAura.transform.localRotation = Quaternion.identity;
                return prefabAura;
            }

            GameObject root = new GameObject("DeathMoonSlash_Aura");
            root.transform.SetParent(transform);
            root.transform.localPosition = auraLocalOffset;
            root.transform.localRotation = Quaternion.identity;
            root.transform.localScale = Vector3.one;

            CreateAuraParticles(root.transform, "Soul_Aura_Outer", soulAuraColor, 0.7f, 28f, 0.16f, 0.34f, 1.4f, 0.3f);
            CreateAuraParticles(root.transform, "Soul_Aura_Inner", soulCoreColor, 0.45f, 20f, 0.12f, 0.22f, 1f, -0.35f);

            Light auraLight = root.AddComponent<Light>();
            auraLight.type = LightType.Point;
            auraLight.range = 4.2f;
            auraLight.intensity = 1.15f;
            auraLight.color = Color.Lerp(soulAuraColor, soulCoreColor, 0.35f);

            return root;
        }

        void CreateAuraParticles(
            Transform parent,
            string objectName,
            Color color,
            float radius,
            float rateOverTime,
            float startSize,
            float lifetime,
            float orbitalVelocity,
            float radialVelocity)
        {
            GameObject particleObject = new GameObject(objectName);
            particleObject.transform.SetParent(parent);
            particleObject.transform.localPosition = Vector3.zero;
            particleObject.transform.localRotation = Quaternion.identity;
            particleObject.transform.localScale = Vector3.one;

            ParticleSystem particles = particleObject.AddComponent<ParticleSystem>();
            particles.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
            ParticleSystemRenderer renderer = particleObject.GetComponent<ParticleSystemRenderer>();
            renderer.material = GetRuntimeAuraMaterial();
            renderer.renderMode = ParticleSystemRenderMode.Billboard;
            renderer.alignment = ParticleSystemRenderSpace.View;
            renderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
            renderer.receiveShadows = false;

            var main = particles.main;
            main.playOnAwake = false;
            main.loop = true;
            main.duration = 1f;
            main.startLifetime = lifetime;
            main.startSpeed = 0.2f;
            main.startSize = startSize;
            main.startColor = new Color(color.r, color.g, color.b, 0.65f);
            main.simulationSpace = ParticleSystemSimulationSpace.Local;
            main.scalingMode = ParticleSystemScalingMode.Local;

            var emission = particles.emission;
            emission.rateOverTime = rateOverTime;

            var shape = particles.shape;
            shape.shapeType = ParticleSystemShapeType.Circle;
            shape.radius = radius;
            shape.arcMode = ParticleSystemShapeMultiModeValue.Random;

            var velocity = particles.velocityOverLifetime;
            velocity.enabled = true;
            velocity.orbitalY = orbitalVelocity;
            velocity.radial = radialVelocity;
            velocity.space = ParticleSystemSimulationSpace.Local;

            var colorOverLifetime = particles.colorOverLifetime;
            colorOverLifetime.enabled = true;
            colorOverLifetime.color = BuildGradient(color, 0.85f);

            var sizeOverLifetime = particles.sizeOverLifetime;
            sizeOverLifetime.enabled = true;
            sizeOverLifetime.size = new ParticleSystem.MinMaxCurve(
                1f,
                new AnimationCurve(
                    new Keyframe(0f, 0.2f),
                    new Keyframe(0.35f, 1f),
                    new Keyframe(1f, 0f)));

            particles.Play();
        }

        Material GetRuntimeAuraMaterial()
        {
            Shader shader = Shader.Find("Universal Render Pipeline/Particles/Unlit");

            if (shader == null)
                shader = Shader.Find("Particles/Standard Unlit");

            if (shader == null)
                shader = Shader.Find("Sprites/Default");

            Material material = new Material(shader)
            {
                name = "DeathMoonSlash_RuntimeAura"
            };

            if (material.HasProperty("_Surface"))
                material.SetFloat("_Surface", 1f);

            if (material.HasProperty("_Blend"))
                material.SetFloat("_Blend", 0f);

            if (material.HasProperty("_BaseColor"))
                material.SetColor("_BaseColor", Color.white);

            if (material.HasProperty("_Color"))
                material.SetColor("_Color", Color.white);

            return material;
        }

        Gradient BuildGradient(Color color, float alpha)
        {
            Gradient gradient = new Gradient();
            gradient.SetKeys(
                new[]
                {
                    new GradientColorKey(Color.Lerp(color, Color.white, 0.2f), 0f),
                    new GradientColorKey(color, 0.55f),
                    new GradientColorKey(Color.Lerp(color, Color.black, 0.25f), 1f)
                },
                new[]
                {
                    new GradientAlphaKey(0f, 0f),
                    new GradientAlphaKey(alpha, 0.2f),
                    new GradientAlphaKey(alpha * 0.55f, 0.75f),
                    new GradientAlphaKey(0f, 1f)
                });
            return gradient;
        }

        void DestroyAuraVFX()
        {
            if (activeAuraVFX == null)
                return;

            Destroy(activeAuraVFX);
            activeAuraVFX = null;
        }

        [ClientRpc]
        void BroadcastSlashFeedbackClientRpc(Vector3 origin, Vector3 direction)
        {
            SpawnSlashCastVFX(origin, direction);
            TryPlayCameraShake(origin);
        }

        void SpawnSlashCastVFX(Vector3 origin, Vector3 direction)
        {
            if (slashCastVFXPrefab != null)
            {
                Instantiate(slashCastVFXPrefab, origin, Quaternion.LookRotation(direction));
                return;
            }

            TwinMoonVFXFactory.CreateImpactVFX(origin, Color.Lerp(soulAuraColor, soulCoreColor, 0.5f));
        }

        void TryPlayCameraShake(Vector3 impactPoint)
        {
            if (!enableCameraShake || PlayerCamera.instance == null || PlayerCamera.instance.player == null)
                return;

            float distanceToLocalPlayer = Vector3.Distance(PlayerCamera.instance.player.transform.position, impactPoint);

            if (distanceToLocalPlayer > cameraShakeRange)
                return;

            if (activeShakeRoutine != null)
                StopCoroutine(activeShakeRoutine);

            activeShakeRoutine = StartCoroutine(DoCameraShake(PlayerCamera.instance.cameraPivotTransform));
        }

        IEnumerator DoCameraShake(Transform cameraPivot)
        {
            if (cameraPivot == null)
                yield break;

            Vector3 originalLocalPosition = cameraPivot.localPosition;
            float elapsed = 0f;

            while (elapsed < cameraShakeDuration)
            {
                elapsed += Time.deltaTime;
                Vector3 offset = Random.insideUnitSphere * cameraShakeStrength;
                cameraPivot.localPosition = originalLocalPosition + offset;
                yield return null;
            }

            cameraPivot.localPosition = originalLocalPosition;
            activeShakeRoutine = null;
        }
    }
}
