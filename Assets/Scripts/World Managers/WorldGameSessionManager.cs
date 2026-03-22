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
            // Keep hook in place in case later world transitions need session-dependent logic.
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

            for (int i = 0; i < WorldObjectManager.instance.sitesOfGrace.Count; i++)
            {
                if (WorldObjectManager.instance.sitesOfGrace[i].siteOfGraceID == player.playerNetworkManager.lastSiteOfGraceUsed.Value)
                {
                    WorldObjectManager.instance.sitesOfGrace[i].TeleportPlayerToSiteOfGrace(player);
                    break;
                }
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
    }
}
