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
        public float enemyHealthMultiplier = 1f;
        public float enemyDamageMultiplier = 1f;
    }

    [CreateAssetMenu(menuName = "Game/Progression Config")]
    public class GameProgressionConfig : ScriptableObject
    {
        public MapProgressionDefinition[] mapDefinitions = new MapProgressionDefinition[5]
        {
            new MapProgressionDefinition { mapName = "Map 1", sceneBuildIndex = 1, bossID = 0, entrySiteOfGraceID = -1, enemyHealthMultiplier = 1f, enemyDamageMultiplier = 1f },
            new MapProgressionDefinition { mapName = "Map 2", sceneBuildIndex = 1, bossID = 1, entrySiteOfGraceID = -1, enemyHealthMultiplier = 1.15f, enemyDamageMultiplier = 1.1f },
            new MapProgressionDefinition { mapName = "Map 3", sceneBuildIndex = 1, bossID = 2, entrySiteOfGraceID = -1, enemyHealthMultiplier = 1.35f, enemyDamageMultiplier = 1.2f },
            new MapProgressionDefinition { mapName = "Map 4", sceneBuildIndex = 1, bossID = 3, entrySiteOfGraceID = -1, enemyHealthMultiplier = 1.6f, enemyDamageMultiplier = 1.35f },
            new MapProgressionDefinition { mapName = "Map 5", sceneBuildIndex = 1, bossID = 4, entrySiteOfGraceID = -1, enemyHealthMultiplier = 1.9f, enemyDamageMultiplier = 1.5f }
        };
    }
}
