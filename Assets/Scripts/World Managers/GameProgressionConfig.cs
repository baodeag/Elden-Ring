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

    [CreateAssetMenu(menuName = "Game/Progression Config")]
    public class GameProgressionConfig : ScriptableObject
    {
        public MapProgressionDefinition[] mapDefinitions = new MapProgressionDefinition[5]
        {
            new MapProgressionDefinition { mapName = "Map 1", sceneBuildIndex = 1, bossID = 0, entrySiteOfGraceID = -1 },
            new MapProgressionDefinition { mapName = "Map 2", sceneBuildIndex = 1, bossID = 1, entrySiteOfGraceID = -1 },
            new MapProgressionDefinition { mapName = "Map 3", sceneBuildIndex = 1, bossID = 2, entrySiteOfGraceID = -1 },
            new MapProgressionDefinition { mapName = "Map 4", sceneBuildIndex = 1, bossID = 3, entrySiteOfGraceID = -1 },
            new MapProgressionDefinition { mapName = "Map 5", sceneBuildIndex = 1, bossID = 4, entrySiteOfGraceID = -1 }
        };
    }
}
