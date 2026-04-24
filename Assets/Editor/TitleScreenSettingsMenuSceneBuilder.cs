using System.Linq;
using TMPro;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace baodeag.Editor
{
    public static class TitleScreenSettingsMenuSceneBuilder
    {
        private const string ScenePath = "Assets/Scenes/Main_Menu_01.unity";
        private const string SessionKey = "baodeag.TitleScreenSettingsMenuSceneBuilder.Ran";

        static TitleScreenSettingsMenuSceneBuilder()
        {
            // Disabled autorun to avoid opening/saving scenes during editor startup.
        }

        [MenuItem("Tools/UI/Rebuild Title Screen Settings Menu")]
        public static void RebuildViaMenu()
        {
            BuildOrUpdateSceneMenu(forceRebuild: true);
        }

        private static void EnsureSceneAuthoredSettingsMenuOnce()
        {
            if (SessionState.GetBool(SessionKey, false))
                return;

            SessionState.SetBool(SessionKey, true);
            BuildOrUpdateSceneMenu(forceRebuild: false);
        }

        private static void BuildOrUpdateSceneMenu(bool forceRebuild)
        {
            SceneSetup[] previousSetup = EditorSceneManager.GetSceneManagerSetup();
            Scene currentActiveScene = SceneManager.GetActiveScene();

            try
            {
                Scene scene = EditorSceneManager.OpenScene(ScenePath, OpenSceneMode.Single);
                GameObject canvasObject = scene.GetRootGameObjects().FirstOrDefault(go => go.name == "Title Screen Canvas");

                if (canvasObject == null)
                    return;

                TitleScreenManager manager = canvasObject.GetComponent<TitleScreenManager>();

                if (manager == null)
                    return;

                SerializedObject managerSerializedObject = new SerializedObject(manager);
                GameObject settingsMenuObject = managerSerializedObject.FindProperty("titleScreenSettingsMenu").objectReferenceValue as GameObject;

                if (settingsMenuObject == null)
                    return;

                TitleScreenSettingsMenuView settingsView = settingsMenuObject.GetComponent<TitleScreenSettingsMenuView>();

                if (settingsView == null)
                    return;

                SerializedObject viewSerializedObject = new SerializedObject(settingsView);
                RectTransform contentRoot = viewSerializedObject.FindProperty("contentRoot").objectReferenceValue as RectTransform;

                if (contentRoot == null)
                    return;

                bool hasAuthoredRows = contentRoot.Find("Master Volume Row") != null && contentRoot.Find("Close Button") != null;

                if (!forceRebuild && hasAuthoredRows)
                    return;

                ClearChildren(contentRoot);
                BuildAuthoredLayout(contentRoot, viewSerializedObject);

                viewSerializedObject.ApplyModifiedPropertiesWithoutUndo();
                EditorUtility.SetDirty(settingsView);
                EditorUtility.SetDirty(settingsMenuObject);
                EditorSceneManager.MarkSceneDirty(scene);
                EditorSceneManager.SaveScene(scene);
            }
            finally
            {
                if (previousSetup != null && previousSetup.Length > 0)
                {
                    EditorSceneManager.RestoreSceneManagerSetup(previousSetup);

                    if (currentActiveScene.IsValid() && currentActiveScene.isLoaded)
                        SceneManager.SetActiveScene(currentActiveScene);
                }
            }
        }

        private static void BuildAuthoredLayout(RectTransform contentRoot, SerializedObject viewSerializedObject)
        {
            CreatePanelBackground(contentRoot);
            CreateHeader(contentRoot);

            float rowStartY = -150f;
            float rowSpacing = 92f;

            Slider masterSlider = CreateSliderRow(contentRoot, "Master Volume Row", "MASTER VOLUME", rowStartY, out TextMeshProUGUI masterValueText);
            Slider musicSlider = CreateSliderRow(contentRoot, "Music Volume Row", "MUSIC VOLUME", rowStartY - rowSpacing, out TextMeshProUGUI musicValueText);
            Slider sfxSlider = CreateSliderRow(contentRoot, "SFX Volume Row", "SFX VOLUME", rowStartY - (rowSpacing * 2f), out TextMeshProUGUI sfxValueText);
            Slider cameraSlider = CreateSliderRow(contentRoot, "Camera Sensitivity Row", "CAMERA SENSITIVITY", rowStartY - (rowSpacing * 3f), out TextMeshProUGUI cameraValueText, 0.3f, 2f);

            SelectionRow fullscreenRow = CreateSelectionRow(contentRoot, "Fullscreen Row", "FULLSCREEN", rowStartY - (rowSpacing * 4f), true);
            SelectionRow resolutionRow = CreateSelectionRow(contentRoot, "Resolution Row", "RESOLUTION", rowStartY - (rowSpacing * 5f), false);
            SelectionRow qualityRow = CreateSelectionRow(contentRoot, "Quality Row", "QUALITY", rowStartY - (rowSpacing * 6f), false);

            Button closeButton = CreateActionButton(contentRoot, "Close Button", "CLOSE", new Vector2(120f, 70f));

            Assign(viewSerializedObject, "masterVolumeSlider", masterSlider);
            Assign(viewSerializedObject, "musicVolumeSlider", musicSlider);
            Assign(viewSerializedObject, "sfxVolumeSlider", sfxSlider);
            Assign(viewSerializedObject, "cameraSensitivitySlider", cameraSlider);
            Assign(viewSerializedObject, "fullscreenToggleButton", fullscreenRow.primaryButton);
            Assign(viewSerializedObject, "resolutionPreviousButton", resolutionRow.primaryButton);
            Assign(viewSerializedObject, "resolutionNextButton", resolutionRow.secondaryButton);
            Assign(viewSerializedObject, "qualityPreviousButton", qualityRow.primaryButton);
            Assign(viewSerializedObject, "qualityNextButton", qualityRow.secondaryButton);
            Assign(viewSerializedObject, "closeButton", closeButton);
            Assign(viewSerializedObject, "fullscreenValueText", fullscreenRow.valueText);
            Assign(viewSerializedObject, "resolutionValueText", resolutionRow.valueText);
            Assign(viewSerializedObject, "qualityValueText", qualityRow.valueText);
            Assign(viewSerializedObject, "masterVolumeValueText", masterValueText);
            Assign(viewSerializedObject, "musicVolumeValueText", musicValueText);
            Assign(viewSerializedObject, "sfxVolumeValueText", sfxValueText);
            Assign(viewSerializedObject, "cameraSensitivityValueText", cameraValueText);
        }

        private static void Assign(SerializedObject targetObject, string propertyName, Object value)
        {
            SerializedProperty property = targetObject.FindProperty(propertyName);

            if (property != null)
                property.objectReferenceValue = value;
        }

        private static void ClearChildren(RectTransform parent)
        {
            for (int i = parent.childCount - 1; i >= 0; i--)
            {
                Object.DestroyImmediate(parent.GetChild(i).gameObject);
            }
        }

        private static void CreatePanelBackground(RectTransform parent)
        {
            GameObject panel = CreateUIObject("Settings Content Background", parent);
            RectTransform rect = panel.GetComponent<RectTransform>();
            Image image = panel.AddComponent<Image>();

            rect.anchorMin = new Vector2(0.5f, 0.5f);
            rect.anchorMax = new Vector2(0.5f, 0.5f);
            rect.pivot = new Vector2(0.5f, 0.5f);
            rect.anchoredPosition = new Vector2(120f, -20f);
            rect.sizeDelta = new Vector2(1180f, 820f);

            image.color = new Color(0f, 0f, 0f, 0.82f);
            image.raycastTarget = false;
        }

        private static void CreateHeader(RectTransform parent)
        {
            TextMeshProUGUI header = CreateText("Settings Title", parent, "SETTINGS", new Vector2(840f, 60f), new Vector2(120f, -55f), TextAlignmentOptions.Center, 40f);
            RectTransform rect = header.rectTransform;
            rect.anchorMin = new Vector2(0.5f, 1f);
            rect.anchorMax = new Vector2(0.5f, 1f);
            rect.pivot = new Vector2(0.5f, 1f);
        }

        private static Slider CreateSliderRow(
            RectTransform parent,
            string rowName,
            string label,
            float anchoredY,
            out TextMeshProUGUI valueText,
            float minValue = 0f,
            float maxValue = 1f)
        {
            RectTransform row = CreateRowRoot(parent, rowName, anchoredY);
            CreateText("Label", row, label, new Vector2(340f, 42f), new Vector2(-470f, -12f), TextAlignmentOptions.Left, 24f, new Vector2(0f, 1f));
            Slider slider = CreateSlider("Slider", row, new Vector2(450f, 30f), new Vector2(-60f, -8f), minValue, maxValue);
            valueText = CreateText("Value", row, "100%", new Vector2(140f, 42f), new Vector2(415f, -12f), TextAlignmentOptions.Right, 22f, new Vector2(0f, 1f));
            return slider;
        }

        private static SelectionRow CreateSelectionRow(RectTransform parent, string rowName, string label, float anchoredY, bool singleButton)
        {
            RectTransform row = CreateRowRoot(parent, rowName, anchoredY);
            CreateText("Label", row, label, new Vector2(340f, 42f), new Vector2(-470f, -12f), TextAlignmentOptions.Left, 24f, new Vector2(0f, 1f));

            if (singleButton)
            {
                TextMeshProUGUI valueText = CreateText("Value", row, "OFF", new Vector2(160f, 42f), new Vector2(-40f, -12f), TextAlignmentOptions.Center, 22f, new Vector2(0f, 1f));
                Button toggleButton = CreateButton("Primary Button", row, "TOGGLE", new Vector2(220f, 52f), new Vector2(210f, -4f), new Vector2(0f, 1f));
                return new SelectionRow(valueText, toggleButton, null);
            }

            TextMeshProUGUI centeredValueText = CreateText("Value", row, "1280 x 1024", new Vector2(200f, 42f), new Vector2(170f, -12f), TextAlignmentOptions.Center, 22f, new Vector2(0f, 1f));
            Button previousButton = CreateButton("Previous Button", row, "<", new Vector2(82f, 52f), new Vector2(55f, -4f), new Vector2(0f, 1f));
            Button nextButton = CreateButton("Next Button", row, ">", new Vector2(82f, 52f), new Vector2(345f, -4f), new Vector2(0f, 1f));
            return new SelectionRow(centeredValueText, previousButton, nextButton);
        }

        private static Button CreateActionButton(RectTransform parent, string name, string label, Vector2 anchoredPosition)
        {
            return CreateButton(name, parent, label, new Vector2(340f, 62f), anchoredPosition, new Vector2(0.5f, 0f));
        }

        private static RectTransform CreateRowRoot(RectTransform parent, string name, float anchoredY)
        {
            GameObject rowObject = CreateUIObject(name, parent);
            RectTransform rowRect = rowObject.GetComponent<RectTransform>();
            rowRect.anchorMin = new Vector2(0.5f, 1f);
            rowRect.anchorMax = new Vector2(0.5f, 1f);
            rowRect.pivot = new Vector2(0.5f, 1f);
            rowRect.anchoredPosition = new Vector2(120f, anchoredY);
            rowRect.sizeDelta = new Vector2(1060f, 78f);
            return rowRect;
        }

        private static Slider CreateSlider(string name, RectTransform parent, Vector2 size, Vector2 anchoredPosition, float minValue, float maxValue)
        {
            GameObject sliderObject = CreateUIObject(name, parent);
            RectTransform sliderRect = sliderObject.GetComponent<RectTransform>();
            Slider slider = sliderObject.AddComponent<Slider>();

            sliderRect.anchorMin = new Vector2(0f, 1f);
            sliderRect.anchorMax = new Vector2(0f, 1f);
            sliderRect.pivot = new Vector2(0f, 1f);
            sliderRect.anchoredPosition = anchoredPosition;
            sliderRect.sizeDelta = size;

            GameObject backgroundObject = CreateUIObject("Background", sliderRect);
            RectTransform backgroundRect = backgroundObject.GetComponent<RectTransform>();
            Image backgroundImage = backgroundObject.AddComponent<Image>();
            backgroundRect.anchorMin = new Vector2(0f, 0.25f);
            backgroundRect.anchorMax = new Vector2(1f, 0.75f);
            backgroundRect.offsetMin = Vector2.zero;
            backgroundRect.offsetMax = Vector2.zero;
            backgroundImage.color = new Color(0.22f, 0.22f, 0.22f, 1f);

            GameObject fillAreaObject = CreateUIObject("Fill Area", sliderRect);
            RectTransform fillAreaRect = fillAreaObject.GetComponent<RectTransform>();
            fillAreaRect.anchorMin = Vector2.zero;
            fillAreaRect.anchorMax = Vector2.one;
            fillAreaRect.offsetMin = new Vector2(10f, 0f);
            fillAreaRect.offsetMax = new Vector2(-10f, 0f);

            GameObject fillObject = CreateUIObject("Fill", fillAreaRect);
            RectTransform fillRect = fillObject.GetComponent<RectTransform>();
            Image fillImage = fillObject.AddComponent<Image>();
            fillRect.anchorMin = Vector2.zero;
            fillRect.anchorMax = Vector2.one;
            fillRect.offsetMin = Vector2.zero;
            fillRect.offsetMax = Vector2.zero;
            fillImage.color = Color.red;

            GameObject handleAreaObject = CreateUIObject("Handle Slide Area", sliderRect);
            RectTransform handleAreaRect = handleAreaObject.GetComponent<RectTransform>();
            handleAreaRect.anchorMin = Vector2.zero;
            handleAreaRect.anchorMax = Vector2.one;
            handleAreaRect.offsetMin = new Vector2(10f, 0f);
            handleAreaRect.offsetMax = new Vector2(-10f, 0f);

            GameObject handleObject = CreateUIObject("Handle", handleAreaRect);
            RectTransform handleRect = handleObject.GetComponent<RectTransform>();
            Image handleImage = handleObject.AddComponent<Image>();
            handleRect.sizeDelta = new Vector2(20f, 40f);
            handleImage.color = Color.white;

            ColorBlock colors = slider.colors;
            colors.normalColor = Color.white;
            colors.highlightedColor = new Color(0.96f, 0.96f, 0.96f, 1f);
            colors.pressedColor = new Color(0.784f, 0.784f, 0.784f, 1f);
            colors.selectedColor = new Color(0.96f, 0.96f, 0.96f, 1f);
            colors.disabledColor = new Color(0.784f, 0.784f, 0.784f, 0.5f);
            slider.colors = colors;
            slider.targetGraphic = handleImage;
            slider.fillRect = fillRect;
            slider.handleRect = handleRect;
            slider.direction = Slider.Direction.LeftToRight;
            slider.minValue = minValue;
            slider.maxValue = maxValue;
            slider.wholeNumbers = false;

            return slider;
        }

        private static Button CreateButton(string name, RectTransform parent, string label, Vector2 size, Vector2 anchoredPosition, Vector2 anchor)
        {
            GameObject buttonObject = CreateUIObject(name, parent);
            RectTransform buttonRect = buttonObject.GetComponent<RectTransform>();
            Image image = buttonObject.AddComponent<Image>();
            Button button = buttonObject.AddComponent<Button>();

            buttonRect.anchorMin = anchor;
            buttonRect.anchorMax = anchor;
            buttonRect.pivot = anchor;
            buttonRect.anchoredPosition = anchoredPosition;
            buttonRect.sizeDelta = size;

            image.color = new Color(0.22f, 0.22f, 0.22f, 1f);

            ColorBlock colors = button.colors;
            colors.normalColor = new Color(0.21698111f, 0.21698111f, 0.21698111f, 1f);
            colors.highlightedColor = new Color(0.9607843f, 0.9607843f, 0.9607843f, 1f);
            colors.pressedColor = Color.red;
            colors.selectedColor = new Color(0.9607843f, 0.9607843f, 0.9607843f, 1f);
            colors.disabledColor = new Color(0.78431374f, 0.78431374f, 0.78431374f, 0.5019608f);
            button.colors = colors;
            button.targetGraphic = image;

            TextMeshProUGUI text = CreateText("Text (TMP)", buttonRect, label, Vector2.zero, Vector2.zero, TextAlignmentOptions.Center, 24f);
            RectTransform textRect = text.rectTransform;
            textRect.anchorMin = Vector2.zero;
            textRect.anchorMax = Vector2.one;
            textRect.offsetMin = new Vector2(10f, 5f);
            textRect.offsetMax = new Vector2(-10f, -5f);

            return button;
        }

        private static TextMeshProUGUI CreateText(
            string name,
            RectTransform parent,
            string value,
            Vector2 size,
            Vector2 anchoredPosition,
            TextAlignmentOptions alignment,
            float fontSize,
            Vector2? anchorOverride = null)
        {
            GameObject textObject = CreateUIObject(name, parent);
            RectTransform textRect = textObject.GetComponent<RectTransform>();
            TextMeshProUGUI text = textObject.AddComponent<TextMeshProUGUI>();

            Vector2 anchor = anchorOverride ?? new Vector2(0.5f, 0.5f);
            textRect.anchorMin = anchor;
            textRect.anchorMax = anchor;
            textRect.pivot = anchor;
            textRect.anchoredPosition = anchoredPosition;
            textRect.sizeDelta = size;

            text.font = TMP_Settings.defaultFontAsset;
            text.fontSharedMaterial = text.font != null ? text.font.material : null;
            text.text = value;
            text.fontSize = fontSize;
            text.color = new Color(1f, 0.8272578f, 0f, 1f);
            text.alignment = alignment;
            text.textWrappingMode = TextWrappingModes.NoWrap;

            return text;
        }

        private static GameObject CreateUIObject(string name, Transform parent)
        {
            GameObject gameObject = new GameObject(name, typeof(RectTransform));
            gameObject.layer = 5;
            gameObject.transform.SetParent(parent, false);
            return gameObject;
        }

        private readonly struct SelectionRow
        {
            public SelectionRow(TextMeshProUGUI valueText, Button primaryButton, Button secondaryButton)
            {
                this.valueText = valueText;
                this.primaryButton = primaryButton;
                this.secondaryButton = secondaryButton;
            }

            public readonly TextMeshProUGUI valueText;
            public readonly Button primaryButton;
            public readonly Button secondaryButton;
        }
    }
}
