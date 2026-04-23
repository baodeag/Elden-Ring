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

        public void ForceEndCurrentAction()
        {
            isPerformingAction = false;

            if (characterAnimatorManager != null)
                characterAnimatorManager.applyRootMotion = false;

            if (characterLocomotionManager != null)
            {
                characterLocomotionManager.canRotate = true;
                characterLocomotionManager.canMove = true;
                characterLocomotionManager.canRun = true;
                characterLocomotionManager.canRoll = true;
                characterLocomotionManager.isRolling = false;
            }

            if (characterCombatManager != null)
            {
                characterCombatManager.DisableCanDoCombo();
                characterCombatManager.DisableCanDoRollingAttack();
                characterCombatManager.DisableCanDoBackstepAttack();
                characterCombatManager.CloseAllDamageColliders();
            }

            if (IsOwner && characterNetworkManager != null)
            {
                characterNetworkManager.isJumping.Value = false;
                characterNetworkManager.isInvulnerable.Value = false;
                characterNetworkManager.isAttacking.Value = false;
                characterNetworkManager.isRipostable.Value = false;
                characterNetworkManager.isBeingCriticallyDamaged.Value = false;
                characterNetworkManager.isParrying.Value = false;
                characterNetworkManager.isRolling.Value = false;
            }

            if (animator != null)
                animator.CrossFade("Empty", 0.05f);
        }
    }
}
