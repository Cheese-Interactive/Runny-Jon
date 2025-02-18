using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopSectionButton : MonoBehaviour {

    [Header("References")]
    private MenuManager menuManager;
    private Button button;

    [Header("UI References")]
    [SerializeField] private Image icon;
    [SerializeField] private TMP_Text nameText;
    private ShopLayout shopLayout;
    private ShopSection shopSection;

    // Start Function
    public void SetMenuManager(MenuManager menuManager) {

        this.menuManager = menuManager;
        button = GetComponent<Button>();
        button.onClick.AddListener(SelectCategory);

    }

    public void SelectCategory() {

        icon = shopSection.GetSelectedIcon();
        menuManager.OpenShopLayout(shopLayout);

    }

    public void DeselectCategory() {

        icon = shopSection.GetUnselectedIcon();
        menuManager.CloseShopLayout(shopLayout);

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

    public void SetShopLayout(ShopLayout shopLayout) {

        this.shopLayout = shopLayout;

    }

    public ShopLayout GetShopLayout() {

        return shopLayout;

    }

    public void SetShopSection(ShopSection shopSection) {

        this.shopSection = shopSection;

    }
}
