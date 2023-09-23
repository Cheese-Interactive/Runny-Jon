using System.Collections;
using System.Diagnostics;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour {

    [Header("References")]
    private GameManager gameManager;

    [Header("UI References")]
    [SerializeField] private CanvasGroup levelCompleteScreen;
    [SerializeField] private TMP_Text tutorialText;
    [SerializeField] private TMP_Text timeLimitText;
    [SerializeField] private TMP_Text timeText;
    [SerializeField] private Button nextLevelButton;

    [Header("Timer")]
    [SerializeField] private TMP_Text timerText;
    private Coroutine timerCoroutine;
    private int timer;

    [Header("Animations")]
    [SerializeField] private float totalTypeTime;
    [SerializeField] private float levelCompleteFadeInDuration;
    private Coroutine typeCoroutine;
    private Coroutine fadeInCoroutine;
    private Coroutine fadeOutCoroutine;

    private void Start() {

        gameManager = FindObjectOfType<GameManager>();

        FadeOutScreen(levelCompleteScreen, 0f);
        StartLevelTimer();

    }

    public void TypeSubtitleText(string text) {

        if (typeCoroutine != null)
            StopCoroutine(typeCoroutine);

        typeCoroutine = StartCoroutine(HandleTypeAnimation(text));

    }

    private IEnumerator HandleTypeAnimation(string text) {

        tutorialText.gameObject.SetActive(true);
        tutorialText.maxVisibleCharacters = 0;
        tutorialText.text = text;
        int length = text.Length;

        while (tutorialText.maxVisibleCharacters < length) {

            yield return new WaitForSeconds(totalTypeTime / length);
            tutorialText.maxVisibleCharacters++;

        }
    }

    private void StartLevelTimer() {

        timerCoroutine = StartCoroutine(HandleLevelTimer());

    }

    private IEnumerator HandleLevelTimer() {

        Stopwatch stopwatch = new Stopwatch();
        stopwatch.Start();

        while (true) {

            timerText.text = string.Format("{0:00}{1:00}.{2:00}", (stopwatch.Elapsed.Minutes >= 1 ? stopwatch.Elapsed.Minutes + ":" : ""), stopwatch.Elapsed.Seconds, Mathf.Round(stopwatch.Elapsed.Milliseconds / 10f));
            yield return null;

        }
    }

    public void ShowLevelCompleteScreen() {

        StopCoroutine(timerCoroutine);
        timeText.text = "Your Time: " + timerText.text;
        int timeLimit = gameManager.GetLevelTimeLimit();
        timeLimitText.text = "Time Limit: " + string.Format("{0:00}:{1:00}", timeLimit / 60, timeLimit % 60);
        FadeInScreen(levelCompleteScreen, 1f, levelCompleteFadeInDuration);

    }

    private void FadeInScreen(CanvasGroup screen, float targetOpacity, float duration) {

        if (fadeInCoroutine != null)
            StopCoroutine(fadeInCoroutine);

        fadeInCoroutine = StartCoroutine(FadeScreen(screen, targetOpacity, duration, true));

    }

    private void FadeOutScreen(CanvasGroup screen, float duration) {

        if (fadeOutCoroutine != null) {

            StopCoroutine(fadeOutCoroutine);

        }

        fadeOutCoroutine = StartCoroutine(FadeScreen(screen, 0f, duration, false));

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

            fadeInCoroutine = null;

        } else {

            screen.gameObject.SetActive(false);
            fadeOutCoroutine = null;

        }
    }
}
