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

            BuildRuntimeLogger.Log($"EventTriggerLoadScene.OnEnable name={name} area={(area != null ? area.name : "null")} registered={registeredTriggers.Count}");
        }

        private void OnDisable()
        {
            registeredTriggers.Remove(this);
            BuildRuntimeLogger.Log($"EventTriggerLoadScene.OnDisable name={name} registered={registeredTriggers.Count}");
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
            BuildRuntimeLogger.Log($"EventTriggerLoadScene.AddPlayerToArea begin trigger={name} area={(area != null ? area.name : "null")} player={(player != null ? player.name : "null")}");

            if (WorldSceneManager.instance != null && WorldSceneManager.instance.ShouldLoadGeneratedWorldAllAtOnce())
            {
                BuildRuntimeLogger.Log($"EventTriggerLoadScene.AddPlayerToArea skip generated-world-all-at-once trigger={name}");
                return;
            }

            WorldLocationManager.instance.LoadAreasBasedOnAreaCurrentIn(area, player);
            BuildRuntimeLogger.Log($"EventTriggerLoadScene.AddPlayerToArea end trigger={name}");
        }

        /// <summary>
        /// Manually fires area loading for a player as if they entered this trigger.
        /// Call this after teleporting a player to bypass physics OnTriggerEnter timing.
        /// </summary>
        public void ManualTriggerForPlayer(PlayerManager player)
        {
            BuildRuntimeLogger.Log($"EventTriggerLoadScene.ManualTriggerForPlayer begin trigger={name} player={(player != null ? player.name : "null")}");

            if (!NetworkManager.Singleton.IsServer)
            {
                BuildRuntimeLogger.Warning($"EventTriggerLoadScene.ManualTriggerForPlayer skipped because not server trigger={name}");
                return;
            }

            if (player == null)
            {
                BuildRuntimeLogger.Warning($"EventTriggerLoadScene.ManualTriggerForPlayer skipped null player trigger={name}");
                return;
            }

            AddPlayerToArea(player);
            BuildRuntimeLogger.Log($"EventTriggerLoadScene.ManualTriggerForPlayer end trigger={name}");
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
