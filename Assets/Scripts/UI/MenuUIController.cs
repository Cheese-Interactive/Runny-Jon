using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MenuUIController : MonoBehaviour {

    [Header("References")]
    private Animator animator;
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
    [SerializeField] private Image wipeScreen;

    [Header("Shop")]
    [SerializeField] private CanvasGroup shopMenu;
    [SerializeField] private Button shopMenuBackButton;
    [SerializeField] private TMP_Text shopQuesoText;

    [Header("Animations")]
    [SerializeField] private float quesoCountUpdateDuration;
    private Coroutine quesoCountCoroutine;
    private Coroutine screenFadeInCoroutine;
    private Coroutine screenFadeOutCoroutine;

    private void Start() {

        animator = GetComponent<Animator>();
        menuManager = FindObjectOfType<MenuManager>();
        playerData = FindObjectOfType<PlayerData>();
        audioManager = FindObjectOfType<MenuAudioManager>();

        //SetLoadingText("Loading Main Menu...");
        //loadingScreen.alpha = 1f;
        wipeScreen.gameObject.SetActive(true);
        StartCoroutine(FadeOutWipeScreen());

        shopQuesoText.text = playerData.GetQuesos() + "";

        playButton.onClick.AddListener(() => StartCoroutine(OpenLevelMenu()));
        shopButton.onClick.AddListener(() => StartCoroutine(OpenShopMenu()));
        quitButton.onClick.AddListener(QuitClicked);

        levelMenuBackButton.onClick.AddListener(() => StartCoroutine(CloseLevelMenu()));
        shopMenuBackButton.onClick.AddListener(() => StartCoroutine(CloseShopMenu()));

        levelMenu.gameObject.SetActive(false);
        shopMenu.gameObject.SetActive(false);

        mainMenu.gameObject.SetActive(true);

        foreach (Button button in FindObjectsOfType<Button>(true))
            button.onClick.AddListener(PlayClickSound);

    }

    private void PlayClickSound() {

        audioManager.PlaySound(MenuAudioManager.UISoundEffectType.Click);

    }

    private void QuitClicked() {

        Application.Quit();

    }

    private IEnumerator OpenLevelMenu() {

        // enable wipe screen
        wipeScreen.gameObject.SetActive(true);

        // disable interacting to prevent multiple animations
        mainMenu.interactable = false;

        // start unload animation
        animator.SetTrigger("unload");

        // wait for unload animation to end
        yield return new WaitForSeconds(animator.GetCurrentAnimatorClipInfo(0)[0].clip.length);

        // disable main menu / enable level menu
        mainMenu.gameObject.SetActive(false);
        levelMenu.gameObject.SetActive(true);

        // start load animation
        animator.SetTrigger("load");

        // wait for load animation to end
        yield return new WaitForSeconds(animator.GetCurrentAnimatorClipInfo(0)[0].clip.length);

        // enable interacting on new menu
        levelMenu.interactable = true;

        // disable wipe screen
        wipeScreen.gameObject.SetActive(false);

    }

    private IEnumerator CloseLevelMenu() {

        if (menuManager.IsLevelSectionOpen()) {

            menuManager.CloseLevelLayout(menuManager.GetCurrentLevelLayout());
            yield break;

        }

        // enable wipe screen
        wipeScreen.gameObject.SetActive(true);

        // disable interacting to prevent multiple animations
        levelMenu.interactable = false;

        // start unload animation
        animator.SetTrigger("unload");

        // wait for unload animation to end
        yield return new WaitForSeconds(animator.GetCurrentAnimatorClipInfo(0)[0].clip.length);

        // disable level menu / enable main menu
        levelMenu.gameObject.SetActive(false);
        mainMenu.gameObject.SetActive(true);

        // start load animation
        animator.SetTrigger("load");

        // wait for load animation to end
        yield return new WaitForSeconds(animator.GetCurrentAnimatorClipInfo(0)[0].clip.length);

        // enable interacting on new menu
        mainMenu.interactable = true;

        // disable wipe screen
        wipeScreen.gameObject.SetActive(false);

    }

    private IEnumerator OpenShopMenu() {

        // enable wipe screen
        wipeScreen.gameObject.SetActive(true);

        // disable interacting to prevent multiple animations
        mainMenu.interactable = false;

        // start unload animation
        animator.SetTrigger("unload");

        // wait for unload animation to end
        yield return new WaitForSeconds(animator.GetCurrentAnimatorClipInfo(0)[0].clip.length);

        // disable main menu / enable shop menu
        mainMenu.gameObject.SetActive(false);
        shopMenu.gameObject.SetActive(true);

        // start load animation
        animator.SetTrigger("load");

        // wait for load animation to end
        yield return new WaitForSeconds(animator.GetCurrentAnimatorClipInfo(0)[0].clip.length);

        // enable interacting on new menu
        shopMenu.interactable = true;

        // disable wipe screen
        wipeScreen.gameObject.SetActive(false);

    }

    private IEnumerator CloseShopMenu() {

        // enable wipe screen
        wipeScreen.gameObject.SetActive(true);

        // disable interacting to prevent multiple animations
        shopMenu.interactable = false;

        // start unload animation
        animator.SetTrigger("unload");

        // wait for unload animation to end
        yield return new WaitForSeconds(animator.GetCurrentAnimatorClipInfo(0)[0].clip.length);

        // disable shop menu / enable main menu
        shopMenu.gameObject.SetActive(false);
        mainMenu.gameObject.SetActive(true);

        // start load animation
        animator.SetTrigger("load");

        // wait for load animation to end
        yield return new WaitForSeconds(animator.GetCurrentAnimatorClipInfo(0)[0].clip.length);

        // enable interacting on new menu
        mainMenu.interactable = true;

        // disable wipe screen
        wipeScreen.gameObject.SetActive(false);

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

    public IEnumerator FadeInWipeScreen() {

        // enable wipe screen
        wipeScreen.gameObject.SetActive(true);

        // start unload animation
        animator.SetTrigger("unload");

        // wait for unload animation to end
        yield return new WaitForSeconds(animator.GetCurrentAnimatorClipInfo(0)[0].clip.length);

    }

    private IEnumerator FadeOutWipeScreen() {

        // enable wipe screen
        wipeScreen.gameObject.SetActive(true);

        // start unload animation
        animator.SetTrigger("load");

        // wait for load animation to end
        yield return new WaitForSeconds(animator.GetCurrentAnimatorClipInfo(0)[0].clip.length);

        // disable wipe screen
        wipeScreen.gameObject.SetActive(false);

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
}