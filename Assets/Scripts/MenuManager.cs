using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour {

    [Header("References")]
    private AudioManager audioManager;
    private PlayerData playerData;

    [Header("Levels")]
    [SerializeField] private List<Level> levels;
    [SerializeField] private int levelRowSize;
    [SerializeField] private Transform levelParent;
    [SerializeField] private GameObject rowPrefab;
    [SerializeField] private LevelButton levelButton;

    private void Start() {

        audioManager = FindObjectOfType<AudioManager>();
        playerData = FindObjectOfType<PlayerData>();

        int currLevelIndex = 0;
        Level level;
        Transform row;
        LevelButton button;

        for (int i = 0; i < Mathf.Ceil(levels.Count / (float) levelRowSize); i++) {

            row = Instantiate(rowPrefab, levelParent).transform;
            row.name = "Row" + (i + 1);

            for (int j = 0; j < levelRowSize && currLevelIndex < levels.Count; j++) {

                level = levels[currLevelIndex];
                currLevelIndex++;

                if (level == null)
                    continue;

                button = Instantiate(levelButton, row);
                button.level = level;
                button.levelNameText.text = level.levelName;
                button.playsText.text = "Plays: " + playerData.GetLevelPlays(level);

                float? record = playerData.GetLevelRecord(level);
                float seconds = 0f;
                if (record != null)
                    seconds = Mathf.Round((float) record % 60 * 100f) / 100f;
                button.recordText.text = "Record: " + (record == null ? "None" : (record > 60f ? string.Format("{0:0}:{1:00}.{2:00}", (int) record / 60, (int) seconds, seconds % 1 * 100) : string.Format("{0:00}.{1:00}", (int) seconds, seconds % 1 * 100)));

                button.image.sprite = level.icon;

            }
        }

        audioManager.PlaySceneMusic();

    }

    public List<Level> GetLevels() {

        return levels;

    }
}
