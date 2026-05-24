using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using Unity.Netcode;
using System.Collections.Generic;

namespace baodeag { 
    public class WorldSaveGameManager : MonoBehaviour
    {
        public static WorldSaveGameManager instance;
        private const int TitleSceneBuildIndex = 0;

        public PlayerManager player;

        [Header("Save/Load")]
        [SerializeField] bool saveGame;
        [SerializeField] bool loadGame;

        [Header("World Scene Index")]
        [SerializeField] int worldSceneIndex = 1;

        [Header("Save Data Writer")]
        private SaveFileDataWriter saveFileDataWriter;

        [Header("Current Character Data")]
        public CharacterSlot currentCharacterSlotBeingUsed;
        public CharacterSaveData currentCharacterData;
        private string saveFileName;

        [Header("Character Slots")]
        public CharacterSaveData characterSlots01;
        public CharacterSaveData characterSlots02;
        public CharacterSaveData characterSlots03;
        public CharacterSaveData characterSlots04;
        public CharacterSaveData characterSlots05;
        public CharacterSaveData characterSlots06;
        public CharacterSaveData characterSlots07;
        public CharacterSaveData characterSlots08;
        public CharacterSaveData characterSlots09;
        public CharacterSaveData characterSlots10;

        [Header("Stage IDs")]
        public int namelessKnightDialogueStageID = 0;
        public int blacksmithDialogueStageID = 0;

        [Header("Dialogues")]
        [SerializeField] List<CharacterDialogue> namelessKnightDialogues = new List<CharacterDialogue>();
        [SerializeField] List<CharacterDialogue> blacksmithDialogues = new List<CharacterDialogue>();
        private bool currentCharacterPlayTimeFrozen;

        private void Awake()
        {
            GameProgressionManager.EnsureInstance();

            // There can only be one instance of this object
            if (instance == null)
            {
                instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
        }

        private void Start()
        {
            // Ensure this object persists across scene loads
            DontDestroyOnLoad(gameObject);
            LoadAllCharacterProfiles();
        }

        private void Update()
        {
            TickCurrentCharacterPlayTime();

            if (saveGame)
            {
                saveGame = false;
                SaveGame();
            }

            if (loadGame)
            {
                loadGame = false;
                LoadGame();
            }
        }

        public static string FormatDuration(float totalSeconds)
        {
            int roundedSeconds = Mathf.Max(0, Mathf.RoundToInt(totalSeconds));
            int hours = roundedSeconds / 3600;
            int minutes = (roundedSeconds % 3600) / 60;
            int seconds = roundedSeconds % 60;
            return $"{hours:00}:{minutes:00}:{seconds:00}";
        }

        public CharacterSaveData GetCharacterDataForSlot(CharacterSlot characterSlot)
        {
            return characterSlot switch
            {
                CharacterSlot.CharacterSlot_01 => characterSlots01,
                CharacterSlot.CharacterSlot_02 => characterSlots02,
                CharacterSlot.CharacterSlot_03 => characterSlots03,
                CharacterSlot.CharacterSlot_04 => characterSlots04,
                CharacterSlot.CharacterSlot_05 => characterSlots05,
                CharacterSlot.CharacterSlot_06 => characterSlots06,
                CharacterSlot.CharacterSlot_07 => characterSlots07,
                CharacterSlot.CharacterSlot_08 => characterSlots08,
                CharacterSlot.CharacterSlot_09 => characterSlots09,
                CharacterSlot.CharacterSlot_10 => characterSlots10,
                _ => null
            };
        }

        public float GetCurrentCharacterPlayedSeconds()
        {
            return currentCharacterData != null ? currentCharacterData.secondsPlayed : 0f;
        }

        public void SetCurrentCharacterPlayTimeFrozen(bool isFrozen)
        {
            currentCharacterPlayTimeFrozen = isFrozen;
        }

        public bool HasFreeCharacterSlot()
        {
            saveFileDataWriter = new SaveFileDataWriter();
            saveFileDataWriter.saveDataDirectoryPath = Application.persistentDataPath;

            //check to see if we can create a new save file (check for other existing files first)
            saveFileDataWriter.saveFilename = DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(CharacterSlot.CharacterSlot_01);

            if (!saveFileDataWriter.CheckToSeeIfFileExists())
                return true;

            saveFileDataWriter.saveFilename = DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(CharacterSlot.CharacterSlot_02);

            if (!saveFileDataWriter.CheckToSeeIfFileExists())
                return true;

            saveFileDataWriter.saveFilename = DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(CharacterSlot.CharacterSlot_03);

            if (!saveFileDataWriter.CheckToSeeIfFileExists())
                return true;

            saveFileDataWriter.saveFilename = DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(CharacterSlot.CharacterSlot_04);

            if (!saveFileDataWriter.CheckToSeeIfFileExists())
                return true;

            saveFileDataWriter.saveFilename = DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(CharacterSlot.CharacterSlot_05);

            if (!saveFileDataWriter.CheckToSeeIfFileExists())
                return true;

            saveFileDataWriter.saveFilename = DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(CharacterSlot.CharacterSlot_06);

            if (!saveFileDataWriter.CheckToSeeIfFileExists())
                return true;

            saveFileDataWriter.saveFilename = DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(CharacterSlot.CharacterSlot_07);

            if (!saveFileDataWriter.CheckToSeeIfFileExists())
                return true;

            saveFileDataWriter.saveFilename = DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(CharacterSlot.CharacterSlot_08);

            if (!saveFileDataWriter.CheckToSeeIfFileExists())
                return true;

            saveFileDataWriter.saveFilename = DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(CharacterSlot.CharacterSlot_09);

            if (!saveFileDataWriter.CheckToSeeIfFileExists())
                return true;

            saveFileDataWriter.saveFilename = DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(CharacterSlot.CharacterSlot_10);

            if (!saveFileDataWriter.CheckToSeeIfFileExists())
                return true;

            return false;
        }

        public string DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(CharacterSlot characterSlot)
        {
            string fileName = "";
            switch (characterSlot)
            {
                case CharacterSlot.CharacterSlot_01:
                    fileName = "characterSlot_01";
                    break;
                case CharacterSlot.CharacterSlot_02:
                    fileName = "characterSlot_02";
                    break;
                case CharacterSlot.CharacterSlot_03:
                    fileName = "characterSlot_03";
                    break;
                case CharacterSlot.CharacterSlot_04:
                    fileName = "characterSlot_04";
                    break;
                case CharacterSlot.CharacterSlot_05:
                    fileName = "characterSlot_05";
                    break;
                case CharacterSlot.CharacterSlot_06:
                    fileName = "characterSlot_06";
                    break;
                case CharacterSlot.CharacterSlot_07:
                    fileName = "characterSlot_07";
                    break;
                case CharacterSlot.CharacterSlot_08:
                    fileName = "characterSlot_08";
                    break;
                case CharacterSlot.CharacterSlot_09:
                    fileName = "characterSlot_09";
                    break;
                case CharacterSlot.CharacterSlot_10:
                    fileName = "characterSlot_10";
                    break;
                default:
                    break;
            }
            return fileName;
        }

        public void AttemptToCreateNewGame()
        {
            saveFileDataWriter = new SaveFileDataWriter();
            saveFileDataWriter.saveDataDirectoryPath = Application.persistentDataPath;

            //check to see if we can create a new save file (check for other existing files first)
            saveFileDataWriter.saveFilename = DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(CharacterSlot.CharacterSlot_01);

            if (!saveFileDataWriter.CheckToSeeIfFileExists())
            {
                //if this profile slot is not taken, make a new one using this slot
                currentCharacterSlotBeingUsed = CharacterSlot.CharacterSlot_01;
                currentCharacterData = new CharacterSaveData();
                NewGame();
                return;
            }

            saveFileDataWriter.saveFilename = DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(CharacterSlot.CharacterSlot_02);

            if (!saveFileDataWriter.CheckToSeeIfFileExists())
            {
                //if this profile slot is not taken, make a new one using this slot
                currentCharacterSlotBeingUsed = CharacterSlot.CharacterSlot_02;
                currentCharacterData = new CharacterSaveData();
                NewGame();
                return;
            }

            saveFileDataWriter.saveFilename = DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(CharacterSlot.CharacterSlot_03);

            if (!saveFileDataWriter.CheckToSeeIfFileExists())
            {
                //if this profile slot is not taken, make a new one using this slot
                currentCharacterSlotBeingUsed = CharacterSlot.CharacterSlot_03;
                currentCharacterData = new CharacterSaveData();
                NewGame();
                return;
            }

            saveFileDataWriter.saveFilename = DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(CharacterSlot.CharacterSlot_04);

            if (!saveFileDataWriter.CheckToSeeIfFileExists())
            {
                //if this profile slot is not taken, make a new one using this slot
                currentCharacterSlotBeingUsed = CharacterSlot.CharacterSlot_04;
                currentCharacterData = new CharacterSaveData();
                NewGame();
                return;
            }

            saveFileDataWriter.saveFilename = DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(CharacterSlot.CharacterSlot_05);

            if (!saveFileDataWriter.CheckToSeeIfFileExists())
            {
                //if this profile slot is not taken, make a new one using this slot
                currentCharacterSlotBeingUsed = CharacterSlot.CharacterSlot_05;
                currentCharacterData = new CharacterSaveData();
                NewGame();
                return;
            }

            saveFileDataWriter.saveFilename = DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(CharacterSlot.CharacterSlot_06);

            if (!saveFileDataWriter.CheckToSeeIfFileExists())
            {
                //if this profile slot is not taken, make a new one using this slot
                currentCharacterSlotBeingUsed = CharacterSlot.CharacterSlot_06;
                currentCharacterData = new CharacterSaveData();
                NewGame();
                return;
            }

            saveFileDataWriter.saveFilename = DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(CharacterSlot.CharacterSlot_07);

            if (!saveFileDataWriter.CheckToSeeIfFileExists())
            {
                //if this profile slot is not taken, make a new one using this slot
                currentCharacterSlotBeingUsed = CharacterSlot.CharacterSlot_07;
                currentCharacterData = new CharacterSaveData();
                NewGame();
                return;
            }

            saveFileDataWriter.saveFilename = DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(CharacterSlot.CharacterSlot_08);

            if (!saveFileDataWriter.CheckToSeeIfFileExists())
            {
                //if this profile slot is not taken, make a new one using this slot
                currentCharacterSlotBeingUsed = CharacterSlot.CharacterSlot_08;
                currentCharacterData = new CharacterSaveData();
                NewGame();
                return;
            }

            saveFileDataWriter.saveFilename = DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(CharacterSlot.CharacterSlot_09);

            if (!saveFileDataWriter.CheckToSeeIfFileExists())
            {
                //if this profile slot is not taken, make a new one using this slot
                currentCharacterSlotBeingUsed = CharacterSlot.CharacterSlot_09;
                currentCharacterData = new CharacterSaveData();
                NewGame();
                return;
            }

            saveFileDataWriter.saveFilename = DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(CharacterSlot.CharacterSlot_10);

            if (!saveFileDataWriter.CheckToSeeIfFileExists())
            {
                //if this profile slot is not taken, make a new one using this slot
                currentCharacterSlotBeingUsed = CharacterSlot.CharacterSlot_10;
                currentCharacterData = new CharacterSaveData();
                NewGame();
                return;
            }

            //if there are no available slots, notify the player
            TitleScreenManager.Instance.DisplayNoFreeCharacterSlotPopUp();
        }

        private void NewGame()
        {
            int selectedStartingClassID = -1;

            if (TitleScreenManager.Instance != null)
                selectedStartingClassID = TitleScreenManager.Instance.GetSelectedStartingClassID();

            GameProgressionManager.Instance.ResetForNewGame(selectedStartingClassID);
            GameProgressionManager.Instance.SaveToCharacterData(currentCharacterData);

            SaveGame();

            WorldSceneManager.instance.LoadWorldScene(GameProgressionManager.Instance.GetSceneBuildIndexForCurrentMap(worldSceneIndex));
        }

        public void LoadGame()
        {
            //load the file, with a file name depending on which slot we are using
            saveFileName = DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(currentCharacterSlotBeingUsed);
            
            saveFileDataWriter = new SaveFileDataWriter();
            //generally works on multiple machines types
            saveFileDataWriter.saveDataDirectoryPath = Application.persistentDataPath;
            saveFileDataWriter.saveFilename = saveFileName;
            currentCharacterData = saveFileDataWriter.LoadSaveFile();

            if (currentCharacterData == null)
            {
                Debug.LogWarning($"WorldSaveGameManager: Could not load save data for slot '{currentCharacterSlotBeingUsed}'.");
                return;
            }

            currentCharacterData.EnsureCollectionsInitialized();
            currentCharacterPlayTimeFrozen = false;
            SetCharacterDataForSlot(currentCharacterSlotBeingUsed, currentCharacterData);

            GameProgressionManager.Instance.LoadFromCharacterData(currentCharacterData);

            int savedSceneBuildIndex = currentCharacterData.sceneIndex > 0
                ? currentCharacterData.sceneIndex
                : worldSceneIndex;
            int mapIndexFromScene = GameProgressionManager.Instance.GetMapIndexForSceneBuildIndex(savedSceneBuildIndex);

            if (mapIndexFromScene >= 0)
                GameProgressionManager.Instance.SetCurrentMapIndex(mapIndexFromScene);

            int pendingSiteOfGraceId = currentCharacterData.lastSiteOfGraceRestedAt;

            if (pendingSiteOfGraceId < 0)
                pendingSiteOfGraceId = GameProgressionManager.Instance.GetEntrySiteOfGraceIDForCurrentMap();

            GameProgressionManager.Instance.SetPendingTransitionSiteOfGraceID(pendingSiteOfGraceId);

            GetStageIDsOnLoad();

            WorldSceneManager.instance.LoadWorldScene(savedSceneBuildIndex);
        }

        public void SaveGame()
        {
            //save the current file under a file name depending on which slot we are using
            saveFileName = DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(currentCharacterSlotBeingUsed);

            saveFileDataWriter = new SaveFileDataWriter();
            //generally works on multiple machines types
            saveFileDataWriter.saveDataDirectoryPath = Application.persistentDataPath;
            saveFileDataWriter.saveFilename = saveFileName;

            if (currentCharacterData != null)
                currentCharacterData.EnsureCollectionsInitialized();

            //pass the player info, from game, to their save file
            player.SaveGameDataToCurrentCharacterData(ref currentCharacterData);
            GameProgressionManager.Instance.SaveToCharacterData(currentCharacterData);

            //write that info onto a json file, saved to this machine
            saveFileDataWriter.CreateNewCharacterSaveFile(currentCharacterData);
            SetCharacterDataForSlot(currentCharacterSlotBeingUsed, currentCharacterData);
        }

        public void DeleteGame(CharacterSlot characterSlot)
        {
            //choose file base on name
            saveFileDataWriter = new SaveFileDataWriter();
            saveFileDataWriter.saveDataDirectoryPath = Application.persistentDataPath;
            saveFileDataWriter.saveFilename = DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(characterSlot);
            saveFileDataWriter.DeleteSaveFile();
            SetCharacterDataForSlot(characterSlot, null);
        }

        //load all character profiles on device when starting the game
        private void LoadAllCharacterProfiles()
        {
            saveFileDataWriter = new SaveFileDataWriter();
            saveFileDataWriter.saveDataDirectoryPath = Application.persistentDataPath;

            saveFileDataWriter.saveFilename = DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(CharacterSlot.CharacterSlot_01);
            characterSlots01 = saveFileDataWriter.LoadSaveFile();

            saveFileDataWriter.saveFilename = DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(CharacterSlot.CharacterSlot_02);
            characterSlots02 = saveFileDataWriter.LoadSaveFile();

            saveFileDataWriter.saveFilename = DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(CharacterSlot.CharacterSlot_03);
            characterSlots03 = saveFileDataWriter.LoadSaveFile();

            saveFileDataWriter.saveFilename = DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(CharacterSlot.CharacterSlot_04);
            characterSlots04 = saveFileDataWriter.LoadSaveFile();

            saveFileDataWriter.saveFilename = DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(CharacterSlot.CharacterSlot_05);
            characterSlots05 = saveFileDataWriter.LoadSaveFile();

            saveFileDataWriter.saveFilename = DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(CharacterSlot.CharacterSlot_06);
            characterSlots06 = saveFileDataWriter.LoadSaveFile();

            saveFileDataWriter.saveFilename = DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(CharacterSlot.CharacterSlot_07);
            characterSlots07 = saveFileDataWriter.LoadSaveFile();

            saveFileDataWriter.saveFilename = DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(CharacterSlot.CharacterSlot_08);
            characterSlots08 = saveFileDataWriter.LoadSaveFile();

            saveFileDataWriter.saveFilename = DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(CharacterSlot.CharacterSlot_09);
            characterSlots09 = saveFileDataWriter.LoadSaveFile();

            saveFileDataWriter.saveFilename = DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(CharacterSlot.CharacterSlot_10);
            characterSlots10 = saveFileDataWriter.LoadSaveFile();
        }

        public int GetWorldSceneIndex()
        {
            return GameProgressionManager.Instance.GetSceneBuildIndexForCurrentMap(worldSceneIndex);
        }

        public SerializableWeapon GetSerializableWeaponFromWeaponItem(WeaponItem weapon)
        {
            SerializableWeapon serializableWeapon = new SerializableWeapon();

            //get weapon id
            serializableWeapon.itemID = weapon.itemID;
            serializableWeapon.upgradeLevel = (int)weapon.upgradeLevel;

            //get ash of war id if one is present
            if (weapon.ashOfWarAction != null)
            {
                serializableWeapon.ashOfWarID = weapon.ashOfWarAction.itemID;
            }
            else
            {
                //we use an invalid id if there is no ash of war, so the value will be null if it tries to search for one using the id
                serializableWeapon.ashOfWarID = -1;
            }

            return serializableWeapon;
        }

        public SerializableRangedProjectile GetSerializableRangedProjectileFromRangedProjectileItem(RangedProjectileItem projectile)
        {
            SerializableRangedProjectile serializableProjectile = new SerializableRangedProjectile();

            if (projectile != null)
            {
                //get projectile id
                serializableProjectile.itemID = projectile.itemID;
                serializableProjectile.itemAmount = projectile.currentAmmoAmount;
            }
            else
            {
                serializableProjectile.itemID = -1;
            }

            return serializableProjectile;
        }

        public SerializableFlask GetSerializableFlaskFromFlaskItem(FlaskItem flask)
        {
            SerializableFlask serializableFlask = new SerializableFlask();

            if (flask != null)
            {
                //get flask id
                serializableFlask.itemID = flask.itemID;
            }
            else
            {
                serializableFlask.itemID = -1;
            }

            return serializableFlask;
        }

        public SerializableQuickSlotItem GetSerializableQuickSlotItemFromQuickSlotItem(QuickSlotItem quickSlotItem)
        {
            SerializableQuickSlotItem serializableQuickSlotItem = new SerializableQuickSlotItem();

            if (quickSlotItem != null)
            {
                //get flask id
                serializableQuickSlotItem.itemID = quickSlotItem.itemID;
                serializableQuickSlotItem.itemAmount = quickSlotItem.itemAmount;
            }
            else
            {
                serializableQuickSlotItem.itemID = -1;
            }

            return serializableQuickSlotItem;
        }

        //load dialogue
        public CharacterDialogue GetCharacterDialogueByEnum(CharacterDialogueID characterDialogueID)
        {
            CharacterDialogue dialogue = null;

            switch (characterDialogueID)
            {
                case CharacterDialogueID.NoDialogueID:
                    break;
                case CharacterDialogueID.NamelessKnightDialogueID:
                    dialogue = FindDialogueByStageID(namelessKnightDialogueStageID, namelessKnightDialogues);
                    break;
                case CharacterDialogueID.BlacksmithDialogueID:
                    dialogue = FindDialogueByStageID(blacksmithDialogueStageID, blacksmithDialogues);
                    break;
                default:
                    break;
            }

            if (dialogue != null)
                dialogue = Instantiate(dialogue);

            return dialogue;
        }

        private CharacterDialogue FindDialogueByStageID(int stageID, List<CharacterDialogue> dialogueList)
        {
            CharacterDialogue dialogue = null;

            for (int i = 0; i < dialogueList.Count; i++)
            {
                if (dialogueList[i] == null)
                    continue;

                if (dialogueList[i].requiredStageID == stageID)
                {
                    dialogue = dialogueList[i];
                    break;
                }
            }

            return dialogue;
        }

        public void SetStageOfDialogue(CharacterDialogueID characterDialogue, int stageIndex)
        {
            switch (characterDialogue)
            {
                case CharacterDialogueID.NoDialogueID:
                    break;
                case CharacterDialogueID.NamelessKnightDialogueID:
                    namelessKnightDialogueStageID = stageIndex;
                    currentCharacterData.namelessKnightStageID = namelessKnightDialogueStageID;
                    break;
                case CharacterDialogueID.BlacksmithDialogueID:
                    blacksmithDialogueStageID = stageIndex;
                    currentCharacterData.blacksmithStageID = blacksmithDialogueStageID;
                    break;
                default:
                    break;
            }
        }

        private void GetStageIDsOnLoad()
        {
            namelessKnightDialogueStageID = currentCharacterData.namelessKnightStageID;
        }

        private void TickCurrentCharacterPlayTime()
        {
            if (!CanAdvanceCurrentCharacterPlayTime())
                return;

            currentCharacterData.secondsPlayed += Time.unscaledDeltaTime;
        }

        private bool CanAdvanceCurrentCharacterPlayTime()
        {
            if (currentCharacterPlayTimeFrozen)
                return false;

            if (currentCharacterData == null)
                return false;

            if (SceneManager.GetActiveScene().buildIndex <= TitleSceneBuildIndex)
                return false;

            return true;
        }

        private void SetCharacterDataForSlot(CharacterSlot characterSlot, CharacterSaveData characterData)
        {
            switch (characterSlot)
            {
                case CharacterSlot.CharacterSlot_01:
                    characterSlots01 = characterData;
                    break;
                case CharacterSlot.CharacterSlot_02:
                    characterSlots02 = characterData;
                    break;
                case CharacterSlot.CharacterSlot_03:
                    characterSlots03 = characterData;
                    break;
                case CharacterSlot.CharacterSlot_04:
                    characterSlots04 = characterData;
                    break;
                case CharacterSlot.CharacterSlot_05:
                    characterSlots05 = characterData;
                    break;
                case CharacterSlot.CharacterSlot_06:
                    characterSlots06 = characterData;
                    break;
                case CharacterSlot.CharacterSlot_07:
                    characterSlots07 = characterData;
                    break;
                case CharacterSlot.CharacterSlot_08:
                    characterSlots08 = characterData;
                    break;
                case CharacterSlot.CharacterSlot_09:
                    characterSlots09 = characterData;
                    break;
                case CharacterSlot.CharacterSlot_10:
                    characterSlots10 = characterData;
                    break;
            }
        }
    }
}

