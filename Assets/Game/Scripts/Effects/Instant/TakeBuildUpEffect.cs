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
                case BuildUp.Fire:
                    CheckForBurningStatus(character);
                    break;
                case BuildUp.Bleed:
                    CheckForBloodLossStatus(character);
                    break;
                case BuildUp.Frost:
                    CheckForFrostBiteStatus(character);
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
                poisonBuildUp.buildUpRemaining = character.characterNetworkManager.poisonBuildUp.Value;
                character.characterEffectsManager.AddTimedEffect(poisonBuildUp);
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

        private void CheckForBurningStatus(CharacterManager character)
        {
            if (character.characterNetworkManager.isBurning.Value)
                return;

            BuildUpEffect fireBuildUp = character.characterEffectsManager.CheckForTimedEffect(WorldCharacterEffectsManager.instance.degradeFireBuildUpEffect.effectID) as BuildUpEffect;

            if (fireBuildUp == null)
            {
                fireBuildUp = Instantiate(WorldCharacterEffectsManager.instance.degradeFireBuildUpEffect);
                fireBuildUp.buildUpRemaining = character.characterNetworkManager.fireBuildUp.Value;
                character.characterEffectsManager.AddTimedEffect(fireBuildUp);
            }

            if (character.characterNetworkManager.fireBuildUp.Value >= character.characterNetworkManager.buildUpCapacity.Value)
            {
                character.characterNetworkManager.fireBuildUp.Value = 0;
                character.characterNetworkManager.isBurning.Value = true;

                BurningEffect burning = Instantiate(WorldCharacterEffectsManager.instance.burningEffect);
                character.characterEffectsManager.AddTimedEffect(burning);
            }
        }

        private void CheckForBloodLossStatus(CharacterManager character)
        {
            BuildUpEffect bleedBuildUp = character.characterEffectsManager.CheckForTimedEffect(WorldCharacterEffectsManager.instance.degradeBleedBuildUpEffect.effectID) as BuildUpEffect;

            if (bleedBuildUp == null)
            {
                bleedBuildUp = Instantiate(WorldCharacterEffectsManager.instance.degradeBleedBuildUpEffect);
                bleedBuildUp.buildUpRemaining = character.characterNetworkManager.bleedBuildUp.Value;
                character.characterEffectsManager.AddTimedEffect(bleedBuildUp);
            }

            if (character.characterNetworkManager.bleedBuildUp.Value > character.characterNetworkManager.buildUpCapacity.Value)
            {
                character.characterNetworkManager.bleedBuildUp.Value = 0;
                character.characterNetworkManager.isBleeding.Value = true;
                //character.characterNetworkManager.BleedCharacterServerRpc();

                //create the poisoned effect
                BloodLossEffect bloodLoss = Instantiate(WorldCharacterEffectsManager.instance.bloodLossEffect);
                character.characterEffectsManager.ProcessInstantEffect(bloodLoss);

                PlayerManager player = character as PlayerManager;

                if (player == null)
                    return;

                if (!player.IsOwner)
                    return;


            }
        }

        private void CheckForFrostBiteStatus(CharacterManager character)
        {
            if (character.characterNetworkManager.isFrostBitten.Value)
                return;

            BuildUpEffect frostBuildUp = character.characterEffectsManager.CheckForTimedEffect(WorldCharacterEffectsManager.instance.degradeFrostBiteBuildUpEffect.effectID) as BuildUpEffect;

            if (frostBuildUp == null)
            {
                frostBuildUp = Instantiate(WorldCharacterEffectsManager.instance.degradeFrostBiteBuildUpEffect);
                frostBuildUp.buildUpRemaining = character.characterNetworkManager.frostBiteBuildUp.Value;
                character.characterEffectsManager.AddTimedEffect(frostBuildUp);
            }

            if (character.characterNetworkManager.frostBiteBuildUp.Value > character.characterNetworkManager.buildUpCapacity.Value)
            {
                character.characterNetworkManager.frostBiteBuildUp.Value = 0;
                character.characterNetworkManager.isFrostBitten.Value = true;

                //create the poisoned effect
                FrostBiteEffect frostBite = Instantiate(WorldCharacterEffectsManager.instance.frostBiteEffect);
                character.characterEffectsManager.AddTimedEffect(frostBite);

                PlayerManager player = character as PlayerManager;

                if (player == null)
                    return;

                if (!player.IsOwner)
                    return;


            }
        }
    }
}
