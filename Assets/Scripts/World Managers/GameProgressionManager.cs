using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

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
            new MapProgressionDefinition { mapName = "Map 1", sceneBuildIndex = 1, bossID = 0, entrySiteOfGraceID = 0, enemyHealthMultiplier = 1f, enemyDamageMultiplier = 1f },
            new MapProgressionDefinition { mapName = "Map 2", sceneBuildIndex = 2, bossID = 1, entrySiteOfGraceID = 100, enemyHealthMultiplier = 1.15f, enemyDamageMultiplier = 1.1f },
            new MapProgressionDefinition { mapName = "Map 3", sceneBuildIndex = 3, bossID = 2, entrySiteOfGraceID = 200, enemyHealthMultiplier = 1.35f, enemyDamageMultiplier = 1.2f },
            new MapProgressionDefinition { mapName = "Map 4", sceneBuildIndex = 4, bossID = 3, entrySiteOfGraceID = 300, enemyHealthMultiplier = 1.6f, enemyDamageMultiplier = 1.35f },
            new MapProgressionDefinition { mapName = "Map 5", sceneBuildIndex = 5, bossID = 4, entrySiteOfGraceID = 400, enemyHealthMultiplier = 1.9f, enemyDamageMultiplier = 1.5f }
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

            instance = FindFirstObjectByType<GameProgressionManager>();

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

        private void OnValidate()
        {
            SyncDefinitionsFromConfigIfPresent();
            EnsureConfigurationIsValid();

#if UNITY_EDITOR
            if (!Application.isPlaying)
                EditorUtility.SetDirty(this);
#endif
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

            if (defeatedMapIndex < 0)
                defeatedMapIndex = currentMapIndex;

            UnlockMap(defeatedMapIndex);

            if (defeatedMapIndex >= TotalMapCount - 1)
            {
                currentMapIndex = defeatedMapIndex;
                gameWon = true;
                hasWonGame = true;
                return false;
            }

            unlockedMapIndex = defeatedMapIndex + 1;
            UnlockMap(unlockedMapIndex);
            currentMapIndex = unlockedMapIndex;
            pendingTransitionSiteOfGraceID = GetEntrySiteOfGraceIDForMap(currentMapIndex);
            nextSceneBuildIndex = GetSceneBuildIndexForMap(currentMapIndex);

            string nextScenePath = UnityEngine.SceneManagement.SceneUtility.GetScenePathByBuildIndex(nextSceneBuildIndex);

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

        public float GetEnemyHealthMultiplierForCurrentMap()
        {
            return GetEnemyHealthMultiplierForMap(currentMapIndex);
        }

        public float GetEnemyDamageMultiplierForCurrentMap()
        {
            return GetEnemyDamageMultiplierForMap(currentMapIndex);
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
                            sceneBuildIndex = i + 1,
                            bossID = i,
                            entrySiteOfGraceID = i == 0 ? 0 : i * 100,
                            enemyHealthMultiplier = 1f,
                            enemyDamageMultiplier = 1f
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
                        sceneBuildIndex = i + 1,
                        bossID = i,
                        entrySiteOfGraceID = i == 0 ? 0 : i * 100,
                        enemyHealthMultiplier = 1f,
                        enemyDamageMultiplier = 1f
                    };
                }

                if (string.IsNullOrWhiteSpace(mapDefinitions[i].mapName))
                    mapDefinitions[i].mapName = $"Map {i + 1}";

                if (mapDefinitions[i].sceneBuildIndex <= 0)
                    mapDefinitions[i].sceneBuildIndex = DefaultWorldSceneBuildIndex;

                if (mapDefinitions[i].enemyHealthMultiplier <= 0f)
                    mapDefinitions[i].enemyHealthMultiplier = 1f;

                if (mapDefinitions[i].enemyDamageMultiplier <= 0f)
                    mapDefinitions[i].enemyDamageMultiplier = 1f;
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

            if (mapDefinitions == null || mapDefinitions.Length != progressionConfig.mapDefinitions.Length)
                mapDefinitions = new MapProgressionDefinition[progressionConfig.mapDefinitions.Length];

            for (int i = 0; i < progressionConfig.mapDefinitions.Length; i++)
            {
                MapProgressionDefinition source = progressionConfig.mapDefinitions[i];

                if (source == null)
                {
                    mapDefinitions[i] = null;
                    continue;
                }

                mapDefinitions[i] = new MapProgressionDefinition
                {
                    mapName = source.mapName,
                    sceneBuildIndex = source.sceneBuildIndex,
                    bossID = source.bossID,
                    entrySiteOfGraceID = source.entrySiteOfGraceID,
                    enemyHealthMultiplier = source.enemyHealthMultiplier,
                    enemyDamageMultiplier = source.enemyDamageMultiplier
                };
            }
        }

        private float GetEnemyHealthMultiplierForMap(int mapIndex)
        {
            if (mapDefinitions == null || mapDefinitions.Length <= 0)
                return 1f;

            int clampedMapIndex = Mathf.Clamp(mapIndex, 0, mapDefinitions.Length - 1);
            MapProgressionDefinition definition = mapDefinitions[clampedMapIndex];

            if (definition == null || definition.enemyHealthMultiplier <= 0f)
                return 1f;

            return definition.enemyHealthMultiplier;
        }

        private float GetEnemyDamageMultiplierForMap(int mapIndex)
        {
            if (mapDefinitions == null || mapDefinitions.Length <= 0)
                return 1f;

            int clampedMapIndex = Mathf.Clamp(mapIndex, 0, mapDefinitions.Length - 1);
            MapProgressionDefinition definition = mapDefinitions[clampedMapIndex];

            if (definition == null || definition.enemyDamageMultiplier <= 0f)
                return 1f;

            return definition.enemyDamageMultiplier;
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
