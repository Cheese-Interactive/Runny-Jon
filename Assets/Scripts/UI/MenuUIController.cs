using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuUIController : MonoBehaviour {

    [Header("UI References")]
    [SerializeField] private CanvasGroup mainMenu;
    [SerializeField] private CanvasGroup levelMenu;
    [SerializeField] private Button playButton;
    [SerializeField] private Button settingsButton;
    [SerializeField] private Button quitButton;

    [Header("Animations")]
    [SerializeField] private float mainMenuFadeDuration;
    [SerializeField] private float levelMenuFadeDuration;
    private Coroutine screenFadeInCoroutine;
    private Coroutine screenFadeOutCoroutine;

    private void Start() {

        playButton.onClick.AddListener(PlayClicked);
        quitButton.onClick.AddListener(QuitClicked);

        levelMenu.gameObject.SetActive(false);
        mainMenu.alpha = 0f;
        FadeInScreen(mainMenu, 1f, mainMenuFadeDuration);

    }

    private void PlayClicked() {

        mainMenu.gameObject.SetActive(false);
        FadeInScreen(levelMenu, 1f, levelMenuFadeDuration);

    }

    private void QuitClicked() {

        Application.Quit();

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

        if (fadeIn) {

            screenFadeInCoroutine = null;

        } else {

            screen.gameObject.SetActive(false);
            screenFadeOutCoroutine = null;

        }
    }
}
