using System.Collections;
using System.Diagnostics;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameUIController : MonoBehaviour {

    [Header("References")]
    private PlayerController playerController;
    private GameManager gameManager;
    private Vector3 startTimerTextPos;
    private Transform startTimerTextParent;

    [Header("UI References")]
    [SerializeField] private TMP_Text subtitleText;
    [SerializeField] private CanvasGroup pauseMenu;
    [SerializeField] private Transform pauseTimerTextPos;
    [SerializeField] private Button pauseResumeButton;
    [SerializeField] private Button pauseMainMenuButton;
    [SerializeField] private Button pauseSettingsButton;
    [SerializeField] private CanvasGroup deathScreen;
    [SerializeField] private CanvasGroup interactIcon;
    [SerializeField] private CanvasGroup loadingScreen;

    [Header("Timer")]
    [SerializeField] private TMP_Text timerText;

    [Header("Level Complete Menu")]
    [SerializeField] private CanvasGroup levelCompleteScreen;
    [SerializeField] private TMP_Text timeText;
    [SerializeField] private GameObject recordText;
    [SerializeField] private TMP_Text deathsText;
    [SerializeField] private Button mainMenuButton;
    [SerializeField] private Button replayButton;

    [Header("Animations")]
    [SerializeField] private float subtitleTypeDuration;
    [SerializeField] private float subtitleFadeDuration;
    [SerializeField] private float interactIconFadeDuration;
    [SerializeField] private float pauseMenuFadeDuration;
    [SerializeField] private float deathScreenFadeDuration;
    [SerializeField] private float levelCompleteFadeInDuration;
    [SerializeField] private float loadingScreenFadeDuration;
    private Coroutine typeCoroutine;
    private Coroutine screenFadeCoroutine;
    private Coroutine textFadeCoroutine;
    private Coroutine interactIconFadeCoroutine;
    private bool textFaded;

    private void Start() {

        playerController = FindObjectOfType<PlayerController>();
        gameManager = FindObjectOfType<GameManager>();

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        loadingScreen.alpha = 1f;
        FadeOutScreen(loadingScreen, loadingScreenFadeDuration);

        startTimerTextPos = timerText.rectTransform.localPosition;
        startTimerTextParent = timerText.rectTransform.parent;

        pauseResumeButton.onClick.AddListener(ResumeGame);
        pauseMainMenuButton.onClick.AddListener(OpenMainMenu);
        mainMenuButton.onClick.AddListener(OpenMainMenu);
        replayButton.onClick.AddListener(ReplayLevel);

        pauseMenu.alpha = 0f;
        deathScreen.alpha = 0f;
        levelCompleteScreen.alpha = 0f;
        pauseMenu.gameObject.SetActive(false);
        deathScreen.gameObject.SetActive(false);
        interactIcon.gameObject.SetActive(false);
        levelCompleteScreen.gameObject.SetActive(false);

    }

    public void TypeSubtitleText(string text) {

        if (typeCoroutine != null)
            StopCoroutine(typeCoroutine);

        typeCoroutine = StartCoroutine(StartTypeAnimation(text));

    }

    private IEnumerator StartTypeAnimation(string text) {

        FadeOutText(subtitleText, subtitleFadeDuration);

        while (!textFaded)
            yield return null;

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

    public IEnumerator HandleLevelTimer() {

        Stopwatch stopwatch;

        while (true) {

            stopwatch = gameManager.GetTimer();

            timerText.text = string.Format("{0:00}{1:00}.{2:00}", (stopwatch.Elapsed.Minutes >= 1 ? stopwatch.Elapsed.Minutes + ":" : ""), stopwatch.Elapsed.Seconds, Mathf.Round(stopwatch.Elapsed.Milliseconds / 10f));
            yield return null;

        }
    }

    public void PauseGame() {

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        timerText.transform.SetParent(pauseTimerTextPos);
        timerText.rectTransform.localPosition = Vector3.zero;
        subtitleText.gameObject.SetActive(false);
        gameManager.PauseTimer();
        FadeInScreen(pauseMenu, 1f, pauseMenuFadeDuration);

    }

    public void ResumeGame() {

        FadeOutScreen(pauseMenu, pauseMenuFadeDuration);
        gameManager.ResumeTimer();
        timerText.transform.SetParent(startTimerTextParent);
        timerText.rectTransform.localPosition = startTimerTextPos;
        subtitleText.gameObject.SetActive(true);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

    }

    public void OpenMainMenu() {

        SceneManager.LoadScene(gameManager.GetMainMenuScene().name);

    }

    public void ReplayLevel() {

        SceneManager.LoadScene(SceneManager.GetActiveScene().name);

    }

    public void FadeInInteractIcon() {

        if (interactIconFadeCoroutine != null)
            StopCoroutine(interactIconFadeCoroutine);

        interactIconFadeCoroutine = StartCoroutine(FadeInteractIcon(1f, interactIconFadeDuration, true));

    }

    public void FadeOutInteractIcon() {

        if (interactIconFadeCoroutine != null)
            StopCoroutine(interactIconFadeCoroutine);

        interactIconFadeCoroutine = StartCoroutine(FadeInteractIcon(0f, interactIconFadeDuration, false));

    }

    private IEnumerator FadeInteractIcon(float targetOpacity, float duration, bool fadeIn) {

        float currentTime = 0f;
        float startOpacity = interactIcon.alpha;
        interactIcon.gameObject.SetActive(true);

        while (currentTime < duration) {

            currentTime += Time.deltaTime;
            interactIcon.alpha = Mathf.Lerp(startOpacity, targetOpacity, currentTime / duration);
            yield return null;

        }

        interactIcon.alpha = targetOpacity;
        interactIconFadeCoroutine = null;

        if (!fadeIn) {

            interactIcon.gameObject.SetActive(false);

        }
    }

    public IEnumerator ShowDeathScreen() {

        yield return StartCoroutine(FadeScreen(deathScreen, 1f, deathScreenFadeDuration, true));

    }

    public IEnumerator HideDeathScreen() {

        yield return StartCoroutine(FadeScreen(deathScreen, 0f, deathScreenFadeDuration, false));

    }

    public void ShowLevelCompleteScreen(bool newRecord, int deaths) {

        timeText.text = "Your Time: " + timerText.text;
        recordText.gameObject.SetActive(newRecord);
        deathsText.text = "Deaths: " + deaths;
        FadeInScreen(levelCompleteScreen, 1f, levelCompleteFadeInDuration);

    }

    private void FadeInScreen(CanvasGroup screen, float targetOpacity, float duration) {

        if (screenFadeCoroutine != null)
            StopCoroutine(screenFadeCoroutine);

        screenFadeCoroutine = StartCoroutine(FadeScreen(screen, targetOpacity, duration, true));

    }

    private void FadeOutScreen(CanvasGroup screen, float duration) {

        if (screenFadeCoroutine != null)
            StopCoroutine(screenFadeCoroutine);

        screenFadeCoroutine = StartCoroutine(FadeScreen(screen, 0f, duration, false));

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
        screenFadeCoroutine = null;

        if (!fadeIn) {

            screen.gameObject.SetActive(false);

        }
    }

    private void FadeInText(TMP_Text text, float targetOpacity, float duration) {

        if (textFadeCoroutine != null)
            StopCoroutine(textFadeCoroutine);

        textFadeCoroutine = StartCoroutine(FadeText(text, targetOpacity, duration, true));

    }

    private void FadeOutText(TMP_Text text, float duration) {

        if (textFadeCoroutine != null)
            StopCoroutine(textFadeCoroutine);

        textFadeCoroutine = StartCoroutine(FadeText(text, 0f, duration, false));

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
        textFadeCoroutine = null;

        if (!fadeIn) {

            text.gameObject.SetActive(false);
            textFaded = true;

        }
    }
}
