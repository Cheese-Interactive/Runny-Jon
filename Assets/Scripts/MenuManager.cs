using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour {

    [Header("References")]
    private MenuUIController UIController;
    private MenuAudioManager audioManager;
    private PlayerData playerData;

    [Header("Levels")]
    [SerializeField] private List<LevelSection> levelSections;
    [SerializeField] private Transform levelLayoutParent;
    [SerializeField] private LevelLayout levelLayout;
    // Sections
    [SerializeField] private int levelSectionButtonRowSize;
    [SerializeField] private Transform levelSectionParent;
    [SerializeField] private Transform levelSectionButtonRowPrefab;
    [SerializeField] private LevelSectionButton levelSectionButton;
    // Levels
    [SerializeField] private int levelRowSize;
    [SerializeField] private Transform levelRowPrefab;
    [SerializeField] private LevelButton levelButton;
    private LevelLayout currActiveLevelLayout;
    private bool levelSectionOpen;

    [Header("Shop")]
    [SerializeField] private List<ShopSection> shopSections;
    [SerializeField] private Transform shopLayoutParent;
    [SerializeField] private ShopLayout shopLayout;
    // Sections
    [SerializeField] private ShopSectionButton shopSectionButton;
    // Items
    [SerializeField] private int shopRowSize;
    [SerializeField] private Transform shopRowPrefab;
    [SerializeField] private ShopItemButton shopItemButton;
    private ShopLayout currActiveShopLayout;

    private void Start() {

        UIController = FindObjectOfType<MenuUIController>();
        audioManager = FindObjectOfType<MenuAudioManager>();
        playerData = FindObjectOfType<PlayerData>();

        int currLevelIndex;
        LevelLayout[] levelLayouts = new LevelLayout[levelSections.Count];
        LevelLayout levelLayout;
        LevelSection levelSection;
        LevelSectionButton levelSectionButton;
        Level level;
        LevelButton levelButton;
        Transform row;

        for (int i = 0; i < levelSections.Count; i++) {

            currLevelIndex = 0;
            levelLayout = Instantiate(this.levelLayout, levelLayoutParent);
            levelLayouts[i] = levelLayout;
            levelSection = levelSections[i];
            levelLayout.name = levelSection.GetSectionName() + "LevelLayout";

            List<Level> levels = levelSection.GetLevels();

            for (int j = 0; j < Mathf.Ceil(levels.Count / (float) levelRowSize); j++) {

                row = Instantiate(levelRowPrefab, levelLayout.GetLevelsParent());
                row.name = "Row" + (j + 1);

                for (int k = 0; k < levelRowSize && currLevelIndex < levels.Count; k++) {

                    level = levels[currLevelIndex];

                    levelButton = Instantiate(this.levelButton, row);
                    levelButton.SetMenuManager(this);
                    levelButton.name = level.name + "Button";
                    levelButton.SetLevel(level);
                    levelButton.SetIcon(level.GetIcon());
                    levelButton.SetNameText(level.GetLevelName());
                    levelButton.SetPlaysText("Plays: " + playerData.GetLevelPlays(level));

                    float? record = playerData.GetLevelRecord(level);
                    float seconds = 0f;
                    if (record != null)
                        seconds = Mathf.Round((float) record % 60 * 100f) / 100f;
                    levelButton.SetRecordText("Record: " + (record == null ? "None" : (record > 60f ? string.Format("{0:0}:{1:00}.{2:00}", (int) record / 60, (int) seconds, seconds % 1 * 100) : string.Format("{0:00}.{1:00}", (int) seconds, seconds % 1 * 100))));

                    currLevelIndex++;

                }
            }

            levelLayout.gameObject.SetActive(false);

        }

        currLevelIndex = 0;

        for (int i = 0; i < Mathf.Ceil(levelLayouts.Length / (float) levelRowSize); i++) {

            row = Instantiate(levelSectionButtonRowPrefab, levelSectionParent);
            row.name = "Row" + (i + 1);

            for (int j = 0; j < levelSectionButtonRowSize && currLevelIndex < levelLayouts.Length; j++) {

                levelSection = levelSections[currLevelIndex];

                levelSectionButton = Instantiate(this.levelSectionButton, row);
                levelSectionButton.SetMenuManager(this);
                levelSectionButton.SetLevelLayout(levelLayouts[currLevelIndex]);
                levelSectionButton.name = levelSection.GetSectionName() + "SectionButton";
                levelSectionButton.SetText(levelSection.GetSectionName());

                currLevelIndex++;

            }
        }

        int currItemIndex;
        ShopLayout[] shopLayouts = new ShopLayout[shopSections.Count];
        ShopLayout shopLayout;
        ShopSection shopSection;
        ShopSectionButton shopSectionButton = null;
        ShopItem shopItem;
        ShopItemButton shopItemButton;
        List<ShopItem> inventory = playerData.GetInventory();

        for (int i = 0; i < shopSections.Count; i++) {

            currItemIndex = 0;
            shopLayout = Instantiate(this.shopLayout, shopLayoutParent);
            shopLayouts[i] = shopLayout;
            shopSection = shopSections[i];
            shopLayout.name = shopSection.GetSectionName() + "ShopLayout";

            List<ShopItem> shopItems = shopSection.GetShopItems();

            for (int j = 0; j < Mathf.Ceil(shopItems.Count / (float) shopRowSize); j++) {

                row = Instantiate(shopRowPrefab, shopLayout.GetShopItemsParent());
                row.name = "Row" + (j + 1);

                for (int k = 0; k < shopRowSize && currItemIndex < shopItems.Count; k++) {

                    shopItem = shopItems[currItemIndex];

                    shopItemButton = Instantiate(this.shopItemButton, row);
                    shopItemButton.SetMenuManager(this);
                    shopItemButton.name = shopItem.name + "Button";
                    shopItemButton.SetShopItem(shopItem);
                    shopItemButton.SetIcon(shopItem.GetIcon());
                    shopItemButton.SetNameText(shopItem.GetItemName());
                    shopItemButton.SetPriceText(shopItem.GetPrice() + "");

                    for (int g = 0; g < inventory.Count; g++)
                        if (inventory[g].Equals(shopItem))
                            shopItemButton.GetButton().interactable = false;

                    currItemIndex++;

                }
            }
        }

        for (int i = 0; i < shopLayouts.Length; i++) {

            for (int j = 0; j < shopLayouts.Length; j++) {

                shopSection = shopSections[i];
                shopSectionButton = Instantiate(this.shopSectionButton, shopLayouts[j].GetShopSectionParent());
                shopSectionButton.SetMenuManager(this);
                shopSectionButton.SetShopSection(shopSection);
                shopSectionButton.SetShopLayout(shopLayouts[i]);
                shopSectionButton.name = shopSection.GetSectionName() + "SectionButton";
                shopSectionButton.SetNameText(shopSection.GetSectionName());

                if (j == i) {

                    shopSectionButton.SetIcon(shopSection.GetSelectedIcon());
                    shopSectionButton.GetButton().interactable = false;

                } else {

                    shopSectionButton.SetIcon(shopSection.GetUnselectedIcon());

                }
            }

            if (i == 0)
                currActiveShopLayout = shopSectionButton.GetShopLayout();
            else
                shopLayouts[i].gameObject.SetActive(false);

        }

        audioManager.PlayMenuMusic();

    }

    public IEnumerator LoadLevel(Object level) {

        UIController.SetLoadingText("Loading Level...");
        yield return UIController.FadeInLoadingScreen();
        AsyncOperation operation = SceneManager.LoadSceneAsync(level.name);
        operation.allowSceneActivation = false;
        float currentTime = 0f;

        while (!operation.isDone && operation.progress < 0.9f) {

            currentTime += Time.deltaTime;
            yield return null;

        }

        yield return new WaitForSeconds(UIController.GetMinLoadingDuration() - currentTime);
        operation.allowSceneActivation = true;

    }

    public void OpenLevelLayout(LevelLayout layout) {

        if (currActiveLevelLayout != null)
            CloseLevelLayout(currActiveLevelLayout);

        levelSectionParent.gameObject.SetActive(false);
        layout.gameObject.SetActive(true);
        currActiveLevelLayout = layout;
        levelSectionOpen = true;

    }

    public void CloseLevelLayout(LevelLayout layout) {

        levelSectionParent.gameObject.SetActive(true);
        layout.gameObject.SetActive(false);
        currActiveLevelLayout = null;
        levelSectionOpen = false;

    }

    public List<ShopSection> GetShopSectionLayouts() {

        return shopSections;

    }

    public void OpenShopLayout(ShopLayout layout) {

        CloseShopLayout(currActiveShopLayout);
        layout.gameObject.SetActive(true);
        currActiveShopLayout = layout;

    }

    public void CloseShopLayout(ShopLayout layout) {

        layout.gameObject.SetActive(false);
        currActiveShopLayout = null;

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

    public LevelLayout GetCurrentActiveLevelLayout() {

        return currActiveLevelLayout;

    }

    public bool IsLevelSectionOpen() {

        return levelSectionOpen;

    }
}
