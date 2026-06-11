using UnityEngine;
using System.Collections;

namespace baodeag
{
    public class EventTriggerBossFight : MonoBehaviour
    {
        [SerializeField] int bossID;
        [SerializeField] Collider triggerCollider;

        private void Awake()
        {
            AutoAssignBossIDFromWorldScene();

            if (triggerCollider == null)
                triggerCollider = GetComponent<Collider>();
        }

        private void OnValidate()
        {
            AutoAssignBossIDFromWorldScene();
        }

        private void AutoAssignBossIDFromWorldScene()
        {
            int sceneBuildIndex = gameObject.scene.buildIndex;

            if (sceneBuildIndex < 1 || sceneBuildIndex > 5)
                return;

            bossID = sceneBuildIndex - 1;
        }

        private void Start()
        {
            StartCoroutine(SyncTriggerState());
        }

        private void OnTriggerEnter(Collider other)
        {
            PlayerManager player = other.GetComponent<PlayerManager>();

            if (player == null)
                player = other.GetComponentInParent<PlayerManager>();

            if (player == null)
                return;

            AIBossCharacterManager boss = WorldAIManager.instance.GetBossCharacterByID(bossID);

            if (boss == null)
            {
                
                StartCoroutine(TryWakeBossWhenAvailable());
                return;
            }

            if (boss.hasBeenDefeated.Value)
            {
                DisableTrigger();
                return;
            }

            if (!boss.bossFightIsActive.Value)
            {
                if (boss.IsServer || boss.IsOwner)
                    boss.WakeBoss();
                else
                    boss.RequestWakeBossServerRpc();
            }
        }

        private IEnumerator SyncTriggerState()
        {
            AIBossCharacterManager boss = null;

            while (boss == null)
            {
                boss = WorldAIManager.instance.GetBossCharacterByID(bossID);
                yield return null;
            }

            if (boss.hasBeenDefeated.Value)
            {
                DisableTrigger();
            }
        }

        private IEnumerator TryWakeBossWhenAvailable()
        {
            AIBossCharacterManager boss = null;
            float timeout = Time.time + 2f;

            while (boss == null && Time.time < timeout)
            {
                boss = WorldAIManager.instance.GetBossCharacterByID(bossID);
                yield return null;
            }

            if (boss == null || boss.hasBeenDefeated.Value || boss.bossFightIsActive.Value)
                yield break;

            if (boss.IsServer || boss.IsOwner)
                boss.WakeBoss();
            else
                boss.RequestWakeBossServerRpc();
        }

        private void DisableTrigger()
        {
            if (triggerCollider != null)
            {
                triggerCollider.enabled = false;
            }

            gameObject.SetActive(false);
        }
    }
}
