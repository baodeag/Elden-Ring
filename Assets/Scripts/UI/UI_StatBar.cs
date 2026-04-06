using UnityEngine;
using UnityEngine.UI;

namespace baodeag
{
    public class UI_StatBar : MonoBehaviour
    {
        protected Slider slider;
        protected RectTransform rectTransform;

        [Header("Bar Options")]
        [SerializeField] protected bool scaleBarLengthWithStats = true;
        [SerializeField] protected float widthScaleMultiplier = 1;
        //secondary bar behind may bar for polish effect(yellow bar that shows how much an action/damage takes away from current stat)

        [Header("Fill Color")]
        [SerializeField] protected Image barFillImage;
        [SerializeField] protected Color barFillColor;

        protected virtual void Awake()
        {
            slider = GetComponent<Slider>();
            rectTransform = GetComponent<RectTransform>();
        }

        protected virtual void Start()
        {
            
        }

        public virtual void SetStat(int newValue)
        {
            slider.value = newValue;
        }

        public virtual void SetMaxStat(int maxValue)
        {
            slider.maxValue = maxValue;
            slider.value = maxValue;

            if (scaleBarLengthWithStats)
            {
                rectTransform.sizeDelta = new Vector2(maxValue * widthScaleMultiplier, rectTransform.sizeDelta.y);
                //reset the position of the bar to be centered
                PlayerUIManager.instance.playerUIHudManager.RefreshHUD();
            }
        }

        public void ToggleBarFillColor(bool isPoisoned)
        {
            if (barFillImage == null)
                return;

            if (isPoisoned)
            {
                barFillImage.color = WorldUtilityManager.Instance.GetPoisonedColor();

            }
            else
            {
                barFillImage.color = barFillColor;
            }
        }
    }
}
