using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

namespace baodeag
{
    public class WorldCharacterEffectsManager : MonoBehaviour
    {
        public static WorldCharacterEffectsManager instance;

        [Header("VFX")]
        public GameObject bloodSplatterVFX;
        public GameObject criticalBloodSplatterVFX;
        public GameObject healingFlaskVFX;
        public GameObject deadSpotVFX;
        public GameObject poisonedVFX;

        [Header("Damage")]
        public TakeDamageEffect takeDamageEffect;
        public TakeBlockedDamageEffect takeBlockedDamageEffect;
        public TakeCriticalDamageEffect takeCriticalDamageEffect;

        [Header("Status Effects")]
        public PoisonedEffect poisonedEffect;

        [Header("Take Build Ups")]
        public TakeBuildUpEffect takePoisonBuildUpEffect;
        public TakeBuildUpEffect takeBleedBuildUpEffect;

        [Header("Degrade Build Ups")]
        public BuildUpEffect degradePoisonBuildUpEffect;

        [Header("Two Hand")]
        public TwoHandingEffect twoHandingEffect;

        [Header("Instant Effects")]
        [SerializeField] List<InstantCharacterEffect> instantEffects;

        [Header("Static Effects")]
        [SerializeField] List<StaticCharacterEffect> staticEffects;

        [Header("Timed Effects")]
        [SerializeField] List<TimedCharacterEffect> timedEffects;

        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }
            else
            {
                Destroy(gameObject);
            }

            GenerateEffectIDs();
        }

        private void GenerateEffectIDs()
        {
            for (int i = 0; i < instantEffects.Count; i++)
            {
                instantEffects[i].instantEffectID = i;
            }

            for (int i = 0; i < staticEffects.Count; i++)
            {
                staticEffects[i].staticEffectID = i;
            }

            for (int i = 0; i < timedEffects.Count; i++)
            {
                timedEffects[i].effectID = i;
            }
        }
    }
}
