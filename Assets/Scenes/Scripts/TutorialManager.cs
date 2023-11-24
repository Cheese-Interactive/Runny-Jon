using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TutorialManager : GameManager {

    private void Awake() {

        InitializeScene();

        playerData = FindObjectOfType<PlayerData>();

        acquiredCollectibles = new List<Collectible>();

        LoadCosmetics();
        UIController.TypeSubtitleText(checkpoints[currCheckpoint].GetSubtitleText());

        for (int i = 2; i < checkpoints.Length; i++)
            checkpoints[i].gameObject.SetActive(false);

        stopwatch = new Stopwatch();

        audioManager.PlaySceneMusic();

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

        audioManager.PlaySound(GameAudioManager.GameSoundEffectType.Checkpoint);

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

            playerController.EnableCrouch();
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

    public override bool CompleteLevel() {

        // verify if lists are equal (make sure player collected all collectibles)
        // TODO: add error or warning to tell player that all collectibles weren't collected
        List<Collectible> tempCollectibles = new List<Collectible>(acquiredCollectibles);

        for (int i = 0; i < requiredCollectibles.Count; i++) {

            // check if player has required collectible
            if (tempCollectibles.Contains(requiredCollectibles[i]))
                // remove collectible from the list
                tempCollectibles.Remove(requiredCollectibles[i]);
            else
                // return because they don't have required collectible
                return false;

        }

        // remaining collectibles in tempCollectibles list will be the bonus optional collectibles

        StartCoroutine(DelayedTimeFreeze());
        playerController.DisableAllMovement();
        playerController.DisableLook();
        stopwatch.Stop();
        audioManager.PlaySound(GameAudioManager.GameSoundEffectType.Victory);
        UIController.ShowLevelCompleteScreen(playerData.OnLevelComplete(currentLevel, deaths, (float) stopwatch.Elapsed.TotalSeconds, 1), deaths);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        return true;

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
