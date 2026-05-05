using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.UI;
using Unity.Netcode;
namespace baodeag
{
    public class PlayerUIPopUpManager : MonoBehaviour
    {
        private enum EndGameActionType
        {
            None = 0,
            RetryCurrentMap = 1,
            ContinueProgression = 2,
            ReturnToTitle = 3
        }

        [Header("Pop Up Parent")]
        [SerializeField] Transform popUpTransformParent; 

        [Header("Message Pop Up")]
        [SerializeField] TextMeshProUGUI popUpMessageText;
        [SerializeField] GameObject popUpMessageGameObject;
        [SerializeField] private GameObject statusEffectPopUpPrefab;
        [SerializeField] GameObject buffStatusPopUpGameObject;
        [SerializeField] UI_StatusEffectWarning buffStatusPopUpWarning;
        private Coroutine buffStatusPopUpCoroutine;

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
        private Coroutine delayedEndGameOverlayCoroutine;

        [Header("End Game Overlay")]
        [SerializeField] GameObject endGameOverlayGameObject;
        [SerializeField] CanvasGroup endGameOverlayCanvasGroup;
        [SerializeField] TextMeshProUGUI endGameTitleText;
        [SerializeField] TextMeshProUGUI endGameSubtitleText;
        [SerializeField] Button endGamePrimaryButton;
        [SerializeField] TextMeshProUGUI endGamePrimaryButtonText;
        [SerializeField] Button endGameSecondaryButton;
        [SerializeField] TextMeshProUGUI endGameSecondaryButtonText;

        private static readonly Color buffPopUpColor = new Color(0.96f, 0.84f, 0.42f, 1f);
        private EndGameActionType pendingPrimaryEndGameAction;
        private EndGameActionType pendingSecondaryEndGameAction;
        private const string WaitingForHostMessage = "Waiting for host to choose the next action...";

        public bool IsEndGameOverlayOpen()
        {
            return endGameOverlayGameObject != null && endGameOverlayGameObject.activeInHierarchy;
        }

        public void CloseAllPopUpWindows()
        {
            popUpMessageGameObject.SetActive(false);
            itemPopUpGameObject.SetActive(false);
            youDiedPopUpGameObject.SetActive(false);
            bossDefeatedPopUpGameObject.SetActive(false);
            graceRestoredPopUpGameObject.SetActive(false);
            dialoguePopUpGameObject.SetActive(false);

            if (buffStatusPopUpGameObject != null)
                buffStatusPopUpGameObject.SetActive(false);

            if (youDiedStylePopUpCoroutine != null)
            {
                StopCoroutine(youDiedStylePopUpCoroutine);
                youDiedStylePopUpCoroutine = null;
            }

            if (bossDefeatedPopUpCoroutine != null)
            {
                StopCoroutine(bossDefeatedPopUpCoroutine);
                bossDefeatedPopUpCoroutine = null;
            }

            if (graceRestoredStylePopUpCoroutine != null)
            {
                StopCoroutine(graceRestoredStylePopUpCoroutine);
                graceRestoredStylePopUpCoroutine = null;
            }

            if (dialogueCoroutine != null)
            {
                StopCoroutine(dialogueCoroutine);
                dialogueCoroutine = null;
            }

            bool endGameOverlayOpen = IsEndGameOverlayOpen();

            PlayerUIManager.instance.popUpWindowIsOpen = endGameOverlayOpen;

            if (!endGameOverlayOpen && !PlayerUIManager.instance.menuWindowIsOpen)
                PlayerUIManager.instance.playerUIHudManager?.ToggleHUDWithOutPopUps(true);
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

        public void SendLosePopUp(string loseMessage)
            => PlayYouDiedStylePopUp(loseMessage);

        public void ShowLoseEndGameOverlay()
        {
            ForceShowEndGameOverlay(
                "DEFEAT",
                "Your run ends here. Retry the map or return to the title screen.",
                "RETRY MAP",
                EndGameActionType.RetryCurrentMap,
                "HOME",
                EndGameActionType.ReturnToTitle);
        }

        public void ShowVictoryEndGameOverlay(bool canContinueProgression)
        {
            ForceShowEndGameOverlay(
                "VICTORY",
                canContinueProgression
                    ? "The path forward is open. Continue to the next stage or return to the title screen."
                    : "You cleared the encounter. Play again from the map start or return to the title screen.",
                canContinueProgression ? "CONTINUE" : "PLAY AGAIN",
                canContinueProgression ? EndGameActionType.ContinueProgression : EndGameActionType.RetryCurrentMap,
                "HOME",
                EndGameActionType.ReturnToTitle);
        }

        public void ShowVictoryEndGameOverlayDelayed(bool canContinueProgression, float delay)
        {
            if (delay <= 0f)
            {
                ShowVictoryEndGameOverlay(canContinueProgression);
                return;
            }

            if (delayedEndGameOverlayCoroutine != null)
            {
                StopCoroutine(delayedEndGameOverlayCoroutine);
                delayedEndGameOverlayCoroutine = null;
            }

            delayedEndGameOverlayCoroutine = StartCoroutine(ShowVictoryEndGameOverlayDelayedCoroutine(canContinueProgression, delay));
        }

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
            PlayBossStylePopUp(message);
        }

        private IEnumerator SendVictoryPopUpDelayedCoroutine(string message, float delay)
        {
            yield return new WaitForSeconds(delay);
            PlayYouDiedStylePopUp(message);
        }

        private IEnumerator ShowVictoryEndGameOverlayDelayedCoroutine(bool canContinueProgression, float delay)
        {
            yield return new WaitForSeconds(delay);

            ShowVictoryEndGameOverlay(canContinueProgression);

            delayedEndGameOverlayCoroutine = null;
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

        public void SendBuffPopUp(Item item)
        {
            if (item == null || buffStatusPopUpGameObject == null || buffStatusPopUpWarning == null)
                return;

            if (buffStatusPopUpCoroutine != null)
            {
                StopCoroutine(buffStatusPopUpCoroutine);
                buffStatusPopUpCoroutine = null;
            }

            buffStatusPopUpGameObject.SetActive(true);
            buffStatusPopUpWarning.canvas.alpha = 1f;
            buffStatusPopUpWarning.SetCustomMessage(item.itemName.ToUpperInvariant(), buffPopUpColor);
            buffStatusPopUpCoroutine = StartCoroutine(FadeOutExistingPopUp(buffStatusPopUpWarning.canvas, 2f, buffStatusPopUpGameObject));
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

        private IEnumerator FadeOutExistingPopUp(CanvasGroup canvas, float duration, GameObject popUpObject)
        {
            float timer = 0f;

            while (timer < duration)
            {
                timer += Time.deltaTime;
                yield return null;
            }

            float fadeOutTimer = 1f;

            while (fadeOutTimer > 0f)
            {
                fadeOutTimer -= Time.deltaTime;
                canvas.alpha = fadeOutTimer;
                yield return null;
            }

            canvas.alpha = 1f;
            popUpObject.SetActive(false);
            buffStatusPopUpCoroutine = null;
        }

        private void ForceShowEndGameOverlay(
            string title,
            string subtitle,
            string primaryButtonLabel,
            EndGameActionType primaryAction,
            string secondaryButtonLabel,
            EndGameActionType secondaryAction)
        {
            if (endGameOverlayGameObject == null ||
                endGameOverlayCanvasGroup == null ||
                endGameTitleText == null ||
                endGameSubtitleText == null ||
                endGamePrimaryButtonText == null ||
                endGameSecondaryButtonText == null)
            {
                Debug.LogWarning("PlayerUIPopUpManager: End game overlay references are missing.");
                return;
            }

            if (delayedEndGameOverlayCoroutine != null)
            {
                StopCoroutine(delayedEndGameOverlayCoroutine);
                delayedEndGameOverlayCoroutine = null;
            }

            pendingPrimaryEndGameAction = primaryAction;
            pendingSecondaryEndGameAction = secondaryAction;

            // Clear transient popups so the end-game board behaves like a full menu state.
            popUpMessageGameObject.SetActive(false);
            itemPopUpGameObject.SetActive(false);
            dialoguePopUpGameObject.SetActive(false);
            youDiedPopUpGameObject.SetActive(false);
            bossDefeatedPopUpGameObject.SetActive(false);
            graceRestoredPopUpGameObject.SetActive(false);

            if (youDiedStylePopUpCoroutine != null)
            {
                StopCoroutine(youDiedStylePopUpCoroutine);
                youDiedStylePopUpCoroutine = null;
            }

            if (bossDefeatedPopUpCoroutine != null)
            {
                StopCoroutine(bossDefeatedPopUpCoroutine);
                bossDefeatedPopUpCoroutine = null;
            }

            if (graceRestoredStylePopUpCoroutine != null)
            {
                StopCoroutine(graceRestoredStylePopUpCoroutine);
                graceRestoredStylePopUpCoroutine = null;
            }

            if (buffStatusPopUpGameObject != null)
                buffStatusPopUpGameObject.SetActive(false);

            endGameTitleText.text = title;
            endGameSubtitleText.text = BuildEndGameSubtitle(subtitle);
            endGamePrimaryButtonText.text = primaryButtonLabel;
            endGameSecondaryButtonText.text = secondaryButtonLabel;

            endGameOverlayCanvasGroup.alpha = 1f;
            endGameOverlayCanvasGroup.interactable = true;
            endGameOverlayCanvasGroup.blocksRaycasts = true;

            Transform currentTransform = endGameOverlayGameObject.transform;
            while (currentTransform != null)
            {
                currentTransform.gameObject.SetActive(true);
                currentTransform = currentTransform.parent;
            }

            endGameOverlayGameObject.SetActive(true);

            PlayerUIManager.instance.CloseAllMenuWindows();
            PlayerUIManager.instance.popUpWindowIsOpen = true;
            PlayerUIManager.instance.menuWindowIsOpen = true;

            if (PlayerInputManager.instance != null)
                PlayerInputManager.instance.SuppressGameplayInputs(true);

            if (PlayerUIManager.instance.playerUIHudManager != null)
                PlayerUIManager.instance.playerUIHudManager.ToggleHUDWithOutPopUps(false);

            ApplyEndGameActionAvailability();

            if (CanLocalPlayerControlEndGameActions() && endGamePrimaryButton != null)
                endGamePrimaryButton.Select();
        }

        private void HideEndGameOverlay()
        {
            if (endGameOverlayGameObject == null)
                return;

            endGameOverlayCanvasGroup.alpha = 0f;
            endGameOverlayCanvasGroup.interactable = false;
            endGameOverlayCanvasGroup.blocksRaycasts = false;
            endGameOverlayGameObject.SetActive(false);

            pendingPrimaryEndGameAction = EndGameActionType.None;
            pendingSecondaryEndGameAction = EndGameActionType.None;

            if (PlayerUIManager.instance != null)
            {
                PlayerUIManager.instance.popUpWindowIsOpen = false;
                PlayerUIManager.instance.menuWindowIsOpen = false;

                if (PlayerUIManager.instance.playerUIHudManager != null)
                    PlayerUIManager.instance.playerUIHudManager.ToggleHUDWithOutPopUps(true);
            }

            if (PlayerInputManager.instance != null)
                PlayerInputManager.instance.SuppressGameplayInputs(false);
        }

        public void HandlePrimaryEndGameButtonPressed()
        {
            ExecuteEndGameAction(pendingPrimaryEndGameAction);
        }

        public void HandleSecondaryEndGameButtonPressed()
        {
            ExecuteEndGameAction(pendingSecondaryEndGameAction);
        }

        public void DismissEndGameOverlayForTransition(bool showLoadingScreen)
        {
            if (IsEndGameOverlayOpen())
                HideEndGameOverlay();

            if (showLoadingScreen &&
                PlayerUIManager.instance != null &&
                PlayerUIManager.instance.playerUILoadingScreenManager != null)
            {
                PlayerUIManager.instance.playerUILoadingScreenManager.ActivateLoadingScreen();
            }
        }

        private string BuildEndGameSubtitle(string hostSubtitle)
        {
            if (!CanLocalPlayerControlEndGameActions())
                return WaitingForHostMessage;

            return hostSubtitle;
        }

        private void ApplyEndGameActionAvailability()
        {
            bool canControlActions = CanLocalPlayerControlEndGameActions();

            if (endGamePrimaryButton != null)
                endGamePrimaryButton.interactable = canControlActions;

            if (endGameSecondaryButton != null)
                endGameSecondaryButton.interactable = canControlActions;
        }

        private bool CanLocalPlayerControlEndGameActions()
        {
            if (WorldGameSessionManager.instance == null ||
                !WorldGameSessionManager.instance.IsMultiplayerSessionActive())
            {
                return true;
            }

            if (PlayerUIManager.instance == null || PlayerUIManager.instance.localPlayer == null)
                return false;

            return PlayerUIManager.instance.localPlayer.IsHost;
        }

        private void ExecuteEndGameAction(EndGameActionType action)
        {
            if (!CanLocalPlayerControlEndGameActions())
                return;

            HideEndGameOverlay();

            if (WorldGameSessionManager.instance == null)
                return;

            SessionEndGameActionType sessionAction = action switch
            {
                EndGameActionType.RetryCurrentMap => SessionEndGameActionType.RetryCurrentMap,
                EndGameActionType.ContinueProgression => SessionEndGameActionType.ContinueProgression,
                EndGameActionType.ReturnToTitle => SessionEndGameActionType.ReturnToTitle,
                _ => SessionEndGameActionType.None
            };

            if (sessionAction == SessionEndGameActionType.None)
                return;

            if (NetworkManager.Singleton != null &&
                NetworkManager.Singleton.IsClient &&
                WorldGameSessionManager.instance.IsMultiplayerSessionActive() &&
                PlayerUIManager.instance != null &&
                PlayerUIManager.instance.localPlayer != null &&
                PlayerUIManager.instance.localPlayer.playerNetworkManager != null)
            {
                bool showLoadingScreen = sessionAction != SessionEndGameActionType.ReturnToTitle;

                if (showLoadingScreen &&
                    PlayerUIManager.instance.playerUILoadingScreenManager != null)
                {
                    PlayerUIManager.instance.playerUILoadingScreenManager.ActivateLoadingScreen();
                }

                PlayerUIManager.instance.localPlayer.playerNetworkManager.RequestSynchronizedEndGameActionServerRpc((int)sessionAction);
                return;
            }

            WorldGameSessionManager.instance.ExecuteSynchronizedEndGameAction(sessionAction, true);
        }
    }
}
