using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelSectionButton : MonoBehaviour {

    [Header("References")]
    private MenuManager menuManager;
    private Button button;

    [Header("UI References")]
    [SerializeField] private TMP_Text text;
    private LevelLayout levelLayout;

    // Start Function
    public void SetMenuManager(MenuManager menuManager) {

        this.menuManager = menuManager;
        button = GetComponent<Button>();
        button.onClick.AddListener(SelectSection);

    }

    private void SelectSection() {

        menuManager.OpenLevelLayout(levelLayout);

    }

    public void SetText(string nameText) {

        this.text.text = nameText;

    }

    public void SetLevelLayout(LevelLayout levelLayout) {

        this.levelLayout = levelLayout;

    }

    public LevelLayout GetLevelLayout() {

        return levelLayout;

    }
}
