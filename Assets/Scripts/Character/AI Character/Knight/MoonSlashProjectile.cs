using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

namespace baodeag
{
    public class MoonSlashProjectile : NetworkBehaviour
    {
        [Header("Projectile")]
        [SerializeField] float speed = 20f;
        [SerializeField] float lifetime = 3f;
        [SerializeField] bool destroyOnHit = true;

        [Header("Damage")]
        [SerializeField] float damage = 38f;
        [SerializeField] float poiseDamage = 16f;
        [SerializeField] LayerMask targetLayers;

        [Header("Feedback")]
        [SerializeField] GameObject impactVFXPrefab;
        [SerializeField] GameObject trailVFXPrefab;
        [SerializeField] Color soulAuraColor = new Color(0.48f, 0.15f, 0.78f, 1f);
        [SerializeField] Color soulCoreColor = new Color(0.2f, 0.36f, 0.78f, 1f);

        Rigidbody cachedRigidbody;
        Collider cachedCollider;
        CharacterManager sourceCharacter;
        GameObject activeTrailVFX;
        readonly HashSet<ulong> hitTargetIds = new HashSet<ulong>();

        Vector3 moveDirection = Vector3.forward;
        bool hasInitialized;
        bool hasImpacted;

        void Awake()
        {
            cachedRigidbody = GetComponent<Rigidbody>();
            cachedCollider = GetComponent<Collider>();

            if (cachedCollider != null)
                cachedCollider.isTrigger = true;
        }

        void OnEnable()
        {
            hasImpacted = false;
            hitTargetIds.Clear();
            Destroy(gameObject, lifetime);
        }

        void Update()
        {
            if (!hasInitialized || hasImpacted)
                return;

            if (cachedRigidbody == null || cachedRigidbody.isKinematic)
            {
                transform.position += moveDirection * speed * Time.deltaTime;
            }
        }

        void FixedUpdate()
        {
            if (!hasInitialized || hasImpacted || cachedRigidbody == null || cachedRigidbody.isKinematic)
                return;

            cachedRigidbody.linearVelocity = moveDirection * speed;
            cachedRigidbody.rotation = Quaternion.LookRotation(moveDirection);
        }

        public void Initialize(
            CharacterManager owner,
            Vector3 direction,
            float damageAmount,
            float projectileSpeed,
            float lifeTime,
            float poiseDamageAmount,
            LayerMask validTargetLayers,
            GameObject impactVFX,
            Color auraColor,
            Color coreColor)
        {
            sourceCharacter = owner;
            moveDirection = direction.sqrMagnitude > 0.0001f ? direction.normalized : transform.forward;
            damage = damageAmount;
            speed = projectileSpeed;
            lifetime = lifeTime;
            poiseDamage = poiseDamageAmount;
            targetLayers = validTargetLayers;
            impactVFXPrefab = impactVFX;
            soulAuraColor = auraColor;
            soulCoreColor = coreColor;
            hasInitialized = true;

            transform.rotation = Quaternion.LookRotation(moveDirection);

            if (cachedRigidbody != null)
            {
                cachedRigidbody.useGravity = false;
                cachedRigidbody.linearVelocity = moveDirection * speed;
                cachedRigidbody.angularVelocity = Vector3.zero;
            }

            SpawnTrailVFX();
        }

        void OnTriggerEnter(Collider other)
        {
            if (!hasInitialized || hasImpacted)
                return;

            if (sourceCharacter == null)
                return;

            if (other == null)
                return;

            if (other.transform.root == sourceCharacter.transform.root)
                return;

            CharacterManager damageTarget = other.GetComponentInParent<CharacterManager>();

            if (damageTarget != null)
            {
                if (!WorldUtilityManager.Instance.CanIDamageThisTarget(sourceCharacter.characterGroup, damageTarget.characterGroup))
                    return;

                if (hitTargetIds.Contains(damageTarget.NetworkObjectId))
                    return;

                hitTargetIds.Add(damageTarget.NetworkObjectId);
                ApplyDamage(damageTarget, other);
                SpawnImpactVFX(other.ClosestPoint(transform.position));

                if (destroyOnHit)
                    DestroyProjectile();

                return;
            }

            if (((1 << other.gameObject.layer) & WorldUtilityManager.Instance.GetEnviroLayers().value) != 0)
            {
                SpawnImpactVFX(other.ClosestPoint(transform.position));
                DestroyProjectile();
            }
        }

        void ApplyDamage(CharacterManager damageTarget, Collider hitCollider)
        {
            if (damageTarget == null || !damageTarget.IsOwner)
                return;

            Vector3 contactPoint = hitCollider != null
                ? hitCollider.ClosestPoint(transform.position)
                : damageTarget.transform.position;

            float angleHitFrom = Vector3.SignedAngle(transform.forward, damageTarget.transform.forward, Vector3.up);

            damageTarget.characterNetworkManager.NotifyTheServerOfCharacterDamageServerRpc(
                damageTarget.NetworkObjectId,
                sourceCharacter.NetworkObjectId,
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

        void SpawnTrailVFX()
        {
            if (activeTrailVFX != null)
                return;

            if (trailVFXPrefab != null)
            {
                activeTrailVFX = Instantiate(trailVFXPrefab, transform);
                activeTrailVFX.transform.localPosition = Vector3.zero;
                activeTrailVFX.transform.localRotation = Quaternion.identity;
                return;
            }

            activeTrailVFX = CreateRuntimeTrail();
        }

        GameObject CreateRuntimeTrail()
        {
            GameObject root = new GameObject("MoonSlashProjectile_Trail");
            root.transform.SetParent(transform);
            root.transform.localPosition = Vector3.zero;
            root.transform.localRotation = Quaternion.identity;

            TrailRenderer trail = root.AddComponent<TrailRenderer>();
            trail.time = 0.2f;
            trail.minVertexDistance = 0.03f;
            trail.widthMultiplier = 0.42f;
            trail.alignment = LineAlignment.View;
            trail.numCornerVertices = 4;
            trail.numCapVertices = 4;
            trail.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
            trail.receiveShadows = false;
            trail.material = CreateRuntimeTrailMaterial();
            trail.colorGradient = BuildTrailGradient();
            trail.widthCurve = new AnimationCurve(
                new Keyframe(0f, 0.2f),
                new Keyframe(0.35f, 1f),
                new Keyframe(1f, 0f));

            return root;
        }

        Material CreateRuntimeTrailMaterial()
        {
            Shader shader = Shader.Find("Universal Render Pipeline/Particles/Unlit");

            if (shader == null)
                shader = Shader.Find("Particles/Standard Unlit");

            if (shader == null)
                shader = Shader.Find("Sprites/Default");

            Material material = new Material(shader)
            {
                name = "MoonSlashProjectile_RuntimeTrail"
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

        Gradient BuildTrailGradient()
        {
            Gradient gradient = new Gradient();
            gradient.SetKeys(
                new[]
                {
                    new GradientColorKey(Color.Lerp(soulAuraColor, Color.white, 0.15f), 0f),
                    new GradientColorKey(Color.Lerp(soulAuraColor, soulCoreColor, 0.5f), 0.35f),
                    new GradientColorKey(soulCoreColor, 1f)
                },
                new[]
                {
                    new GradientAlphaKey(0.85f, 0f),
                    new GradientAlphaKey(0.42f, 0.6f),
                    new GradientAlphaKey(0f, 1f)
                });
            return gradient;
        }

        void SpawnImpactVFX(Vector3 impactPoint)
        {
            if (impactVFXPrefab != null)
            {
                Instantiate(impactVFXPrefab, impactPoint, Quaternion.identity);
                return;
            }

            TwinMoonVFXFactory.CreateImpactVFX(impactPoint, Color.Lerp(soulAuraColor, soulCoreColor, 0.5f));
        }

        void DestroyProjectile()
        {
            if (hasImpacted)
                return;

            hasImpacted = true;

            if (activeTrailVFX != null)
                activeTrailVFX.transform.SetParent(null, true);

            if (cachedCollider != null)
                cachedCollider.enabled = false;

            if (cachedRigidbody != null)
            {
                if (!cachedRigidbody.isKinematic)
                    cachedRigidbody.linearVelocity = Vector3.zero;
            }

            Destroy(gameObject, 0.02f);
        }
    }
}
