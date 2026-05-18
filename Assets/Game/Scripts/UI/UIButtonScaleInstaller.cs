using UnityEngine;
using UnityEngine.UI;

namespace baodeag
{
    public class UIButtonScaleInstaller : MonoBehaviour
    {
        [SerializeField] private string[] targetSpriteNames = { "Button_long2" };

        private void Awake()
        {
            Install();
        }

        private void OnEnable()
        {
            Install();
        }

        public void Install()
        {
            Button[] buttons = GetComponentsInChildren<Button>(true);

            for (int i = 0; i < buttons.Length; i++)
            {
                Button button = buttons[i];

                if (button == null)
                    continue;

                Image image = button.targetGraphic as Image;

                if (image == null || image.sprite == null)
                    continue;

                if (!IsTargetSprite(image.sprite.name))
                    continue;

                if (button.GetComponent<UIButtonScaleOnInteract>() == null)
                    button.gameObject.AddComponent<UIButtonScaleOnInteract>();
            }
        }

        private bool IsTargetSprite(string spriteName)
        {
            if (string.IsNullOrEmpty(spriteName) || targetSpriteNames == null)
                return false;

            for (int i = 0; i < targetSpriteNames.Length; i++)
            {
                if (string.Equals(spriteName, targetSpriteNames[i], System.StringComparison.OrdinalIgnoreCase))
                    return true;
            }

            return false;
        }
    }
}
