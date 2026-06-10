using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Threading.Tasks;

namespace baodeag
{
    public class PlayerUICharacterMenuManager : PlayerUIMenu
    {
        private const string RelayJoinPlaceholder = "Enter 6-character Relay code";
        private const string DirectJoinPlaceholder = "127.0.0.1:7777";
        private const string CharacterMenuShopTitle = "Roundtable Shop";

        private bool joinWorldUIInitialized;
        private bool menuButtonsInitialized;
        private const string BackToMainMenuButtonName = "Back To Main Menu";
        private const string BackToMainMenuButtonLabel = "BACK TO MAIN MENU";

        [Header("Relay Join UI")]
        [SerializeField] private RectTransform serializedJoinWorldControlsRoot;
        [SerializeField] private TextMeshProUGUI serializedWorldAddressLabel;
        [SerializeField] private TMP_InputField serializedJoinWorldAddressInputField;
        [SerializeField] private Button serializedCheckCodeButton;
        [SerializeField] private Button serializedJoinWorldButton;
        [SerializeField] private TextMeshProUGUI serializedJoinStatusLabel;

        private RectTransform joinWorldControlsRoot;
        private TextMeshProUGUI worldAddressLabel;
        private TextMeshProUGUI joinStatusLabel;
        private TMP_InputField joinWorldAddressInputField;
        private Button joinWorldButton;
        private Button checkCodeButton;
        private Button shopButton;
        private Button settingsButton;
        private Button backToMainMenuButton;
        private Color worldAddressLabelDefaultColor = Color.white;
        private Color joinStatusLabelDefaultColor = Color.white;
        private bool isJoiningWorld;
        private bool isCheckingRelayCode;
        private string verifiedRelayJoinCode = string.Empty;

        private void OnEnable()
        {
            if (WorldGameSessionManager.instance != null)
                WorldGameSessionManager.instance.CurrentConnectionAddressChanged += OnCurrentConnectionAddressChanged;
        }

        private void OnDisable()
        {
            if (WorldGameSessionManager.instance != null)
                WorldGameSessionManager.instance.CurrentConnectionAddressChanged -= OnCurrentConnectionAddressChanged;
        }

        public override void OpenMenu()
        {
            base.OpenMenu();

            EnsureJoinWorldUI();
            EnsureMenuButtons();
            RefreshJoinWorldUI();
        }

        private void OnCurrentConnectionAddressChanged()
        {
            EnsureJoinWorldUI();
            RefreshJoinWorldUI();
        }

        private void EnsureMenuButtons()
        {
            if (menuButtonsInitialized)
                return;

            Button[] buttons = GetComponentsInChildren<Button>(true);
            Button buttonTemplate = null;

            for (int i = 0; i < buttons.Length; i++)
            {
                if (buttons[i] == null)
                    continue;

                if (buttons[i].name == "Shop")
                    shopButton = buttons[i];

                if (buttons[i].name == "Settings")
                {
                    settingsButton = buttons[i];
                }

                if (buttons[i].name == BackToMainMenuButtonName)
                    backToMainMenuButton = buttons[i];
            }

            buttonTemplate = settingsButton != null ? settingsButton : shopButton;

            if (backToMainMenuButton == null && buttonTemplate != null)
                backToMainMenuButton = CreateBackToMainMenuButton(buttonTemplate);

            if (shopButton != null)
            {
                shopButton.onClick.RemoveAllListeners();
                shopButton.onClick.AddListener(OpenShopMenu);
            }

            if (settingsButton != null)
            {
                settingsButton.onClick.RemoveAllListeners();
                settingsButton.onClick.AddListener(OpenSettingsMenu);
            }

            if (backToMainMenuButton != null)
            {
                backToMainMenuButton.onClick.RemoveAllListeners();
                backToMainMenuButton.onClick.AddListener(ReturnToPressStartScreen);
                ForceButtonUsable(backToMainMenuButton);
            }

            menuButtonsInitialized = shopButton != null || settingsButton != null || backToMainMenuButton != null;
        }

        private Button CreateBackToMainMenuButton(Button buttonTemplate)
        {
            GameObject buttonObject = Instantiate(buttonTemplate.gameObject, buttonTemplate.transform.parent);
            buttonObject.name = BackToMainMenuButtonName;
            buttonObject.transform.SetAsLastSibling();

            TextMeshProUGUI buttonLabel = buttonObject.GetComponentInChildren<TextMeshProUGUI>(true);

            if (buttonLabel != null)
                buttonLabel.text = BackToMainMenuButtonLabel;

            return buttonObject.GetComponent<Button>();
        }

        private void OpenShopMenu()
        {
            if (PlayerUIManager.instance == null || PlayerUIManager.instance.playerUIShopManager == null)
                return;

            PlayerUIManager.instance.TransitionToMenu(this, PlayerUIManager.instance.playerUIShopManager);
            PlayerUIManager.instance.playerUIShopManager.OpenGlobalShop(CharacterMenuShopTitle);
        }

        private void OpenSettingsMenu()
        {
            if (PlayerUIManager.instance == null || PlayerUIManager.instance.playerUISettingsManager == null)
                return;

            PlayerUIManager.instance.playerUISettingsManager.OpenFromCharacterMenu();
        }

        private void ReturnToPressStartScreen()
        {
            if (PlayerUIManager.instance != null)
                PlayerUIManager.instance.CloseAllMenuWindows();

            if (WorldGameSessionManager.instance != null)
            {
                WorldGameSessionManager.instance.ReturnToTitleFromEndGame();
                return;
            }

            if (NetworkManager.Singleton != null &&
                (NetworkManager.Singleton.IsHost || NetworkManager.Singleton.IsClient))
            {
                NetworkManager.Singleton.Shutdown();
            }

            SceneManager.LoadScene(0);
        }

        private void EnsureJoinWorldUI()
        {
            if (joinWorldUIInitialized)
                return;

            if (UseSerializedJoinWorldUI())
            {
                joinWorldUIInitialized = true;
                RefreshJoinControlsState();
                return;
            }

            RectTransform menuRoot = transform.childCount > 0 ? transform.GetChild(0) as RectTransform : null;

            if (menuRoot == null)
                return;

            Button buttonTemplate = menuRoot.GetComponentInChildren<Button>(true);
            TextMeshProUGUI textTemplate = menuRoot.GetComponentInChildren<TextMeshProUGUI>(true);

            if (buttonTemplate == null || textTemplate == null)
                return;

            joinWorldControlsRoot = CreateControlsRoot(menuRoot);
            worldAddressLabel = CreateInfoLabel(joinWorldControlsRoot, textTemplate);
            joinWorldAddressInputField = CreateAddressInputField(joinWorldControlsRoot, textTemplate);
            checkCodeButton = CreateCheckCodeButton(joinWorldControlsRoot, textTemplate);
            joinWorldButton = CreateJoinWorldButton(joinWorldControlsRoot, textTemplate);
            joinStatusLabel = CreateStatusLabel(joinWorldControlsRoot, textTemplate);

            joinWorldUIInitialized = true;
            RefreshJoinControlsState();
        }

        private bool UseSerializedJoinWorldUI()
        {
            if (serializedJoinWorldControlsRoot == null ||
                serializedWorldAddressLabel == null ||
                serializedJoinWorldAddressInputField == null ||
                serializedCheckCodeButton == null ||
                serializedJoinWorldButton == null ||
                serializedJoinStatusLabel == null)
            {
                TryFindSerializedJoinWorldUIByName();
            }

            if (serializedJoinWorldControlsRoot == null ||
                serializedWorldAddressLabel == null ||
                serializedJoinWorldAddressInputField == null ||
                serializedCheckCodeButton == null ||
                serializedJoinWorldButton == null ||
                serializedJoinStatusLabel == null)
            {
                return false;
            }

            joinWorldControlsRoot = serializedJoinWorldControlsRoot;
            worldAddressLabel = serializedWorldAddressLabel;
            joinWorldAddressInputField = serializedJoinWorldAddressInputField;
            checkCodeButton = serializedCheckCodeButton;
            joinWorldButton = serializedJoinWorldButton;
            joinStatusLabel = serializedJoinStatusLabel;

            worldAddressLabelDefaultColor = worldAddressLabel.color;
            joinStatusLabelDefaultColor = joinStatusLabel.color;

            joinWorldAddressInputField.onValueChanged.RemoveAllListeners();
            joinWorldAddressInputField.onValueChanged.AddListener(_ =>
            {
                verifiedRelayJoinCode = string.Empty;
                RefreshJoinControlsState();
            });

            checkCodeButton.onClick.RemoveAllListeners();
            checkCodeButton.onClick.AddListener(CheckRelayCodeFromCharacterMenu);

            joinWorldButton.onClick.RemoveAllListeners();
            joinWorldButton.onClick.AddListener(JoinWorldFromCharacterMenu);

            return true;
        }

        private void TryFindSerializedJoinWorldUIByName()
        {
            Transform controlsRoot = transform.Find("Menu/Relay Join Panel");

            if (controlsRoot == null)
                controlsRoot = transform.Find("Relay Join Panel");

            if (controlsRoot == null)
                return;

            serializedJoinWorldControlsRoot = controlsRoot as RectTransform;
            serializedWorldAddressLabel = FindChildComponent<TextMeshProUGUI>(controlsRoot, "World Address Label");
            serializedJoinWorldAddressInputField = FindChildComponent<TMP_InputField>(controlsRoot, "Relay Code Input");
            serializedCheckCodeButton = FindChildComponent<Button>(controlsRoot, "Check Code Button");
            serializedJoinWorldButton = FindChildComponent<Button>(controlsRoot, "Join World Button");
            serializedJoinStatusLabel = FindChildComponent<TextMeshProUGUI>(controlsRoot, "Join Status Label");
        }

        private T FindChildComponent<T>(Transform root, string childName) where T : Component
        {
            Transform[] children = root.GetComponentsInChildren<Transform>(true);

            for (int i = 0; i < children.Length; i++)
            {
                if (children[i] != null && children[i].name == childName)
                    return children[i].GetComponent<T>();
            }

            return null;
        }

        private RectTransform CreateControlsRoot(RectTransform menuRoot)
        {
            GameObject controlsRootObject = new GameObject("Join World Controls", typeof(RectTransform), typeof(Image));
            RectTransform controlsRoot = controlsRootObject.GetComponent<RectTransform>();
            Image background = controlsRootObject.GetComponent<Image>();

            controlsRoot.SetParent(menuRoot, false);
            controlsRoot.SetAsLastSibling();
            controlsRoot.anchorMin = new Vector2(1f, 1f);
            controlsRoot.anchorMax = new Vector2(1f, 1f);
            controlsRoot.pivot = new Vector2(1f, 1f);
            controlsRoot.anchoredPosition = new Vector2(-48f, -48f);
            controlsRoot.sizeDelta = new Vector2(430f, 410f);

            background.color = new Color(0f, 0f, 0f, 0.72f);

            return controlsRoot;
        }

        private TextMeshProUGUI CreateInfoLabel(RectTransform parent, TextMeshProUGUI textTemplate)
        {
            GameObject labelObject = new GameObject("World Address Label", typeof(RectTransform), typeof(TextMeshProUGUI));
            RectTransform labelRect = labelObject.GetComponent<RectTransform>();
            TextMeshProUGUI label = labelObject.GetComponent<TextMeshProUGUI>();

            labelRect.SetParent(parent, false);
            labelRect.anchorMin = new Vector2(0f, 1f);
            labelRect.anchorMax = new Vector2(1f, 1f);
            labelRect.pivot = new Vector2(0.5f, 1f);
            labelRect.anchoredPosition = new Vector2(0f, -18f);
            labelRect.sizeDelta = new Vector2(-32f, 86f);

            CopyTextStyle(textTemplate, label);
            label.fontSize = 20f;
            label.alignment = TextAlignmentOptions.Left;
            label.enableWordWrapping = true;
            label.overflowMode = TextOverflowModes.Ellipsis;
            label.text = "WORLD ADDRESS";
            worldAddressLabelDefaultColor = label.color;

            return label;
        }

        private TextMeshProUGUI CreateStatusLabel(RectTransform parent, TextMeshProUGUI textTemplate)
        {
            GameObject labelObject = new GameObject("Join Status Label", typeof(RectTransform), typeof(TextMeshProUGUI));
            RectTransform labelRect = labelObject.GetComponent<RectTransform>();
            TextMeshProUGUI label = labelObject.GetComponent<TextMeshProUGUI>();

            labelRect.SetParent(parent, false);
            labelRect.anchorMin = new Vector2(0f, 1f);
            labelRect.anchorMax = new Vector2(1f, 1f);
            labelRect.pivot = new Vector2(0.5f, 1f);
            labelRect.anchoredPosition = new Vector2(0f, -352f);
            labelRect.sizeDelta = new Vector2(-32f, 42f);

            CopyTextStyle(textTemplate, label);
            label.fontSize = 18f;
            label.alignment = TextAlignmentOptions.Left;
            label.enableWordWrapping = true;
            label.overflowMode = TextOverflowModes.Ellipsis;
            label.text = string.Empty;
            joinStatusLabelDefaultColor = label.color;

            return label;
        }

        private TMP_InputField CreateAddressInputField(RectTransform parent, TextMeshProUGUI textTemplate)
        {
            GameObject inputRootObject = new GameObject("Join World Address Input", typeof(RectTransform), typeof(Image), typeof(TMP_InputField));
            RectTransform inputRootRect = inputRootObject.GetComponent<RectTransform>();
            Image inputBackground = inputRootObject.GetComponent<Image>();
            TMP_InputField inputField = inputRootObject.GetComponent<TMP_InputField>();

            inputRootRect.SetParent(parent, false);
            inputRootRect.anchorMin = new Vector2(0f, 1f);
            inputRootRect.anchorMax = new Vector2(1f, 1f);
            inputRootRect.pivot = new Vector2(0.5f, 1f);
            inputRootRect.anchoredPosition = new Vector2(0f, -128f);
            inputRootRect.sizeDelta = new Vector2(-32f, 54f);

            inputBackground.color = new Color(1f, 1f, 1f, 0.12f);

            GameObject textAreaObject = new GameObject("Text Area", typeof(RectTransform), typeof(RectMask2D));
            RectTransform textAreaRect = textAreaObject.GetComponent<RectTransform>();
            textAreaRect.SetParent(inputRootRect, false);
            textAreaRect.anchorMin = Vector2.zero;
            textAreaRect.anchorMax = Vector2.one;
            textAreaRect.offsetMin = new Vector2(16f, 8f);
            textAreaRect.offsetMax = new Vector2(-16f, -8f);

            GameObject placeholderObject = new GameObject("Placeholder", typeof(RectTransform), typeof(TextMeshProUGUI));
            RectTransform placeholderRect = placeholderObject.GetComponent<RectTransform>();
            TextMeshProUGUI placeholderText = placeholderObject.GetComponent<TextMeshProUGUI>();
            placeholderRect.SetParent(textAreaRect, false);
            placeholderRect.anchorMin = Vector2.zero;
            placeholderRect.anchorMax = Vector2.one;
            placeholderRect.offsetMin = Vector2.zero;
            placeholderRect.offsetMax = Vector2.zero;
            CopyTextStyle(textTemplate, placeholderText);
            placeholderText.fontSize = 24f;
            placeholderText.color = new Color(1f, 1f, 1f, 0.35f);
            placeholderText.alignment = TextAlignmentOptions.Left;
            placeholderText.text = DirectJoinPlaceholder;

            GameObject textObject = new GameObject("Text", typeof(RectTransform), typeof(TextMeshProUGUI));
            RectTransform textRect = textObject.GetComponent<RectTransform>();
            TextMeshProUGUI inputText = textObject.GetComponent<TextMeshProUGUI>();
            textRect.SetParent(textAreaRect, false);
            textRect.anchorMin = Vector2.zero;
            textRect.anchorMax = Vector2.one;
            textRect.offsetMin = Vector2.zero;
            textRect.offsetMax = Vector2.zero;
            CopyTextStyle(textTemplate, inputText);
            inputText.fontSize = 24f;
            inputText.alignment = TextAlignmentOptions.Left;
            inputText.text = string.Empty;

            inputField.textViewport = textAreaRect;
            inputField.textComponent = inputText;
            inputField.placeholder = placeholderText;
            inputField.contentType = TMP_InputField.ContentType.Standard;
            inputField.lineType = TMP_InputField.LineType.SingleLine;
            inputField.SetTextWithoutNotify(string.Empty);
            inputField.onValueChanged.AddListener(_ =>
            {
                verifiedRelayJoinCode = string.Empty;
                RefreshJoinControlsState();
            });

            return inputField;
        }

        private Button CreateCheckCodeButton(RectTransform parent, TextMeshProUGUI textTemplate)
        {
            return CreateActionButton(
                parent,
                textTemplate,
                "Check Relay Code Button",
                "CHECK CODE",
                -204f,
                CheckRelayCodeFromCharacterMenu);
        }

        private Button CreateJoinWorldButton(RectTransform parent, TextMeshProUGUI textTemplate)
        {
            Button button = CreateActionButton(
                parent,
                textTemplate,
                "Join World Button",
                "JOIN WORLD",
                -282f,
                JoinWorldFromCharacterMenu);

            button.interactable = false;
            return button;
        }

        private Button CreateActionButton(RectTransform parent, TextMeshProUGUI textTemplate, string objectName, string label, float anchoredY, UnityEngine.Events.UnityAction callback)
        {
            GameObject buttonObject = new GameObject(objectName, typeof(RectTransform), typeof(Image), typeof(Button));
            RectTransform buttonRect = buttonObject.GetComponent<RectTransform>();
            Image buttonImage = buttonObject.GetComponent<Image>();
            Button button = buttonObject.GetComponent<Button>();

            buttonRect.SetParent(parent, false);
            buttonRect.anchorMin = new Vector2(0f, 1f);
            buttonRect.anchorMax = new Vector2(1f, 1f);
            buttonRect.pivot = new Vector2(0.5f, 1f);
            buttonRect.anchoredPosition = new Vector2(0f, anchoredY);
            buttonRect.sizeDelta = new Vector2(-32f, 62f);

            buttonImage.color = new Color(1f, 1f, 1f, 0.14f);
            buttonImage.raycastTarget = true;

            button.targetGraphic = buttonImage;
            button.onClick.RemoveAllListeners();
            button.onClick.AddListener(callback);

            ColorBlock colors = button.colors;
            colors.normalColor = new Color(1f, 1f, 1f, 0.14f);
            colors.highlightedColor = new Color(1f, 1f, 1f, 0.24f);
            colors.pressedColor = new Color(1f, 1f, 1f, 0.34f);
            colors.selectedColor = colors.highlightedColor;
            colors.disabledColor = new Color(1f, 1f, 1f, 0.05f);
            button.colors = colors;

            GameObject textObject = new GameObject("Label", typeof(RectTransform), typeof(TextMeshProUGUI));
            RectTransform textRect = textObject.GetComponent<RectTransform>();
            TextMeshProUGUI buttonText = textObject.GetComponent<TextMeshProUGUI>();

            textRect.SetParent(buttonRect, false);
            textRect.anchorMin = Vector2.zero;
            textRect.anchorMax = Vector2.one;
            textRect.offsetMin = Vector2.zero;
            textRect.offsetMax = Vector2.zero;

            CopyTextStyle(textTemplate, buttonText);
            buttonText.fontSize = 24f;
            buttonText.alignment = TextAlignmentOptions.Center;
            buttonText.raycastTarget = false;
            buttonText.text = label;

            ForceButtonUsable(button);
            return button;
        }

        private void RefreshJoinWorldUI()
        {
            if (!joinWorldUIInitialized || worldAddressLabel == null)
                return;

            bool showJoinWorldPanel = WorldGameSessionManager.instance != null &&
                                      WorldGameSessionManager.instance.RequiresRelayForCurrentMode();

            if (joinWorldControlsRoot != null)
                joinWorldControlsRoot.gameObject.SetActive(showJoinWorldPanel);

            if (!showJoinWorldPanel)
            {
                verifiedRelayJoinCode = string.Empty;
                ClearJoinStatus();
                return;
            }

            worldAddressLabel.color = worldAddressLabelDefaultColor;
            UpdateJoinInputPlaceholder();
            ClearJoinStatus();

            if (NetworkManager.Singleton.IsHost)
            {
                string currentConnectionAddress = WorldGameSessionManager.instance.GetCurrentConnectionAddress();
                string addressLabel = WorldGameSessionManager.instance.HasRelayJoinCode()
                    ? "YOUR RELAY CODE"
                    : "YOUR LOCAL ADDRESS";

                ClearOwnHostCodeFromJoinInput(currentConnectionAddress);
                worldAddressLabel.text = $"{addressLabel}\n<size=140%>{currentConnectionAddress}</size>\n<size=75%>Send this to the other player.</size>";
            }
            else if (NetworkManager.Singleton.IsClient)
            {
                worldAddressLabel.text = WorldGameSessionManager.instance.RequiresRelayForCurrentMode()
                    ? "JOIN ANOTHER WORLD\n<size=75%>Enter a valid Relay code to join.</size>"
                    : "JOIN ANOTHER WORLD\n<size=75%>Enter a local IP address to join directly.</size>";
            }
            else
            {
                worldAddressLabel.text = WorldGameSessionManager.instance.RequiresRelayForCurrentMode()
                    ? "WORLD ADDRESS\n<size=75%>Multiplayer requires a valid Relay code.</size>"
                    : "WORLD ADDRESS\n<size=75%>Singleplayer uses a local host address, not Relay.</size>";
            }

            RefreshJoinControlsState();
        }

        private async void CheckRelayCodeFromCharacterMenu()
        {
            if (isCheckingRelayCode || isJoiningWorld)
                return;

            if (!WorldGameSessionManager.instance.RequiresRelayForCurrentMode())
            {
                verifiedRelayJoinCode = string.Empty;
                ShowJoinStatus("Singleplayer mode does not use Relay.", joinStatusLabelDefaultColor);
                RefreshJoinControlsState();
                return;
            }

            string addressInput = GetJoinAddressInput();

            if (IsOwnRelayCodeInput(addressInput))
            {
                verifiedRelayJoinCode = string.Empty;
                ShowJoinStatus("You are already hosting this Relay code.", new Color(1f, 0.78f, 0.25f, 1f));
                RefreshJoinControlsState();
                return;
            }

            if (!IsRelayCodeInput(addressInput))
            {
                verifiedRelayJoinCode = string.Empty;

                if (IsAddressInput(addressInput))
                {
                    ShowJoinStatus("IP join does not need Relay check.", joinStatusLabelDefaultColor);
                }
                else
                {
                    ShowJoinStatus("Check failed: enter a 6-character Relay code.", new Color(1f, 0.38f, 0.28f, 1f));
                }

                RefreshJoinControlsState();
                return;
            }

            isCheckingRelayCode = true;
            RefreshJoinControlsState();
            ShowJoinStatus("Checking code...", joinStatusLabelDefaultColor);

            bool codeIsValid = await WorldGameSessionManager.instance.CheckRelayJoinCodeAsync(addressInput);

            isCheckingRelayCode = false;

            if (!codeIsValid)
            {
                verifiedRelayJoinCode = string.Empty;
                ShowJoinStatus("Check failed: Relay code is invalid or expired.", new Color(1f, 0.38f, 0.28f, 1f));
                RefreshJoinControlsState();
                return;
            }

            verifiedRelayJoinCode = addressInput.Trim().ToUpperInvariant();
            ShowJoinStatus($"Code ready: {verifiedRelayJoinCode}. You can join this world now.", new Color(0.45f, 1f, 0.55f, 1f));
            RefreshJoinControlsState();
        }

        private async void JoinWorldFromCharacterMenu()
        {
            if (isJoiningWorld)
                return;

            string addressInput = GetJoinAddressInput();

            if (IsOwnRelayCodeInput(addressInput))
            {
                ShowJoinStatus("You cannot join the world you are already hosting.", new Color(1f, 0.78f, 0.25f, 1f));
                RefreshJoinControlsState();
                return;
            }

            if (WorldGameSessionManager.instance.RequiresRelayForCurrentMode() && !IsRelayCodeInput(addressInput))
            {
                ShowJoinStatus("Multiplayer mode only accepts Relay codes.", new Color(1f, 0.78f, 0.25f, 1f));
                RefreshJoinControlsState();
                return;
            }

            if (IsRelayCodeInput(addressInput) && !IsCurrentRelayCodeVerified(addressInput))
            {
                ShowJoinStatus("Check code first. Relay code must pass before joining.", new Color(1f, 0.78f, 0.25f, 1f));
                RefreshJoinControlsState();
                return;
            }

            isJoiningWorld = true;
            RefreshJoinControlsState();
            ShowJoinStatus("Connecting...", joinStatusLabelDefaultColor);

            bool joinStarted = await WorldGameSessionManager.instance.StartGameAsClientAsync(addressInput);

            isJoiningWorld = false;
            RefreshJoinControlsState();

            if (!joinStarted)
            {
                ShowJoinStatus("Join failed: Relay code is invalid or expired.", new Color(1f, 0.38f, 0.28f, 1f));
                return;
            }

            PlayerUIManager.instance.CloseAllMenuWindows();
        }

        private void SetJoinWorldButtonInteractable(bool isInteractable)
        {
            if (joinWorldButton != null)
                joinWorldButton.interactable = isInteractable;
        }

        private void RefreshCheckCodeButtonState()
        {
            if (checkCodeButton != null)
            {
                checkCodeButton.gameObject.SetActive(WorldGameSessionManager.instance != null &&
                                                     WorldGameSessionManager.instance.RequiresRelayForCurrentMode());
                checkCodeButton.interactable = !isCheckingRelayCode &&
                                               !isJoiningWorld;
            }
        }

        private void RefreshJoinControlsState()
        {
            RefreshCheckCodeButtonState();
            RefreshJoinButtonState();
        }

        private void RefreshJoinButtonState()
        {
            if (joinWorldButton == null)
                return;

            if (isCheckingRelayCode || isJoiningWorld)
            {
                joinWorldButton.interactable = false;
                return;
            }

            string addressInput = GetJoinAddressInput();

            if (WorldGameSessionManager.instance != null && WorldGameSessionManager.instance.RequiresRelayForCurrentMode())
            {
                joinWorldButton.interactable = IsRelayCodeInput(addressInput) &&
                                               !IsOwnRelayCodeInput(addressInput) &&
                                               IsCurrentRelayCodeVerified(addressInput);
                return;
            }

            if (IsRelayCodeInput(addressInput))
            {
                joinWorldButton.interactable = !IsOwnRelayCodeInput(addressInput) &&
                                               IsCurrentRelayCodeVerified(addressInput);
                return;
            }

            joinWorldButton.interactable = IsAddressInput(addressInput);
        }

        private string GetJoinAddressInput()
        {
            return joinWorldAddressInputField != null && !string.IsNullOrWhiteSpace(joinWorldAddressInputField.text)
                ? joinWorldAddressInputField.text
                : string.Empty;
        }

        private void ClearOwnHostCodeFromJoinInput(string currentConnectionAddress)
        {
            if (joinWorldAddressInputField == null)
                return;

            if (string.IsNullOrWhiteSpace(joinWorldAddressInputField.text) ||
                string.Equals(joinWorldAddressInputField.text.Trim(), currentConnectionAddress, System.StringComparison.OrdinalIgnoreCase))
            {
                joinWorldAddressInputField.SetTextWithoutNotify(string.Empty);
            }
        }

        private bool IsRelayCodeInput(string addressInput)
        {
            if (string.IsNullOrWhiteSpace(addressInput))
                return false;

            string trimmedInput = addressInput
                .Replace("\u200B", string.Empty)
                .Replace("\uFEFF", string.Empty)
                .Trim();

            if (trimmedInput.Contains(":") || trimmedInput.Contains("."))
                return false;

            return trimmedInput.Length == 6;
        }

        private bool IsAddressInput(string addressInput)
        {
            if (string.IsNullOrWhiteSpace(addressInput))
                return false;

            string trimmedInput = addressInput.Trim();
            return trimmedInput.Contains(".") || trimmedInput.Contains(":");
        }

        private bool IsCurrentRelayCodeVerified(string addressInput)
        {
            if (!IsRelayCodeInput(addressInput))
                return false;

            return verifiedRelayJoinCode == addressInput.Trim().ToUpperInvariant() &&
                   WorldGameSessionManager.instance.IsRelayJoinCodeChecked(addressInput);
        }

        private bool IsOwnRelayCodeInput(string addressInput)
        {
            return NetworkManager.Singleton != null &&
                   NetworkManager.Singleton.IsHost &&
                   WorldGameSessionManager.instance != null &&
                   WorldGameSessionManager.instance.IsCurrentRelayJoinCode(addressInput);
        }

        private void ShowJoinStatus(string statusText, Color statusColor)
        {
            if (joinStatusLabel == null)
                return;

            joinStatusLabel.color = statusColor;
            joinStatusLabel.text = statusText;
        }

        private void ClearJoinStatus()
        {
            if (joinStatusLabel == null)
                return;

            joinStatusLabel.color = joinStatusLabelDefaultColor;
            joinStatusLabel.text = string.Empty;
        }

        private void UpdateJoinInputPlaceholder()
        {
            if (joinWorldAddressInputField == null)
                return;

            TMP_Text placeholderText = joinWorldAddressInputField.placeholder as TMP_Text;

            if (placeholderText == null)
                return;

            if (WorldGameSessionManager.instance != null &&
                NetworkManager.Singleton != null &&
                NetworkManager.Singleton.IsHost &&
                WorldGameSessionManager.instance.HasRelayJoinCode())
            {
                placeholderText.text = WorldGameSessionManager.instance.GetCurrentConnectionAddress();
                return;
            }

            placeholderText.text = WorldGameSessionManager.instance != null &&
                                   WorldGameSessionManager.instance.RequiresRelayForCurrentMode()
                ? RelayJoinPlaceholder
                : DirectJoinPlaceholder;
        }

        private void ForceButtonUsable(Button button)
        {
            if (button == null)
                return;

            button.enabled = true;
            button.interactable = true;
            button.navigation = Navigation.defaultNavigation;

            CanvasGroup canvasGroup = button.GetComponent<CanvasGroup>();

            if (canvasGroup != null)
            {
                canvasGroup.alpha = 1f;
                canvasGroup.interactable = true;
                canvasGroup.blocksRaycasts = true;
            }

            Graphic[] graphics = button.GetComponentsInChildren<Graphic>(true);

            for (int i = 0; i < graphics.Length; i++)
            {
                if (graphics[i] != null)
                    graphics[i].raycastTarget = true;
            }
        }

        private void CopyTextStyle(TextMeshProUGUI source, TextMeshProUGUI destination)
        {
            destination.font = source.font;
            destination.fontSharedMaterial = source.fontSharedMaterial;
            destination.color = source.color;
            destination.enableWordWrapping = source.enableWordWrapping;
            destination.richText = source.richText;
        }
    }
}
