using UnityEngine;

namespace baodeag
{
    public class AIBossCharacterNetworkManager : AICharacterNetworkManager
    {
        AIBossCharacterManager aiBossCharacter;

        protected override void Awake()
        {
            base.Awake();

            aiBossCharacter = GetComponent<AIBossCharacterManager>();
        }

        public override void OnHPChanged(int oldValue, int newValue)
        {
            base.OnHPChanged(oldValue, newValue);

            if (aiBossCharacter != null &&
                currentHealth.Value <= 0 &&
                !aiBossCharacter.isDead.Value)
            {
                Debug.Log($"[BossFlow] HP reached zero for '{aiBossCharacter.name}' old={oldValue} new={newValue} isServer={IsServer} isOwner={aiBossCharacter.IsOwner}");

                if (IsServer)
                    aiBossCharacter.StartCoroutine(aiBossCharacter.ProcessDeathEvent());

                return;
            }

            if (aiBossCharacter.IsOwner)
            {
                if (currentHealth.Value <= 0)
                    return;

                float healthNeededForShift = maxHealth.Value * (aiBossCharacter.minimumHealthPercentageToShift / 100);

                if (currentHealth.Value <= healthNeededForShift)
                {
                    aiBossCharacter.PhaseShift();
                }
            }
        }

        public override void OnIsActiveChanged(bool oldStatus, bool newStatus)
        {
            if (aiBossCharacter == null)
            {
                base.OnIsActiveChanged(oldStatus, newStatus);
                return;
            }

            // Regular AI can be fully deactivated outside activation range, but bosses
            // should remain visible in the arena unless they have actually been defeated.
            if (!aiBossCharacter.hasBeenDefeated.Value)
            {
                gameObject.SetActive(true);
                return;
            }

            base.OnIsActiveChanged(oldStatus, newStatus);
        }
    }
}
