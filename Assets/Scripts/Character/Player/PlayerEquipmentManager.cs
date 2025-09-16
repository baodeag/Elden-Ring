using UnityEngine;

namespace baodeag
{
    public class PlayerEquipmentManager : CharacterEquipmentManager
    {
        PlayerManager player;

        public WeaponModelInstantiationSlot rightHandSlot;
        public WeaponModelInstantiationSlot leftHandSlot;

        public GameObject rightHandWeaponModel;
        public GameObject leftHandWeaponModel;

        protected override void Awake()
        {
            base.Awake();
            player = GetComponent<PlayerManager>();
            InitializeWeaponSlot();
        }

        protected override void Start()
        {
            base.Start();
            LoadWeaponOnBothHands();
        }

        private void InitializeWeaponSlot()
        {
            WeaponModelInstantiationSlot[] weaponSlots = GetComponentsInChildren<WeaponModelInstantiationSlot>();

            foreach(var weaponSlot in weaponSlots)
            {
                if(weaponSlot.weaponSlot == WeaponModelSlot.RightHand)
                {
                    rightHandSlot = weaponSlot;
                }
                else if(weaponSlot.weaponSlot == WeaponModelSlot.LeftHand)
                {
                    leftHandSlot = weaponSlot;
                }
            }
        }

        public void LoadWeaponOnBothHands()
        {
            LoadRightWeapon();
            LoadLeftWeapon();
        }

        public void LoadRightWeapon()
        {
            if(player.playerInventoryManager.currentRightHandWeapon != null)
            {
                rightHandWeaponModel = Instantiate(player.playerInventoryManager.currentRightHandWeapon.weaponModel);
                rightHandSlot.LoadWeapon(rightHandWeaponModel);
            }

        }

        public void LoadLeftWeapon()
        {
            if(player.playerInventoryManager.currentLeftHandWeapon != null)
            {
                leftHandWeaponModel = Instantiate(player.playerInventoryManager.currentLeftHandWeapon.weaponModel);
                leftHandSlot.LoadWeapon(leftHandWeaponModel);
            }
        }
    }
}
