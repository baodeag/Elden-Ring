using UnityEngine;
using Unity.Netcode;

namespace baodeag
{
    public class AIMonster33BossCharacterNetworkManager : AICharacterNetworkManager
    {
        public NetworkVariable<bool> isPowerUpPhaseActive = new NetworkVariable<bool>
            (false,
            NetworkVariableReadPermission.Everyone,
            NetworkVariableWritePermission.Owner);

        AIMonster33CharacterManager monster33BossCharacter;

        protected override void Awake()
        {
            base.Awake();

            monster33BossCharacter = GetComponent<AIMonster33CharacterManager>();
        }

        public override void OnHPChanged(int oldValue, int newValue)
        {
            base.OnHPChanged(oldValue, newValue);

            if (monster33BossCharacter == null || !monster33BossCharacter.IsOwner)
                return;

            if (currentHealth.Value <= 0 || monster33BossCharacter.hasActivatedPowerUpPhase)
                return;

            float healthNeededForShift = maxHealth.Value * (monster33BossCharacter.minimumHealthPercentageToShift / 100f);

            if (currentHealth.Value <= healthNeededForShift)
            {
                monster33BossCharacter.TryActivatePowerUpPhase();
            }
        }

        public override void OnIsActiveChanged(bool oldStatus, bool newStatus)
        {
            if (monster33BossCharacter == null)
            {
                base.OnIsActiveChanged(oldStatus, newStatus);
                return;
            }

            if (!monster33BossCharacter.hasBeenDefeated.Value)
            {
                gameObject.SetActive(true);
                return;
            }

            base.OnIsActiveChanged(oldStatus, newStatus);
        }

        [ClientRpc]
        public void ActivatePowerUpPhaseFXClientRpc()
        {
            if (monster33BossCharacter == null)
                monster33BossCharacter = GetComponent<AIMonster33CharacterManager>();

            if (monster33BossCharacter == null)
                return;

            monster33BossCharacter.monster33CombatManager?.ApplyPowerUpBuff();
            monster33BossCharacter.phase2FireController?.ActivateAfterPowerUpAnimation();
        }
    }
}
