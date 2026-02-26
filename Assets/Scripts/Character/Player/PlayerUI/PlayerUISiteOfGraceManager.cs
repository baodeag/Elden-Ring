using UnityEngine;

namespace baodeag
{
    public class PlayerUISiteOfGraceManager : PlayerUIMenu
    {
        public void OpenTeleportLocationMenu()
        {
            CloseMenu();
            PlayerUIManager.instance.playerUITeleportLocationManager.OpenMenu();
        }

        public void OpenLevelUpMenu()
        {
            CloseMenu();
            PlayerUIManager.instance.playerUILevelUpManager.OpenMenu();
        }
    }
}
