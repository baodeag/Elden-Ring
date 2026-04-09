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

        [Header("Progression")]
        public int startingClassID = -1;
        public int currentMapIndex = 0;
        public bool gameWon = false;
        public SerializableDictionary<int, bool> mapsUnlocked;

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
        public SerializableDictionary<int, bool> sitesOfGrace;

        [Header("Boss")]
        public SerializableDictionary<int, bool> bossesAwakened;
        public SerializableDictionary<int, bool> bossesDefeated;

        [Header("World Items")]
        public SerializableDictionary<int, bool> worldItemsLooted;

        [Header("Shop")]
        public SerializableDictionary<string, int> merchantStockRemaining;

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
        public List<SerializableActiveBuff> activeBuffs;
        public List<int> headEquipmentInInventory;
        public List<int> bodyEquipmentInInventory;
        public List<int> handEquipmentInInventory;
        public List<int> legEquipmentInInventory;

        [Header("Dialogue")]
        public int namelessKnightStageID = 0;
        public int blacksmithStageID = 0;

        public int currentSpell;

        public CharacterSaveData()
        {
            EnsureCollectionsInitialized();
        }

        public void EnsureCollectionsInitialized()
        {
            sitesOfGrace ??= new SerializableDictionary<int, bool>();
            mapsUnlocked ??= new SerializableDictionary<int, bool>();
            bossesAwakened ??= new SerializableDictionary<int, bool>();
            bossesDefeated ??= new SerializableDictionary<int, bool>();
            worldItemsLooted ??= new SerializableDictionary<int, bool>();
            merchantStockRemaining ??= new SerializableDictionary<string, int>();

            weaponsInInventory ??= new List<SerializableWeapon>();
            projectilesInInventory ??= new List<SerializableRangedProjectile>();
            quickSlotItemsInInventory ??= new List<SerializableQuickSlotItem>();
            activeBuffs ??= new List<SerializableActiveBuff>();
            headEquipmentInInventory ??= new List<int>();
            bodyEquipmentInInventory ??= new List<int>();
            handEquipmentInInventory ??= new List<int>();
            legEquipmentInInventory ??= new List<int>();
        }
    }
}
