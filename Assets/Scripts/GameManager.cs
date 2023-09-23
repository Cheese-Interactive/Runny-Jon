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

    public void FinishTutorialStage(Checkpoint.CheckpointType checkpointType) {

        switch (checkpointType) {

            case Checkpoint.CheckpointType.Normal:

            break;

            case Checkpoint.CheckpointType.Sprint:

            break;

            case Checkpoint.CheckpointType.Jump:

            break;

            case Checkpoint.CheckpointType.Slide:

            break;

            case Checkpoint.CheckpointType.WallRun:

            break;

            case Checkpoint.CheckpointType.Swing:

            break;

            default:

            return;

        }
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
