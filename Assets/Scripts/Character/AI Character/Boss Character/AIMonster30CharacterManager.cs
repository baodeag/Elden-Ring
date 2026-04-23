using UnityEngine;

namespace baodeag
{
    public class AIMonster30CharacterManager : AIBossCharacterManager
    {
        [HideInInspector] public AIMonster30CombatManager monster30CombatManager;

        protected override void Awake()
        {
            base.Awake();

            monster30CombatManager = GetComponent<AIMonster30CombatManager>();
        }

        public override void OnNetworkSpawn()
        {
            base.OnNetworkSpawn();

            if (!IsOwner)
                return;

            aiCharacterNetworkManager.isAwake.Value = true;
            currentState = idle;
        }
    }
}
