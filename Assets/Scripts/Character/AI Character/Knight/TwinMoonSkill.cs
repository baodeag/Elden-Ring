using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

namespace baodeag
{
    public class TwinMoonSkill : NetworkBehaviour
    {
        [Header("Activation")]
        [SerializeField] bool onlyAvailableWhenPoweredUp = true;
        [SerializeField] bool activatePowerUpAtHalfHealth = true;
        [SerializeField, Range(0f, 1f)] float powerUpHealthThreshold = 0.5f;
        [SerializeField] float skillCooldown = 16f;
        [SerializeField] float minimumActivationDistance = 1.75f;
        [SerializeField] float maximumActivationDistance = 8f;
        [SerializeField, Range(0, 100)] int activationChance = 100;
        [SerializeField] float skillDecisionLockout = 1.25f;

        [Header("Animations")]
        [SerializeField] string jumpAnimation = "Main_Jump_01";
        [SerializeField] string hoverAnimation = "Main_Jump_01";
        [SerializeField] string slamAnimation = "Main_Heavy_Jump_Attack_01";

        [Header("Phase 1 - Jump")]
        [SerializeField] float jumpDuration = 0.55f;
        [SerializeField] float jumpHeight = 4.5f;
        [SerializeField] float forwardTravelDuringJump = 1.35f;

        [Header("Phase 2 - Hover")]
        [SerializeField] float hoverDuration = 0.75f;
        [SerializeField] GameObject chargeVFXPrefab;
        [SerializeField] Vector3 chargeVFXLocalOffset = new Vector3(0f, 1.25f, 0f);

        [Header("Phase 3 - Slam")]
        [SerializeField] float slamSpeed = 18f;
        [SerializeField] GameObject impactVFXPrefab;
        [SerializeField] float postImpactRecovery = 0.4f;

        [Header("Phase 4 - Twin Moons")]
        [SerializeField] GameObject shockwaveVFXPrefab;
        [SerializeField] float shockwaveDuration = 0.45f;
        [SerializeField] float secondWaveDelay = 0.4f;
        [SerializeField] float firstWaveRadius = 4f;
        [SerializeField] float secondWaveRadius = 6.25f;
        [SerializeField] float firstWaveDamage = 32f;
        [SerializeField] float secondWaveDamage = 44f;
        [SerializeField] float firstWavePoiseDamage = 20f;
        [SerializeField] float secondWavePoiseDamage = 28f;
        [SerializeField] float shockwaveHitThickness = 1.1f;
        [SerializeField] float shockwaveVerticalHitTolerance = 1.25f;
        [SerializeField] float firstWaveKnockbackForce = 8f;
        [SerializeField] float secondWaveKnockbackForce = 12f;
        [SerializeField] float upwardKnockbackForce = 2.25f;

        [Header("Power Up")]
        [SerializeField] Color powerUpColor = new Color(0.15f, 0.9f, 1f, 1f);
        [SerializeField] Color powerUpEmissionColor = new Color(0.2f, 2.8f, 4f, 1f);
        [SerializeField] float poweredUpDamageMultiplier = 1.25f;
        [SerializeField] float poweredUpSkillDamageMultiplier = 1.2f;
        [SerializeField] bool recolorSwordMesh = true;
        [SerializeField] bool recolorSwordTrails = true;

        [Header("Impact Feedback")]
        [SerializeField] bool enableCameraShake = true;
        [SerializeField] float cameraShakeDuration = 0.2f;
        [SerializeField] float cameraShakeStrength = 0.18f;
        [SerializeField] float cameraShakeRange = 16f;
        [SerializeField] bool enableSlowMotion = true;
        [SerializeField] float slowMotionScale = 0.35f;
        [SerializeField] float slowMotionDuration = 0.12f;

        [Header("Debug")]
        [SerializeField] bool drawDebugGizmos = true;

        AICharacterManager aiCharacter;
        AIKnightCombatManager knightCombatManager;
        CharacterController characterController;
        Rigidbody attachedRigidbody;
        Transform swordVisualRoot;
        Transform cachedTarget;
        Renderer[] swordRenderers;
        TrailRenderer[] swordTrails;
        ParticleSystem[] swordTrailParticles;
        readonly List<Material[]> originalSwordMaterials = new List<Material[]>();

        Coroutine activeSkillRoutine;
        Coroutine activeShakeRoutine;
        GameObject activeChargeVFX;
        float nextSkillReadyTime;
        float nextSkillDecisionTime;
        bool isPoweredUp;
        bool hasSubscribedToHealth;
        bool initializedSwordVisuals;
        bool slowMotionRoutineRunning;

        public bool IsPoweredUp => isPoweredUp;

        void Awake()
        {
            aiCharacter = GetComponent<AICharacterManager>();
            knightCombatManager = GetComponent<AIKnightCombatManager>();
            characterController = GetComponent<CharacterController>();
            attachedRigidbody = GetComponent<Rigidbody>();
            CacheSwordVisualReferences();
        }

        void OnEnable()
        {
            TrySubscribeToHealth();
        }

        void OnDisable()
        {
            UnsubscribeFromHealth();
        }

        public override void OnNetworkSpawn()
        {
            base.OnNetworkSpawn();
            TrySubscribeToHealth();
            EvaluatePowerUpState();
        }

        public override void OnNetworkDespawn()
        {
            base.OnNetworkDespawn();
            UnsubscribeFromHealth();
        }

        public bool TryActivateSkill()
        {
            if (!IsOwner || aiCharacter == null || knightCombatManager == null)
                return false;

            if (activeSkillRoutine != null || aiCharacter.isDead.Value || aiCharacter.isPerformingAction)
                return false;

            if (onlyAvailableWhenPoweredUp && !isPoweredUp)
                return false;

            if (Time.time < nextSkillReadyTime || Time.time < nextSkillDecisionTime)
                return false;

            CharacterManager target = knightCombatManager.currentTarget;

            if (target == null || target.isDead.Value)
                return false;

            float distanceToTarget = Vector3.Distance(transform.position, target.transform.position);

            if (distanceToTarget < minimumActivationDistance || distanceToTarget > maximumActivationDistance)
                return false;

            nextSkillDecisionTime = Time.time + skillDecisionLockout;

            if (Random.Range(0, 100) >= activationChance)
                return false;

            nextSkillReadyTime = Time.time + skillCooldown;
            cachedTarget = target.transform;
            activeSkillRoutine = StartCoroutine(PerformTwinMoonSkill());
            return true;
        }

        void TrySubscribeToHealth()
        {
            if (hasSubscribedToHealth || aiCharacter == null || aiCharacter.characterNetworkManager == null)
                return;

            aiCharacter.characterNetworkManager.currentHealth.OnValueChanged += OnHealthChanged;
            hasSubscribedToHealth = true;
        }

        void UnsubscribeFromHealth()
        {
            if (!hasSubscribedToHealth || aiCharacter == null || aiCharacter.characterNetworkManager == null)
                return;

            aiCharacter.characterNetworkManager.currentHealth.OnValueChanged -= OnHealthChanged;
            hasSubscribedToHealth = false;
        }

        void OnHealthChanged(int oldValue, int newValue)
        {
            EvaluatePowerUpState();
        }

        void EvaluatePowerUpState()
        {
            if (!activatePowerUpAtHalfHealth || isPoweredUp || aiCharacter == null || aiCharacter.characterNetworkManager == null)
                return;

            int maxHealth = aiCharacter.characterNetworkManager.maxHealth.Value;

            if (maxHealth <= 0)
                return;

            float healthRatio = aiCharacter.characterNetworkManager.currentHealth.Value / (float)maxHealth;

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
            knightCombatManager?.ApplyPowerUpBuff(poweredUpDamageMultiplier);
            ApplySwordPowerUpVisuals();
        }

        IEnumerator PerformTwinMoonSkill()
        {
            SetSkillState(true);

            if (aiCharacter.navMeshAgent != null && aiCharacter.navMeshAgent.enabled)
            {
                aiCharacter.navMeshAgent.isStopped = true;
                aiCharacter.navMeshAgent.ResetPath();
                aiCharacter.navMeshAgent.enabled = false;
            }

            if (attachedRigidbody != null)
            {
                attachedRigidbody.linearVelocity = Vector3.zero;
                attachedRigidbody.angularVelocity = Vector3.zero;
            }

            yield return StartCoroutine(PhaseJump());
            yield return StartCoroutine(PhaseHover());
            yield return StartCoroutine(PhaseSlam());
            yield return StartCoroutine(PhaseTwinShockwaves());

            yield return new WaitForSeconds(postImpactRecovery);

            if (aiCharacter != null && !aiCharacter.isDead.Value)
                ResetAfterSkill();

            activeSkillRoutine = null;
        }

        IEnumerator PhaseJump()
        {
            PlayAnimationIfAssigned(jumpAnimation);

            float elapsed = 0f;
            Vector3 startPosition = transform.position;
            Vector3 targetForward = GetSkillForward();

            while (elapsed < jumpDuration)
            {
                elapsed += Time.deltaTime;
                float normalizedTime = Mathf.Clamp01(elapsed / jumpDuration);
                float heightOffset = Mathf.Sin(normalizedTime * Mathf.PI * 0.5f) * jumpHeight;
                Vector3 desiredPosition = startPosition
                    + (targetForward * forwardTravelDuringJump * normalizedTime)
                    + Vector3.up * heightOffset;

                MoveCharacter(desiredPosition - transform.position);
                FaceTowards(targetForward);
                yield return null;
            }
        }

        IEnumerator PhaseHover()
        {
            PlayAnimationIfAssigned(hoverAnimation);
            SpawnChargeVFX();

            float elapsed = 0f;
            Vector3 hoverPosition = transform.position;
            Vector3 targetForward = GetSkillForward();

            while (elapsed < hoverDuration)
            {
                elapsed += Time.deltaTime;
                FaceTowards(targetForward);
                MoveCharacter(hoverPosition - transform.position);
                yield return null;
            }

            DestroyChargeVFX();
        }

        IEnumerator PhaseSlam()
        {
            PlayAnimationIfAssigned(slamAnimation);
            Vector3 targetForward = GetSkillForward();

            while (!IsGroundedForImpact())
            {
                FaceTowards(targetForward);
                MoveCharacter(Vector3.down * slamSpeed * Time.deltaTime);
                yield return null;
            }

            if (IsServer)
                BroadcastImpactFeedbackClientRpc(transform.position);
        }

        IEnumerator PhaseTwinShockwaves()
        {
            yield return StartCoroutine(ExpandShockwave(1, firstWaveRadius, firstWaveDamage, firstWavePoiseDamage, firstWaveKnockbackForce, 0f));
            yield return new WaitForSeconds(secondWaveDelay);
            yield return StartCoroutine(ExpandShockwave(2, secondWaveRadius, secondWaveDamage, secondWavePoiseDamage, secondWaveKnockbackForce, secondWaveDelay));
        }

        IEnumerator ExpandShockwave(int waveIndex, float maxRadius, float damage, float poiseDamage, float knockbackForce, float visualDelay)
        {
            GameObject waveVFX = SpawnShockwaveVFX(maxRadius);
            float elapsed = 0f;
            float scaledDamage = damage * GetCurrentSkillDamageMultiplier();
            float scaledPoiseDamage = poiseDamage * GetCurrentSkillDamageMultiplier();

            if (IsOwner)
            {
                SpawnShockwaveHitbox(maxRadius, scaledDamage, scaledPoiseDamage, knockbackForce);
            }

            while (elapsed < shockwaveDuration)
            {
                elapsed += Time.deltaTime;
                float currentRadius = Mathf.Lerp(0f, maxRadius, Mathf.Clamp01(elapsed / shockwaveDuration));

                if (waveVFX != null)
                {
                    waveVFX.transform.position = transform.position + Vector3.up * 0.05f;
                    float scale = Mathf.Max(0.1f, currentRadius * 2f);
                    waveVFX.transform.localScale = new Vector3(scale, 1f, scale);
                }
                yield return null;
            }

            if (waveVFX != null)
                Destroy(waveVFX, 1f);
        }

        void SpawnShockwaveHitbox(float radius, float damage, float poiseDamage, float knockbackForce)
        {
            GameObject hitboxObject = new GameObject("TwinMoonShockwaveHitbox");
            hitboxObject.transform.position = transform.position + Vector3.up * 0.05f;
            hitboxObject.layer = gameObject.layer;

            TwinMoonShockwaveHitbox hitbox = hitboxObject.AddComponent<TwinMoonShockwaveHitbox>();
            hitbox.Initialize(
                this,
                aiCharacter,
                radius,
                shockwaveDuration,
                damage,
                poiseDamage,
                knockbackForce,
                shockwaveHitThickness,
                shockwaveVerticalHitTolerance);
        }

        public void ApplyShockwaveHit(CharacterManager target, float damage, float poiseDamage, float knockbackForce)
        {
            ApplyShockwaveFrostBuildUp(target);
            ApplyDamageToTarget(target, damage, poiseDamage);
            ApplyKnockback(target, knockbackForce);
        }

        void ApplyShockwaveFrostBuildUp(CharacterManager target)
        {
            if (!isPoweredUp || target == null || !target.IsOwner)
                return;

            if (target is not PlayerManager player || player.playerEffectsManager == null)
                return;

            if (knightCombatManager == null)
                return;

            player.playerEffectsManager.ApplyFrostBuildUpFromHit(knightCombatManager.PoweredUpFrostBuildUpAmount);
        }

        void ApplyDamageToTarget(CharacterManager target, float damage, float poiseDamage)
        {
            if (target == null || !target.IsOwner)
                return;

            Vector3 directionToTarget = (target.transform.position - transform.position).normalized;
            float angleHitFrom = Vector3.SignedAngle(transform.forward, target.transform.forward, Vector3.up);
            Vector3 contactPoint = target.characterCombatManager != null && target.characterCombatManager.lockOnTransform != null
                ? target.characterCombatManager.lockOnTransform.position
                : target.transform.position;

            target.characterNetworkManager.NotifyTheServerOfCharacterDamageServerRpc(
                target.NetworkObjectId,
                NetworkObjectId,
                damage,
                0f,
                0f,
                0f,
                0f,
                poiseDamage,
                angleHitFrom,
                contactPoint.x,
                contactPoint.y,
                contactPoint.z);
        }

        void ApplyKnockback(CharacterManager target, float knockbackForce)
        {
            if (target == null || !target.IsOwner)
                return;

            Vector3 direction = (target.transform.position - transform.position).normalized;
            direction.y = 0f;

            if (direction.sqrMagnitude <= 0.0001f)
                direction = transform.forward;

            Rigidbody targetRigidbody = target.GetComponent<Rigidbody>();

            if (targetRigidbody != null && !targetRigidbody.isKinematic)
            {
                targetRigidbody.AddForce(direction * knockbackForce + Vector3.up * upwardKnockbackForce, ForceMode.VelocityChange);
                return;
            }

            if (target.characterController != null && target.characterController.enabled)
            {
                Vector3 knockbackMotion = direction * knockbackForce * 0.12f;
                knockbackMotion.y = upwardKnockbackForce * 0.05f;
                target.characterController.Move(knockbackMotion);
            }
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
                aiCharacter.characterNetworkManager.isJumping.Value = skillIsActive;
                aiCharacter.characterNetworkManager.isInvulnerable.Value = skillIsActive;
            }
        }

        void ResetAfterSkill()
        {
            SetSkillState(false);
            aiCharacter.characterCombatManager.DisableCanDoCombo();
            aiCharacter.characterCombatManager.DisableCanDoRollingAttack();
            aiCharacter.characterCombatManager.DisableCanDoBackstepAttack();

            if (aiCharacter.navMeshAgent != null && !aiCharacter.navMeshAgent.enabled)
                aiCharacter.navMeshAgent.enabled = true;
        }

        void PlayAnimationIfAssigned(string animationName)
        {
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

        void MoveCharacter(Vector3 worldDelta)
        {
            if (characterController != null && characterController.enabled)
            {
                characterController.Move(worldDelta);
                return;
            }

            if (attachedRigidbody != null && !attachedRigidbody.isKinematic)
            {
                attachedRigidbody.MovePosition(attachedRigidbody.position + worldDelta);
                return;
            }

            transform.position += worldDelta;
        }

        bool IsGroundedForImpact()
        {
            if (aiCharacter.characterLocomotionManager != null && aiCharacter.characterLocomotionManager.isGrounded)
                return true;

            Vector3 rayOrigin = transform.position + Vector3.up * 0.2f;
            return Physics.Raycast(rayOrigin, Vector3.down, 0.45f, WorldUtilityManager.Instance.GetEnviroLayers(), QueryTriggerInteraction.Ignore);
        }

        Vector3 GetSkillForward()
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

        void FaceTowards(Vector3 direction)
        {
            if (direction.sqrMagnitude <= 0.0001f)
                return;

            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 15f * Time.deltaTime);
        }

        void SpawnChargeVFX()
        {
            if (activeChargeVFX != null)
                return;

            if (chargeVFXPrefab != null)
            {
                activeChargeVFX = Instantiate(chargeVFXPrefab, transform);
                activeChargeVFX.transform.localPosition = chargeVFXLocalOffset;
                activeChargeVFX.transform.localRotation = Quaternion.identity;
                return;
            }

            activeChargeVFX = TwinMoonVFXFactory.CreateChargeVFX(transform, chargeVFXLocalOffset, powerUpColor, hoverDuration);
        }

        void DestroyChargeVFX()
        {
            if (activeChargeVFX == null)
                return;

            Destroy(activeChargeVFX);
            activeChargeVFX = null;
        }

        void SpawnImpactVFX(Vector3 impactPoint)
        {
            if (impactVFXPrefab != null)
            {
                Instantiate(impactVFXPrefab, impactPoint, Quaternion.identity);
                return;
            }

            TwinMoonVFXFactory.CreateImpactVFX(impactPoint, powerUpColor);
        }

        GameObject SpawnShockwaveVFX(float radius)
        {
            if (shockwaveVFXPrefab == null)
                return TwinMoonVFXFactory.CreateShockwaveVFX(transform.position + Vector3.up * 0.05f, radius, shockwaveDuration, powerUpColor);

            return Instantiate(shockwaveVFXPrefab, transform.position + Vector3.up * 0.05f, Quaternion.identity);
        }

        [ClientRpc]
        void BroadcastImpactFeedbackClientRpc(Vector3 impactPoint)
        {
            SpawnImpactVFX(impactPoint);

            StartCoroutine(PlayShockwaveVisualsOnly());
            TryPlayCameraShake(impactPoint);
            TryPlaySlowMotion(impactPoint);
        }

        IEnumerator PlayShockwaveVisualsOnly()
        {
            yield return StartCoroutine(PlayShockwaveVisual(firstWaveRadius));
            yield return new WaitForSeconds(secondWaveDelay);
            yield return StartCoroutine(PlayShockwaveVisual(secondWaveRadius));
        }

        IEnumerator PlayShockwaveVisual(float radius)
        {
            GameObject waveVFX = SpawnShockwaveVFX(radius);
            float elapsed = 0f;

            while (elapsed < shockwaveDuration)
            {
                elapsed += Time.deltaTime;

                if (waveVFX != null)
                {
                    float currentRadius = Mathf.Lerp(0f, radius, Mathf.Clamp01(elapsed / shockwaveDuration));
                    float scale = Mathf.Max(0.1f, currentRadius * 2f);
                    waveVFX.transform.position = transform.position + Vector3.up * 0.05f;
                    waveVFX.transform.localScale = new Vector3(scale, 1f, scale);
                }

                yield return null;
            }

            if (waveVFX != null)
                Destroy(waveVFX, 1f);
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
                elapsed += Time.unscaledDeltaTime;
                Vector3 offset = Random.insideUnitSphere * cameraShakeStrength;
                offset.z = 0f;
                cameraPivot.localPosition = originalLocalPosition + offset;
                yield return null;
            }

            cameraPivot.localPosition = originalLocalPosition;
            activeShakeRoutine = null;
        }

        void TryPlaySlowMotion(Vector3 impactPoint)
        {
            if (!enableSlowMotion || slowMotionRoutineRunning || PlayerCamera.instance == null || PlayerCamera.instance.player == null)
                return;

            float distanceToLocalPlayer = Vector3.Distance(PlayerCamera.instance.player.transform.position, impactPoint);

            if (distanceToLocalPlayer > cameraShakeRange)
                return;

            StartCoroutine(DoSlowMotion());
        }

        IEnumerator DoSlowMotion()
        {
            slowMotionRoutineRunning = true;
            float originalTimeScale = Time.timeScale;
            float originalFixedDeltaTime = Time.fixedDeltaTime;

            Time.timeScale = slowMotionScale;
            Time.fixedDeltaTime = 0.02f * slowMotionScale;

            yield return new WaitForSecondsRealtime(slowMotionDuration);

            Time.timeScale = originalTimeScale;
            Time.fixedDeltaTime = originalFixedDeltaTime;
            slowMotionRoutineRunning = false;
        }

        void CacheSwordVisualReferences()
        {
            if (initializedSwordVisuals)
                return;

            initializedSwordVisuals = true;

            if (knightCombatManager == null || knightCombatManager.SwordDamageCollider == null)
                return;

            swordVisualRoot = knightCombatManager.SwordDamageCollider.transform;
            swordRenderers = swordVisualRoot.GetComponentsInChildren<Renderer>(true);
            swordTrails = swordVisualRoot.GetComponentsInChildren<TrailRenderer>(true);
            swordTrailParticles = swordVisualRoot.GetComponentsInChildren<ParticleSystem>(true);
        }

        void ApplySwordPowerUpVisuals()
        {
            CacheSwordVisualReferences();

            if (recolorSwordMesh && swordRenderers != null)
            {
                for (int i = 0; i < swordRenderers.Length; i++)
                {
                    Renderer renderer = swordRenderers[i];

                    if (renderer == null || renderer is ParticleSystemRenderer)
                        continue;

                    Material[] runtimeMaterials = renderer.materials;
                    originalSwordMaterials.Add(runtimeMaterials);

                    for (int j = 0; j < runtimeMaterials.Length; j++)
                    {
                        Material material = runtimeMaterials[j];

                        if (material == null)
                            continue;

                        if (material.HasProperty("_BaseColor"))
                            material.SetColor("_BaseColor", powerUpColor);

                        if (material.HasProperty("_Color"))
                            material.SetColor("_Color", powerUpColor);

                        if (material.HasProperty("_EmissionColor"))
                        {
                            material.EnableKeyword("_EMISSION");
                            material.SetColor("_EmissionColor", powerUpEmissionColor);
                        }
                    }

                    renderer.materials = runtimeMaterials;
                }
            }

            if (recolorSwordTrails && swordTrails != null)
            {
                for (int i = 0; i < swordTrails.Length; i++)
                {
                    if (swordTrails[i] == null)
                        continue;

                    swordTrails[i].startColor = powerUpColor;
                    swordTrails[i].endColor = new Color(powerUpColor.r, powerUpColor.g, powerUpColor.b, 0f);
                    swordTrails[i].emitting = true;
                }
            }

            if (recolorSwordTrails && swordTrailParticles != null)
            {
                for (int i = 0; i < swordTrailParticles.Length; i++)
                {
                    if (swordTrailParticles[i] == null)
                        continue;

                    var main = swordTrailParticles[i].main;
                    main.startColor = powerUpColor;
                    swordTrailParticles[i].Play();
                }
            }
        }

        float GetCurrentSkillDamageMultiplier()
        {
            return isPoweredUp ? poweredUpSkillDamageMultiplier : 1f;
        }

        void OnDrawGizmosSelected()
        {
            if (!drawDebugGizmos)
                return;

            Gizmos.color = new Color(0.2f, 0.8f, 1f, 0.15f);
            Gizmos.DrawWireSphere(transform.position, firstWaveRadius);

            Gizmos.color = new Color(0.45f, 0.55f, 1f, 0.15f);
            Gizmos.DrawWireSphere(transform.position, secondWaveRadius);

            Gizmos.color = new Color(0.2f, 1f, 0.6f, 0.15f);
            Gizmos.DrawWireSphere(transform.position, maximumActivationDistance);
        }
    }
}
