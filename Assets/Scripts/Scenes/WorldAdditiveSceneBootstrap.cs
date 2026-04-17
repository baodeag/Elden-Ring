using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace baodeag
{
    public class WorldAdditiveSceneBootstrap : MonoBehaviour
    {
        [SerializeField] private List<string> additiveScenesToLoad = new List<string>();
        [SerializeField] private bool loadOnStart = true;

        private IEnumerator Start()
        {
            if (!loadOnStart || additiveScenesToLoad.Count == 0)
                yield break;

            yield return null;

            if (NetworkManager.Singleton != null &&
                NetworkManager.Singleton.IsListening &&
                !NetworkManager.Singleton.IsServer)
            {
                yield break;
            }

            List<string> scenesToLoad = new List<string>();

            for (int i = 0; i < additiveScenesToLoad.Count; i++)
            {
                string sceneName = additiveScenesToLoad[i];

                if (string.IsNullOrWhiteSpace(sceneName))
                    continue;

                if (!IsSceneLoaded(sceneName) && !scenesToLoad.Contains(sceneName))
                    scenesToLoad.Add(sceneName);
            }

            if (scenesToLoad.Count == 0)
                yield break;

            if (WorldSceneManager.instance != null &&
                NetworkManager.Singleton != null &&
                NetworkManager.Singleton.IsListening)
            {
                WorldSceneManager.instance.LoadAdditiveScenes(scenesToLoad);
                yield break;
            }

            for (int i = 0; i < scenesToLoad.Count; i++)
            {
                SceneManager.LoadScene(scenesToLoad[i], LoadSceneMode.Additive);
            }
        }

        public void SetAdditiveScenes(IReadOnlyList<string> sceneNames)
        {
            additiveScenesToLoad.Clear();

            if (sceneNames == null)
                return;

            for (int i = 0; i < sceneNames.Count; i++)
            {
                string sceneName = sceneNames[i];

                if (!string.IsNullOrWhiteSpace(sceneName) && !additiveScenesToLoad.Contains(sceneName))
                    additiveScenesToLoad.Add(sceneName);
            }
        }

        private bool IsSceneLoaded(string sceneName)
        {
            for (int i = 0; i < SceneManager.sceneCount; i++)
            {
                Scene scene = SceneManager.GetSceneAt(i);

                if (scene.IsValid() && scene.isLoaded && scene.name == sceneName)
                    return true;
            }

            return false;
        }
    }
}
