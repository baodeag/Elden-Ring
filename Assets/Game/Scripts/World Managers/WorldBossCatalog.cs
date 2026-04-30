using UnityEngine;

namespace baodeag
{
    [System.Serializable]
    public class WorldBossDefinition
    {
        public string worldName = "World_01";
        public int sceneBuildIndex = 1;
        public int bossID;
        public GameObject bossPrefab;
    }

    [CreateAssetMenu(menuName = "Game/World Boss Catalog")]
    public class WorldBossCatalog : ScriptableObject
    {
        private const string DefaultResourcePath = "WorldBossCatalog";

        [SerializeField] private WorldBossDefinition[] bosses = new WorldBossDefinition[0];

        public static WorldBossCatalog LoadDefault()
        {
            return Resources.Load<WorldBossCatalog>(DefaultResourcePath);
        }

        public GameObject GetBossPrefabForScene(int sceneBuildIndex)
        {
            for (int i = 0; i < bosses.Length; i++)
            {
                if (bosses[i] != null && bosses[i].sceneBuildIndex == sceneBuildIndex)
                    return bosses[i].bossPrefab;
            }

            return null;
        }

        public int GetBossIDForScene(int sceneBuildIndex)
        {
            for (int i = 0; i < bosses.Length; i++)
            {
                if (bosses[i] != null && bosses[i].sceneBuildIndex == sceneBuildIndex)
                    return bosses[i].bossID;
            }

            return sceneBuildIndex - 1;
        }
    }
}
