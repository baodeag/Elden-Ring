using UnityEngine;
using System.Collections.Generic;
using System.Collections;

namespace baodeag
{
    public class AIPatrolPath : MonoBehaviour
    {
        public int patrolPathID = 0;
        public List<Vector3> patrolPoints = new List<Vector3>();
        private Coroutine registerCoroutine;

        private void Awake()
        {
            CachePatrolPoints();
        }

        private void OnEnable()
        {
            TryRegisterWithWorldAIManager();
        }

        private void OnDisable()
        {
            if (registerCoroutine != null)
            {
                StopCoroutine(registerCoroutine);
                registerCoroutine = null;
            }
        }

        private void CachePatrolPoints()
        {
            patrolPoints.Clear();

            for (int i = 0; i < transform.childCount; i++)
            {
                patrolPoints.Add(transform.GetChild(i).position);
            }
        }

        private void TryRegisterWithWorldAIManager()
        {
            if (WorldAIManager.instance != null)
            {
                WorldAIManager.instance.AddPatrolPathToList(this);
                return;
            }

            if (registerCoroutine == null)
                registerCoroutine = StartCoroutine(RegisterWhenWorldAIManagerIsReady());
        }

        private IEnumerator RegisterWhenWorldAIManagerIsReady()
        {
            while (WorldAIManager.instance == null)
                yield return null;

            WorldAIManager.instance.AddPatrolPathToList(this);
            registerCoroutine = null;
        }
    }
}
