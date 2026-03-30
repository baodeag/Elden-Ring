using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.UI;

namespace baodeag
{
    public class PlayerUIPopUpManager : MonoBehaviour
    {
        [Header("Pop Up Parent")]
        [SerializeField] Transform popUpTransformParent; 

        [Header("Message Pop Up")]
        [SerializeField] TextMeshProUGUI popUpMessageText;
        [SerializeField] GameObject popUpMessageGameObject;
        [SerializeField] private GameObject statusEffectPopUpPrefab;

        [Header("Item Pop Up")]
        [SerializeField] GameObject itemPopUpGameObject;
        [SerializeField] Image itemIcon;
        [SerializeField] TextMeshProUGUI itemName;
        [SerializeField] TextMeshProUGUI itemAmount;

        [Header("You Died Popup")]
        [SerializeField] GameObject youDiedPopUpGameObject;
        [SerializeField] TextMeshProUGUI youDiedPopUpBackgroundText;
        [SerializeField] TextMeshProUGUI youDiedPopUpText;
        [SerializeField] CanvasGroup youDiedPopUpCanvasGroup;
        private Coroutine youDiedStylePopUpCoroutine;

        [Header("Boss Defeated Popup")]
        [SerializeField] GameObject bossDefeatedPopUpGameObject;
        [SerializeField] TextMeshProUGUI bossDefeatedPopUpBackgroundText;
        [SerializeField] TextMeshProUGUI bossDefeatedPopUpText;
        [SerializeField] CanvasGroup bossDefeatedPopUpCanvasGroup;
        private Coroutine bossDefeatedPopUpCoroutine;

        [Header("Grace Restored Popup")]
        [SerializeField] GameObject graceRestoredPopUpGameObject;
        [SerializeField] TextMeshProUGUI graceRestoredPopUpBackgroundText;
        [SerializeField] TextMeshProUGUI graceRestoredPopUpText;
        [SerializeField] CanvasGroup graceRestoredPopUpCanvasGroup;
        private Coroutine graceRestoredStylePopUpCoroutine;

        [Header("Dialogue Pop Up")]
        [SerializeField] GameObject dialoguePopUpGameObject;
        [SerializeField] TextMeshProUGUI dialoguePopUpText;
        [SerializeField] CharacterDialogue currentDialogue;
        private Coroutine dialogueCoroutine;

        public void CloseAllPopUpWindows()
        {
            popUpMessageGameObject.SetActive(false);
            itemPopUpGameObject.SetActive(false);

            PlayerUIManager.instance.popUpWindowIsOpen = false;
        }

        public void SendPlayerMessagePopUp(string messageText)
        {
            PlayerUIManager.instance.popUpWindowIsOpen = true;
            popUpMessageText.text = messageText;
            popUpMessageGameObject.SetActive(true);
        }

        public void SendItemPopUp(Item item, int amount)
        {
            itemAmount.enabled = false;
            itemIcon.sprite = item.itemIcon;
            itemName.text = item.itemName;

            if (amount > 0)
            {
                itemAmount.enabled = true;
                itemAmount.text = "x" + amount.ToString();
            }

            itemPopUpGameObject.SetActive(true);
            PlayerUIManager.instance.popUpWindowIsOpen = true;
        }

        public void SendYouDiedPopUp()
        {
            PlayYouDiedStylePopUp("YOU DIED");
        }

        public void SendBossDefeatedPopUp(string bossDefeatedMessage)
            => PlayBossStylePopUp(bossDefeatedMessage);

        public void SendMapUnlockedPopUp(string mapUnlockedMessage)
            => PlayBossStylePopUp(mapUnlockedMessage);

        public void SendVictoryPopUp(string victoryMessage)
            => PlayYouDiedStylePopUp(victoryMessage);

        public void SendMapUnlockedPopUpDelayed(string mapUnlockedMessage, float delay)
        {
            StartCoroutine(SendMapUnlockedPopUpDelayedCoroutine(mapUnlockedMessage, delay));
        }

        public void SendVictoryPopUpDelayed(string victoryMessage, float delay)
        {
            StartCoroutine(SendVictoryPopUpDelayedCoroutine(victoryMessage, delay));
        }

        private IEnumerator SendMapUnlockedPopUpDelayedCoroutine(string message, float delay)
        {
            yield return new WaitForSeconds(delay);
            Debug.Log($"PlayerUIPopUpManager: showing delayed map unlocked popup '{message}'.");
            PlayBossStylePopUp(message);
        }

        private IEnumerator SendVictoryPopUpDelayedCoroutine(string message, float delay)
        {
            yield return new WaitForSeconds(delay);
            Debug.Log($"PlayerUIPopUpManager: showing delayed victory popup '{message}'.");
            PlayYouDiedStylePopUp(message);
        }

        private void PlayBossStylePopUp(string message)
        {
            if (bossDefeatedPopUpCoroutine != null)
            {
                StopCoroutine(bossDefeatedPopUpCoroutine);
                bossDefeatedPopUpCoroutine = null;
            }

            bossDefeatedPopUpText.text = message;
            bossDefeatedPopUpBackgroundText.text = message;
            bossDefeatedPopUpBackgroundText.characterSpacing = 0;
            bossDefeatedPopUpCanvasGroup.alpha = 0;
            bossDefeatedPopUpGameObject.SetActive(true);

            bossDefeatedPopUpCoroutine = StartCoroutine(PlayBossStylePopUpCoroutine());
        }

        private IEnumerator PlayBossStylePopUpCoroutine()
        {
            float stretchDuration = 5f;
            float stretchTarget = 19f;
            float fadeInDuration = 0.9f;
            float visibleDelay = 1.25f;
            float fadeOutDuration = 1f;

            float timer = 0f;

            while (timer < fadeInDuration)
            {
                timer += Time.deltaTime;
                float normalizedFade = Mathf.Clamp01(timer / fadeInDuration);
                bossDefeatedPopUpCanvasGroup.alpha = normalizedFade;

                float normalizedStretch = Mathf.Clamp01(timer / stretchDuration);
                bossDefeatedPopUpBackgroundText.characterSpacing = Mathf.Lerp(0f, stretchTarget, normalizedStretch);

                yield return null;
            }

            bossDefeatedPopUpCanvasGroup.alpha = 1f;

            while (timer < stretchDuration)
            {
                timer += Time.deltaTime;
                float normalizedStretch = Mathf.Clamp01(timer / stretchDuration);
                bossDefeatedPopUpBackgroundText.characterSpacing = Mathf.Lerp(0f, stretchTarget, normalizedStretch);
                yield return null;
            }

            yield return new WaitForSeconds(visibleDelay);

            float fadeOutTimer = 0f;

            while (fadeOutTimer < fadeOutDuration)
            {
                fadeOutTimer += Time.deltaTime;
                bossDefeatedPopUpCanvasGroup.alpha = Mathf.Lerp(1f, 0f, fadeOutTimer / fadeOutDuration);
                yield return null;
            }

            bossDefeatedPopUpCanvasGroup.alpha = 0f;
            bossDefeatedPopUpCoroutine = null;
        }

        public void SendGraceRestoredPopUp(string graceRestoredMessage)
        {
            PlayGraceRestoredStylePopUp(graceRestoredMessage);
        }

        private void PlayYouDiedStylePopUp(string message)
        {
            if (youDiedStylePopUpCoroutine != null)
            {
                StopCoroutine(youDiedStylePopUpCoroutine);
                youDiedStylePopUpCoroutine = null;
            }

            youDiedPopUpText.text = message;
            youDiedPopUpBackgroundText.text = message;
            youDiedPopUpBackgroundText.characterSpacing = 0;
            youDiedPopUpCanvasGroup.alpha = 0;
            youDiedPopUpGameObject.SetActive(true);

            youDiedStylePopUpCoroutine = StartCoroutine(PlayYouDiedStylePopUpCoroutine());
        }

        private IEnumerator PlayYouDiedStylePopUpCoroutine()
        {
            StartCoroutine(StretchPopUpTextOverTime(youDiedPopUpBackgroundText, 8, 19));
            yield return StartCoroutine(FadeInPopUpOverTime(youDiedPopUpCanvasGroup, 1.5f));
            yield return StartCoroutine(WaitThenFadeOutPopUpOverTime(youDiedPopUpCanvasGroup, 2, 2.5f));
            youDiedStylePopUpCoroutine = null;
        }

        private void PlayGraceRestoredStylePopUp(string message)
        {
            if (graceRestoredStylePopUpCoroutine != null)
            {
                StopCoroutine(graceRestoredStylePopUpCoroutine);
                graceRestoredStylePopUpCoroutine = null;
            }

            graceRestoredPopUpText.text = message;
            graceRestoredPopUpBackgroundText.text = message;
            graceRestoredPopUpGameObject.SetActive(true);
            graceRestoredPopUpBackgroundText.characterSpacing = 0;
            graceRestoredPopUpCanvasGroup.alpha = 0;

            graceRestoredStylePopUpCoroutine = StartCoroutine(PlayGraceRestoredStylePopUpCoroutine());
        }

        private IEnumerator PlayGraceRestoredStylePopUpCoroutine()
        {
            StartCoroutine(StretchPopUpTextOverTime(graceRestoredPopUpBackgroundText, 8, 19));
            yield return StartCoroutine(FadeInPopUpOverTime(graceRestoredPopUpCanvasGroup, 1.5f));
            yield return StartCoroutine(WaitThenFadeOutPopUpOverTime(graceRestoredPopUpCanvasGroup, 2, 2.5f));
            graceRestoredStylePopUpCoroutine = null;
        }

        public void SendStatusEffectPopUp(BuildUp status)
        {
            GameObject popUp = Instantiate(statusEffectPopUpPrefab, popUpTransformParent);
            UI_StatusEffectWarning popUpWarning = popUp.GetComponent<UI_StatusEffectWarning>();
            popUpWarning.SetWarningMessage(status);

            StartCoroutine(FadeOutThenDestroy(popUpWarning.canvas, 2, popUp));
        }

        public void SendDialoguePopUp(CharacterDialogue dialogue, AICharacterManager aiCharacter)
        {
            PlayerUIManager.instance.playerUIHudManager.ToggleHUDWithOutPopUps(false);
            currentDialogue = dialogue;

            if (dialogueCoroutine != null)
                StopCoroutine(dialogueCoroutine);

            //close all pop up windows
            PlayerUIManager.instance.playerUIPopUpManager.CloseAllPopUpWindows();
            PlayerUIManager.instance.popUpWindowIsOpen = true;

            dialogueCoroutine = StartCoroutine(dialogue.PlayDialogueCoroutine(aiCharacter));
        }

        public void SendNextDialoguePopUpInIndex(CharacterDialogue dialogue, AICharacterManager aiCharacter)
        {
            currentDialogue = dialogue;

            if (dialogueCoroutine != null)
                StopCoroutine(dialogueCoroutine);

            if (aiCharacter.aiCharacterSoundFXManager.dialogueIsPlaying)
                aiCharacter.aiCharacterSoundFXManager.audioSource.Stop();

            //close all pop up windows
            PlayerUIManager.instance.playerUIPopUpManager.CloseAllPopUpWindows();
            PlayerUIManager.instance.popUpWindowIsOpen = true;

            currentDialogue.dialogueIndex++;
            dialogueCoroutine = StartCoroutine(dialogue.PlayDialogueCoroutine(aiCharacter));
        }

        public void SetDialoguePopUpSubtitles(string dialogueText)
        {
            dialoguePopUpGameObject.SetActive(true);
            dialoguePopUpText.text = dialogueText;
        }

        public void EndDialoguePopUp()
        {
            dialoguePopUpGameObject.SetActive(false);
            PlayerUIManager.instance.playerUIHudManager.ToggleHUDWithOutPopUps(true);
        }

        public void CancelDialoguePopUp(AICharacterManager aiCharacter)
        {
            PlayerUIManager.instance.playerUIHudManager.ToggleHUDWithOutPopUps(true);

            if (dialogueCoroutine != null)
                StopCoroutine(dialogueCoroutine);

            if (aiCharacter.aiCharacterSoundFXManager.audioSource.isPlaying)
                aiCharacter.aiCharacterSoundFXManager.audioSource.Stop();

            dialoguePopUpGameObject.SetActive(false);
            currentDialogue.OnDialogueCancelled(aiCharacter);
        }

        private IEnumerator StretchPopUpTextOverTime(TextMeshProUGUI text, float duration, float stretchAmount)
        {
            if(duration > 0f)
            {
                text.characterSpacing = 0;
                float timer = 0;
                yield return null;

                while(timer < duration)
                {
                    timer = timer + Time.deltaTime;
                    text.characterSpacing = Mathf.Lerp(text.characterSpacing, stretchAmount, duration * (Time.deltaTime / 20));
                    yield return null;
                }
            }
        }

        private IEnumerator FadeInPopUpOverTime(CanvasGroup canvas, float duration)
        {
            if(duration > 0)
            {
                canvas.alpha = 0;
                float timer = 0;
                yield return null;
                while(timer < duration)
                {
                    timer = timer + Time.deltaTime;
                    canvas.alpha = Mathf.Lerp(canvas.alpha, 1, duration * Time.deltaTime);
                    yield return null;
                }
            }

            canvas.alpha = 1;
            yield return null;
        }

        private IEnumerator WaitThenFadeOutPopUpOverTime(CanvasGroup canvas, float duration, float delay)
        {
            if(duration > 0)
            {
                while(delay > 0)
                {
                    delay = delay - Time.deltaTime;
                    yield return null;
                }

                canvas.alpha = 1;
                float timer = 0;
                yield return null;
                while(timer < duration)
                {
                    timer = timer + Time.deltaTime;
                    canvas.alpha = Mathf.Lerp(canvas.alpha, 0, duration * Time.deltaTime);
                    yield return null;
                }
            }
            canvas.alpha = 0;
            yield return null;
        }

        private IEnumerator FadeOutThenDestroy(CanvasGroup canvas, float duration, GameObject objectToDestroy)
        {
            float timer = 0;

            while (timer < duration)
            {
                timer += Time.deltaTime;
                yield return null;
            }

            float fadeOutTimer = 1;

            while (fadeOutTimer > 0)
            {
                fadeOutTimer -= Time.deltaTime;
                canvas.alpha = fadeOutTimer;
                yield return null;
            }

            Destroy(objectToDestroy);

            yield return null;
        }
    }
}
