using UnityEngine;

namespace baodeag
{
    public class AIKnightBossNetworkManager : AIBossCharacterNetworkManager
    {
        AIKnightBossCharacterManager knightBossCharacter;

        protected override void Awake()
        {
            base.Awake();
            knightBossCharacter = GetComponent<AIKnightBossCharacterManager>();
        }

        public override void OnHPChanged(int oldValue, int newValue)
        {
            base.OnHPChanged(oldValue, newValue);

            if (knightBossCharacter == null || !knightBossCharacter.IsOwner)
                return;

            if (currentHealth.Value <= 0)
                return;

            knightBossCharacter.twinMoonSkill?.EvaluatePowerUpStateFromBossNetwork();
        }

        public override void OnIsActiveChanged(bool oldStatus, bool newStatus)
        {
            if (knightBossCharacter == null)
            {
                base.OnIsActiveChanged(oldStatus, newStatus);
                return;
            }

            if (!knightBossCharacter.hasBeenDefeated.Value)
            {
                gameObject.SetActive(true);
                return;
            }

            base.OnIsActiveChanged(oldStatus, newStatus);
        }
    }
}
