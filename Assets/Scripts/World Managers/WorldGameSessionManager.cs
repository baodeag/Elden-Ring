using UnityEngine;
using Unity.Netcode;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using System.Collections;
using Unity.Netcode.Transports.UTP;
using System.IO;
using System.Text.RegularExpressions;

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
        private Coroutine joinAsClientCoroutine;

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

        public void ScheduleMapTransition(bool shouldLoadNextScene, int nextSceneBuildIndex, bool gameWon, int unlockedMapIndex, string queuedMapUnlockedMessage)
        {
            if (mapTransitionCoroutine != null)
                StopCoroutine(mapTransitionCoroutine);

            mapTransitionCoroutine = StartCoroutine(ScheduleMapTransitionCoroutine(shouldLoadNextScene, nextSceneBuildIndex, gameWon, unlockedMapIndex, queuedMapUnlockedMessage));
        }

        private IEnumerator ScheduleMapTransitionCoroutine(bool shouldLoadNextScene, int nextSceneBuildIndex, bool gameWon, int unlockedMapIndex, string queuedMapUnlockedMessage)
        {
            yield return new WaitForSeconds(5f);

            if (gameWon)
            {
                ReturnToTitleAfterVictory();
                mapTransitionCoroutine = null;
                yield break;
            }

            if (!string.IsNullOrEmpty(queuedMapUnlockedMessage))
                yield return new WaitForSeconds(3f);

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
            if (joinAsClientCoroutine != null)
                StopCoroutine(joinAsClientCoroutine);

            if (!TryParseAddressInput(addressInput, out string hostAddress, out ushort port))
            {
                Debug.LogError($"Invalid address '{addressInput}'. Use an IP or host name, optionally with ':port', for example '127.0.0.1:7777'.");
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

        public string GetSuggestedHostAddress()
        {
            return $"127.0.0.1:{GetUnityTransportPortForCurrentProject()}";
        }

        public string GetCurrentConnectionAddress()
        {
            ushort port = GetUnityTransportPortForCurrentProject();
            return $"127.0.0.1:{port}";
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
