using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    [Header("References")]
    private PlayerController playerController;
    private UIController UIController;

    [Header("Level")]
    private Level currentLevel;

    private void Start() {

        playerController = FindObjectOfType<PlayerController>();
        UIController = FindObjectOfType<UIController>();
        currentLevel = FindObjectOfType<Level>();

        playerController.transform.position = currentLevel.spawn.position;

    }

    public void CompleteLevel() {

        playerController.HaltAllMovement();
        UIController.ShowLevelCompleteScreen();
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

    }

    public int GetLevelTimeLimit() {

        return currentLevel.timeLimit;

    }
}
