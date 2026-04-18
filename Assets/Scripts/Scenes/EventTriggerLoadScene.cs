using UnityEngine;
using Unity.Netcode;

namespace baodeag
{
    public class EventTriggerLoadScene : MonoBehaviour
    {
        [Header("Area")]
        [SerializeField] WorldLocationSceneSet area;

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
                return;

            WorldLocationManager.instance.LoadAreasBasedOnAreaCurrentIn(area, player);
        }

        /// <summary>
        /// Manually fires area loading for a player as if they entered this trigger.
        /// Call this after teleporting a player to bypass physics OnTriggerEnter timing.
        /// </summary>
        public void ManualTriggerForPlayer(PlayerManager player)
        {
            if (!NetworkManager.Singleton.IsServer)
                return;

            if (player == null)
                return;

            AddPlayerToArea(player);
        }

        public WorldLocationSceneSet GetArea() => area;
    }
}
