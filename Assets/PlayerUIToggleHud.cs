using UnityEngine;

namespace baodeag
{
    public class PlayerUIToggleHud : MonoBehaviour
    {
        private void OnEnable()
        {
            //hid the hud
            PlayerUIManager.instance.playerUIHudManager.ToggleHUD(false);
        }

        private void OnDisable()
        {
            //show the hud
            PlayerUIManager.instance.playerUIHudManager.ToggleHUD(true);
        }
    }
}
