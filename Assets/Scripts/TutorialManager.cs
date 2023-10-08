using System.Collections;
using System.Diagnostics;
using UnityEngine;

public class TutorialManager : GameManager {

    [Header("References")]
    private PlayerController playerController;
    private UIController UIController;

    [Header("Checkpoints")]
    [SerializeField] private Checkpoint[] checkpoints;
    private int currCheckpoint;

    [Header("Level")]
    [SerializeField] private Level currentLevel;
    private Stopwatch stopwatch;

    [Header("Animations")]
    [SerializeField] private float fadeOutDuration;

    private void Start() {

        playerController = FindObjectOfType<PlayerController>();
        UIController = FindObjectOfType<UIController>();

        Transform firstCheckpoint = checkpoints[currCheckpoint].transform;
        playerController.transform.position = firstCheckpoint.position;
        playerController.transform.rotation = firstCheckpoint.rotation;
        playerController.SetLookRotations(0f, firstCheckpoint.rotation.eulerAngles.y);
        playerController.ResetVelocity();

        for (int i = 2; i < checkpoints.Length; i++)
            checkpoints[i].gameObject.SetActive(false);

        UIController.TypeSubtitleText(checkpoints[currCheckpoint].GetSubtitleText());

    }

    public void CheckpointReached(Checkpoint.CheckpointType checkpointType) {

        if (checkpointType != checkpoints[currCheckpoint + 1].GetCheckpointType())
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

        checkpoints[currCheckpoint].StartFadeOutCheckpoint(fadeOutDuration);
        checkpoints[currCheckpoint].GetComponentInChildren<CheckpointArrow>().StartFadeOutArrow(fadeOutDuration);

        if (currCheckpoint != checkpoints.Length - 1)
            checkpoints[currCheckpoint + 1].gameObject.SetActive(true);

    }

    public override void CompleteLevel() {


    }

    public override Level GetCurrentLevel() {

        return currentLevel;

    }

    public override int GetLevelTimeLimit() {

        return currentLevel.timeLimit;

    }

    public override void KillPlayer() {

        StartCoroutine(HandlePlayerDeath());

    }

    private IEnumerator HandlePlayerDeath() {

        playerController.DisableAllMovement();
        yield return StartCoroutine(UIController.ShowDeathScreen());
        Transform spawn = checkpoints[currCheckpoint].GetPlayerSpawn();
        playerController.transform.position = spawn.position;
        playerController.transform.rotation = spawn.rotation;
        playerController.SetLookRotations(0f, spawn.rotation.eulerAngles.y);
        playerController.ResetVelocity();
        playerController.EnableAllMovement();
        yield return StartCoroutine(UIController.HideDeathScreen());

    }
}
