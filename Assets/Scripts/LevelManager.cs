using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class LevelManager : GameManager {

    [Header("References")]
    private PlayerController playerController;
    private UIController UIController;

    [Header("Level")]
    private Level currentLevel;
    private Stopwatch stopwatch;

    private void Start() {

        playerController = FindObjectOfType<PlayerController>();
        UIController = FindObjectOfType<UIController>();
        currentLevel = FindObjectOfType<Level>();

        playerController.transform.position = currentLevel.spawn.position;

    }

    public override void CompleteLevel() {

        playerController.DisableAllMovement();
        UIController.ShowLevelCompleteScreen();
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

    }

    public override Level GetCurrentLevel() {

        return currentLevel;

    }

    public override int GetLevelTimeLimit() {

        return currentLevel.timeLimit;

    }

    public override void KillPlayer() {

    }
}
