using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuUIController : MonoBehaviour {

    [Header("References")]
    private MenuManager menuManager;
    private PlayerData playerData;
    private MenuAudioManager audioManager;

    [Header("UI References")]
    [SerializeField] private CanvasGroup mainMenu;
    [SerializeField] private Button playButton;
    [SerializeField] private Button shopButton;
    [SerializeField] private Button settingsButton;
    [SerializeField] private Button quitButton;
    [SerializeField] private CanvasGroup levelMenu;
    [SerializeField] private Button levelMenuBackButton;
    [SerializeField] private CanvasGroup loadingScreen;
    [SerializeField] private TMP_Text loadingText;

    [Header("Shop")]
    [SerializeField] private CanvasGroup shopMenu;
    [SerializeField] private Button shopMenuBackButton;
    [SerializeField] private TMP_Text shopQuesoText;

    [Header("Animations")]
    [SerializeField] private float quesoCountUpdateDuration;
    [SerializeField] private float loadingScreenFadeDuration;
    [SerializeField] private float minLoadingDuration;
    private Coroutine quesoCountCoroutine;
    private Coroutine screenFadeInCoroutine;
    private Coroutine screenFadeOutCoroutine;
    private Coroutine loadingScreenFadeCoroutine;

    private void Start() {

        menuManager = FindObjectOfType<MenuManager>();
        playerData = FindObjectOfType<PlayerData>();
        audioManager = FindObjectOfType<MenuAudioManager>();

        SetLoadingText("Loading Main Menu...");
        loadingScreen.alpha = 1f;
        FadeOutScreen(loadingScreen, loadingScreenFadeDuration);

        shopQuesoText.text = playerData.GetQuesos() + "";

        playButton.onClick.AddListener(PlayClicked);
        shopButton.onClick.AddListener(ShopClicked);
        quitButton.onClick.AddListener(QuitClicked);

        levelMenuBackButton.onClick.AddListener(CloseLevelMenu);
        shopMenuBackButton.onClick.AddListener(CloseShopMenu);

        levelMenu.gameObject.SetActive(false);
        shopMenu.gameObject.SetActive(false);

        mainMenu.gameObject.SetActive(true);

        foreach (Button button in FindObjectsOfType<Button>(true))
            button.onClick.AddListener(PlayClickSound);

    }

    private void PlayClickSound() {

        audioManager.PlaySound(MenuAudioManager.UISoundEffectType.Click);

    }

    private void PlayClicked() {

        OpenLevelMenu();

    }

    private void ShopClicked() {

        OpenShopMenu();

    }

    private void QuitClicked() {

        Application.Quit();

    }

    private void OpenLevelMenu() {

        mainMenu.gameObject.SetActive(false);
        levelMenu.gameObject.SetActive(true);

    }

    private void CloseLevelMenu() {

        if (menuManager.IsLevelSectionOpen()) {

            menuManager.CloseLevelLayout(menuManager.GetCurrentLevelLayout());
            return;

        }

        levelMenu.gameObject.SetActive(false);
        mainMenu.gameObject.SetActive(true);

    }

    private void OpenShopMenu() {

        mainMenu.gameObject.SetActive(false);
        shopMenu.gameObject.SetActive(true);

    }

    private void CloseShopMenu() {

        shopMenu.gameObject.SetActive(false);
        mainMenu.gameObject.SetActive(true);

    }

    public void UpdateQuesoCount() {

        if (quesoCountCoroutine != null)
            StopCoroutine(quesoCountCoroutine);

        quesoCountCoroutine = StartCoroutine(LerpQuesoCount(playerData.GetQuesos(), quesoCountUpdateDuration));

    }

    private IEnumerator LerpQuesoCount(int targetCount, float duration) {

        float currentTime = 0f;
        int startCount = int.Parse(shopQuesoText.text);

        while (currentTime < duration) {

            currentTime += Time.unscaledDeltaTime;
            shopQuesoText.text = Mathf.RoundToInt(Mathf.Lerp(startCount, targetCount, currentTime / duration)) + "";
            yield return null;

        }

        shopQuesoText.text = targetCount + "";
        quesoCountCoroutine = null;

    }

    public void FadeInScreen(CanvasGroup screen, float targetOpacity, float duration) {

        if (screenFadeInCoroutine != null)
            StopCoroutine(screenFadeInCoroutine);

        screenFadeInCoroutine = StartCoroutine(FadeScreen(screen, targetOpacity, duration, true));

    }

    private void FadeOutScreen(CanvasGroup screen, float duration) {

        if (screenFadeOutCoroutine != null)
            StopCoroutine(screenFadeOutCoroutine);

        screenFadeOutCoroutine = StartCoroutine(FadeScreen(screen, 0f, duration, false));

    }

    public IEnumerator FadeInLoadingScreen() {

        if (loadingScreenFadeCoroutine != null)
            StopCoroutine(loadingScreenFadeCoroutine);

        loadingScreenFadeCoroutine = StartCoroutine(FadeScreen(loadingScreen, 1f, loadingScreenFadeDuration, true));
        yield return loadingScreenFadeCoroutine;

    }

    private IEnumerator FadeScreen(CanvasGroup screen, float targetOpacity, float duration, bool fadeIn) {

        if (fadeIn)
            screen.alpha = 0f;

        float currentTime = 0f;
        float startOpacity = screen.alpha;
        screen.gameObject.SetActive(true);

        while (currentTime < duration) {

            currentTime += Time.deltaTime;
            screen.alpha = Mathf.Lerp(startOpacity, targetOpacity, currentTime / duration);
            yield return null;

        }

        screen.alpha = targetOpacity;
        screenFadeOutCoroutine = null;

        if (!fadeIn)
            screen.gameObject.SetActive(false);

    }

    public void SetLoadingText(string text) {

        loadingText.text = text;

    }

    public float GetMinLoadingDuration() {

        return minLoadingDuration;

    }
}