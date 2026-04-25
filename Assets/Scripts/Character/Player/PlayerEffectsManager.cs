using UnityEngine;
using System.Collections.Generic;

namespace baodeag
{
    public class PlayerEffectsManager : CharacterEffectsManager
    {
        public const int DefaultFireBuildUpFromHit = 25;

        [Header("Debug delete later")]
        [SerializeField] bool applyPoisonBuildUp = false;
        [SerializeField] bool applyFireBuildUp = false;
        [SerializeField] bool applyBleedBuildUp = false;
        [SerializeField] bool applyFrostBuildUp = false;

        [Header("Fire Build-Up")]
        [SerializeField] float fireBuildUpHitCooldown = 0.5f;
        [SerializeField] float fireBuildUpDegradeRate = 5f;
        [SerializeField] float fireBuildUpDegradeDelay = 3f;
        [SerializeField] float burnDamagePerTick = 5f;
        [SerializeField] float burnTickInterval = 1f;
        [SerializeField] float burnDuration = 8f;

        float lastFireBuildUpHitTime = -999f;
        float burnDamageTickTimer = 0f;
        float burnTimeRemaining = 0f;

        protected override void Update()
        {
            base.Update();

            ProcessDebugBuildUps();

            if (!character.IsOwner)
                return;

            HandleFireBuildUpDegradation();
            HandleBurningDamage();
        }

        private void ProcessDebugBuildUps()
        {
            if (applyPoisonBuildUp)
            {
                applyPoisonBuildUp = false;
                ApplyDebugBuildUp(WorldCharacterEffectsManager.instance.takePoisonBuildUpEffect, 25);
            }

            if (applyFireBuildUp)
            {
                applyFireBuildUp = false;
                ApplyFireBuildUp(DefaultFireBuildUpFromHit);
            }

            if (applyBleedBuildUp)
            {
                applyBleedBuildUp = false;
                ApplyDebugBuildUp(WorldCharacterEffectsManager.instance.takeBleedBuildUpEffect, 25);
            }

            if (applyFrostBuildUp)
            {
                applyFrostBuildUp = false;
                ApplyDebugBuildUp(WorldCharacterEffectsManager.instance.takeFrostBuildUpEffect, 25);
            }
        }

        private void ApplyDebugBuildUp(TakeBuildUpEffect buildUpTemplate, int buildUpAmount)
        {
            if (buildUpTemplate == null)
                return;

            TakeBuildUpEffect buildUp = Instantiate(buildUpTemplate);
            buildUp.buildUpAmount = buildUpAmount;
            character.characterEffectsManager.ProcessInstantEffect(buildUp);
        }

        private void HandleFireBuildUpDegradation()
        {
            if (character.characterNetworkManager.isBurning.Value)
                return;

            float buildUp = character.characterNetworkManager.fireBuildUp.Value;
            if (buildUp <= 0)
                return;

            if (Time.time - lastFireBuildUpHitTime < fireBuildUpDegradeDelay)
                return;

            character.characterNetworkManager.fireBuildUp.Value = Mathf.Max(0, buildUp - fireBuildUpDegradeRate * Time.deltaTime);
            RefreshFireBuildUpBar();
        }

        private void HandleBurningDamage()
        {
            if (!character.characterNetworkManager.isBurning.Value)
            {
                burnDamageTickTimer = 0f;
                burnTimeRemaining = 0f;
                return;
            }

            if (character.isDead.Value)
            {
                character.characterNetworkManager.isBurning.Value = false;
                return;
            }

            burnTimeRemaining -= Time.deltaTime;
            if (burnTimeRemaining <= 0)
            {
                character.characterNetworkManager.isBurning.Value = false;
                return;
            }

            burnDamageTickTimer += Time.deltaTime;
            if (burnDamageTickTimer < burnTickInterval)
                return;

            burnDamageTickTimer = 0f;

            int burnDamage = Mathf.RoundToInt(burnDamagePerTick);
            int newHealth = Mathf.Max(0, character.characterNetworkManager.currentHealth.Value - burnDamage);
            character.characterNetworkManager.currentHealth.Value = newHealth;

            if (newHealth <= 0)
                character.characterNetworkManager.isBurning.Value = false;
        }

        public void ApplyFireBuildUpFromHit(int buildUpAmount)
        {
            ApplyFireBuildUp(buildUpAmount, true);
        }

        public void ApplyFireBuildUp(int buildUpAmount, bool useHitCooldown = false)
        {
            if (!character.IsOwner)
                return;

            if (useHitCooldown && Time.time - lastFireBuildUpHitTime < fireBuildUpHitCooldown)
                return;

            lastFireBuildUpHitTime = Time.time;
            character.characterEffectsManager.AddBuildUps(BuildUp.Fire, buildUpAmount);

            TryActivateBurningFromFireBuildUp();
            RefreshFireBuildUpBar();
        }

        private void TryActivateBurningFromFireBuildUp()
        {
            if (character.characterNetworkManager.isBurning.Value)
                return;

            if (character.characterNetworkManager.fireBuildUp.Value < character.characterNetworkManager.buildUpCapacity.Value)
                return;

            character.characterNetworkManager.fireBuildUp.Value = 0;
            character.characterNetworkManager.isBurning.Value = true;
            burnTimeRemaining = burnDuration;
            burnDamageTickTimer = 0f;
        }

        private void RefreshFireBuildUpBar()
        {
            if (PlayerUIManager.instance == null || PlayerUIManager.instance.localPlayer != character)
                return;

            if (PlayerUIManager.instance.playerUIHudManager == null || character.characterNetworkManager == null)
                return;

            PlayerUIManager.instance.playerUIHudManager.SetMaxBuildUpValue(Mathf.RoundToInt(character.characterNetworkManager.buildUpCapacity.Value));
            PlayerUIManager.instance.playerUIHudManager.SetNewFireBuildUpAmount(0, character.characterNetworkManager.fireBuildUp.Value);
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

            if (PlayerUIManager.instance != null && PlayerUIManager.instance.localPlayer == character)
                PlayerUIManager.instance.playerUIHudManager?.ClearActiveBuffs();

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

                if (PlayerUIManager.instance != null && PlayerUIManager.instance.localPlayer == character)
                    PlayerUIManager.instance.playerUIHudManager?.ShowActiveBuff(buffItem);
            }
        }
    }
}
