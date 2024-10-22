using System.Collections;
using System.Diagnostics;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameUIController : MonoBehaviour {

    [Header("References")]
    private Animator animator;
    private PlayerController playerController;
    private GameManager gameManager;
    private Vector3 startTimerTextPos;
    private Transform startTimerTextParent;

    [Header("UI References")]
    [SerializeField] private Image crosshair;
    [SerializeField] private TMP_Text interactText;
    [SerializeField] private Sprite interactCrosshair;
    [SerializeField] private TMP_Text subtitleText;
    [SerializeField] private CanvasGroup pauseMenu;
    [SerializeField] private Transform pauseTimerTextPos;
    [SerializeField] private Button pauseResumeButton;
    [SerializeField] private Button pauseRestartButton;
    [SerializeField] private Button pauseMainMenuButton;
    [SerializeField] private CanvasGroup deathScreen;
    [SerializeField] private Image wipeScreen;
    private Sprite defaultCrosshair;
    private bool defaultCrosshairEnabled;

    [Header("Timer")]
    [SerializeField] private TMP_Text timerText;

    [Header("Level Complete Menu")]
    [SerializeField] private CanvasGroup levelCompleteScreen;
    [SerializeField] private RectTransform timeLayoutGroup;
    [SerializeField] private TMP_Text timeText;
    [SerializeField] private GameObject recordText;
    [SerializeField] private TMP_Text deathsText;
    [SerializeField] private Button mainMenuButton;
    [SerializeField] private Button replayButton;

    [Header("Animations")]
    [SerializeField] private float subtitleTypeDuration;
    [SerializeField] private float subtitleFadeDuration;
    [SerializeField] private float pauseMenuFadeDuration;
    [SerializeField] private float deathScreenFadeDuration;
    private Coroutine typeCoroutine;
    private Coroutine screenFadeCoroutine;
    private Coroutine textFadeCoroutine;
    private bool textFaded;
    private bool levelComplete;

    private void Start() {

        animator = GetComponent<Animator>();
        playerController = FindObjectOfType<PlayerController>();
        gameManager = FindObjectOfType<GameManager>();

        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        startTimerTextPos = timerText.rectTransform.localPosition;
        startTimerTextParent = timerText.rectTransform.parent;

        pauseResumeButton.onClick.AddListener(ResumeButtonClicked);
        pauseRestartButton.onClick.AddListener(ReloadLevel);
        pauseMainMenuButton.onClick.AddListener(OpenMainMenu);
        mainMenuButton.onClick.AddListener(OpenMainMenu);
        replayButton.onClick.AddListener(ReloadLevel);

        pauseMenu.alpha = 0f;
        deathScreen.alpha = 0f;
        pauseMenu.gameObject.SetActive(false);
        deathScreen.gameObject.SetActive(false);
        levelCompleteScreen.gameObject.SetActive(false);

        wipeScreen.gameObject.SetActive(true);

        // hide wipe screen
        animator.SetTrigger("hideWipeScreen");

        // show default crosshair
        defaultCrosshairEnabled = true;
        defaultCrosshair = crosshair.sprite;
        EnableCrosshair();

    }

    private void Update() {

        // if game unfocuses in standalone build, pause game
        if (!Application.isEditor && !Application.isFocused)
            PauseGame();

    }

    public void EnableCrosshair() {

        crosshair.gameObject.SetActive(true);

    }

    public void DisableCrosshair() {

        crosshair.gameObject.SetActive(false);

    }

    public void EnableInteractCrosshair(string text) {

        SetInteractText(text);

        if (!defaultCrosshairEnabled)
            return;

        interactText.gameObject.SetActive(true);

        crosshair.sprite = interactCrosshair;
        defaultCrosshairEnabled = false;

    }

    public void DisableInteractCrosshair() {

        if (defaultCrosshairEnabled)
            return;

        interactText.gameObject.SetActive(false);
        SetInteractText("");

        crosshair.sprite = defaultCrosshair;
        defaultCrosshairEnabled = true;

    }

    private void SetInteractText(string text) {

        interactText.text = text;

    }

    public void TypeSubtitleText(string text) {

        if (typeCoroutine != null)
            StopCoroutine(typeCoroutine);

        typeCoroutine = StartCoroutine(HandleTypeAnimation(text));

    }

    private IEnumerator HandleTypeAnimation(string text) {

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

    private void ResumeButtonClicked() {

        ResumeGame();

    }

    public void PauseGame() {

        if (gameManager.GetGamePaused() || levelComplete)
            return;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        timerText.transform.SetParent(pauseTimerTextPos);
        timerText.rectTransform.localPosition = Vector3.zero;
        subtitleText.gameObject.SetActive(false);
        FadeInScreen(pauseMenu, 1f, pauseMenuFadeDuration);
        gameManager.PauseGame();
        playerController.DisableAllMovement();
        playerController.DisableLook();

    }

    public void ResumeGame() {

        if (!gameManager.GetGamePaused())
            return;

        FadeOutScreen(pauseMenu, pauseMenuFadeDuration);
        timerText.transform.SetParent(startTimerTextParent);
        timerText.rectTransform.localPosition = startTimerTextPos;
        subtitleText.gameObject.SetActive(true);
        gameManager.ResumeGame();
        playerController.EnableAllMovement();
        playerController.EnableLook();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

    }

    public void OpenMainMenu() {

        levelComplete = false;
        StartCoroutine(LoadLevel(0));

    }

    public void ReloadLevel() {

        levelComplete = false;
        StartCoroutine(LoadLevel(SceneManager.GetActiveScene().buildIndex));

    }

    public IEnumerator LoadLevel(int buildIndex) {

        yield return ShowWipeScreen();
        AsyncOperation operation = SceneManager.LoadSceneAsync(buildIndex);
        operation.allowSceneActivation = false;
        float currentTime = 0f;

        while (!operation.isDone && operation.progress < 0.9f) {

            currentTime += Time.unscaledDeltaTime;
            yield return null;

        }

        operation.allowSceneActivation = true;
        Time.timeScale = 1f;

    }

    public IEnumerator ShowDeathScreen() {

        yield return StartCoroutine(FadeScreen(deathScreen, 1f, deathScreenFadeDuration, true));

    }

    public IEnumerator HideDeathScreen() {

        yield return StartCoroutine(FadeScreen(deathScreen, 0f, deathScreenFadeDuration, false));

    }

    public void ShowLevelCompleteScreen(bool newRecord, int deaths) {

        // flag level as complete
        levelComplete = true;

        // update time text
        timeText.text = "Your Time: " + timerText.text;

        // enable record text if there's a new record
        recordText.gameObject.SetActive(newRecord);

        // update death count
        deathsText.text = "Deaths: " + deaths;

        // activate level complete screen
        levelCompleteScreen.gameObject.SetActive(true);

        // rebuild level complete layouts
        LayoutRebuilder.ForceRebuildLayoutImmediate(levelCompleteScreen.GetComponent<RectTransform>());
        LayoutRebuilder.ForceRebuildLayoutImmediate(timeLayoutGroup.GetComponent<RectTransform>());

        // play animation
        animator.SetTrigger("showLevelComplete");

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

    private IEnumerator ShowWipeScreen() {

        // start animation
        animator.SetTrigger("showWipeScreen");

        // wait for animation to end
        yield return new WaitForSecondsRealtime(animator.GetCurrentAnimatorClipInfo(0)[0].clip.length);

    }

    private IEnumerator FadeScreen(CanvasGroup screen, float targetOpacity, float duration, bool fadeIn) {

        float currentTime = 0f;
        float startOpacity = screen.alpha;
        screen.gameObject.SetActive(true);

        while (currentTime < duration) {

            currentTime += Time.unscaledDeltaTime;
            screen.alpha = Mathf.Lerp(startOpacity, targetOpacity, currentTime / duration);
            yield return null;

        }

        screen.alpha = targetOpacity;
        screenFadeCoroutine = null;

        if (!fadeIn)
            screen.gameObject.SetActive(false);

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

            currentTime += Time.unscaledDeltaTime;
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
