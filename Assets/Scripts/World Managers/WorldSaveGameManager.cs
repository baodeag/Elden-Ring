using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace baodeag { 
    public class WorldSaveGameManager : MonoBehaviour
    {
        public static WorldSaveGameManager instance;

        [SerializeField] PlayerManager player;

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
        //public CharacterSaveData characterSlots02;
        //public CharacterSaveData characterSlots03;
        //public CharacterSaveData characterSlots04;
        //public CharacterSaveData characterSlots05;
        //public CharacterSaveData characterSlots06;
        //public CharacterSaveData characterSlots07;
        //public CharacterSaveData characterSlots08;
        //public CharacterSaveData characterSlots09;
        //public CharacterSaveData characterSlots10;

        private void Awake()
        {
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
        }

        private void Update()
        {
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

        private void DecideCharacterFileNameBasedOnCharacterSlotBeingUsed()
        {
            switch (currentCharacterSlotBeingUsed)
            {
                case CharacterSlot.CharacterSlot_01:
                    saveFileName = "characterSlot_01";
                    break;
                case CharacterSlot.CharacterSlot_02:
                    saveFileName = "characterSlot_02";
                    break;
                case CharacterSlot.CharacterSlot_03:
                    saveFileName = "characterSlot_03";
                    break;
                case CharacterSlot.CharacterSlot_04:
                    saveFileName = "characterSlot_04";
                    break;
                case CharacterSlot.CharacterSlot_05:
                    saveFileName = "characterSlot_05";
                    break;
                case CharacterSlot.CharacterSlot_06:
                    saveFileName = "characterSlot_06";
                    break;
                case CharacterSlot.CharacterSlot_07:
                    saveFileName = "characterSlot_07";
                    break;
                case CharacterSlot.CharacterSlot_08:
                    saveFileName = "characterSlot_08";
                    break;
                case CharacterSlot.CharacterSlot_09:
                    saveFileName = "characterSlot_09";
                    break;
                case CharacterSlot.CharacterSlot_10:
                    saveFileName = "characterSlot_10";
                    break;
            }
        }

        public void CreateNewGame()
        {
            //creat a new file, with a file name depending on which slot we are using
            DecideCharacterFileNameBasedOnCharacterSlotBeingUsed();

            currentCharacterData = new CharacterSaveData();
        }

        public void LoadGame()
        {
            //load the file, with a file name depending on which slot we are using
            DecideCharacterFileNameBasedOnCharacterSlotBeingUsed();
            
            saveFileDataWriter = new SaveFileDataWriter();
            //generally works on multiple machines types
            saveFileDataWriter.saveDataDirectoryPath = Application.persistentDataPath;
            saveFileDataWriter.saveFilename = saveFileName;
            currentCharacterData = saveFileDataWriter.LoadSaveFile();

            StartCoroutine(LoadWorldScene());
        }

        public void SaveGame()
        {
            //save the current file under a file name depending on which slot we are using
            DecideCharacterFileNameBasedOnCharacterSlotBeingUsed();

            saveFileDataWriter = new SaveFileDataWriter();
            //generally works on multiple machines types
            saveFileDataWriter.saveDataDirectoryPath = Application.persistentDataPath;
            saveFileDataWriter.saveFilename = saveFileName;

            //pass the player info, from game, to their save file
            player.SaveGameDataToCurrentCharacterData(ref currentCharacterData);

            //write that info onto a json file, saved to this machine
            saveFileDataWriter.CreateNewCharacterSaveFile(currentCharacterData);
        }

        public IEnumerator LoadWorldScene()
        {
            AsyncOperation loadOperation = SceneManager.LoadSceneAsync(worldSceneIndex);

            yield return null;
        }

        public int GetWorldSceneIndex()
        {
            return worldSceneIndex;
        }
    }
}
