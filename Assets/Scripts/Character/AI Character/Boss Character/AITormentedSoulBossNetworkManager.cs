using UnityEngine;

namespace baodeag
{
    public class AITormentedSoulBossNetworkManager : AIBossCharacterNetworkManager
    {
        AITormentedSoulBossCharacterManager tormentedSoulBossCharacter;

        protected override void Awake()
        {
            base.Awake();
            tormentedSoulBossCharacter = GetComponent<AITormentedSoulBossCharacterManager>();
        }

        public override void OnHPChanged(int oldValue, int newValue)
        {
            base.OnHPChanged(oldValue, newValue);

            if (tormentedSoulBossCharacter == null || !tormentedSoulBossCharacter.IsOwner)
                return;

            if (currentHealth.Value <= 0)
                return;

            tormentedSoulBossCharacter.deathMoonSlash?.EvaluatePowerUpStateFromBossNetwork();
        }

        public override void OnIsActiveChanged(bool oldStatus, bool newStatus)
        {
            if (tormentedSoulBossCharacter == null)
            {
                base.OnIsActiveChanged(oldStatus, newStatus);
                return;
            }

            if (!tormentedSoulBossCharacter.hasBeenDefeated.Value)
            {
                gameObject.SetActive(true);
                return;
            }

            base.OnIsActiveChanged(oldStatus, newStatus);
        }
    }
}
