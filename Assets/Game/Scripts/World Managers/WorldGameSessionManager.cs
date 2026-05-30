using UnityEngine;
using Unity.Netcode;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using System.Collections;
using Unity.Netcode.Transports.UTP;
using Unity.Networking.Transport.Relay;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Relay;
using Unity.Services.Relay.Models;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace baodeag
{
    public enum SessionLaunchMode
    {
        None = 0,
        Singleplayer = 1,
        Multiplayer = 2
    }

    public enum SessionEndGameActionType
    {
        None = 0,
        RetryCurrentMap = 1,
        ContinueProgression = 2,
        ReturnToTitle = 3
    }

    public class WorldGameSessionManager : MonoBehaviour
    {
        public static WorldGameSessionManager instance;
        private const int MaxDeathsPerMapBeforeLose = 5;

        [Header("Active Players In Session")]
        public List<PlayerManager> players = new List<PlayerManager>();

        private Coroutine revivalCoroutine;
        private Coroutine pendingMapEntryCoroutine;
        private Coroutine returnToTitleCoroutine;
        private Coroutine autoContinueVictoryCoroutine;

        private UnityTransport unityTransport;
        private const ushort DefaultUnityTransportPort = 7777;
        private const int DefaultRelayMaxConnections = 4;
        private const string RelayConnectionType = "dtls";
        private Coroutine joinAsClientCoroutine;
        private string currentRelayJoinCode = string.Empty;
        private string checkedRelayJoinCode = string.Empty;
        private JoinAllocation checkedRelayJoinAllocation;
        private bool isStartingRelaySession;
        private SessionLaunchMode currentLaunchMode = SessionLaunchMode.Singleplayer;
        private readonly Dictionary<ulong, int> playerDeathsThisMap = new Dictionary<ulong, int>();
        private int trackedDeathMapIndex = -1;
        private bool sessionLoseTriggered;
        private bool sessionWinTriggered;
        private bool pendingVictoryShouldLoadNextScene;
        private int pendingVictoryNextSceneBuildIndex = -1;
        private int pendingVictoryUnlockedMapIndex = -1;

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

            unityTransport = NetworkManager.Singleton != null
                ? NetworkManager.Singleton.NetworkConfig.NetworkTransport as UnityTransport
                : null;

            ConfigureUnityTransportPortForCurrentProject();
        }

        private void Start()
        {
            DontDestroyOnLoad(gameObject);
        }

        private void OnEnable()
        {
            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        private void OnDisable()
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }

        private void OnSceneLoaded(Scene newScene, LoadSceneMode loadMode)
        {
            if (loadMode != LoadSceneMode.Single)
                return;

            if (newScene.buildIndex <= 0)
                return;

            ResetTransientSessionStateForCurrentMap();

            if (!GameProgressionManager.Instance.HasPendingTransitionSiteOfGrace())
                return;

            if (pendingMapEntryCoroutine != null)
                StopCoroutine(pendingMapEntryCoroutine);

            pendingMapEntryCoroutine = StartCoroutine(HandlePendingMapEntryCoroutine());
        }

        public void ProcessPendingMapEntryWithoutSceneReload()
        {
            if (!GameProgressionManager.Instance.HasPendingTransitionSiteOfGrace())
                return;

            if (pendingMapEntryCoroutine != null)
                StopCoroutine(pendingMapEntryCoroutine);

            pendingMapEntryCoroutine = StartCoroutine(HandlePendingMapEntryCoroutine());
        }

        public void ReturnToTitleAfterVictory(float delay = 6f)
        {
            if (returnToTitleCoroutine != null)
                StopCoroutine(returnToTitleCoroutine);

            returnToTitleCoroutine = StartCoroutine(ReturnToTitleAfterVictoryCoroutine(delay));
        }

        public void ReturnToTitleAfterDefeat(float delay = 6f)
        {
            if (returnToTitleCoroutine != null)
                StopCoroutine(returnToTitleCoroutine);

            returnToTitleCoroutine = StartCoroutine(ReturnToTitleAfterVictoryCoroutine(delay));
        }

        public void ScheduleMapTransition(bool shouldLoadNextScene, int nextSceneBuildIndex, bool gameWon, int unlockedMapIndex)
        {
            pendingVictoryShouldLoadNextScene = shouldLoadNextScene;
            pendingVictoryNextSceneBuildIndex = nextSceneBuildIndex;
            pendingVictoryUnlockedMapIndex = unlockedMapIndex;
        }

        public bool TryRegisterPlayerDeathForLose(ulong playerClientId, int mapIndex, out int deathCount)
        {
            deathCount = 0;

            if (sessionLoseTriggered || sessionWinTriggered)
                return false;

            SyncDeathTrackingMap(mapIndex);

            playerDeathsThisMap.TryGetValue(playerClientId, out int previousDeaths);
            deathCount = previousDeaths + 1;
            playerDeathsThisMap[playerClientId] = deathCount;

            if (deathCount < MaxDeathsPerMapBeforeLose)
                return false;

            sessionLoseTriggered = true;
            sessionWinTriggered = false;
            CancelPendingRevival();
            ClearPendingVictoryTransition();

            return true;
        }

        public void HandleSessionLose(int mapIndex, ulong failedPlayerClientId, int deathCount)
        {
            if (sessionWinTriggered)
                return;

            SyncDeathTrackingMap(mapIndex);
            sessionLoseTriggered = true;
            CancelPendingRevival();
            ClearPendingVictoryTransition();

            if (PlayerUIManager.instance != null && PlayerUIManager.instance.playerUIPopUpManager != null)
            {
                PlayerUIManager.instance.playerUIPopUpManager.ShowLoseEndGameOverlay();
            }

            if (WorldSaveGameManager.instance != null)
                WorldSaveGameManager.instance.SetCurrentCharacterPlayTimeFrozen(true);

            if (WorldAIManager.instance != null)
                WorldAIManager.instance.DisableAllBossFights();
        }

        public void HandleSessionVictory(bool canContinueProgression, float popupDelay = 0f)
        {
            if (sessionLoseTriggered || sessionWinTriggered)
                return;

            sessionWinTriggered = true;
            CancelPendingRevival();

            if (WorldSaveGameManager.instance != null)
                WorldSaveGameManager.instance.SetCurrentCharacterPlayTimeFrozen(true);

            if (PlayerUIManager.instance != null && PlayerUIManager.instance.playerUIPopUpManager != null)
            {
                PlayerUIManager.instance.playerUIPopUpManager.ShowVictoryEndGameOverlayDelayed(canContinueProgression, popupDelay);
            }
        }

        public void ExecuteSynchronizedEndGameAction(SessionEndGameActionType action, bool performWorldTransition)
        {
            switch (action)
            {
                case SessionEndGameActionType.RetryCurrentMap:
                    if (performWorldTransition)
                        RetryCurrentMapFromStart();
                    break;
                case SessionEndGameActionType.ContinueProgression:
                    if (performWorldTransition)
                        ContinuePendingVictoryFlow();
                    break;
                case SessionEndGameActionType.ReturnToTitle:
                    ReturnToTitleFromEndGame();
                    break;
            }
        }

        public void RetryCurrentMapFromStart()
        {
            int currentMapIndex = GameProgressionManager.Instance.CurrentMapIndex;
            int sceneBuildIndex;

            if (!GameProgressionManager.Instance.PrepareTransitionToMap(currentMapIndex, out sceneBuildIndex))
                sceneBuildIndex = GameProgressionManager.Instance.GetSceneBuildIndexForCurrentMap();

            int entrySiteOfGraceID = GameProgressionManager.Instance.GetEntrySiteOfGraceIDForCurrentMap();

            if (entrySiteOfGraceID >= 0)
                SetLastRestedSiteOfGrace(entrySiteOfGraceID);

            ClearPendingVictoryTransition();
            ResetTransientSessionStateForCurrentMap();
            LoadSceneForProgression(sceneBuildIndex);
        }

        public void ContinuePendingVictoryFlow()
        {
            if (pendingVictoryShouldLoadNextScene &&
                pendingVictoryNextSceneBuildIndex >= 0 &&
                pendingVictoryNextSceneBuildIndex != SceneManager.GetActiveScene().buildIndex)
            {
                LoadSceneForProgression(pendingVictoryNextSceneBuildIndex);
                ClearPendingVictoryTransition();
                return;
            }

            if (pendingVictoryUnlockedMapIndex >= 0)
            {
                ResetTransientSessionStateForCurrentMap();
                ProcessPendingMapEntryWithoutSceneReload();
                ClearPendingVictoryTransition();
                return;
            }

            RetryCurrentMapFromStart();
        }

        public void AutoContinuePendingVictoryFlow(float delay = 3f)
        {
            if (autoContinueVictoryCoroutine != null)
                StopCoroutine(autoContinueVictoryCoroutine);

            autoContinueVictoryCoroutine = StartCoroutine(AutoContinuePendingVictoryFlowCoroutine(delay));
        }

        public void ReturnToTitleFromEndGame()
        {
            ReturnToTitleAfterVictory(0f);
        }

        public int GetDeathCountForPlayerThisMap(ulong playerClientId)
        {
            if (playerDeathsThisMap.TryGetValue(playerClientId, out int deathCount))
                return deathCount;

            return 0;
        }

        public int GetMaxDeathsPerMapBeforeLoseCount()
        {
            return MaxDeathsPerMapBeforeLose;
        }

        public bool CanRevivePlayers()
        {
            return !sessionLoseTriggered && !sessionWinTriggered;
        }

        private void LoadSceneForProgression(int nextSceneBuildIndex)
        {
            if (WorldAIManager.instance != null)
            {
                WorldAIManager.instance.PrepareForWorldSceneTransition();
            }

            if (WorldSceneManager.instance != null)
            {
                WorldSceneManager.instance.LoadWorldScene(nextSceneBuildIndex);
                return;
            }

            SceneManager.LoadScene(nextSceneBuildIndex, LoadSceneMode.Single);
        }

        private IEnumerator HandlePendingMapEntryCoroutine()
        {
            BuildRuntimeLogger.Log("WorldGameSessionManager.HandlePendingMapEntryCoroutine begin");
            int targetSiteOfGraceID = GameProgressionManager.Instance.ConsumePendingTransitionSiteOfGraceID();
            BuildRuntimeLogger.Log($"WorldGameSessionManager.HandlePendingMapEntryCoroutine targetSiteOfGraceID={targetSiteOfGraceID}");

            if (targetSiteOfGraceID < 0)
            {
                BuildRuntimeLogger.Warning("WorldGameSessionManager.HandlePendingMapEntryCoroutine no target site of grace; deactivating loading screen");
                if (PlayerUIManager.instance != null)
                    PlayerUIManager.instance.playerUILoadingScreenManager.DeactivateLoadingScreen(1.5f);

                pendingMapEntryCoroutine = null;
                yield break;
            }

            float timeout = 10f;
            float elapsedTime = 0f;

            while ((PlayerUIManager.instance == null ||
                    PlayerUIManager.instance.localPlayer == null ||
                    WorldObjectManager.instance == null) &&
                   elapsedTime < timeout)
            {
                BuildRuntimeLogger.MainThreadHeartbeat($"HandlePendingMapEntry waiting managers elapsed={elapsedTime:0.00}");
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            if (PlayerUIManager.instance == null || PlayerUIManager.instance.localPlayer == null)
            {
                BuildRuntimeLogger.Warning("WorldGameSessionManager.HandlePendingMapEntryCoroutine abort: PlayerUIManager/localPlayer missing after timeout");
                pendingMapEntryCoroutine = null;
                yield break;
            }

            SetLoadingProgress(0.05f, "Loading Map");

            bool loadGeneratedWorldAllAtOnce = WorldSceneManager.instance != null &&
                                               WorldSceneManager.instance.ShouldLoadGeneratedWorldAllAtOnce();
            BuildRuntimeLogger.Log($"WorldGameSessionManager.HandlePendingMapEntryCoroutine loadGeneratedWorldAllAtOnce={loadGeneratedWorldAllAtOnce}");

            if (loadGeneratedWorldAllAtOnce)
            {
                yield return WaitForRequiredAreaScenes(null, 30f, 0.05f, 0.75f);
            }
            else
            {
                BuildRuntimeLogger.Log("WorldGameSessionManager.HandlePendingMapEntryCoroutine yielding before initial area trigger");
                yield return null;

                WorldLocationSceneSet initialArea = TriggerInitialAreaLoadForPlayer(PlayerUIManager.instance.localPlayer);
                BuildRuntimeLogger.Log($"WorldGameSessionManager.HandlePendingMapEntryCoroutine initialArea={(initialArea != null ? initialArea.name : "null")}");
                yield return WaitForRequiredAreaScenes(initialArea, 30f, 0.05f, 0.55f);
            }

            elapsedTime = 0f;

            while ((WorldObjectManager.instance == null ||
                    WorldObjectManager.instance.sitesOfGrace == null ||
                    WorldObjectManager.instance.sitesOfGrace.Count == 0) &&
                   elapsedTime < timeout)
            {
                BuildRuntimeLogger.MainThreadHeartbeat($"HandlePendingMapEntry waiting sitesOfGrace elapsed={elapsedTime:0.00}");
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            SiteOfGraceInteractable targetSiteOfGrace = null;

            if (WorldObjectManager.instance != null && WorldObjectManager.instance.sitesOfGrace != null)
            {
                for (int i = 0; i < WorldObjectManager.instance.sitesOfGrace.Count; i++)
                {
                    SiteOfGraceInteractable siteOfGrace = WorldObjectManager.instance.sitesOfGrace[i];

                    if (siteOfGrace != null && siteOfGrace.siteOfGraceID == targetSiteOfGraceID)
                    {
                        targetSiteOfGrace = siteOfGrace;
                        break;
                    }
                }

                if (targetSiteOfGrace == null && WorldObjectManager.instance.sitesOfGrace.Count > 0)
                    targetSiteOfGrace = WorldObjectManager.instance.sitesOfGrace[0];
            }

            if (targetSiteOfGrace != null)
            {
                BuildRuntimeLogger.Log($"WorldGameSessionManager.HandlePendingMapEntryCoroutine teleporting to siteOfGraceID={targetSiteOfGrace.siteOfGraceID}");
                PlayerManager localPlayer = PlayerUIManager.instance.localPlayer;
                targetSiteOfGrace.TeleportPlayerToSiteOfGrace(localPlayer, false);
                localPlayer.playerNetworkManager.lastSiteOfGraceUsed.Value = targetSiteOfGrace.siteOfGraceID;
                WorldSaveGameManager.instance.currentCharacterData.lastSiteOfGraceRestedAt = targetSiteOfGrace.siteOfGraceID;
                WorldSaveGameManager.instance.SaveGame();

                // Wait one frame for the teleport position to take effect, then
                // manually fire area loading so additive scenes are queued while
                // the loading screen is still active (bypasses OnTriggerEnter
                // timing issues and catches cases where the trigger collider is
                // too small to overlap the spawn point).
                yield return null;

                if (loadGeneratedWorldAllAtOnce)
                {
                    if (WorldSceneManager.instance != null)
                        WorldSceneManager.instance.CheckForRequiredRenderers();

                    SetLoadingProgress(0.95f, "Loading Map");
                }
                else
                {
                    BuildRuntimeLogger.Log("WorldGameSessionManager.HandlePendingMapEntryCoroutine yielding before nearest area trigger");
                    yield return null;

                    WorldLocationSceneSet entryArea = TriggerNearestAreaLoadForPlayer(localPlayer, targetSiteOfGrace.transform.position);
                    BuildRuntimeLogger.Log($"WorldGameSessionManager.HandlePendingMapEntryCoroutine entryArea={(entryArea != null ? entryArea.name : "null")}");
                    yield return WaitForRequiredAreaScenes(entryArea, 30f, 0.55f, 0.95f);
                }
            }
            else
            {
                BuildRuntimeLogger.Warning("WorldGameSessionManager.HandlePendingMapEntryCoroutine target site of grace not found; waiting fallback");
                SetLoadingProgress(0.95f, "Loading Map");
                yield return new WaitForSeconds(4f);
            }

            if (PlayerUIManager.instance != null)
            {
                SetLoadingProgress(1f, "Ready");
                PlayerUIManager.instance.playerUILoadingScreenManager.DeactivateLoadingScreen(2.5f);
            }

            BuildRuntimeLogger.Log("WorldGameSessionManager.HandlePendingMapEntryCoroutine end");
            pendingMapEntryCoroutine = null;
        }

        private IEnumerator AutoContinuePendingVictoryFlowCoroutine(float delay)
        {
            while (delay > 0f)
            {
                if (sessionLoseTriggered || sessionWinTriggered)
                {
                    autoContinueVictoryCoroutine = null;
                    yield break;
                }

                delay -= Time.deltaTime;
                yield return null;
            }

            ContinuePendingVictoryFlow();
            autoContinueVictoryCoroutine = null;
        }

        /// <summary>
        /// Finds the nearest EventTriggerLoadScene to <paramref name="origin"/> within
        /// <paramref name="searchRadius"/> metres and manually fires area loading for
        /// <paramref name="player"/>. This is called right after a world-transition
        /// teleport so that additive sub-scenes start loading while a loading screen
        /// is still covering the screen.
        /// </summary>
        private WorldLocationSceneSet TriggerNearestAreaLoadForPlayer(PlayerManager player, Vector3 origin, float searchRadius = 120f)
        {
            BuildRuntimeLogger.Log($"WorldGameSessionManager.TriggerNearestAreaLoadForPlayer begin player={(player != null ? player.name : "null")} origin={origin} radius={searchRadius}");
            List<EventTriggerLoadScene> allTriggers = EventTriggerLoadScene.GetRegisteredTriggersSnapshot();
            BuildRuntimeLogger.Log($"WorldGameSessionManager.TriggerNearestAreaLoadForPlayer registeredTriggers={allTriggers.Count}");

            EventTriggerLoadScene nearest = null;
            float nearestDistSq = searchRadius * searchRadius;

            for (int i = 0; i < allTriggers.Count; i++)
            {
                if (allTriggers[i] == null)
                    continue;

                Collider triggerCollider = allTriggers[i].GetComponent<Collider>();
                float distSq = (allTriggers[i].transform.position - origin).sqrMagnitude;

                if (triggerCollider != null)
                {
                    Vector3 closestPoint = triggerCollider.ClosestPoint(origin);
                    distSq = (closestPoint - origin).sqrMagnitude;

                    if (distSq <= 0.01f)
                    {
                        nearest = allTriggers[i];
                        break;
                    }
                }

                if (distSq < nearestDistSq)
                {
                    nearestDistSq = distSq;
                    nearest = allTriggers[i];
                }
            }

            if (nearest != null)
            {
                BuildRuntimeLogger.Log($"WorldGameSessionManager.TriggerNearestAreaLoadForPlayer firing trigger={nearest.name} area={(nearest.GetArea() != null ? nearest.GetArea().name : "null")}");
                nearest.ManualTriggerForPlayer(player);
                BuildRuntimeLogger.Log($"WorldGameSessionManager.TriggerNearestAreaLoadForPlayer end trigger={nearest.name}");
                return nearest.GetArea();
            }

            BuildRuntimeLogger.Warning("WorldGameSessionManager.TriggerNearestAreaLoadForPlayer no registered trigger found");
            return null;
        }

        private WorldLocationSceneSet TriggerInitialAreaLoadForPlayer(PlayerManager player)
        {
            BuildRuntimeLogger.Log($"WorldGameSessionManager.TriggerInitialAreaLoadForPlayer begin player={(player != null ? player.name : "null")}");
            List<EventTriggerLoadScene> allTriggers = EventTriggerLoadScene.GetRegisteredTriggersSnapshot();
            BuildRuntimeLogger.Log($"WorldGameSessionManager.TriggerInitialAreaLoadForPlayer registeredTriggers={allTriggers.Count}");
            EventTriggerLoadScene firstTrigger = null;

            for (int i = 0; i < allTriggers.Count; i++)
            {
                EventTriggerLoadScene trigger = allTriggers[i];

                if (trigger == null)
                    continue;

                if (firstTrigger == null ||
                    string.Compare(trigger.name, firstTrigger.name, System.StringComparison.OrdinalIgnoreCase) < 0)
                {
                    firstTrigger = trigger;
                }
            }

            if (firstTrigger == null)
            {
                BuildRuntimeLogger.Warning("WorldGameSessionManager.TriggerInitialAreaLoadForPlayer no registered trigger found");
                return null;
            }

            BuildRuntimeLogger.Log($"WorldGameSessionManager.TriggerInitialAreaLoadForPlayer firing trigger={firstTrigger.name} area={(firstTrigger.GetArea() != null ? firstTrigger.GetArea().name : "null")}");
            firstTrigger.ManualTriggerForPlayer(player);
            BuildRuntimeLogger.Log($"WorldGameSessionManager.TriggerInitialAreaLoadForPlayer end trigger={firstTrigger.name}");
            return firstTrigger.GetArea();
        }

        private IEnumerator WaitForRequiredAreaScenes(WorldLocationSceneSet area, float timeout, float startProgress = 0.1f, float endProgress = 0.95f)
        {
            List<string> requiredScenes = area != null
                ? area.GetRequiredSceneIDsForWorldLocation()
                : new List<string>();

            BuildRuntimeLogger.Log($"WorldGameSessionManager.WaitForRequiredAreaScenes begin area={(area != null ? area.name : "null")} requiredScenes={requiredScenes.Count} timeout={timeout}");

            if (WorldSceneManager.instance != null && WorldSceneManager.instance.ShouldLoadGeneratedWorldAllAtOnce())
            {
                WorldSceneManager.instance.LoadAllGeneratedWorldAreaScenes();
                requiredScenes = WorldSceneManager.instance.GetGeneratedWorldAreaSceneNames();
            }
            else if (area == null)
            {
                BuildRuntimeLogger.Warning("WorldGameSessionManager.WaitForRequiredAreaScenes area null; fallback wait");
                SetLoadingProgress(endProgress, "Loading Map");
                yield return new WaitForSeconds(4f);
                yield break;
            }

            float elapsedTime = 0f;
            SetLoadingProgress(startProgress, "Loading Map");

            while (elapsedTime < timeout)
            {
                bool allScenesLoaded = true;
                int requiredSceneCount = 0;
                int loadedSceneCount = 0;

                for (int i = 0; i < requiredScenes.Count; i++)
                {
                    if (string.IsNullOrWhiteSpace(requiredScenes[i]))
                        continue;

                    requiredSceneCount++;
                    Scene scene = SceneManager.GetSceneByName(requiredScenes[i]);

                    if (scene.IsValid() && scene.isLoaded)
                    {
                        loadedSceneCount++;
                    }
                    else
                    {
                        allScenesLoaded = false;
                    }
                }

                float sceneProgress = requiredSceneCount <= 0 ? 1f : (float)loadedSceneCount / requiredSceneCount;
                SetLoadingProgress(Mathf.Lerp(startProgress, endProgress, sceneProgress), "Loading Map");
                BuildRuntimeLogger.MainThreadHeartbeat($"WaitForRequiredAreaScenes loaded={loadedSceneCount}/{requiredSceneCount} elapsed={elapsedTime:0.00}");

                if (allScenesLoaded)
                {
                    BuildRuntimeLogger.Log($"WorldGameSessionManager.WaitForRequiredAreaScenes all scenes loaded loaded={loadedSceneCount}/{requiredSceneCount} elapsed={elapsedTime:0.00}");
                    if (WorldSceneManager.instance != null)
                        WorldSceneManager.instance.CheckForRequiredRenderers();

                    SetLoadingProgress(endProgress, "Loading Map");
                    yield return null;
                    yield break;
                }

                elapsedTime += Time.deltaTime;
                yield return null;
            }

            string areaName = area != null ? area.name : "null";
            BuildRuntimeLogger.Warning($"WorldGameSessionManager: Timed out while waiting for required area scenes for '{areaName}'. Continuing so the player is not stuck on the loading screen.");

            if (WorldSceneManager.instance != null)
                WorldSceneManager.instance.CheckForRequiredRenderers();

            SetLoadingProgress(endProgress, "Loading Map");
            yield return null;
        }

        private void SetLoadingProgress(float progress, string label)
        {
            if (PlayerUIManager.instance == null || PlayerUIManager.instance.playerUILoadingScreenManager == null)
                return;

            PlayerUIManager.instance.playerUILoadingScreenManager.SetProgress(progress, label);
        }

        private IEnumerator ReturnToTitleAfterVictoryCoroutine(float delay)
        {
            while (delay > 0f)
            {
                delay -= Time.deltaTime;
                yield return null;
            }

            if (WorldSaveGameManager.instance != null &&
                WorldSaveGameManager.instance.currentCharacterData != null &&
                SceneManager.GetActiveScene().buildIndex != 0)
            {
                WorldSaveGameManager.instance.SaveGame();
            }

            if (NetworkManager.Singleton != null &&
                (NetworkManager.Singleton.IsHost || NetworkManager.Singleton.IsClient))
            {
                NetworkManager.Singleton.Shutdown();
                yield return null;
            }

            SceneManager.LoadScene(0);
            returnToTitleCoroutine = null;
        }

        private void SetLastRestedSiteOfGrace(int siteOfGraceID)
        {
            if (siteOfGraceID < 0)
                return;

            if (PlayerUIManager.instance != null && PlayerUIManager.instance.localPlayer != null)
                PlayerUIManager.instance.localPlayer.playerNetworkManager.lastSiteOfGraceUsed.Value = siteOfGraceID;

            if (WorldSaveGameManager.instance != null && WorldSaveGameManager.instance.currentCharacterData != null)
                WorldSaveGameManager.instance.currentCharacterData.lastSiteOfGraceRestedAt = siteOfGraceID;
        }

        public bool StartGameAsHost()
        {
            if (NetworkManager.Singleton.IsHost)
                return true;

            if (NetworkManager.Singleton.IsClient)
            {
                Debug.LogWarning("Client session is active. Shut it down before starting a host.");
                return false;
            }

            ConfigureUnityTransportPortForCurrentProject();

            if (!NetworkManager.Singleton.StartHost())
            {
                Debug.LogError("Failed to start host session.");
                return false;
            }

            Debug.Log($"Host started with UnityTransport on {GetCurrentConnectionAddress()}.");
            return true;
        }

        public void SetLaunchMode(SessionLaunchMode launchMode)
        {
            currentLaunchMode = launchMode == SessionLaunchMode.None
                ? SessionLaunchMode.Singleplayer
                : launchMode;
        }

        public SessionLaunchMode GetLaunchMode()
        {
            return currentLaunchMode;
        }

        public bool RequiresRelayForCurrentMode()
        {
            return currentLaunchMode == SessionLaunchMode.Multiplayer;
        }

        public bool AllowsDirectAddressForCurrentMode()
        {
            return !RequiresRelayForCurrentMode();
        }

        public async Task<bool> StartGameAsRelayHostAsync(int maxConnections = DefaultRelayMaxConnections)
        {
            if (NetworkManager.Singleton.IsHost)
            {
                if (!string.IsNullOrWhiteSpace(currentRelayJoinCode))
                    return true;

                Debug.LogWarning("A local host session is already active. Shut it down before starting a Relay host.");
                return false;
            }

            if (NetworkManager.Singleton.IsClient)
            {
                Debug.LogWarning("Client session is active. Shut it down before starting a Relay host.");
                return false;
            }

            if (unityTransport == null)
            {
                Debug.LogError("UnityTransport is missing from NetworkManager. Cannot start Relay host.");
                return false;
            }

            if (isStartingRelaySession)
                return false;

            isStartingRelaySession = true;

            try
            {
                await EnsureUnityServicesSignedInAsync();

                Allocation allocation = await RelayService.Instance.CreateAllocationAsync(maxConnections);
                currentRelayJoinCode = await RelayService.Instance.GetJoinCodeAsync(allocation.AllocationId);

                unityTransport.UseWebSockets = false;
                unityTransport.SetRelayServerData(new RelayServerData(allocation, RelayConnectionType));

                if (!NetworkManager.Singleton.StartHost())
                {
                    Debug.LogError("Failed to start Relay host session.");
                    currentRelayJoinCode = string.Empty;
                    return false;
                }

                Debug.Log($"Relay host started. Join code: {currentRelayJoinCode}");
                return true;
            }
            catch (System.Exception exception)
            {
                Debug.LogError($"Failed to start Relay host: {exception.Message}");
                currentRelayJoinCode = string.Empty;
                return false;
            }
            finally
            {
                isStartingRelaySession = false;
            }
        }

        private void ConfigureUnityTransportPortForCurrentProject()
        {
            if (unityTransport == null)
                return;

            ushort port = GetUnityTransportPortForCurrentProject();
            unityTransport.SetConnectionData("127.0.0.1", port, "0.0.0.0");
        }

        private ushort GetUnityTransportPortForCurrentProject()
        {
            DirectoryInfo projectDirectory = Directory.GetParent(Application.dataPath);

            if (projectDirectory == null)
                return DefaultUnityTransportPort;

            string projectFolderName = projectDirectory.Name;
            Match cloneSuffixMatch = Regex.Match(projectFolderName, @"_clone_(\d+)$", RegexOptions.IgnoreCase);

            if (!cloneSuffixMatch.Success)
                return DefaultUnityTransportPort;

            if (!int.TryParse(cloneSuffixMatch.Groups[1].Value, out int cloneIndex))
                return DefaultUnityTransportPort;

            int candidatePort = DefaultUnityTransportPort + cloneIndex + 1;

            if (candidatePort > ushort.MaxValue)
                return DefaultUnityTransportPort;

            Debug.Log($"ParrelSync clone detected. Using UnityTransport port {candidatePort} for project '{projectFolderName}'.");
            return (ushort)candidatePort;
        }

        public bool StartGameAsClient(string addressInput)
        {
            _ = StartGameAsClientAsync(addressInput);
            return true;
        }

        public async Task<bool> StartGameAsClientAsync(string addressInput)
        {
            if (joinAsClientCoroutine != null)
                StopCoroutine(joinAsClientCoroutine);

            if (TryNormalizeRelayJoinCode(addressInput, out string relayJoinCode))
            {
                return await StartGameAsRelayClientAsync(relayJoinCode);
            }

            if (RequiresRelayForCurrentMode())
            {
                Debug.LogError("Multiplayer mode requires a valid Relay join code.");
                return false;
            }

            if (!TryParseAddressInput(addressInput, out string hostAddress, out ushort port))
            {
                Debug.LogError($"Invalid address '{addressInput}'. Use a Relay join code or an IP/host name, optionally with ':port', for example '127.0.0.1:7777'.");
                return false;
            }

            joinAsClientCoroutine = StartCoroutine(JoinAsClientCoroutine(hostAddress, port));
            return true;
        }

        private IEnumerator JoinAsClientCoroutine(string hostAddress, ushort port)
        {
            if (SceneManager.GetActiveScene().buildIndex != 0)
            {
                WorldSaveGameManager.instance.SaveGame();
            }

            if (NetworkManager.Singleton.IsHost || NetworkManager.Singleton.IsClient)
            {
                NetworkManager.Singleton.Shutdown();
            }

            yield return null;

            if (unityTransport == null)
            {
                Debug.LogError("UnityTransport is missing from NetworkManager. Cannot join by address.");
                yield break;
            }

            unityTransport.SetConnectionData(hostAddress, port);

            if (!NetworkManager.Singleton.StartClient())
            {
                Debug.LogError($"Failed to start client for {hostAddress}:{port}.");
                yield break;
            }

            Debug.Log($"Client connecting to {hostAddress}:{port}.");
        }

        public async Task<bool> StartGameAsRelayClientAsync(string relayJoinCode)
        {
            if (!TryNormalizeRelayJoinCode(relayJoinCode, out relayJoinCode))
            {
                Debug.LogError($"Invalid Relay join code '{relayJoinCode}'.");
                return false;
            }

            if (unityTransport == null)
            {
                Debug.LogError("UnityTransport is missing from NetworkManager. Cannot join Relay session.");
                return false;
            }

            try
            {
                await EnsureUnityServicesSignedInAsync();

                JoinAllocation joinAllocation = GetCheckedRelayJoinAllocation(relayJoinCode);

                if (joinAllocation == null)
                    joinAllocation = await RelayService.Instance.JoinAllocationAsync(relayJoinCode);

                if (SceneManager.GetActiveScene().buildIndex != 0)
                {
                    WorldSaveGameManager.instance.SaveGame();
                }

                if (!await ShutdownNetworkSessionIfNeededAsync())
                    return false;

                currentRelayJoinCode = string.Empty;
                unityTransport.UseWebSockets = false;
                unityTransport.SetRelayServerData(new RelayServerData(joinAllocation, RelayConnectionType));

                if (!NetworkManager.Singleton.StartClient())
                {
                    Debug.LogError($"Failed to start Relay client for join code {relayJoinCode}.");
                    return false;
                }

                Debug.Log($"Relay client connecting with join code {relayJoinCode}.");
                ClearCheckedRelayJoinCode();
                return true;
            }
            catch (System.Exception exception)
            {
                Debug.LogError($"Failed to join Relay session '{relayJoinCode}': {exception.Message}");
                return false;
            }
        }

        public async Task<bool> CheckRelayJoinCodeAsync(string relayJoinCode)
        {
            if (!TryNormalizeRelayJoinCode(relayJoinCode, out relayJoinCode))
            {
                Debug.LogError($"Invalid Relay join code '{relayJoinCode}'.");
                ClearCheckedRelayJoinCode();
                return false;
            }

            try
            {
                await EnsureUnityServicesSignedInAsync();

                checkedRelayJoinAllocation = await RelayService.Instance.JoinAllocationAsync(relayJoinCode);
                checkedRelayJoinCode = relayJoinCode;

                Debug.Log($"Relay join code {relayJoinCode} is valid.");
                return true;
            }
            catch (System.Exception exception)
            {
                Debug.LogError($"Relay join code check failed for '{relayJoinCode}': {exception.Message}");
                ClearCheckedRelayJoinCode();
                return false;
            }
        }

        private async Task<bool> ShutdownNetworkSessionIfNeededAsync()
        {
            if (NetworkManager.Singleton == null)
                return true;

            NetworkManager networkManager = NetworkManager.Singleton;

            if (networkManager.IsHost || networkManager.IsClient || networkManager.IsServer || networkManager.IsListening)
            {
                networkManager.Shutdown();
            }

            float timeoutTime = Time.realtimeSinceStartup + 5f;

            while (networkManager != null &&
                   (networkManager.ShutdownInProgress || networkManager.IsListening) &&
                   Time.realtimeSinceStartup < timeoutTime)
            {
                await Task.Yield();
            }

            if (networkManager != null && (networkManager.ShutdownInProgress || networkManager.IsListening))
            {
                Debug.LogError("Timed out while shutting down the current network session before joining Relay.");
                return false;
            }

            return true;
        }

        public string GetSuggestedHostAddress()
        {
            if (!string.IsNullOrWhiteSpace(currentRelayJoinCode))
                return currentRelayJoinCode;

            return $"127.0.0.1:{GetUnityTransportPortForCurrentProject()}";
        }

        public string GetCurrentConnectionAddress()
        {
            if (!string.IsNullOrWhiteSpace(currentRelayJoinCode))
                return currentRelayJoinCode;

            ushort port = GetUnityTransportPortForCurrentProject();
            return $"127.0.0.1:{port}";
        }

        public bool HasRelayJoinCode()
        {
            return !string.IsNullOrWhiteSpace(currentRelayJoinCode);
        }

        public bool IsRelayJoinCodeChecked(string relayJoinCode)
        {
            return TryNormalizeRelayJoinCode(relayJoinCode, out relayJoinCode) &&
                   checkedRelayJoinAllocation != null &&
                   checkedRelayJoinCode == relayJoinCode;
        }

        private JoinAllocation GetCheckedRelayJoinAllocation(string relayJoinCode)
        {
            if (!IsRelayJoinCodeChecked(relayJoinCode))
                return null;

            return checkedRelayJoinAllocation;
        }

        private void ClearCheckedRelayJoinCode()
        {
            checkedRelayJoinCode = string.Empty;
            checkedRelayJoinAllocation = null;
        }

        private async Task EnsureUnityServicesSignedInAsync()
        {
            if (UnityServices.State != ServicesInitializationState.Initialized)
            {
                await UnityServices.InitializeAsync();
            }

            if (!AuthenticationService.Instance.IsSignedIn)
            {
                await AuthenticationService.Instance.SignInAnonymouslyAsync();
            }
        }

        private bool TryNormalizeRelayJoinCode(string addressInput, out string relayJoinCode)
        {
            relayJoinCode = string.Empty;

            if (string.IsNullOrWhiteSpace(addressInput))
                return false;

            string trimmedInput = addressInput
                .Replace("\u200B", string.Empty)
                .Replace("\uFEFF", string.Empty)
                .Trim();

            if (!Regex.IsMatch(trimmedInput, @"^[A-Za-z0-9]{6}$"))
                return false;

            relayJoinCode = trimmedInput.ToUpperInvariant();
            return true;
        }

        private bool TryParseAddressInput(string addressInput, out string hostAddress, out ushort port)
        {
            hostAddress = "127.0.0.1";
            port = DefaultUnityTransportPort;

            if (string.IsNullOrWhiteSpace(addressInput))
                return true;

            string trimmedInput = addressInput
                .Replace("\u200B", string.Empty)
                .Replace("\uFEFF", string.Empty)
                .Trim();

            if (string.IsNullOrWhiteSpace(trimmedInput) || trimmedInput == "..." || trimmedInput.StartsWith("...:"))
                return true;

            string[] parts = trimmedInput.Split(':');

            if (parts.Length == 1)
            {
                hostAddress = parts[0];
                return !string.IsNullOrWhiteSpace(hostAddress);
            }

            if (parts.Length == 2)
            {
                hostAddress = parts[0];

                if (string.IsNullOrWhiteSpace(hostAddress))
                    return false;

                return ushort.TryParse(parts[1], out port);
            }

            return false;
        }

        public void WaitThenRevivePlayer(PlayerManager player)
        {
            if (player == null || !player.IsOwner)
                return;

            if (!CanRevivePlayers())
                return;

            if (revivalCoroutine != null)
                StopCoroutine(revivalCoroutine);

            revivalCoroutine = StartCoroutine(RevivePlayerCoroutine(player, 5));
        }

        private IEnumerator RevivePlayerCoroutine(PlayerManager player, float delay)
        {
            yield return new WaitForSeconds(delay);

            if (player == null || !player.IsOwner || !CanRevivePlayers())
                yield break;

            PlayerUIManager.instance.playerUILoadingScreenManager.ActivateLoadingScreen();

            player.ReviveCharacter();

            if (NetworkManager.Singleton.IsServer && players.Count <= 1)
            {
                WorldAIManager.instance.ResetAllCharacters();
            }

            int targetSiteOfGraceId = player.playerNetworkManager.lastSiteOfGraceUsed.Value;

            if (targetSiteOfGraceId < 0)
            {
                targetSiteOfGraceId = WorldSaveGameManager.instance.currentCharacterData.lastSiteOfGraceRestedAt;
            }

            SiteOfGraceInteractable targetSiteOfGrace = null;

            for (int i = 0; i < WorldObjectManager.instance.sitesOfGrace.Count; i++)
            {
                if (WorldObjectManager.instance.sitesOfGrace[i].siteOfGraceID == targetSiteOfGraceId)
                {
                    targetSiteOfGrace = WorldObjectManager.instance.sitesOfGrace[i];
                    break;
                }
            }

            if (targetSiteOfGrace == null && WorldObjectManager.instance.sitesOfGrace.Count > 0)
            {
                targetSiteOfGrace = WorldObjectManager.instance.sitesOfGrace[0];
                player.playerNetworkManager.lastSiteOfGraceUsed.Value = targetSiteOfGrace.siteOfGraceID;
            }

            if (targetSiteOfGrace != null)
            {
                targetSiteOfGrace.TeleportPlayerToSiteOfGrace(player);
            }
            else
            {
                PlayerUIManager.instance.playerUILoadingScreenManager.DeactivateLoadingScreen(0.5f);
            }
        }

        private void ResetTransientSessionStateForCurrentMap()
        {
            trackedDeathMapIndex = GameProgressionManager.Instance.CurrentMapIndex;
            playerDeathsThisMap.Clear();
            sessionLoseTriggered = false;
            sessionWinTriggered = false;
            CancelPendingRevival();
            ClearPendingVictoryTransition();

            if (WorldSaveGameManager.instance != null)
                WorldSaveGameManager.instance.SetCurrentCharacterPlayTimeFrozen(false);
        }

        private void ClearPendingVictoryTransition()
        {
            if (autoContinueVictoryCoroutine != null)
            {
                StopCoroutine(autoContinueVictoryCoroutine);
                autoContinueVictoryCoroutine = null;
            }

            pendingVictoryShouldLoadNextScene = false;
            pendingVictoryNextSceneBuildIndex = -1;
            pendingVictoryUnlockedMapIndex = -1;
        }

        private void SyncDeathTrackingMap(int mapIndex)
        {
            int resolvedMapIndex = mapIndex >= 0
                ? mapIndex
                : GameProgressionManager.Instance.CurrentMapIndex;

            if (trackedDeathMapIndex == resolvedMapIndex)
                return;

            trackedDeathMapIndex = resolvedMapIndex;
            playerDeathsThisMap.Clear();
            sessionLoseTriggered = false;
            sessionWinTriggered = false;
            ClearPendingVictoryTransition();
        }

        private void CancelPendingRevival()
        {
            if (revivalCoroutine != null)
            {
                StopCoroutine(revivalCoroutine);
                revivalCoroutine = null;
            }
        }

        public void AddPlayerToActivePlayersList(PlayerManager player)
        {
            if (!players.Contains(player))
            {
                players.Add(player);
            }

            for (int i = players.Count - 1; i > -1; i--)
            {
                if (players[i] == null)
                {
                    players.RemoveAt(i);
                }
            }
        }

        public void RemovePlayerFromActivePlayersList(PlayerManager player)
        {
            if (players.Contains(player))
            {
                players.Remove(player);
            }

            for (int i = players.Count - 1; i > -1; i--)
            {
                if (players[i] == null)
                {
                    players.RemoveAt(i);
                }
            }
        }

        public int GetActivePlayerCount()
        {
            for (int i = players.Count - 1; i > -1; i--)
            {
                if (players[i] == null)
                    players.RemoveAt(i);
            }

            return players.Count;
        }

        public bool IsMultiplayerSessionActive()
        {
            return GetActivePlayerCount() > 1;
        }

        public PlayerManager GetPlayerByClientId(ulong clientId)
        {
            if (NetworkManager.Singleton != null &&
                NetworkManager.Singleton.ConnectedClients != null &&
                NetworkManager.Singleton.ConnectedClients.TryGetValue(clientId, out NetworkClient connectedClient) &&
                connectedClient.PlayerObject != null)
            {
                return connectedClient.PlayerObject.GetComponent<PlayerManager>();
            }

            for (int i = 0; i < players.Count; i++)
            {
                if (players[i] != null && players[i].OwnerClientId == clientId)
                    return players[i];
            }

            return null;
        }
    }
}
