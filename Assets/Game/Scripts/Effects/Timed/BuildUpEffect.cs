using UnityEngine;

namespace baodeag
{
    [CreateAssetMenu(menuName = "Character Effects/Timed Effects/Build Up Effect")]
    public class BuildUpEffect : TimedCharacterEffect
    {
        [Header("Type")]
        public BuildUp buildUpType;

        [Header("Degradation")]
        public int buildUpAmountDegradation = -1;
        public float buildUpRemaining = 1;

        public override void ProcessEffect(CharacterManager character)
        {
            if (!character.IsOwner)
                return;

            buildUpRemaining = GetCurrentBuildUpValue(character);

            //if the build up fades out, or reaches its climax remove this timed effect
            if (buildUpRemaining < 0 || 
                buildUpRemaining >= character.characterStatsManager.CalculateBuildUpCapacityBasedOnVitalityLevel(character.characterNetworkManager.vigor.Value))
            {
                character.characterEffectsManager.RemoveTimedEffect(effectID);
                return;
            }

            DegradeBuildUp(character);
        }

        public override void RemoveEffect(CharacterManager character)
        {
            base.RemoveEffect(character);
        }

        private void DegradeBuildUp(CharacterManager character)
        {
            character.characterStatsManager.DegradeBuildUps(buildUpType, buildUpAmountDegradation, this);
        }

        private float GetCurrentBuildUpValue(CharacterManager character)
        {
            switch (buildUpType)
            {
                case BuildUp.Poison:
                    return character.characterNetworkManager.poisonBuildUp.Value;
                case BuildUp.Fire:
                    return character.characterNetworkManager.fireBuildUp.Value;
                case BuildUp.Bleed:
                    return character.characterNetworkManager.bleedBuildUp.Value;
                case BuildUp.Frost:
                    return character.characterNetworkManager.frostBiteBuildUp.Value;
                default:
                    return buildUpRemaining;
            }
        }
    }
}
