using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopCategoryButton : MonoBehaviour {

    [Header("UI References")]
    [SerializeField] private Image icon;
    [SerializeField] private TMP_Text nameText;
    private ShopLayout shopLayout;
    private ShopCategory shopCategory;

    private void Start() {

        GetComponent<Button>().onClick.AddListener(SelectCategory);

    }

    public void SelectCategory() {

        icon = shopCategory.GetSelectedIcon();

    }

    public void DeselectCategory() {

        icon = shopCategory.GetUnselectedIcon();

    }

    public void SetIcon(Image icon) {

        this.icon = icon;

    }

    public void SetNameText(string nameText) {

        this.nameText.text = nameText;

    }

    public void SetShopCategory(ShopCategory shopCategory) {

        this.shopCategory = shopCategory;

    }

    public void SetShopCategoryLayout(ShopLayout shopLayout) {

        this.shopLayout = shopLayout;

    }
}
