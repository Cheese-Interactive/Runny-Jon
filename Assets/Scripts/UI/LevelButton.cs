using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelButton : MonoBehaviour {

    [Header("References")]
    private MenuUIController menuUIController;
    public TMP_Text levelNameText;
    public TMP_Text playsText;
    public TMP_Text recordText;
    public Image image;
    [HideInInspector] public Level level;

    private void Start() {

        menuUIController = FindObjectOfType<MenuUIController>();
        GetComponent<Button>().onClick.AddListener(LoadLevel);

    }

    private void LoadLevel() {

        menuUIController.FadeInLoadingScreen();
        StartCoroutine(menuUIController.LoadLevel(level));

    }
}