using UnityEngine;
using UnityEngine.UI;

namespace baodeag
{
    public class UI_StatBar : MonoBehaviour
    {
        private Slider slider;
        //variable to scale bar size depending on stat(higher stat = larger bar)
        //secondary bar behind may bar for polish effect(yellow bar that shows how much an action/damage takes away from current stat)

        protected virtual void Awake()
        {
            slider = GetComponent<Slider>();
        }

        public virtual void SetStat(int newValue)
        {
            slider.value = newValue;
        }

        public virtual void SetMaxStat(int maxValue)
        {
            slider.maxValue = maxValue;
            slider.value = maxValue;
        }
    }
}
