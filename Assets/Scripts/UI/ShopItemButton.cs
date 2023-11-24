using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopItemButton : MonoBehaviour {

    [Header("References")]
    private Image background;
    private MenuManager menuManager;
    private MenuAudioManager audioManager;
    private Button button;
    private Color startColor;

    [Header("UI References")]
    [SerializeField] private Image icon;
    [SerializeField] private TMP_Text nameText;
    [SerializeField] private TMP_Text priceText;
    [SerializeField] private Button selectButton;
    [SerializeField] private TMP_Text selectButtonText;
    private ShopItem shopItem;

    [Header("Selection")]
    [SerializeField] private Image selectedOverlay;

    [Header("Animations")]
    [SerializeField] private Color errorColor;
    [SerializeField] private float errorDisplayDuration;
    [SerializeField] private float errorResetDuration;
    private Coroutine errorCoroutine;

    // Start Function
    public void Initialize(MenuManager menuManager) {

        this.menuManager = menuManager;

        audioManager = FindObjectOfType<MenuAudioManager>();
        background = GetComponent<Image>();
        button = GetComponent<Button>();
        button.onClick.AddListener(PurchaseItem);
        selectButton.onClick.AddListener(SelectItem);

        selectButton.gameObject.SetActive(false);
        selectedOverlay.gameObject.SetActive(false);

        startColor = background.color;

    }

    private void PurchaseItem() {

        if (menuManager.PurchaseItem(this)) {

            audioManager.PlaySound(MenuAudioManager.UISoundEffectType.Success);

        } else {

            if (errorCoroutine != null)
                StopCoroutine(errorCoroutine);

            errorCoroutine = StartCoroutine(ShowErrorButton(background, errorColor, errorDisplayDuration, errorResetDuration));
            audioManager.PlaySound(MenuAudioManager.UISoundEffectType.Error);

        }
    }

    private void SelectItem() {

        menuManager.ChangeSelectedItem(this);

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

    public ShopItem GetShopItem() {

        return shopItem;

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

    public void SetSelected(bool selected) {

        if (selected) {

            shopItem.SetSelected(true);
            selectButton.interactable = false;
            selectButtonText.text = "Selected";

        } else {

            shopItem.SetSelected(false);
            selectButton.interactable = true;
            selectButtonText.text = "Select";

        }
    }

    public void SetPurchased(bool purchased) {

        if (purchased) {

            shopItem.SetPurchased(true);
            button.interactable = false;
            selectButton.gameObject.SetActive(true);

        } else {

            shopItem.SetPurchased(false);
            button.interactable = true;
            selectButton.gameObject.SetActive(false);

        }
    }
}
