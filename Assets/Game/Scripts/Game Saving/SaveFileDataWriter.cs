using UnityEngine;
using System;
using System.IO;

namespace baodeag
{
    public class SaveFileDataWriter
    {
        public string saveDataDirectoryPath = "";
        public string saveFilename = "";

        //before we create a new save file, we must check to see if one already exists
        public bool CheckToSeeIfFileExists()
        {
            if(File.Exists(Path.Combine(saveDataDirectoryPath, saveFilename)))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        //used to delete character save file
        public void DeleteSaveFile()
        {
            File.Delete(Path.Combine(saveDataDirectoryPath, saveFilename));
        }

        //used to create a save file upon starting a new game

        public void CreateNewCharacterSaveFile(CharacterSaveData characterData)
        {
            //make a path to save file
            string savePath = Path.Combine(saveDataDirectoryPath, saveFilename);

            try
            {
                //create the directory the file will be written to if it doesn't already exist
                Directory.CreateDirectory(Path.GetDirectoryName(savePath));
                

                //serialize the c# game data object into json format
                string dataStore = JsonUtility.ToJson(characterData, true);

                //write the file to our system
                using (FileStream stream = new FileStream(savePath, FileMode.Create))
                {
                    using (StreamWriter fileWriter = new StreamWriter(stream))
                    {
                        fileWriter.Write(dataStore);
                    }
                }
            }
            catch (Exception)
            {
                
            }
        }

        //used to load a save file upon loading a previous game
        public CharacterSaveData LoadSaveFile()
        {
            CharacterSaveData characterData = null;
            //make a path to load file
            string loadPath = Path.Combine(saveDataDirectoryPath, saveFilename);

            if (File.Exists(loadPath))
            {
                try
                {
                    string dataToLoad = "";

                    using (FileStream stream = new FileStream(loadPath, FileMode.Open))
                    {
                        using (StreamReader reader = new StreamReader(stream))
                        {
                            dataToLoad = reader.ReadToEnd();
                        }
                    }

                    //deserialize the json data back into a c# game data object
                    characterData = JsonUtility.FromJson<CharacterSaveData>(dataToLoad);
                }

                catch (Exception)
                {
                    
                }
            }
            return characterData;
        }
    }
}
