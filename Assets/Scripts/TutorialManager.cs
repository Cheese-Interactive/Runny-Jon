using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialManager : GameManager {

    [Header("References")]
    private PlayerController playerController;
    private UIController UIController;

    [Header("Checkpoints")]
    [SerializeField] private Transform checkpointParent;
    private Checkpoint[] checkpoints;

    [Header("Level")]
    private Level currentLevel;

    private void Start() {

        playerController = FindObjectOfType<PlayerController>();
        UIController = FindObjectOfType<UIController>();
        checkpoints = checkpointParent.GetComponentsInChildren<Checkpoint>();
        currentLevel = FindObjectOfType<Level>();

        for (int i = 1; i < checkpoints.Length; i++)
            checkpoints[i].gameObject.SetActive(false);

        UIController.TypeSubtitleText("Use WASD to move around. Walk to the checkpoint in front of you.");

    }

    public void FinishTutorialStage(Checkpoint.CheckpointType checkpointType) {

        switch (checkpointType) {

            case Checkpoint.CheckpointType.Normal:

            break;

            case Checkpoint.CheckpointType.Walk:


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

    public Level GetCurrentLevel() {

        return currentLevel;

    }
}
