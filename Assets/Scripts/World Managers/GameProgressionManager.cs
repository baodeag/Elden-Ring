using UnityEngine;

namespace baodeag
{
    public class GameProgressionManager : MonoBehaviour
    {
        public static GameProgressionManager instance;

        private const int DefaultWorldSceneBuildIndex = 1;
        private const int TotalMapCount = 5;

        [Header("Config Asset")]
        [SerializeField] private GameProgressionConfig progressionConfig;

        [Header("Map Definitions")]
        [SerializeField] private MapProgressionDefinition[] mapDefinitions = new MapProgressionDefinition[TotalMapCount]
        {
            new MapProgressionDefinition { mapName = "Map 1", sceneBuildIndex = DefaultWorldSceneBuildIndex, bossID = 0 },
            new MapProgressionDefinition { mapName = "Map 2", sceneBuildIndex = DefaultWorldSceneBuildIndex, bossID = 1 },
            new MapProgressionDefinition { mapName = "Map 3", sceneBuildIndex = DefaultWorldSceneBuildIndex, bossID = 2 },
            new MapProgressionDefinition { mapName = "Map 4", sceneBuildIndex = DefaultWorldSceneBuildIndex, bossID = 3 },
            new MapProgressionDefinition { mapName = "Map 5", sceneBuildIndex = DefaultWorldSceneBuildIndex, bossID = 4 }
        };

        [Header("Current Progression")]
        [SerializeField] private int startingClassID = -1;
        [SerializeField] private int currentMapIndex;
        [SerializeField] private bool gameWon;
        [SerializeField] private SerializableDictionary<int, bool> mapsUnlocked = new SerializableDictionary<int, bool>();
        [SerializeField] private int pendingTransitionSiteOfGraceID = -1;

        public int StartingClassID => startingClassID;
        public int CurrentMapIndex => currentMapIndex;
        public bool GameWon => gameWon;

        public static GameProgressionManager Instance
        {
            get
            {
                EnsureInstance();
                return instance;
            }
        }

        public static void EnsureInstance()
        {
            if (instance != null)
                return;

            GameObject managerObject = new GameObject("Game Progression Manager");
            instance = managerObject.AddComponent<GameProgressionManager>();
        }

        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
                DontDestroyOnLoad(gameObject);
                EnsureConfigurationIsValid();
                ValidateConfigurationAndLogWarnings();
                return;
            }

            if (instance != this)
            {
                Destroy(gameObject);
                return;
            }

            DontDestroyOnLoad(gameObject);
            EnsureConfigurationIsValid();
            ValidateConfigurationAndLogWarnings();
        }

        public void ResetForNewGame(int selectedStartingClassID)
        {
            EnsureConfigurationIsValid();

            startingClassID = selectedStartingClassID;
            currentMapIndex = 0;
            gameWon = false;
            pendingTransitionSiteOfGraceID = GetEntrySiteOfGraceIDForMap(currentMapIndex);

            mapsUnlocked.Clear();
            UnlockMap(0);
        }

        public void LoadFromCharacterData(CharacterSaveData characterData)
        {
            EnsureConfigurationIsValid();

            if (characterData == null)
            {
                ResetForNewGame(-1);
                return;
            }

            characterData.EnsureCollectionsInitialized();

            startingClassID = characterData.startingClassID;
            currentMapIndex = Mathf.Clamp(characterData.currentMapIndex, 0, TotalMapCount - 1);
            gameWon = characterData.gameWon;
            pendingTransitionSiteOfGraceID = -1;

            mapsUnlocked.Clear();

            foreach (var unlockedMap in characterData.mapsUnlocked)
            {
                mapsUnlocked[unlockedMap.Key] = unlockedMap.Value;
            }

            if (mapsUnlocked.Count == 0)
            {
                UnlockMap(0);
            }
            else if (!IsMapUnlocked(currentMapIndex))
            {
                UnlockMap(currentMapIndex);
            }
        }

        public void SaveToCharacterData(CharacterSaveData characterData)
        {
            if (characterData == null)
                return;

            EnsureConfigurationIsValid();
            characterData.EnsureCollectionsInitialized();

            characterData.startingClassID = startingClassID;
            characterData.currentMapIndex = currentMapIndex;
            characterData.gameWon = gameWon;
            characterData.sceneIndex = GetSceneBuildIndexForMap(currentMapIndex, characterData.sceneIndex);

            characterData.mapsUnlocked.Clear();

            foreach (var unlockedMap in mapsUnlocked)
            {
                characterData.mapsUnlocked[unlockedMap.Key] = unlockedMap.Value;
            }
        }

        public bool RegisterBossDefeat(int bossID, out int nextSceneBuildIndex, out int unlockedMapIndex, out bool hasWonGame)
        {
            EnsureConfigurationIsValid();
            ValidateConfigurationAndLogWarnings();

            nextSceneBuildIndex = -1;
            unlockedMapIndex = -1;
            hasWonGame = false;

            int defeatedMapIndex = GetMapIndexForBossID(bossID);

            Debug.Log($"GameProgressionManager.RegisterBossDefeat: bossID={bossID}, currentMapIndex(before)={currentMapIndex}, defeatedMapIndex={defeatedMapIndex}");

            if (defeatedMapIndex < 0)
                defeatedMapIndex = currentMapIndex;

            UnlockMap(defeatedMapIndex);

            if (defeatedMapIndex >= TotalMapCount - 1)
            {
                currentMapIndex = defeatedMapIndex;
                gameWon = true;
                hasWonGame = true;
                Debug.Log($"GameProgressionManager.RegisterBossDefeat: final map cleared. gameWon={gameWon}, currentMapIndex={currentMapIndex}");
                return false;
            }

            unlockedMapIndex = defeatedMapIndex + 1;
            UnlockMap(unlockedMapIndex);
            currentMapIndex = unlockedMapIndex;
            pendingTransitionSiteOfGraceID = GetEntrySiteOfGraceIDForMap(currentMapIndex);
            nextSceneBuildIndex = GetSceneBuildIndexForMap(currentMapIndex);

            string nextScenePath = UnityEngine.SceneManagement.SceneUtility.GetScenePathByBuildIndex(nextSceneBuildIndex);

            Debug.Log($"GameProgressionManager.RegisterBossDefeat: unlockedMapIndex={unlockedMapIndex}, nextSceneBuildIndex={nextSceneBuildIndex}, nextScenePath='{nextScenePath}', pendingEntrySiteOfGraceID={pendingTransitionSiteOfGraceID}");

            if (string.IsNullOrEmpty(nextScenePath))
                return false;

            return nextSceneBuildIndex != UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex;
        }

        public bool IsMapUnlocked(int mapIndex)
        {
            EnsureConfigurationIsValid();

            if (!mapsUnlocked.ContainsKey(mapIndex))
                return false;

            return mapsUnlocked[mapIndex];
        }

        public int GetSceneBuildIndexForCurrentMap(int fallbackSceneBuildIndex = DefaultWorldSceneBuildIndex)
        {
            return GetSceneBuildIndexForMap(currentMapIndex, fallbackSceneBuildIndex);
        }

        public int GetMapIndexForSceneBuildIndex(int sceneBuildIndex)
        {
            EnsureConfigurationIsValid();

            for (int i = 0; i < mapDefinitions.Length; i++)
            {
                if (mapDefinitions[i] != null && mapDefinitions[i].sceneBuildIndex == sceneBuildIndex)
                    return i;
            }

            return -1;
        }

        public string GetMapName(int mapIndex)
        {
            EnsureConfigurationIsValid();

            if (mapDefinitions == null || mapDefinitions.Length <= 0)
                return $"Map {mapIndex + 1}";

            int clampedMapIndex = Mathf.Clamp(mapIndex, 0, mapDefinitions.Length - 1);
            MapProgressionDefinition definition = mapDefinitions[clampedMapIndex];

            if (definition == null || string.IsNullOrWhiteSpace(definition.mapName))
                return $"Map {clampedMapIndex + 1}";

            return definition.mapName;
        }

        public int GetEntrySiteOfGraceIDForCurrentMap()
        {
            return GetEntrySiteOfGraceIDForMap(currentMapIndex);
        }

        public int ConsumePendingTransitionSiteOfGraceID()
        {
            int pendingSiteOfGraceID = pendingTransitionSiteOfGraceID;
            pendingTransitionSiteOfGraceID = -1;
            return pendingSiteOfGraceID;
        }

        public bool HasPendingTransitionSiteOfGrace()
        {
            return pendingTransitionSiteOfGraceID >= 0;
        }

        public void SetCurrentMapIndex(int mapIndex)
        {
            EnsureConfigurationIsValid();
            currentMapIndex = Mathf.Clamp(mapIndex, 0, TotalMapCount - 1);
            UnlockMap(currentMapIndex);
        }

        private void UnlockMap(int mapIndex)
        {
            if (mapIndex < 0 || mapIndex >= TotalMapCount)
                return;

            mapsUnlocked[mapIndex] = true;
        }

        private int GetMapIndexForBossID(int bossID)
        {
            for (int i = 0; i < mapDefinitions.Length; i++)
            {
                if (mapDefinitions[i] != null && mapDefinitions[i].bossID == bossID)
                    return i;
            }

            return -1;
        }

        private int GetSceneBuildIndexForMap(int mapIndex, int fallbackSceneBuildIndex = DefaultWorldSceneBuildIndex)
        {
            if (mapDefinitions == null || mapDefinitions.Length <= 0)
                return fallbackSceneBuildIndex;

            int clampedMapIndex = Mathf.Clamp(mapIndex, 0, mapDefinitions.Length - 1);
            int buildIndex = mapDefinitions[clampedMapIndex].sceneBuildIndex;

            if (buildIndex <= 0)
                return fallbackSceneBuildIndex;

            return buildIndex;
        }

        private int GetEntrySiteOfGraceIDForMap(int mapIndex)
        {
            if (mapDefinitions == null || mapDefinitions.Length <= 0)
                return -1;

            int clampedMapIndex = Mathf.Clamp(mapIndex, 0, mapDefinitions.Length - 1);
            MapProgressionDefinition definition = mapDefinitions[clampedMapIndex];

            if (definition == null)
                return -1;

            return definition.entrySiteOfGraceID;
        }

        private void EnsureConfigurationIsValid()
        {
            SyncDefinitionsFromConfigIfPresent();

            if (mapDefinitions == null || mapDefinitions.Length != TotalMapCount)
            {
                MapProgressionDefinition[] definitions = new MapProgressionDefinition[TotalMapCount];

                for (int i = 0; i < definitions.Length; i++)
                {
                    if (mapDefinitions != null && i < mapDefinitions.Length && mapDefinitions[i] != null)
                    {
                        definitions[i] = mapDefinitions[i];
                    }
                    else
                    {
                        definitions[i] = new MapProgressionDefinition
                        {
                            mapName = $"Map {i + 1}",
                            sceneBuildIndex = DefaultWorldSceneBuildIndex,
                            bossID = i
                        };
                    }
                }

                mapDefinitions = definitions;
            }

            for (int i = 0; i < mapDefinitions.Length; i++)
            {
                if (mapDefinitions[i] == null)
                {
                    mapDefinitions[i] = new MapProgressionDefinition
                    {
                        mapName = $"Map {i + 1}",
                        sceneBuildIndex = DefaultWorldSceneBuildIndex,
                        bossID = i
                    };
                }

                if (string.IsNullOrWhiteSpace(mapDefinitions[i].mapName))
                    mapDefinitions[i].mapName = $"Map {i + 1}";

                if (mapDefinitions[i].sceneBuildIndex <= 0)
                    mapDefinitions[i].sceneBuildIndex = DefaultWorldSceneBuildIndex;
            }

            mapsUnlocked ??= new SerializableDictionary<int, bool>();

            if (mapsUnlocked.Count == 0)
                UnlockMap(0);

            currentMapIndex = Mathf.Clamp(currentMapIndex, 0, TotalMapCount - 1);
        }

        private void SyncDefinitionsFromConfigIfPresent()
        {
            if (progressionConfig == null || progressionConfig.mapDefinitions == null || progressionConfig.mapDefinitions.Length <= 0)
                return;

            if (mapDefinitions == progressionConfig.mapDefinitions)
                return;

            mapDefinitions = progressionConfig.mapDefinitions;
        }

        private void ValidateConfigurationAndLogWarnings()
        {
            if (mapDefinitions == null)
                return;

            for (int i = 0; i < mapDefinitions.Length; i++)
            {
                MapProgressionDefinition definition = mapDefinitions[i];

                if (definition == null)
                {
                    Debug.LogWarning($"GameProgressionManager: Map definition at index {i} is null.");
                    continue;
                }

                if (definition.sceneBuildIndex <= 0)
                {
                    Debug.LogWarning($"GameProgressionManager: '{definition.mapName}' has invalid sceneBuildIndex. It will fall back to build index {DefaultWorldSceneBuildIndex}.");
                }

                if (definition.entrySiteOfGraceID < 0)
                {
                    Debug.LogWarning($"GameProgressionManager: '{definition.mapName}' has no entrySiteOfGraceID configured. Same-scene progression and spawn handoff may fall back poorly.");
                }
            }
        }
    }
}
