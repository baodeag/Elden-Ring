using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

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
        private TMP_InputField joinWorldAddressInputField;
        private Button joinWorldButton;
        private Button shopButton;
        private Button settingsButton;

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
            joinWorldButton = CreateJoinWorldButton(joinWorldControlsRoot, buttonTemplate);

            joinWorldUIInitialized = true;
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
            controlsRoot.sizeDelta = new Vector2(430f, 270f);

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

            return inputField;
        }

        private Button CreateJoinWorldButton(RectTransform parent, Button buttonTemplate)
        {
            GameObject buttonObject = Object.Instantiate(buttonTemplate.gameObject, parent);
            buttonObject.name = "Join World Button";
            buttonObject.SetActive(true);

            RectTransform buttonRect = buttonObject.GetComponent<RectTransform>();
            buttonRect.anchorMin = new Vector2(0f, 1f);
            buttonRect.anchorMax = new Vector2(1f, 1f);
            buttonRect.pivot = new Vector2(0.5f, 1f);
            buttonRect.anchoredPosition = new Vector2(0f, -202f);
            buttonRect.sizeDelta = new Vector2(-32f, 60f);

            Button button = buttonObject.GetComponent<Button>();
            button.onClick.RemoveAllListeners();
            button.onClick.AddListener(JoinWorldFromCharacterMenu);

            TextMeshProUGUI buttonText = button.GetComponentInChildren<TextMeshProUGUI>(true);

            if (buttonText != null)
            {
                buttonText.text = "JOIN WORLD";
            }

            return button;
        }

        private void RefreshJoinWorldUI()
        {
            if (!joinWorldUIInitialized || worldAddressLabel == null)
                return;

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
        }

        private void JoinWorldFromCharacterMenu()
        {
            string addressInput = joinWorldAddressInputField != null && !string.IsNullOrWhiteSpace(joinWorldAddressInputField.text)
                ? joinWorldAddressInputField.text
                : "127.0.0.1:7777";

            PlayerUIManager.instance.CloseAllMenuWindows();
            WorldGameSessionManager.instance.StartGameAsClient(addressInput);
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
