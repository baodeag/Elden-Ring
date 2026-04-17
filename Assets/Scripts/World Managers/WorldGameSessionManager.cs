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
    public class WorldGameSessionManager : MonoBehaviour
    {
        public static WorldGameSessionManager instance;

        [Header("Active Players In Session")]
        public List<PlayerManager> players = new List<PlayerManager>();

        private Coroutine revivalCoroutine;
        private Coroutine pendingMapEntryCoroutine;
        private Coroutine returnToTitleCoroutine;
        private Coroutine mapTransitionCoroutine;

        private UnityTransport unityTransport;
        private const ushort DefaultUnityTransportPort = 7777;
        private const int DefaultRelayMaxConnections = 4;
        private const string RelayConnectionType = "dtls";
        private Coroutine joinAsClientCoroutine;
        private string currentRelayJoinCode = string.Empty;
        private string checkedRelayJoinCode = string.Empty;
        private JoinAllocation checkedRelayJoinAllocation;
        private bool isStartingRelaySession;

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

        public void ScheduleMapTransition(bool shouldLoadNextScene, int nextSceneBuildIndex, bool gameWon, int unlockedMapIndex)
        {
            if (mapTransitionCoroutine != null)
                StopCoroutine(mapTransitionCoroutine);

            mapTransitionCoroutine = StartCoroutine(ScheduleMapTransitionCoroutine(shouldLoadNextScene, nextSceneBuildIndex, gameWon, unlockedMapIndex));
        }

        private IEnumerator ScheduleMapTransitionCoroutine(bool shouldLoadNextScene, int nextSceneBuildIndex, bool gameWon, int unlockedMapIndex)
        {
            yield return new WaitForSeconds(5f);

            if (gameWon)
            {
                ReturnToTitleAfterVictory();
                mapTransitionCoroutine = null;
                yield break;
            }

            if (shouldLoadNextScene && nextSceneBuildIndex >= 0 && nextSceneBuildIndex != SceneManager.GetActiveScene().buildIndex)
            {
                LoadSceneForProgression(nextSceneBuildIndex);
            }
            else if (unlockedMapIndex >= 0)
            {
                ProcessPendingMapEntryWithoutSceneReload();
            }

            mapTransitionCoroutine = null;
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
            int targetSiteOfGraceID = GameProgressionManager.Instance.ConsumePendingTransitionSiteOfGraceID();

            if (targetSiteOfGraceID < 0)
            {
                if (PlayerUIManager.instance != null)
                    PlayerUIManager.instance.playerUILoadingScreenManager.DeactivateLoadingScreen(1.5f);

                pendingMapEntryCoroutine = null;
                yield break;
            }

            float timeout = 10f;
            float elapsedTime = 0f;

            while ((PlayerUIManager.instance == null ||
                    PlayerUIManager.instance.localPlayer == null ||
                    WorldObjectManager.instance == null ||
                    WorldObjectManager.instance.sitesOfGrace == null ||
                    WorldObjectManager.instance.sitesOfGrace.Count == 0) &&
                   elapsedTime < timeout)
            {
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            if (PlayerUIManager.instance == null || PlayerUIManager.instance.localPlayer == null)
            {
                pendingMapEntryCoroutine = null;
                yield break;
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
                PlayerManager localPlayer = PlayerUIManager.instance.localPlayer;
                targetSiteOfGrace.TeleportPlayerToSiteOfGrace(localPlayer, false);
                localPlayer.playerNetworkManager.lastSiteOfGraceUsed.Value = targetSiteOfGrace.siteOfGraceID;
                WorldSaveGameManager.instance.currentCharacterData.lastSiteOfGraceRestedAt = targetSiteOfGrace.siteOfGraceID;
                WorldSaveGameManager.instance.SaveGame();
            }

            // Give the new world scene and additive areas extra time to finish creating
            // objects before revealing gameplay.
            yield return new WaitForSeconds(4f);

            if (PlayerUIManager.instance != null)
                PlayerUIManager.instance.playerUILoadingScreenManager.DeactivateLoadingScreen(2.5f);

            pendingMapEntryCoroutine = null;
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

            if (revivalCoroutine != null)
                StopCoroutine(revivalCoroutine);

            revivalCoroutine = StartCoroutine(RevivePlayerCoroutine(player, 5));
        }

        private IEnumerator RevivePlayerCoroutine(PlayerManager player, float delay)
        {
            yield return new WaitForSeconds(delay);

            if (player == null || !player.IsOwner)
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
    }
}
