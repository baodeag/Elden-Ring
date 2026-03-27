using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace baodeag
{
    public class TitleScreenSettingsMenuView : MonoBehaviour
    {
        [SerializeField] private RectTransform contentRoot;

        private TitleScreenManager owner;
        private bool hasBuiltLayout;

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

        public void Initialize(TitleScreenManager manager)
        {
            owner = manager;
            EnsureLayout();
            Refresh();
        }

        public void Refresh()
        {
            EnsureLayout();

            if (!GameSettingsManager.HasInstance)
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

        public void CloseMenu()
        {
            if (owner != null)
                owner.CloseSettingsMenu();
        }

        private void EnsureLayout()
        {
            if (hasBuiltLayout || contentRoot == null)
                return;

            ClearChildren(contentRoot);

            Button buttonTemplate = FindFirstObjectByType<Button>(FindObjectsInactive.Include);
            Slider sliderTemplate = FindFirstObjectByType<Slider>(FindObjectsInactive.Include);
            CreateContentBackground();

            CreateHeader("SETTINGS", new Vector2(0f, -55f), 40f);

            float rowStartY = -150f;
            float rowSpacing = 92f;

            CreateSliderRow("MASTER VOLUME", rowStartY, out masterVolumeSlider, out masterVolumeValueText, OnMasterVolumeChanged);
            CreateSliderRow("MUSIC VOLUME", rowStartY - rowSpacing, out musicVolumeSlider, out musicVolumeValueText, OnMusicVolumeChanged);
            CreateSliderRow("SFX VOLUME", rowStartY - (rowSpacing * 2f), out sfxVolumeSlider, out sfxVolumeValueText, OnSFXVolumeChanged);
            CreateSliderRow("CAMERA SENSITIVITY", rowStartY - (rowSpacing * 3f), out cameraSensitivitySlider, out cameraSensitivityValueText, OnCameraSensitivityChanged, 0.3f, 2f);

            CreateSelectionRow("FULLSCREEN", rowStartY - (rowSpacing * 4f), out fullscreenValueText, ToggleFullscreen, null);
            CreateSelectionRow("RESOLUTION", rowStartY - (rowSpacing * 5f), out resolutionValueText, () => CycleResolution(-1), () => CycleResolution(1));
            CreateSelectionRow("QUALITY", rowStartY - (rowSpacing * 6f), out qualityValueText, () => CycleQuality(-1), () => CycleQuality(1));

            CreateActionButton("CLOSE", new Vector2(0f, 70f), CloseMenu);

            // If the scene has a hidden slider template, use its look; otherwise use our generated fallback.
            if (sliderTemplate != null)
            {
                CopySliderVisuals(sliderTemplate, masterVolumeSlider);
                CopySliderVisuals(sliderTemplate, musicVolumeSlider);
                CopySliderVisuals(sliderTemplate, sfxVolumeSlider);
                CopySliderVisuals(sliderTemplate, cameraSensitivitySlider);
            }

            // If the scene has an existing button, borrow its colors.
            if (buttonTemplate != null)
            {
                ApplyButtonColorsToChildren(contentRoot, buttonTemplate.colors);
            }

            hasBuiltLayout = true;
        }

        private void CreateHeader(string label, Vector2 anchoredPosition, float fontSize)
        {
            TextMeshProUGUI headerText = CreateText("Settings Title", label, contentRoot, new Vector2(840f, 60f), anchoredPosition, TextAlignmentOptions.Center, fontSize);
            headerText.rectTransform.anchorMin = new Vector2(0.5f, 1f);
            headerText.rectTransform.anchorMax = new Vector2(0.5f, 1f);
            headerText.rectTransform.pivot = new Vector2(0.5f, 1f);
        }

        private void CreateContentBackground()
        {
            GameObject panelObject = new GameObject("Settings Content Background", typeof(RectTransform), typeof(Image));
            RectTransform panelRect = panelObject.GetComponent<RectTransform>();
            Image panelImage = panelObject.GetComponent<Image>();

            panelRect.SetParent(contentRoot, false);
            panelRect.anchorMin = new Vector2(0.5f, 0.5f);
            panelRect.anchorMax = new Vector2(0.5f, 0.5f);
            panelRect.pivot = new Vector2(0.5f, 0.5f);
            panelRect.anchoredPosition = new Vector2(0f, -20f);
            panelRect.sizeDelta = new Vector2(1180f, 820f);

            panelImage.color = new Color(0f, 0f, 0f, 0.82f);
            panelImage.raycastTarget = false;
        }

        private void CreateSliderRow(
            string label,
            float anchoredY,
            out Slider slider,
            out TextMeshProUGUI valueText,
            UnityAction<float> callback,
            float minValue = 0f,
            float maxValue = 1f)
        {
            RectTransform rowRect = CreateRowRoot($"{label} Row", anchoredY);

            TextMeshProUGUI labelText = CreateText($"{label} Label", label, rowRect, new Vector2(340f, 42f), new Vector2(-470f, -12f), TextAlignmentOptions.Left, 24f);
            labelText.rectTransform.anchorMin = new Vector2(0f, 1f);
            labelText.rectTransform.anchorMax = new Vector2(0f, 1f);
            labelText.rectTransform.pivot = new Vector2(0f, 1f);

            slider = CreateSlider($"{label} Slider", rowRect, new Vector2(450f, 30f), new Vector2(-60f, -8f));
            slider.minValue = minValue;
            slider.maxValue = maxValue;
            slider.wholeNumbers = false;
            slider.onValueChanged.AddListener(callback);

            valueText = CreateText($"{label} Value", string.Empty, rowRect, new Vector2(140f, 42f), new Vector2(415f, -12f), TextAlignmentOptions.Right, 22f);
            valueText.rectTransform.anchorMin = new Vector2(0f, 1f);
            valueText.rectTransform.anchorMax = new Vector2(0f, 1f);
            valueText.rectTransform.pivot = new Vector2(0f, 1f);
        }

        private void CreateSelectionRow(
            string label,
            float anchoredY,
            out TextMeshProUGUI valueText,
            UnityAction primaryAction,
            UnityAction secondaryAction)
        {
            RectTransform rowRect = CreateRowRoot($"{label} Row", anchoredY);

            TextMeshProUGUI labelText = CreateText($"{label} Label", label, rowRect, new Vector2(340f, 42f), new Vector2(-470f, -12f), TextAlignmentOptions.Left, 24f);
            labelText.rectTransform.anchorMin = new Vector2(0f, 1f);
            labelText.rectTransform.anchorMax = new Vector2(0f, 1f);
            labelText.rectTransform.pivot = new Vector2(0f, 1f);

            if (secondaryAction == null)
            {
                valueText = CreateText($"{label} Value", string.Empty, rowRect, new Vector2(160f, 42f), new Vector2(-40f, -12f), TextAlignmentOptions.Center, 22f);
                valueText.rectTransform.anchorMin = new Vector2(0f, 1f);
                valueText.rectTransform.anchorMax = new Vector2(0f, 1f);
                valueText.rectTransform.pivot = new Vector2(0f, 1f);

                CreateButton("Toggle Button", "TOGGLE", rowRect, new Vector2(220f, 52f), new Vector2(210f, -4f), new Vector2(0f, 1f), new Vector2(0f, 1f), new Vector2(0f, 1f), primaryAction);
                return;
            }

            valueText = CreateText($"{label} Value", string.Empty, rowRect, new Vector2(200f, 42f), new Vector2(170f, -12f), TextAlignmentOptions.Center, 22f);
            valueText.rectTransform.anchorMin = new Vector2(0f, 1f);
            valueText.rectTransform.anchorMax = new Vector2(0f, 1f);
            valueText.rectTransform.pivot = new Vector2(0f, 1f);

            CreateButton("Previous Button", "<", rowRect, new Vector2(82f, 52f), new Vector2(55f, -4f), new Vector2(0f, 1f), new Vector2(0f, 1f), new Vector2(0f, 1f), primaryAction);
            CreateButton("Next Button", ">", rowRect, new Vector2(82f, 52f), new Vector2(345f, -4f), new Vector2(0f, 1f), new Vector2(0f, 1f), new Vector2(0f, 1f), secondaryAction);
        }

        private void CreateActionButton(string label, Vector2 anchoredPosition, UnityAction action)
        {
            CreateButton($"{label} Button", label, contentRoot, new Vector2(340f, 62f), anchoredPosition, new Vector2(0.5f, 0f), new Vector2(0.5f, 0f), new Vector2(0.5f, 0f), action);
        }

        private RectTransform CreateRowRoot(string name, float anchoredY)
        {
            GameObject rowObject = new GameObject(name, typeof(RectTransform));
            RectTransform rowRect = rowObject.GetComponent<RectTransform>();
            rowRect.SetParent(contentRoot, false);
            rowRect.anchorMin = new Vector2(0.5f, 1f);
            rowRect.anchorMax = new Vector2(0.5f, 1f);
            rowRect.pivot = new Vector2(0.5f, 1f);
            rowRect.anchoredPosition = new Vector2(0f, anchoredY);
            rowRect.sizeDelta = new Vector2(1060f, 78f);
            return rowRect;
        }

        private Button CreateButton(
            string name,
            string label,
            RectTransform parent,
            Vector2 size,
            Vector2 anchoredPosition,
            Vector2 anchorMin,
            Vector2 anchorMax,
            Vector2 pivot,
            UnityAction action)
        {
            GameObject buttonObject = new GameObject(name, typeof(RectTransform), typeof(Image), typeof(Button));
            RectTransform buttonRect = buttonObject.GetComponent<RectTransform>();
            Image image = buttonObject.GetComponent<Image>();
            Button button = buttonObject.GetComponent<Button>();

            buttonRect.SetParent(parent, false);
            buttonRect.anchorMin = anchorMin;
            buttonRect.anchorMax = anchorMax;
            buttonRect.pivot = pivot;
            buttonRect.anchoredPosition = anchoredPosition;
            buttonRect.sizeDelta = size;

            image.color = new Color(0.22f, 0.22f, 0.22f, 1f);
            button.targetGraphic = image;
            button.onClick.AddListener(action);

            TextMeshProUGUI buttonText = CreateText("Text (TMP)", label, buttonRect, new Vector2(-20f, -10f), Vector2.zero, TextAlignmentOptions.Center, 24f);
            buttonText.rectTransform.anchorMin = Vector2.zero;
            buttonText.rectTransform.anchorMax = Vector2.one;
            buttonText.rectTransform.pivot = new Vector2(0.5f, 0.5f);
            buttonText.rectTransform.sizeDelta = new Vector2(-20f, -10f);

            return button;
        }

        private Slider CreateSlider(string name, RectTransform parent, Vector2 size, Vector2 anchoredPosition)
        {
            GameObject sliderObject = new GameObject(name, typeof(RectTransform), typeof(Slider));
            RectTransform sliderRect = sliderObject.GetComponent<RectTransform>();
            Slider slider = sliderObject.GetComponent<Slider>();

            sliderRect.SetParent(parent, false);
            sliderRect.anchorMin = new Vector2(0f, 1f);
            sliderRect.anchorMax = new Vector2(0f, 1f);
            sliderRect.pivot = new Vector2(0f, 1f);
            sliderRect.anchoredPosition = anchoredPosition;
            sliderRect.sizeDelta = size;

            GameObject background = new GameObject("Background", typeof(RectTransform), typeof(Image));
            RectTransform backgroundRect = background.GetComponent<RectTransform>();
            Image backgroundImage = background.GetComponent<Image>();
            backgroundRect.SetParent(sliderRect, false);
            backgroundRect.anchorMin = new Vector2(0f, 0.25f);
            backgroundRect.anchorMax = new Vector2(1f, 0.75f);
            backgroundRect.offsetMin = Vector2.zero;
            backgroundRect.offsetMax = Vector2.zero;
            backgroundImage.color = new Color(0.22f, 0.22f, 0.22f, 1f);

            GameObject fillArea = new GameObject("Fill Area", typeof(RectTransform));
            RectTransform fillAreaRect = fillArea.GetComponent<RectTransform>();
            fillAreaRect.SetParent(sliderRect, false);
            fillAreaRect.anchorMin = Vector2.zero;
            fillAreaRect.anchorMax = Vector2.one;
            fillAreaRect.offsetMin = new Vector2(10f, 0f);
            fillAreaRect.offsetMax = new Vector2(-10f, 0f);

            GameObject fill = new GameObject("Fill", typeof(RectTransform), typeof(Image));
            RectTransform fillRect = fill.GetComponent<RectTransform>();
            Image fillImage = fill.GetComponent<Image>();
            fillRect.SetParent(fillAreaRect, false);
            fillRect.anchorMin = new Vector2(0f, 0f);
            fillRect.anchorMax = new Vector2(1f, 1f);
            fillRect.offsetMin = Vector2.zero;
            fillRect.offsetMax = Vector2.zero;
            fillImage.color = Color.red;

            GameObject handleArea = new GameObject("Handle Slide Area", typeof(RectTransform));
            RectTransform handleAreaRect = handleArea.GetComponent<RectTransform>();
            handleAreaRect.SetParent(sliderRect, false);
            handleAreaRect.anchorMin = Vector2.zero;
            handleAreaRect.anchorMax = Vector2.one;
            handleAreaRect.offsetMin = new Vector2(10f, 0f);
            handleAreaRect.offsetMax = new Vector2(-10f, 0f);

            GameObject handle = new GameObject("Handle", typeof(RectTransform), typeof(Image));
            RectTransform handleRect = handle.GetComponent<RectTransform>();
            Image handleImage = handle.GetComponent<Image>();
            handleRect.SetParent(handleAreaRect, false);
            handleRect.sizeDelta = new Vector2(20f, 40f);
            handleImage.color = Color.red;

            slider.fillRect = fillRect;
            slider.handleRect = handleRect;
            slider.targetGraphic = handleImage;
            slider.direction = Slider.Direction.LeftToRight;

            return slider;
        }

        private TextMeshProUGUI CreateText(
            string name,
            string value,
            RectTransform parent,
            Vector2 size,
            Vector2 anchoredPosition,
            TextAlignmentOptions alignment,
            float fontSize)
        {
            GameObject textObject = new GameObject(name, typeof(RectTransform), typeof(TextMeshProUGUI));
            RectTransform textRect = textObject.GetComponent<RectTransform>();
            TextMeshProUGUI text = textObject.GetComponent<TextMeshProUGUI>();

            textRect.SetParent(parent, false);
            textRect.sizeDelta = size;
            textRect.anchoredPosition = anchoredPosition;

            text.font = TMP_Settings.defaultFontAsset;
            text.fontSharedMaterial = text.font != null ? text.font.material : null;
            text.text = value;
            text.fontSize = fontSize;
            text.color = new Color(1f, 0.8272578f, 0f, 1f);
            text.alignment = alignment;
            text.textWrappingMode = TextWrappingModes.NoWrap;

            return text;
        }

        private void ClearChildren(RectTransform parent)
        {
            for (int i = parent.childCount - 1; i >= 0; i--)
            {
                Destroy(parent.GetChild(i).gameObject);
            }
        }

        private void ApplyButtonColorsToChildren(RectTransform parent, ColorBlock colors)
        {
            Button[] buttons = parent.GetComponentsInChildren<Button>(true);

            for (int i = 0; i < buttons.Length; i++)
            {
                buttons[i].colors = colors;
            }
        }

        private void CopySliderVisuals(Slider template, Slider target)
        {
            if (template == null || target == null)
                return;

            Image templateBackground = template.GetComponentInChildren<Image>(true);
            Image[] targetImages = target.GetComponentsInChildren<Image>(true);

            if (templateBackground != null && targetImages.Length > 0)
                targetImages[0].color = templateBackground.color;
        }

        private void OnMasterVolumeChanged(float value)
        {
            GameSettingsManager.Instance.SetMasterVolume(value);
            Refresh();
        }

        private void OnMusicVolumeChanged(float value)
        {
            GameSettingsManager.Instance.SetMusicVolume(value);
            Refresh();
        }

        private void OnSFXVolumeChanged(float value)
        {
            GameSettingsManager.Instance.SetSFXVolume(value);
            Refresh();
        }

        private void OnCameraSensitivityChanged(float value)
        {
            GameSettingsManager.Instance.SetCameraSensitivity(value);
            Refresh();
        }

        private void ToggleFullscreen()
        {
            GameSettingsManager.Instance.ToggleFullscreen();
            Refresh();
        }

        private void CycleResolution(int direction)
        {
            GameSettingsManager.Instance.CycleResolution(direction);
            Refresh();
        }

        private void CycleQuality(int direction)
        {
            GameSettingsManager.Instance.CycleQuality(direction);
            Refresh();
        }
    }
}
