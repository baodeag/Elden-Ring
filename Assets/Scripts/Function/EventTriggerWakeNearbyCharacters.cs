using UnityEngine;
using Unity.Netcode;
using System.Collections.Generic;

namespace baodeag
{
    public class EventTriggerWakeNearbyCharacters : MonoBehaviour
    {
        [SerializeField] float awakernRadius = 0;

        private void OnTriggerEnter(Collider other)
        {
            if (!NetworkManager.Singleton.IsServer)
                return;

            PlayerManager player = other.GetComponent<PlayerManager>();

            if (player == null)
                return;

            Collider[] creatureInRadius = Physics.OverlapSphere(transform.position, awakernRadius, WorldUtilityManager.Instance.GetCharacterLayers());
            List<AICharacterManager> creaturesToWake = new List<AICharacterManager>();

            for (int i = 0; i < creatureInRadius.Length; i++)
            {
                AICharacterManager aiCharacter = creatureInRadius[i].GetComponent<AICharacterManager>();

                if (aiCharacter == null)
                    continue;

                if (aiCharacter.isDead.Value)
                    continue;

                if (aiCharacter.aiCharacterNetworkManager.isAwake.Value)
                    continue;

                if (!creaturesToWake.Contains(aiCharacter))
                    creaturesToWake.Add(aiCharacter);
            }

            for (int i = 0; i < creaturesToWake.Count; i++)
            {
                creaturesToWake[i].aiCharacterCombatManager.SetTarget(player);
            }
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(transform.position, awakernRadius);
        }
    }
}
