using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.InputManagerEntry;
using TMPro;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace baodeag { 
    public class TitleScreenManager : MonoBehaviour
    {
        public static TitleScreenManager Instance;
        private const string DefaultNetworkAddress = "127.0.0.1:7777";

        //main menu
        [Header("Main Menu Menus")]
        [SerializeField] GameObject titleScreenMainMenu;
        [SerializeField] GameObject titleScreenModeSelectionMenu;
        [SerializeField] GameObject titleScreenLoadMenu;
        [SerializeField] GameObject titleScreenCharacterCreationMenu;
        [SerializeField] GameObject titleScreenSettingsMenu;
        [SerializeField] GameObject titleScreenBanner;

        [Header("Main Menu Buttons")]
        [SerializeField] Button loadMenuReturnButton;
        [SerializeField] Button mainMenuLoadGameButton;
        [SerializeField] Button mainMenuNewGameButton;
        [SerializeField] Button mainMenuSettingsButton;
        [SerializeField] Button singleplayerModeButton;
        [SerializeField] Button multiplayerModeButton;
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
        [SerializeField] private int selectedStartingClassID = -1;
        private bool networkMenuInitialized;
        private TitleScreenSettingsMenuView settingsMenuView;
        private TextMeshProUGUI characterClassButtonLabel;

        [Header("Character Class Review UI")]
        [SerializeField] private GameObject classReviewPanel;
        [SerializeField] private TextMeshProUGUI classReviewTitleText;
        [SerializeField] private TextMeshProUGUI classReviewSubtitleText;
        [SerializeField] private GameObject classReviewStatsInfoPanel;
        [SerializeField] private GameObject classReviewItemsInfoPanel;
        [SerializeField] private TextMeshProUGUI classReviewStatsInfoText;
        [SerializeField] private TextMeshProUGUI classReviewItemsInfoText;
        [SerializeField] private TextMeshProUGUI classReviewStatsText;
        [SerializeField] private TextMeshProUGUI classReviewLoadoutText;
        [SerializeField] private TextMeshProUGUI classReviewHintText;
        [SerializeField] private Button classReviewStatsTabButton;
        [SerializeField] private Button classReviewItemsTabButton;

        private bool launchModeMenuInitialized;
        private enum ClassReviewTab
        {
            Stats,
            Items
        }

        private ClassReviewTab currentClassReviewTab = ClassReviewTab.Stats;
        private string currentClassReviewStatsContent = string.Empty;
        private string currentClassReviewItemsContent = string.Empty;

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
            EnsureCharacterClassSelectionUI();
            EnsureLaunchModeMenu();
            RefreshSaveMenuButtons();
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
            CloseTitleScreenMainMenu();
            CloseLoadGameMenuIfOpen();
            CloseCharacterCreationMenuIfOpen();
            OpenLaunchModeMenu();

            if (EventSystem.current != null && EventSystem.current.currentSelectedGameObject != null)
            {
                EventSystem.current.currentSelectedGameObject.SetActive(false);
            }

            if (singleplayerModeButton != null)
                singleplayerModeButton.Select();
        }

        public void SelectSingleplayerMode()
        {
            SetLaunchMode(SessionLaunchMode.Singleplayer);
            CloseLaunchModeMenu();
            OpenTitleScreenMainMenu();

            if (mainMenuNewGameButton != null)
                mainMenuNewGameButton.Select();
        }

        public void SelectMultiplayerMode()
        {
            SetLaunchMode(SessionLaunchMode.Multiplayer);
            CloseLaunchModeMenu();
            OpenTitleScreenMainMenu();

            if (mainMenuNewGameButton != null)
                mainMenuNewGameButton.Select();
        }

        public void JoinOnlineGame()
        {
            Debug.Log("Join World has moved to the in-game character menu.");
        }

        public async void HostWorld()
        {
            if (!await WorldGameSessionManager.instance.StartGameAsRelayHostAsync())
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

            CloseLaunchModeMenu();
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

        public async void AttemptToCreateNewCharacter()
        {
            if (!await EnsureHostSessionForSaveMenus())
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
            selectedStartingClassID = GetSelectedStartingClassID();
            WorldSaveGameManager.instance.AttemptToCreateNewGame();
        }

        public async void OpenLoadGameMenu()
        {
            if (!await EnsureHostSessionForSaveMenus())
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

            RefreshSaveMenuButtons();

            //select the appropriate button
            if (mainMenuLoadGameButton != null && mainMenuLoadGameButton.gameObject.activeSelf)
            {
                mainMenuLoadGameButton.Select();
            }
            else if (mainMenuNewGameButton != null)
            {
                mainMenuNewGameButton.Select();
            }
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
            CloseLaunchModeMenu();
            titleScreenMainMenu.SetActive(true);
            RefreshSaveMenuButtons();
        }

        public void CloseTitleScreenMainMenu()
        {
            titleScreenMainMenu.SetActive(false);
        }

        public void OpenLaunchModeMenu()
        {
            EnsureLaunchModeMenu();

            if (titleScreenModeSelectionMenu == null)
                return;

            SetBannerActive(true);
            titleScreenModeSelectionMenu.SetActive(true);
        }

        public void CloseLaunchModeMenu()
        {
            if (titleScreenModeSelectionMenu != null)
                titleScreenModeSelectionMenu.SetActive(false);
        }

        private void EnsureLaunchModeMenu()
        {
            if (launchModeMenuInitialized)
                return;

            if (titleScreenModeSelectionMenu == null ||
                singleplayerModeButton == null ||
                multiplayerModeButton == null)
                return;

            if (titleScreenModeSelectionMenu != null)
                titleScreenModeSelectionMenu.SetActive(false);

            launchModeMenuInitialized = titleScreenModeSelectionMenu != null &&
                                        singleplayerModeButton != null &&
                                        multiplayerModeButton != null;
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
            EnsureCharacterClassSelectionUI();

            PlayerManager player = NetworkManager.Singleton.LocalClient.PlayerObject.GetComponent<PlayerManager>();

            //set default body type
            player.playerBodyManager.ToggleBodyType(true);

            if (startingClasses != null && startingClasses.Length > 0)
            {
                selectedStartingClassID = GetSelectedStartingClassID();
                PreviewClass(selectedStartingClassID);
            }

            UpdateCharacterClassPrimaryButtonLabel();
        }

        private async Task<bool> EnsureHostSessionForSaveMenus()
        {
            if (NetworkManager.Singleton.IsHost)
                return true;

            if (NetworkManager.Singleton.IsClient)
            {
                Debug.LogWarning("New Game and Load Game are only available for the host session.");
                return false;
            }

            return WorldGameSessionManager.instance.RequiresRelayForCurrentMode()
                ? await WorldGameSessionManager.instance.StartGameAsRelayHostAsync()
                : WorldGameSessionManager.instance.StartGameAsHost();
        }

        private void SetLaunchMode(SessionLaunchMode launchMode)
        {
            if (WorldGameSessionManager.instance != null)
                WorldGameSessionManager.instance.SetLaunchMode(launchMode);

            RefreshSaveMenuButtons();
        }

        private void RefreshSaveMenuButtons()
        {
            if (mainMenuLoadGameButton == null)
                return;

            bool allowLoadGame = WorldGameSessionManager.instance == null ||
                                 !WorldGameSessionManager.instance.RequiresRelayForCurrentMode();

            mainMenuLoadGameButton.gameObject.SetActive(allowLoadGame);
        }

        private void CloseLoadGameMenuIfOpen()
        {
            if (titleScreenLoadMenu != null)
                titleScreenLoadMenu.SetActive(false);
        }

        private void CloseCharacterCreationMenuIfOpen()
        {
            if (titleScreenCharacterCreationMenu != null)
                titleScreenCharacterCreationMenu.SetActive(false);
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
            button.onClick = new Button.ButtonClickedEvent();
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
            EnsureCharacterClassSelectionUI();
            ToggleCharacterCreationScreenMainMenuButtons(false);

            characterClassMenu.SetActive(true);

            int selectedClassID = GetSelectedStartingClassID();

            if (characterClassButtons != null && selectedClassID >= 0 && selectedClassID < characterClassButtons.Length && characterClassButtons[selectedClassID] != null)
            {
                characterClassButtons[selectedClassID].Select();
                PreviewClass(selectedClassID);
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

            selectedStartingClassID = Mathf.Clamp(classID, 0, startingClasses.Length - 1);
            startingClasses[selectedStartingClassID].SetClass(player);
            UpdateClassReviewPanel(selectedStartingClassID, true);
            UpdateCharacterClassPrimaryButtonLabel();
            CloseChooseCharacterClassSubMenu();
        }

        public void PreviewClass(int classID)
        {
            PlayerManager player = NetworkManager.Singleton.LocalClient.PlayerObject.GetComponent<PlayerManager>();

            if (startingClasses.Length <= 0)
                return;

            int clampedClassID = Mathf.Clamp(classID, 0, startingClasses.Length - 1);
            startingClasses[clampedClassID].SetClass(player);
            UpdateClassReviewPanel(clampedClassID, clampedClassID == selectedStartingClassID);
        }

        public int GetSelectedStartingClassID()
        {
            if (startingClasses == null || startingClasses.Length <= 0)
                return -1;

            if (selectedStartingClassID < 0 || selectedStartingClassID >= startingClasses.Length)
                selectedStartingClassID = 0;

            return selectedStartingClassID;
        }

        private void EnsureCharacterClassSelectionUI()
        {
            if (startingClasses == null || startingClasses.Length <= 0 || characterClassMenu == null)
                return;

            if (characterClassButtonLabel == null && characterClassButton != null)
                characterClassButtonLabel = characterClassButton.GetComponentInChildren<TextMeshProUGUI>(true);

            RefreshSerializedCharacterClassButtons();
            UpdateCharacterClassPrimaryButtonLabel();
            UpdateClassReviewPanel(GetSelectedStartingClassID(), true);
        }

        private void RefreshSerializedCharacterClassButtons()
        {
            if (characterClassButtons == null || characterClassButtons.Length <= 0)
                return;

            for (int i = 0; i < startingClasses.Length; i++)
            {
                if (i >= characterClassButtons.Length)
                    break;

                ConfigureSerializedCharacterClassButton(characterClassButtons[i], i);
            }

            for (int i = 0; i < characterClassButtons.Length; i++)
            {
                if (characterClassButtons[i] == null)
                    continue;

                characterClassButtons[i].gameObject.SetActive(i < startingClasses.Length);
            }
        }

        private void ConfigureSerializedCharacterClassButton(Button button, int classID)
        {
            if (button == null)
                return;

            button.gameObject.SetActive(true);

            TextMeshProUGUI buttonText = button.GetComponentInChildren<TextMeshProUGUI>(true);

            if (buttonText != null)
            {
                buttonText.text = GetFormattedClassButtonLabel(startingClasses[classID]);
            }
        }

        private void UpdateClassReviewPanel(int classID, bool isSelectedClass)
        {
            if (startingClasses == null || startingClasses.Length <= 0)
                return;

            if (classReviewTitleText == null)
                return;

            if (classReviewPanel != null && !classReviewPanel.activeSelf)
                classReviewPanel.SetActive(true);

            int clampedClassID = Mathf.Clamp(classID, 0, startingClasses.Length - 1);
            CharacterClass characterClass = startingClasses[clampedClassID];

            classReviewTitleText.text = characterClass.className.ToUpperInvariant();
            classReviewSubtitleText.text = GetClassSubtitle(characterClass);
            currentClassReviewStatsContent = GetFormattedClassStats(characterClass);
            currentClassReviewItemsContent = GetFormattedClassLoadout(characterClass);
            currentClassReviewTab = ClassReviewTab.Stats;

            if (classReviewStatsText != null)
                classReviewStatsText.text = "STATS";

            if (classReviewLoadoutText != null)
                classReviewLoadoutText.text = "ITEMS";

            RefreshClassReviewInfoPanel();
            if (classReviewHintText != null)
            {
                classReviewHintText.text = isSelectedClass
                    ? "Selected. Press START."
                    : "Preview. Click to select.";
            }
        }

        public void ShowClassReviewStats()
        {
            currentClassReviewTab = ClassReviewTab.Stats;
            RefreshClassReviewInfoPanel();
        }

        public void ShowClassReviewItems()
        {
            currentClassReviewTab = ClassReviewTab.Items;
            RefreshClassReviewInfoPanel();
        }

        private void RefreshClassReviewInfoPanel()
        {
            if (classReviewStatsInfoText != null)
                classReviewStatsInfoText.text = currentClassReviewStatsContent;

            if (classReviewItemsInfoText != null)
                classReviewItemsInfoText.text = currentClassReviewItemsContent;

            bool showStats = currentClassReviewTab == ClassReviewTab.Stats;

            if (classReviewStatsInfoPanel != null)
                classReviewStatsInfoPanel.SetActive(showStats);

            if (classReviewItemsInfoPanel != null)
                classReviewItemsInfoPanel.SetActive(!showStats);

            if (showStats && classReviewStatsInfoText != null)
                PrepareClassReviewInfoText(classReviewStatsInfoText);
            else if (!showStats && classReviewItemsInfoText != null)
                PrepareClassReviewInfoText(classReviewItemsInfoText);

            UpdateClassReviewTabVisual(classReviewStatsTabButton, classReviewStatsText, currentClassReviewTab == ClassReviewTab.Stats);
            UpdateClassReviewTabVisual(classReviewItemsTabButton, classReviewLoadoutText, currentClassReviewTab == ClassReviewTab.Items);
        }

        private void PrepareClassReviewInfoText(TextMeshProUGUI infoText)
        {
            infoText.ForceMeshUpdate();

            if (infoText.transform is RectTransform infoRect)
            {
                float preferredHeight = Mathf.Max(infoText.preferredHeight + 16f, 150f);
                infoRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, preferredHeight);
                infoRect.anchoredPosition = Vector2.zero;
                LayoutRebuilder.ForceRebuildLayoutImmediate(infoRect);
            }

            ScrollRect infoScrollRect = infoText.GetComponentInParent<ScrollRect>();
            if (infoScrollRect != null)
                infoScrollRect.verticalNormalizedPosition = 1f;

            Canvas.ForceUpdateCanvases();
        }

        private void UpdateClassReviewTabVisual(Button button, TextMeshProUGUI label, bool isActive)
        {
            if (label != null)
                label.color = isActive ? new Color(1f, 0.85f, 0.35f, 1f) : Color.white;

            if (button?.targetGraphic is Image image)
            {
                image.color = isActive
                    ? new Color(1f, 1f, 1f, 0.16f)
                    : new Color(1f, 1f, 1f, 0.08f);
            }
        }

        private void UpdateCharacterClassPrimaryButtonLabel()
        {
            if (characterClassButton == null)
                return;

            if (startingClasses == null || startingClasses.Length <= 0)
            {
                characterClassButtonLabel.text = "CLASS";
                return;
            }

            int classID = GetSelectedStartingClassID();
            characterClassButtonLabel.text = $"CLASS\n<size=72%>{startingClasses[classID].className.ToUpperInvariant()}</size>";
        }

        private string GetFormattedClassButtonLabel(CharacterClass characterClass)
        {
            return $"{characterClass.className.ToUpperInvariant()}\n<size=70%>{GetClassSubtitle(characterClass).ToUpperInvariant()}</size>";
        }

        private string GetClassSubtitle(CharacterClass characterClass)
        {
            string normalizedClassName = characterClass.className.Trim().ToLowerInvariant();

            switch (normalizedClassName)
            {
                case "knight":
                    return "Balanced melee starter.";
                case "ranger":
                    return "Fast bow skirmisher.";
                case "vanguard":
                    return "Heavy bruiser. High poise.";
                case "mystic":
                    return "Ranged sorcery caster.";
                case "confessor":
                    return "Faith melee hybrid.";
                default:
                    return BuildStatArchetypeSummary(characterClass);
            }
        }

        private string GetClassDescription(CharacterClass characterClass)
        {
            string normalizedClassName = characterClass.className.Trim().ToLowerInvariant();

            switch (normalizedClassName)
            {
                case "knight":
                    return "Safe all-round melee.";
                case "ranger":
                    return "Kite and pick targets off.";
                case "vanguard":
                    return "Trades hits. Strong early melee.";
                case "mystic":
                    return "Glass cannon with spells.";
                case "confessor":
                    return "Steady sustain and utility.";
                default:
                    return $"{BuildStatArchetypeSummary(characterClass)}.";
            }
        }

        private string BuildStatArchetypeSummary(CharacterClass characterClass)
        {
            int highestStatValue = characterClass.vitality;
            string highestStatName = "VIG";

            UpdateHighestStat(characterClass.endurance, "END", ref highestStatValue, ref highestStatName);
            UpdateHighestStat(characterClass.mind, "MND", ref highestStatValue, ref highestStatName);
            UpdateHighestStat(characterClass.strength, "STR", ref highestStatValue, ref highestStatName);
            UpdateHighestStat(characterClass.dexterity, "DEX", ref highestStatValue, ref highestStatName);
            UpdateHighestStat(characterClass.intelligence, "INT", ref highestStatValue, ref highestStatName);
            UpdateHighestStat(characterClass.faith, "FTH", ref highestStatValue, ref highestStatName);

            return $"{highestStatName}-focused.";
        }

        private void UpdateHighestStat(int statValue, string statName, ref int highestStatValue, ref string highestStatName)
        {
            if (statValue <= highestStatValue)
                return;

            highestStatValue = statValue;
            highestStatName = statName;
        }

        private string GetFormattedClassStats(CharacterClass characterClass)
        {
            return $"VIG {characterClass.vitality}   END {characterClass.endurance}   MND {characterClass.mind}\n" +
                   $"STR {characterClass.strength}   DEX {characterClass.dexterity}   INT {characterClass.intelligence}   FTH {characterClass.faith}";
        }

        private string GetFormattedClassLoadout(CharacterClass characterClass)
        {
            string rightHand = GetCompactItemListLabel(characterClass.mainHandWeapons, "Unarmed");
            string leftHand = GetCompactItemListLabel(characterClass.offHandWeapons, "Unarmed");
            string consumables = GetCompactItemListLabel(characterClass.quickSlotItems, "None");

            return $"RH  {rightHand}\nLH  {leftHand}\nITM {consumables}";
        }

        private string GetWeaponListLabel(WeaponItem[] weapons)
        {
            if (weapons == null || weapons.Length <= 0)
                return "Unarmed";

            List<string> labels = new List<string>();

            for (int i = 0; i < weapons.Length; i++)
            {
                WeaponItem weapon = weapons[i];

                if (weapon == null)
                    continue;

                labels.Add(weapon.itemName);
            }

            if (labels.Count <= 0)
                return "Unarmed";

            return string.Join(", ", labels);
        }

        private string GetQuickSlotListLabel(QuickSlotItem[] quickSlots)
        {
            if (quickSlots == null || quickSlots.Length <= 0)
                return "None";

            List<string> labels = new List<string>();

            for (int i = 0; i < quickSlots.Length; i++)
            {
                QuickSlotItem item = quickSlots[i];

                if (item == null)
                    continue;

                labels.Add(item.itemName);
            }

            if (labels.Count <= 0)
                return "None";

            return string.Join(", ", labels);
        }

        private string GetCompactItemListLabel<T>(T[] items, string fallbackLabel) where T : Item
        {
            if (items == null || items.Length <= 0)
                return fallbackLabel;

            List<string> labels = new List<string>();

            for (int i = 0; i < items.Length; i++)
            {
                T item = items[i];

                if (item == null)
                    continue;

                if (labels.Contains(item.itemName))
                    continue;

                labels.Add(item.itemName);

                if (labels.Count >= 2)
                    break;
            }

            if (labels.Count <= 0)
                return fallbackLabel;

            return string.Join(", ", labels);
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
            for (int i = 0; i < player.playerInventoryManager.weaponsInRightHandSlots.Length; i++)
            {
                player.playerInventoryManager.weaponsInRightHandSlots[i] = InstantiateWeaponOrFallback(GetWeaponAtIndex(mainHandWeapons, i));
            }

            player.playerInventoryManager.currentRightHandWeapon = player.playerInventoryManager.weaponsInRightHandSlots[0];
            player.playerNetworkManager.currentRightHandWeaponID.Value = player.playerInventoryManager.weaponsInRightHandSlots[0].itemID;

            for (int i = 0; i < player.playerInventoryManager.weaponsInLeftHandSlots.Length; i++)
            {
                player.playerInventoryManager.weaponsInLeftHandSlots[i] = InstantiateWeaponOrFallback(GetWeaponAtIndex(offHandWeapons, i));
            }

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

            if (player.playerInventoryManager.legEquipment != null)
                player.playerEquipmentManager.ForceClassLegPreview(player.playerInventoryManager.legEquipment.itemName);

            //set the quick slot items
            player.playerInventoryManager.quickSlotItemIndex = 0;
            player.playerInventoryManager.quickSlotItemsInQuickSlots[0] = null;
            player.playerInventoryManager.quickSlotItemsInQuickSlots[1] = null;
            player.playerInventoryManager.quickSlotItemsInQuickSlots[2] = null;

            if (GetQuickSlotItemAtIndex(quickSlotItems, 0) != null)
                player.playerInventoryManager.quickSlotItemsInQuickSlots[0] = Instantiate(GetQuickSlotItemAtIndex(quickSlotItems, 0));

            if (GetQuickSlotItemAtIndex(quickSlotItems, 1) != null)
                player.playerInventoryManager.quickSlotItemsInQuickSlots[1] = Instantiate(GetQuickSlotItemAtIndex(quickSlotItems, 1));

            if (GetQuickSlotItemAtIndex(quickSlotItems, 2) != null)
                player.playerInventoryManager.quickSlotItemsInQuickSlots[2] = Instantiate(GetQuickSlotItemAtIndex(quickSlotItems, 2));

            List<BuffCharmItem> defaultBuffCharms = WorldItemDatabase.Instance != null ? WorldItemDatabase.Instance.GetDefaultBuffCharms() : null;

            if (defaultBuffCharms != null)
            {
                int defaultCharmIndex = 0;

                for (int i = 0; i < player.playerInventoryManager.quickSlotItemsInQuickSlots.Length && defaultCharmIndex < defaultBuffCharms.Count; i++)
                {
                    if (player.playerInventoryManager.quickSlotItemsInQuickSlots[i] != null)
                        continue;

                    player.playerInventoryManager.quickSlotItemsInQuickSlots[i] = Instantiate(defaultBuffCharms[defaultCharmIndex]);
                    defaultCharmIndex++;
                }
            }

            player.playerEquipmentManager.LoadQuickSlotEquipment(player.playerInventoryManager.quickSlotItemsInQuickSlots[player.playerInventoryManager.quickSlotItemIndex]);
        }

        private WeaponItem InstantiateWeaponOrFallback(WeaponItem sourceWeapon)
        {
            WeaponItem fallbackWeapon = sourceWeapon;

            if (fallbackWeapon == null && WorldItemDatabase.Instance != null)
                fallbackWeapon = WorldItemDatabase.Instance.unarmedWeapon;

            return fallbackWeapon != null ? Instantiate(fallbackWeapon) : null;
        }

        private WeaponItem GetWeaponAtIndex(WeaponItem[] weapons, int index)
        {
            if (weapons == null || index < 0 || index >= weapons.Length)
                return null;

            return weapons[index];
        }

        private QuickSlotItem GetQuickSlotItemAtIndex(QuickSlotItem[] quickSlotItems, int index)
        {
            if (quickSlotItems == null || index < 0 || index >= quickSlotItems.Length)
                return null;

            return quickSlotItems[index];
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
