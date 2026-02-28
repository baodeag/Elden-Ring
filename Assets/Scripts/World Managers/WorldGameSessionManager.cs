using NUnit.Framework;
using UnityEngine;
using Unity.Netcode;
using System.Collections.Generic;
using System.Collections;

namespace baodeag
{
    public class WorldGameSessionManager : MonoBehaviour
    {
        public static WorldGameSessionManager instance;

        [Header("Active Players In Session")]
        public List<PlayerManager> players = new List<PlayerManager>();

        private Coroutine revivalCoroutine;

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
