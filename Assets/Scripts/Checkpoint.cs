using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour {

    [Header("References")]
    private GameManager gameManager;
    private TutorialManager tutorialManager;
    private bool isTutorial;

    [Header("Checkpoint")]
    [SerializeField] private CheckpointType checkpointType;

    public enum CheckpointType {

        Spawn, Normal, Walk, Sprint, Jump, Slide, WallRun, Swing, Finish

    }

    private void Start() {

        gameManager = FindObjectOfType<GameManager>();
        isTutorial = gameManager.GetCurrentLevel().isTutorial;

        if (isTutorial) {

            tutorialManager = (TutorialManager) gameManager;

        }
    }

    private void OnCollisionEnter(Collision collision) {

        if (isTutorial)
            tutorialManager.FinishTutorialStage(checkpointType);

    }
}
