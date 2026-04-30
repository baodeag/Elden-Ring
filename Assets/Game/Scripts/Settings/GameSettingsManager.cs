using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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
            masterVolume = Mathf.Clamp01(value);
            ApplyAudioAndGameplaySettings();
            SaveSettings();
        }

        public void SetMusicVolume(float value)
        {
            musicVolume = Mathf.Clamp01(value);
            ApplyAudioAndGameplaySettings();
            SaveSettings();
        }

        public void SetSFXVolume(float value)
        {
            sfxVolume = Mathf.Clamp01(value);
            ApplyAudioAndGameplaySettings();
            SaveSettings();
        }

        public void SetCameraSensitivity(float value)
        {
            cameraSensitivity = Mathf.Clamp(value, 0.3f, 2f);
            ApplyAudioAndGameplaySettings();
            SaveSettings();
        }

        public void ToggleFullscreen()
        {
            isFullscreen = !isFullscreen;
            ApplyDisplaySettings();
            SaveSettings();
        }

        public void CycleResolution(int direction)
        {
            if (availableResolutions.Count == 0)
                return;

            resolutionIndex += direction;

            if (resolutionIndex < 0)
                resolutionIndex = availableResolutions.Count - 1;
            else if (resolutionIndex >= availableResolutions.Count)
                resolutionIndex = 0;

            ApplyDisplaySettings();
            SaveSettings();
        }

        public void CycleQuality(int direction)
        {
            string[] qualityNames = QualitySettings.names;

            if (qualityNames == null || qualityNames.Length == 0)
                return;

            qualityIndex += direction;

            if (qualityIndex < 0)
                qualityIndex = qualityNames.Length - 1;
            else if (qualityIndex >= qualityNames.Length)
                qualityIndex = 0;

            ApplyDisplaySettings();
            SaveSettings();
        }

        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            ApplyAllSettings(false);
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
    }
}
