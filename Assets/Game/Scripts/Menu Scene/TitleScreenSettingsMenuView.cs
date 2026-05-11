using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace baodeag
{
    public class TitleScreenSettingsMenuView : MonoBehaviour
    {
        [Header("Root")]
        [SerializeField] private RectTransform contentRoot;

        [Header("Sliders")]
        [SerializeField] private Slider masterVolumeSlider;
        [SerializeField] private Slider musicVolumeSlider;
        [SerializeField] private Slider sfxVolumeSlider;
        [SerializeField] private Slider cameraSensitivitySlider;

        [Header("Buttons")]
        [SerializeField] private Button fullscreenToggleButton;
        [SerializeField] private Button resolutionPreviousButton;
        [SerializeField] private Button resolutionNextButton;
        [SerializeField] private Button qualityPreviousButton;
        [SerializeField] private Button qualityNextButton;
        [SerializeField] private Button closeButton;

        [Header("Texts")]
        [SerializeField] private TextMeshProUGUI fullscreenValueText;
        [SerializeField] private TextMeshProUGUI resolutionValueText;
        [SerializeField] private TextMeshProUGUI qualityValueText;
        [SerializeField] private TextMeshProUGUI masterVolumeValueText;
        [SerializeField] private TextMeshProUGUI musicVolumeValueText;
        [SerializeField] private TextMeshProUGUI sfxVolumeValueText;
        [SerializeField] private TextMeshProUGUI cameraSensitivityValueText;

        private TitleScreenManager owner;
        private bool listenersBound;

        public void Initialize(TitleScreenManager manager)
        {
            owner = manager;
            AutoBindSceneReferences();
            BindListeners();
            Refresh();
        }

        public void Refresh()
        {
            AutoBindSceneReferences();

            if (!GameSettingsManager.HasInstance)
                return;

            GameSettingsManager settings = GameSettingsManager.Instance;

            SetSliderWithoutNotify(masterVolumeSlider, settings.masterVolume);
            SetSliderWithoutNotify(musicVolumeSlider, settings.musicVolume);
            SetSliderWithoutNotify(sfxVolumeSlider, settings.sfxVolume);
            SetSliderWithoutNotify(cameraSensitivitySlider, settings.cameraSensitivity);

            SetText(masterVolumeValueText, GetPercentLabel(settings.masterVolume));
            SetText(musicVolumeValueText, GetPercentLabel(settings.musicVolume));
            SetText(sfxVolumeValueText, GetPercentLabel(settings.sfxVolume));
            SetText(cameraSensitivityValueText, $"x{settings.cameraSensitivity:0.00}");
            SetText(fullscreenValueText, settings.isFullscreen ? "ON" : "OFF");
            SetText(resolutionValueText, settings.GetCurrentResolutionLabel());
            SetText(qualityValueText, settings.GetCurrentQualityLabel());
        }

        public void CloseMenu()
        {
            if (owner != null)
                owner.CloseSettingsMenu();
        }

        private void AutoBindSceneReferences()
        {
            if (contentRoot == null)
                contentRoot = transform.Find("Content Panel") as RectTransform;

            if (contentRoot == null)
                return;

            masterVolumeSlider ??= FindSlider("Master Volume Row/Slider");
            musicVolumeSlider ??= FindSlider("Music Volume Row/Slider");
            sfxVolumeSlider ??= FindSlider("SFX Volume Row/Slider");
            cameraSensitivitySlider ??= FindSlider("Camera Sensitivity Row/Slider");

            fullscreenToggleButton ??= FindButton("Fullscreen Row/Primary Button");
            resolutionPreviousButton ??= FindButton("Resolution Row/Previous Button");
            resolutionNextButton ??= FindButton("Resolution Row/Next Button");
            qualityPreviousButton ??= FindButton("Quality Row/Previous Button");
            qualityNextButton ??= FindButton("Quality Row/Next Button");
            closeButton ??= FindButton("Close Button");

            fullscreenValueText ??= FindText("Fullscreen Row/Value");
            resolutionValueText ??= FindText("Resolution Row/Value");
            qualityValueText ??= FindText("Quality Row/Value");
            masterVolumeValueText ??= FindText("Master Volume Row/Value");
            musicVolumeValueText ??= FindText("Music Volume Row/Value");
            sfxVolumeValueText ??= FindText("SFX Volume Row/Value");
            cameraSensitivityValueText ??= FindText("Camera Sensitivity Row/Value");
        }

        private Slider FindSlider(string path) => FindComponentByPath<Slider>(path);
        private Button FindButton(string path) => FindComponentByPath<Button>(path);
        private TextMeshProUGUI FindText(string path) => FindComponentByPath<TextMeshProUGUI>(path);

        private T FindComponentByPath<T>(string path) where T : Component
        {
            if (contentRoot == null || string.IsNullOrWhiteSpace(path))
                return null;

            Transform direct = contentRoot.Find(path);
            if (direct != null)
                return direct.GetComponent<T>();

            string[] segments = path.Split('/');
            if (segments.Length == 0)
                return null;

            foreach (Transform candidate in contentRoot.GetComponentsInChildren<Transform>(true))
            {
                if (candidate.name != segments[0])
                    continue;

                Transform current = candidate;
                for (int i = 1; i < segments.Length && current != null; i++)
                    current = current.Find(segments[i]);

                if (current != null)
                    return current.GetComponent<T>();
            }

            return null;
        }

        private void BindListeners()
        {
            if (listenersBound)
                return;

            BindSlider(masterVolumeSlider, OnMasterVolumeChanged, 0f, 1f);
            BindSlider(musicVolumeSlider, OnMusicVolumeChanged, 0f, 1f);
            BindSlider(sfxVolumeSlider, OnSFXVolumeChanged, 0f, 1f);
            BindSlider(cameraSensitivitySlider, OnCameraSensitivityChanged, 0.3f, 2f);

            BindButton(fullscreenToggleButton, ToggleFullscreen);
            BindButton(resolutionPreviousButton, () => CycleResolution(-1));
            BindButton(resolutionNextButton, () => CycleResolution(1));
            BindButton(qualityPreviousButton, () => CycleQuality(-1));
            BindButton(qualityNextButton, () => CycleQuality(1));
            BindButton(closeButton, CloseMenu);

            listenersBound = true;
        }

        private void BindSlider(Slider slider, UnityEngine.Events.UnityAction<float> callback, float minValue, float maxValue)
        {
            if (slider == null)
                return;

            slider.minValue = minValue;
            slider.maxValue = maxValue;
            slider.wholeNumbers = false;
            slider.onValueChanged.RemoveAllListeners();
            slider.onValueChanged.AddListener(callback);
        }

        private void BindButton(Button button, UnityEngine.Events.UnityAction callback)
        {
            if (button == null)
                return;

            button.onClick.RemoveAllListeners();
            button.onClick.AddListener(callback);
        }

        private void SetSliderWithoutNotify(Slider slider, float value)
        {
            if (slider != null)
                slider.SetValueWithoutNotify(value);
        }

        private void SetText(TextMeshProUGUI text, string value)
        {
            if (text != null)
                text.text = value;
        }

        private string GetPercentLabel(float value)
        {
            return $"{Mathf.RoundToInt(Mathf.Clamp01(value) * 100f)}%";
        }

        private void ApplySettingsAndRefresh(Action<GameSettingsManager> applyAction)
        {
            if (!GameSettingsManager.HasInstance)
                return;

            applyAction(GameSettingsManager.Instance);
            Refresh();
        }

        private void OnMasterVolumeChanged(float value)
        {
            ApplySettingsAndRefresh(settings => settings.SetMasterVolume(value));
        }

        private void OnMusicVolumeChanged(float value)
        {
            ApplySettingsAndRefresh(settings => settings.SetMusicVolume(value));
        }

        private void OnSFXVolumeChanged(float value)
        {
            ApplySettingsAndRefresh(settings => settings.SetSFXVolume(value));
        }

        private void OnCameraSensitivityChanged(float value)
        {
            ApplySettingsAndRefresh(settings => settings.SetCameraSensitivity(value));
        }

        private void ToggleFullscreen()
        {
            ApplySettingsAndRefresh(settings => settings.ToggleFullscreen());
        }

        private void CycleResolution(int direction)
        {
            ApplySettingsAndRefresh(settings => settings.CycleResolution(direction));
        }

        private void CycleQuality(int direction)
        {
            ApplySettingsAndRefresh(settings => settings.CycleQuality(direction));
        }
    }
}
