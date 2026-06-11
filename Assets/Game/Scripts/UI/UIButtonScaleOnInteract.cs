using UnityEngine;
using UnityEngine.EventSystems;

namespace baodeag
{
    public class UIButtonScaleOnInteract : MonoBehaviour, IPointerDownHandler, IPointerEnterHandler, IPointerExitHandler, IPointerUpHandler
    {
        [SerializeField] private Vector3 hoverScale = new Vector3(0.98f, 0.98f, 1f);
        [SerializeField] private Vector3 pressedScale = new Vector3(0.95f, 0.95f, 1f);
        [SerializeField] private float transitionSpeed = 18f;
        [SerializeField] private bool useUnscaledTime = true;

        private RectTransform rectTransform;
        private Vector3 baseScale;
        private bool isPointerInside;
        private bool isPressed;
        private bool hasBaseScale;

        private void Awake()
        {
            rectTransform = GetComponent<RectTransform>();
            CaptureBaseScale();
        }

        private void OnEnable()
        {
            ResetState();
        }

        private void OnDisable()
        {
            ResetState();
        }

        private void Update()
        {
            if (rectTransform == null)
                return;

            float deltaTime = useUnscaledTime ? Time.unscaledDeltaTime : Time.deltaTime;
            float lerpFactor = 1f - Mathf.Exp(-transitionSpeed * deltaTime);
            rectTransform.localScale = Vector3.Lerp(rectTransform.localScale, GetTargetScale(), lerpFactor);
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            isPointerInside = true;
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            isPointerInside = false;
            isPressed = false;
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            isPressed = true;
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            isPressed = false;
        }

        private Vector3 GetTargetScale()
        {
            if (isPressed)
                return Vector3.Scale(baseScale, pressedScale);

            if (isPointerInside)
                return Vector3.Scale(baseScale, hoverScale);

            return baseScale;
        }

        private void ResetState()
        {
            isPointerInside = false;
            isPressed = false;

            if (rectTransform == null)
                rectTransform = GetComponent<RectTransform>();

            if (rectTransform != null)
            {
                rectTransform.localScale = baseScale;
            }
        }

        private void CaptureBaseScale()
        {
            if (rectTransform == null)
                rectTransform = GetComponent<RectTransform>();

            if (hasBaseScale)
                return;

            baseScale = rectTransform != null ? rectTransform.localScale : Vector3.one;
            hasBaseScale = true;
        }
    }
}
