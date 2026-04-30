using TMPro;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace baodeag
{
    public class TitleScreenSettingsMenuManager : MonoBehaviour
    {
        private TitleScreenManager titleScreenManager;
        private GameObject titleScreenMainMenu;
        private Button mainMenuLoadGameButton;
        private Button mainMenuNewGameButton;
        private Button settingsButton;
        private RectTransform menuParent;

        private bool hasInitialized;
        private bool settingsMenuBuilt;
        private bool settingsMenuIsOpen;
        private GameObject settingsMenuRoot;
        private Slider masterVolumeSlider;
        private Slider musicVolumeSlider;
        private Slider sfxVolumeSlider;
        private Slider cameraSensitivitySlider;
        private TextMeshProUGUI fullscreenValueText;
        private TextMeshProUGUI resolutionValueText;
        private TextMeshProUGUI qualityValueText;
        private TextMeshProUGUI masterVolumeValueText;
        private TextMeshProUGUI musicVolumeValueText;
        private TextMeshProUGUI sfxVolumeValueText;
        private TextMeshProUGUI cameraSensitivityValueText;
        private readonly List<GameObject> hiddenSiblingMenus = new List<GameObject>();
        private readonly List<bool> hiddenSiblingMenuStates = new List<bool>();

        public void Initialize(TitleScreenManager owner, GameObject mainMenuRoot, Button loadButton, Button newGameButton, Button settingsMenuButton)
        {
            if (hasInitialized)
                return;

            titleScreenManager = owner;
            titleScreenMainMenu = mainMenuRoot;
            mainMenuLoadGameButton = loadButton;
            mainMenuNewGameButton = newGameButton;
            settingsButton = settingsMenuButton;
            menuParent = titleScreenMainMenu != null ? titleScreenMainMenu.transform.parent as RectTransform : null;

            if (GameSettingsManager.Instance != null)
                GameSettingsManager.Instance.InitializeIfNeeded();
            hasInitialized = true;
        }

        private void Update()
        {
            if (!settingsMenuIsOpen)
                return;

            if (Input.GetKeyDown(KeyCode.Escape))
                CloseSettingsMenu();
        }

        public void OpenSettingsMenu()
        {
            if (!settingsMenuBuilt)
                BuildSettingsMenu();

            HideSiblingMenus();
            RefreshSettingsDisplay();
            settingsMenuRoot.SetActive(true);
            settingsMenuIsOpen = true;
        }

        public void CloseSettingsMenu()
        {
            if (settingsMenuRoot != null)
                settingsMenuRoot.SetActive(false);

            RestoreSiblingMenus();
            settingsMenuIsOpen = false;

            if (settingsButton != null)
                settingsButton.Select();
        }

        private void BuildSettingsMenu()
        {
            if (titleScreenMainMenu == null || mainMenuLoadGameButton == null)
                return;

            if (menuParent == null)
                return;

            Button buttonTemplate = mainMenuLoadGameButton;
            TextMeshProUGUI textTemplate = buttonTemplate.GetComponentInChildren<TextMeshProUGUI>(true);
            Slider sliderTemplate = titleScreenMainMenu.transform.root.GetComponentInChildren<Slider>(true);

            if (textTemplate == null || sliderTemplate == null)
                return;

            settingsMenuRoot = new GameObject("Title Screen Settings Menu", typeof(RectTransform), typeof(CanvasRenderer), typeof(Image));
            RectTransform rootRect = settingsMenuRoot.GetComponent<RectTransform>();
            Image background = settingsMenuRoot.GetComponent<Image>();

            rootRect.SetParent(menuParent, false);
            rootRect.SetAsLastSibling();
            rootRect.anchorMin = Vector2.zero;
            rootRect.anchorMax = Vector2.one;
            rootRect.offsetMin = Vector2.zero;
            rootRect.offsetMax = Vector2.zero;
            rootRect.pivot = new Vector2(0.5f, 0.5f);
            background.color = new Color(0f, 0f, 0f, 0.98f);

            GameObject contentObject = new GameObject("Settings Content", typeof(RectTransform));
            RectTransform contentRect = contentObject.GetComponent<RectTransform>();
            contentRect.SetParent(rootRect, false);
            contentRect.anchorMin = new Vector2(0.5f, 0.5f);
            contentRect.anchorMax = new Vector2(0.5f, 0.5f);
            contentRect.pivot = new Vector2(0.5f, 0.5f);
            contentRect.sizeDelta = new Vector2(960f, 760f);
            contentRect.anchoredPosition = Vector2.zero;

            CreateHeader(contentRect, textTemplate, "SETTINGS", new Vector2(0f, -40f), 38f);

            float rowStartY = -130f;
            float rowSpacing = 82f;

            CreateSliderRow(contentRect, textTemplate, sliderTemplate, "MASTER VOLUME", rowStartY, out masterVolumeSlider, out masterVolumeValueText, OnMasterVolumeChanged);
            CreateSliderRow(contentRect, textTemplate, sliderTemplate, "MUSIC VOLUME", rowStartY - rowSpacing, out musicVolumeSlider, out musicVolumeValueText, OnMusicVolumeChanged);
            CreateSliderRow(contentRect, textTemplate, sliderTemplate, "SFX VOLUME", rowStartY - (rowSpacing * 2f), out sfxVolumeSlider, out sfxVolumeValueText, OnSFXVolumeChanged);
            CreateSliderRow(contentRect, textTemplate, sliderTemplate, "CAMERA SENSITIVITY", rowStartY - (rowSpacing * 3f), out cameraSensitivitySlider, out cameraSensitivityValueText, OnCameraSensitivityChanged, 0.3f, 2f);

            CreateSelectionRow(contentRect, textTemplate, buttonTemplate, "FULLSCREEN", rowStartY - (rowSpacing * 4f), out fullscreenValueText, ToggleFullscreen, null);
            CreateSelectionRow(contentRect, textTemplate, buttonTemplate, "RESOLUTION", rowStartY - (rowSpacing * 5f), out resolutionValueText, () => CycleResolution(-1), () => CycleResolution(1));
            CreateSelectionRow(contentRect, textTemplate, buttonTemplate, "QUALITY", rowStartY - (rowSpacing * 6f), out qualityValueText, () => CycleQuality(-1), () => CycleQuality(1));

            Button closeButton = CreateActionButton(contentRect, buttonTemplate, textTemplate, "CLOSE", new Vector2(0f, 140f));
            closeButton.onClick.RemoveAllListeners();
            closeButton.onClick.AddListener(CloseSettingsMenu);

            settingsMenuRoot.SetActive(false);
            settingsMenuBuilt = true;
        }

        private void HideSiblingMenus()
        {
            if (menuParent == null)
                return;

            hiddenSiblingMenus.Clear();
            hiddenSiblingMenuStates.Clear();

            for (int i = 0; i < menuParent.childCount; i++)
            {
                GameObject childObject = menuParent.GetChild(i).gameObject;

                if (childObject == settingsMenuRoot)
                    continue;

                hiddenSiblingMenus.Add(childObject);
                hiddenSiblingMenuStates.Add(childObject.activeSelf);
                childObject.SetActive(false);
            }
        }

        private void RestoreSiblingMenus()
        {
            for (int i = 0; i < hiddenSiblingMenus.Count; i++)
            {
                if (hiddenSiblingMenus[i] == null)
                    continue;

                hiddenSiblingMenus[i].SetActive(hiddenSiblingMenuStates[i]);
            }
        }

        private void CreateHeader(RectTransform parent, TextMeshProUGUI textTemplate, string label, Vector2 anchoredPosition, float fontSize)
        {
            GameObject headerObject = new GameObject($"{label} Header", typeof(RectTransform), typeof(CanvasRenderer), typeof(TextMeshProUGUI));
            RectTransform headerRect = headerObject.GetComponent<RectTransform>();
            TextMeshProUGUI headerText = headerObject.GetComponent<TextMeshProUGUI>();

            headerRect.SetParent(parent, false);
            headerRect.anchorMin = new Vector2(0.5f, 1f);
            headerRect.anchorMax = new Vector2(0.5f, 1f);
            headerRect.pivot = new Vector2(0.5f, 1f);
            headerRect.anchoredPosition = anchoredPosition;
            headerRect.sizeDelta = new Vector2(840f, 60f);

            CopyTextStyle(textTemplate, headerText);
            headerText.alignment = TextAlignmentOptions.Center;
            headerText.fontSize = fontSize;
            headerText.text = label;
        }

        private void CreateSliderRow(
            RectTransform parent,
            TextMeshProUGUI textTemplate,
            Slider sliderTemplate,
            string label,
            float anchoredY,
            out Slider slider,
            out TextMeshProUGUI valueText,
            UnityEngine.Events.UnityAction<float> callback,
            float minValue = 0f,
            float maxValue = 1f)
        {
            GameObject rowObject = new GameObject($"{label} Row", typeof(RectTransform));
            RectTransform rowRect = rowObject.GetComponent<RectTransform>();
            rowRect.SetParent(parent, false);
            rowRect.anchorMin = new Vector2(0.5f, 1f);
            rowRect.anchorMax = new Vector2(0.5f, 1f);
            rowRect.pivot = new Vector2(0.5f, 1f);
            rowRect.anchoredPosition = new Vector2(0f, anchoredY);
            rowRect.sizeDelta = new Vector2(860f, 70f);

            TextMeshProUGUI labelText = CreateRowLabel(rowRect, textTemplate, label, new Vector2(-320f, -12f), 24f, TextAlignmentOptions.Left);
            labelText.rectTransform.sizeDelta = new Vector2(300f, 42f);

            GameObject sliderObject = Instantiate(sliderTemplate.gameObject, rowRect);
            sliderObject.name = $"{label} Slider";
            sliderObject.SetActive(true);

            slider = sliderObject.GetComponent<Slider>();
            slider.onValueChanged.RemoveAllListeners();

            RectTransform sliderRect = sliderObject.GetComponent<RectTransform>();
            sliderRect.anchorMin = new Vector2(0.5f, 1f);
            sliderRect.anchorMax = new Vector2(0.5f, 1f);
            sliderRect.pivot = new Vector2(0.5f, 1f);
            sliderRect.anchoredPosition = new Vector2(40f, -8f);
            sliderRect.sizeDelta = new Vector2(360f, 30f);

            slider.minValue = minValue;
            slider.maxValue = maxValue;
            slider.wholeNumbers = false;
            slider.onValueChanged.AddListener(callback);

            valueText = CreateRowLabel(rowRect, textTemplate, string.Empty, new Vector2(330f, -12f), 22f, TextAlignmentOptions.Right);
            valueText.rectTransform.sizeDelta = new Vector2(120f, 42f);
        }

        private void CreateSelectionRow(
            RectTransform parent,
            TextMeshProUGUI textTemplate,
            Button buttonTemplate,
            string label,
            float anchoredY,
            out TextMeshProUGUI valueText,
            UnityEngine.Events.UnityAction primaryAction,
            UnityEngine.Events.UnityAction secondaryAction)
        {
            GameObject rowObject = new GameObject($"{label} Row", typeof(RectTransform));
            RectTransform rowRect = rowObject.GetComponent<RectTransform>();
            rowRect.SetParent(parent, false);
            rowRect.anchorMin = new Vector2(0.5f, 1f);
            rowRect.anchorMax = new Vector2(0.5f, 1f);
            rowRect.pivot = new Vector2(0.5f, 1f);
            rowRect.anchoredPosition = new Vector2(0f, anchoredY);
            rowRect.sizeDelta = new Vector2(860f, 70f);

            TextMeshProUGUI labelText = CreateRowLabel(rowRect, textTemplate, label, new Vector2(-320f, -12f), 24f, TextAlignmentOptions.Left);
            labelText.rectTransform.sizeDelta = new Vector2(300f, 42f);

            if (secondaryAction == null)
            {
                Button toggleButton = CreateMiniButton(rowRect, buttonTemplate, textTemplate, "TOGGLE", new Vector2(215f, -4f), new Vector2(220f, 50f));
                toggleButton.onClick.RemoveAllListeners();
                toggleButton.onClick.AddListener(primaryAction);

                valueText = CreateRowLabel(rowRect, textTemplate, string.Empty, new Vector2(10f, -12f), 22f, TextAlignmentOptions.Center);
                valueText.rectTransform.sizeDelta = new Vector2(180f, 42f);
                return;
            }

            Button previousButton = CreateMiniButton(rowRect, buttonTemplate, textTemplate, "<", new Vector2(120f, -4f), new Vector2(80f, 50f));
            previousButton.onClick.RemoveAllListeners();
            previousButton.onClick.AddListener(primaryAction);

            Button nextButton = CreateMiniButton(rowRect, buttonTemplate, textTemplate, ">", new Vector2(340f, -4f), new Vector2(80f, 50f));
            nextButton.onClick.RemoveAllListeners();
            nextButton.onClick.AddListener(secondaryAction);

            valueText = CreateRowLabel(rowRect, textTemplate, string.Empty, new Vector2(230f, -12f), 22f, TextAlignmentOptions.Center);
            valueText.rectTransform.sizeDelta = new Vector2(180f, 42f);
        }

        private Button CreateActionButton(RectTransform parent, Button buttonTemplate, TextMeshProUGUI textTemplate, string label, Vector2 anchoredPosition)
        {
            GameObject buttonObject = Instantiate(buttonTemplate.gameObject, parent);
            buttonObject.name = $"{label} Button";
            buttonObject.SetActive(true);

            RectTransform buttonRect = buttonObject.GetComponent<RectTransform>();
            buttonRect.anchorMin = new Vector2(0.5f, 0f);
            buttonRect.anchorMax = new Vector2(0.5f, 0f);
            buttonRect.pivot = new Vector2(0.5f, 0f);
            buttonRect.anchoredPosition = anchoredPosition;
            buttonRect.sizeDelta = new Vector2(320f, 60f);

            TextMeshProUGUI buttonText = buttonObject.GetComponentInChildren<TextMeshProUGUI>(true);

            if (buttonText != null)
            {
                CopyTextStyle(textTemplate, buttonText);
                buttonText.alignment = TextAlignmentOptions.Center;
                buttonText.text = label;
            }

            return buttonObject.GetComponent<Button>();
        }

        private Button CreateMiniButton(RectTransform parent, Button buttonTemplate, TextMeshProUGUI textTemplate, string label, Vector2 anchoredPosition, Vector2 size)
        {
            GameObject buttonObject = Instantiate(buttonTemplate.gameObject, parent);
            buttonObject.name = $"{label} Button";
            buttonObject.SetActive(true);

            RectTransform buttonRect = buttonObject.GetComponent<RectTransform>();
            buttonRect.anchorMin = new Vector2(0f, 1f);
            buttonRect.anchorMax = new Vector2(0f, 1f);
            buttonRect.pivot = new Vector2(0f, 1f);
            buttonRect.anchoredPosition = anchoredPosition;
            buttonRect.sizeDelta = size;

            TextMeshProUGUI buttonText = buttonObject.GetComponentInChildren<TextMeshProUGUI>(true);

            if (buttonText != null)
            {
                CopyTextStyle(textTemplate, buttonText);
                buttonText.alignment = TextAlignmentOptions.Center;
                buttonText.fontSize = 24f;
                buttonText.text = label;
            }

            return buttonObject.GetComponent<Button>();
        }

        private TextMeshProUGUI CreateRowLabel(RectTransform parent, TextMeshProUGUI textTemplate, string label, Vector2 anchoredPosition, float fontSize, TextAlignmentOptions alignment)
        {
            GameObject labelObject = new GameObject($"{label} Label", typeof(RectTransform), typeof(CanvasRenderer), typeof(TextMeshProUGUI));
            RectTransform labelRect = labelObject.GetComponent<RectTransform>();
            TextMeshProUGUI labelText = labelObject.GetComponent<TextMeshProUGUI>();

            labelRect.SetParent(parent, false);
            labelRect.anchorMin = new Vector2(0f, 1f);
            labelRect.anchorMax = new Vector2(0f, 1f);
            labelRect.pivot = new Vector2(0f, 1f);
            labelRect.anchoredPosition = anchoredPosition;
            labelRect.sizeDelta = new Vector2(240f, 42f);

            CopyTextStyle(textTemplate, labelText);
            labelText.fontSize = fontSize;
            labelText.alignment = alignment;
            labelText.text = label;

            return labelText;
        }

        private void RefreshSettingsDisplay()
        {
            if (!settingsMenuBuilt || !GameSettingsManager.HasInstance)
                return;

            GameSettingsManager settings = GameSettingsManager.Instance;

            if (masterVolumeSlider != null)
                masterVolumeSlider.SetValueWithoutNotify(settings.masterVolume);

            if (musicVolumeSlider != null)
                musicVolumeSlider.SetValueWithoutNotify(settings.musicVolume);

            if (sfxVolumeSlider != null)
                sfxVolumeSlider.SetValueWithoutNotify(settings.sfxVolume);

            if (cameraSensitivitySlider != null)
                cameraSensitivitySlider.SetValueWithoutNotify(settings.cameraSensitivity);

            if (masterVolumeValueText != null)
                masterVolumeValueText.text = $"{Mathf.RoundToInt(settings.masterVolume * 100f)}%";

            if (musicVolumeValueText != null)
                musicVolumeValueText.text = $"{Mathf.RoundToInt(settings.musicVolume * 100f)}%";

            if (sfxVolumeValueText != null)
                sfxVolumeValueText.text = $"{Mathf.RoundToInt(settings.sfxVolume * 100f)}%";

            if (cameraSensitivityValueText != null)
                cameraSensitivityValueText.text = $"x{settings.cameraSensitivity:0.00}";

            if (fullscreenValueText != null)
                fullscreenValueText.text = settings.isFullscreen ? "ON" : "OFF";

            if (resolutionValueText != null)
                resolutionValueText.text = settings.GetCurrentResolutionLabel();

            if (qualityValueText != null)
                qualityValueText.text = settings.GetCurrentQualityLabel();
        }

        private void OnMasterVolumeChanged(float value)
        {
            GameSettingsManager.Instance.SetMasterVolume(value);
            RefreshSettingsDisplay();
        }

        private void OnMusicVolumeChanged(float value)
        {
            GameSettingsManager.Instance.SetMusicVolume(value);
            RefreshSettingsDisplay();
        }

        private void OnSFXVolumeChanged(float value)
        {
            GameSettingsManager.Instance.SetSFXVolume(value);
            RefreshSettingsDisplay();
        }

        private void OnCameraSensitivityChanged(float value)
        {
            GameSettingsManager.Instance.SetCameraSensitivity(value);
            RefreshSettingsDisplay();
        }

        private void ToggleFullscreen()
        {
            GameSettingsManager.Instance.ToggleFullscreen();
            RefreshSettingsDisplay();
        }

        private void CycleResolution(int direction)
        {
            GameSettingsManager.Instance.CycleResolution(direction);
            RefreshSettingsDisplay();
        }

        private void CycleQuality(int direction)
        {
            GameSettingsManager.Instance.CycleQuality(direction);
            RefreshSettingsDisplay();
        }

        private void CopyTextStyle(TextMeshProUGUI source, TextMeshProUGUI destination)
        {
            destination.font = source.font;
            destination.fontSharedMaterial = source.fontSharedMaterial;
            destination.color = source.color;
            destination.richText = source.richText;
            destination.textWrappingMode = source.textWrappingMode;
        }
    }
}
