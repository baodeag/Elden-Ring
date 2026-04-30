using UnityEngine;

namespace baodeag
{
    public class DeathCycloneVFX : MonoBehaviour
    {
        [SerializeField] Color outerColor = new Color(0.52f, 0.15f, 0.82f, 0.85f);
        [SerializeField] Color innerColor = new Color(0.22f, 0.38f, 0.86f, 0.75f);
        [SerializeField] float outerRadius = 2.7f;
        [SerializeField] float innerRadius = 1.6f;
        [SerializeField] float height = 2.8f;
        [SerializeField] float swirlRotationSpeed = 85f;
        [SerializeField] float pulseAmplitude = 0.08f;
        [SerializeField] float pulseSpeed = 3f;

        ParticleSystem outerSwirl;
        ParticleSystem innerSwirl;
        ParticleSystem soulDust;
        Light cycloneLight;
        Material sharedParticleMaterial;
        Vector3 baseScale;

        void Awake()
        {
            baseScale = transform.localScale;
            CreateVFX();
        }

        void Update()
        {
            transform.Rotate(Vector3.up, swirlRotationSpeed * Time.deltaTime, Space.Self);

            float pulse = 1f + Mathf.Sin(Time.time * pulseSpeed) * pulseAmplitude;
            transform.localScale = baseScale * pulse;
        }

        void OnDestroy()
        {
            if (sharedParticleMaterial != null)
                Destroy(sharedParticleMaterial);
        }

        void CreateVFX()
        {
            outerSwirl = CreateSwirl("OuterSwirl", outerColor, outerRadius, 34f, 0.16f, 0.8f, 2.6f, -0.45f, height);
            innerSwirl = CreateSwirl("InnerSwirl", innerColor, innerRadius, 22f, 0.12f, 0.6f, 3.4f, -0.75f, height * 0.8f);
            soulDust = CreateDust("SoulDust", Color.Lerp(outerColor, innerColor, 0.5f));

            cycloneLight = gameObject.AddComponent<Light>();
            cycloneLight.type = LightType.Point;
            cycloneLight.range = outerRadius * 2.1f;
            cycloneLight.intensity = 1.25f;
            cycloneLight.color = Color.Lerp(outerColor, innerColor, 0.35f);
        }

        ParticleSystem CreateSwirl(
            string objectName,
            Color color,
            float radius,
            float emissionRate,
            float startSize,
            float startLifetime,
            float orbitalVelocity,
            float radialVelocity,
            float verticalHeight)
        {
            GameObject particleObject = new GameObject(objectName);
            particleObject.transform.SetParent(transform);
            particleObject.transform.localPosition = Vector3.zero;
            particleObject.transform.localRotation = Quaternion.identity;
            particleObject.transform.localScale = Vector3.one;

            ParticleSystem particles = particleObject.AddComponent<ParticleSystem>();
            particles.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);

            ParticleSystemRenderer renderer = particleObject.GetComponent<ParticleSystemRenderer>();
            renderer.material = GetParticleMaterial();
            renderer.renderMode = ParticleSystemRenderMode.Billboard;
            renderer.alignment = ParticleSystemRenderSpace.View;
            renderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
            renderer.receiveShadows = false;

            var main = particles.main;
            main.playOnAwake = false;
            main.loop = true;
            main.duration = 1f;
            main.startLifetime = startLifetime;
            main.startSpeed = 0.2f;
            main.startSize = startSize;
            main.startColor = color;
            main.simulationSpace = ParticleSystemSimulationSpace.Local;
            main.scalingMode = ParticleSystemScalingMode.Local;
            main.gravityModifier = 0f;

            var emission = particles.emission;
            emission.rateOverTime = emissionRate;

            var shape = particles.shape;
            shape.shapeType = ParticleSystemShapeType.ConeVolume;
            shape.radius = radius;
            shape.angle = 8f;
            shape.length = verticalHeight;
            shape.arcMode = ParticleSystemShapeMultiModeValue.Random;

            var velocity = particles.velocityOverLifetime;
            velocity.enabled = true;
            velocity.orbitalY = orbitalVelocity;
            velocity.radial = radialVelocity;
            velocity.y = new ParticleSystem.MinMaxCurve(0.45f);
            velocity.space = ParticleSystemSimulationSpace.Local;

            var colorOverLifetime = particles.colorOverLifetime;
            colorOverLifetime.enabled = true;
            colorOverLifetime.color = BuildGradient(color, 0.9f);

            var sizeOverLifetime = particles.sizeOverLifetime;
            sizeOverLifetime.enabled = true;
            sizeOverLifetime.size = new ParticleSystem.MinMaxCurve(
                1f,
                new AnimationCurve(
                    new Keyframe(0f, 0.2f),
                    new Keyframe(0.35f, 1f),
                    new Keyframe(1f, 0.15f)));

            var trails = particles.trails;
            trails.enabled = true;
            trails.dieWithParticles = true;
            trails.lifetime = 0.18f;
            trails.widthOverTrail = 0.35f;
            trails.colorOverTrail = BuildGradient(color, 0.75f);

            particles.Play();
            return particles;
        }

        ParticleSystem CreateDust(string objectName, Color color)
        {
            GameObject particleObject = new GameObject(objectName);
            particleObject.transform.SetParent(transform);
            particleObject.transform.localPosition = Vector3.zero;
            particleObject.transform.localRotation = Quaternion.identity;
            particleObject.transform.localScale = Vector3.one;

            ParticleSystem particles = particleObject.AddComponent<ParticleSystem>();
            particles.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);

            ParticleSystemRenderer renderer = particleObject.GetComponent<ParticleSystemRenderer>();
            renderer.material = GetParticleMaterial();
            renderer.renderMode = ParticleSystemRenderMode.Billboard;
            renderer.alignment = ParticleSystemRenderSpace.View;
            renderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
            renderer.receiveShadows = false;

            var main = particles.main;
            main.playOnAwake = false;
            main.loop = true;
            main.duration = 1f;
            main.startLifetime = 1.25f;
            main.startSpeed = 0.18f;
            main.startSize = 0.22f;
            main.startColor = new Color(color.r, color.g, color.b, 0.4f);
            main.simulationSpace = ParticleSystemSimulationSpace.Local;
            main.scalingMode = ParticleSystemScalingMode.Local;

            var emission = particles.emission;
            emission.rateOverTime = 18f;

            var shape = particles.shape;
            shape.shapeType = ParticleSystemShapeType.Circle;
            shape.radius = outerRadius * 0.85f;
            shape.radiusThickness = 0.55f;
            shape.arcMode = ParticleSystemShapeMultiModeValue.Random;

            var velocity = particles.velocityOverLifetime;
            velocity.enabled = true;
            velocity.orbitalY = 1.6f;
            velocity.y = new ParticleSystem.MinMaxCurve(0.35f);
            velocity.space = ParticleSystemSimulationSpace.Local;

            var colorOverLifetime = particles.colorOverLifetime;
            colorOverLifetime.enabled = true;
            colorOverLifetime.color = BuildGradient(color, 0.45f);

            particles.Play();
            return particles;
        }

        Material GetParticleMaterial()
        {
            if (sharedParticleMaterial != null)
                return sharedParticleMaterial;

            Shader shader = Shader.Find("Universal Render Pipeline/Particles/Unlit");

            if (shader == null)
                shader = Shader.Find("Particles/Standard Unlit");

            if (shader == null)
                shader = Shader.Find("Sprites/Default");

            sharedParticleMaterial = new Material(shader)
            {
                name = "DeathCycloneVFX_Runtime"
            };

            if (sharedParticleMaterial.HasProperty("_Surface"))
                sharedParticleMaterial.SetFloat("_Surface", 1f);

            if (sharedParticleMaterial.HasProperty("_Blend"))
                sharedParticleMaterial.SetFloat("_Blend", 0f);

            if (sharedParticleMaterial.HasProperty("_BaseColor"))
                sharedParticleMaterial.SetColor("_BaseColor", Color.white);

            if (sharedParticleMaterial.HasProperty("_Color"))
                sharedParticleMaterial.SetColor("_Color", Color.white);

            return sharedParticleMaterial;
        }

        Gradient BuildGradient(Color color, float alpha)
        {
            Gradient gradient = new Gradient();
            gradient.SetKeys(
                new[]
                {
                    new GradientColorKey(Color.Lerp(color, Color.white, 0.15f), 0f),
                    new GradientColorKey(color, 0.45f),
                    new GradientColorKey(Color.Lerp(color, Color.black, 0.35f), 1f)
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
    }
}
