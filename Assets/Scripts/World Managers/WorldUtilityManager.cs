using UnityEngine;

namespace baodeag
{
    [System.Serializable]
    public class MapProgressionDefinition
    {
        public string mapName = "Map 1";
        public int sceneBuildIndex = 1;
        public int bossID;
        public int entrySiteOfGraceID = -1;
    }

    public class WorldUtilityManager : MonoBehaviour
    {
        public static WorldUtilityManager Instance;

        [Header("Layers")]
        [SerializeField] LayerMask characterLayers;
        [SerializeField] LayerMask enviroLayers;
        [SerializeField] LayerMask slipperyEnviroLayers;

        [Header("UI Colors")]
        [SerializeField] Color poisonedColor;

        [Header("Materials")]
        [SerializeField] Material frozenMaterial;

        [Header("Forces")]
        public float slopeSlideForce = -15;

        [Header("Detection")]
        public float hiddenTargetDetectionRadiusPenalty = 0.25f; //the modifier of distance an ai can detect their target if they are sneaking & hidden

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject);
            }

            DontDestroyOnLoad(gameObject);
        }

        public LayerMask GetCharacterLayers()
        {
            return characterLayers;
        }

        public LayerMask GetEnviroLayers()
        {
            return enviroLayers;
        }

        public LayerMask GetSlipperyEnviroLayers()
        {
            return slipperyEnviroLayers;
        }

        public Color GetPoisonedColor()
        {
            return poisonedColor;
        }

        public Material GetFrozenMaterial()
        {
            return frozenMaterial;
        }

        public bool CanIDamageThisTarget(CharacterGroup attackingCharacter, CharacterGroup targetCharacter)
        {
            if (attackingCharacter == CharacterGroup.Team01)
            {
                switch (targetCharacter)
                {
                    case CharacterGroup.Team01: return false;
                    case CharacterGroup.Team02: return true;
                    default:
                        break;
                }
            }
            else if (attackingCharacter == CharacterGroup.Team02)
            {
                switch (targetCharacter)
                {
                    case CharacterGroup.Team01: return true;
                    case CharacterGroup.Team02: return false;
                    default:
                        break;
                }
            }

            return false;
        }

        public float GetAngleOfTarget(Transform characterTransform, Vector3 targetsDirection)
        {
            targetsDirection.y = 0;
            float viewableAngle = Vector3.Angle(characterTransform.forward, targetsDirection);
            Vector3 cross = Vector3.Cross(characterTransform.forward, targetsDirection);

            if (cross.y < 0)
                viewableAngle = -viewableAngle;

            return viewableAngle;
        }

        public DamageIntensity GetDamageIntensityBasedOnPoiseDamage(float poiseDamage)
        {
            //throwing dagger, small items
            DamageIntensity damageIntensity = DamageIntensity.Ping;

            //dagger / light attacks
            if (poiseDamage >= 10)
                damageIntensity = DamageIntensity.Light;

            //standard weapons / medium attacks
            if (poiseDamage >= 30)
                damageIntensity = DamageIntensity.Medium;

            //great weapons / heavy attacks
            if (poiseDamage >= 70)
                damageIntensity = DamageIntensity.Heavy;

            //ultra weapons / colossal attacks
            if (poiseDamage >= 120)
                damageIntensity = DamageIntensity.Colossal;

            return damageIntensity;
        }

        public Vector3 GetRipostingPositionBasedOnWeaponClass(WeaponClass weaponClass)
        {
            Vector3 position = new Vector3(0.11f, 0, 0.7f);
            switch (weaponClass)
            {
                case WeaponClass.StraightSword: //change position here if you desire
                    break;
                case WeaponClass.Spear:
                    break;
                case WeaponClass.MediumShield:
                    break;
                case WeaponClass.Fist:
                    break;
                default:
                    break;
            }

            return position;
        }

        public Vector3 GetBackstabPositionBasedOnWeaponClass(WeaponClass weaponClass)
        {
            Vector3 position = new Vector3(0.12f, 0, 0.74f);
            switch (weaponClass)
            {
                case WeaponClass.StraightSword: //change position here if you desire
                    break;
                case WeaponClass.Spear:
                    break;
                case WeaponClass.MediumShield:
                    break;
                case WeaponClass.Fist:
                    break;
                default:
                    break;
            }

            return position;
        }
    }

    public class GameProgressionManager : MonoBehaviour
    {
        public static GameProgressionManager instance;

        private const int DefaultWorldSceneBuildIndex = 1;
        private const int TotalMapCount = 5;

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
                return;
            }

            if (instance != this)
            {
                Destroy(gameObject);
                return;
            }

            DontDestroyOnLoad(gameObject);
            EnsureConfigurationIsValid();
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
    }
}
