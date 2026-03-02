using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.InputManagerEntry;
using TMPro;

namespace baodeag { 
    public class TitleScreenManager : MonoBehaviour
    {
        public static TitleScreenManager Instance;

        //main menu
        [Header("Main Menu Menus")]
        [SerializeField] GameObject titleScreenMainMenu;
        [SerializeField] GameObject titleScreenLoadMenu;
        [SerializeField] GameObject titleScreenCharacterCreationMenu;

        [Header("Main Menu Buttons")]
        [SerializeField] Button loadMenuReturnButton;
        [SerializeField] Button mainMenuLoadGameButton;
        [SerializeField] Button mainMenuNewGameButton;
        [SerializeField] Button deleteCharacterPopUpConfirmButton;

        [Header("Main Menu Pop Ups")]
        [SerializeField] GameObject noCharacterSlotsPopUp;
        [SerializeField] Button noCharacterSlotsOkayButton;
        [SerializeField] GameObject deleteCharacterSlotPopup;

        //character creation menu
        [Header("Character Creation Main Panel Buttons")]
        [SerializeField] Button characterNameButton;
        [SerializeField] Button characterClassButton;
        [SerializeField] Button characterHairButton;
        [SerializeField] Button characterHairColorButton;
        [SerializeField] Button characterSexButton;
        [SerializeField] TextMeshProUGUI characterSexText;
        [SerializeField] Button startGameButton;

        [Header("Character Creation Class Panel Buttons")]
        [SerializeField] Button[] characterClassButtons;
        [SerializeField] Button[] characterHairButtons;
        [SerializeField] Button[] characterHairColorButtons;

        [Header("Character Creation Secondary Panel Menus")]
        [SerializeField] GameObject characterClassMenu;
        [SerializeField] GameObject characterHairMenu;
        [SerializeField] GameObject characterHairColorMenu;
        [SerializeField] GameObject characterNameMenu;
        [SerializeField] TMP_InputField characterNameInputField;

        [Header("Color Sliders")]
        [SerializeField] Slider redSlider;
        [SerializeField] Slider greenSlider;
        [SerializeField] Slider blueSlider;

        [Header("Hidden Gear")]
        private HeadEquipmentItem hiddenHelmet;

        [Header("Character Slots")]
        public CharacterSlot currentSelectedSlot = CharacterSlot.NO_SLOT;

        [Header("Classes")]
        public CharacterClass[] startingClasses;


        private void Awake()
        {
            if(Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
        }

        public void StartNetworkAsHost()
        {
            WorldGameSessionManager.instance.StartGameAsHost();
        }

        public void AttemptToCreateNewCharacter()
        {
            if (WorldSaveGameManager.instance.HasFreeCharacterSlot())
            {
                OpenCharacterCreationMenu();
            }
            else
            {
                //if there are no available slots, notify the player
                DisplayNoFreeCharacterSlotPopUp();
            }
        }

        public void StartNewGame()
        {
            WorldSaveGameManager.instance.AttemptToCreateNewGame();
        }

        public void OpenLoadGameMenu()
        {
            //close main menu
            titleScreenMainMenu.SetActive(false);

            //open load menu
            titleScreenLoadMenu.SetActive(true);

            //select the return button first
            loadMenuReturnButton.Select();
        }

        public void CloseLoadGameMenu()
        {
            //close load menu
            titleScreenLoadMenu.SetActive(false);

            //open main menu
            titleScreenMainMenu.SetActive(true);

            //select the load button
            mainMenuLoadGameButton.Select();
        }

        public void ToggleBodyType()
        {
            PlayerManager player = NetworkManager.Singleton.LocalClient.PlayerObject.GetComponent<PlayerManager>();

            player.playerNetworkManager.isMale.Value = !player.playerNetworkManager.isMale.Value;

            if (player.playerNetworkManager.isMale.Value)
            {
                characterSexText.text = "MALE";
            }
            else
            {
                characterSexText.text = "FEMALE";
            }
        }

        public void OpenTitleScreenMainMenu()
        {
            titleScreenMainMenu.SetActive(true);
        }

        public void CloseTitleScreenMainMenu()
        {
            titleScreenMainMenu.SetActive(false);
        }

        public void OpenCharacterCreationMenu()
        {
            CloseTitleScreenMainMenu();

            titleScreenCharacterCreationMenu.SetActive(true);

            PlayerManager player = NetworkManager.Singleton.LocalClient.PlayerObject.GetComponent<PlayerManager>();

            //set default body type
            player.playerBodyManager.ToggleBodyType(true);
        }

        public void CloseCharacterCreationMenu()
        {
            titleScreenCharacterCreationMenu.SetActive(false);

            OpenTitleScreenMainMenu();
        }

        public void OpenChooseCharacterClassSubMenu()
        {
            ToggleCharacterCreationScreenMainMenuButtons(false);

            characterClassMenu.SetActive(true);

            if (characterClassButtons.Length > 0)
            {
                characterClassButtons[0].Select();
                characterClassButtons[0].OnSelect(null);
            }
        }

        public void CloseChooseCharacterClassSubMenu()
        {
            ToggleCharacterCreationScreenMainMenuButtons(true);

            characterClassMenu.SetActive(false);

            characterClassButton.Select();
            characterClassButton.OnSelect(null);
        }

        public void OpenChooseHairStyleSubMenu()
        {
            PlayerManager player = NetworkManager.Singleton.LocalClient.PlayerObject.GetComponent<PlayerManager>();

            ToggleCharacterCreationScreenMainMenuButtons(false);

            characterHairMenu.SetActive(true);

            if (characterHairButtons.Length > 0)
            {
                characterHairButtons[0].Select();
                characterHairButtons[0].OnSelect(null);
            }

            //store the helmet the player had on
            if (player.playerInventoryManager.headEquipment != null)
                hiddenHelmet = Instantiate(player.playerInventoryManager.headEquipment);

            //unequip the helmet and reload the gear
            player.playerInventoryManager.headEquipment = null;
            player.playerEquipmentManager.EquipArmor();
        }

        public void CloseChooseHairStyleSubMenu()
        {
            PlayerManager player = NetworkManager.Singleton.LocalClient.PlayerObject.GetComponent<PlayerManager>();

            ToggleCharacterCreationScreenMainMenuButtons(true);
            characterHairMenu.SetActive(false);
            characterHairButton.Select();
            characterHairButton.OnSelect(null);

            if (hiddenHelmet != null)
                player.playerInventoryManager.headEquipment = hiddenHelmet;

            player.playerEquipmentManager.EquipArmor();
        }

        public void OpenChooseHairColorSubMenu()
        {
            PlayerManager player = NetworkManager.Singleton.LocalClient.PlayerObject.GetComponent<PlayerManager>();

            ToggleCharacterCreationScreenMainMenuButtons(false);

            characterHairColorMenu.SetActive(true);

            if (characterHairColorButtons.Length > 0)
            {
                characterHairColorButtons[0].Select();
                characterHairColorButtons[0].OnSelect(null);
            }

            //store the helmet the player had on
            if (player.playerInventoryManager.headEquipment != null)
                hiddenHelmet = Instantiate(player.playerInventoryManager.headEquipment);

            //unequip the helmet and reload the gear
            player.playerInventoryManager.headEquipment = null;
            player.playerEquipmentManager.EquipArmor();
        }

        public void CloseChooseHairColorSubMenu()
        {
            PlayerManager player = NetworkManager.Singleton.LocalClient.PlayerObject.GetComponent<PlayerManager>();

            ToggleCharacterCreationScreenMainMenuButtons(true);
            characterHairColorMenu.SetActive(false);
            characterHairColorButton.Select();
            characterHairColorButton.OnSelect(null);

            if (hiddenHelmet != null)
                player.playerInventoryManager.headEquipment = hiddenHelmet;

            player.playerEquipmentManager.EquipArmor();
        }

        public void OpenChooseNameSubMenu()
        {
            PlayerManager player = NetworkManager.Singleton.LocalClient.PlayerObject.GetComponent<PlayerManager>();

            ToggleCharacterCreationScreenMainMenuButtons(false);

            characterNameButton.gameObject.SetActive(false);
            characterNameMenu.SetActive(true);
            characterNameInputField.Select();
        }

        public void CloseChooseNameSubMenu()
        {
            PlayerManager player = NetworkManager.Singleton.LocalClient.PlayerObject.GetComponent<PlayerManager>();

            ToggleCharacterCreationScreenMainMenuButtons(true);

            characterNameMenu.SetActive(false);
            characterNameButton.gameObject.SetActive(true);
            characterNameButton.Select();

            player.playerNetworkManager.characterName.Value = characterNameInputField.text;
        }

        private void ToggleCharacterCreationScreenMainMenuButtons(bool status)
        {
            characterNameButton.enabled = status;
            characterClassButton.enabled = status;
            characterHairButton.enabled = status;
            characterHairColorButton.enabled = status;
            characterSexButton.enabled = status;
            startGameButton.enabled = status;
        }

        public void DisplayNoFreeCharacterSlotPopUp()
        {
            noCharacterSlotsPopUp.SetActive(true);
            noCharacterSlotsOkayButton.Select();
        }

        public void CloseNoFreeCharacterSlotPopUp()
        {
            noCharacterSlotsPopUp.SetActive(false);
            mainMenuNewGameButton.Select();
        }

        public void SelectCharacterSlot(CharacterSlot characterSlot)
        {
            currentSelectedSlot = characterSlot;
        }

        public void SelectNoSlot()
        {
            currentSelectedSlot = CharacterSlot.NO_SLOT;
        }

        public void AttemptToDeleteCharacterSlot()
        {
            if(currentSelectedSlot != CharacterSlot.NO_SLOT)
            {
                deleteCharacterSlotPopup.SetActive(true);
                deleteCharacterPopUpConfirmButton.Select();
            }
        }

        public void DeleteCharacterSlot()
        {
            deleteCharacterSlotPopup.SetActive(false);
            WorldSaveGameManager.instance.DeleteGame(currentSelectedSlot);

            //we disable and then enable the load menu, to refresh the slots will now become active
            titleScreenLoadMenu.SetActive(false);
            titleScreenLoadMenu.SetActive(true);

            loadMenuReturnButton.Select();
        }

        public void CloseDeleteCharacterPopUp()
        {
            deleteCharacterSlotPopup.SetActive(false);
            loadMenuReturnButton.Select();
        }

        //character class

        public void SelectClass(int classID)
        {
            PlayerManager player = NetworkManager.Singleton.LocalClient.PlayerObject.GetComponent<PlayerManager>();

            if (startingClasses.Length <= 0)
                return;

            startingClasses[classID].SetClass(player);
            CloseChooseCharacterClassSubMenu();
        }

        public void PreviewClass(int classID)
        {
            PlayerManager player = NetworkManager.Singleton.LocalClient.PlayerObject.GetComponent<PlayerManager>();

            if (startingClasses.Length <= 0)
                return;

            startingClasses[classID].SetClass(player);
        }

        public void SetCharacterClass(PlayerManager player, int vitality, int endurance, int mind, int strength, int dexterity, int intelligence, int faith,
            WeaponItem[] mainHandWeapons, WeaponItem[] offHandWeapons, 
            HeadEquipmentItem headEquipment, BodyEquipmentItem bodyEquipment, LegEquipmentItem legEquipment, HandEquipmentItem handEquipment,
            QuickSlotItem[] quickSlotItems)
        {
            // clear the hidden helmet
            hiddenHelmet = null;

            //set the stats
            player.playerNetworkManager.vigor.Value = vitality;
            player.playerNetworkManager.endurance.Value = endurance;
            player.playerNetworkManager.mind.Value = mind;
            player.playerNetworkManager.strength.Value = strength;
            player.playerNetworkManager.dexterity.Value = dexterity;
            player.playerNetworkManager.intelligence.Value = intelligence;
            player.playerNetworkManager.faith.Value = faith;

            //set the weapons
            player.playerInventoryManager.weaponInRightHandSlots[0] = Instantiate(mainHandWeapons[0]);
            player.playerInventoryManager.weaponInRightHandSlots[1] = Instantiate(mainHandWeapons[1]);
            player.playerInventoryManager.weaponInRightHandSlots[2] = Instantiate(mainHandWeapons[2]);
            player.playerInventoryManager.currentRightHandWeapon = player.playerInventoryManager.weaponInRightHandSlots[0];
            player.playerNetworkManager.currentRightHandWeaponID.Value = player.playerInventoryManager.weaponInRightHandSlots[0].itemID;

            player.playerInventoryManager.weaponInLeftHandSlots[0] = Instantiate(offHandWeapons[0]);
            player.playerInventoryManager.weaponInLeftHandSlots[1] = Instantiate(offHandWeapons[1]);
            player.playerInventoryManager.weaponInLeftHandSlots[2] = Instantiate(offHandWeapons[2]);
            player.playerInventoryManager.currentLeftHandWeapon = player.playerInventoryManager.weaponInLeftHandSlots[0];
            player.playerNetworkManager.currentLeftHandWeaponID.Value = player.playerInventoryManager.weaponInLeftHandSlots[0].itemID;

            //set the armor
            //head equipment
            if (headEquipment != null)
            {
                HeadEquipmentItem equipment = Instantiate(headEquipment);
                player.playerInventoryManager.headEquipment = equipment;
            }
            else
            {
                player.playerInventoryManager.headEquipment = null;
            }

            //body equipment
            if (bodyEquipment != null)
            {
                BodyEquipmentItem equipment = Instantiate(bodyEquipment);
                player.playerInventoryManager.bodyEquipment = equipment;
            }
            else
            {
                player.playerInventoryManager.bodyEquipment = null;
            }

            //leg equipment
            if (legEquipment != null)
            {
                LegEquipmentItem equipment = Instantiate(legEquipment);
                player.playerInventoryManager.legEquipment = equipment;
            }
            else
            {
                player.playerInventoryManager.legEquipment = null;
            }

            //hand equipment
            if (handEquipment != null)
            {
                HandEquipmentItem equipment = Instantiate(handEquipment);
                player.playerInventoryManager.handEquipment = equipment;
            }
            else
            {
                player.playerInventoryManager.handEquipment = null;
            }

            player.playerEquipmentManager.EquipArmor();

            //set the quick slot items
            player.playerInventoryManager.quickSlotItemIndex = 0;

            if (quickSlotItems[0] != null)
                player.playerInventoryManager.quickSlotItemsInQuickSlots[0] = Instantiate(quickSlotItems[0]);

            if (quickSlotItems[1] != null)
                player.playerInventoryManager.quickSlotItemsInQuickSlots[1] = Instantiate(quickSlotItems[1]);

            if (quickSlotItems[2] != null)
                player.playerInventoryManager.quickSlotItemsInQuickSlots[2] = Instantiate(quickSlotItems[2]);

            player.playerEquipmentManager.LoadQuickSlotEquipment(player.playerInventoryManager.quickSlotItemsInQuickSlots[player.playerInventoryManager.quickSlotItemIndex]);
        }

        //character hair

        public void SelectHair(int hairID)
        {
            PlayerManager player = NetworkManager.Singleton.LocalClient.PlayerObject.GetComponent<PlayerManager>();

            player.playerNetworkManager.hairStyleID.Value = hairID;

            CloseChooseHairStyleSubMenu();
        }

        public void PreviewHair(int hairID)
        {
            PlayerManager player = NetworkManager.Singleton.LocalClient.PlayerObject.GetComponent<PlayerManager>();

            player.playerNetworkManager.hairStyleID.Value = hairID;
        }

        public void SelectHairColor()
        {
            PlayerManager player = NetworkManager.Singleton.LocalClient.PlayerObject.GetComponent<PlayerManager>();

            player.playerNetworkManager.hairColorRed.Value = redSlider.value;
            player.playerNetworkManager.hairColorGreen.Value = greenSlider.value;
            player.playerNetworkManager.hairColorBlue.Value = blueSlider.value;

            CloseChooseHairColorSubMenu();
        }

        public void PreviewHairColor()
        {
            PlayerManager player = NetworkManager.Singleton.LocalClient.PlayerObject.GetComponent<PlayerManager>();

            player.playerNetworkManager.hairColorRed.Value = redSlider.value;
            player.playerNetworkManager.hairColorGreen.Value = greenSlider.value;
            player.playerNetworkManager.hairColorBlue.Value = blueSlider.value;
        }

        public void SetRedColorSlider(float redValue)
        {
            redSlider.value = redValue;
        }

        public void SetGreenColorSlider(float greenValue)
        {
            greenSlider.value = greenValue;
        }

        public void SetBlueColorSlider(float blueValue)
        {
            blueSlider.value = blueValue;
        }
    }
}