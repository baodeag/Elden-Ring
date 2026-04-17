using UnityEngine;
using Unity.Netcode;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using System.Collections;
using System.IO;

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

        //scene renderers
        private Coroutine requiredRenderersCoroutine;

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
            RefreshCurrentWorldSceneID(SceneManager.GetActiveScene());
        }

        private void OnEnable()
        {
            SceneManager.sceneLoaded += OnUnitySceneLoaded;
        }

        private void OnDisable()
        {
            SceneManager.sceneLoaded -= OnUnitySceneLoaded;
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
                    break;

                case SceneEventType.Unload:
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

                    RefreshCurrentWorldSceneID(sceneEvent.Scene);

                    //double check loaded scenes to make sure they are loaded, if not remove them from the loaded list
                    for (int i = 0; i < loadedScenes.Count; i++)
                    {
                        if (!loadedScenes[i].isLoaded)
                            loadedScenes.RemoveAt(i);
                    }

                    sceneIsLoading = false;
                    CheckForRequiredRenderers();
                    break;

                case SceneEventType.UnloadComplete:
                    for (int i = 0; i < loadedScenes.Count; i++)
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
            PrepareForSingleWorldSceneLoad();

            //activate loading screen
            PlayerUIManager.instance.playerUILoadingScreenManager.ActivateLoadingScreen();

            string worldScenePath = SceneUtility.GetScenePathByBuildIndex(buildIndex);

            if (string.IsNullOrEmpty(worldScenePath))
            {
                Debug.LogWarning($"WorldSceneManager: Could not resolve scene path for build index {buildIndex}.");
                return;
            }

            bool startedNetworkSceneLoad = false;

            if (NetworkManager.Singleton != null &&
                NetworkManager.Singleton.SceneManager != null &&
                (NetworkManager.Singleton.IsHost || NetworkManager.Singleton.IsServer || NetworkManager.Singleton.IsClient))
            {
                string worldSceneName = Path.GetFileNameWithoutExtension(worldScenePath);
                var loadSceneStatus = NetworkManager.Singleton.SceneManager.LoadScene(worldSceneName, LoadSceneMode.Single);
                startedNetworkSceneLoad = loadSceneStatus == SceneEventProgressStatus.Started;

                if (!startedNetworkSceneLoad)
                {
                    Debug.LogWarning($"WorldSceneManager: Netcode scene load did not start for '{worldSceneName}' (build index {buildIndex}). Falling back to SceneManager.LoadScene.");
                }
            }

            if (!startedNetworkSceneLoad)
            {
                SceneManager.LoadScene(buildIndex, LoadSceneMode.Single);
            }

            //load player save data
            PlayerUIManager.instance.localPlayer.LoadGameDataFromCurrentCharacterData(ref WorldSaveGameManager.instance.currentCharacterData);
        }

        private void PrepareForSingleWorldSceneLoad()
        {
            if (loadingAdditiveScenesCoroutine != null)
            {
                StopCoroutine(loadingAdditiveScenesCoroutine);
                loadingAdditiveScenesCoroutine = null;
            }

            if (unloadAdditiveScenesCoroutine != null)
            {
                StopCoroutine(unloadAdditiveScenesCoroutine);
                unloadAdditiveScenesCoroutine = null;
            }

            if (requiredRenderersCoroutine != null)
            {
                StopCoroutine(requiredRenderersCoroutine);
                requiredRenderersCoroutine = null;
            }

            quedSceneIDs.Clear();
            quedUnloadSceneIDs.Clear();
            doNotUnLoadList.Clear();
            loadedScenes.Clear();
            quedScenesToLoad = 0;
            quedScenesToUnload = 0;
            sceneIsLoading = false;
            sceneIsUnloading = false;

            if (WorldLocationManager.instance != null)
                WorldLocationManager.instance.ResetForWorldSceneTransition();
        }

        private void OnUnitySceneLoaded(Scene scene, LoadSceneMode loadMode)
        {
            if (loadMode != LoadSceneMode.Single)
                return;

            RefreshCurrentWorldSceneID(scene);
            loadedScenes.Clear();
            loadedScenes.Add(scene);

            if (WorldLocationManager.instance != null)
                WorldLocationManager.instance.ResetForWorldSceneTransition();
        }

        private void RefreshCurrentWorldSceneID(Scene scene)
        {
            if (!scene.IsValid())
                return;

            if (scene.buildIndex >= 1 && scene.buildIndex <= 5)
            {
                world = scene.name;
            }
        }

        public string GetCurrentWorldSceneID()
        {
            Scene activeScene = SceneManager.GetActiveScene();

            if (activeScene.IsValid() && activeScene.buildIndex >= 1 && activeScene.buildIndex <= 5)
                return activeScene.name;

            return world;
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
            sceneIsLoading = true;
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
            float waitTime = 0.1f;

            //check to see if a scene is currently being loaded/unloaded and if it is, wait
            for (int i = 0; i < quedSceneIDs.Count; i++)
            {
                if (PlayerUIManager.instance.playerUILoadingScreenManager.LoadingScreenIsActive())
                    waitTime = 0;

                while (sceneIsLoading || sceneIsUnloading)
                {
                    yield return new WaitForSeconds(waitTime);
                }

                if (quedSceneIDs[i] == null)
                {
                    quedScenesToLoad--;
                    continue;
                }

                //sort through a qued list of scenes, and load them one by one
                LoadAdditiveScene(quedSceneIDs[i]);

                while (sceneIsLoading || sceneIsUnloading)
                {
                    yield return new WaitForSeconds(waitTime);
                }

                quedScenesToLoad--;

                if (quedScenesToLoad <= 0)
                    quedSceneIDs.Clear();

                yield return new WaitForFixedUpdate();
            }

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
                    sceneIsUnloading = true;
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
            float waitTime = 1.0f;

            for (int i = 0; i < quedUnloadSceneIDs.Count; i++)
            {
                if (PlayerUIManager.instance.playerUILoadingScreenManager.LoadingScreenIsActive())
                    waitTime = 0;

                while (sceneIsLoading || sceneIsUnloading)
                {
                    yield return new WaitForSeconds(waitTime);
                }

                //dont unload scenes while we are loading new areas as new areas may add these scens to do not unload list
                while (quedScenesToLoad > 0)
                {
                    yield return new WaitForSeconds(waitTime);
                }

                if (quedUnloadSceneIDs[i] == null)
                {
                    quedScenesToUnload--;
                    continue;
                }

                UnloadAdditiveScene(quedUnloadSceneIDs[i]);

                while (sceneIsLoading || sceneIsUnloading)
                {
                    yield return new WaitForSeconds(waitTime);
                }

                quedScenesToUnload--;

                if (quedScenesToUnload <= 0)
                    quedUnloadSceneIDs.Clear();

                yield return null;
            }

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

        public void CheckForUnrequiredScenes()
        {
            List<string> scenesToUnload = new List<string>();

            //get all currently loaded scenes
            for (int i = 0; i < loadedScenes.Count; i++)
            {
                scenesToUnload.Add(loadedScenes[i].name);
            }

            doNotUnLoadList = WorldLocationManager.instance.GenerateDoNotUnloadListBasedOnPlayerLocations();

            //compare all loaded scenes
            for (int i = 0; i < scenesToUnload.Count; i++)
            {
                if (doNotUnLoadList.Contains(scenesToUnload[i]))
                    scenesToUnload.Remove(scenesToUnload[i]);
            }

            UnloadAdditiveScenes(scenesToUnload);
        }

        public void CheckForRequiredRenderers()
        {
            if (WorldLocationManager.instance == null)
                return;

            if (requiredRenderersCoroutine != null)
                StopCoroutine(requiredRenderersCoroutine);

            WorldLocationSceneSet location = PlayerUIManager.instance.localPlayer.areaCurrentlyIn;

            if (location != null)
                requiredRenderersCoroutine = StartCoroutine(CheckForRequiredSceneRenderersCoroutine(location));
        }

        private IEnumerator CheckForRequiredSceneRenderersCoroutine(WorldLocationSceneSet location)
        {
            //wait until scenes have finished loading to search for renderers/root objects
            while (sceneIsLoading)
            {
                yield return new WaitForEndOfFrame();
            }

            List<string> scenesRelevantToLocationCurrentlyIn = location.GetRequiredSceneIDsForWorldLocation();
            List<int> sceneBuildIndexes = new List<int>();

            if (scenesRelevantToLocationCurrentlyIn != null)
            {
                for (int i = 0; i < scenesRelevantToLocationCurrentlyIn.Count; i++)
                {
                    sceneBuildIndexes.Add(GetBuildIndexFromSceneID(scenesRelevantToLocationCurrentlyIn[i]));
                }
            }

            for (int i = 0; i < WorldLocationManager.instance.worldLocationRenderers.Count; i++)
            {
                if (WorldLocationManager.instance.worldLocationRenderers[i] == null)
                    continue;

                if (sceneBuildIndexes.Contains(WorldLocationManager.instance.worldLocationRenderers[i].renderSceneID))
                {
                    if (PlayerUIManager.instance.playerUILoadingScreenManager.LoadingScreenIsActive())
                    {
                        WorldLocationManager.instance.worldLocationRenderers[i].ToggleMeshRenderers(true);
                    }
                    else
                    {
                        WorldLocationManager.instance.worldLocationRenderers[i].ToggleAllMeshRenderersOverTime(true);
                    }
                }
                else
                {
                    if (PlayerUIManager.instance.playerUILoadingScreenManager.LoadingScreenIsActive())
                    {
                        WorldLocationManager.instance.worldLocationRenderers[i].ToggleMeshRenderers(false);
                    }
                    else
                    {
                        WorldLocationManager.instance.worldLocationRenderers[i].ToggleAllMeshRenderersOverTime(false);
                    }
                }
            }

            yield return null;
        }

        public int GetBuildIndexFromSceneID(string sceneID)
        {
            int buildIndex = SceneUtility.GetBuildIndexByScenePath(sceneID);
            return buildIndex;
        }
    }
}
