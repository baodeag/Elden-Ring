using UnityEngine;
using TMPro;

namespace baodeag
{
    public class UI_StatusEffectWarning : MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI warningText;
        public CanvasGroup canvas;

        [Header("Effect Colors")]
        [SerializeField] Color poisonedColor;
        [SerializeField] Color bloodLossColor;
        [SerializeField] Color frostColor;

        public void SetWarningMessage(BuildUp status)
        {
            switch (status)
            {
                case BuildUp.Poison:
                    SetCustomMessage("Poisoned!", poisonedColor);
                    break;
                case BuildUp.Bleed:
                    SetCustomMessage("Blood Loss!", bloodLossColor);
                    break;
                case BuildUp.Frost:
                    SetCustomMessage("Frostbite!", frostColor);
                    break;
                default:
                    break;
            }
        }

        public void SetCustomMessage(string message, Color color)
        {
            warningText.color = color;
            warningText.text = message;
        }
    }
}
