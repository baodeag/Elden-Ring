using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TextCore.Text;

namespace baodeag
{
    public class WorldAIManager : MonoBehaviour
    {
        public static WorldAIManager instance;

        [Header("Loading")]
        public bool isPerformingLoadingOperation = false;

        [Header("Characters")]
        [SerializeField] List<AICharacterSpawner> aiCharacterSpawners;
        [SerializeField] List<AICharacterManager> spawnedInCharacters;
        private Coroutine spawnAllCharactersCoroutine;
        private Coroutine despawnAllCharactersCoroutine;
        private Coroutine resetAllCharactersCoroutine;
        private Coroutine despawnDeadCharactersCoroutine;

        [Header("Beacon Prefab")]
        public GameObject beaconGameObject;

        [Header("Dialogue Interactable Prefab")]
        public GameObject dialogueInteractable;

        [Header("Bosses")]
        [SerializeField] List<AIBossCharacterManager> spawnedInBosses;

        [Header("Patrol Paths")]
        [SerializeField] List<AIPatrolPath> aiPatrolPaths = new List<AIPatrolPath>();

        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
        }

        public void SpawnCharacter(AICharacterSpawner aiCharacterSpawner)
        {
            if (NetworkManager.Singleton.IsServer)
            {
                aiCharacterSpawners.Add(aiCharacterSpawner);
                aiCharacterSpawner.AttemptToSpawnCharacter();
            }
        }

        public void AddCharacterToSpawnedCharacterList(AICharacterManager character)
        {
            if (spawnedInCharacters.Contains(character))
                return;

            spawnedInCharacters.Add(character);

            AIBossCharacterManager bossCharacter = character as AIBossCharacterManager;

            if (bossCharacter != null)
            {
                if (spawnedInBosses.Contains(bossCharacter))
                    return;

                spawnedInBosses.Add(bossCharacter);
            }
        }

        public AIBossCharacterManager GetBossCharacterByID(int ID)
        {
            return spawnedInBosses.FirstOrDefault(boss => boss.bossID == ID);
        }

        //if you have more than 25-30 enemies per area, reset their stats and animations instead of despawning and respawning them
        public void SpawnAllCharacters()
        {
            isPerformingLoadingOperation = true;

            if (spawnAllCharactersCoroutine != null)
                StopCoroutine(spawnAllCharactersCoroutine);

            spawnAllCharactersCoroutine = StartCoroutine(SpawnAllCharactersCoroutine());
        }

        private IEnumerator SpawnAllCharactersCoroutine()
        {
            for (int i = 0; i < aiCharacterSpawners.Count; i++)
            {
                yield return new WaitForFixedUpdate();

                aiCharacterSpawners[i].AttemptToSpawnCharacter();

                yield return null;
            }

            isPerformingLoadingOperation = false;

            yield return null;
        }

        public void ResetAllCharacters()
        {
            isPerformingLoadingOperation = true;

            if (resetAllCharactersCoroutine != null)
                StopCoroutine(resetAllCharactersCoroutine);

            resetAllCharactersCoroutine = StartCoroutine(ResetAllCharactersCoroutine());
        }

        private IEnumerator ResetAllCharactersCoroutine()
        {
            for (int i = 0; i < aiCharacterSpawners.Count; i++)
            {
                yield return new WaitForFixedUpdate();

                aiCharacterSpawners[i].ResetCharacter();

                yield return null;
            }

            isPerformingLoadingOperation = false;

            yield return null;
        }

        private void DespawnAllCharacters()
        {
            isPerformingLoadingOperation = true;

            if (despawnAllCharactersCoroutine != null)
                StopCoroutine(despawnAllCharactersCoroutine);

            despawnAllCharactersCoroutine = StartCoroutine(DespawnAllCharactersCoroutine());
        }

        private IEnumerator DespawnAllCharactersCoroutine()
        {
            for (int i = 0; i < spawnedInCharacters.Count; i++)
            {
                yield return new WaitForFixedUpdate();

                spawnedInCharacters[i].GetComponent<NetworkObject>().Despawn();

                yield return null;
            }

            spawnedInCharacters.Clear();

            isPerformingLoadingOperation = false;

            yield return null;
        }

        private void DisableAllCharacters()
        {

        }

        public void DespawnAllDeadCharacters()
        {
            if (!NetworkManager.Singleton.IsServer)
                return;

            if (despawnDeadCharactersCoroutine != null)
                StopCoroutine(despawnDeadCharactersCoroutine);

            despawnDeadCharactersCoroutine = StartCoroutine(DespawnAllDeadCharactersCoroutine());
        }

        private IEnumerator DespawnAllDeadCharactersCoroutine()
        {
            isPerformingLoadingOperation = true;

            for (int i = spawnedInCharacters.Count - 1; i > -1; i--)
            {
                AICharacterManager character = spawnedInCharacters[i];

                if (character == null)
                {
                    spawnedInCharacters.RemoveAt(i);
                    continue;
                }

                if (!character.isDead.Value)
                    continue;

                if (character is AIBossCharacterManager)
                    continue;

                NetworkObject networkObject = character.GetComponent<NetworkObject>();

                if (networkObject != null && networkObject.IsSpawned)
                    networkObject.Despawn();

                spawnedInCharacters.RemoveAt(i);

                yield return new WaitForFixedUpdate();
            }

            isPerformingLoadingOperation = false;
            despawnDeadCharactersCoroutine = null;
        }

        public void DisableAllBossFights()
        {
            for (int i = 0; i < spawnedInBosses.Count; i++)
            {
                if (spawnedInBosses[i] == null)
                    continue;

                spawnedInBosses[i].bossFightIsActive.Value = false;
            }
        }

        //patrol paths
        public void AddPatrolPathToList(AIPatrolPath patrolPath)
        {
            if (aiPatrolPaths.Contains(patrolPath))
                return;

            aiPatrolPaths.Add(patrolPath);
        }

        public AIPatrolPath GetAIPatrolPathByID(int patrolPathID)
        {
            AIPatrolPath patrolPath = null;

            for (int i = 0; i < aiPatrolPaths.Count; i++)
            {
                if (aiPatrolPaths[i].patrolPathID == patrolPathID)
                    patrolPath = aiPatrolPaths[i];
            }

            return patrolPath;
        }

        public void RemoveCharacterFromSpawnedCharacterList(AICharacterManager character)
        {
            if (character == null)
                return;

            spawnedInCharacters.Remove(character);

            AIBossCharacterManager bossCharacter = character as AIBossCharacterManager;

            if (bossCharacter != null)
                spawnedInBosses.Remove(bossCharacter);
        }
    }
}
