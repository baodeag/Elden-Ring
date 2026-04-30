using UnityEngine;

namespace baodeag
{
    public class SpellItem : Item
    {
        [Header("Spell Class")]
        public SpellClass spellClass;

        [Header("Spell Modifiers")]
        public float fullChargeEffectMultiplier = 2;

        [Header("Spell Costs")]
        public int spellSlotUsed = 1;
        public int staminaCost = 25;
        public int focusPointCost = 25;

        [Header("Spell FX")]
        [SerializeField] protected GameObject spellCastWarmUpFX;
        [SerializeField] protected GameObject spellChargeFX;
        [SerializeField] protected GameObject spellCastReleaseFX;
        [SerializeField] protected GameObject spellCastReleaseFXFullCharge;

        [Header("Animations")]
        [SerializeField] protected string mainHandSpellAnimation;
        [SerializeField] protected string offHandSpellAnimation;

        [Header("Sound FX")]
        public AudioClip warmUpSoundFX;
        public AudioClip releaseSoundFX;

        //play the spell casting animation
        public virtual void AttemptToCastSpell(PlayerManager player)
        {

        }

        //spell fx that are instantiated when attempting to cast the spell
        public virtual void InstantiateWarmUpSpellFX(PlayerManager player)
        {

        }

        //apply spell effects
        public virtual void SuccessfullyCastSpell(PlayerManager player)
        {
            if (player.IsOwner)
            {
                player.playerNetworkManager.currentFocusPoints.Value -= focusPointCost;
                player.playerNetworkManager.currentStamina.Value -= staminaCost;
            }
        }

        public virtual void SuccessfullyChargeSpell(PlayerManager player)
        {

        }

        public virtual void SuccessfullyCastSpellFullCharge(PlayerManager player)
        {
            if (player.IsOwner)
            {
                player.playerNetworkManager.currentFocusPoints.Value -= Mathf.RoundToInt(focusPointCost * fullChargeEffectMultiplier);
                player.playerNetworkManager.currentStamina.Value -= staminaCost * fullChargeEffectMultiplier;
            }
        }

        //helper function to check weather or not we are able to use the spell when attempting to cast it
        public virtual bool CanICastThisSpell(PlayerManager player)
        {
            if (player.playerNetworkManager.currentFocusPoints.Value <= focusPointCost)
                return false;

            if (player.playerNetworkManager.currentStamina.Value <= staminaCost)
                return false;

            if (player.isPerformingAction)
                return false;

            if (player.playerNetworkManager.isJumping.Value)
                return false;

            return true; 
        }
    }
}
