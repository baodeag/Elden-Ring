using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;
using UnityEngine.UIElements;

namespace baodeag
{
    public class PlayerEquipmentManager : CharacterEquipmentManager
    {
        PlayerManager player;

        [Header("Weapon Model Instantiation Slots")]
        [HideInInspector] public WeaponModelInstantiationSlot rightHandWeaponSlot;
        [HideInInspector] public WeaponModelInstantiationSlot leftHandWeaponSlot;
        [HideInInspector] public WeaponModelInstantiationSlot leftHandShieldSlot;
        [HideInInspector] public WeaponModelInstantiationSlot backSlot;

        [Header("Weapon Models")]
        [HideInInspector] public GameObject rightHandWeaponModel;
        [HideInInspector] public GameObject leftHandWeaponModel;

        [Header("Weapon Managers")]
        public WeaponManager rightWeaponManager;
        public WeaponManager leftWeaponManager;

        [Header("General Equipment Models")]
        public GameObject hatsObject;
        [HideInInspector] public GameObject[] hats;
        public GameObject hoodsObject;
        [HideInInspector] public GameObject[] hoods;
        public GameObject faceCoversObject;
        [HideInInspector] public GameObject[] faceCovers;
        public GameObject helmetAccessoriesObject;
        [HideInInspector] public GameObject[] helmetAccessories;
        public GameObject backAccessoriesObject;
        [HideInInspector] public GameObject[] backAccessories;
        public GameObject hipAccessoriesObject;
        [HideInInspector] public GameObject[] hipAccessories;
        public GameObject rightShoulderObject;
        [HideInInspector] public GameObject[] rightShoulder;
        public GameObject rightElbowObject;
        [HideInInspector] public GameObject[] rightElbow;
        public GameObject rightKneeObject;
        [HideInInspector] public GameObject[] rightKnee;
        public GameObject leftShoulderObject;
        [HideInInspector] public GameObject[] leftShoulder;
        public GameObject leftElbowObject;
        [HideInInspector] public GameObject[] leftElbow;
        public GameObject leftKneeObject;
        [HideInInspector] public GameObject[] leftKnee;

        [Header("Male Equipment Models")]
        public GameObject maleFullHelmetObject;
        [HideInInspector] public GameObject[] maleHeadFullHelmets;
        public GameObject maleFullBodyObject;
        [HideInInspector] public GameObject[] maleBodies;
        public GameObject maleRightUpperArmObject;
        [HideInInspector] public GameObject[] maleRightUpperArms;
        public GameObject maleRightLowerArmObject;
        [HideInInspector] public GameObject[] maleRightLowerArms;
        public GameObject maleRightHandObject;
        [HideInInspector] public GameObject[] maleRightHands;
        public GameObject maleLeftUpperArmObject;
        [HideInInspector] public GameObject[] maleLeftUpperArms;
        public GameObject maleLeftLowerArmObject;
        [HideInInspector] public GameObject[] maleLeftLowerArms;
        public GameObject maleLeftHandObject;
        [HideInInspector] public GameObject[] maleLeftHands;
        public GameObject maleHipsObject;
        [HideInInspector] public GameObject[] maleHips;
        public GameObject maleRightLegObject;
        [HideInInspector] public GameObject[] maleRightLegs;
        public GameObject maleLeftLegObject;
        [HideInInspector] public GameObject[] maleLeftLegs;

        [Header("Female Equipment Models")]
        public GameObject femaleFullHelmetObject;
        [HideInInspector] public GameObject[] femaleHeadFullHelmets;
        public GameObject femaleFullBodyObject;
        [HideInInspector] public GameObject[] femaleBodies;
        public GameObject femaleRightUpperArmObject;
        [HideInInspector] public GameObject[] femaleRightUpperArms;
        public GameObject femaleRightLowerArmObject;
        [HideInInspector] public GameObject[] femaleRightLowerArms;
        public GameObject femaleRightHandObject;
        [HideInInspector] public GameObject[] femaleRightHands;
        public GameObject femaleLeftUpperArmObject;
        [HideInInspector] public GameObject[] femaleLeftUpperArms;
        public GameObject femaleLeftLowerArmObject;
        [HideInInspector] public GameObject[] femaleLeftLowerArms;
        public GameObject femaleLeftHandObject;
        [HideInInspector] public GameObject[] femaleLeftHands;
        public GameObject femaleHipsObject;
        [HideInInspector] public GameObject[] femaleHips;
        public GameObject femaleRightLegObject;
        [HideInInspector] public GameObject[] femaleRightLegs;
        public GameObject femaleLeftLegObject;
        [HideInInspector] public GameObject[] femaleLeftLegs;

        protected override void Awake()
        {
            base.Awake();
            player = GetComponent<PlayerManager>();
            InitializeWeaponSlot();
            InitializeArmorModels();        
        }

        protected override void Start()
        {
            base.Start();
            EquipWeapons();
        }

        public void EquipArmor()
        {
            LoadHeadEquipment(player.playerInventoryManager.headEquipment);
            LoadBodyEquipment(player.playerInventoryManager.bodyEquipment);
            LoadLegEquipment(player.playerInventoryManager.legEquipment);
            LoadHandEquipment(player.playerInventoryManager.handEquipment);
        }

        public void ForceClassLegPreview(string equipmentName)
        {
            if (string.IsNullOrEmpty(equipmentName))
                return;

            TryLoadClassLegFallback(equipmentName, player.playerNetworkManager.isMale.Value);
        }

        //quick slots
        public void SwitchQuickSlotItem()
        {
            if (!player.IsOwner)
                return;

            QuickSlotItem selectedItem = null;

            player.playerInventoryManager.quickSlotItemIndex += 1;

            if (player.playerInventoryManager.quickSlotItemIndex < 0 || player.playerInventoryManager.quickSlotItemIndex > 2)
            {
                player.playerInventoryManager.quickSlotItemIndex = 0;

                //we check if we are holding more than one weapon
                float itemCount = 0;
                QuickSlotItem firstItem = null;
                int firstItemPosition = 0;

                for (int i = 0; i < player.playerInventoryManager.quickSlotItemsInQuickSlots.Length; i++)
                {
                    if (player.playerInventoryManager.quickSlotItemsInQuickSlots[i] != null)
                    {
                        itemCount += 1;

                        if (firstItem == null)
                        {
                            firstItem = player.playerInventoryManager.quickSlotItemsInQuickSlots[i];
                            firstItemPosition = i;
                        }
                    }
                }

                if (itemCount <= 1)
                {
                    //if we are only holding one weapon, we just equip it
                    player.playerInventoryManager.quickSlotItemIndex = -1;
                    selectedItem = null;
                    player.playerNetworkManager.currentQuickSlotItemID.Value = -1;
                }
                else
                {
                    player.playerInventoryManager.quickSlotItemIndex = firstItemPosition;
                    player.playerNetworkManager.currentQuickSlotItemID.Value = firstItem.itemID;
                }

                return;
            }

            //if the next weapon does not equal the unarmed weapon
            if (player.playerInventoryManager.quickSlotItemsInQuickSlots[player.playerInventoryManager.quickSlotItemIndex] != null)
            {
                selectedItem = player.playerInventoryManager.quickSlotItemsInQuickSlots[player.playerInventoryManager.quickSlotItemIndex];
                //assign the network weapon id so it switches for everyone
                player.playerNetworkManager.currentQuickSlotItemID.Value = player.playerInventoryManager.quickSlotItemsInQuickSlots[player.playerInventoryManager.quickSlotItemIndex].itemID;
            }
            else
            {
                player.playerNetworkManager.currentQuickSlotItemID.Value = -1;
            }

            if (selectedItem == null && player.playerInventoryManager.quickSlotItemIndex <= 2)
            {
                SwitchQuickSlotItem();
            }
        }

        //equipment
        private void InitializeArmorModels()
        {
            //  HATS
            List<GameObject> hatsList = new List<GameObject>();

            foreach (Transform child in hatsObject.transform)
            {
                hatsList.Add(child.gameObject);
            }

            hats = hatsList.ToArray();

            //  HOODS
            List<GameObject> hoodsList = new List<GameObject>();

            foreach (Transform child in hoodsObject.transform)
            {
                hoodsList.Add(child.gameObject);
            }

            hoods = hoodsList.ToArray();

            //  FACE COVERS
            List<GameObject> faceCoversList = new List<GameObject>();

            foreach (Transform child in faceCoversObject.transform)
            {
                faceCoversList.Add(child.gameObject);
            }

            faceCovers = faceCoversList.ToArray();

            //  HELMET ACCESSORIES
            List<GameObject> helmetAccessoriesList = new List<GameObject>();

            foreach (Transform child in helmetAccessoriesObject.transform)
            {
                helmetAccessoriesList.Add(child.gameObject);
            }

            helmetAccessories = helmetAccessoriesList.ToArray();

            //  BACK ACCESSORIES
            List<GameObject> backAccessoriesList = new List<GameObject>();

            foreach (Transform child in backAccessoriesObject.transform)
            {
                backAccessoriesList.Add(child.gameObject);
            }

            backAccessories = backAccessoriesList.ToArray();

            //  HIP ACCESSORIES
            List<GameObject> hipAccessoriesList = new List<GameObject>();

            foreach (Transform child in hipAccessoriesObject.transform)
            {
                hipAccessoriesList.Add(child.gameObject);
            }

            hipAccessories = hipAccessoriesList.ToArray();

            //  RIGHT SHOULDER
            List<GameObject> rightShoulderList = new List<GameObject>();

            foreach (Transform child in rightShoulderObject.transform)
            {
                rightShoulderList.Add(child.gameObject);
            }

            rightShoulder = rightShoulderList.ToArray();

            //  RIGHT ELBOW
            List<GameObject> rightElbowList = new List<GameObject>();

            foreach (Transform child in rightElbowObject.transform)
            {
                rightElbowList.Add(child.gameObject);
            }

            rightElbow = rightElbowList.ToArray();

            //  RIGHT KNEE
            List<GameObject> rightKneeList = new List<GameObject>();

            foreach (Transform child in rightKneeObject.transform)
            {
                rightKneeList.Add(child.gameObject);
            }

            rightKnee = rightKneeList.ToArray();

            //  LEFT SHOULDER
            List<GameObject> leftShoulderList = new List<GameObject>();

            foreach (Transform child in leftShoulderObject.transform)
            {
                leftShoulderList.Add(child.gameObject);
            }

            leftShoulder = leftShoulderList.ToArray();

            //  LEFT ELBOW
            List<GameObject> leftElbowList = new List<GameObject>();

            foreach (Transform child in leftElbowObject.transform)
            {
                leftElbowList.Add(child.gameObject);
            }

            leftElbow = leftElbowList.ToArray();

            //  LEFT KNEE
            List<GameObject> leftKneeList = new List<GameObject>();

            foreach (Transform child in leftKneeObject.transform)
            {
                leftKneeList.Add(child.gameObject);
            }

            leftKnee = leftKneeList.ToArray();

            //  MALE EQUIPMENT

            List<GameObject> maleFullHelmetsList = new List<GameObject>();

            foreach (Transform child in maleFullHelmetObject.transform)
            {
                maleFullHelmetsList.Add(child.gameObject);
            }

            maleHeadFullHelmets = maleFullHelmetsList.ToArray();

            List<GameObject> maleBodiesList = new List<GameObject>();

            foreach (Transform child in maleFullBodyObject.transform)
            {
                maleBodiesList.Add(child.gameObject);
            }

            maleBodies = maleBodiesList.ToArray();

            //  MALE RIGHT UPPER ARM
            List<GameObject> maleRightUpperArmList = new List<GameObject>();

            foreach (Transform child in maleRightUpperArmObject.transform)
            {
                maleRightUpperArmList.Add(child.gameObject);
            }

            maleRightUpperArms = maleRightUpperArmList.ToArray();

            //  MALE RIGHT LOWER ARM
            List<GameObject> maleRightLowerArmList = new List<GameObject>();

            foreach (Transform child in maleRightLowerArmObject.transform)
            {
                maleRightLowerArmList.Add(child.gameObject);
            }

            maleRightLowerArms = maleRightLowerArmList.ToArray();

            //  MALE RIGHT HANDS
            List<GameObject> maleRightHandsList = new List<GameObject>();

            foreach (Transform child in maleRightHandObject.transform)
            {
                maleRightHandsList.Add(child.gameObject);
            }

            maleRightHands = maleRightHandsList.ToArray();

            //  MALE LEFT UPPER ARM
            List<GameObject> maleLeftUpperArmList = new List<GameObject>();

            foreach (Transform child in maleLeftUpperArmObject.transform)
            {
                maleLeftUpperArmList.Add(child.gameObject);
            }

            maleLeftUpperArms = maleLeftUpperArmList.ToArray();

            //  MALE LEFT LOWER ARM
            List<GameObject> maleLeftLowerArmList = new List<GameObject>();

            foreach (Transform child in maleLeftLowerArmObject.transform)
            {
                maleLeftLowerArmList.Add(child.gameObject);
            }

            maleLeftLowerArms = maleLeftLowerArmList.ToArray();

            //  MALE LEFT HANDS
            List<GameObject> maleLeftHandsList = new List<GameObject>();

            foreach (Transform child in maleLeftHandObject.transform)
            {
                maleLeftHandsList.Add(child.gameObject);
            }

            maleLeftHands = maleLeftHandsList.ToArray();

            //  MALE HIPS
            List<GameObject> maleHipsList = new List<GameObject>();

            foreach (Transform child in maleHipsObject.transform)
            {
                maleHipsList.Add(child.gameObject);
            }

            maleHips = maleHipsList.ToArray();

            //  MALE RIGHT LEG
            List<GameObject> maleRightLegList = new List<GameObject>();

            foreach (Transform child in maleRightLegObject.transform)
            {
                maleRightLegList.Add(child.gameObject);
            }

            maleRightLegs = maleRightLegList.ToArray();

            //  MALE LEFT LEG
            List<GameObject> maleLeftLegList = new List<GameObject>();

            foreach (Transform child in maleLeftLegObject.transform)
            {
                maleLeftLegList.Add(child.gameObject);
            }

            maleLeftLegs = maleLeftLegList.ToArray();

            //  FEMALE FULL HELMETS
            List<GameObject> femaleFullHelmetsList = new List<GameObject>();

            foreach (Transform child in femaleFullHelmetObject.transform)
            {
                femaleFullHelmetsList.Add(child.gameObject);
            }

            femaleHeadFullHelmets = femaleFullHelmetsList.ToArray();

            //  FEMALE BODY
            List<GameObject> femaleBodyList = new List<GameObject>();

            foreach (Transform child in femaleFullBodyObject.transform)
            {
                femaleBodyList.Add(child.gameObject);
            }

            femaleBodies = femaleBodyList.ToArray();

            //  FEMALE RIGHT UPPER ARM
            List<GameObject> femaleRightUpperArmList = new List<GameObject>();

            foreach (Transform child in femaleRightUpperArmObject.transform)
            {
                femaleRightUpperArmList.Add(child.gameObject);
            }

            femaleRightUpperArms = femaleRightUpperArmList.ToArray();

            //  FEMALE RIGHT LOWER ARM
            List<GameObject> femaleRightLowerArmList = new List<GameObject>();

            foreach (Transform child in femaleRightLowerArmObject.transform)
            {
                femaleRightLowerArmList.Add(child.gameObject);
            }

            femaleRightLowerArms = femaleRightLowerArmList.ToArray();

            //  FEMALE RIGHT HANDS
            List<GameObject> femaleRightHandsList = new List<GameObject>();

            foreach (Transform child in femaleRightHandObject.transform)
            {
                femaleRightHandsList.Add(child.gameObject);
            }

            femaleRightHands = femaleRightHandsList.ToArray();

            //  FEMALE LEFT UPPER ARM
            List<GameObject> femaleLeftUpperArmList = new List<GameObject>();

            foreach (Transform child in femaleLeftUpperArmObject.transform)
            {
                femaleLeftUpperArmList.Add(child.gameObject);
            }

            femaleLeftUpperArms = femaleLeftUpperArmList.ToArray();

            //  FEMALE LEFT LOWER ARM
            List<GameObject> femaleLeftLowerArmList = new List<GameObject>();

            foreach (Transform child in femaleLeftLowerArmObject.transform)
            {
                femaleLeftLowerArmList.Add(child.gameObject);
            }

            femaleLeftLowerArms = femaleLeftLowerArmList.ToArray();

            //  FEMALE LEFT HANDS
            List<GameObject> femaleLeftHandsList = new List<GameObject>();

            foreach (Transform child in femaleLeftHandObject.transform)
            {
                femaleLeftHandsList.Add(child.gameObject);
            }

            femaleLeftHands = femaleLeftHandsList.ToArray();

            //  FEMALE HIPS
            List<GameObject> femaleHipsList = new List<GameObject>();

            foreach (Transform child in femaleHipsObject.transform)
            {
                femaleHipsList.Add(child.gameObject);
            }

            femaleHips = femaleHipsList.ToArray();

            //  FEMALE RIGHT LEG
            List<GameObject> femaleRightLegList = new List<GameObject>();

            foreach (Transform child in femaleRightLegObject.transform)
            {
                femaleRightLegList.Add(child.gameObject);
            }

            femaleRightLegs = femaleRightLegList.ToArray();

            //  FEMALE LEFT LEG
            List<GameObject> femaleLeftLegList = new List<GameObject>();

            foreach (Transform child in femaleLeftLegObject.transform)
            {
                femaleLeftLegList.Add(child.gameObject);
            }

            femaleLeftLegs = femaleLeftLegList.ToArray();
        }

        public void LoadHeadEquipment(HeadEquipmentItem equipment)
        {
            UnloadHeadEquipmentModels();

            if (equipment == null)
            {
                if (player.IsOwner)
                    player.playerNetworkManager.headEquipmentID.Value = -1; // -1 will never be an item id, its always null

                player.playerInventoryManager.headEquipment = null;
                return;
            }

            player.playerInventoryManager.headEquipment = equipment;

            switch (equipment.headEquipmentType)
            {
                case HeadEquipmentType.FullHelmet:
                    player.playerBodyManager.DisableHead();
                    player.playerBodyManager.DisableHair();
                    break;
                case HeadEquipmentType.Hat:
                    break;
                case HeadEquipmentType.Hood:
                    player.playerBodyManager.DisableHair();
                    break;
                case HeadEquipmentType.FaceCover:
                    player.playerBodyManager.DisableFacialHair();
                    break;
                default:
                    break;
            }

            //load head equipment models
            foreach (var model in equipment.equipmentModels)
            {
                model.LoadModel(player, player.playerNetworkManager.isMale.Value);
            }

            player.playerStatsManager.CalculateTotalArmorAbsorption();

            if (player.IsOwner)
                player.playerNetworkManager.headEquipmentID.Value = equipment.itemID;
        }

        private void UnloadHeadEquipmentModels()
        {
            foreach (var model in maleHeadFullHelmets)
            {
                model.SetActive(false);
            }

            foreach (var model in femaleHeadFullHelmets)
            {
                model.SetActive(false);
            }

            foreach (var model in hats)
            {
                model.SetActive(false);
            }

            foreach (var model in faceCovers)
            {
                model.SetActive(false);
            }

            foreach (var model in hoods)
            {
                model.SetActive(false);
            }

            foreach (var model in helmetAccessories)
            {
                model.SetActive(false);
            }

            player.playerBodyManager.EnableHead();
            player.playerBodyManager.EnableHair();
        }

        public void LoadBodyEquipment(BodyEquipmentItem equipment)
        {
            UnloadBodyEquipmentModels();

            if (equipment == null)
            {
                if (player.IsOwner)
                    player.playerNetworkManager.bodyEquipmentID.Value = -1; // -1 will never be an item id, its always null

                player.playerInventoryManager.bodyEquipment = null;
                return;
            }

            player.playerInventoryManager.bodyEquipment = equipment;

            //disable naked body
            player.playerBodyManager.DisableBody();

            //load body equipment models
            foreach (var model in equipment.equipmentModels)
            {
                model.LoadModel(player, player.playerNetworkManager.isMale.Value);
            }

            player.playerStatsManager.CalculateTotalArmorAbsorption();

            if (player.IsOwner)
                player.playerNetworkManager.bodyEquipmentID.Value = equipment.itemID;
        }

        private void UnloadBodyEquipmentModels()
        {
            foreach (var model in rightShoulder)
            {
                model.SetActive(false);
            }

            foreach (var model in rightElbow)
            {
                model.SetActive(false);
            }


            foreach (var model in leftShoulder)
            {
                model.SetActive(false);
            }

            foreach (var model in leftElbow)
            {
                model.SetActive(false);
            }

            foreach (var model in backAccessories)
            {
                model.SetActive(false);
            }

            //  MALE
            foreach (var model in maleBodies)
            {
                model.SetActive(false);
            }

            foreach (var model in maleRightUpperArms)
            {
                model.SetActive(false);
            }

            foreach (var model in maleLeftUpperArms)
            {
                model.SetActive(false);
            }

            //  FEMALE
            foreach (var model in femaleBodies)
            {
                model.SetActive(false);
            }

            foreach (var model in femaleRightUpperArms)
            {
                model.SetActive(false);
            }

            foreach (var model in femaleLeftUpperArms)
            {
                model.SetActive(false);
            }

            player.playerBodyManager.EnableBody();
        }

        public void LoadLegEquipment(LegEquipmentItem equipment)
        {
            UnloadLegEquipmentModels();

            if (equipment == null)
            {
                if (player.IsOwner)
                    player.playerNetworkManager.legEquipmentID.Value = -1; // -1 will never be an item id, its always null

                player.playerInventoryManager.legEquipment = null;
                return;
            }

            player.playerInventoryManager.legEquipment = equipment;

            //disable naked body
            player.playerBodyManager.DisableLowerBody();

            //load leg equipment models
            foreach (var model in equipment.equipmentModels)
            {
                model.LoadModel(player, player.playerNetworkManager.isMale.Value);
            }

            if (!HasVisibleLegEquipment(player.playerNetworkManager.isMale.Value))
            {
                TryLoadClassLegFallback(equipment.itemName, player.playerNetworkManager.isMale.Value);

                if (!HasVisibleLegEquipment(player.playerNetworkManager.isMale.Value))
                {
                    Debug.LogWarning($"Fallback leg equipment also failed on {player.name}. Re-enabling default lower body.");
                    player.playerBodyManager.EnableLowerBody();
                }
            }

            player.playerStatsManager.CalculateTotalArmorAbsorption();

            if (player.IsOwner)
                player.playerNetworkManager.legEquipmentID.Value = equipment.itemID;
        }

        private bool HasVisibleLegEquipment(bool isMale)
        {
            GameObject[] hips = isMale ? maleHips : femaleHips;
            GameObject[] rightLegs = isMale ? maleRightLegs : femaleRightLegs;
            GameObject[] leftLegs = isMale ? maleLeftLegs : femaleLeftLegs;

            foreach (var model in hips)
            {
                if (model.activeSelf)
                    return true;
            }

            foreach (var model in rightLegs)
            {
                if (model.activeSelf)
                    return true;
            }

            foreach (var model in leftLegs)
            {
                if (model.activeSelf)
                    return true;
            }

            return false;
        }

        private void TryLoadClassLegFallback(string equipmentName, bool isMale)
        {
            switch (equipmentName)
            {
                case "Mystic Leggings":
                    SetLegModelsActiveByName(isMale, "Chr_Hips_03", "Chr_LegRight_03", "Chr_LegLeft_03");
                    break;
                case "Ranger Leggings":
                    SetLegModelsActiveByName(isMale, "Chr_Hips_06", "Chr_LegRight_06", "Chr_LegLeft_06");
                    break;
                case "Vanguard Leggings":
                    SetLegModelsActiveByName(isMale, "Chr_Hips_13", "Chr_LegRight_13", "Chr_LegLeft_13");
                    break;
                case "Confessor Leggings":
                    SetLegModelsActiveByName(isMale, "Chr_Hips_10", "Chr_LegRight_10", "Chr_LegLeft_10");
                    break;
                default:
                    break;
            }
        }

        private void SetLegModelsActiveByName(bool isMale, string hipsCodeName, string rightLegCodeName, string leftLegCodeName)
        {
            SetActiveModelByName(isMale ? maleHips : femaleHips, BuildGenderedModelName(hipsCodeName, isMale));
            SetActiveModelByName(isMale ? maleRightLegs : femaleRightLegs, BuildGenderedModelName(rightLegCodeName, isMale));
            SetActiveModelByName(isMale ? maleLeftLegs : femaleLeftLegs, BuildGenderedModelName(leftLegCodeName, isMale));
        }

        private void SetActiveModelByName(GameObject[] models, string modelName)
        {
            foreach (var model in models)
            {
                if (model.name == modelName)
                {
                    SetModelAndParentsActive(model.transform);
                    return;
                }
            }

            Transform[] allTransforms = player.GetComponentsInChildren<Transform>(true);

            foreach (var transform in allTransforms)
            {
                if (transform.name == modelName)
                {
                    SetModelAndParentsActive(transform);
                    return;
                }
            }
        }

        private string BuildGenderedModelName(string baseName, bool isMale)
        {
            int lastUnderscore = baseName.LastIndexOf('_');

            if (lastUnderscore <= 0 || lastUnderscore >= baseName.Length - 1)
                return baseName;

            string prefix = baseName.Substring(0, lastUnderscore);
            string suffix = baseName.Substring(lastUnderscore + 1);
            string genderToken = isMale ? "Male" : "Female";

            return $"{prefix}_{genderToken}_{suffix}";
        }

        private void SetModelAndParentsActive(Transform modelTransform)
        {
            Transform current = modelTransform;

            while (current != null)
            {
                current.gameObject.SetActive(true);

                if (current == player.transform)
                    break;

                current = current.parent;
            }
        }

        private void UnloadLegEquipmentModels()
        {
            foreach (var model in maleHips)
            {
                model.SetActive(false);
            }

            foreach (var model in femaleHips)
            {
                model.SetActive(false);
            }

            foreach (var model in leftKnee)
            {
                model.SetActive(false);
            }

            foreach (var model in rightKnee)
            {
                model.SetActive(false);
            }

            foreach (var model in maleLeftLegs)
            {
                model.SetActive(false);
            }

            foreach (var model in maleRightLegs)
            {
                model.SetActive(false);
            }

            foreach (var model in femaleLeftLegs)
            {
                model.SetActive(false);
            }

            foreach (var model in femaleRightLegs)
            {
                model.SetActive(false);
            }

            player.playerBodyManager.EnableLowerBody();
        }

        public void LoadHandEquipment(HandEquipmentItem equipment)
        {
            UnloadHandEquipmentModels();

            if (equipment == null)
            {
                if (player.IsOwner)
                    player.playerNetworkManager.handEquipmentID.Value = -1; // -1 will never be an item id, its always null

                player.playerInventoryManager.handEquipment = null;
                return;
            }

            player.playerInventoryManager.handEquipment = equipment;

            //disable naked arms
            player.playerBodyManager.DisableArms();

            //load hand equipment models
            foreach (var model in equipment.equipmentModels)
            {
                model.LoadModel(player, player.playerNetworkManager.isMale.Value);
            }

            player.playerStatsManager.CalculateTotalArmorAbsorption();

            if (player.IsOwner)
                player.playerNetworkManager.handEquipmentID.Value = equipment.itemID;
        }

        private void UnloadHandEquipmentModels()
        {
            foreach (var model in maleLeftLowerArms)
            {
                model.SetActive(false);
            }

            foreach (var model in maleRightLowerArms)
            {
                model.SetActive(false);
            }

            foreach (var model in femaleLeftLowerArms)
            {
                model.SetActive(false);
            }

            foreach (var model in femaleRightLowerArms)
            {
                model.SetActive(false);
            }

            foreach (var model in maleLeftHands)
            {
                model.SetActive(false);
            }

            foreach (var model in maleRightHands)
            {
                model.SetActive(false);
            }

            foreach (var model in femaleLeftHands)
            {
                model.SetActive(false);
            }

            foreach (var model in femaleRightHands)
            {
                model.SetActive(false);
            }

            player.playerBodyManager.EnableArms();
        }

        //projectile
        public void LoadMainProjectileEquipment(RangedProjectileItem equipment)
        {
            if (equipment == null)
            {
                if (player.IsOwner)
                    player.playerNetworkManager.mainProjectileID.Value = -1; // -1 will never be an item id, its always null

                player.playerInventoryManager.mainProjectile = null;
                return;
            }

            player.playerInventoryManager.mainProjectile = equipment;

            if (player.IsOwner)
                player.playerNetworkManager.mainProjectileID.Value = equipment.itemID;
        }

        public void LoadSecondaryProjectileEquipment(RangedProjectileItem equipment)
        {
            if (equipment == null)
            {
                if (player.IsOwner)
                    player.playerNetworkManager.secondaryProjectileID.Value = -1; // -1 will never be an item id, its always null

                player.playerInventoryManager.secondaryProjectile = null;
                return;
            }

            player.playerInventoryManager.secondaryProjectile = equipment;

            if (player.IsOwner)
                player.playerNetworkManager.secondaryProjectileID.Value = equipment.itemID;
        }

        //quick slot
        public void LoadQuickSlotEquipment(QuickSlotItem equipment)
        {
            if (equipment == null)
            {
                if (player.IsOwner)
                    player.playerNetworkManager.currentQuickSlotItemID.Value = -1; // -1 will never be an item id, its always null

                player.playerInventoryManager.currentQuickSlotItem = null;
                return;
            }

            player.playerInventoryManager.currentQuickSlotItem = equipment;

            if (player.IsOwner)
                player.playerNetworkManager.currentQuickSlotItemID.Value = equipment.itemID;
        }

        //weapons
        private void InitializeWeaponSlot()
        {
            WeaponModelInstantiationSlot[] weaponSlots = GetComponentsInChildren<WeaponModelInstantiationSlot>();

            foreach(var weaponSlot in weaponSlots)
            {
                if(weaponSlot.weaponSlot == WeaponModelSlot.RightHand)
                {
                    rightHandWeaponSlot = weaponSlot;
                }
                else if(weaponSlot.weaponSlot == WeaponModelSlot.LeftHandWeaponSlot)
                {
                    leftHandWeaponSlot = weaponSlot;
                }
                else if (weaponSlot.weaponSlot == WeaponModelSlot.LeftHandShieldSlot)
                {
                    leftHandShieldSlot = weaponSlot;
                }
                else if (weaponSlot.weaponSlot == WeaponModelSlot.BackSlot)
                {
                    backSlot = weaponSlot;
                }

            }
        }

        public void EquipWeapons()
        {
            LoadRightWeapon();
            LoadLeftWeapon();
        }

        //right weapon
        public void SwitchRightWeapon()
        {
            if(!player.IsOwner)
                return;

            player.playerNetworkManager.isTwoHandingWeapon.Value = false;

            player.playerAnimatorManager.PlayTargetActionAnimation("Swap_Right_Weapon_01", false, false, true, true);

            WeaponItem selectedWeapon = null;

            player.playerInventoryManager.rightHandWeaponIndex += 1;

            if (player.playerInventoryManager.rightHandWeaponIndex < 0 || player.playerInventoryManager.rightHandWeaponIndex > 2)
            {
                player.playerInventoryManager.rightHandWeaponIndex = 0;

                //we check if we are holding more than one weapon
                float weaponCount = 0;
                WeaponItem firstWeapon = null;
                int firstWeaponPosition = 0;

                for (int i = 0; i < player.playerInventoryManager.weaponsInRightHandSlots.Length; i++)
                {
                    if (player.playerInventoryManager.weaponsInRightHandSlots[i].itemID != WorldItemDatabase.Instance.unarmedWeapon.itemID)
                    {
                        weaponCount += 1;
                        if (firstWeapon == null)
                        {
                            firstWeapon = player.playerInventoryManager.weaponsInRightHandSlots[i];
                            firstWeaponPosition = i;
                        }
                    }
                }

                if (weaponCount <= 1)
                {
                    //if we are only holding one weapon, we just equip it
                    player.playerInventoryManager.rightHandWeaponIndex = -1;
                    selectedWeapon = WorldItemDatabase.Instance.unarmedWeapon;
                    player.playerInventoryManager.currentRightHandWeapon = selectedWeapon;
                    player.playerNetworkManager.currentRightHandWeaponID.Value = selectedWeapon.itemID;
                }
                else
                {
                    player.playerInventoryManager.rightHandWeaponIndex = firstWeaponPosition;
                    player.playerInventoryManager.currentRightHandWeapon = selectedWeapon;
                    player.playerNetworkManager.currentRightHandWeaponID.Value = firstWeapon.itemID;
                }

                return;
            }

            foreach (WeaponItem weapon in player.playerInventoryManager.weaponsInRightHandSlots)
            {
                //if the next weapon does not equal the unarmed weapon
                if (player.playerInventoryManager.weaponsInRightHandSlots[player.playerInventoryManager.rightHandWeaponIndex].itemID != WorldItemDatabase.Instance.unarmedWeapon.itemID)
                {
                    selectedWeapon = player.playerInventoryManager.weaponsInRightHandSlots[player.playerInventoryManager.rightHandWeaponIndex];
                    //assign the network weapon id so it switches for everyone
                    player.playerInventoryManager.currentRightHandWeapon = selectedWeapon;
                    player.playerNetworkManager.currentRightHandWeaponID.Value = selectedWeapon.itemID;
                    return;
                }
            }

            if(selectedWeapon == null && player.playerInventoryManager.rightHandWeaponIndex <= 2)
            {
                SwitchRightWeapon();
            }
        }

        public void LoadRightWeapon()
        {
            if(player.playerInventoryManager.currentRightHandWeapon != null)
            {
                //remove old weapon model
                rightHandWeaponSlot.UnloadWeapon();

                GameObject weaponModelPrefab = player.playerInventoryManager.currentRightHandWeapon.weaponModel;

                if (weaponModelPrefab == null && WorldItemDatabase.Instance != null && WorldItemDatabase.Instance.unarmedWeapon != null)
                {
                    Debug.LogWarning($"Weapon '{player.playerInventoryManager.currentRightHandWeapon.itemName}' is missing a weaponModel. Falling back to Unarmed model.");
                    weaponModelPrefab = WorldItemDatabase.Instance.unarmedWeapon.weaponModel;
                }

                if (weaponModelPrefab == null)
                {
                    Debug.LogWarning($"Weapon '{player.playerInventoryManager.currentRightHandWeapon.itemName}' could not be loaded because no fallback weaponModel was found.");
                    return;
                }

                //bring new weapon model
                rightHandWeaponModel = Instantiate(weaponModelPrefab);
                rightHandWeaponSlot.PlaceWeaponModelIntoSlot(rightHandWeaponModel);
                rightWeaponManager = rightHandWeaponModel.GetComponent<WeaponManager>();

                if (rightWeaponManager != null)
                    rightWeaponManager.SetWeaponDamage(player, player.playerInventoryManager.currentRightHandWeapon);

                player.playerAnimatorManager.UpdateAnimatorController(player.playerInventoryManager.currentRightHandWeapon.weaponAnimator);
            }

        }

        //left weapon
        public void SwitchLeftWeapon()
        {
            if (!player.IsOwner)
                return;

            player.playerNetworkManager.isTwoHandingWeapon.Value = false;

            player.playerAnimatorManager.PlayTargetActionAnimation("Swap_Left_Weapon_01", false, false, true, true);

            WeaponItem selectedWeapon = null;

            player.playerInventoryManager.leftHandWeaponIndex += 1;

            if (player.playerInventoryManager.leftHandWeaponIndex < 0 || player.playerInventoryManager.leftHandWeaponIndex > 2)
            {
                player.playerInventoryManager.leftHandWeaponIndex = 0;

                //we check if we are holding more than one weapon
                float weaponCount = 0;
                WeaponItem firstWeapon = null;
                int firstWeaponPosition = 0;

                for (int i = 0; i < player.playerInventoryManager.weaponsInLeftHandSlots.Length; i++)
                {
                    if (player.playerInventoryManager.weaponsInLeftHandSlots[i].itemID != WorldItemDatabase.Instance.unarmedWeapon.itemID)
                    {
                        weaponCount += 1;
                        if (firstWeapon == null)
                        {
                            firstWeapon = player.playerInventoryManager.weaponsInLeftHandSlots[i];
                            firstWeaponPosition = i;
                        }
                    }
                }

                if (weaponCount <= 1)
                {
                    //if we are only holding one weapon, we just equip it
                    player.playerInventoryManager.leftHandWeaponIndex = -1;
                    selectedWeapon = WorldItemDatabase.Instance.unarmedWeapon;
                    player.playerInventoryManager.currentLeftHandWeapon = selectedWeapon;
                    player.playerNetworkManager.currentLeftHandWeaponID.Value = selectedWeapon.itemID;
                }
                else
                {
                    player.playerInventoryManager.leftHandWeaponIndex = firstWeaponPosition;
                    player.playerInventoryManager.currentLeftHandWeapon = selectedWeapon;
                    player.playerNetworkManager.currentLeftHandWeaponID.Value = firstWeapon.itemID;
                }

                return;
            }

            foreach (WeaponItem weapon in player.playerInventoryManager.weaponsInLeftHandSlots)
            {
                //if the next weapon does not equal the unarmed weapon
                if (player.playerInventoryManager.weaponsInLeftHandSlots[player.playerInventoryManager.leftHandWeaponIndex].itemID != WorldItemDatabase.Instance.unarmedWeapon.itemID)
                {
                    selectedWeapon = player.playerInventoryManager.weaponsInLeftHandSlots[player.playerInventoryManager.leftHandWeaponIndex];
                    //assign the network weapon id so it switches for everyone
                    player.playerInventoryManager.currentLeftHandWeapon = selectedWeapon;
                    player.playerNetworkManager.currentLeftHandWeaponID.Value = selectedWeapon.itemID;
                    return;
                }
            }

            if (selectedWeapon == null && player.playerInventoryManager.leftHandWeaponIndex <= 2)
            {
                SwitchLeftWeapon();
            }
        }

        public void LoadLeftWeapon()
        {
            if(player.playerInventoryManager.currentLeftHandWeapon != null)
            {
                //remove old weapon model
                if (leftHandWeaponSlot.currentWeaponModel != null)
                    leftHandWeaponSlot.UnloadWeapon();

                if (leftHandShieldSlot.currentWeaponModel != null)
                    leftHandShieldSlot.UnloadWeapon();

                GameObject weaponModelPrefab = player.playerInventoryManager.currentLeftHandWeapon.weaponModel;

                if (weaponModelPrefab == null && WorldItemDatabase.Instance != null && WorldItemDatabase.Instance.unarmedWeapon != null)
                {
                    Debug.LogWarning($"Weapon '{player.playerInventoryManager.currentLeftHandWeapon.itemName}' is missing a weaponModel. Falling back to Unarmed model.");
                    weaponModelPrefab = WorldItemDatabase.Instance.unarmedWeapon.weaponModel;
                }

                if (weaponModelPrefab == null)
                {
                    Debug.LogWarning($"Weapon '{player.playerInventoryManager.currentLeftHandWeapon.itemName}' could not be loaded because no fallback weaponModel was found.");
                    return;
                }

                //bring new weapon model
                leftHandWeaponModel = Instantiate(weaponModelPrefab);

                switch (player.playerInventoryManager.currentLeftHandWeapon.weaponModelType)
                {
                    case WeaponModelType.Weapon:
                        leftHandWeaponSlot.PlaceWeaponModelIntoSlot(leftHandWeaponModel);
                        break;
                    case WeaponModelType.Shield:
                        leftHandShieldSlot.PlaceWeaponModelIntoSlot(leftHandWeaponModel);
                        break;
                    default:
                        break;
                }

                leftWeaponManager = leftHandWeaponModel.GetComponent<WeaponManager>();

                if (leftWeaponManager != null)
                    leftWeaponManager.SetWeaponDamage(player, player.playerInventoryManager.currentLeftHandWeapon);
            }
        }

        //two hand
        public void UnTwoHandWeapon()
        {
            player.playerAnimatorManager.UpdateAnimatorController(player.playerInventoryManager.currentRightHandWeapon.weaponAnimator);

            //left hand
            if (player.playerInventoryManager.currentLeftHandWeapon.weaponModelType == WeaponModelType.Weapon)
            {
                leftHandWeaponSlot.PlaceWeaponModelIntoSlot(leftHandWeaponModel);
            }
            else if (player.playerInventoryManager.currentLeftHandWeapon.weaponModelType == WeaponModelType.Shield)
            {
                leftHandShieldSlot.PlaceWeaponModelIntoSlot(leftHandWeaponModel);
            }

            //right hand
            rightHandWeaponSlot.PlaceWeaponModelIntoSlot(rightHandWeaponModel);

            rightWeaponManager.SetWeaponDamage(player, player.playerInventoryManager.currentRightHandWeapon);
            leftWeaponManager.SetWeaponDamage(player, player.playerInventoryManager.currentLeftHandWeapon);
        }

        public void TwoHandRightWeapon()
        {
            if (player.playerInventoryManager.currentRightHandWeapon == WorldItemDatabase.Instance.unarmedWeapon)
            {
                if (player.IsOwner)
                {
                    player.playerNetworkManager.isTwoHandingRightWeapon.Value = false;
                    player.playerNetworkManager.isTwoHandingWeapon.Value = false;
                }

                return;
            }

            player.playerAnimatorManager.UpdateAnimatorController(player.playerInventoryManager.currentRightHandWeapon.weaponAnimator);

            backSlot.PlaceWeaponModelInUnequippedSlot(leftHandWeaponModel, player.playerInventoryManager.currentLeftHandWeapon.weaponClass, player);

            rightHandWeaponSlot.PlaceWeaponModelIntoSlot(rightHandWeaponModel);

            rightWeaponManager.SetWeaponDamage(player, player.playerInventoryManager.currentRightHandWeapon);
            leftWeaponManager.SetWeaponDamage(player, player.playerInventoryManager.currentLeftHandWeapon);
        }

        public void TwoHandLeftWeapon()
        {
            if (player.playerInventoryManager.currentLeftHandWeapon == WorldItemDatabase.Instance.unarmedWeapon)
            {
                if (player.IsOwner)
                {
                    player.playerNetworkManager.isTwoHandingLeftWeapon.Value = false;
                    player.playerNetworkManager.isTwoHandingWeapon.Value = false;
                }
                return;
            }

            player.playerAnimatorManager.UpdateAnimatorController(player.playerInventoryManager.currentLeftHandWeapon.weaponAnimator);
            
            backSlot.PlaceWeaponModelInUnequippedSlot(rightHandWeaponModel, player.playerInventoryManager.currentRightHandWeapon.weaponClass, player);
            
            rightHandWeaponSlot.PlaceWeaponModelIntoSlot(leftHandWeaponModel);

            rightWeaponManager.SetWeaponDamage(player, player.playerInventoryManager.currentRightHandWeapon);
            leftWeaponManager.SetWeaponDamage(player, player.playerInventoryManager.currentLeftHandWeapon);
        }

        //damage colliders

        public void OpenDamageCollider()
        {
            //open right weapon damage collider
            if (player.playerNetworkManager.isUsingRightHand.Value)
            {
                rightWeaponManager.ToggleWeaponTrail(true);
                rightWeaponManager.meleeDamageCollider.EnableDamageCollider();
                player.characterSoundFXManager.PlaySoundFX(WorldSoundFXManager.instance.ChooseRandomSFXFromArray(player.playerInventoryManager.currentRightHandWeapon.whooshes));
            }
            //open left weapon damage collider
            else if (player.playerNetworkManager.isUsingLeftHand.Value)
            {
                leftWeaponManager.ToggleWeaponTrail(true);
                leftWeaponManager.meleeDamageCollider.EnableDamageCollider();
                player.characterSoundFXManager.PlaySoundFX(WorldSoundFXManager.instance.ChooseRandomSFXFromArray(player.playerInventoryManager.currentLeftHandWeapon.whooshes));
            }
        }

        public void CloseDamageCollider()
        {
            //close right weapon damage collider
            if (player.playerNetworkManager.isUsingRightHand.Value)
            {
                rightWeaponManager.meleeDamageCollider.DisableDamageCollider();
                rightWeaponManager.ToggleWeaponTrail(false);
            }
            //close left weapon damage collider
            else if (player.playerNetworkManager.isUsingLeftHand.Value)
            {
                leftWeaponManager.meleeDamageCollider.DisableDamageCollider();
                leftWeaponManager.ToggleWeaponTrail(false);
            }
        }

        public void OpenMainHandDamageCollider()
        {
            rightWeaponManager.ToggleWeaponTrail(true);
            rightWeaponManager.meleeDamageCollider.EnableDamageCollider();
            player.characterSoundFXManager.PlaySoundFX(WorldSoundFXManager.instance.ChooseRandomSFXFromArray(player.playerInventoryManager.currentRightHandWeapon.whooshes));
        }

        public void CloseMainHandDamageCollider()
        {
            rightWeaponManager.ToggleWeaponTrail(false);
            rightWeaponManager.meleeDamageCollider.DisableDamageCollider();
        }

        public void OpenOffHandDamageCollider()
        {
            leftWeaponManager.meleeDamageCollider.EnableDamageCollider();
            player.characterSoundFXManager.PlaySoundFX(WorldSoundFXManager.instance.ChooseRandomSFXFromArray(player.playerInventoryManager.currentLeftHandWeapon.whooshes));
        }

        public void CloseOffHandDamageCollider()
        {
            leftWeaponManager.meleeDamageCollider.DisableDamageCollider();
        }

        //unique weapons
        public void UnHideWeapons()
        {
            if (player.playerEquipmentManager.rightHandWeaponModel != null)
                player.playerEquipmentManager.rightHandWeaponModel.SetActive(true);

            if (player.playerEquipmentManager.leftHandWeaponModel != null)
                player.playerEquipmentManager.leftHandWeaponModel.SetActive(true);
        }
    }
}
