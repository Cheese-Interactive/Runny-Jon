using System.Collections;
using System.Diagnostics;
using TMPro;
using UnityEngine;
using UnityEngine.Device;
using UnityEngine.UI;

public class UIController : MonoBehaviour {

    [Header("References")]
    private LevelManager gameManager;

    [Header("UI References")]
    [SerializeField] private CanvasGroup levelCompleteScreen;
    [SerializeField] private TMP_Text subtitleText;
    [SerializeField] private TMP_Text timeLimitText;
    [SerializeField] private TMP_Text timeText;
    [SerializeField] private CanvasGroup deathScreen;
    [SerializeField] private Button nextLevelButton;

    [Header("Timer")]
    [SerializeField] private TMP_Text timerText;
    private Coroutine timerCoroutine;

    [Header("Animations")]
    [SerializeField] private float subtitleTypeDuration;
    [SerializeField] private float subtitleFadeDuration;
    [SerializeField] private float deathScreenFadeDuration;
    [SerializeField] private float levelCompleteFadeInDuration;
    private Coroutine typeCoroutine;
    private Coroutine screenFadeInCoroutine;
    private Coroutine screenFadeOutCoroutine;
    private Coroutine textFadeInCoroutine;
    private Coroutine textFadeOutCoroutine;
    private bool textFaded;

    private void Start() {

        gameManager = FindObjectOfType<LevelManager>();

        FadeOutScreen(levelCompleteScreen, 0f);

        deathScreen.alpha = 0f;
        deathScreen.gameObject.SetActive(false);

    }

    public void TypeSubtitleText(string text) {

        if (typeCoroutine != null)
            StopCoroutine(typeCoroutine);

        typeCoroutine = StartCoroutine(StartTypeAnimation(text));

    }

    private IEnumerator StartTypeAnimation(string text) {

        FadeOutText(subtitleText, subtitleFadeDuration);

        while (!textFaded) {

            yield return null;

        }

        subtitleText.alpha = 1f;
        textFaded = false;

        subtitleText.gameObject.SetActive(true);
        subtitleText.maxVisibleCharacters = 0;
        subtitleText.text = text;
        int length = text.Length;

        while (subtitleText.maxVisibleCharacters < length) {

            yield return new WaitForSeconds(subtitleTypeDuration / length);
            subtitleText.maxVisibleCharacters++;

        }
    }

    private IEnumerator HandleLevelTimer() {

        Stopwatch stopwatch = new Stopwatch();
        stopwatch.Start();

        while (true) {

            timerText.text = string.Format("{0:00}{1:00}.{2:00}", (stopwatch.Elapsed.Minutes >= 1 ? stopwatch.Elapsed.Minutes + ":" : ""), stopwatch.Elapsed.Seconds, Mathf.Round(stopwatch.Elapsed.Milliseconds / 10f));
            yield return null;

        }
    }

    public IEnumerator ShowDeathScreen() {

        yield return StartCoroutine(FadeScreen(deathScreen, 1f, deathScreenFadeDuration, true));

    }

    public IEnumerator HideDeathScreen() {

        yield return StartCoroutine(FadeScreen(deathScreen, 0f, deathScreenFadeDuration, false));

    }

    public void ShowLevelCompleteScreen() {

        StopCoroutine(timerCoroutine);
        timeText.text = "Your Time: " + timerText.text;
        int timeLimit = gameManager.GetLevelTimeLimit();
        timeLimitText.text = "Time Limit: " + string.Format("{0:00}:{1:00}", timeLimit / 60, timeLimit % 60);
        FadeInScreen(levelCompleteScreen, 1f, levelCompleteFadeInDuration);

    }

    private void FadeInScreen(CanvasGroup screen, float targetOpacity, float duration) {

        if (screenFadeInCoroutine != null)
            StopCoroutine(screenFadeInCoroutine);

        screenFadeInCoroutine = StartCoroutine(FadeScreen(screen, targetOpacity, duration, true));

    }

    private void FadeOutScreen(CanvasGroup screen, float duration) {

        if (screenFadeOutCoroutine != null)
            StopCoroutine(screenFadeOutCoroutine);

        screenFadeOutCoroutine = StartCoroutine(FadeScreen(screen, 0f, duration, false));

    }

    private IEnumerator FadeScreen(CanvasGroup screen, float targetOpacity, float duration, bool fadeIn) {

        float currentTime = 0f;
        float startOpacity = screen.alpha;
        screen.gameObject.SetActive(true);

        while (currentTime < duration) {

            currentTime += Time.deltaTime;
            screen.alpha = Mathf.Lerp(startOpacity, targetOpacity, currentTime / duration);
            yield return null;

        }

        screen.alpha = targetOpacity;

        if (fadeIn) {

            screenFadeInCoroutine = null;

        } else {

            screen.gameObject.SetActive(false);
            screenFadeOutCoroutine = null;

        }
    }

    private void FadeInText(TMP_Text text, float targetOpacity, float duration) {

        if (textFadeInCoroutine != null)
            StopCoroutine(textFadeInCoroutine);

        textFadeInCoroutine = StartCoroutine(FadeText(text, targetOpacity, duration, true));

    }

    private void FadeOutText(TMP_Text text, float duration) {

        if (textFadeOutCoroutine != null)
            StopCoroutine(textFadeOutCoroutine);

        textFadeOutCoroutine = StartCoroutine(FadeText(text, 0f, duration, false));

    }

    private IEnumerator FadeText(TMP_Text text, float targetOpacity, float duration, bool fadeIn) {

        float currentTime = 0f;
        float startOpacity = text.alpha;
        text.gameObject.SetActive(true);

        while (currentTime < duration) {

            currentTime += Time.deltaTime;
            text.alpha = Mathf.Lerp(startOpacity, targetOpacity, currentTime / duration);
            yield return null;

        }

        text.alpha = targetOpacity;

        if (fadeIn) {

            textFadeInCoroutine = null;

        } else {

            text.gameObject.SetActive(false);
            textFadeOutCoroutine = null;
            textFaded = true;

        }
    }
}
