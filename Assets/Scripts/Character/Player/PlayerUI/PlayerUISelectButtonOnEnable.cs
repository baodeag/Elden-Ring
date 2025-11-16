using UnityEngine;
using UnityEngine.UI;

namespace baodeag
{
    public class PlayerUISelectButtonOnEnable : MonoBehaviour
    {
        private Button button;

        private void Awake()
        {
            button = GetComponent<Button>();
        }

        private void OnEnable()
        {
            button.Select();
            button.OnSelect(null);
        }
    }
}
