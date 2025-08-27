using UnityEngine;
using Unity.Netcode;

namespace baodeag { 
    public class TitleScreenManager : MonoBehaviour
    {
        public void StartNetworkAsHost()
        {
            // Code to start network as host
            NetworkManager.Singleton.StartHost();
        }

        public void StartNewGame()
        {
            StartCoroutine(WorldSaveGameManager.instance.LoadNewGame());
        }
    }
}