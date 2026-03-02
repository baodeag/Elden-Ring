using UnityEngine;
using UnityEngine.AI;
using Unity.Netcode;

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

        [Header("Nav Mesh Agent")]
        public NavMeshAgent navMeshAgent;

        [Header("Current State")]
        public AIState currentState;

        [Header("States")]
        public IdleState idle;
        public PursueTargetState pursueTarget;
        public CombatStanceState combatStance;
        public AttackState attack;
        public InvestigateSoundState investigateSound;

        [Header("Activation Beacon")]
        protected AIActivationBeacon beacon;

        protected override void Awake()
        {
            base.Awake();

            aiCharacterCombatManager = GetComponent<AICharacterCombatManager>();
            aiCharacterNetworkManager = GetComponent<AICharacterNetworkManager>();
            aiCharacterLocomotionManager = GetComponent<AICharacterLocomotionManager>();
            aiCharacterInventoryManager = GetComponent<AICharacterInventoryManager>();

            navMeshAgent = GetComponentInChildren<NavMeshAgent>();
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

            if (!aiCharacterNetworkManager.isAwake.Value)
                animator.Play(aiCharacterNetworkManager.sleepingAnimation.Value.ToString());

            if (isDead.Value)
                animator.Play("Dead_01");

            CreateActivationBeacon();
        }

        public override void OnNetworkDespawn()
        {
            base.OnNetworkDespawn();

            aiCharacterNetworkManager.currentHealth.OnValueChanged -= aiCharacterNetworkManager.OnHPChanged;
            aiCharacterNetworkManager.isBlocking.OnValueChanged -= aiCharacterNetworkManager.OnIsBlockingChanged;
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

            if (beacon != null)
                Destroy(beacon);
        }

        protected override void Update()
        {
            base.Update();

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

        private void ProcessStateMachine()
        {
            AIState nextState = currentState?.Tick(this);

            if (nextState != null)
            {
                currentState = nextState;
            }

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
        public void ActivateCharacter(PlayerManager player)
        {
            aiCharacterCombatManager.AddPlayerToPlayersWithinRange(player);

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
                aiCharacterNetworkManager.isActive.Value = false;
            }
        }

        public void DeactivateCharacter(PlayerManager player)
        {
            aiCharacterCombatManager.RemovePlayerFromPlayersWithinRange(player);

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
