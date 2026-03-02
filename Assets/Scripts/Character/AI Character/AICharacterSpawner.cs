using UnityEngine;
using Unity.Netcode;
using Unity.VisualScripting;

namespace baodeag
{
    public class AICharacterSpawner : MonoBehaviour
    {
        [Header("Character")]
        [SerializeField] GameObject characterGameObject;
        [SerializeField] GameObject instantiateGameObject;
        private AICharacterManager aiCharacter;

        [Header("Patrol")]
        [SerializeField] bool hasPatrolPath = false;
        [SerializeField] int patrolPathID = 0;

        [Header("Sleep")]
        [SerializeField] bool isSleeping = false;

        [Header("Stats")]
        [SerializeField] bool manuallySetStats = true;
        [SerializeField] int stamina = 150;
        [SerializeField] int health = 400;

        private void Awake()
        {
            
        }
        private void Start()
        {
            WorldAIManager.instance.SpawnCharacter(this);
            gameObject.SetActive(false);
        }

        public void AttemptToSpawnCharacter()
        {
            if (characterGameObject != null)
            {
                instantiateGameObject = Instantiate(characterGameObject);
                instantiateGameObject.transform.position = transform.position;
                instantiateGameObject.transform.rotation = transform.rotation;
                instantiateGameObject.GetComponent<NetworkObject>().Spawn();
                aiCharacter = instantiateGameObject.GetComponent<AICharacterManager>();

                if (aiCharacter == null)
                    return;

                WorldAIManager.instance.AddCharacterToSpawnedCharacterList(aiCharacter);

                if (hasPatrolPath)
                    aiCharacter.idle.aiPatrolPath = WorldAIManager.instance.GetAIPatrolPathByID(patrolPathID);

                if (isSleeping)
                    aiCharacter.aiCharacterNetworkManager.isAwake.Value = false;

                if (manuallySetStats)
                {
                    aiCharacter.aiCharacterNetworkManager.maxHealth.Value = health;
                    aiCharacter.aiCharacterNetworkManager.currentHealth.Value = health;
                    aiCharacter.aiCharacterNetworkManager.maxStamina.Value = stamina;
                    aiCharacter.aiCharacterNetworkManager.currentStamina.Value = stamina;
                }

                aiCharacter.aiCharacterNetworkManager.isActive.Value = false;
            }
        }

        public void ResetCharacter()
        {
            if (instantiateGameObject == null)
                return;

            if (aiCharacter == null)
                return;

            instantiateGameObject.transform.position = transform.position;
            instantiateGameObject.transform.rotation = transform.rotation;
            aiCharacter.aiCharacterNetworkManager.currentHealth.Value = aiCharacter.aiCharacterNetworkManager.maxHealth.Value;
            aiCharacter.aiCharacterCombatManager.SetTarget(null);

            if (aiCharacter.isDead.Value)
            {
                aiCharacter.isDead.Value = false;
                aiCharacter.characterAnimatorManager.PlayTargetActionAnimation("Empty", false, false, true, true, true, true);
                aiCharacter.currentState.SwitchState(aiCharacter, aiCharacter.idle);
            }

            aiCharacter.characterUIManager.ResetCharacterHPBar();
        }
    }
}
