using UnityEngine;

namespace baodeag
{
    [RequireComponent(typeof(LineRenderer))]
    public class TwinMoonShockwaveRing : MonoBehaviour
    {
        [SerializeField] int segmentCount = 48;
        [SerializeField] float startRadius = 0.35f;
        [SerializeField] float endRadius = 6f;
        [SerializeField] float duration = 0.45f;
        [SerializeField] float startWidth = 0.45f;
        [SerializeField] float endWidth = 0.08f;
        [SerializeField] float heightOffset = 0.05f;
        [SerializeField] Color startColor = new Color(0.35f, 0.95f, 1f, 0.95f);
        [SerializeField] Color endColor = new Color(0.35f, 0.95f, 1f, 0f);

        LineRenderer lineRenderer;
        float elapsed;

        void Awake()
        {
            lineRenderer = GetComponent<LineRenderer>();
            lineRenderer.loop = true;
            lineRenderer.useWorldSpace = false;
            lineRenderer.positionCount = segmentCount;
            lineRenderer.textureMode = LineTextureMode.Stretch;
            lineRenderer.alignment = LineAlignment.View;
        }

        public void Initialize(float radius, float lifetime, Color color)
        {
            endRadius = radius;
            duration = Mathf.Max(0.05f, lifetime);
            startColor = color;
            endColor = new Color(color.r, color.g, color.b, 0f);
            DrawRing(startRadius);
            ApplyVisuals(0f);
            Destroy(gameObject, duration + 0.25f);
        }

        void Update()
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / duration);
            float radius = Mathf.Lerp(startRadius, endRadius, t);
            DrawRing(radius);
            ApplyVisuals(t);
        }

        void DrawRing(float radius)
        {
            for (int i = 0; i < segmentCount; i++)
            {
                float angle = (i / (float)segmentCount) * Mathf.PI * 2f;
                Vector3 point = new Vector3(Mathf.Cos(angle) * radius, heightOffset, Mathf.Sin(angle) * radius);
                lineRenderer.SetPosition(i, point);
            }
        }

        void ApplyVisuals(float normalizedTime)
        {
            lineRenderer.widthMultiplier = Mathf.Lerp(startWidth, endWidth, normalizedTime);

            Gradient gradient = new Gradient();
            gradient.SetKeys(
                new[]
                {
                    new GradientColorKey(Color.Lerp(startColor, Color.white, 0.15f), 0f),
                    new GradientColorKey(startColor, 0.45f),
                    new GradientColorKey(startColor, 1f)
                },
                new[]
                {
                    new GradientAlphaKey(Mathf.Lerp(startColor.a, endColor.a, normalizedTime), 0f),
                    new GradientAlphaKey(Mathf.Lerp(startColor.a * 0.75f, endColor.a, normalizedTime), 0.65f),
                    new GradientAlphaKey(endColor.a, 1f)
                });

            lineRenderer.colorGradient = gradient;
        }
    }
}
