using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.SceneManagement;

public abstract class GameManager : MonoBehaviour {

    [Header("References")]
    [SerializeField] protected Checkpoint[] checkpoints;
    protected PlayerController playerController;
    protected GameUIController UIController;
    protected GameAudioManager audioManager;
    protected PlayerData playerData;
    protected bool playerKilled;
    protected List<ShopItem> selectedItems;

    [Header("Level")]
    [SerializeField] protected Level currentLevel;
    [SerializeField] protected float delayedMovementStopDuration;
    protected int deaths;
    protected Stopwatch stopwatch;
    protected int currCheckpoint;
    private bool gamePaused;

    public void PauseGame() {

        PauseTimer();
        Time.timeScale = 0f;
        gamePaused = true;

    }

    public void ResumeGame() {

        ResumeTimer();
        Time.timeScale = 1f;
        gamePaused = false;

    }

    protected void LoadCosmetics() {

        selectedItems = JsonUtility.FromJson<SelectedShopItems>(PlayerPrefs.GetString("SelectedShopItems")).GetSelectedItems();

        foreach (ShopItem shopItem in selectedItems) {

            if (shopItem == null)
                break;

            Cosmetic cosmetic = (Cosmetic) playerController.gameObject.AddComponent(shopItem.GetCosmeticScript().GetType());
            shopItem.GetCosmeticScript().CopyTo(cosmetic);

        }
    }

    public abstract void StartTimer();

    public abstract void PauseTimer();

    public abstract void ResumeTimer();

    public abstract Stopwatch GetTimer();

    public abstract void CheckpointReached(Checkpoint.CheckpointType checkpointType);

    public abstract void CompleteLevel();

    protected IEnumerator DelayedMovementDisable() {

        yield return new WaitForSeconds(delayedMovementStopDuration);
        Time.timeScale = 0f;

    }

    public abstract Level GetCurrentLevel();

    public abstract int GetLevelTimeLimit();

    public abstract void KillPlayer();

    public abstract Scene GetMainMenuScene();

    public abstract Checkpoint[] GetCheckpoints();

    public void SetCheckpoint(Checkpoint checkpoint) {

        for (int i = 0; i < checkpoints.Length; i++) {

            if (checkpoints[i] == checkpoint) {

                currCheckpoint = i;
                break;

            }
        }

        for (int i = 1; i < checkpoints.Length - 1; i++) {

            if (i + 1 != currCheckpoint)
                checkpoints[i].gameObject.SetActive(false);

        }

        if (currCheckpoint != checkpoints.Length - 1)
            checkpoints[currCheckpoint + 1].gameObject.SetActive(true);

        StartCoroutine(RespawnPlayer());

    }

    protected IEnumerator RespawnPlayer() {

        playerController.StopWallRun();
        playerController.DisableAllMovement();
        playerController.DisableLook();
        playerController.Uncrouch();
        yield return StartCoroutine(UIController.ShowDeathScreen());
        Transform spawn = checkpoints[currCheckpoint].GetPlayerSpawn();
        playerController.transform.position = spawn.position;
        playerController.transform.rotation = spawn.rotation;
        playerController.SetLookRotations(0f, spawn.rotation.eulerAngles.y);
        playerController.StopSwing();
        playerController.ResetVelocity();
        playerController.EnableAllMovement();
        playerController.EnableLook();
        yield return StartCoroutine(UIController.HideDeathScreen());
        playerKilled = false;

    }

    public bool GetGamePaused() {

        return gamePaused;

    }
}
