using UnityEngine;

namespace baodeag
{
    public class SpellItem : Item
    {
        [Header("Spell Class")]
        public SpellClass spellClass;

        [Header("Spell Modifiers")]
        public float fullChargeEffectMultiplier = 2;
        public int spellSlotUsed = 1;

        [Header("Spell FX")]
        [SerializeField] protected GameObject spellCastWarmUpFX;
        [SerializeField] protected GameObject spellCastReleaseFX;

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

        }

        //helper function to check weather or not we are able to use the spell when attempting to cast it
        public virtual bool CanICastThisSpell(PlayerManager player)
        {
            return true; 
        }
    }
}
