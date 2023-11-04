using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelButton : MonoBehaviour {

    [Header("References")]
    private MenuUIController menuUIController;

    [Header("UI References")]
    public TMP_Text levelNameText;
    public TMP_Text playsText;
    public TMP_Text recordText;
    public Image image;
    private Level level;

    private void Start() {

        menuUIController = FindObjectOfType<MenuUIController>();
        GetComponent<Button>().onClick.AddListener(LoadLevel);

    }

    private void LoadLevel() {

        menuUIController.FadeInLoadingScreen();
        StartCoroutine(menuUIController.LoadLevel(level));

    }

    public void SetLevel(Level level) {

        this.level = level;

    }
}