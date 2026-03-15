using UnityEngine;

namespace baodeag
{
    [CreateAssetMenu(menuName = "Character Effects/Instant Effects/Take Build Up Effect")]
    public class TakeBuildUpEffect : InstantCharacterEffect
    {
        [Header("Build Up")]
        [SerializeField] BuildUp buildUpType;
        public int buildUpAmount = 10;

        public override void ProcessEffect(CharacterManager character)
        {
            base.ProcessEffect(character);

            character.characterEffectsManager.AddBuildUps(buildUpType, buildUpAmount);

            switch (buildUpType)
            {
                case BuildUp.Poison:
                    CheckForPoisonedStatus(character);
                    break;
                case BuildUp.Bleed:
                    CheckForBloodLossStatus(character);
                    break;
                default:
                    break;
            }
        }

        private void CheckForPoisonedStatus(CharacterManager character)
        {
            if (character.characterNetworkManager.isPoisoned.Value)
                return;

            BuildUpEffect poisonBuildUp = character.characterEffectsManager.CheckForTimedEffect(WorldCharacterEffectsManager.instance.degradePoisonBuildUpEffect.effectID) as BuildUpEffect;

            if (poisonBuildUp == null)
            {
                poisonBuildUp = Instantiate(WorldCharacterEffectsManager.instance.degradePoisonBuildUpEffect);
                character.characterEffectsManager.AddTimedEffect(poisonBuildUp);
                poisonBuildUp.ProcessEffect(character);
            }

            if (character.characterNetworkManager.poisonBuildUp.Value > character.characterNetworkManager.buildUpCapacity.Value)
            {
                character.characterNetworkManager.poisonBuildUp.Value = 0;
                character.characterNetworkManager.isPoisoned.Value = true;

                //create the poisoned effect
                PoisonedEffect poison = Instantiate(WorldCharacterEffectsManager.instance.poisonedEffect);
                character.characterEffectsManager.AddTimedEffect(poison);

                PlayerManager player = character as PlayerManager;

                if (player == null)
                    return;

                if (!player.IsOwner)
                    return;


            }
        }

        private void CheckForBloodLossStatus(CharacterManager character)
        {

        }
    }
}
