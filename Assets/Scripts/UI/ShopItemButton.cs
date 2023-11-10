using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopItemButton : MonoBehaviour {

    [Header("References")]
    [SerializeField] private Image icon;
    private Image background;
    private MenuManager menuManager;
    private MenuAudioManager audioManager;
    private Button button;
    private Color startColor;

    [Header("UI References")]
    [SerializeField] private TMP_Text nameText;
    [SerializeField] private TMP_Text priceText;
    private ShopItem shopItem;

    [Header("Animations")]
    [SerializeField] private Color errorColor;
    [SerializeField] private float errorDisplayDuration;
    [SerializeField] private float errorResetDuration;
    private Coroutine errorCoroutine;

    // Start Function
    public void SetMenuManager(MenuManager menuManager) {

        this.menuManager = menuManager;
        audioManager = FindObjectOfType<MenuAudioManager>();
        background = GetComponent<Image>();
        button = GetComponent<Button>();
        button.onClick.AddListener(PurchaseItem);

        startColor = background.color;

    }

    private void PurchaseItem() {

        if (menuManager.PurchaseItem(shopItem)) {

            button.interactable = false;
            audioManager.PlaySound(MenuAudioManager.UISoundEffectType.Success);

        } else {

            if (errorCoroutine != null)
                StopCoroutine(errorCoroutine);

            errorCoroutine = StartCoroutine(ShowErrorButton(background, errorColor, errorDisplayDuration, errorResetDuration));
            audioManager.PlaySound(MenuAudioManager.UISoundEffectType.Error);

        }
    }

    public Button GetButton() {

        return button;

    }

    public void SetIcon(Image icon) {

        this.icon = icon;

    }

    public void SetNameText(string nameText) {

        this.nameText.text = nameText;

    }

    public void SetPriceText(string priceText) {

        this.priceText.text = priceText;

    }

    public void SetShopItem(ShopItem shopItem) {

        this.shopItem = shopItem;

    }

    private IEnumerator ShowErrorButton(Image image, Color targetColor, float displayDuration, float resetDuration) {

        float currentTime = 0f;
        image.color = targetColor;
        yield return new WaitForSeconds(displayDuration);

        while (currentTime < resetDuration) {

            currentTime += Time.deltaTime;
            image.color = Color.Lerp(targetColor, startColor, currentTime / resetDuration);
            yield return null;
        }

        image.color = startColor;
        errorCoroutine = null;

    }
}
