using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour {

    [Header("References")]
    private MenuUIController UIController;
    private MenuAudioManager audioManager;
    private PlayerData playerData;

    [Header("Levels")]
    [SerializeField] private List<Level> levels;
    [SerializeField] private int levelRowSize;
    [SerializeField] private Transform levelParent;
    [SerializeField] private GameObject levelRowPrefab;
    [SerializeField] private LevelButton levelButton;

    [Header("Shop")]
    [SerializeField] private List<ShopCategory> shopCategories;
    [SerializeField] private Transform shopLayoutParent;
    [SerializeField] private ShopLayout shopLayout;
    // Categories
    [SerializeField] private ShopCategoryButton shopCategoryButton;
    // Items
    [SerializeField] private int shopRowSize;
    [SerializeField] private Transform shopRowPrefab;
    [SerializeField] private ShopItemButton shopItemButton;
    private ShopLayout currActiveLayout;

    private void Start() {

        UIController = FindObjectOfType<MenuUIController>();
        audioManager = FindObjectOfType<MenuAudioManager>();
        playerData = FindObjectOfType<PlayerData>();

        int currLevelIndex = 0;
        Level level;
        Transform row;
        LevelButton levelButton;

        for (int i = 0; i < Mathf.Ceil(levels.Count / (float) levelRowSize); i++) {

            row = Instantiate(levelRowPrefab, levelParent).transform;
            row.name = "Row" + (i + 1);

            for (int j = 0; j < levelRowSize && currLevelIndex < levels.Count; j++) {

                level = levels[currLevelIndex];
                currLevelIndex++;

                if (level == null)
                    continue;

                levelButton = Instantiate(this.levelButton, row);
                levelButton.SetLevel(level);
                levelButton.name = level.name + "Button";
                levelButton.levelNameText.text = level.levelName;
                levelButton.playsText.text = "Plays: " + playerData.GetLevelPlays(level);

                float? record = playerData.GetLevelRecord(level);
                float seconds = 0f;
                if (record != null)
                    seconds = Mathf.Round((float) record % 60 * 100f) / 100f;
                levelButton.recordText.text = "Record: " + (record == null ? "None" : (record > 60f ? string.Format("{0:0}:{1:00}.{2:00}", (int) record / 60, (int) seconds, seconds % 1 * 100) : string.Format("{0:00}.{1:00}", (int) seconds, seconds % 1 * 100)));

                levelButton.image.sprite = level.icon;

            }
        }

        int currItemIndex;
        ShopLayout[] layouts = new ShopLayout[shopCategories.Count];
        ShopLayout layout;
        ShopCategory shopCategory;
        ShopCategoryButton shopCategoryButton = null;
        ShopItem shopItem;
        ShopItemButton shopItemButton;

        for (int i = 0; i < shopCategories.Count; i++) {

            currItemIndex = 0;
            layout = Instantiate(shopLayout, shopLayoutParent);
            layouts[i] = layout;
            shopCategory = shopCategories[i];
            layout.name = shopCategory.GetCategoryName() + "ShopLayout";

            List<ShopItem> shopItems = shopCategory.GetShopItems();

            for (int j = 0; j < Mathf.RoundToInt(shopItems.Count / (float) levelRowSize); j++) {

                row = Instantiate(shopRowPrefab, layout.GetShopItemsParent());
                row.name = "Row" + (j + 1);

                for (int k = 0; k < shopRowSize && currItemIndex < shopItems.Count; k++) {

                    shopItem = shopItems[currItemIndex];
                    currItemIndex++;

                    if (shopItem == null)
                        continue;

                    shopItemButton = Instantiate(this.shopItemButton, row);
                    shopItemButton.SetShopItem(shopItem);
                    shopItemButton.SetIcon(shopItem.GetIcon());
                    shopItemButton.SetNameText(shopItem.GetItemName());
                    shopItemButton.SetPriceText(shopItem.GetPrice() + "");

                }
            }
        }

        for (int i = 0; i < layouts.Length; i++) {

            for (int j = 0; j < layouts.Length; j++) {

                shopCategory = shopCategories[i];
                shopCategoryButton = Instantiate(this.shopCategoryButton, layouts[j].GetShopCategoryParent());
                shopCategoryButton.SetMenuManager(this);
                shopCategoryButton.SetShopCategory(shopCategory);
                shopCategoryButton.SetShopLayout(layouts[i]);
                shopCategoryButton.name = shopCategory.GetCategoryName() + "CategoryButton";
                shopCategoryButton.SetNameText(shopCategory.GetCategoryName());

                if (j == i) {

                    shopCategoryButton.SetIcon(shopCategory.GetSelectedIcon());
                    shopCategoryButton.GetButton().interactable = false;

                } else {

                    shopCategoryButton.SetIcon(shopCategory.GetUnselectedIcon());

                }
            }

            if (i == 0)
                currActiveLayout = shopCategoryButton.GetShopLayout();
            else
                layouts[i].gameObject.SetActive(false);

        }

        audioManager.PlayMenuMusic();

    }

    public List<Level> GetLevels() {

        return levels;

    }

    public List<ShopCategory> GetShopCategoryLayouts() {

        return shopCategories;

    }

    public void OpenShopLayout(ShopLayout layout) {

        CloseShopLayout(currActiveLayout);
        layout.gameObject.SetActive(true);
        currActiveLayout = layout;

    }

    public void CloseShopLayout(ShopLayout layout) {

        layout.gameObject.SetActive(false);
        currActiveLayout = null;

    }

    public bool PurchaseItem(ShopItem shopItem) {

        if (playerData.PurchaseItem(shopItem)) {

            UIController.UpdateQuesoCount();
            return true;

        } else {

            // TODO: Add error message or alert
            return false;

        }
    }
}
