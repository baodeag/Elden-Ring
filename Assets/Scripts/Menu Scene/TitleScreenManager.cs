using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.InputManagerEntry;
using TMPro;
using UnityEngine.EventSystems;
using System.Collections.Generic;

namespace baodeag { 
    public class TitleScreenManager : MonoBehaviour
    {
        public static TitleScreenManager Instance;
        private const string DefaultNetworkAddress = "127.0.0.1:7777";
        private const float RuntimeClassButtonHeight = 72f;

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
        [SerializeField] private int selectedStartingClassID = -1;
        private bool networkMenuInitialized;
        private TitleScreenSettingsMenuView settingsMenuView;
        private readonly List<Button> runtimeCharacterClassButtons = new List<Button>();
        private RectTransform classReviewPanelRoot;
        private TextMeshProUGUI classReviewTitleText;
        private TextMeshProUGUI classReviewSubtitleText;
        private TextMeshProUGUI classReviewDescriptionText;
        private TextMeshProUGUI classReviewStatsText;
        private TextMeshProUGUI classReviewLoadoutText;
        private TextMeshProUGUI classReviewHintText;
        private TextMeshProUGUI characterClassButtonLabel;


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
            selectedStartingClassID = GetSelectedStartingClassID();
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
            EnsureCharacterClassSelectionUI();
            ToggleCharacterCreationScreenMainMenuButtons(false);

            characterClassMenu.SetActive(true);

            if (runtimeCharacterClassButtons.Count > 0)
            {
                int selectedClassID = GetSelectedStartingClassID();
                runtimeCharacterClassButtons[selectedClassID].Select();
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

            BuildRuntimeCharacterClassButtons();
            EnsureClassReviewPanel();
            UpdateCharacterClassPrimaryButtonLabel();
            UpdateClassReviewPanel(GetSelectedStartingClassID(), true);
        }

        private void BuildRuntimeCharacterClassButtons()
        {
            runtimeCharacterClassButtons.Clear();

            Button[] discoveredButtons = characterClassMenu.GetComponentsInChildren<Button>(true);

            if (discoveredButtons == null || discoveredButtons.Length <= 0)
                return;

            Button templateButton = discoveredButtons[0];

            for (int i = 0; i < startingClasses.Length; i++)
            {
                Button targetButton = i < discoveredButtons.Length
                    ? discoveredButtons[i]
                    : Instantiate(templateButton, characterClassMenu.transform, false);

                ConfigureRuntimeCharacterClassButton(targetButton, i);
                runtimeCharacterClassButtons.Add(targetButton);
            }

            for (int i = startingClasses.Length; i < discoveredButtons.Length; i++)
            {
                discoveredButtons[i].gameObject.SetActive(false);
            }
        }

        private void ConfigureRuntimeCharacterClassButton(Button button, int classID)
        {
            if (button == null)
                return;

            button.gameObject.SetActive(true);
            button.name = $"Class {classID}";
            button.onClick.RemoveAllListeners();
            button.onClick.AddListener(() => SelectClass(classID));

            EventTrigger[] legacyTriggers = button.GetComponents<EventTrigger>();

            for (int i = 0; i < legacyTriggers.Length; i++)
            {
                legacyTriggers[i].enabled = false;
            }

            RuntimeTitleScreenClassSelectionButton previewDriver = button.GetComponent<RuntimeTitleScreenClassSelectionButton>();

            if (previewDriver == null)
                previewDriver = button.gameObject.AddComponent<RuntimeTitleScreenClassSelectionButton>();

            previewDriver.Configure(this, classID);

            RectTransform buttonRect = button.GetComponent<RectTransform>();

            if (buttonRect != null)
                buttonRect.sizeDelta = new Vector2(buttonRect.sizeDelta.x, RuntimeClassButtonHeight);

            LayoutElement layoutElement = button.GetComponent<LayoutElement>();

            if (layoutElement == null)
                layoutElement = button.gameObject.AddComponent<LayoutElement>();

            layoutElement.minHeight = RuntimeClassButtonHeight;
            layoutElement.preferredHeight = RuntimeClassButtonHeight;

            TextMeshProUGUI buttonText = button.GetComponentInChildren<TextMeshProUGUI>(true);

            if (buttonText != null)
            {
                buttonText.enableAutoSizing = false;
                buttonText.fontSize = 20;
                buttonText.alignment = TextAlignmentOptions.Center;
                buttonText.text = GetFormattedClassButtonLabel(startingClasses[classID]);
            }
        }

        private void EnsureClassReviewPanel()
        {
            if (classReviewPanelRoot != null)
                return;

            if (titleScreenCharacterCreationMenu == null)
                return;

            Transform rightPanel = titleScreenCharacterCreationMenu.transform.Find("Right Panel (Character Review)");

            if (rightPanel == null)
                return;

            TextMeshProUGUI templateText = FindBestRuntimeTextTemplate();

            classReviewPanelRoot = new GameObject("Class Review Overlay", typeof(RectTransform), typeof(Image), typeof(VerticalLayoutGroup), typeof(ContentSizeFitter)).GetComponent<RectTransform>();
            classReviewPanelRoot.SetParent(rightPanel, false);
            classReviewPanelRoot.anchorMin = new Vector2(0f, 0f);
            classReviewPanelRoot.anchorMax = new Vector2(1f, 0f);
            classReviewPanelRoot.pivot = new Vector2(0.5f, 0f);
            classReviewPanelRoot.anchoredPosition = new Vector2(0f, 24f);
            classReviewPanelRoot.sizeDelta = new Vector2(-56f, 0f);

            Image background = classReviewPanelRoot.GetComponent<Image>();
            background.color = new Color(0f, 0f, 0f, 0.72f);

            VerticalLayoutGroup layout = classReviewPanelRoot.GetComponent<VerticalLayoutGroup>();
            layout.padding = new RectOffset(24, 24, 20, 20);
            layout.spacing = 10;
            layout.childControlWidth = true;
            layout.childControlHeight = false;
            layout.childForceExpandHeight = false;
            layout.childForceExpandWidth = true;

            ContentSizeFitter fitter = classReviewPanelRoot.GetComponent<ContentSizeFitter>();
            fitter.verticalFit = ContentSizeFitter.FitMode.PreferredSize;

            classReviewTitleText = CreateReviewText("Class Title", templateText, 30, FontStyles.Bold, new Color(1f, 0.85f, 0.35f));
            classReviewSubtitleText = CreateReviewText("Class Subtitle", templateText, 18, FontStyles.Normal, Color.white);
            classReviewDescriptionText = CreateReviewText("Class Description", templateText, 17, FontStyles.Normal, new Color(0.93f, 0.93f, 0.93f));
            classReviewStatsText = CreateReviewText("Class Stats", templateText, 17, FontStyles.Bold, new Color(0.95f, 0.95f, 0.95f));
            classReviewLoadoutText = CreateReviewText("Class Loadout", templateText, 16, FontStyles.Normal, new Color(0.88f, 0.88f, 0.88f));
            classReviewHintText = CreateReviewText("Class Hint", templateText, 15, FontStyles.Italic, new Color(1f, 0.85f, 0.35f));
        }

        private TextMeshProUGUI CreateReviewText(string objectName, TextMeshProUGUI template, float fontSize, FontStyles fontStyle, Color color)
        {
            GameObject textObject = new GameObject(objectName, typeof(RectTransform), typeof(TextMeshProUGUI), typeof(LayoutElement));
            textObject.transform.SetParent(classReviewPanelRoot, false);

            LayoutElement layoutElement = textObject.GetComponent<LayoutElement>();
            layoutElement.minHeight = fontSize + 10f;

            TextMeshProUGUI text = textObject.GetComponent<TextMeshProUGUI>();

            if (template != null)
            {
                text.font = template.font;
                text.fontSharedMaterial = template.fontSharedMaterial;
                text.textWrappingMode = TextWrappingModes.Normal;
            }

            text.fontSize = fontSize;
            text.fontStyle = fontStyle;
            text.color = color;
            text.alignment = TextAlignmentOptions.TopLeft;
            text.text = string.Empty;
            text.margin = Vector4.zero;

            return text;
        }

        private TextMeshProUGUI FindBestRuntimeTextTemplate()
        {
            if (characterClassMenu != null)
            {
                TextMeshProUGUI menuText = characterClassMenu.GetComponentInChildren<TextMeshProUGUI>(true);

                if (menuText != null)
                    return menuText;
            }

            if (startGameButton != null)
                return startGameButton.GetComponentInChildren<TextMeshProUGUI>(true);

            return null;
        }

        private void UpdateClassReviewPanel(int classID, bool isSelectedClass)
        {
            if (startingClasses == null || startingClasses.Length <= 0)
                return;

            EnsureClassReviewPanel();

            if (classReviewPanelRoot == null)
                return;

            int clampedClassID = Mathf.Clamp(classID, 0, startingClasses.Length - 1);
            CharacterClass characterClass = startingClasses[clampedClassID];

            classReviewTitleText.text = characterClass.className.ToUpperInvariant();
            classReviewSubtitleText.text = GetClassSubtitle(characterClass);
            classReviewDescriptionText.text = GetClassDescription(characterClass);
            classReviewStatsText.text = GetFormattedClassStats(characterClass);
            classReviewLoadoutText.text = GetFormattedClassLoadout(characterClass);
            classReviewHintText.text = isSelectedClass
                ? "Selected for New Game. Choose START when you are ready."
                : "Previewing class. Press Confirm to lock this choice in.";
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
                    return "Balanced frontline fighter";
                case "ranger":
                    return "Dexterity archer skirmisher";
                case "vanguard":
                    return "Heavy bruiser with strong poise";
                case "mystic":
                    return "Sorcery-focused ranged caster";
                case "confessor":
                    return "Faith hybrid with safe sustain";
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
                    return "A dependable all-rounder with sturdy armor, a shield, and enough offense to carry the early maps safely.";
                case "ranger":
                    return "Starts with ranged pressure and mobility, letting you thin packs before they can close the gap.";
                case "vanguard":
                    return "Built to trade hits and dominate close combat, ideal if you want raw strength and a forgiving health pool.";
                case "mystic":
                    return "Leans on mind and intelligence for spellcasting, trading durability for strong ranged burst and utility.";
                case "confessor":
                    return "A flexible hybrid that mixes melee with faith scaling, giving you safer progression and adaptable combat options.";
                default:
                    return $"An adaptable {BuildStatArchetypeSummary(characterClass).ToLowerInvariant()} that starts with {GetLoadoutHeadline(characterClass)}.";
            }
        }

        private string BuildStatArchetypeSummary(CharacterClass characterClass)
        {
            int highestStatValue = characterClass.vitality;
            string highestStatName = "Vigor";

            UpdateHighestStat(characterClass.endurance, "Endurance", ref highestStatValue, ref highestStatName);
            UpdateHighestStat(characterClass.mind, "Mind", ref highestStatValue, ref highestStatName);
            UpdateHighestStat(characterClass.strength, "Strength", ref highestStatValue, ref highestStatName);
            UpdateHighestStat(characterClass.dexterity, "Dexterity", ref highestStatValue, ref highestStatName);
            UpdateHighestStat(characterClass.intelligence, "Intelligence", ref highestStatValue, ref highestStatName);
            UpdateHighestStat(characterClass.faith, "Faith", ref highestStatValue, ref highestStatName);

            return $"{highestStatName}-leaning adventurer";
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
            return $"STATS  VIG {characterClass.vitality}  END {characterClass.endurance}  MND {characterClass.mind}\n" +
                   $"        STR {characterClass.strength}  DEX {characterClass.dexterity}  INT {characterClass.intelligence}  FTH {characterClass.faith}";
        }

        private string GetFormattedClassLoadout(CharacterClass characterClass)
        {
            string rightHand = GetWeaponListLabel(characterClass.mainHandWeapons);
            string leftHand = GetWeaponListLabel(characterClass.offHandWeapons);
            string consumables = GetQuickSlotListLabel(characterClass.quickSlotItems);

            return $"LOADOUT  RH: {rightHand}\n" +
                   $"         LH: {leftHand}\n" +
                   $"         ITEMS: {consumables}";
        }

        private string GetLoadoutHeadline(CharacterClass characterClass)
        {
            string firstWeaponName = GetFirstWeaponName(characterClass.mainHandWeapons);

            if (!string.IsNullOrEmpty(firstWeaponName))
                return firstWeaponName.ToLowerInvariant();

            return "a flexible kit";
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

        private string GetFirstWeaponName(WeaponItem[] weapons)
        {
            if (weapons == null)
                return string.Empty;

            for (int i = 0; i < weapons.Length; i++)
            {
                if (weapons[i] != null)
                    return weapons[i].itemName;
            }

            return string.Empty;
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

    public class RuntimeTitleScreenClassSelectionButton : MonoBehaviour, ISelectHandler, IPointerEnterHandler
    {
        private TitleScreenManager manager;
        private int classID;

        public void Configure(TitleScreenManager targetManager, int targetClassID)
        {
            manager = targetManager;
            classID = targetClassID;
        }

        public void OnSelect(BaseEventData eventData)
        {
            manager?.PreviewClass(classID);
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            manager?.PreviewClass(classID);
        }
    }
}
