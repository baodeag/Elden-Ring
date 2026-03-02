using NUnit.Framework;
using UnityEngine;
using Unity.Netcode;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using System.Collections;
using Netcode.Transports.Facepunch;
using Steamworks;
using Steamworks.Data;

namespace baodeag
{
    public class WorldGameSessionManager : MonoBehaviour
    {
        public static WorldGameSessionManager instance;

        [Header("Active Players In Session")]
        public List<PlayerManager> players = new List<PlayerManager>();

        private Coroutine revivalCoroutine;

        [Header("Active Lobby")]
        public Lobby? currentLobby;
        private FacepunchTransport transport;
        private Coroutine joinningAsClientCoroutine;

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

            transport = GetComponent<FacepunchTransport>();
        }

        private void Start()
        {
            DontDestroyOnLoad(gameObject);

            SteamMatchmaking.OnLobbyEntered += OnLobbyEntered;
            SteamMatchmaking.OnLobbyCreated += OnLobbyCreated;
            SteamFriends.OnGameLobbyJoinRequested += OnGameLobbyJoinRequested;
        }

        private void OnDestroy()
        {
            SteamMatchmaking.OnLobbyEntered -= OnLobbyEntered;
            SteamMatchmaking.OnLobbyCreated -= OnLobbyCreated;
            SteamFriends.OnGameLobbyJoinRequested -= OnGameLobbyJoinRequested;
        }

        private void OnApplicationQuit()
        {
            DisconnectFromLobby();
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
            //if we arent on the menu scene, allow others to join our lobby
            if (SceneManager.GetActiveScene().buildIndex != 0)
            {
                ToggleLobbyIsJoinable(true);
            }
            else
            {
                ToggleLobbyIsJoinable(false);
            }
        }

        //face punch
        public void ToggleLobbyIsJoinable(bool status)
        {
            currentLobby?.SetJoinable(status);
        }

        //called when a lobby is created
        private void OnLobbyCreated(Result result, Lobby lobby)
        {
            if (result != Result.OK)
            {
                Debug.LogError($"Lobby could no be created, {result}", this);
                return;
            }

            lobby.SetPublic();
            lobby.SetJoinable(false); //we only want to set to joinable once we are in the world
            lobby.SetGameServer(lobby.Owner.Id);
        }

        //called when entering a lobby
        private void OnGameLobbyJoinRequested(Lobby joinedLobby, SteamId steamID)
        {
            //if we are on the main menu when trying to join, do not allow the player to join until they load into the world
            if (SceneManager.GetActiveScene().buildIndex == 0)
            {
                if (!NetworkManager.Singleton.IsHost && !NetworkManager.Singleton.IsClient)
                {
                    Debug.Log("We are not allowed to join another game, we aren't a client or a host. Start the game first");
                    return;
                }

                //optionally send a pop up letting the player know through a ui element
            }

            //save before joining
            WorldSaveGameManager.instance.SaveGame();
            NetworkManager.Singleton.Shutdown();

            Debug.Log($"Attempting to join game, {joinedLobby.Id}, from {steamID}");
            currentLobby = joinedLobby;

            //if we have a current lobby, join it
            currentLobby?.Join();
        }

        private void OnLobbyEntered(Lobby lobby)
        {
            if (NetworkManager.Singleton.IsHost)
            {
                return;
            }
            else
            {
                StartGameAsClient(lobby.Owner.Id);
            }
        }

        public async void StartGameAsHost()
        {
            NetworkManager.Singleton.StartHost();

            //currentLobby = await SteamMatchmaking.CreateLobbyAsync(4);
        }

        public void StartGameAsClient(SteamId id)
        {
            if (PlayerUIManager.instance.localPlayer.isDead.Value)
            {
                return;
            }

            if (joinningAsClientCoroutine != null)
                StopCoroutine(joinningAsClientCoroutine);

            joinningAsClientCoroutine = StartCoroutine(AttemptToJoinAsClient(id));
        }

        private IEnumerator AttemptToJoinAsClient(SteamId id)
        {
            //optionally activate loading screen until joined

            while (transport.targetSteamId != id)
            {
                transport.targetSteamId = id;
                yield return null;
            }

            yield return null;

            NetworkManager.Singleton.StartClient();
        }

        public void DisconnectFromLobby()
        {
            currentLobby?.Leave();
        }

        public void WaitThenReviveHost()
        {
            if (revivalCoroutine != null)
                StopCoroutine(revivalCoroutine);

            revivalCoroutine = StartCoroutine(ReviveHostCoroutine(5));
        }

        private IEnumerator ReviveHostCoroutine(float delay)
        {
            yield return new WaitForSeconds(delay);

            PlayerUIManager.instance.playerUILoadingScreenManager.ActivateLoadingScreen();

            PlayerUIManager.instance.localPlayer.ReviveCharacter();

            for (int i = 0; i < WorldObjectManager.instance.sitesOfGrace.Count; i++)
            {
                if (WorldObjectManager.instance.sitesOfGrace[i].siteOfGraceID == WorldSaveGameManager.instance.currentCharacterData.lastSiteOfGraceRestedAt)
                {
                    WorldObjectManager.instance.sitesOfGrace[i].TeleportToSiteOfGrace();
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
