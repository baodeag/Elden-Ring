using UnityEngine;

namespace baodeag
{
    public class WorldSoundFXManager : MonoBehaviour
    {
        public static WorldSoundFXManager instance;

        [Header("Damage Sounds")]
        public AudioClip[] physicalDamageSFX;

        [Header("Action Sounds")]
        public AudioClip rollSFX;

        private void Awake()
        {
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
            DontDestroyOnLoad(gameObject);
        }

        public AudioClip ChooseRandomSFXFromArray(AudioClip[] array)
        {
            int index = Random.Range(0, array.Length);
            return array[index];
        }

        /*
        public AudioClip ChooseRandomFootStepSoundBasedOnGround(GameObject steppedOnObject, CharacterManager character)
        {
            if (steppedOnObject.tag == "Dirt")
            {
                return ChooseRandomSFXFromArray(character.characterSoundFXManager.footStepsDirt);
            }
            else if (steppedOnObject.tag == "Stone")
            {
                return ChooseRandomSFXFromArray(character.characterSoundFXManager.footStepsStone);
            }

            return null;
        }
        */
    }
}
