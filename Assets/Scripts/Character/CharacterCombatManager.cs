using UnityEngine;
using Unity.Netcode;

namespace baodeag
{
    public class CharacterCombatManager : NetworkBehaviour
    {
        CharacterManager character;

        [Header("Last Attack Animation Performed")]
        public string lastAttackAnimationPerformed;

        [Header("Attack Target")]
        public CharacterManager currentTarget;

        [Header("Attack Type")]
        public AttackType currentAttackType;

        [Header("Lock On Transform")]
        public Transform lockOnTransform;

        protected virtual void Awake()
        {
            character = GetComponent<CharacterManager>();
        }

        public virtual void SetTarget(CharacterManager newTarget)
        {
            if (character.IsOwner)
            {
                if (newTarget != null)
                {
                    currentTarget = newTarget;
                    character.characterNetworkManager.currentTargetNetworkObjectID.Value = newTarget.GetComponent<NetworkObject>().NetworkObjectId;
                }
                else
                {
                    currentTarget = null;
                }
            }
        }
    }
}
