using UnityEngine;

namespace baodeag
{
    public class AshOfWar : Item
    {
        [Header("Ash of War Information")]
        public WeaponClass[] usableWeaponClasses;

        [Header("Costs")]
        public int focusPointCost = 20;
        public int staminaCost = 20;

        public virtual void AttemptToPerformAction(PlayerManager playerPerformingAction)
        {
            Debug.Log("Performed");
        }

        public virtual bool CanIUseThisAbility(PlayerManager playerPerformingAbility)
        {
            return false;
        }

        protected virtual void DeductStaminaCost(PlayerManager playerPerformingAction)
        {
            playerPerformingAction.playerNetworkManager.currentStamina.Value -= staminaCost;
        }

        protected virtual void DeductFocusPointCost(PlayerManager playerPerformingAction)
        {

        }
    }
}
