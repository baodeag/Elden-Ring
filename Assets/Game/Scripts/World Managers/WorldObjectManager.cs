using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

namespace baodeag
{
    public class WorldObjectManager : MonoBehaviour
    {
        public static WorldObjectManager instance;

        [Header("Network Objects")]
        [SerializeField] List<NetworkObjectSpawner> networkObjectSpawners;
        [SerializeField] List<GameObject> spawnedInObjects;

        [Header("Fog Wall")]
        public List<FogWallInteractable> fogWalls;

        [Header("Site Of Grace")]
        public List<SiteOfGraceInteractable> sitesOfGrace;

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

        public void SpawnObject(NetworkObjectSpawner networkObjectSpawner)
        {
            if (NetworkManager.Singleton.IsServer)
            {
                networkObjectSpawners.Add(networkObjectSpawner);
                networkObjectSpawner.AttemptToSpawnCharacter();
            }
        }

        public void AddFogWallToList(FogWallInteractable fogWall)
        {
            if (!fogWalls.Contains(fogWall))
            {
                fogWalls.Add(fogWall);
            }
        }

        public void RemoveFogWallFromList(FogWallInteractable fogWall)
        {
            if (fogWalls.Contains(fogWall))
            {
                fogWalls.Remove(fogWall);
            }
        }

        public void AddSiteOfGraceToList(SiteOfGraceInteractable siteOfGrace)
        {
            if (!sitesOfGrace.Contains(siteOfGrace))
            {
                sitesOfGrace.Add(siteOfGrace);
            }
        }

        public void RemoveSiteOfGraceFromList(SiteOfGraceInteractable siteOfGrace)
        {
            if (sitesOfGrace.Contains(siteOfGrace))
            {
                sitesOfGrace.Remove(siteOfGrace);
            }
        }
    }
}
