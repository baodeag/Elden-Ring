using System.Collections.Generic;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace baodeag
{
    public class GameSettingsManager : MonoBehaviour
    {
        private const string MasterVolumeKey = "Settings.MasterVolume";
        private const string MusicVolumeKey = "Settings.MusicVolume";
        private const string SfxVolumeKey = "Settings.SFXVolume";
        private const string CameraSensitivityKey = "Settings.CameraSensitivity";
        private const string FullscreenKey = "Settings.Fullscreen";
        private const string QualityKey = "Settings.Quality";
        private const string ResolutionWidthKey = "Settings.ResolutionWidth";
        private const string ResolutionHeightKey = "Settings.ResolutionHeight";

        public static GameSettingsManager Instance { get; private set; }
        public static bool HasInstance => Instance != null;
        public static event Action SettingsChanged;

        public float masterVolume { get; private set; } = 1f;
        public float musicVolume { get; private set; } = 1f;
        public float sfxVolume { get; private set; } = 1f;
        public float cameraSensitivity { get; private set; } = 1f;
        public bool isFullscreen { get; private set; } = true;
        public int qualityIndex { get; private set; }
        public int resolutionIndex { get; private set; }

        private readonly List<Resolution> availableResolutions = new List<Resolution>();
        private bool hasInitialized;

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void Bootstrap()
        {
            if (Instance != null)
                return;

            GameObject managerObject = new GameObject("Game Settings Manager");
            managerObject.AddComponent<GameSettingsManager>();
            DontDestroyOnLoad(managerObject);
        }

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject);
                return;
            }

            DontDestroyOnLoad(gameObject);
            InitializeIfNeeded();
        }

        private void OnEnable()
        {
            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        private void OnDisable()
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }

        public void InitializeIfNeeded()
        {
            if (hasInitialized)
                return;

            CacheAvailableResolutions();
            LoadSettings();
            ApplyAllSettings(false);
            hasInitialized = true;
            NotifySettingsChanged();
        }

        public IReadOnlyList<Resolution> GetAvailableResolutions()
        {
            InitializeIfNeeded();
            return availableResolutions;
        }

        public float GetEffectiveMusicVolume()
        {
            return masterVolume * musicVolume;
        }

        public float GetEffectiveSFXVolume()
        {
            return masterVolume * sfxVolume;
        }

        public string GetCurrentResolutionLabel()
        {
            if (availableResolutions.Count == 0)
                return $"{Screen.currentResolution.width} x {Screen.currentResolution.height}";

            Resolution resolution = availableResolutions[resolutionIndex];
            return $"{resolution.width} x {resolution.height}";
        }

        public string GetCurrentQualityLabel()
        {
            string[] qualityNames = QualitySettings.names;

            if (qualityNames == null || qualityNames.Length == 0)
                return "Default";

            if (qualityIndex < 0 || qualityIndex >= qualityNames.Length)
                return qualityNames[0];

            return qualityNames[qualityIndex];
        }

        public void SetMasterVolume(float value)
        {
            UpdateAudioSetting(value, Mathf.Clamp01, clampedValue => masterVolume = clampedValue);
        }

        public void SetMusicVolume(float value)
        {
            UpdateAudioSetting(value, Mathf.Clamp01, clampedValue => musicVolume = clampedValue);
        }

        public void SetSFXVolume(float value)
        {
            UpdateAudioSetting(value, Mathf.Clamp01, clampedValue => sfxVolume = clampedValue);
        }

        public void SetCameraSensitivity(float value)
        {
            UpdateAudioSetting(value, clampedValue => Mathf.Clamp(clampedValue, 0.3f, 2f), clampedValue => cameraSensitivity = clampedValue);
        }

        public void ToggleFullscreen()
        {
            isFullscreen = !isFullscreen;
            ApplyDisplaySettings();
            SaveSettings();
            NotifySettingsChanged();
        }

        public void CycleResolution(int direction)
        {
            if (availableResolutions.Count == 0)
                return;

            resolutionIndex = CycleIndex(resolutionIndex, direction, availableResolutions.Count);
            ApplyDisplaySettings();
            SaveSettings();
            NotifySettingsChanged();
        }

        public void CycleQuality(int direction)
        {
            string[] qualityNames = QualitySettings.names;

            if (qualityNames == null || qualityNames.Length == 0)
                return;

            qualityIndex = CycleIndex(qualityIndex, direction, qualityNames.Length);
            ApplyDisplaySettings();
            SaveSettings();
            NotifySettingsChanged();
        }

        private void UpdateAudioSetting(float value, Func<float, float> clampFunc, Action<float> assignAction)
        {
            assignAction(clampFunc(value));
            ApplyAudioAndGameplaySettings();
            SaveSettings();
            NotifySettingsChanged();
        }

        private int CycleIndex(int currentIndex, int direction, int itemCount)
        {
            int nextIndex = currentIndex + direction;

            if (nextIndex < 0)
                return itemCount - 1;

            if (nextIndex >= itemCount)
                return 0;

            return nextIndex;
        }

        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            ApplyAllSettings(false);
            NotifySettingsChanged();
        }

        private void CacheAvailableResolutions()
        {
            availableResolutions.Clear();

            Resolution[] resolutions = Screen.resolutions;
            HashSet<string> uniqueResolutions = new HashSet<string>();

            for (int i = 0; i < resolutions.Length; i++)
            {
                string resolutionKey = $"{resolutions[i].width}x{resolutions[i].height}";

                if (uniqueResolutions.Contains(resolutionKey))
                    continue;

                uniqueResolutions.Add(resolutionKey);
                availableResolutions.Add(resolutions[i]);
            }

            if (availableResolutions.Count == 0)
            {
                availableResolutions.Add(Screen.currentResolution);
            }
        }

        private void LoadSettings()
        {
            masterVolume = PlayerPrefs.GetFloat(MasterVolumeKey, 1f);
            musicVolume = PlayerPrefs.GetFloat(MusicVolumeKey, 1f);
            sfxVolume = PlayerPrefs.GetFloat(SfxVolumeKey, 1f);
            cameraSensitivity = PlayerPrefs.GetFloat(CameraSensitivityKey, 1f);
            isFullscreen = PlayerPrefs.GetInt(FullscreenKey, Screen.fullScreen ? 1 : 0) == 1;
            qualityIndex = Mathf.Clamp(PlayerPrefs.GetInt(QualityKey, QualitySettings.GetQualityLevel()), 0, Mathf.Max(0, QualitySettings.names.Length - 1));

            int savedWidth = PlayerPrefs.GetInt(ResolutionWidthKey, Screen.currentResolution.width);
            int savedHeight = PlayerPrefs.GetInt(ResolutionHeightKey, Screen.currentResolution.height);

            resolutionIndex = 0;

            for (int i = 0; i < availableResolutions.Count; i++)
            {
                if (availableResolutions[i].width == savedWidth && availableResolutions[i].height == savedHeight)
                {
                    resolutionIndex = i;
                    break;
                }
            }
        }

        private void SaveSettings()
        {
            Resolution currentResolution = availableResolutions.Count > 0 ? availableResolutions[resolutionIndex] : Screen.currentResolution;

            PlayerPrefs.SetFloat(MasterVolumeKey, masterVolume);
            PlayerPrefs.SetFloat(MusicVolumeKey, musicVolume);
            PlayerPrefs.SetFloat(SfxVolumeKey, sfxVolume);
            PlayerPrefs.SetFloat(CameraSensitivityKey, cameraSensitivity);
            PlayerPrefs.SetInt(FullscreenKey, isFullscreen ? 1 : 0);
            PlayerPrefs.SetInt(QualityKey, qualityIndex);
            PlayerPrefs.SetInt(ResolutionWidthKey, currentResolution.width);
            PlayerPrefs.SetInt(ResolutionHeightKey, currentResolution.height);
            PlayerPrefs.Save();
        }

        private void ApplyAllSettings(bool saveSettings)
        {
            ApplyDisplaySettings();
            ApplyAudioAndGameplaySettings();

            if (saveSettings)
                SaveSettings();
        }

        private void ApplyDisplaySettings()
        {
            QualitySettings.SetQualityLevel(qualityIndex);

            if (availableResolutions.Count == 0)
                return;

            Resolution resolution = availableResolutions[resolutionIndex];
            FullScreenMode fullScreenMode = isFullscreen ? FullScreenMode.FullScreenWindow : FullScreenMode.Windowed;
            Screen.SetResolution(resolution.width, resolution.height, fullScreenMode, resolution.refreshRateRatio);
        }

        private void ApplyAudioAndGameplaySettings()
        {
            AudioListener.volume = masterVolume;

            if (PlayerCamera.instance != null)
                PlayerCamera.instance.SetCameraSensitivityMultiplier(cameraSensitivity);

            if (WorldSoundFXManager.instance != null)
                WorldSoundFXManager.instance.ApplyAudioSettings();

            if (PlayerUIManager.instance != null)
                PlayerUIManager.instance.ApplyAudioSettings();

            CharacterSoundFXManager[] characterSoundManagers =
                FindObjectsByType<CharacterSoundFXManager>(FindObjectsInactive.Exclude, FindObjectsSortMode.None);

            for (int i = 0; i < characterSoundManagers.Length; i++)
            {
                characterSoundManagers[i].RefreshAudioSettings();
            }
        }

        private void NotifySettingsChanged()
        {
            SettingsChanged?.Invoke();
        }
    }

    public static class GameSettingsMenuViewUtility
    {
        public static Slider FindSlider(RectTransform contentRoot, string path) => FindComponentByPath<Slider>(contentRoot, path);
        public static Button FindButton(RectTransform contentRoot, string path) => FindComponentByPath<Button>(contentRoot, path);
        public static TextMeshProUGUI FindText(RectTransform contentRoot, string path) => FindComponentByPath<TextMeshProUGUI>(contentRoot, path);

        public static T FindComponentByPath<T>(RectTransform contentRoot, string path) where T : Component
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
    }

}
