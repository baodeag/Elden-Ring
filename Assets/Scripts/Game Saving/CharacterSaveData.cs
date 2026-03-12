using System.Collections.Generic;
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

        [Header("Dead Spot")]
        public bool hasDeadSpot = false;
        public float deadSpotPositionX;
        public float deadSpotPositionY;
        public float deadSpotPositionZ;
        public int deadSpotRuneCount;

        [Header("Body Type")]
        public bool isMale = true;
        public int hairStyleID;
        public float hairColorRed;
        public float hairColorGreen;
        public float hairColorBlue;

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
        public int runes;

        [Header("Stats")]
        public int vitality;
        public int mind;
        public int endurance;
        public int strength;
        public int dexterity;
        public int intelligence;
        public int faith;

        [Header("Sites of Grace")]
        public int lastSiteOfGraceRestedAt = 0;
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

        public int quickSlotIndex;
        public SerializableQuickSlotItem quickSlotItem01;
        public SerializableQuickSlotItem quickSlotItem02;
        public SerializableQuickSlotItem quickSlotItem03;

        public SerializableRangedProjectile mainProjectile;
        public SerializableRangedProjectile secondaryProjectile;

        public int currentHealthFlasksRemaining = 3;
        public int currentFocusPointsFlaskRemaining = 1;

        [Header("Inventory")]
        public List<SerializableWeapon> weaponsInInventory;
        public List<SerializableRangedProjectile> projectilesInInventory;
        public List<SerializableQuickSlotItem> quickSlotItemsInInventory;
        public List<int> headEquipmentInInventory;
        public List<int> bodyEquipmentInInventory;
        public List<int> handEquipmentInInventory;
        public List<int> legEquipmentInInventory;

        [Header("Dialogue")]
        public int namelessKnightStageID = 0;

        //this will change when we add multiple spells
        public int currentSpell;

        public CharacterSaveData()
        {
            sitesOfGrace = new SerializableDictionary<int, bool>();
            bossesAwakened = new SerializableDictionary<int, bool>();
            bossesDefeated = new SerializableDictionary<int, bool>();
            worldItemsLooted = new SerializableDictionary<int, bool>();

            weaponsInInventory = new List<SerializableWeapon>();
            projectilesInInventory = new List<SerializableRangedProjectile>();
            quickSlotItemsInInventory = new List<SerializableQuickSlotItem>();
            headEquipmentInInventory = new List<int>();
            bodyEquipmentInInventory = new List<int>();
            handEquipmentInInventory = new List<int>();
            legEquipmentInInventory = new List<int>();
        }
    }
}
