using UnityEngine;

namespace baodeag
{
    public class AIForceAnimatorBool : MonoBehaviour
    {
        [SerializeField] string parameterName = "isTwoHandingWeapon";
        [SerializeField] bool parameterValue = true;

        Animator cachedAnimator;

        void Awake()
        {
            cachedAnimator = GetComponent<Animator>();
            ApplyValue();
        }

        void OnEnable()
        {
            ApplyValue();
        }

        void Start()
        {
            ApplyValue();
        }

        void ApplyValue()
        {
            if (cachedAnimator == null)
                cachedAnimator = GetComponent<Animator>();

            if (cachedAnimator == null || string.IsNullOrWhiteSpace(parameterName))
                return;

            cachedAnimator.SetBool(parameterName, parameterValue);
        }
    }
}
