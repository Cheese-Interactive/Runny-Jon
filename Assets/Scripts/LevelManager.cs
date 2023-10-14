using System.Diagnostics;
using UnityEngine;

public class LevelManager : GameManager {

    private void Start() {

        playerController = FindObjectOfType<PlayerController>();
        UIController = FindObjectOfType<GameUIController>();

        Transform firstCheckpoint = checkpoints[currCheckpoint].transform;
        playerController.transform.position = firstCheckpoint.position;
        playerController.transform.rotation = firstCheckpoint.rotation;
        UnityEngine.Debug.Log(playerController.transform.rotation.eulerAngles);
        playerController.SetLookRotations(0f, firstCheckpoint.rotation.eulerAngles.y);
        playerController.ResetVelocity();

        for (int i = 2; i < checkpoints.Length; i++)
            checkpoints[i].gameObject.SetActive(false);

    }

    public override void StartTimer() {

        stopwatch = new Stopwatch();
        stopwatch.Start();
        StartCoroutine(UIController.HandleLevelTimer());

    }

    public override Stopwatch GetTimer() {

        return stopwatch;

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

        StartCoroutine(RespawnPlayer());

    }
}
