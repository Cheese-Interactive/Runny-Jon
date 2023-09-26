using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialManager : GameManager {

    [Header("References")]
    private PlayerController playerController;
    private UIController UIController;

    [Header("Checkpoints")]
    [SerializeField] private Checkpoint[] checkpoints;
    private int nextCheckpoint;

    [Header("Level")]
    [SerializeField] private Level currentLevel;

    [Header("Animations")]
    [SerializeField] private float fadeOutDuration;

    private void Start() {

        playerController = FindObjectOfType<PlayerController>();
        UIController = FindObjectOfType<UIController>();

        nextCheckpoint = 1;

        for (int i = 2; i < checkpoints.Length; i++)
            checkpoints[i].gameObject.SetActive(false);

        UIController.TypeSubtitleText("Use WASD to move around. Walk to the checkpoint in front of you.");

    }

    public void CheckpointReached(Checkpoint.CheckpointType checkpointType) {

        if (checkpointType != checkpoints[nextCheckpoint].GetCheckpointType())
            return;

        checkpoints[nextCheckpoint].StartFadeOutCheckpoint(fadeOutDuration);
        checkpoints[nextCheckpoint].GetComponentInChildren<CheckpointArrow>().StartFadeOutArrow(fadeOutDuration);

        if (nextCheckpoint != checkpoints.Length - 1)
            checkpoints[++nextCheckpoint].gameObject.SetActive(true);

        switch (checkpointType) {

            case Checkpoint.CheckpointType.Walk:

            UIController.TypeSubtitleText("Use left shift to sprint. Sprint to the next checkpoint.");
            playerController.EnableSprint();
            break;

            case Checkpoint.CheckpointType.Sprint:

            UIController.TypeSubtitleText("Use space to jump. Jump over the gaps to the next checkpoint.");
            playerController.EnableJump();
            break;

            case Checkpoint.CheckpointType.Jump:

            UIController.TypeSubtitleText("Use c to slide. Slide under the barrier to the next checkpoint.");
            playerController.EnableSlide();
            break;

            case Checkpoint.CheckpointType.Slide:

            UIController.TypeSubtitleText("Jump onto the wall while moving to wall run. Wall run to the next checkpoint.");
            playerController.EnableWallRun();
            break;

            case Checkpoint.CheckpointType.WallRun:

            UIController.TypeSubtitleText("Use right click to swing. An indicator should appear on any swingable objects. Swing to the next checkpoint.");
            playerController.EnableSwing();
            break;

            case Checkpoint.CheckpointType.Swing:

            UIController.TypeSubtitleText("Great job! You've learned all the basic movement mechanics. Complete the rest of the level.");
            nextCheckpoint++;
            break;

        }
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
