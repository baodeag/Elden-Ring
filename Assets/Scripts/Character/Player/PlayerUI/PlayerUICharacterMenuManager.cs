using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;
using System.Threading.Tasks;

namespace baodeag
{
    public class PlayerUICharacterMenuManager : PlayerUIMenu
    {
        private const string DefaultJoinAddress = "Relay code or 127.0.0.1:7777";
        private const string CharacterMenuShopTitle = "Roundtable Shop";

        private bool joinWorldUIInitialized;
        private bool menuButtonsInitialized;
        private RectTransform joinWorldControlsRoot;
        private TextMeshProUGUI worldAddressLabel;
        private TextMeshProUGUI joinStatusLabel;
        private TMP_InputField joinWorldAddressInputField;
        private Button joinWorldButton;
        private Button checkCodeButton;
        private Button shopButton;
        private Button settingsButton;
        private Color worldAddressLabelDefaultColor = Color.white;
        private Color joinStatusLabelDefaultColor = Color.white;
        private bool isJoiningWorld;
        private bool isCheckingRelayCode;
        private string verifiedRelayJoinCode = string.Empty;

        public override void OpenMenu()
        {
            base.OpenMenu();

            EnsureJoinWorldUI();
            EnsureMenuButtons();
            RefreshJoinWorldUI();
        }

        private void EnsureMenuButtons()
        {
            if (menuButtonsInitialized)
                return;

            Button[] buttons = GetComponentsInChildren<Button>(true);

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
            }

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

            menuButtonsInitialized = shopButton != null || settingsButton != null;
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

        private void EnsureJoinWorldUI()
        {
            if (joinWorldUIInitialized)
                return;

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
            placeholderText.text = DefaultJoinAddress;

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

            worldAddressLabel.color = worldAddressLabelDefaultColor;
            ClearJoinStatus();

            if (NetworkManager.Singleton.IsHost)
            {
                string addressLabel = WorldGameSessionManager.instance.HasRelayJoinCode()
                    ? "YOUR RELAY CODE"
                    : "YOUR LOCAL ADDRESS";

                worldAddressLabel.text = $"{addressLabel}\n<size=140%>{WorldGameSessionManager.instance.GetCurrentConnectionAddress()}</size>\n<size=75%>Send this to the other player.</size>";
            }
            else if (NetworkManager.Singleton.IsClient)
            {
                worldAddressLabel.text = "JOIN ANOTHER WORLD\n<size=75%>Enter a Relay code, or use IP for LAN.</size>";
            }
            else
            {
                worldAddressLabel.text = "WORLD ADDRESS\n<size=75%>Enter a Relay code, or use IP for LAN.</size>";
            }

            RefreshJoinControlsState();
        }

        private async void CheckRelayCodeFromCharacterMenu()
        {
            if (isCheckingRelayCode || isJoiningWorld)
                return;

            string addressInput = GetJoinAddressInput();

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

            if (IsRelayCodeInput(addressInput))
            {
                joinWorldButton.interactable = IsCurrentRelayCodeVerified(addressInput);
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
