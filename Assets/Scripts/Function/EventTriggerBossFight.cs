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
            if (triggerCollider == null)
                triggerCollider = GetComponent<Collider>();
        }

        private void Start()
        {
            StartCoroutine(SyncTriggerState());
        }

        private void OnTriggerEnter(Collider other)
        {
            PlayerManager player = other.GetComponent<PlayerManager>();

            if (player == null)
                return;

            AIBossCharacterManager boss = WorldAIManager.instance.GetBossCharacterByID(bossID);

            if (boss == null)
                return;

            if (boss.hasBeenDefeated.Value)
            {
                DisableTrigger();
                return;
            }

            if (!boss.bossFightIsActive.Value)
            {
                boss.WakeBoss();
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
