using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace baodeag
{
    public class UIButtonClickSoundInstaller : MonoBehaviour
    {
        private const string DefaultButtonClickSFXPath = "SFX/Clic03";
        private static UIButtonClickSoundInstaller instance;

        private AudioSource audioSource;
        private AudioClip fallbackClickSFX;
        private float nextScanTime;

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        private static void Bootstrap()
        {
            if (instance != null)
                return;

            GameObject installerObject = new GameObject("UI Button Click Sound Installer");
            instance = installerObject.AddComponent<UIButtonClickSoundInstaller>();
            DontDestroyOnLoad(installerObject);
        }

        private void Awake()
        {
            if (instance != null && instance != this)
            {
                Destroy(gameObject);
                return;
            }

            instance = this;
            audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.playOnAwake = false;
            audioSource.spatialBlend = 0f;
            fallbackClickSFX = Resources.Load<AudioClip>(DefaultButtonClickSFXPath);
            SceneManager.sceneLoaded += OnSceneLoaded;
            InstallButtonClickSounds();
        }

        private void OnDestroy()
        {
            if (instance == this)
            {
                SceneManager.sceneLoaded -= OnSceneLoaded;
                instance = null;
            }
        }

        private void Update()
        {
            if (Time.unscaledTime < nextScanTime)
                return;

            nextScanTime = Time.unscaledTime + 1f;
            InstallButtonClickSounds();
        }

        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            InstallButtonClickSounds();
        }

        private void InstallButtonClickSounds()
        {
            Button[] buttons = FindObjectsByType<Button>(FindObjectsInactive.Include, FindObjectsSortMode.None);

            for (int i = 0; i < buttons.Length; i++)
            {
                Button button = buttons[i];

                if (button == null || button.GetComponent<UIButtonClickSound>() != null)
                    continue;

                button.gameObject.AddComponent<UIButtonClickSound>();
            }
        }

        public static void PlayClickSound()
        {
            if (instance == null || instance.audioSource == null)
            {
                return;
            }

            AudioClip clickClip = WorldSoundFXManager.instance != null
                ? WorldSoundFXManager.instance.buttonClickUISFX
                : null;

            if (clickClip == null)
                clickClip = instance.fallbackClickSFX;

            if (clickClip == null)
                return;

            if (GameSettingsManager.HasInstance)
                instance.audioSource.volume = GameSettingsManager.Instance.GetEffectiveSFXVolume();

            instance.audioSource.PlayOneShot(clickClip);
        }
    }

    public class UIButtonClickSound : MonoBehaviour, IPointerClickHandler, ISubmitHandler
    {
        private Button button;

        private void Awake()
        {
            button = GetComponent<Button>();
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            PlayIfButtonCanClick();
        }

        public void OnSubmit(BaseEventData eventData)
        {
            PlayIfButtonCanClick();
        }

        private void PlayIfButtonCanClick()
        {
            if (button == null)
                button = GetComponent<Button>();

            if (button == null || !button.IsActive() || !button.IsInteractable())
                return;

            UIButtonClickSoundInstaller.PlayClickSound();
        }
    }
}
