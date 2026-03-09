using UnityEngine;
using System.Collections.Generic;

namespace baodeag
{
    public class WorldSubsceneManager : MonoBehaviour
    {
        public static WorldSubsceneManager instance;

        [SerializeField] private List<PlayerManager> playersIn_Area01_Subarea00 = new List<PlayerManager>();
        [SerializeField] private List<PlayerManager> playersIn_Area01_Subarea01 = new List<PlayerManager>();
        [SerializeField] private List<PlayerManager> playersIn_Area01_Subarea02 = new List<PlayerManager>();
        [SerializeField] private List<PlayerManager> playersIn_Area01_Subarea03 = new List<PlayerManager>();
        [SerializeField] private List<PlayerManager> playersIn_Area01_Subarea04 = new List<PlayerManager>();
        [SerializeField] private List<PlayerManager> playersIn_Area01_Subarea05 = new List<PlayerManager>();


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

            //the world scene is never unloaded
            doNotUnloadLocations.Add(WorldSceneManager.instance.world);
            int playersInScene;

            //sub area 00
            //set players in scene count to 0
            playersInScene = 0;

            //check for ant players in this specific scene
            for (int i = 0; i < playersIn_Area01_Subarea00.Count; i++)
            {
                if (playersIn_Area01_Subarea00[i] != null)
                    playersInScene++;
            }

            //if the players in this scene are greater than 0, keep scenes loaded that are required for this scene
            if (playersInScene > 0)
            {
                doNotUnloadLocations.Add(WorldSceneManager.instance.area_01_Subarea_00);
                doNotUnloadLocations.Add(WorldSceneManager.instance.area_01_Subarea_01);
                doNotUnloadLocations.Add(WorldSceneManager.instance.area_01_Subarea_04);
            }

            //sub area 01
            //set players in scene count to 0
            playersInScene = 0;

            //check for ant players in this specific scene
            for (int i = 0; i < playersIn_Area01_Subarea01.Count; i++)
            {
                if (playersIn_Area01_Subarea01[i] != null)
                    playersInScene++;
            }

            //if the players in this scene are greater than 0, keep scenes loaded that are required for this scene
            if (playersInScene > 0)
            {
                doNotUnloadLocations.Add(WorldSceneManager.instance.area_01_Subarea_01);
                doNotUnloadLocations.Add(WorldSceneManager.instance.area_01_Subarea_00);
                doNotUnloadLocations.Add(WorldSceneManager.instance.area_01_Subarea_02);
                doNotUnloadLocations.Add(WorldSceneManager.instance.area_01_Subarea_04);
            }

            //sub area 02
            //set players in scene count to 0
            playersInScene = 0;

            //check for ant players in this specific scene
            for (int i = 0; i < playersIn_Area01_Subarea02.Count; i++)
            {
                if (playersIn_Area01_Subarea02[i] != null)
                    playersInScene++;
            }

            //if the players in this scene are greater than 0, keep scenes loaded that are required for this scene
            if (playersInScene > 0)
            {
                doNotUnloadLocations.Add(WorldSceneManager.instance.area_01_Subarea_02);
                doNotUnloadLocations.Add(WorldSceneManager.instance.area_01_Subarea_03);
                doNotUnloadLocations.Add(WorldSceneManager.instance.area_01_Subarea_01);
            }

            //sub area 03
            //set players in scene count to 0
            playersInScene = 0;

            //check for ant players in this specific scene
            for (int i = 0; i < playersIn_Area01_Subarea03.Count; i++)
            {
                if (playersIn_Area01_Subarea03[i] != null)
                    playersInScene++;
            }

            //if the players in this scene are greater than 0, keep scenes loaded that are required for this scene
            if (playersInScene > 0)
            {
                doNotUnloadLocations.Add(WorldSceneManager.instance.area_01_Subarea_03);
                doNotUnloadLocations.Add(WorldSceneManager.instance.area_01_Subarea_02);
                doNotUnloadLocations.Add(WorldSceneManager.instance.area_01_Subarea_04);
            }

            //sub area 04
            //set players in scene count to 0
            playersInScene = 0;

            //check for ant players in this specific scene
            for (int i = 0; i < playersIn_Area01_Subarea04.Count; i++)
            {
                if (playersIn_Area01_Subarea04[i] != null)
                    playersInScene++;
            }

            //if the players in this scene are greater than 0, keep scenes loaded that are required for this scene
            if (playersInScene > 0)
            {
                doNotUnloadLocations.Add(WorldSceneManager.instance.area_01_Subarea_04);
                doNotUnloadLocations.Add(WorldSceneManager.instance.area_01_Subarea_00);
                doNotUnloadLocations.Add(WorldSceneManager.instance.area_01_Subarea_01);
                doNotUnloadLocations.Add(WorldSceneManager.instance.area_01_Subarea_03);
                doNotUnloadLocations.Add(WorldSceneManager.instance.area_01_Subarea_05);
            }

            //sub area 05
            //set players in scene count to 0
            playersInScene = 0;

            //check for ant players in this specific scene
            for (int i = 0; i < playersIn_Area01_Subarea05.Count; i++)
            {
                if (playersIn_Area01_Subarea05[i] != null)
                    playersInScene++;
            }

            //if the players in this scene are greater than 0, keep scenes loaded that are required for this scene
            if (playersInScene > 0)
            {
                doNotUnloadLocations.Add(WorldSceneManager.instance.area_01_Subarea_05);
                doNotUnloadLocations.Add(WorldSceneManager.instance.area_01_Subarea_00);
                doNotUnloadLocations.Add(WorldSceneManager.instance.area_01_Subarea_01);
                doNotUnloadLocations.Add(WorldSceneManager.instance.area_01_Subarea_04);
            }

            return doNotUnloadLocations;
        }

        //this is called whenever a player enters a new additive scene
        public void LoadAreasBasedOnAreaCurrentIn(WorldSceneLocation areaCurrentlyIn, PlayerManager player)
        {
            if (IsPlayerAlreadyInArea(areaCurrentlyIn, player))
                return;

            RemovePlayerFromPreviousLocation(player);

            AddPlayerToNewLocation(areaCurrentlyIn, player);

            LoadAdditiveScenesAroundCurrentArea(areaCurrentlyIn);

            WorldSceneManager.instance.CheckForUnrequiredScenes();
        }

        private bool IsPlayerAlreadyInArea(WorldSceneLocation area, PlayerManager player)
        {
            bool isPlayerInArea = false;

            switch (area)
            {
                case WorldSceneLocation.Area01_Subarea00:
                    if (playersIn_Area01_Subarea00.Contains(player))
                        isPlayerInArea = true;
                    break;

                case WorldSceneLocation.Area01_Subarea01:
                    if (playersIn_Area01_Subarea01.Contains(player))
                        isPlayerInArea = true;
                    break;

                case WorldSceneLocation.Area01_Subarea02:
                    if (playersIn_Area01_Subarea02.Contains(player))
                        isPlayerInArea = true;
                    break;

                case WorldSceneLocation.Area01_Subarea03:
                    if (playersIn_Area01_Subarea03.Contains(player))
                        isPlayerInArea = true;
                    break;

                case WorldSceneLocation.Area01_Subarea04:
                    if (playersIn_Area01_Subarea04.Contains(player))
                        isPlayerInArea = true;
                    break;

                case WorldSceneLocation.Area01_Subarea05:
                    if (playersIn_Area01_Subarea05.Contains(player))
                        isPlayerInArea = true;
                    break;

                default:
                    break;
            }

            return isPlayerInArea;
        }

        private void RemovePlayerFromPreviousLocation(PlayerManager player)
        {
            if (player == null)
                return;

            if (playersIn_Area01_Subarea00.Contains(player))
                playersIn_Area01_Subarea00.Remove(player);

            if (playersIn_Area01_Subarea01.Contains(player))
                playersIn_Area01_Subarea01.Remove(player);

            if (playersIn_Area01_Subarea02.Contains(player))
                playersIn_Area01_Subarea02.Remove(player);

            if (playersIn_Area01_Subarea03.Contains(player))
                playersIn_Area01_Subarea03.Remove(player);

            if (playersIn_Area01_Subarea04.Contains(player))
                playersIn_Area01_Subarea04.Remove(player);

            if (playersIn_Area01_Subarea05.Contains(player))
                playersIn_Area01_Subarea05.Remove(player);
        }

        private void AddPlayerToNewLocation(WorldSceneLocation area, PlayerManager player)
        {
            switch (area)
            {
                case WorldSceneLocation.Area01_Subarea00:
                    if (!playersIn_Area01_Subarea00.Contains(player))
                        playersIn_Area01_Subarea00.Add(player);
                    break;

                case WorldSceneLocation.Area01_Subarea01:
                    if (!playersIn_Area01_Subarea01.Contains(player))
                        playersIn_Area01_Subarea01.Add(player);
                    break;

                case WorldSceneLocation.Area01_Subarea02:
                    if (!playersIn_Area01_Subarea02.Contains(player))
                        playersIn_Area01_Subarea02.Add(player);
                    break;

                case WorldSceneLocation.Area01_Subarea03:
                    if (!playersIn_Area01_Subarea03.Contains(player))
                        playersIn_Area01_Subarea03.Add(player);
                    break;

                case WorldSceneLocation.Area01_Subarea04:
                    if (!playersIn_Area01_Subarea04.Contains(player))
                        playersIn_Area01_Subarea04.Add(player);
                    break;

                case WorldSceneLocation.Area01_Subarea05:
                    if (!playersIn_Area01_Subarea05.Contains(player))
                        playersIn_Area01_Subarea05.Add(player);
                    break;

                default:
                    break;
            }
        }

        private void LoadAdditiveScenesAroundCurrentArea(WorldSceneLocation area)
        {
            List<string> scenesToLoad = new List<string>();

            switch (area)
            {
                case WorldSceneLocation.Area01_Subarea00:
                    scenesToLoad.Add(WorldSceneManager.instance.area_01_Subarea_00);
                    scenesToLoad.Add(WorldSceneManager.instance.area_01_Subarea_01);
                    scenesToLoad.Add(WorldSceneManager.instance.area_01_Subarea_04);
                    break;

                case WorldSceneLocation.Area01_Subarea01:
                    scenesToLoad.Add(WorldSceneManager.instance.area_01_Subarea_01);
                    scenesToLoad.Add(WorldSceneManager.instance.area_01_Subarea_00);
                    scenesToLoad.Add(WorldSceneManager.instance.area_01_Subarea_02);
                    scenesToLoad.Add(WorldSceneManager.instance.area_01_Subarea_04);
                    break;

                case WorldSceneLocation.Area01_Subarea02:
                    scenesToLoad.Add(WorldSceneManager.instance.area_01_Subarea_02);
                    scenesToLoad.Add(WorldSceneManager.instance.area_01_Subarea_03);
                    scenesToLoad.Add(WorldSceneManager.instance.area_01_Subarea_01);
                    break;

                case WorldSceneLocation.Area01_Subarea03:
                    scenesToLoad.Add(WorldSceneManager.instance.area_01_Subarea_03);
                    scenesToLoad.Add(WorldSceneManager.instance.area_01_Subarea_02);
                    scenesToLoad.Add(WorldSceneManager.instance.area_01_Subarea_04);
                    break;

                case WorldSceneLocation.Area01_Subarea04:
                    scenesToLoad.Add(WorldSceneManager.instance.area_01_Subarea_04);
                    scenesToLoad.Add(WorldSceneManager.instance.area_01_Subarea_00);
                    scenesToLoad.Add(WorldSceneManager.instance.area_01_Subarea_01);
                    scenesToLoad.Add(WorldSceneManager.instance.area_01_Subarea_03);
                    scenesToLoad.Add(WorldSceneManager.instance.area_01_Subarea_05);
                    break;

                case WorldSceneLocation.Area01_Subarea05:
                    scenesToLoad.Add(WorldSceneManager.instance.area_01_Subarea_05);
                    scenesToLoad.Add(WorldSceneManager.instance.area_01_Subarea_00);
                    scenesToLoad.Add(WorldSceneManager.instance.area_01_Subarea_01);
                    scenesToLoad.Add(WorldSceneManager.instance.area_01_Subarea_04);
                    break;

                default:
                    break;
            }

            if (scenesToLoad.Count <= 0)
                return;

            WorldSceneManager.instance.LoadAdditiveScenes(scenesToLoad);
        }


    }
}
