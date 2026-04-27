using UnityEngine;

namespace baodeag
{
    public static class TwinMoonVFXFactory
    {
        static Material runtimeAdditiveMaterial;

        public static GameObject CreateChargeVFX(Transform parent, Vector3 localOffset, Color color, float duration)
        {
            GameObject root = new GameObject("TwinMoon_Charge_VFX");
            root.transform.SetParent(parent);
            root.transform.localPosition = localOffset;
            root.transform.localRotation = Quaternion.identity;
            root.transform.localScale = Vector3.one;

            AddAutoDestroy(root, duration + 0.35f);
            CreateChargeSwirl(root.transform, color);
            CreateChargeCore(root.transform, color);
            CreateChargeSparks(root.transform, color);

            Light chargeLight = root.AddComponent<Light>();
            chargeLight.type = LightType.Point;
            chargeLight.range = 3f;
            chargeLight.intensity = 1.15f;
            chargeLight.color = Color.Lerp(color, Color.white, 0.2f);

            return root;
        }

        public static GameObject CreateImpactVFX(Vector3 position, Color color)
        {
            GameObject root = new GameObject("TwinMoon_Impact_VFX");
            root.transform.position = position;
            root.transform.rotation = Quaternion.identity;

            AddAutoDestroy(root, 2f);
            CreateImpactFlash(root.transform, color);
            CreateImpactBurst(root.transform, color);
            CreateImpactDustRing(root.transform, color);
            return root;
        }

        public static GameObject CreateShockwaveVFX(Vector3 position, float radius, float duration, Color color)
        {
            GameObject root = new GameObject("TwinMoon_Shockwave_VFX");
            root.transform.position = position;
            root.transform.rotation = Quaternion.identity;

            CreateShockwaveRing(root.transform, radius, duration, color);
            CreateShockwaveDust(root.transform, radius, color, duration);
            AddAutoDestroy(root, duration + 1f);
            return root;
        }

        static void CreateChargeSwirl(Transform parent, Color color)
        {
            ParticleSystem particles = CreateParticleSystem("Charge_Swirl", parent, new Vector3(0f, 0.1f, 0f));
            var main = particles.main;
            main.duration = 0.9f;
            main.loop = true;
            main.startLifetime = 0.55f;
            main.startSpeed = 0.2f;
            main.startSize = 0.18f;
            main.startColor = new Color(color.r, color.g, color.b, 0.8f);
            main.simulationSpace = ParticleSystemSimulationSpace.Local;
            main.gravityModifier = 0f;

            var emission = particles.emission;
            emission.rateOverTime = 42f;

            var shape = particles.shape;
            shape.shapeType = ParticleSystemShapeType.Circle;
            shape.radius = 0.35f;
            shape.arcMode = ParticleSystemShapeMultiModeValue.Random;

            var velocity = particles.velocityOverLifetime;
            velocity.enabled = true;
            velocity.orbitalY = 2.8f;
            velocity.radial = -0.35f;
            velocity.space = ParticleSystemSimulationSpace.Local;

            var colorOverLifetime = particles.colorOverLifetime;
            colorOverLifetime.enabled = true;
            colorOverLifetime.color = BuildGradient(color, 0.8f);

            var sizeOverLifetime = particles.sizeOverLifetime;
            sizeOverLifetime.enabled = true;
            sizeOverLifetime.size = BuildCurve(0.25f, 1.15f, 0.15f);

            particles.Play();
        }

        static void CreateChargeCore(Transform parent, Color color)
        {
            ParticleSystem particles = CreateParticleSystem("Charge_Core", parent, Vector3.zero);
            var main = particles.main;
            main.duration = 0.9f;
            main.loop = true;
            main.startLifetime = 0.4f;
            main.startSpeed = 0f;
            main.startSize = 0.55f;
            main.startColor = new Color(color.r, color.g, color.b, 0.45f);

            var emission = particles.emission;
            emission.rateOverTime = 9f;

            var shape = particles.shape;
            shape.shapeType = ParticleSystemShapeType.Sphere;
            shape.radius = 0.05f;

            var sizeOverLifetime = particles.sizeOverLifetime;
            sizeOverLifetime.enabled = true;
            sizeOverLifetime.size = BuildCurve(0.2f, 1.1f, 0.1f);

            var colorOverLifetime = particles.colorOverLifetime;
            colorOverLifetime.enabled = true;
            colorOverLifetime.color = BuildGradient(color, 0.5f);

            particles.Play();
        }

        static void CreateChargeSparks(Transform parent, Color color)
        {
            ParticleSystem particles = CreateParticleSystem("Charge_Sparks", parent, Vector3.zero);
            var main = particles.main;
            main.duration = 0.9f;
            main.loop = true;
            main.startLifetime = 0.35f;
            main.startSpeed = 1.65f;
            main.startSize = 0.08f;
            main.startColor = Color.Lerp(color, Color.white, 0.3f);

            var emission = particles.emission;
            emission.rateOverTime = 28f;

            var shape = particles.shape;
            shape.shapeType = ParticleSystemShapeType.Sphere;
            shape.radius = 0.18f;

            var velocity = particles.velocityOverLifetime;
            velocity.enabled = true;
            velocity.radial = -1.25f;
            velocity.orbitalY = 1.5f;
            velocity.space = ParticleSystemSimulationSpace.Local;

            var trails = particles.trails;
            trails.enabled = true;
            trails.dieWithParticles = true;
            trails.lifetime = 0.12f;
            trails.widthOverTrail = 0.35f;
            trails.colorOverTrail = BuildGradient(color, 0.8f);

            particles.Play();
        }

        static void CreateImpactFlash(Transform parent, Color color)
        {
            ParticleSystem particles = CreateParticleSystem("Impact_Flash", parent, new Vector3(0f, 0.05f, 0f));
            var main = particles.main;
            main.duration = 0.2f;
            main.loop = false;
            main.startLifetime = 0.12f;
            main.startSpeed = 0f;
            main.startSize = 1.15f;
            main.startColor = Color.Lerp(color, Color.white, 0.55f);

            var emission = particles.emission;
            emission.rateOverTime = 0f;
            emission.SetBursts(new[] { new ParticleSystem.Burst(0f, 1) });

            var shape = particles.shape;
            shape.shapeType = ParticleSystemShapeType.Circle;
            shape.radius = 0f;

            var sizeOverLifetime = particles.sizeOverLifetime;
            sizeOverLifetime.enabled = true;
            sizeOverLifetime.size = BuildCurve(0.2f, 1.4f, 0f);

            particles.Play();
        }

        static void CreateImpactBurst(Transform parent, Color color)
        {
            ParticleSystem particles = CreateParticleSystem("Impact_Burst", parent, new Vector3(0f, 0.05f, 0f));
            var main = particles.main;
            main.duration = 0.4f;
            main.loop = false;
            main.startLifetime = 0.35f;
            main.startSpeed = 4.8f;
            main.startSize = 0.2f;
            main.startColor = new Color(color.r, color.g, color.b, 0.85f);
            main.gravityModifier = 0.15f;

            var emission = particles.emission;
            emission.rateOverTime = 0f;
            emission.SetBursts(new[] { new ParticleSystem.Burst(0f, 40) });

            var shape = particles.shape;
            shape.shapeType = ParticleSystemShapeType.Circle;
            shape.radius = 0.15f;

            var velocity = particles.velocityOverLifetime;
            velocity.enabled = true;
            velocity.y = new ParticleSystem.MinMaxCurve(2.2f);

            var colorOverLifetime = particles.colorOverLifetime;
            colorOverLifetime.enabled = true;
            colorOverLifetime.color = BuildGradient(color, 0.9f);

            particles.Play();
        }

        static void CreateImpactDustRing(Transform parent, Color color)
        {
            ParticleSystem particles = CreateParticleSystem("Impact_DustRing", parent, new Vector3(0f, 0.02f, 0f));
            var main = particles.main;
            main.duration = 0.55f;
            main.loop = false;
            main.startLifetime = 0.55f;
            main.startSpeed = 5.2f;
            main.startSize = 0.26f;
            main.startColor = new Color(0.85f, 0.95f, 1f, 0.42f);
            main.gravityModifier = 0.05f;

            var emission = particles.emission;
            emission.rateOverTime = 0f;
            emission.SetBursts(new[] { new ParticleSystem.Burst(0f, 54) });

            var shape = particles.shape;
            shape.shapeType = ParticleSystemShapeType.Circle;
            shape.radius = 0.1f;
            shape.arc = 360f;

            var velocity = particles.velocityOverLifetime;
            velocity.enabled = true;
            velocity.orbitalY = 0.4f;

            var colorOverLifetime = particles.colorOverLifetime;
            colorOverLifetime.enabled = true;
            colorOverLifetime.color = BuildGradient(new Color(color.r, color.g, color.b, 0.5f), 0.5f);

            particles.Play();
        }

        static void CreateShockwaveRing(Transform parent, float radius, float duration, Color color)
        {
            GameObject ring = new GameObject("Shockwave_Ring");
            ring.transform.SetParent(parent);
            ring.transform.localPosition = Vector3.zero;
            ring.transform.localRotation = Quaternion.identity;
            ring.transform.localScale = Vector3.one;

            LineRenderer lineRenderer = ring.AddComponent<LineRenderer>();
            lineRenderer.material = GetRuntimeAdditiveMaterial();
            lineRenderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
            lineRenderer.receiveShadows = false;
            lineRenderer.numCapVertices = 6;
            lineRenderer.numCornerVertices = 6;

            TwinMoonShockwaveRing shockwaveRing = ring.AddComponent<TwinMoonShockwaveRing>();
            shockwaveRing.Initialize(radius, duration, color);
        }

        static void CreateShockwaveDust(Transform parent, float radius, Color color, float duration)
        {
            ParticleSystem particles = CreateParticleSystem("Shockwave_Dust", parent, new Vector3(0f, 0.02f, 0f));
            var main = particles.main;
            main.duration = duration;
            main.loop = false;
            main.startLifetime = duration * 0.95f;
            main.startSpeed = Mathf.Max(3.5f, radius * 1.45f);
            main.startSize = Mathf.Lerp(0.14f, 0.28f, Mathf.InverseLerp(0f, 6f, radius));
            main.startColor = new Color(color.r, color.g, color.b, 0.35f);
            main.gravityModifier = 0f;

            var emission = particles.emission;
            emission.rateOverTime = 0f;
            emission.SetBursts(new[] { new ParticleSystem.Burst(0f, 64) });

            var shape = particles.shape;
            shape.shapeType = ParticleSystemShapeType.Circle;
            shape.radius = 0.12f;

            var colorOverLifetime = particles.colorOverLifetime;
            colorOverLifetime.enabled = true;
            colorOverLifetime.color = BuildGradient(color, 0.38f);

            var sizeOverLifetime = particles.sizeOverLifetime;
            sizeOverLifetime.enabled = true;
            sizeOverLifetime.size = BuildCurve(0.4f, 1.35f, 0f);

            particles.Play();
        }

        static ParticleSystem CreateParticleSystem(string name, Transform parent, Vector3 localPosition)
        {
            GameObject particleObject = new GameObject(name);
            particleObject.transform.SetParent(parent);
            particleObject.transform.localPosition = localPosition;
            particleObject.transform.localRotation = Quaternion.identity;
            particleObject.transform.localScale = Vector3.one;

            ParticleSystem particles = particleObject.AddComponent<ParticleSystem>();
            particles.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);

            ParticleSystemRenderer renderer = particleObject.GetComponent<ParticleSystemRenderer>();
            renderer.material = GetRuntimeAdditiveMaterial();
            renderer.renderMode = ParticleSystemRenderMode.Billboard;
            renderer.alignment = ParticleSystemRenderSpace.View;
            renderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
            renderer.receiveShadows = false;

            var main = particles.main;
            main.playOnAwake = false;
            main.scalingMode = ParticleSystemScalingMode.Local;

            return particles;
        }

        static Material GetRuntimeAdditiveMaterial()
        {
            if (runtimeAdditiveMaterial != null)
                return runtimeAdditiveMaterial;

            Shader shader = Shader.Find("Universal Render Pipeline/Particles/Unlit");

            if (shader == null)
                shader = Shader.Find("Particles/Standard Unlit");

            if (shader == null)
                shader = Shader.Find("Sprites/Default");

            runtimeAdditiveMaterial = new Material(shader)
            {
                name = "TwinMoon_Runtime_Additive"
            };

            if (runtimeAdditiveMaterial.HasProperty("_Surface"))
                runtimeAdditiveMaterial.SetFloat("_Surface", 1f);

            if (runtimeAdditiveMaterial.HasProperty("_Blend"))
                runtimeAdditiveMaterial.SetFloat("_Blend", 0f);

            if (runtimeAdditiveMaterial.HasProperty("_BaseColor"))
                runtimeAdditiveMaterial.SetColor("_BaseColor", Color.white);

            if (runtimeAdditiveMaterial.HasProperty("_Color"))
                runtimeAdditiveMaterial.SetColor("_Color", Color.white);

            return runtimeAdditiveMaterial;
        }

        static Gradient BuildGradient(Color color, float alpha)
        {
            Gradient gradient = new Gradient();
            gradient.SetKeys(
                new[]
                {
                    new GradientColorKey(Color.Lerp(color, Color.white, 0.2f), 0f),
                    new GradientColorKey(color, 0.45f),
                    new GradientColorKey(color, 1f)
                },
                new[]
                {
                    new GradientAlphaKey(0f, 0f),
                    new GradientAlphaKey(alpha, 0.12f),
                    new GradientAlphaKey(alpha * 0.45f, 0.7f),
                    new GradientAlphaKey(0f, 1f)
                });
            return gradient;
        }

        static ParticleSystem.MinMaxCurve BuildCurve(float start, float mid, float end)
        {
            return new ParticleSystem.MinMaxCurve(1f, new AnimationCurve(
                new Keyframe(0f, start),
                new Keyframe(0.35f, mid),
                new Keyframe(1f, end)));
        }

        static void AddAutoDestroy(GameObject target, float lifetime)
        {
            Utility_DestroyAfterTime destroyAfterTime = target.AddComponent<Utility_DestroyAfterTime>();
            destroyAfterTime.SetLifetime(lifetime);
        }
    }
}
