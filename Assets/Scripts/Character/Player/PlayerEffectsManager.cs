using UnityEngine;

namespace baodeag
{
    public class PlayerEffectsManager : CharacterEffectsManager
    {
        [Header("Debug Delete later")]
        [SerializeField] InstantCharacterEffect effectToTest;
        [SerializeField] bool processEffect = false;

        private void Update()
        {
            if (processEffect)
            {
                processEffect = false;
                InstantCharacterEffect effect = Instantiate(effectToTest);
                ProcessInstantEffect(effectToTest);
            }
        }
    }
}
