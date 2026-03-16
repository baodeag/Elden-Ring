using NUnit.Framework;
using NUnit.Framework.Internal;
using System.Collections.Generic;
using UnityEngine;

namespace baodeag
{
    public class CharacterEffectsManager : MonoBehaviour
    {
        //process instant effect (take dmg, heal)

        //process timed effects (poison, burn, buff)

        //process static effects (add/remove buffs)

        protected CharacterManager character;

        [Header("Current Active FX")]
        public GameObject activeQuickSlotItemFX;
        public GameObject activeSpellWarmUpFX;
        public GameObject activeDrawnProjectileFX;

        [Header("VFX")]
        [SerializeField] GameObject bloodSplatterVFX;
        [SerializeField] GameObject criticalBloodSplatterVFX;

        [Header("Status Effect VFX")]
        [HideInInspector] public GameObject poisonedVFX;
        [HideInInspector] public GameObject frostBiteVFX;

        [Header("Static Effects")]
        public List<StaticCharacterEffect> staticEffects = new List<StaticCharacterEffect>();

        [Header("Timed Effects")]
        [SerializeField] protected float effectTickTimer = 0;
        [SerializeField] protected float defaultEffectTickTime = 1;
        public List<TimedCharacterEffect> timedEffects = new List<TimedCharacterEffect>();

        protected virtual void Awake()
        {
            character = GetComponent<CharacterManager>();
        }

        protected virtual void Update()
        {
            effectTickTimer -= Time.deltaTime;

            if (effectTickTimer <= 0)
            {
                effectTickTimer = defaultEffectTickTime;
                ProcessTimedEffects();
            }
        }

        public virtual void ProcessInstantEffect(InstantCharacterEffect effect)
        {
            effect.ProcessEffect(character);
        }

        public void PlayBloodSplatterVFX(Vector3 contactPoint)
        {
            if (bloodSplatterVFX != null)
            {
                GameObject bloodSplatter = Instantiate(bloodSplatterVFX, contactPoint, Quaternion.identity);
            }
            else
            {
                GameObject bloodSplatter = Instantiate(WorldCharacterEffectsManager.instance.bloodSplatterVFX, contactPoint, Quaternion.identity);
            }
        }

        public void PlayCriticalBloodSplatterVFX(Vector3 contactPoint)
        {
            if (bloodSplatterVFX != null)
            {
                GameObject bloodSplatter = Instantiate(criticalBloodSplatterVFX, contactPoint, Quaternion.identity);
            }
            else
            {
                GameObject bloodSplatter = Instantiate(WorldCharacterEffectsManager.instance.criticalBloodSplatterVFX, contactPoint, Quaternion.identity);
            }
        }

        public virtual void AddBuildUps(BuildUp buildUpType, float amount)
        {
            if (!character.IsOwner)
                return;

            switch (buildUpType)
            {
                case BuildUp.Poison:
                    character.characterNetworkManager.poisonBuildUp.Value += amount;
                    break;
                case BuildUp.Bleed:
                    character.characterNetworkManager.bleedBuildUp.Value += amount;
                    break;
                case BuildUp.Frost:
                    character.characterNetworkManager.frostBiteBuildUp.Value += amount;
                    break;
                default:
                    break;
            }
        }

        //static effects
        public void AddStaticEffect(StaticCharacterEffect effect)
        {
            staticEffects.Add(effect);
            effect.ProcessStaticEffect(character);

            for (int i = staticEffects.Count - 1; i > -1; i--)
            {
                if (staticEffects[i] == null)
                    staticEffects.RemoveAt(i);

            }
        }

        public void RemoveStaticEffect(int effectID)
        {
            StaticCharacterEffect effect;

            for (int i = 0; i < staticEffects.Count; i++)
            {
                if (staticEffects[i] != null)
                {
                    if (staticEffects[i].staticEffectID == effectID)
                    {
                        effect = staticEffects[i];
                        effect.RemoveStaticEffect(character);
                        staticEffects.Remove(effect);
                    }
                }
            }

            for (int i = staticEffects.Count - 1; i > -1; i--)
            {
                if (staticEffects[i] == null)
                    staticEffects.RemoveAt(i);
            }
        }

        //timed effects
        //process all current timed effects
        public void ProcessTimedEffects()
        {
            for (int i = 0; i < timedEffects.Count; i++)
            {
                if (timedEffects[i] == null)
                    continue;

                timedEffects[i].ProcessEffect(character);
            }
        }

        //add a new effect
        public void AddTimedEffect(TimedCharacterEffect effect)
        {
            bool effectIsAlreadyOnCharacter = false;

            //if we already have the effect on us, we just reset the timer
            for (int i = 0; i < timedEffects.Count; i++)
            {
                if (timedEffects[i] == null)
                    continue;

                if (timedEffects[i].effectID == effect.effectID)
                {
                    effectIsAlreadyOnCharacter = true;
                    timedEffects[i].timeRemainingOnEffect = timedEffects[i].defaultLengthOfEffect;
                }
            }

            if (!effectIsAlreadyOnCharacter)
            {
                timedEffects.Add(effect);
                effect.timeRemainingOnEffect = effect.defaultLengthOfEffect;

                //process the first tick instantly
                effect.ProcessEffect(character);
            }
        }

        //remove an effect
        public void RemoveTimedEffect(int effectID)
        {
            TimedCharacterEffect effect;

            //find and remove
            for (int i = 0; i < timedEffects.Count; i++)
            {
                if (timedEffects[i] == null)
                    continue;

                if (timedEffects[i].effectID == effectID)
                {
                    effect = timedEffects[i];
                    effect.RemoveEffect(character);
                    timedEffects.Remove(effect);
                }
            }

            //remove null entries from list
            for (int i = 0; i < timedEffects.Count; i++)
            {
                if (timedEffects[i] == null)
                    timedEffects.RemoveAt(i);
            }
        }

        //checks if we are aready affected by an effect
        public TimedCharacterEffect CheckForTimedEffect(int effectID)
        {
            TimedCharacterEffect timedEffect = null;

            for (int i = 0; i < timedEffects.Count; i++)
            {
                if (timedEffects[i].effectID == effectID)
                {
                    timedEffect = timedEffects[i];
                    break;
                }
            }

            return timedEffect;
        }

        public void ProcessEffectDamage(int effectDamage)
        {
            if (!character.IsOwner)
                return;

            if (character.isDead.Value)
                return;

            character.characterNetworkManager.currentHealth.Value -= effectDamage;

            if (character.characterNetworkManager.currentHealth.Value >= 1)
                return;

            if (!character.characterNetworkManager.isBeingCriticallyDamaged.Value)
                character.characterAnimatorManager.PlayTargetActionAnimation("Dead_01", true);
            
            character.characterNetworkManager.isPoisoned.Value = false;
            character.isDead.Value = true;
        }
    }
}
