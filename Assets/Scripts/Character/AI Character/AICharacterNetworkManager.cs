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

        public override void OnLockOnTargetIDChange(ulong oldID, ulong newID)
        {
            base.OnLockOnTargetIDChange(oldID, newID);

            //if your character has a target, disable the interactable collider
            if (aiCharacter.aiCharacterCombatManager.currentTarget != null && aiCharacter.aiCharacterSoundFXManager.interactableDialogueObject  != null)
                aiCharacter.aiCharacterSoundFXManager.interactableDialogueObject.SetActive(false);

            //optionally re-anable it when the target is gone
            if (aiCharacter.aiCharacterCombatManager.currentTarget == null && aiCharacter.aiCharacterSoundFXManager.interactableDialogueObject != null)
                aiCharacter.aiCharacterSoundFXManager.interactableDialogueObject.SetActive(true);
        }
    }
}
