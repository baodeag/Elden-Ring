using UnityEngine;

namespace baodeag
{
    public class PlayerBodyManager : MonoBehaviour
    {
        PlayerManager player;

        [Header("Hair Object")]
        [SerializeField] public GameObject hair; //hair object to enable when unequipping helmets
        [SerializeField] public GameObject facialHair; //facial hair object to enable when unequipping helmets

        [Header("Male")]
        [SerializeField] public GameObject maleObject; //the master male gameobject parent 
        [SerializeField] public GameObject maleHead; //default head model when unequipping armor
        [SerializeField] public GameObject[] maleBody; //default body model when unequipping armor (chest, upper right arm, upper left arm)
        [SerializeField] public GameObject[] maleArms; //default arm models when unequipping armor (lower right arm, lower left arm, hands)
        [SerializeField] public GameObject[] maleLegs; //default leg models when unequipping armor (legs, hips)
        [SerializeField] public GameObject maleEyebrows; //facial feature
        [SerializeField] public GameObject maleFacialHair; //facial feature

        [Header("Female")]
        [SerializeField] public GameObject femaleObject; //the master female gameobject parent 
        [SerializeField] public GameObject femaleHead;
        [SerializeField] public GameObject[] femaleBody;
        [SerializeField] public GameObject[] femaleArms;
        [SerializeField] public GameObject[] femaleLegs;
        [SerializeField] public GameObject femaleEyebrows;

        private void Awake()
        {
            player = GetComponent<PlayerManager>();
        }

        //enable head features
        public void EnableHead()
        {
            maleHead.SetActive(true);
            femaleHead.SetActive(true);

            maleEyebrows.SetActive(true);
            femaleEyebrows.SetActive(true);
        }

        public void DisableHead()
        {
            maleHead.SetActive(false);
            femaleHead.SetActive(false);

            maleEyebrows.SetActive(false);
            femaleEyebrows.SetActive(false);
        }

        //enable hair features
        public void EnableHair()
        {
            hair.SetActive(true);
        }

        public void DisableHair()
        {
            hair.SetActive(false);
        }

        //enable facial hair features
        public void EnableFacialHair()
        {
            facialHair.SetActive(true);
        }

        public void DisableFacialHair()
        {
            facialHair.SetActive(false);
        }

        //enable body parts
        public void EnableBody()
        {
            foreach (var model in maleBody)
            {
                model.SetActive(true);
            }

            foreach (var model in femaleBody)
            {
                model.SetActive(true);
            }
        }

        public void DisableBody()
        {
            foreach (var model in maleBody)
            {
                model.SetActive(false);
            }
            foreach (var model in femaleBody)
            {
                model.SetActive(false);
            }
        }

        //enable leg parts
        public void EnableLowerBody()
        {
            foreach (var model in maleLegs)
            {
                model.SetActive(true);
            }
            foreach (var model in femaleLegs)
            {
                model.SetActive(true);
            }
        }

        public void DisableLowerBody()
        {
            foreach (var model in maleLegs)
            {
                model.SetActive(false);
            }
            foreach (var model in femaleLegs)
            {
                model.SetActive(false);
            }
        }

        //enable arm parts
        public void EnableArms()
        {
            foreach (var model in maleArms)
            {
                model.SetActive(true);
            }
            foreach (var model in femaleArms)
            {
                model.SetActive(true);
            }
        }

        public void DisableArms()
        {
            foreach (var model in maleArms)
            {
                model.SetActive(false);
            }
            foreach (var model in femaleArms)
            {
                model.SetActive(false);
            }
        }

        public void ToggleBodyType(bool isMale)
        {
            if (isMale)
            {
                maleObject.SetActive(true);
                femaleObject.SetActive(false);
            }
            else
            {
                maleObject.SetActive(false);
                femaleObject.SetActive(true);
            }

            player.playerEquipmentManager.EquipArmor();
        }
    }
}

