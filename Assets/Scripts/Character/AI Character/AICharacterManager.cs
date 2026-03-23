using UnityEngine;
using UnityEngine.AI;
using Unity.Netcode;
using System.Collections;

namespace baodeag
{
    public class AICharacterManager : CharacterManager
    {
        [Header("Character Name")]
        public string characterName = "";

        [HideInInspector] public AICharacterCombatManager aiCharacterCombatManager;
        [HideInInspector] public AICharacterNetworkManager aiCharacterNetworkManager;
        [HideInInspector] public AICharacterLocomotionManager aiCharacterLocomotionManager;
        [HideInInspector] public AICharacterInventoryManager aiCharacterInventoryManager;
        [HideInInspector] public AICharacterSoundFXManager aiCharacterSoundFXManager;

        [Header("Nav Mesh Agent")]
        public NavMeshAgent navMeshAgent;

        [Header("Current State")]
        public AIState currentState;
        [HideInInspector] public bool hasManuallySwitchedState = false;

        [Header("States")]
        public IdleState idle;
        public PursueTargetState pursueTarget;
        public CombatStanceState combatStance;
        public AttackState attack;
        public InvestigateSoundState investigateSound;

        [Header("Activation Beacon")]
        protected AIActivationBeacon beacon;
        private bool deadStateApplied;
        private DamageCollider[] damageColliders;
        private Coroutine deadPoseRoutine;
        private Coroutine coopDeathDespawnRoutine;

        protected override void Awake()
        {
            base.Awake();

            aiCharacterCombatManager = GetComponent<AICharacterCombatManager>();
            aiCharacterNetworkManager = GetComponent<AICharacterNetworkManager>();
            aiCharacterLocomotionManager = GetComponent<AICharacterLocomotionManager>();
            aiCharacterInventoryManager = GetComponent<AICharacterInventoryManager>();
            aiCharacterSoundFXManager = GetComponent<AICharacterSoundFXManager>();

            navMeshAgent = GetComponentInChildren<NavMeshAgent>();
            damageColliders = GetComponentsInChildren<DamageCollider>(true);
        }

        protected override void Start()
        {
            base.Start();

            animator.keepAnimatorStateOnDisable = true;
        }

        public override void OnNetworkSpawn()
        {
            base.OnNetworkSpawn();

            if (IsOwner)
            {
                idle = Instantiate(idle);
                pursueTarget = Instantiate(pursueTarget);
                combatStance = Instantiate(combatStance);
                attack = Instantiate(attack);
                investigateSound = Instantiate(investigateSound);
                currentState = idle;
            }

            aiCharacterNetworkManager.currentHealth.OnValueChanged += aiCharacterNetworkManager.OnHPChanged;
            aiCharacterNetworkManager.isBlocking.OnValueChanged += aiCharacterNetworkManager.OnIsBlockingChanged;
            aiCharacterNetworkManager.isPoisoned.OnValueChanged += aiCharacterNetworkManager.OnIsPoisonedChanged;
            aiCharacterNetworkManager.isBleeding.OnValueChanged += aiCharacterNetworkManager.OnIsBleedingChanged;
            aiCharacterNetworkManager.isFrostBitten.OnValueChanged += aiCharacterNetworkManager.OnIsFrostBittenChanged;
            aiCharacterNetworkManager.isFrozen.OnValueChanged += aiCharacterNetworkManager.OnIsFrozenChanged;

            if (!aiCharacterNetworkManager.isAwake.Value)
                animator.Play(aiCharacterNetworkManager.sleepingAnimation.Value.ToString());

            if (isDead.Value)
                QueueLateJoinDeadPose();

            CreateActivationBeacon();

            if (!IsOwner)
                aiCharacterNetworkManager.OnIsPoisonedChanged(false, aiCharacterNetworkManager.isPoisoned.Value);
        }

        public override void OnNetworkDespawn()
        {
            base.OnNetworkDespawn();

            aiCharacterNetworkManager.currentHealth.OnValueChanged -= aiCharacterNetworkManager.OnHPChanged;
            aiCharacterNetworkManager.isBlocking.OnValueChanged -= aiCharacterNetworkManager.OnIsBlockingChanged;
            aiCharacterNetworkManager.isPoisoned.OnValueChanged -= aiCharacterNetworkManager.OnIsPoisonedChanged;
            aiCharacterNetworkManager.isBleeding.OnValueChanged -= aiCharacterNetworkManager.OnIsBleedingChanged;
            aiCharacterNetworkManager.isFrostBitten.OnValueChanged -= aiCharacterNetworkManager.OnIsFrostBittenChanged;
            aiCharacterNetworkManager.isFrozen.OnValueChanged -= aiCharacterNetworkManager.OnIsFrozenChanged;
        }

        protected override void OnEnable()
        {
            base.OnEnable();

            if (characterUIManager.hasFloatingHPBar)
                characterNetworkManager.currentHealth.OnValueChanged += characterUIManager.OnHPChanged;
        }

        protected override void OnDisable()
        {
            base.OnDisable();

            if (characterUIManager.hasFloatingHPBar)
                characterNetworkManager.currentHealth.OnValueChanged -= characterUIManager.OnHPChanged;
        }

        public override void OnDestroy()
        {
            base.OnDestroy();

            if (WorldAIManager.instance != null)
                WorldAIManager.instance.RemoveCharacterFromSpawnedCharacterList(this);

            if (beacon != null)
                Destroy(beacon);
        }

        protected override void Update()
        {
            base.Update();

            if (isDead.Value)
            {
                ApplyDeadState();
                return;
            }

            if (deadPoseRoutine != null)
            {
                StopCoroutine(deadPoseRoutine);
                deadPoseRoutine = null;
            }

            deadStateApplied = false;
            animator.SetBool("isDead", false);
            animator.speed = 1;

            aiCharacterCombatManager.HandleActionRecovery(this);

            if (navMeshAgent == null)
                return;

            if (IsOwner)
                ProcessStateMachine();

            if (!navMeshAgent.enabled)
                return;

            Vector3 positionDifference = navMeshAgent.transform.position - transform.position;

            if (positionDifference.magnitude > 0.2f)
                navMeshAgent.transform.localPosition = Vector3.zero;
        }

        public void ApplyDeadState(bool snapToFinalPose = false)
        {
            if (deadStateApplied && !snapToFinalPose)
                return;

            deadStateApplied = true;
            isPerformingAction = false;
            animator.SetBool("isDead", true);
            characterNetworkManager.isMoving.Value = false;
            characterCombatManager.SetTarget(null);
            characterLocomotionManager.canMove = false;
            characterLocomotionManager.canRotate = false;
            characterLocomotionManager.canRun = false;
            characterLocomotionManager.canRoll = false;

            if (damageColliders != null)
            {
                foreach (DamageCollider damageCollider in damageColliders)
                {
                    if (damageCollider != null)
                        damageCollider.DisableDamageCollider();
                }
            }

            aiCharacterCombatManager?.CloseAllDamageColliders();

            if (navMeshAgent != null && navMeshAgent.enabled)
            {
                navMeshAgent.isStopped = true;
                navMeshAgent.ResetPath();
                navMeshAgent.enabled = false;
            }

            animator.SetBool("isMoving", false);

            if (snapToFinalPose)
            {
                animator.speed = 1;
                animator.Play("Dead_01", 0, 1f);
                animator.Update(1f / 30f);
                animator.speed = 0;
            }
            else
            {
                animator.speed = 1;
                animator.Play("Dead_01", 0, 0f);
            }
        }

        private void QueueLateJoinDeadPose()
        {
            if (deadPoseRoutine != null)
                StopCoroutine(deadPoseRoutine);

            deadPoseRoutine = StartCoroutine(ApplyLateJoinDeadPoseRoutine());
        }

        private IEnumerator ApplyLateJoinDeadPoseRoutine()
        {
            for (int i = 0; i < 8; i++)
            {
                ApplyDeadState(true);
                yield return null;
            }

            deadPoseRoutine = null;
        }

        public void BeginCoopDeathDespawn(float delay = 1.5f)
        {
            if (!IsServer)
                return;

            if (this is AIBossCharacterManager)
                return;

            if (coopDeathDespawnRoutine != null)
                StopCoroutine(coopDeathDespawnRoutine);

            coopDeathDespawnRoutine = StartCoroutine(CoopDeathDespawnRoutine(delay));
        }

        private IEnumerator CoopDeathDespawnRoutine(float delay)
        {
            yield return new WaitForSeconds(delay);

            coopDeathDespawnRoutine = null;

            if (this == null || NetworkObject == null || !NetworkObject.IsSpawned)
                yield break;

            WorldAIManager.instance?.RemoveCharacterFromSpawnedCharacterList(this);
            NetworkObject.Despawn();
        }

        private void ProcessStateMachine()
        {
            AIState nextState = currentState?.Tick(this);

            if (nextState != null && !hasManuallySwitchedState)
            {
                currentState = nextState;
            }

            hasManuallySwitchedState = false;

            //the position/rotation should be reset only after the state machine has processed its tick
            navMeshAgent.transform.localPosition = Vector3.zero;
            navMeshAgent.transform.localRotation = Quaternion.identity;

            if (aiCharacterCombatManager.currentTarget != null)
            {
                aiCharacterCombatManager.targetsDirection = aiCharacterCombatManager.currentTarget.transform.position - transform.position;
                aiCharacterCombatManager.viewableAngle = WorldUtilityManager.Instance.GetAngleOfTarget(transform, aiCharacterCombatManager.targetsDirection);
                aiCharacterCombatManager.distanceFromTarget = Vector3.Distance(transform.position, aiCharacterCombatManager.currentTarget.transform.position);
            }

            if (navMeshAgent.enabled)
            {
                Vector3 agentDestination = navMeshAgent.destination;
                float remainingDistance = Vector3.Distance(agentDestination, transform.position);

                if (remainingDistance > navMeshAgent.stoppingDistance)
                {
                    aiCharacterNetworkManager.isMoving.Value = true;
                }
                else
                {
                    aiCharacterNetworkManager.isMoving.Value = false;
                }
            }
            else
            {
                aiCharacterNetworkManager.isMoving.Value = false;
            }
        }

        //activation
        public virtual void ActivateCharacter(PlayerManager player)
        {
            aiCharacterCombatManager.AddPlayerToPlayersWithinRange(player);

            if (player.IsLocalPlayer)
            {

            }

            if (!NetworkManager.Singleton.IsHost)
                return;

            if (aiCharacterCombatManager.playersWithinActivationRange.Count > 0)
            {
                aiCharacterNetworkManager.isActive.Value = true;
            }
            else
            {
                aiCharacterNetworkManager.isActive.Value = false;
            }
        }

        public virtual void DeactivateCharacter(PlayerManager player)
        {
            aiCharacterCombatManager.RemovePlayerFromPlayersWithinRange(player);

            if (player.IsLocalPlayer)
            {

            }

            if (beacon != null)
            {
                beacon.gameObject.transform.position = transform.position;
                beacon.gameObject.SetActive(true);
            }

            if (!NetworkManager.Singleton.IsHost)
                return;

            if (aiCharacterCombatManager.playersWithinActivationRange.Count > 0)
            {
                aiCharacterNetworkManager.isActive.Value = true;
            }
            else
            {
                aiCharacterCombatManager.SetTarget(null);
                aiCharacterNetworkManager.isActive.Value = false;
            }
        }

        public void CreateActivationBeacon()
        {
            if (beacon == null)
            {
                GameObject beaconGameObject = Instantiate(WorldAIManager.instance.beaconGameObject);
                beaconGameObject.transform.position = transform.position;

                beacon = beaconGameObject.GetComponent<AIActivationBeacon>();
                beacon.SetOwnerOfBeacon(this);
            }
            else
            {
                beacon.transform.position = transform.position;
                beacon.gameObject.SetActive(true);
            }
        }
    }
}
