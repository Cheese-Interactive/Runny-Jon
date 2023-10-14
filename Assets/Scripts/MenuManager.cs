using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour {

    [Header("Levels")]
    [SerializeField] private List<Level> levels;
    [SerializeField] private int levelRowSize;
    [SerializeField] private Transform levelParent;
    [SerializeField] private RectTransform levelMenu;
    [SerializeField] private GameObject rowPrefab;
    [SerializeField] private LevelButton levelButton;

    private void Start() {

        int currLevelIndex = 0;
        Level level;
        Transform row;
        LevelButton button;

        for (int i = 0; i < Mathf.Ceil(levels.Count / (float) levelRowSize); i++) {

            row = Instantiate(rowPrefab, levelParent).transform;
            row.name = "Row" + (i + 1);

            for (int j = 0; j < levelRowSize && currLevelIndex < levels.Count; j++) {

                level = levels[currLevelIndex];
                button = Instantiate(levelButton, row);
                button.level = level;
                button.levelNameText.text = level.levelName;
                button.playsText.text = "0";
                button.recordText.text = "NONE";
                button.image.sprite = level.icon;
                currLevelIndex++;

            }
        }
    }

    private void LoadLevel() {


    }
}
