using UnityEngine;

namespace baodeag
{
    public class PlayerEffectsManager : CharacterEffectsManager
    {
        [Header("Debug delete later")]
        [SerializeField] bool applyPoisonBuildUp = false;
        [SerializeField] bool applyBleedBuildUp = false;
        [SerializeField] bool applyFrostBuildUp = false;

        protected override void Update()
        {
            base.Update();

            if (applyPoisonBuildUp)
            {
                applyPoisonBuildUp = false;
                TakeBuildUpEffect buildUp = Instantiate(WorldCharacterEffectsManager.instance.takePoisonBuildUpEffect);
                buildUp.buildUpAmount = 25;
                character.characterEffectsManager.ProcessInstantEffect(buildUp);
            }

            if (applyBleedBuildUp)
            {
                applyBleedBuildUp = false;
                TakeBuildUpEffect buildUp = Instantiate(WorldCharacterEffectsManager.instance.takeBleedBuildUpEffect);
                buildUp.buildUpAmount = 25;
                character.characterEffectsManager.ProcessInstantEffect(buildUp);
            }

            if (applyFrostBuildUp)
            {
                applyFrostBuildUp = false;
                TakeBuildUpEffect buildUp = Instantiate(WorldCharacterEffectsManager.instance.takeFrostBuildUpEffect);
                buildUp.buildUpAmount = 25;
                character.characterEffectsManager.ProcessInstantEffect(buildUp);
            }
        }
    }
}
