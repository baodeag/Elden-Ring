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
        [SerializeField] float fireBuildUpHitCooldown = 0.5f;   // tránh multi-hit trong 1 swing
        [SerializeField] float fireBuildUpDegradeRate = 5f;     // điểm/giây giảm khi không bị tấn công
        [SerializeField] float fireBuildUpDegradeDelay = 3f;    // giây chờ sau hit rồi mới bắt đầu giảm
        [SerializeField] float burnDamagePerTick = 5f;          // HP mất mỗi tick khi đang burning
        [SerializeField] float burnTickInterval = 1f;           // giây giữa mỗi tick damage
        [SerializeField] float burnDuration = 8f;               // tổng thời gian burning kéo dài

        float lastFireBuildUpHitTime = -999f;
        float burnDamageTickTimer = 0f;
        float burnTimeRemaining = 0f;

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

            if (applyFireBuildUp)
            {
                applyFireBuildUp = false;
                TakeBuildUpEffect buildUp = Instantiate(WorldCharacterEffectsManager.instance.takeFireBuildUpEffect);
                buildUp.buildUpAmount = DefaultFireBuildUpFromHit;
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

            if (!character.IsOwner)
                return;

            HandleFireBuildUpDegradation();
            HandleBurningDamage();
        }

        // Giảm dần fire build-up sau khi không bị tấn công
        private void HandleFireBuildUpDegradation()
        {
            if (character.characterNetworkManager.isBurning.Value)
                return;

            float buildUp = character.characterNetworkManager.fireBuildUp.Value;
            if (buildUp <= 0)
                return;

            // Chờ delay rồi mới bắt đầu giảm
            float timeSinceLastHit = Time.time - lastFireBuildUpHitTime;
            if (timeSinceLastHit < fireBuildUpDegradeDelay)
                return;

            float newBuildUp = Mathf.Max(0, buildUp - fireBuildUpDegradeRate * Time.deltaTime);
            character.characterNetworkManager.fireBuildUp.Value = newBuildUp;

            // Cập nhật UI
            if (PlayerUIManager.instance != null && PlayerUIManager.instance.localPlayer == character &&
                PlayerUIManager.instance.playerUIHudManager != null)
            {
                PlayerUIManager.instance.playerUIHudManager.SetNewFireBuildUpAmount(buildUp, newBuildUp);
            }
        }

        // Trừ máu player theo tick khi đang burning
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

            // Đếm ngược thời gian burning
            burnTimeRemaining -= Time.deltaTime;
            if (burnTimeRemaining <= 0)
            {
                character.characterNetworkManager.isBurning.Value = false;
                Debug.Log("[Burning] Burning expired.");
                return;
            }

            burnDamageTickTimer += Time.deltaTime;
            if (burnDamageTickTimer < burnTickInterval)
                return;

            burnDamageTickTimer = 0f;

            // Trừ máu
            int burnDamage = Mathf.RoundToInt(burnDamagePerTick);
            int newHealth = Mathf.Max(0, character.characterNetworkManager.currentHealth.Value - burnDamage);
            character.characterNetworkManager.currentHealth.Value = newHealth;

            Debug.Log($"[Burning] Burn tick: -{burnDamagePerTick} HP → {newHealth} (còn {burnTimeRemaining:F1}s)");

            if (newHealth <= 0)
                character.characterNetworkManager.isBurning.Value = false;
        }

        public void ApplyFireBuildUpFromHit(int buildUpAmount)
        {
            if (!character.IsOwner)
                return;

            // Cooldown theo thời gian thực, tránh multi-hit từ cùng 1 swing
            if (Time.time - lastFireBuildUpHitTime < fireBuildUpHitCooldown)
                return;

            lastFireBuildUpHitTime = Time.time;

            character.characterEffectsManager.AddBuildUps(BuildUp.Fire, buildUpAmount);
            Debug.Log($"[FireBuildUp] Added {buildUpAmount}. fireBuildUp={character.characterNetworkManager.fireBuildUp.Value}, capacity={character.characterNetworkManager.buildUpCapacity.Value}");

            // Kích hoạt burning nếu build-up đầy
            if (!character.characterNetworkManager.isBurning.Value &&
                character.characterNetworkManager.fireBuildUp.Value >= character.characterNetworkManager.buildUpCapacity.Value)
            {
                character.characterNetworkManager.fireBuildUp.Value = 0;
                character.characterNetworkManager.isBurning.Value = true;
                burnTimeRemaining = burnDuration;
                burnDamageTickTimer = 0f;
                Debug.Log($"[Burning] Burning activated! Duration={burnDuration}s");
            }

            ForceRefreshFireBuildUpBar();
        }

        private void ForceRefreshFireBuildUpBar()
        {
            if (PlayerUIManager.instance == null || PlayerUIManager.instance.localPlayer != character)
            {
                Debug.Log("[FireBuildUp] ForceRefresh skipped: PlayerUIManager null or wrong player");
                return;
            }

            if (PlayerUIManager.instance.playerUIHudManager == null || character.characterNetworkManager == null)
            {
                Debug.Log("[FireBuildUp] ForceRefresh skipped: HudManager or networkManager null");
                return;
            }

            float currentBuildUp = character.characterNetworkManager.fireBuildUp.Value;
            float capacity = character.characterNetworkManager.buildUpCapacity.Value;
            Debug.Log($"[FireBuildUp] ForceRefreshFireBuildUpBar: buildUp={currentBuildUp}, capacity={capacity}");

            // Set max FIRST, then set current value so bar becomes visible correctly
            PlayerUIManager.instance.playerUIHudManager.SetMaxBuildUpValue(Mathf.RoundToInt(capacity));
            PlayerUIManager.instance.playerUIHudManager.SetNewFireBuildUpAmount(0, currentBuildUp);
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
