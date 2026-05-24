using UnityEngine;
using TMPro;

namespace baodeag
{
    public class UI_Character_Save_Slot : MonoBehaviour
    {
        SaveFileDataWriter saveFileWriter;

        [Header("Game Slot")]
        public CharacterSlot characterSlot;

        [Header("Character Info")]
        public TextMeshProUGUI characterName;
        public TextMeshProUGUI timePlayed;

        private void OnEnable()
        {
            LoadSaveSlots();
        }

        private void LoadSaveSlots()
        {
            CharacterSaveData characterData = WorldSaveGameManager.instance.GetCharacterDataForSlot(characterSlot);

            saveFileWriter = new SaveFileDataWriter();
            saveFileWriter.saveDataDirectoryPath = Application.persistentDataPath;
            saveFileWriter.saveFilename = WorldSaveGameManager.instance.DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(characterSlot);

            if (!saveFileWriter.CheckToSeeIfFileExists() || characterData == null)
            {
                gameObject.SetActive(false);
                return;
            }

            characterName.text = characterData.characterName;

            if (timePlayed != null)
                timePlayed.text = WorldSaveGameManager.FormatDuration(characterData.secondsPlayed);
        }

        public void LoadGameFromCharacterSlot()
        {
            WorldSaveGameManager.instance.currentCharacterSlotBeingUsed = characterSlot;
            WorldSaveGameManager.instance.LoadGame();
        }

        public void SelectCurrentSlot()
        {
            TitleScreenManager.Instance.SelectCharacterSlot(characterSlot);
        }
    }
}
