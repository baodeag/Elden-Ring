using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.InputManagerEntry;
using TMPro;
using UnityEngine.EventSystems;

namespace baodeag { 
    public class TitleScreenManager : MonoBehaviour
    {
        public static TitleScreenManager Instance;
        private const string DefaultNetworkAddress = "127.0.0.1:7777";

        //main menu
        [Header("Main Menu Menus")]
        [SerializeField] GameObject titleScreenMainMenu;
        [SerializeField] GameObject titleScreenLoadMenu;
        [SerializeField] GameObject titleScreenCharacterCreationMenu;
        [SerializeField] GameObject titleScreenSettingsMenu;
        [SerializeField] GameObject titleScreenBanner;

        [Header("Main Menu Buttons")]
        [SerializeField] Button loadMenuReturnButton;
        [SerializeField] Button mainMenuLoadGameButton;
        [SerializeField] Button mainMenuNewGameButton;
        [SerializeField] Button mainMenuSettingsButton;
        [SerializeField] Button deleteCharacterPopUpConfirmButton;
        [SerializeField] Button hostWorldButton;
        [SerializeField] Button joinWorldButton;
        [SerializeField] TMP_InputField networkAddressInputField;

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
        private bool networkMenuInitialized;
        private TitleScreenSettingsMenuView settingsMenuView;


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

        private void Start()
        {
            HideLegacyNetworkControls();
            EnsureSettingsMenu();
        }

        private void Update()
        {
            if (titleScreenSettingsMenu != null && titleScreenSettingsMenu.activeSelf && Input.GetKeyDown(KeyCode.Escape))
                CloseSettingsMenu();
        }

        public void StartNetworkAsHost()
        {
            HostWorld();
        }

        public void PressStart()
        {
            OpenTitleScreenMainMenu();

            if (EventSystem.current != null && EventSystem.current.currentSelectedGameObject != null)
            {
                EventSystem.current.currentSelectedGameObject.SetActive(false);
            }

            mainMenuNewGameButton.Select();
        }

        public void JoinOnlineGame()
        {
            Debug.Log("Join World has moved to the in-game character menu.");
        }

        public void HostWorld()
        {
            if (!WorldGameSessionManager.instance.StartGameAsHost())
                return;

            if (networkAddressInputField != null)
            {
                networkAddressInputField.text = WorldGameSessionManager.instance.GetCurrentConnectionAddress();
            }

            mainMenuNewGameButton.Select();
        }

        public void JoinWorld()
        {
            string addressInput = networkAddressInputField != null && !string.IsNullOrWhiteSpace(networkAddressInputField.text)
                ? networkAddressInputField.text
                : DefaultNetworkAddress;

            WorldGameSessionManager.instance.StartGameAsClient(addressInput);
        }

        private void HideLegacyNetworkControls()
        {
            if (hostWorldButton != null)
                hostWorldButton.gameObject.SetActive(false);

            if (joinWorldButton != null)
                joinWorldButton.gameObject.SetActive(false);

            if (networkAddressInputField != null)
                networkAddressInputField.gameObject.SetActive(false);

            if (titleScreenMainMenu != null)
            {
                Transform[] legacyControls = titleScreenMainMenu.GetComponentsInChildren<Transform>(true);

                for (int i = 0; i < legacyControls.Length; i++)
                {
                    string objectName = legacyControls[i].name;

                    if (objectName == "Join World Button" ||
                        objectName == "Host World Button" ||
                        objectName == "Network Address Input")
                    {
                        legacyControls[i].gameObject.SetActive(false);
                    }
                }
            }
        }

        private void EnsureSettingsMenu()
        {
            if (titleScreenSettingsMenu == null)
                return;

            if (titleScreenBanner == null && titleScreenMainMenu != null)
            {
                Transform bannerTransform = titleScreenMainMenu.transform.Find("Title Screen Banner");

                if (bannerTransform != null)
                    titleScreenBanner = bannerTransform.gameObject;
            }

            settingsMenuView = titleScreenSettingsMenu.GetComponent<TitleScreenSettingsMenuView>();

            if (settingsMenuView != null)
                settingsMenuView.Initialize(this);
        }

        public void OpenSettingsMenu()
        {
            if (titleScreenSettingsMenu == null)
                return;

            CloseTitleScreenMainMenu();
            titleScreenLoadMenu.SetActive(false);
            titleScreenCharacterCreationMenu.SetActive(false);
            SetBannerActive(false);
            titleScreenSettingsMenu.SetActive(true);

            if (settingsMenuView == null)
                EnsureSettingsMenu();

            if (settingsMenuView != null)
                settingsMenuView.Refresh();
        }

        public void CloseSettingsMenu()
        {
            if (titleScreenSettingsMenu != null)
                titleScreenSettingsMenu.SetActive(false);

            SetBannerActive(true);
            OpenTitleScreenMainMenu();

            if (mainMenuSettingsButton != null)
                mainMenuSettingsButton.Select();
        }

        public void AttemptToCreateNewCharacter()
        {
            if (!EnsureHostSessionForSaveMenus())
                return;

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
            if (!EnsureHostSessionForSaveMenus())
                return;

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

        private void SetBannerActive(bool isActive)
        {
            if (titleScreenBanner != null)
                titleScreenBanner.SetActive(isActive);
        }

        public void OpenCharacterCreationMenu()
        {
            CloseTitleScreenMainMenu();

            titleScreenCharacterCreationMenu.SetActive(true);

            PlayerManager player = NetworkManager.Singleton.LocalClient.PlayerObject.GetComponent<PlayerManager>();

            //set default body type
            player.playerBodyManager.ToggleBodyType(true);
        }

        private bool EnsureHostSessionForSaveMenus()
        {
            if (NetworkManager.Singleton.IsHost)
                return true;

            if (NetworkManager.Singleton.IsClient)
            {
                Debug.LogWarning("New Game and Load Game are only available for the host session.");
                return false;
            }

            return WorldGameSessionManager.instance.StartGameAsHost();
        }

        private void BuildNetworkMenuControls()
        {
            if (networkMenuInitialized || titleScreenMainMenu == null || mainMenuNewGameButton == null)
                return;

            RectTransform menuParent = mainMenuNewGameButton.transform.parent as RectTransform;

            if (menuParent == null)
                return;

            SetupJoinButton(menuParent);
            SetupHostButton(menuParent);
            SetupNetworkAddressInput(menuParent);
            PopulateDefaultNetworkAddressField();

            networkMenuInitialized = true;
        }

        private void SetupHostButton(RectTransform menuParent)
        {
            if (hostWorldButton != null)
            {
                ConfigureButton(hostWorldButton, "Host World Button", "HOST", HostWorld);
                return;
            }

            GameObject hostButtonObject = Instantiate(mainMenuNewGameButton.gameObject, menuParent);
            hostButtonObject.name = "Host World Button";
            hostButtonObject.SetActive(true);
            hostWorldButton = hostButtonObject.GetComponent<Button>();

            RectTransform hostRect = hostButtonObject.GetComponent<RectTransform>();
            RectTransform newGameRect = mainMenuNewGameButton.GetComponent<RectTransform>();

            hostRect.anchoredPosition = new Vector2(newGameRect.anchoredPosition.x, newGameRect.anchoredPosition.y + 90f);

            ConfigureButton(hostWorldButton, "Host World Button", "HOST", HostWorld);
        }

        private void SetupJoinButton(RectTransform menuParent)
        {
            if (joinWorldButton == null)
            {
                foreach (Button button in menuParent.GetComponentsInChildren<Button>(true))
                {
                    if (button != null && button.name == "Join World Button")
                    {
                        joinWorldButton = button;
                        break;
                    }
                }
            }

            if (joinWorldButton == null)
            {
                GameObject joinButtonObject = Instantiate(mainMenuLoadGameButton.gameObject, menuParent);
                joinButtonObject.name = "Join World Button";
                joinButtonObject.SetActive(true);
                joinWorldButton = joinButtonObject.GetComponent<Button>();
            }

            RectTransform joinRect = joinWorldButton.GetComponent<RectTransform>();
            RectTransform loadRect = mainMenuLoadGameButton.GetComponent<RectTransform>();
            joinRect.anchoredPosition = new Vector2(loadRect.anchoredPosition.x, loadRect.anchoredPosition.y + 90f);

            ConfigureButton(joinWorldButton, "Join World Button", "JOIN", JoinWorld);
        }

        private void SetupNetworkAddressInput(RectTransform menuParent)
        {
            if (networkAddressInputField == null && characterNameInputField != null)
            {
                GameObject addressInputObject = Instantiate(characterNameInputField.gameObject, menuParent);
                addressInputObject.name = "Network Address Input";
                addressInputObject.SetActive(true);
                networkAddressInputField = addressInputObject.GetComponent<TMP_InputField>();
            }

            if (networkAddressInputField == null)
                return;

            RectTransform addressRect = networkAddressInputField.GetComponent<RectTransform>();
            RectTransform hostRect = hostWorldButton != null
                ? hostWorldButton.GetComponent<RectTransform>()
                : mainMenuNewGameButton.GetComponent<RectTransform>();

            addressRect.anchoredPosition = new Vector2(hostRect.anchoredPosition.x, hostRect.anchoredPosition.y + 90f);
            addressRect.sizeDelta = new Vector2(420f, addressRect.sizeDelta.y);

            networkAddressInputField.contentType = TMP_InputField.ContentType.Standard;
            networkAddressInputField.lineType = TMP_InputField.LineType.SingleLine;
            networkAddressInputField.SetTextWithoutNotify(DefaultNetworkAddress);

            if (networkAddressInputField.textComponent != null)
            {
                networkAddressInputField.textComponent.text = DefaultNetworkAddress;
            }

            if (networkAddressInputField.placeholder is TMP_Text placeholderText)
            {
                placeholderText.text = DefaultNetworkAddress;
            }
        }

        private void ConfigureButton(Button button, string objectName, string label, UnityEngine.Events.UnityAction callback)
        {
            if (button == null)
                return;

            button.name = objectName;
            button.onClick.RemoveAllListeners();
            button.onClick.AddListener(callback);

            TextMeshProUGUI buttonText = button.GetComponentInChildren<TextMeshProUGUI>(true);

            if (buttonText != null)
            {
                buttonText.text = label;
            }
        }

        private void PopulateDefaultNetworkAddressField()
        {
            if (networkAddressInputField == null || WorldGameSessionManager.instance == null)
                return;

            networkAddressInputField.SetTextWithoutNotify(DefaultNetworkAddress);

            if (networkAddressInputField.textComponent != null)
            {
                networkAddressInputField.textComponent.text = DefaultNetworkAddress;
            }
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
            player.playerInventoryManager.weaponsInRightHandSlots[0] = Instantiate(mainHandWeapons[0]);
            player.playerInventoryManager.weaponsInRightHandSlots[1] = Instantiate(mainHandWeapons[1]);
            player.playerInventoryManager.weaponsInRightHandSlots[2] = Instantiate(mainHandWeapons[2]);
            player.playerInventoryManager.currentRightHandWeapon = player.playerInventoryManager.weaponsInRightHandSlots[0];
            player.playerNetworkManager.currentRightHandWeaponID.Value = player.playerInventoryManager.weaponsInRightHandSlots[0].itemID;

            player.playerInventoryManager.weaponsInLeftHandSlots[0] = Instantiate(offHandWeapons[0]);
            player.playerInventoryManager.weaponsInLeftHandSlots[1] = Instantiate(offHandWeapons[1]);
            player.playerInventoryManager.weaponsInLeftHandSlots[2] = Instantiate(offHandWeapons[2]);
            player.playerInventoryManager.currentLeftHandWeapon = player.playerInventoryManager.weaponsInLeftHandSlots[0];
            player.playerNetworkManager.currentLeftHandWeaponID.Value = player.playerInventoryManager.weaponsInLeftHandSlots[0].itemID;

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
