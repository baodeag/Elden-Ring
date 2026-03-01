using Unity.Netcode;
using UnityEngine;
using Unity.Collections;

namespace baodeag
{
    public class AICharacterNetworkManager : CharacterNetworkManager
    {
        AICharacterManager aiCharacter;

        [Header("Sleep")]
        public NetworkVariable<bool> isAwake = new NetworkVariable<bool>
            (true,
            NetworkVariableReadPermission.Everyone,
            NetworkVariableWritePermission.Owner);
        public NetworkVariable<FixedString64Bytes> sleepingAnimation = new NetworkVariable<FixedString64Bytes>
            ("Sleep_01",
            NetworkVariableReadPermission.Everyone,
            NetworkVariableWritePermission.Owner);
        public NetworkVariable<FixedString64Bytes> wakingAnimation = new NetworkVariable<FixedString64Bytes>
            ("Wake_01",
            NetworkVariableReadPermission.Everyone,
            NetworkVariableWritePermission.Owner);

        protected override void Awake()
        {
            base.Awake();
            aiCharacter = GetComponent<AICharacterManager>();
        }

        public override void OnIsDeadChanged(bool oldStatus, bool newStatus)
        {
            base.OnIsDeadChanged(oldStatus, newStatus);

            if (aiCharacter.isDead.Value)
            {
                aiCharacter.aiCharacterInventoryManager.DropItem();
                aiCharacter.aiCharacterCombatManager.AwardRunesOnDeath(PlayerUIManager.instance.localPlayer);
            }
        }
    }
}
