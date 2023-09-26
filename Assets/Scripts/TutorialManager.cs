using System.Collections;
using System.Collections.Generic;
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

    [Header("Animations")]
    [SerializeField] private float fadeOutDuration;

    private void Start() {

        playerController = FindObjectOfType<PlayerController>();
        UIController = FindObjectOfType<UIController>();

        for (int i = 2; i < checkpoints.Length; i++)
            checkpoints[i].gameObject.SetActive(false);

        UIController.TypeSubtitleText(checkpoints[currCheckpoint].GetSubtitleText());

    }

    public void CheckpointReached(Checkpoint.CheckpointType checkpointType) {

        if (checkpointType != checkpoints[currCheckpoint + 1].GetCheckpointType())
            return;

        currCheckpoint++;

        switch (checkpointType) {

            case Checkpoint.CheckpointType.Sprint:

            UIController.TypeSubtitleText(checkpoints[currCheckpoint].GetSubtitleText());
            playerController.EnableSprint();
            break;

            case Checkpoint.CheckpointType.Jump:

            UIController.TypeSubtitleText(checkpoints[currCheckpoint].GetSubtitleText());
            playerController.EnableJump();
            break;

            case Checkpoint.CheckpointType.Slide:

            UIController.TypeSubtitleText(checkpoints[currCheckpoint].GetSubtitleText());
            playerController.EnableSlide();
            break;

            case Checkpoint.CheckpointType.WallRun:

            UIController.TypeSubtitleText(checkpoints[currCheckpoint].GetSubtitleText());
            playerController.EnableWallRun();
            break;

            case Checkpoint.CheckpointType.Swing:

            UIController.TypeSubtitleText(checkpoints[currCheckpoint].GetSubtitleText());
            playerController.EnableSwing();
            break;

            case Checkpoint.CheckpointType.Finish:

            UIController.TypeSubtitleText(checkpoints[currCheckpoint].GetSubtitleText());
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
}
