using UnityEngine;

namespace baodeag
{
    public class PlayerUISiteOfGraceManager : PlayerUIMenu
    {
        public void OpenTeleportLocationMenu()
        {
            PlayerUIManager.instance.TransitionToMenu(this, PlayerUIManager.instance.playerUITeleportLocationManager);
        }

        public void OpenLevelUpMenu()
        {
            PlayerUIManager.instance.TransitionToMenu(this, PlayerUIManager.instance.playerUILevelUpManager);
        }
    }
}
