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
        private Coroutine delayedUnloadCoroutine;
        private readonly Dictionary<string, float> pendingUnrequiredSceneUnloadTimes = new Dictionary<string, float>();

        [Header("Scene Streaming")]
        [SerializeField] private float unrequiredSceneUnloadDelay = 12f;
        [SerializeField] private bool loadNonWorld01MapsAllAtOnce = true;
        [SerializeField] private string roomStreamingWorldSceneName = "World_01";
        private bool generatedWorldAllScenesQueued = false;

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
            PlayerUIManager.instance.playerUILoadingScreenManager.SetProgress(0.02f, "Loading World");

            string worldScenePath = SceneUtility.GetScenePathByBuildIndex(buildIndex);
            

            if (string.IsNullOrEmpty(worldScenePath))
            {
                
                return;
            }

            bool startedNetworkSceneLoad = false;

            if (NetworkManager.Singleton != null &&
                NetworkManager.Singleton.SceneManager != null &&
                (NetworkManager.Singleton.IsHost || NetworkManager.Singleton.IsServer))
            {
                string worldSceneName = Path.GetFileNameWithoutExtension(worldScenePath);
                
                
                var loadSceneStatus = NetworkManager.Singleton.SceneManager.LoadScene(worldSceneName, LoadSceneMode.Single);
                
                startedNetworkSceneLoad = loadSceneStatus == SceneEventProgressStatus.Started;
                

                if (!startedNetworkSceneLoad)
                {
                    
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
            pendingUnrequiredSceneUnloadTimes.Clear();
            generatedWorldAllScenesQueued = false;
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
            generatedWorldAllScenesQueued = false;

            if (WorldLocationManager.instance != null)
                WorldLocationManager.instance.ResetForWorldSceneTransition();
        }

        public void LogLoadingStateSnapshot(string source)
        {
            string activeSceneName = SceneManager.GetActiveScene().name;
            bool loadingScreenActive =
                PlayerUIManager.instance != null &&
                PlayerUIManager.instance.playerUILoadingScreenManager != null &&
                PlayerUIManager.instance.playerUILoadingScreenManager.LoadingScreenIsActive();

            bool aiLoading = WorldAIManager.instance != null && WorldAIManager.instance.isPerformingLoadingOperation;
            bool networkListening = NetworkManager.Singleton != null && NetworkManager.Singleton.IsListening;
            bool networkHost = NetworkManager.Singleton != null && NetworkManager.Singleton.IsHost;
            bool networkServer = NetworkManager.Singleton != null && NetworkManager.Singleton.IsServer;
            bool networkClient = NetworkManager.Singleton != null && NetworkManager.Singleton.IsClient;

            
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

        public bool ShouldLoadGeneratedWorldAllAtOnce()
        {
            return loadNonWorld01MapsAllAtOnce &&
                   !string.IsNullOrWhiteSpace(roomStreamingWorldSceneName) &&
                   !string.Equals(GetCurrentWorldSceneID(), roomStreamingWorldSceneName, System.StringComparison.OrdinalIgnoreCase) &&
                   !string.IsNullOrWhiteSpace(GetGeneratedWorldAreaScenePrefix());
        }

        public void LoadAllGeneratedWorldAreaScenes()
        {
            if (!ShouldLoadGeneratedWorldAllAtOnce())
                return;

            if (generatedWorldAllScenesQueued)
                return;

            List<string> scenesToLoad = GetGeneratedWorldAreaSceneNames();

            if (scenesToLoad.Count <= 0)
                return;

            generatedWorldAllScenesQueued = true;
            LoadAdditiveScenes(scenesToLoad);
        }

        public List<string> GetGeneratedWorldAreaSceneNames()
        {
            List<string> sceneNames = new List<string>();
            string areaScenePrefix = GetGeneratedWorldAreaScenePrefix();

            if (string.IsNullOrWhiteSpace(areaScenePrefix))
                return sceneNames;

            for (int i = 0; i < SceneManager.sceneCountInBuildSettings; i++)
            {
                string scenePath = SceneUtility.GetScenePathByBuildIndex(i);

                if (string.IsNullOrWhiteSpace(scenePath))
                    continue;

                string sceneName = Path.GetFileNameWithoutExtension(scenePath);

                if (!sceneName.StartsWith(areaScenePrefix, System.StringComparison.OrdinalIgnoreCase))
                    continue;

                if (!sceneNames.Contains(sceneName))
                    sceneNames.Add(sceneName);
            }

            return sceneNames;
        }

        private string GetGeneratedWorldAreaScenePrefix()
        {
            string currentWorldSceneID = GetCurrentWorldSceneID();

            if (string.IsNullOrWhiteSpace(currentWorldSceneID))
                return string.Empty;

            System.Text.RegularExpressions.Match worldIndexMatch =
                System.Text.RegularExpressions.Regex.Match(currentWorldSceneID, @"^World_(\d+)$", System.Text.RegularExpressions.RegexOptions.IgnoreCase);

            if (!worldIndexMatch.Success)
                return string.Empty;

            return $"Area_{worldIndexMatch.Groups[1].Value}_";
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
            SceneEventProgressStatus loadSceneStatus = SceneEventProgressStatus.Started;

            if (NetworkManager.Singleton != null &&
                NetworkManager.Singleton.SceneManager != null &&
                (NetworkManager.Singleton.IsHost || NetworkManager.Singleton.IsServer))
            {
                
                loadSceneStatus = NetworkManager.Singleton.SceneManager.LoadScene(sceneName, LoadSceneMode.Additive);
                
            }
            else
            {
                
                StartCoroutine(LoadAdditiveSceneNonNetworkCoroutine(sceneName));
                return;
            }

            if (loadSceneStatus != SceneEventProgressStatus.Started)
            {
                
                StartCoroutine(LoadAdditiveSceneNonNetworkCoroutine(sceneName));
            }
        }

        private IEnumerator LoadAdditiveSceneNonNetworkCoroutine(string sceneName)
        {
            
            AsyncOperation loadingOperation = null;

            try
            {
                loadingOperation = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
            }
            catch (System.Exception)
            {
                
                sceneIsLoading = false;
                yield break;
            }

            if (loadingOperation == null)
            {
                sceneIsLoading = false;
                yield break;
            }

            while (!loadingOperation.isDone)
            {
                
                yield return null;
            }

            Scene scene = SceneManager.GetSceneByName(sceneName);

            if (scene.IsValid() && scene.isLoaded)
            {
                bool alreadyTracked = false;

                for (int i = 0; i < loadedScenes.Count; i++)
                {
                    if (loadedScenes[i].IsValid() && loadedScenes[i].name == scene.name)
                    {
                        alreadyTracked = true;
                        break;
                    }
                }

                if (!alreadyTracked)
                    loadedScenes.Add(scene);
            }

            sceneIsLoading = false;
            
            CheckForRequiredRenderers();
        }

        //used to load multiple additive scenes at once when entering new area
        public void LoadAdditiveScenes(List<string> scenesToLoad)
        {
            

            if (NetworkManager.Singleton != null &&
                NetworkManager.Singleton.IsClient &&
                !NetworkManager.Singleton.IsServer)
            {
                
                return;
            }

            if (scenesToLoad == null)
            {
                
                return;
            }

            //pass all of our scenes to load to our qued scene list
            for (int i = 0; i < scenesToLoad.Count; i++)
            {
                if (string.IsNullOrWhiteSpace(scenesToLoad[i]))
                    continue;

                if (IsSceneLoadedOrQueued(scenesToLoad[i]))
                    continue;

                quedSceneIDs.Add(scenesToLoad[i]);
            }

            quedScenesToLoad = quedSceneIDs.Count;
            

            if (quedScenesToLoad <= 0)
                return;

            if (loadingAdditiveScenesCoroutine != null)
                StopCoroutine(loadingAdditiveScenesCoroutine);

            loadingAdditiveScenesCoroutine = StartCoroutine(LoadAdditiveScenesCoroutine());
            
        }

        private bool IsSceneLoadedOrQueued(string sceneName)
        {
            if (quedSceneIDs.Contains(sceneName))
                return true;

            Scene scene = SceneManager.GetSceneByName(sceneName);

            if (scene.IsValid() && scene.isLoaded)
                return true;

            for (int i = 0; i < loadedScenes.Count; i++)
            {
                if (loadedScenes[i].IsValid() && loadedScenes[i].isLoaded && loadedScenes[i].name == sceneName)
                    return true;
            }

            return false;
        }

        private IEnumerator LoadAdditiveScenesCoroutine()
        {
            
            yield return null;

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
            if (ShouldLoadGeneratedWorldAllAtOnce())
            {
                doNotUnLoadList = GetGeneratedWorldAreaSceneNames();
                doNotUnLoadList.Add(GetCurrentWorldSceneID());
                pendingUnrequiredSceneUnloadTimes.Clear();
                return;
            }

            List<string> scenesToUnload = new List<string>();

            //get all currently loaded scenes
            for (int i = 0; i < loadedScenes.Count; i++)
            {
                scenesToUnload.Add(loadedScenes[i].name);
            }

            doNotUnLoadList = WorldLocationManager.instance.GenerateDoNotUnloadListBasedOnPlayerLocations();

            for (int i = doNotUnLoadList.Count - 1; i >= 0; i--)
            {
                if (pendingUnrequiredSceneUnloadTimes.ContainsKey(doNotUnLoadList[i]))
                    pendingUnrequiredSceneUnloadTimes.Remove(doNotUnLoadList[i]);
            }

            //compare all loaded scenes
            for (int i = scenesToUnload.Count - 1; i >= 0; i--)
            {
                if (doNotUnLoadList.Contains(scenesToUnload[i]))
                    scenesToUnload.Remove(scenesToUnload[i]);
            }

            QueueUnrequiredScenesForDelayedUnload(scenesToUnload);
        }

        private void QueueUnrequiredScenesForDelayedUnload(List<string> scenesToUnload)
        {
            if (scenesToUnload == null || scenesToUnload.Count <= 0)
                return;

            List<string> expiredScenes = new List<string>();

            for (int i = 0; i < scenesToUnload.Count; i++)
            {
                string sceneName = scenesToUnload[i];

                if (string.IsNullOrWhiteSpace(sceneName))
                    continue;

                if (!pendingUnrequiredSceneUnloadTimes.ContainsKey(sceneName))
                    pendingUnrequiredSceneUnloadTimes[sceneName] = Time.time + unrequiredSceneUnloadDelay;

                if (Time.time >= pendingUnrequiredSceneUnloadTimes[sceneName])
                    expiredScenes.Add(sceneName);
            }

            for (int i = 0; i < expiredScenes.Count; i++)
            {
                pendingUnrequiredSceneUnloadTimes.Remove(expiredScenes[i]);
            }

            if (expiredScenes.Count > 0)
                UnloadAdditiveScenes(expiredScenes);

            if (pendingUnrequiredSceneUnloadTimes.Count > 0 && delayedUnloadCoroutine == null)
                delayedUnloadCoroutine = StartCoroutine(DelayedUnrequiredSceneUnloadCoroutine());
        }

        private IEnumerator DelayedUnrequiredSceneUnloadCoroutine()
        {
            while (pendingUnrequiredSceneUnloadTimes.Count > 0)
            {
                float nextUnloadTime = float.MaxValue;

                foreach (KeyValuePair<string, float> pair in pendingUnrequiredSceneUnloadTimes)
                {
                    if (pair.Value < nextUnloadTime)
                        nextUnloadTime = pair.Value;
                }

                yield return new WaitForSeconds(Mathf.Max(0.25f, nextUnloadTime - Time.time));
                CheckForUnrequiredScenes();
            }

            delayedUnloadCoroutine = null;
        }

        public void CheckForRequiredRenderers()
        {
            

            if (WorldLocationManager.instance == null)
            {
                
                return;
            }

            if (requiredRenderersCoroutine != null)
                StopCoroutine(requiredRenderersCoroutine);

            WorldLocationSceneSet location = PlayerUIManager.instance.localPlayer.areaCurrentlyIn;

            if (location != null)
            {
                
                requiredRenderersCoroutine = StartCoroutine(CheckForRequiredSceneRenderersCoroutine(location));
            }
            else
            {
                
            }
        }

        private IEnumerator CheckForRequiredSceneRenderersCoroutine(WorldLocationSceneSet location)
        {
            

            //wait until scenes have finished loading to search for renderers/root objects
            while (sceneIsLoading)
            {
                
                yield return new WaitForEndOfFrame();
            }

            if (ShouldLoadGeneratedWorldAllAtOnce())
            {
                for (int i = 0; i < WorldLocationManager.instance.worldLocationRenderers.Count; i++)
                {
                    if (WorldLocationManager.instance.worldLocationRenderers[i] == null)
                        continue;

                    WorldLocationManager.instance.worldLocationRenderers[i].ToggleMeshRenderers(true);
                    WorldLocationManager.instance.worldLocationRenderers[i].ToggleRootObjects(true);
                }

                
                yield break;
            }

            List<string> scenesRelevantToLocationCurrentlyIn = location.GetRequiredSceneIDsForWorldLocation();
            List<int> sceneBuildIndexes = new List<int>();

            if (scenesRelevantToLocationCurrentlyIn != null)
            {
                for (int i = 0; i < scenesRelevantToLocationCurrentlyIn.Count; i++)
                {
                    int sceneBuildIndex = GetBuildIndexFromSceneID(scenesRelevantToLocationCurrentlyIn[i]);

                    if (!sceneBuildIndexes.Contains(sceneBuildIndex))
                        sceneBuildIndexes.Add(sceneBuildIndex);
                }
            }

            if (doNotUnLoadList != null)
            {
                for (int i = 0; i < doNotUnLoadList.Count; i++)
                {
                    int sceneBuildIndex = GetBuildIndexFromSceneID(doNotUnLoadList[i]);

                    if (!sceneBuildIndexes.Contains(sceneBuildIndex))
                        sceneBuildIndexes.Add(sceneBuildIndex);
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
                    // Keep loaded areas visible until their additive scene is
                    // actually unloaded. Turning renderers off immediately at
                    // room borders causes visible pop-out when the player moves
                    // back and forth across generated World_02 room triggers.
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

    internal static class BuildRuntimeLogger
    {
        private static readonly object FileLock = new object();
        private static string logPath;
        private static bool initialized;
        private static bool unityLogHooked;
        private static System.Threading.Timer loadingWatchdogTimer;
        private static volatile bool loadingWatchActive;
        private static volatile int loadingWatchSequence;
        private static System.DateTime lastMainThreadHeartbeatUtc = System.DateTime.UtcNow;
        private static System.DateTime lastMainThreadHeartbeatWriteUtc = System.DateTime.MinValue;
        private static string lastMainThreadHeartbeatContext = "Not started";
        private static string lastMainThreadHeartbeatWrittenContext = string.Empty;

        public static string LogPath
        {
            get
            {
                Initialize();
                return logPath;
            }
        }

        public static void Log(string message)
        {
            Write("INFO", message);
            
        }

        public static void Warning(string message)
        {
            Write("WARN", message);
            
        }

        public static void Error(string message)
        {
            Write("ERROR", message);
            
        }

        private static void Initialize()
        {
            if (initialized)
                return;

            initialized = true;

            try
            {
                string directory = Path.Combine(Application.persistentDataPath, "BuildLogs");
                string rootDirectory = Path.GetDirectoryName(Application.dataPath);

                if (string.IsNullOrEmpty(rootDirectory))
                    rootDirectory = Application.persistentDataPath;

                directory = Path.Combine(rootDirectory, "logs");
                Directory.CreateDirectory(directory);
                string timestamp = System.DateTime.Now.ToString("yyyyMMdd-HHmmss");
                logPath = Path.Combine(directory, timestamp + ".txt");

                WriteRaw("=== Build Runtime Log ===");
                WriteRaw("LogPath: " + logPath);
                WriteRaw("UnityVersion: " + Application.unityVersion);
                WriteRaw("Product: " + Application.companyName + "/" + Application.productName);
                WriteRaw("Platform: " + Application.platform);
                WriteRaw("DataPath: " + Application.dataPath);
                WriteRaw("PersistentDataPath: " + Application.persistentDataPath);

                if (!unityLogHooked)
                {
                    Application.logMessageReceivedThreaded += HandleUnityLogMessage;
                    unityLogHooked = true;
                }
            }
            catch (System.Exception)
            {
                
            }
        }

        public static void BeginLoadingWatch(string reason)
        {
            Initialize();
            loadingWatchActive = true;
            loadingWatchSequence++;
            MainThreadHeartbeat("BeginLoadingWatch: " + reason);
            WriteRaw("[" + System.DateTime.Now.ToString("HH:mm:ss.fff") + "] [WATCH] Loading watch BEGIN reason=" + reason + " sequence=" + loadingWatchSequence);

            if (loadingWatchdogTimer == null)
                loadingWatchdogTimer = new System.Threading.Timer(WriteLoadingWatchdogSnapshot, null, 1000, 1000);
        }

        public static void EndLoadingWatch(string reason)
        {
            MainThreadHeartbeat("EndLoadingWatch: " + reason);
            loadingWatchActive = false;
            WriteRaw("[" + System.DateTime.Now.ToString("HH:mm:ss.fff") + "] [WATCH] Loading watch END reason=" + reason + " sequence=" + loadingWatchSequence);
        }

        public static void MainThreadHeartbeat(string context)
        {
            lastMainThreadHeartbeatUtc = System.DateTime.UtcNow;
            lastMainThreadHeartbeatContext = context;

            if (loadingWatchActive &&
                ((lastMainThreadHeartbeatUtc - lastMainThreadHeartbeatWriteUtc).TotalSeconds >= 0.5 ||
                 lastMainThreadHeartbeatWrittenContext != context))
            {
                lastMainThreadHeartbeatWriteUtc = lastMainThreadHeartbeatUtc;
                lastMainThreadHeartbeatWrittenContext = context;
                WriteRaw("[" + System.DateTime.Now.ToString("HH:mm:ss.fff") + "] [MAIN] heartbeat context=" + context);
            }
        }

        private static void WriteLoadingWatchdogSnapshot(object state)
        {
            if (!loadingWatchActive)
                return;

            double secondsSinceHeartbeat = (System.DateTime.UtcNow - lastMainThreadHeartbeatUtc).TotalSeconds;
            string prefix = secondsSinceHeartbeat >= 2.0 ? "[WATCH-STALLED]" : "[WATCH]";
            WriteRaw("[" + System.DateTime.Now.ToString("HH:mm:ss.fff") + "] " + prefix +
                     " loadingActive=true secondsSinceMainThreadHeartbeat=" + secondsSinceHeartbeat.ToString("0.00") +
                     " lastMainThreadContext=" + lastMainThreadHeartbeatContext +
                     " sequence=" + loadingWatchSequence);
        }

        private static void HandleUnityLogMessage(string condition, string stackTrace, LogType type)
        {
            if (string.IsNullOrEmpty(logPath))
                return;

            WriteRaw("[" + System.DateTime.Now.ToString("HH:mm:ss.fff") + "] [UNITY-" + type + "] " + condition);

            if (!string.IsNullOrWhiteSpace(stackTrace) && (type == LogType.Exception || type == LogType.Error || type == LogType.Assert))
                WriteRaw(stackTrace);
        }

        private static void Write(string level, string message)
        {
            Initialize();
            WriteRaw("[" + System.DateTime.Now.ToString("HH:mm:ss.fff") + "] [" + level + "] " + message);
        }

        private static void WriteRaw(string message)
        {
            if (string.IsNullOrEmpty(logPath))
                return;

            try
            {
                lock (FileLock)
                {
                    File.AppendAllText(logPath, message + System.Environment.NewLine);
                }
            }
            catch
            {
                // Keep diagnostics from affecting gameplay or scene loading.
            }
        }
    }
}
