using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

namespace baodeag
{
    public class DeathCycloneSkill : NetworkBehaviour
    {
        [Header("Damage")]
        [SerializeField] float damagePerTick = 12f;
        [SerializeField] float radius = 4.5f;
        [SerializeField] float duration = 3f;
        [SerializeField] float tickInterval = 0.3f;
        [SerializeField] float pullForce = 6f;
        [SerializeField] float innerSafeRadius = 1.35f;
        [SerializeField] float maxPullDistance = 4.5f;
        [SerializeField] float characterControllerPullMultiplier = 0.0125f;
        [SerializeField] float cooldown = 10f;
        [SerializeField] LayerMask playerLayer;

        [Header("Activation")]
        [SerializeField] bool allowUseWhenPoweredUp = true;
        [SerializeField] bool allowUseWhenPlayerStaysClose = true;
        [SerializeField] float closeRangeDistance = 2.75f;
        [SerializeField] float closeRangeDurationBeforeUse = 2f;
        [SerializeField] float minimumActivationDistance = 1f;
        [SerializeField] float maximumActivationDistance = 6f;
        [SerializeField, Range(0, 100)] int activationChance = 100;
        [SerializeField] float decisionLockout = 1f;

        [Header("Animation")]
        [SerializeField] Animator animator;
        [SerializeField] string animatorTrigger = "DeathCyclone";
        [SerializeField] float faceTargetDuration = 0.2f;

        [Header("VFX / Audio")]
        [SerializeField] GameObject cycloneVFX;
        [SerializeField] AudioSource audioSource;
        [SerializeField] AudioClip cycloneLoopSFX;

        [Header("Feedback")]
        [SerializeField] bool enableCameraShake = true;
        [SerializeField] float cameraShakeDuration = 0.18f;
        [SerializeField] float cameraShakeStrength = 0.12f;
        [SerializeField] float cameraShakeRange = 14f;
        [SerializeField] float poiseDamagePerTick = 8f;
        [SerializeField] bool drawDebugGizmos = true;

        AICharacterManager aiCharacter;
        AITormentedSoulCombatManager tormentedSoulCombatManager;
        Rigidbody attachedRigidbody;

        Coroutine activeSkillRoutine;
        Coroutine activeShakeRoutine;
        GameObject activeCycloneVFX;
        float nextReadyTime;
        float nextDecisionTime;
        float proximityTimer;

        public bool TryActivateSkill()
        {
            if (!IsOwner || aiCharacter == null || tormentedSoulCombatManager == null)
                return false;

            if (activeSkillRoutine != null || aiCharacter.isDead.Value || aiCharacter.isPerformingAction)
                return false;

            if (Time.time < nextReadyTime || Time.time < nextDecisionTime)
                return false;

            CharacterManager target = tormentedSoulCombatManager.currentTarget;

            if (target == null || target.isDead.Value)
            {
                proximityTimer = 0f;
                return false;
            }

            float distanceToTarget = Vector3.Distance(transform.position, target.transform.position);

            if (distanceToTarget < minimumActivationDistance || distanceToTarget > maximumActivationDistance)
                return false;

            bool poweredUpReady = allowUseWhenPoweredUp && tormentedSoulCombatManager.IsPoweredUp;
            bool closeRangeReady = allowUseWhenPlayerStaysClose && proximityTimer >= closeRangeDurationBeforeUse;

            if (!poweredUpReady && !closeRangeReady)
                return false;

            nextDecisionTime = Time.time + decisionLockout;

            if (Random.Range(0, 100) >= activationChance)
                return false;

            nextReadyTime = Time.time + cooldown;
            proximityTimer = 0f;
            activeSkillRoutine = StartCoroutine(PerformDeathCyclone(target.transform));
            return true;
        }

        void Awake()
        {
            aiCharacter = GetComponent<AICharacterManager>();
            tormentedSoulCombatManager = GetComponent<AITormentedSoulCombatManager>();
            attachedRigidbody = GetComponent<Rigidbody>();

            if (animator == null)
                animator = GetComponent<Animator>();
        }

        void Update()
        {
            UpdateProximityTimer();
        }

        void UpdateProximityTimer()
        {
            if (tormentedSoulCombatManager == null)
                return;

            CharacterManager target = tormentedSoulCombatManager.currentTarget;

            if (target == null || target.isDead.Value)
            {
                proximityTimer = 0f;
                return;
            }

            float distanceToTarget = Vector3.Distance(transform.position, target.transform.position);

            if (distanceToTarget <= closeRangeDistance)
            {
                proximityTimer += Time.deltaTime;
                return;
            }

            proximityTimer = 0f;
        }

        IEnumerator PerformDeathCyclone(Transform target)
        {
            SetSkillState(true);
            StopNavigation();
            HaltMomentum();

            yield return StartCoroutine(FaceTargetBeforeSpin(target));

            if (animator != null && !string.IsNullOrWhiteSpace(animatorTrigger))
                animator.SetTrigger(animatorTrigger);

            SpawnCycloneVFX();
            PlayCycloneAudio();
            TryPlayCameraShake(transform.position);

            float elapsed = 0f;
            float tickTimer = 0f;

            while (elapsed < duration)
            {
                elapsed += Time.deltaTime;
                tickTimer += Time.deltaTime;
                HaltMomentum();

                if (tickTimer >= tickInterval)
                {
                    tickTimer = 0f;
                    ApplyCycloneTick();
                }

                yield return null;
            }

            StopCycloneAudio();
            DestroyCycloneVFX();
            ResetAfterSkill();
            activeSkillRoutine = null;
        }

        IEnumerator FaceTargetBeforeSpin(Transform target)
        {
            if (target == null)
                yield break;

            float elapsed = 0f;

            while (elapsed < faceTargetDuration)
            {
                elapsed += Time.deltaTime;

                Vector3 direction = target.position - transform.position;
                direction.y = 0f;

                if (direction.sqrMagnitude > 0.0001f)
                {
                    Quaternion targetRotation = Quaternion.LookRotation(direction.normalized);
                    transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 20f * Time.deltaTime);
                }

                yield return null;
            }
        }

        void ApplyCycloneTick()
        {
            LayerMask overlapMask = playerLayer.value != 0
                ? playerLayer
                : WorldUtilityManager.Instance.GetCharacterLayers();

            Collider[] colliders = Physics.OverlapSphere(transform.position, radius, overlapMask, QueryTriggerInteraction.Collide);
            HashSet<ulong> processedTargets = new HashSet<ulong>();

            for (int i = 0; i < colliders.Length; i++)
            {
                CharacterManager damageTarget = colliders[i].GetComponentInParent<CharacterManager>();

                if (damageTarget == null || damageTarget == aiCharacter || damageTarget.isDead.Value)
                    continue;

                if (processedTargets.Contains(damageTarget.NetworkObjectId))
                    continue;

                if (!WorldUtilityManager.Instance.CanIDamageThisTarget(aiCharacter.characterGroup, damageTarget.characterGroup))
                    continue;

                processedTargets.Add(damageTarget.NetworkObjectId);
                PullTargetTowardsBoss(damageTarget);
                ApplyDamageToTarget(damageTarget, colliders[i]);
            }
        }

        void ApplyDamageToTarget(CharacterManager damageTarget, Collider hitCollider)
        {
            if (damageTarget == null)
                return;

            Vector3 contactPoint = hitCollider != null
                ? hitCollider.ClosestPoint(transform.position)
                : damageTarget.transform.position;

            float angleHitFrom = Vector3.SignedAngle(transform.forward, damageTarget.transform.forward, Vector3.up);

            damageTarget.characterNetworkManager.NotifyTheServerOfCharacterDamageServerRpc(
                damageTarget.NetworkObjectId,
                NetworkObjectId,
                damagePerTick,
                0f,
                0f,
                0f,
                0f,
                poiseDamagePerTick,
                angleHitFrom,
                contactPoint.x,
                contactPoint.y,
                contactPoint.z);
        }

        void PullTargetTowardsBoss(CharacterManager damageTarget)
        {
            if (damageTarget == null)
                return;

            Vector3 pullDirection = transform.position - damageTarget.transform.position;
            pullDirection.y = 0f;
            float distanceToBoss = pullDirection.magnitude;

            if (distanceToBoss <= innerSafeRadius || pullDirection.sqrMagnitude <= 0.0001f)
                return;

            pullDirection.Normalize();
            float effectiveMaxDistance = Mathf.Max(innerSafeRadius + 0.01f, maxPullDistance);
            float pullStrength = Mathf.InverseLerp(innerSafeRadius, effectiveMaxDistance, distanceToBoss) * pullForce;

            if (pullStrength <= 0.001f)
                return;

            Rigidbody targetRigidbody = damageTarget.GetComponent<Rigidbody>();

            if (targetRigidbody != null && !targetRigidbody.isKinematic)
            {
                targetRigidbody.AddForce(pullDirection * pullStrength, ForceMode.Acceleration);
            }

            if (IsServer)
                ApplyPullToOwnerClientRpc(damageTarget.NetworkObjectId, transform.position, pullStrength);
        }

        [ClientRpc]
        void ApplyPullToOwnerClientRpc(ulong targetNetworkObjectId, Vector3 bossPosition, float force)
        {
            if (!NetworkManager.Singleton.SpawnManager.SpawnedObjects.ContainsKey(targetNetworkObjectId))
                return;

            CharacterManager damageTarget = NetworkManager.Singleton.SpawnManager.SpawnedObjects[targetNetworkObjectId].GetComponent<CharacterManager>();

            if (damageTarget == null || !damageTarget.IsOwner)
                return;

            Vector3 pullDirection = bossPosition - damageTarget.transform.position;
            pullDirection.y = 0f;

            if (pullDirection.sqrMagnitude <= 0.0001f)
                return;

            pullDirection.Normalize();

            Rigidbody targetRigidbody = damageTarget.GetComponent<Rigidbody>();

            if (targetRigidbody != null && !targetRigidbody.isKinematic)
            {
                targetRigidbody.AddForce(pullDirection * force, ForceMode.Acceleration);
                return;
            }

            if (damageTarget.characterController != null && damageTarget.characterController.enabled)
            {
                // CharacterController owners need local movement input, so we use a very light nudge.
                damageTarget.characterController.Move(pullDirection * force * characterControllerPullMultiplier);
            }
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

        void HaltMomentum()
        {
            if (attachedRigidbody == null || attachedRigidbody.isKinematic)
                return;

            attachedRigidbody.linearVelocity = Vector3.zero;
            attachedRigidbody.angularVelocity = Vector3.zero;
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
            SetSkillState(false);

            if (aiCharacter != null && aiCharacter.navMeshAgent != null && !aiCharacter.navMeshAgent.enabled)
                aiCharacter.navMeshAgent.enabled = true;
        }

        void SpawnCycloneVFX()
        {
            if (cycloneVFX == null || activeCycloneVFX != null)
                return;

            activeCycloneVFX = Instantiate(cycloneVFX, transform);
            activeCycloneVFX.transform.localPosition = Vector3.zero;
            activeCycloneVFX.transform.localRotation = Quaternion.identity;
        }

        void DestroyCycloneVFX()
        {
            if (activeCycloneVFX == null)
                return;

            Destroy(activeCycloneVFX);
            activeCycloneVFX = null;
        }

        void PlayCycloneAudio()
        {
            if (audioSource == null || cycloneLoopSFX == null)
                return;

            audioSource.clip = cycloneLoopSFX;
            audioSource.loop = true;
            audioSource.Play();
        }

        void StopCycloneAudio()
        {
            if (audioSource == null)
                return;

            if (audioSource.isPlaying)
                audioSource.Stop();

            audioSource.loop = false;
            audioSource.clip = null;
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
                cameraPivot.localPosition = originalLocalPosition + Random.insideUnitSphere * cameraShakeStrength;
                yield return null;
            }

            cameraPivot.localPosition = originalLocalPosition;
            activeShakeRoutine = null;
        }

        void OnDrawGizmosSelected()
        {
            if (!drawDebugGizmos)
                return;

            Gizmos.color = new Color(0.55f, 0.1f, 0.75f, 0.35f);
            Gizmos.DrawSphere(transform.position, radius);
            Gizmos.color = new Color(0.75f, 0.3f, 1f, 0.85f);
            Gizmos.DrawWireSphere(transform.position, radius);
        }
    }
}
