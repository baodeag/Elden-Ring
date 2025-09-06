using UnityEngine;
using UnityEngine.Rendering;

namespace baodeag
{
    [System.Serializable]
    //since we want to ref this data for every save file, this scripts is not a monobehaviour and is instead serializable
    public class CharacterSaveData 
    {
        [Header("Character Name")]
        public string characterName;

        [Header("Time Played")]
        public float secondsPlayed;

        // we can only save data from basic variables types (int, float, string, bool)
        [Header("World Coordinates")]
        public float xPosition;
        public float yPosition;
        public float zPosition;
    }
}
