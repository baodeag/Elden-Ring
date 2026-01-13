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

        [Header("Body Type")]
        public bool isMale = true;

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
        public int currentFocusPoints;

        [Header("Stats")]
        public int vitality;
        public int endurance;
        public int mind;

        [Header("Sites of Grace")]
        public SerializableDictionary<int, bool> sitesOfGrace; //the int is the site of grace ID, the bool is the activated state

        [Header("Boss")]
        public SerializableDictionary<int, bool> bossesAwakened; //the int is the boss ID, the bool is the awakened state
        public SerializableDictionary<int, bool> bossesDefeated; //the int is the boss ID, the bool is the defeated state

        [Header("World Items")]
        public SerializableDictionary<int, bool> worldItemsLooted; //the int is the world item ID, the bool is the looted state

        [Header("Equipment")]
        public int headEquipment;
        public int bodyEquipment;
        public int legEquipment;
        public int handEquipment;

        public int rightWeaponIndex;
        public SerializableWeapon rightWeapon01;
        public SerializableWeapon rightWeapon02;
        public SerializableWeapon rightWeapon03;

        public int leftWeaponIndex;
        public SerializableWeapon leftWeapon01;
        public SerializableWeapon leftWeapon02;
        public SerializableWeapon leftWeapon03;

        public SerializableRangedProjectile mainProjectile;
        public SerializableRangedProjectile secondaryProjectile;

        //this will change when we add multiple spells
        public int currentSpell;

        public CharacterSaveData()
        {
            sitesOfGrace = new SerializableDictionary<int, bool>();
            bossesAwakened = new SerializableDictionary<int, bool>();
            bossesDefeated = new SerializableDictionary<int, bool>();
            worldItemsLooted = new SerializableDictionary<int, bool>();
        }
    }
}
