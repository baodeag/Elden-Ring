using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace baodeag
{
    public class PlayerUILoadingScreenManager : MonoBehaviour
    {
        [SerializeField] GameObject loadingScreen;
        [SerializeField] CanvasGroup canvasGroup;
        [Header("Loading Progress")]
        [SerializeField] Slider progressBar;
        [SerializeField] Text progressText;
        private Coroutine fadeLoadingScreenCoroutine;
        private const string DefaultProgressLabel = "Loading";


        private void Start()
        {
            SceneManager.activeSceneChanged += OnSceneChanged;
        }

        private void OnSceneChanged(Scene arg0, Scene arg1)
        {
            // Scene transitions can require extra time for world scenes, additive areas,
            // and teleport targets to become ready. The loading screen is now closed
            // explicitly by the systems that know when the world is actually ready.
        }

        public void ActivateLoadingScreen()
        {
            if (fadeLoadingScreenCoroutine != null)
            {
                StopCoroutine(fadeLoadingScreenCoroutine);
                fadeLoadingScreenCoroutine = null;
            }

            canvasGroup.alpha = 1;
            loadingScreen.SetActive(true);
            SetProgress(0, DefaultProgressLabel);
        }

        public void SetProgress(float progress, string label = DefaultProgressLabel)
        {
            progress = Mathf.Clamp01(progress);

            if (progressBar != null)
                progressBar.value = progress;

            if (progressText != null)
            {
                int percent = Mathf.RoundToInt(progress * 100f);
                string progressLabel = string.IsNullOrWhiteSpace(label) ? DefaultProgressLabel : label;
                progressText.text = $"{progressLabel} {percent}%";
            }
        }

        public void DeactivateLoadingScreen(float delay = 1)
        {
            //if the loading screen is not active, return
            if (!loadingScreen.activeSelf)
                return;

            if (fadeLoadingScreenCoroutine != null)
            {
                StopCoroutine(fadeLoadingScreenCoroutine);
                fadeLoadingScreenCoroutine = null;
            }

            SetProgress(1, "Ready");

            //the duration is how long the fade will take, the delay is how long to wait before starting the fade
            fadeLoadingScreenCoroutine = StartCoroutine(FadeLoadingScreen(1, delay));
        }

        //private IEnumerator FadeLoadingScreen(float duration, float delay)
        //{
        //    while (WorldAIManager.instance.isPerformingLoadingOperation)
        //    {
        //        yield return null;
        //    }

        //    loadingScreen.SetActive(true);

        //    if (duration > 0)
        //    {
        //        while (delay > 0)
        //        {
        //            delay -= Time.deltaTime;
        //            yield return null;
        //        }

        //        canvasGroup.alpha = 1;
        //        float elapsedTime = 0;

        //        while (elapsedTime < duration)
        //        {
        //            elapsedTime += Time.deltaTime;
        //            canvasGroup.alpha = Mathf.Lerp(1, 0, elapsedTime / duration);
        //            yield return null;
        //        }
        //    }

        //    canvasGroup.alpha = 0;
        //    loadingScreen.SetActive(false);
        //    fadeLoadingScreenCoroutine = null;
        //    yield return null;
        //}

        private IEnumerator FadeLoadingScreen(float duration, float delay)
        {
            // Wait for AI loading operations to complete (with safety check)
            float waitTimeout = 10f; // Maximum wait time
            float waitTimer = 0f;
            
            while (WorldAIManager.instance != null && 
                   WorldAIManager.instance.isPerformingLoadingOperation && 
                   waitTimer < waitTimeout)
            {
                waitTimer += Time.deltaTime;
                yield return null;
            }

            loadingScreen.SetActive(true);

            if (duration > 0)
            {
                while (delay > 0)
                {
                    delay -= Time.deltaTime;
                    yield return null;
                }

                canvasGroup.alpha = 1;
                float elapsedTime = 0;

                while (elapsedTime < duration)
                {
                    elapsedTime += Time.deltaTime;
                    canvasGroup.alpha = Mathf.Lerp(1, 0, elapsedTime / duration);
                    yield return null;
                }
            }

            canvasGroup.alpha = 0;
            loadingScreen.SetActive(false);
            fadeLoadingScreenCoroutine = null;
            yield return null;
        }

        public bool LoadingScreenIsActive()
        {
            return loadingScreen.activeSelf;
        }

        public void ForceHideLoadingScreen()
        {
            if (fadeLoadingScreenCoroutine != null)
            {
                StopCoroutine(fadeLoadingScreenCoroutine);
                fadeLoadingScreenCoroutine = null;
            }

            SetProgress(1, "Ready");
            canvasGroup.alpha = 0;
            loadingScreen.SetActive(false);
        }
    }
}
