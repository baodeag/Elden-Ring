using baodead;
using UnityEngine;

namespace baodeag
{
    public class AIDurkCharacterManager : AIBossCharacterManager
    {
        [HideInInspector] public AIDurkSoundFXManager durkSoundFXManager;
        [HideInInspector] public AIDurkCombatManager durkCombatManager;

        protected override void Awake()
        {
            base.Awake();

            durkSoundFXManager = GetComponent<AIDurkSoundFXManager>();
            durkCombatManager = GetComponent<AIDurkCombatManager>();
        }
    }
}
