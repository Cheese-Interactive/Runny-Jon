using System.Collections;
using System.Diagnostics;
using UnityEngine;

public abstract class GameManager : MonoBehaviour {

    [Header("References")]
    [SerializeField] protected Checkpoint[] checkpoints;
    protected PlayerController playerController;
    protected GameUIController UIController;
    protected AudioManager audioManager;
    protected PlayerData playerData;

    [Header("Level")]
    [SerializeField] protected Object mainMenuScene;
    [SerializeField] protected Level currentLevel;
    protected Stopwatch stopwatch;
    protected int currCheckpoint;

    public abstract void StartTimer();

    public abstract void PauseTimer();

    public abstract void ResumeTimer();

    public abstract Stopwatch GetTimer();

    public abstract void CheckpointReached(Checkpoint.CheckpointType checkpointType);

    public abstract void CompleteLevel();

    public abstract Level GetCurrentLevel();

    public abstract int GetLevelTimeLimit();

    public abstract void KillPlayer();

    public abstract Object GetMainMenuScene();

    protected IEnumerator RespawnPlayer() {

        playerController.DisableAllMovement();
        playerController.DisableLook();
        yield return StartCoroutine(UIController.ShowDeathScreen());
        Transform spawn = checkpoints[currCheckpoint].GetPlayerSpawn();
        playerController.transform.position = spawn.position;
        playerController.transform.rotation = spawn.rotation;
        playerController.SetLookRotations(0f, spawn.rotation.eulerAngles.y);
        playerController.ResetVelocity();
        playerController.EnableAllMovement();
        playerController.EnableLook();
        yield return StartCoroutine(UIController.HideDeathScreen());

    }
}
