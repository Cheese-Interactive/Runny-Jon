using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour {

    [Header("References")]
    private GameManager gameManager;
    private TutorialManager tutorialManager;
    private Material material;
    private bool isTutorial;

    [Header("Checkpoint")]
    [SerializeField] private CheckpointType checkpointType;
    [SerializeField] private string subtitleText;
    [SerializeField] private Transform playerSpawn;
    [SerializeField] private float fadeOutDuration;

    [Header("Animations")]
    private Coroutine fadeOutCoroutine;

    public enum CheckpointType {

        Spawn, Normal, Sprint, Jump, Slide, WallRun, Swing

    }

    private void OnEnable() {

        gameManager = FindObjectOfType<GameManager>();
        material = GetComponent<MeshRenderer>().material;
        isTutorial = gameManager.GetCurrentLevel().isTutorial;

        if (isTutorial) {

            tutorialManager = (TutorialManager) gameManager;

        }
    }

    private void OnTriggerEnter(Collider collider) {

        gameManager.CheckpointReached(checkpointType);

    }

    public void StartFadeOutCheckpoint() {

        if (fadeOutCoroutine != null)
            fadeOutCoroutine = null;

        fadeOutCoroutine = StartCoroutine(FadeOutCheckpoint(fadeOutDuration));

    }

    private IEnumerator FadeOutCheckpoint(float duration) {

        float currentTime = 0f;
        Color startColor = material.color;
        Color targetColor = new Color(material.color.r, material.color.g, material.color.b, 0f);
        gameObject.SetActive(true);

        while (currentTime < duration) {

            currentTime += Time.deltaTime;
            material.color = Color.Lerp(startColor, targetColor, currentTime / duration);
            yield return null;

        }

        material.color = targetColor;
        gameObject.SetActive(false);
        fadeOutCoroutine = null;

    }

    public CheckpointType GetCheckpointType() {

        return checkpointType;

    }

    public string GetSubtitleText() {

        return subtitleText;

    }

    public Transform GetPlayerSpawn() {

        return playerSpawn;

    }

    public float GetFadeOutDuration() {

        return fadeOutDuration;

    }
}
