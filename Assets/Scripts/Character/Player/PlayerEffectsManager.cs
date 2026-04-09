using UnityEngine;
using System.Collections.Generic;

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

        public void SaveActiveBuffs(List<SerializableActiveBuff> activeBuffs)
        {
            if (activeBuffs == null)
                return;

            activeBuffs.Clear();

            for (int i = 0; i < timedEffects.Count; i++)
            {
                if (timedEffects[i] is not PlayerStatBuffTimedEffect buffEffect)
                    continue;

                if (buffEffect.sourceItemID < 0 || buffEffect.timeRemainingOnEffect <= 0)
                    continue;

                activeBuffs.Add(new SerializableActiveBuff
                {
                    sourceItemID = buffEffect.sourceItemID,
                    timeRemaining = buffEffect.timeRemainingOnEffect
                });
            }
        }

        public void LoadActiveBuffs(List<SerializableActiveBuff> activeBuffs)
        {
            if (!character.IsOwner || activeBuffs == null)
                return;

            for (int i = 0; i < activeBuffs.Count; i++)
            {
                SerializableActiveBuff savedBuff = activeBuffs[i];

                if (savedBuff == null || savedBuff.sourceItemID < 0)
                    continue;

                BuffCharmItem buffItem = WorldItemDatabase.Instance.GetQuickSlotItemByID(savedBuff.sourceItemID) as BuffCharmItem;

                if (buffItem == null)
                    continue;

                PlayerStatBuffTimedEffect effect = buffItem.CreateEffectInstance();
                effect.timeRemainingOnEffect = Mathf.Max(1f, savedBuff.timeRemaining + 1f);
                AddTimedEffect(effect);
            }
        }
    }
}
