using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;

namespace baodeag
{
    public class CharacterEffectsManager : MonoBehaviour
    {
        //process instant effect (take dmg, heal)

        //process timed effects (poison, burn, buff)

        //process static effects (add/remove buffs)

        CharacterManager character;

        [Header("Current Active FX")]
        public GameObject activeQuickSlotItemFX;
        public GameObject activeSpellWarmUpFX;
        public GameObject activeDrawnProjectileFX;

        [Header("VFX")]
        [SerializeField] GameObject bloodSplatterVFX;
        [SerializeField] GameObject criticalBloodSplatterVFX;

        [Header("Static Effects")]
        public List<StaticCharacterEffect> staticEffects = new List<StaticCharacterEffect>();

        protected virtual void Awake()
        {
            character = GetComponent<CharacterManager>();
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
    }
}
