using UnityEngine;

namespace baodeag
{
    public class PlayerBodyManager : MonoBehaviour
    {
        [Header("Hair Object")]
        [SerializeField] public GameObject hair; //hair object to enable when unequipping helmets
        [SerializeField] public GameObject facialHair; //facial hair object to enable when unequipping helmets

        [Header("Male")]
        [SerializeField] public GameObject maleHead; //default head model when unequipping armor
        [SerializeField] public GameObject[] maleBody; //default body model when unequipping armor (chest, upper right arm, upper left arm)
        [SerializeField] public GameObject[] maleArms; //default arm models when unequipping armor (lower right arm, lower left arm, hands)
        [SerializeField] public GameObject[] maleLegs; //default leg models when unequipping armor (legs)
        [SerializeField] public GameObject maleEyebrows; //facial feature
        [SerializeField] public GameObject maleFacialHair; //facial feature

        [Header("Female")]
        [SerializeField] public GameObject femaleHead;
        [SerializeField] public GameObject[] femaleBody;
        [SerializeField] public GameObject[] femaleArms;
        [SerializeField] public GameObject[] femaleLegs;
        [SerializeField] public GameObject femaleEyebrows;


        //enable body features
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

        public void EnableHair()
        {
            hair.SetActive(true);
        }

        public void DisableHair()
        {
            hair.SetActive(false);
        }

        public void EnableFacialHair()
        {
            facialHair.SetActive(false);
        }

        public void DisableFacialHair()
        {
            facialHair.SetActive(false);
        }

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
    }
}

