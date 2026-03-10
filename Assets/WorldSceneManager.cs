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

        //do not unload
        public List<string> doNotUnLoadList = new List<string>();

        //qued scenes
        private List<string> quedSceneIDs = new List<string>();
        private List<string> quedUnloadSceneIDs = new List<string>();
        private int quedScenesToLoad = 0;
        private int quedScenesToUnload = 0;
        private Coroutine loadingAdditiveScenesCoroutine;
        private Coroutine unloadAdditiveScenesCoroutine;

        //loading status
        private bool sceneIsLoading = false;
        private bool sceneIsUnloading = false;

        [Header("Scene ID")]
        public string world = "World_01";
        public string area_01_Subarea_00 = "Area_01_Subarea_00";
        public string area_01_Subarea_01 = "Area_01_Subarea_01";
        public string area_01_Subarea_02 = "Area_01_Subarea_02";
        public string area_01_Subarea_03 = "Area_01_Subarea_03";
        public string area_01_Subarea_04 = "Area_01_Subarea_04";
        public string area_01_Subarea_05 = "Area_01_Subarea_05";

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

            DontDestroyOnLoad(gameObject);
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
            StartCoroutine(UnloadAllAdditiveScenesNonNetwork());
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
                    if (quedScenesToLoad <= 0)
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
                    if (quedScenesToUnload <= 0)
                        quedUnloadSceneIDs.Clear();

                    for(int i = 0; i < loadedScenes.Count; i++)
                    {
                        if (!loadedScenes[i].isLoaded)
                            loadedScenes.RemoveAt(i);
                    }

                    sceneIsUnloading = false;
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

        //scene loading

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
        public void LoadAdditiveScenes(List<string> scenesToLoad)
        {
            if (!NetworkManager.IsServer)
                return;

            //pass all of our scenes to load to our qued scene list
            for (int i = 0; i < scenesToLoad.Count; i++)
            {
                quedSceneIDs.Add(scenesToLoad[i]);
            }

            quedScenesToLoad = quedSceneIDs.Count;

            if (loadingAdditiveScenesCoroutine != null)
                StopCoroutine(loadingAdditiveScenesCoroutine);

            loadingAdditiveScenesCoroutine = StartCoroutine(LoadAdditiveScenesCoroutine());
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
                quedScenesToLoad--;

                yield return new WaitForFixedUpdate();
            }

            quedScenesToLoad = 0;
            loadingAdditiveScenesCoroutine = null;

            yield return null;
        }

        //scene unloading

        //used to unload additive scenes in main world scene
        private void UnloadAdditiveScene(string sceneName)
        {
            if (!NetworkManager.Singleton.IsServer)
                return;

            //check the do not unload list, because another player may still need a specific scene loaded from thei pov
            for (int i = 0; i < doNotUnLoadList.Count; i++)
            {
                if (sceneName == doNotUnLoadList[i])
                    return;
            }

            for (int i = 0; i < loadedScenes.Count; i++)
            {
                if (loadedScenes[i] == null)
                    continue;

                if (loadedScenes[i].name == sceneName && loadedScenes[i].isLoaded)
                {
                    var sceneLoad = NetworkManager.SceneManager.UnloadScene(loadedScenes[i]);
                    break;
                }
            }
        }

        public void UnloadAdditiveScenes(List<string> sceneList)
        {
            if (!NetworkManager.Singleton.IsServer)
                return;

            for (int i = 0; i < sceneList.Count; i++)
            {
                quedUnloadSceneIDs.Add(sceneList[i]);
            }

            quedScenesToUnload = quedUnloadSceneIDs.Count;

            if (unloadAdditiveScenesCoroutine != null)
                StopCoroutine(unloadAdditiveScenesCoroutine);

            unloadAdditiveScenesCoroutine = StartCoroutine(UnloadAdditiveScenesCoroutine());
        }

        private IEnumerator UnloadAdditiveScenesCoroutine()
        {
            for (int i = 0; i < quedUnloadSceneIDs.Count; i++)
            {
                while (sceneIsLoading || sceneIsUnloading)
                {
                    yield return new WaitForFixedUpdate();
                }

                UnloadAdditiveScene(quedUnloadSceneIDs[i]);
                quedScenesToUnload--;

                yield return null;
            }

            quedScenesToUnload = 0;
            unloadAdditiveScenesCoroutine = null;
        }

        private IEnumerator UnloadAllAdditiveScenesNonNetwork()
        {
            for (int i = 0; i < loadedScenes.Count; i++)
            {
                if (loadedScenes[i] == null)
                    continue;

                if (!loadedScenes[i].IsValid())
                    continue;

                var loadingOperation = SceneManager.UnloadSceneAsync(loadedScenes[i].name);

                yield return null;

                while (loadingOperation != null && !loadingOperation.isDone)
                {
                    yield return null;
                }
            }

            yield return null;
        }

        public void CheckForUnrequiredScenes()
        {
            List<string> scenesToUnload = new List<string>();

            //get all currently loaded scenes
            for (int i = 0; i < loadedScenes.Count; i++)
            {
                scenesToUnload.Add(loadedScenes[i].name);
            }

            doNotUnLoadList = WorldSubsceneManager.instance.GenerateDoNotUnloadListBasedOnPlayerLocations();

            //compare all loaded scenes
            for (int i = 0; i < scenesToUnload.Count; i++)
            {
                if (doNotUnLoadList.Contains(scenesToUnload[i]))
                    scenesToUnload.Remove(scenesToUnload[i]);
            }

            UnloadAdditiveScenes(scenesToUnload);
        }

        //scene id
        public string GetSceneIDFromWorldSceneLocation(WorldSceneLocation area)
        {
            string sceneID = "";

            switch (area)
            {
                case WorldSceneLocation.Area01_Subarea00:
                    return area_01_Subarea_00;
                case WorldSceneLocation.Area01_Subarea01:
                    return area_01_Subarea_01;
                case WorldSceneLocation.Area01_Subarea02:
                    return area_01_Subarea_02;
                case WorldSceneLocation.Area01_Subarea03:
                    return area_01_Subarea_03;
                case WorldSceneLocation.Area01_Subarea04:
                    return area_01_Subarea_04;
                case WorldSceneLocation.Area01_Subarea05:
                    return area_01_Subarea_05;
                default:
                    break;
            }

            return sceneID;
        }
    }
}
