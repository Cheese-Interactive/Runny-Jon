using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelButton : MonoBehaviour {

    [Header("References")]
    private MenuManager menuManager;
    private Button button;
    private Image background;

    [Header("UI References")]
    [SerializeField] private TMP_Text nameText;
    [SerializeField] private TMP_Text playsText;
    [SerializeField] private TMP_Text recordText;
    private Level level;

    // Start Function
    public void SetMenuManager(MenuManager menuManager) {

        this.menuManager = menuManager;
        button = GetComponent<Button>();
        background = GetComponent<Image>();
        button.onClick.AddListener(LoadLevel);

    }

    private void LoadLevel() {

        StartCoroutine(menuManager.LoadLevel(level));

    }

    public void SetIcon(Sprite icon) {

        this.background.sprite = icon;

    }

    public void SetNameText(string nameText) {

        this.nameText.text = nameText;

    }

    public void SetPlaysText(string playsText) {

        this.playsText.text = playsText;

    }

    public void SetRecordText(string recordText) {

        this.recordText.text = recordText;

    }

    public void SetLevel(Level level) {

        this.level = level;

    }
}