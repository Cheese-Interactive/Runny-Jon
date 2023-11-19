using System.Diagnostics;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : GameManager {

    private void Start() {

        playerController = FindObjectOfType<PlayerController>();
        UIController = FindObjectOfType<GameUIController>();
        audioManager = FindObjectOfType<GameAudioManager>();
        playerData = FindObjectOfType<PlayerData>();

        LoadCosmetics();

        Transform spawn = checkpoints[currCheckpoint].GetPlayerSpawn();
        playerController.transform.position = spawn.position;
        playerController.SetLookRotations(0f, spawn.rotation.eulerAngles.y);
        playerController.ResetVelocity();

        for (int i = 2; i < checkpoints.Length; i++)
            checkpoints[i].gameObject.SetActive(false);

        stopwatch = new Stopwatch();

        audioManager.PlaySceneMusic();

    }

    public override void StartTimer() {

        stopwatch = new Stopwatch();
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

        audioManager.PlaySound(GameAudioManager.GameSoundEffectType.Checkpoint);

        currCheckpoint++;
        UIController.TypeSubtitleText(checkpoints[currCheckpoint].GetSubtitleText());
        checkpoints[currCheckpoint].StartFadeOutCheckpoint();
        checkpoints[currCheckpoint].GetComponentInChildren<CheckpointArrow>().StartFadeOutArrow();

        if (currCheckpoint != checkpoints.Length - 1)
            checkpoints[currCheckpoint + 1].gameObject.SetActive(true);

    }

    public override void CompleteLevel() {

        StartCoroutine(DelayedMovementDisable());
        playerController.DisableAllMovement();
        playerController.DisableLook();
        stopwatch.Stop();
        audioManager.PlaySound(GameAudioManager.GameSoundEffectType.Victory);
        UIController.ShowLevelCompleteScreen(playerData.OnLevelComplete(currentLevel, deaths, (float) stopwatch.Elapsed.TotalSeconds, 1), deaths);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

    }

    public override Level GetCurrentLevel() {

        return currentLevel;

    }

    public override int GetLevelTimeLimit() {

        return currentLevel.GetTimeLimit();

    }

    public override void KillPlayer() {

        if (playerKilled)
            return;

        audioManager.PlaySound(GameAudioManager.GameSoundEffectType.Death);
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
