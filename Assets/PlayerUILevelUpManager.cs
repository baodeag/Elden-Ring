using baodeag;
using System.Collections;
using UnityEngine;

namespace baodeag
{
    public class PlayerUILevelUpManager : PlayerUIMenu
    {
        public override void CloseMenu()
        {
            CloseMenuAfterFixedFrame();
        }
    }
}
