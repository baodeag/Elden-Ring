using UnityEngine;
using UnityEngine.SceneManagement;
using Unity.Netcode;

namespace baodeag
{
    public class WorldMapTransitionInteractable : Interactable
    {
        [Header("Map Transition")]
        [SerializeField] private int targetMapIndex = 1;
        [SerializeField] private string transitionText = "Travel to Map 2";

        protected override void Awake()
        {
            base.Awake();
            interactableText = transitionText;
        }

        protected override void Start()
        {
            base.Start();
            interactableText = transitionText;
        }

        public override void Interact(PlayerManager player)
        {
            if (player == null || !player.IsOwner)
                return;

            if (!GameProgressionManager.Instance.PrepareTransitionToMap(targetMapIndex, out int sceneBuildIndex))
            {
                
                return;
            }

            player.playerInteractionManager.RemoveInteractionFromList(this);
            PlayerUIManager.instance.playerUIPopUpManager.CloseAllPopUpWindows();

            if (WorldSaveGameManager.instance != null && WorldSaveGameManager.instance.currentCharacterData != null)
            {
                
                WorldSaveGameManager.instance.currentCharacterData.currentMapIndex = targetMapIndex;
                WorldSaveGameManager.instance.currentCharacterData.sceneIndex = sceneBuildIndex;
                WorldSaveGameManager.instance.SaveGame(player, sceneBuildIndex, targetMapIndex);
            }

            if (NetworkManager.Singleton != null &&
                NetworkManager.Singleton.IsClient &&
                !NetworkManager.Singleton.IsServer)
            {
                RequestWorldSceneTransitionServerRpc(sceneBuildIndex);
                return;
            }

            if (WorldSceneManager.instance != null)
            {
                WorldSceneManager.instance.LoadWorldScene(sceneBuildIndex);
            }
            else
            {
                SceneManager.LoadScene(sceneBuildIndex, LoadSceneMode.Single);
            }
        }

        [ServerRpc(RequireOwnership = false)]
        private void RequestWorldSceneTransitionServerRpc(int sceneBuildIndex)
        {
            if (WorldSceneManager.instance != null)
            {
                WorldSceneManager.instance.LoadWorldScene(sceneBuildIndex);
            }
            else
            {
                SceneManager.LoadScene(sceneBuildIndex, LoadSceneMode.Single);
            }
        }
    }
}
