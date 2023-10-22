using System.Diagnostics;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TutorialManager : GameManager {

    private void Start() {

        playerController = FindObjectOfType<PlayerController>();
        UIController = FindObjectOfType<GameUIController>();
        audioManager = FindObjectOfType<AudioManager>();
        playerData = FindObjectOfType<PlayerData>();

        UIController.TypeSubtitleText(checkpoints[currCheckpoint].GetSubtitleText());

        Transform firstCheckpoint = checkpoints[currCheckpoint].transform;
        playerController.transform.position = firstCheckpoint.position;
        playerController.transform.rotation = firstCheckpoint.rotation;
        playerController.SetLookRotations(0f, firstCheckpoint.rotation.eulerAngles.y);
        playerController.ResetVelocity();

        for (int i = 2; i < checkpoints.Length; i++)
            checkpoints[i].gameObject.SetActive(false);

        stopwatch = new Stopwatch();

        audioManager.PlayMusic(AudioManager.MusicType.EverythingIsAwesome);

    }

    public override void StartTimer() {

        stopwatch.Start();
        StartCoroutine(UIController.HandleLevelTimer());

    }

    public override void PauseTimer() {

        stopwatch.Stop();

    }

    public override void ResumeTimer() {

        stopwatch.Start();

    }

    public override Stopwatch GetTimer() {

        return stopwatch;

    }

    public override void CheckpointReached(Checkpoint.CheckpointType checkpointType) {

        if (currCheckpoint + 1 >= checkpoints.Length || checkpointType != checkpoints[currCheckpoint + 1].GetCheckpointType())
            return;

        currCheckpoint++;
        UIController.TypeSubtitleText(checkpoints[currCheckpoint].GetSubtitleText());

        switch (checkpointType) {

            case Checkpoint.CheckpointType.Sprint:

            playerController.EnableSprint();
            break;

            case Checkpoint.CheckpointType.Jump:

            playerController.EnableJump();
            break;

            case Checkpoint.CheckpointType.Slide:

            playerController.EnableSlide();
            break;

            case Checkpoint.CheckpointType.WallRun:

            playerController.EnableWallRun();
            break;

            case Checkpoint.CheckpointType.Swing:

            playerController.EnableSwing();
            break;

        }

        checkpoints[currCheckpoint].StartFadeOutCheckpoint();
        checkpoints[currCheckpoint].GetComponentInChildren<CheckpointArrow>().StartFadeOutArrow();

        if (currCheckpoint != checkpoints.Length - 1)
            checkpoints[currCheckpoint + 1].gameObject.SetActive(true);

    }

    public override void CompleteLevel() {

        stopwatch.Stop();
        audioManager.PlaySound(AudioManager.SoundEffectType.Victory);
        playerController.DisableAllMovement();
        playerController.DisableLook();
        UIController.ShowLevelCompleteScreen(playerData.OnLevelComplete(currentLevel, deaths, (float) stopwatch.Elapsed.TotalSeconds), deaths);
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

        if (playerKilled)
            return;

        playerKilled = true;

        deaths++;
        StartCoroutine(RespawnPlayer());

    }

    public override Scene GetMainMenuScene() {

        return SceneManager.GetSceneByName("MainMenu");

    }

    public override Checkpoint[] GetCheckpoints() {

        return checkpoints;

    }
}
