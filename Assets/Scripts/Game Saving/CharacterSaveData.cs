using UnityEngine;
using UnityEngine.Rendering;

namespace baodeag
{
    [System.Serializable]
    //since we want to ref this data for every save file, this scripts is not a monobehaviour and is instead serializable
    public class CharacterSaveData 
    {
        [Header("Scene Index")]
        public int sceneIndex = 1;

        [Header("Character Name")]
        public string characterName = "Character";

        [Header("Time Played")]
        public float secondsPlayed;


        // we can only save data from basic variables types (int, float, string, bool)
        [Header("World Coordinates")]
        public float xPosition;
        public float yPosition;
        public float zPosition;

        [Header("Resources")]
        public int currentHealth;
        public float currentStamina;

        [Header("Stats")]
        public int vitality;
        public int endurance;

        [Header("Boss")]
        public SerializableDictionary<int, bool> bossesAwakened; //the int is the boss ID, the bool is the awakened state
        public SerializableDictionary<int, bool> bossesDefeated; //the int is the boss ID, the bool is the defeated state

        public CharacterSaveData()
        {
            bossesAwakened = new SerializableDictionary<int, bool>();
            bossesDefeated = new SerializableDictionary<int, bool>();
        }
    }
}
