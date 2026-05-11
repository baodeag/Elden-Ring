using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace baodeag
{
    public class PlayerUISettingsManager : PlayerUIMenu
    {
        [SerializeField] private RectTransform contentRoot;

        private Slider masterVolumeSlider;
        private Slider musicVolumeSlider;
        private Slider sfxVolumeSlider;
        private Slider cameraSensitivitySlider;

        private TextMeshProUGUI masterVolumeValueText;
        private TextMeshProUGUI musicVolumeValueText;
        private TextMeshProUGUI sfxVolumeValueText;
        private TextMeshProUGUI cameraSensitivityValueText;
        private TextMeshProUGUI fullscreenValueText;
        private TextMeshProUGUI resolutionValueText;
        private TextMeshProUGUI qualityValueText;

        private Button fullscreenToggleButton;
        private Button resolutionPreviousButton;
        private Button resolutionNextButton;
        private Button qualityPreviousButton;
        private Button qualityNextButton;
        private Button closeButton;

        private bool listenersBound;

        public override void OpenMenu()
        {
            EnsureInitialized();
            Refresh();
            base.OpenMenu();
        }

        public void OpenFromCharacterMenu()
        {
            EnsureInitialized();

            if (PlayerUIManager.instance == null)
                return;

            PlayerUIManager.instance.TransitionToMenu(PlayerUIManager.instance.playerUICharacterMenuManager, this);
        }

        public void Refresh()
        {
            EnsureInitialized();

            if (!GameSettingsManager.HasInstance)
                return;

            GameSettingsManager.Instance.InitializeIfNeeded();

            if (masterVolumeSlider != null)
                masterVolumeSlider.SetValueWithoutNotify(GameSettingsManager.Instance.masterVolume);

            if (musicVolumeSlider != null)
                musicVolumeSlider.SetValueWithoutNotify(GameSettingsManager.Instance.musicVolume);

            if (sfxVolumeSlider != null)
                sfxVolumeSlider.SetValueWithoutNotify(GameSettingsManager.Instance.sfxVolume);

            if (cameraSensitivitySlider != null)
                cameraSensitivitySlider.SetValueWithoutNotify(GameSettingsManager.Instance.cameraSensitivity);

            if (masterVolumeValueText != null)
                masterVolumeValueText.text = $"{Mathf.RoundToInt(GameSettingsManager.Instance.masterVolume * 100f)}%";

            if (musicVolumeValueText != null)
                musicVolumeValueText.text = $"{Mathf.RoundToInt(GameSettingsManager.Instance.musicVolume * 100f)}%";

            if (sfxVolumeValueText != null)
                sfxVolumeValueText.text = $"{Mathf.RoundToInt(GameSettingsManager.Instance.sfxVolume * 100f)}%";

            if (cameraSensitivityValueText != null)
                cameraSensitivityValueText.text = $"x{GameSettingsManager.Instance.cameraSensitivity:0.00}";

            if (fullscreenValueText != null)
                fullscreenValueText.text = GameSettingsManager.Instance.isFullscreen ? "ON" : "OFF";

            if (resolutionValueText != null)
                resolutionValueText.text = GameSettingsManager.Instance.GetCurrentResolutionLabel();

            if (qualityValueText != null)
                qualityValueText.text = GameSettingsManager.Instance.GetCurrentQualityLabel();
        }

        private void CacheReferences()
        {
            if (contentRoot == null)
                contentRoot = transform.Find("Content Panel") as RectTransform;

            masterVolumeSlider ??= FindSlider("Master Volume Row/Slider");
            musicVolumeSlider ??= FindSlider("Music Volume Row/Slider");
            sfxVolumeSlider ??= FindSlider("SFX Volume Row/Slider");
            cameraSensitivitySlider ??= FindSlider("Camera Sensitivity Row/Slider");

            masterVolumeValueText ??= FindText("Master Volume Row/Value");
            musicVolumeValueText ??= FindText("Music Volume Row/Value");
            sfxVolumeValueText ??= FindText("SFX Volume Row/Value");
            cameraSensitivityValueText ??= FindText("Camera Sensitivity Row/Value");
            fullscreenValueText ??= FindText("Fullscreen Row/Value");
            resolutionValueText ??= FindText("Resolution Row/Value");
            qualityValueText ??= FindText("Quality Row/Value");

            fullscreenToggleButton ??= FindButton("Fullscreen Row/Primary Button");
            resolutionPreviousButton ??= FindButton("Resolution Row/Previous Button");
            resolutionNextButton ??= FindButton("Resolution Row/Next Button");
            qualityPreviousButton ??= FindButton("Quality Row/Previous Button");
            qualityNextButton ??= FindButton("Quality Row/Next Button");
            closeButton ??= FindButton("Close Button");
        }

        private void EnsureInitialized()
        {
            if (menu == null)
                menu = gameObject;

            CacheReferences();
            BindListeners();
        }

        private void BindListeners()
        {
            if (listenersBound)
                return;

            BindSlider(masterVolumeSlider, 0f, 1f, value =>
            {
                GameSettingsManager.Instance.SetMasterVolume(value);
                Refresh();
            });

            BindSlider(musicVolumeSlider, 0f, 1f, value =>
            {
                GameSettingsManager.Instance.SetMusicVolume(value);
                Refresh();
            });

            BindSlider(sfxVolumeSlider, 0f, 1f, value =>
            {
                GameSettingsManager.Instance.SetSFXVolume(value);
                Refresh();
            });

            BindSlider(cameraSensitivitySlider, 0.3f, 2f, value =>
            {
                GameSettingsManager.Instance.SetCameraSensitivity(value);
                Refresh();
            });

            BindButton(fullscreenToggleButton, () =>
            {
                GameSettingsManager.Instance.ToggleFullscreen();
                Refresh();
            });

            BindButton(resolutionPreviousButton, () =>
            {
                GameSettingsManager.Instance.CycleResolution(-1);
                Refresh();
            });

            BindButton(resolutionNextButton, () =>
            {
                GameSettingsManager.Instance.CycleResolution(1);
                Refresh();
            });

            BindButton(qualityPreviousButton, () =>
            {
                GameSettingsManager.Instance.CycleQuality(-1);
                Refresh();
            });

            BindButton(qualityNextButton, () =>
            {
                GameSettingsManager.Instance.CycleQuality(1);
                Refresh();
            });

            BindButton(closeButton, () =>
            {
                if (PlayerUIManager.instance != null && PlayerUIManager.instance.CloseCurrentMenuStep())
                    return;

                CloseMenuAfterFixedFrame();
            });

            listenersBound = true;
        }

        private void BindSlider(Slider slider, float minValue, float maxValue, UnityEngine.Events.UnityAction<float> callback)
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

        private Slider FindSlider(string relativePath)
        {
            Transform target = contentRoot != null ? contentRoot.Find(relativePath) : null;
            return target != null ? target.GetComponent<Slider>() : null;
        }

        private Button FindButton(string relativePath)
        {
            Transform target = contentRoot != null ? contentRoot.Find(relativePath) : null;
            return target != null ? target.GetComponent<Button>() : null;
        }

        private TextMeshProUGUI FindText(string relativePath)
        {
            Transform target = contentRoot != null ? contentRoot.Find(relativePath) : null;
            return target != null ? target.GetComponent<TextMeshProUGUI>() : null;
        }
    }
}
