using UnityEngine;
using Unity.Netcode;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using System.Collections;

namespace baodeag
{
    public class WorldSceneManager : NetworkBehaviour
    {
        public static WorldSceneManager instance;

        //loaded scenes
        public List<Scene> loadedScenes = new List<Scene>();

        //qued scenes
        private List<string> quedSceneIDs = new List<string>();
        private int quedSceneToLoad = 0;
        private Coroutine loadingAdditiveSceneCoroutine;

        //loading status
        private bool sceneIsLoading = false;
        private bool sceneIsUnloading = false;

        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
        }

        public override void OnNetworkSpawn()
        {
            base.OnNetworkSpawn();

            NetworkManager.SceneManager.OnSceneEvent += OnSceneEvent;
        }

        public override void OnNetworkDespawn()
        {
            base.OnNetworkDespawn();

            NetworkManager.SceneManager.OnSceneEvent -= OnSceneEvent;

            //unload all scenes
        }

        private void OnSceneEvent(SceneEvent sceneEvent)
        {
            if (!NetworkManager.IsServer)
                return;

            switch (sceneEvent.SceneEventType)
            {
                case SceneEventType.Load:
                    sceneIsLoading = true;
                    break;
                case SceneEventType.Unload:
                    sceneIsUnloading = true;
                    break;
                case SceneEventType.Synchronize:
                    break;
                case SceneEventType.ReSynchronize:
                    break;
                case SceneEventType.LoadEventCompleted:
                    break;
                case SceneEventType.UnloadEventCompleted:
                    sceneIsUnloading = false;
                    break;
                case SceneEventType.LoadComplete:
                    //called when the scene is finished loading, add it to the list of loaded scenes
                    loadedScenes.Add(sceneEvent.Scene); 
                    
                    //clear the list ids of the scenes to load count it 0
                    if (quedSceneToLoad <= 0)
                        quedSceneIDs.Clear();

                    //double check loaded scenes to make sure they are loaded, if not remove them from the loaded list
                    for (int i = 0; i < loadedScenes.Count; i++)
                    {
                        if (!loadedScenes[i].isLoaded)
                            loadedScenes.RemoveAt(i);
                    }

                    sceneIsLoading = false;
                    break;
                case SceneEventType.UnloadComplete:
                    break;
                case SceneEventType.SynchronizeComplete:
                    break;
                case SceneEventType.ActiveSceneChanged:
                    break;
                case SceneEventType.ObjectSceneChanged:
                    break;
                default:
                    break;
            }
        }

        //used to load our main world scene
        public void LoadWorldScene(int buildIndex)
        {
            //activate loading screen
            PlayerUIManager.instance.playerUILoadingScreenManager.ActivateLoadingScreen();

            //get world scene, and load it
            string worldScene = SceneUtility.GetScenePathByBuildIndex(buildIndex);
            NetworkManager.Singleton.SceneManager.LoadScene(worldScene, LoadSceneMode.Single);

            //load player save data
            PlayerUIManager.instance.localPlayer.LoadGameDataFromCurrentCharacterData(ref WorldSaveGameManager.instance.currentCharacterData);
        }

        //used to load additive scenes in main world scene
        private void LoadAdditiveScene(string sceneName)
        {
            for (int i = 0; i < loadedScenes.Count; i++)
            {
                //if the scene in the list is null, continue to look at other scenes
                if (loadedScenes[i] == null)
                    continue;

                //if the scene is already loaded, abort
                if (loadedScenes[i].name == sceneName && loadedScenes[i].isLoaded)
                    return;
            }

            //load the scene
            var loadSceneStatus = NetworkManager.Singleton.SceneManager.LoadScene(sceneName, LoadSceneMode.Additive);
        }

        //used to load multiple additive scenes at once when entering new area
        public void LoadAdditiveScene(List<string> scenesToLoad)
        {
            if (!NetworkManager.IsServer)
                return;

            //pass all of our scenes to load to our qued scene list
            for (int i = 0; i < scenesToLoad.Count; i++)
            {
                quedSceneIDs.Add(scenesToLoad[i]);
            }

            quedSceneToLoad = quedSceneIDs.Count;

            if (loadingAdditiveSceneCoroutine != null)
                StopCoroutine(loadingAdditiveSceneCoroutine);

            loadingAdditiveSceneCoroutine = StartCoroutine(LoadAdditiveScenesCoroutine());
        }

        private IEnumerator LoadAdditiveScenesCoroutine()
        {
            //check to see if a scene is currently being loaded/unloaded and if it is, wait
            for (int i = 0; i < quedSceneIDs.Count; i++)
            {
                while (sceneIsLoading || sceneIsUnloading)
                {
                    yield return null;
                }

                if (quedSceneIDs[i] == null)
                    continue;

                //sort through a qued list of scenes, and load them one by one
                LoadAdditiveScene(quedSceneIDs[i]);
                quedSceneToLoad--;

                yield return new WaitForFixedUpdate();
            }

            quedSceneToLoad = 0;
            loadingAdditiveSceneCoroutine = null;

            yield return null;
        }
    }
}
