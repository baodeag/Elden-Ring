using UnityEngine;
using UnityEngine.Rendering;
using System.Collections.Generic;
using System.Collections;

namespace baodeag
{
    public class WorldSubsceneManager : MonoBehaviour
    {
        public static WorldSubsceneManager instance;

        private Dictionary<WorldLocationSceneSet, List<PlayerManager>> playersInLocation = new Dictionary<WorldLocationSceneSet, List<PlayerManager>>();

        [Header("Probe Volumn Set")]
        [SerializeField] ProbeVolumeBakingSet bakeSet;

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

        public List<string> GenerateDoNotUnloadListBasedOnPlayerLocations()
        {
            List<string> doNotUnloadLocations = new List<string>();

            doNotUnloadLocations.Add(WorldSceneManager.instance.world);

            List<WorldLocationSceneSet> areasWithPlayersActive = new List<WorldLocationSceneSet>();

            foreach (KeyValuePair<WorldLocationSceneSet, List<PlayerManager>> pair in playersInLocation)
            {
                for (int i = 0; i < pair.Value.Count; i++)
                {
                    if (pair.Value[i] == null)
                        pair.Value.RemoveAt(i);
                }

                if (pair.Value.Count > 0 && !areasWithPlayersActive.Contains(pair.Key))
                    areasWithPlayersActive.Add(pair.Key);
            }

            for (int i = 0; i < areasWithPlayersActive.Count; i++)
            {
                List<string> scenesRequired = areasWithPlayersActive[i].GetRequiredSceneIDsForWorldLocation();

                for (int j = 0; j < scenesRequired.Count; j++)
                {
                    doNotUnloadLocations.Add(scenesRequired[j]);
                }
            }

            return doNotUnloadLocations;
        }

        //this is called whenever a player enters a new additive scene
        public void LoadAreasBasedOnAreaCurrentIn(WorldLocationSceneSet areaCurrentlyIn, PlayerManager player)
        {
            if (IsPlayerAlreadyInArea(areaCurrentlyIn, player))
                return;

            RemovePlayerFromPreviousLocation(player);

            AddPlayerToNewLocation(areaCurrentlyIn, player);

            LoadAdditiveScenesAroundCurrentArea(areaCurrentlyIn);

            WorldSceneManager.instance.CheckForUnrequiredScenes();
        }

        private bool IsPlayerAlreadyInArea(WorldLocationSceneSet area, PlayerManager player)
        {
            bool playerInArea = false;

            if (playersInLocation.ContainsKey(area) && playersInLocation[area].Contains(player))
                playerInArea = true;

            return playerInArea;
        }

        private void RemovePlayerFromPreviousLocation(PlayerManager player)
        {
            if (player == null)
                return;

            foreach (KeyValuePair<WorldLocationSceneSet, List<PlayerManager>> pair in playersInLocation)
            {
                if (pair.Value.Contains(player))
                    pair.Value.Remove(player);

                for (int i = 0; i < pair.Value.Count; i++)
                {
                    if (pair.Value[i] == null)
                        pair.Value.RemoveAt(i);
                }
            }
        }

        private void AddPlayerToNewLocation(WorldLocationSceneSet area, PlayerManager player)
        {
            if (player == null)
                return;

            //set the baking set
            if (player.IsOwner)
                StartCoroutine(WaitThenSetActiveScene());

            if (!playersInLocation.ContainsKey(area))
                playersInLocation[area] = new List<PlayerManager>();

            if (!playersInLocation[area].Contains(player))
                playersInLocation[area].Add(player);

            player.areaCurrentlyIn = area;

            foreach (KeyValuePair<WorldLocationSceneSet, List<PlayerManager>> pair in playersInLocation)
            {
                for (int i = 0; i < pair.Value.Count; i++)
                {
                    if (pair.Value[i] == null)
                        pair.Value.RemoveAt(i);
                }
            }
        }

        private void LoadAdditiveScenesAroundCurrentArea(WorldLocationSceneSet area)
        {
            List<string> scenesToLoad = new List<string>();

            List<WorldLocationSceneSet> worldLocations = new List<WorldLocationSceneSet>();

            scenesToLoad = area.GetRequiredSceneIDsForWorldLocation();

            if (scenesToLoad.Count <= 0)
                return;

            WorldSceneManager.instance.LoadAdditiveScenes(scenesToLoad);
        }

        private IEnumerator WaitThenSetActiveScene()
        {
            bool hasScene = false;

            while (!hasScene)
            {
                //wait for the scene
                for (int i = 0; i < WorldSceneManager.instance.loadedScenes.Count; i++)
                {
                    if (WorldSceneManager.instance.loadedScenes[i].name == WorldSceneManager.instance.world)
                    {
                        hasScene = true;
                        ProbeReferenceVolume.instance.SetActiveScene(WorldSceneManager.instance.loadedScenes[i]);
                        ProbeReferenceVolume.instance.SetActiveBakingSet(bakeSet);
                    }

                    yield return null;
                }
            }
            yield return null;
        }
    }
}
