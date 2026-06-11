using UnityEngine;
using Unity.Netcode;
using System.Collections.Generic;

namespace baodeag
{
    public class EventTriggerLoadScene : MonoBehaviour
    {
        private static readonly List<EventTriggerLoadScene> registeredTriggers = new List<EventTriggerLoadScene>();

        [Header("Area")]
        [SerializeField] WorldLocationSceneSet area;

        private void OnEnable()
        {
            if (!registeredTriggers.Contains(this))
                registeredTriggers.Add(this);

            
        }

        private void OnDisable()
        {
            registeredTriggers.Remove(this);
            
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!NetworkManager.Singleton.IsServer) 
                return;

            PlayerManager player = other.GetComponent<PlayerManager>();

            if (player == null)
                return;

            AddPlayerToArea(player);
        }

        private void AddPlayerToArea(PlayerManager player)
        {
            

            if (WorldSceneManager.instance != null && WorldSceneManager.instance.ShouldLoadGeneratedWorldAllAtOnce())
            {
                
                return;
            }

            WorldLocationManager.instance.LoadAreasBasedOnAreaCurrentIn(area, player);
            
        }

        /// <summary>
        /// Manually fires area loading for a player as if they entered this trigger.
        /// Call this after teleporting a player to bypass physics OnTriggerEnter timing.
        /// </summary>
        public void ManualTriggerForPlayer(PlayerManager player)
        {
            

            if (!NetworkManager.Singleton.IsServer)
            {
                
                return;
            }

            if (player == null)
            {
                
                return;
            }

            AddPlayerToArea(player);
            
        }

        public WorldLocationSceneSet GetArea() => area;

        public static List<EventTriggerLoadScene> GetRegisteredTriggersSnapshot()
        {
            List<EventTriggerLoadScene> snapshot = new List<EventTriggerLoadScene>();

            for (int i = registeredTriggers.Count - 1; i >= 0; i--)
            {
                if (registeredTriggers[i] == null || !registeredTriggers[i].isActiveAndEnabled)
                {
                    registeredTriggers.RemoveAt(i);
                    continue;
                }

                snapshot.Add(registeredTriggers[i]);
            }

            return snapshot;
        }
    }
}
