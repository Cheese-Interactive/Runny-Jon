using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CallingButton : Interactable {

    [Header("References")]
    [SerializeField] private Elevator elevator;
    [SerializeField] private ButtonType buttonType;
    private MeshRenderer meshRenderer;

    [Header("Animations")]
    [SerializeField] private Color clickedColor;
    [SerializeField] private float clickedDuration;
    [SerializeField] private float fadeDuration;
    private Coroutine fadeCoroutine;

    public enum ButtonType {

        Top, Bottom

    }

    private void Start() {

        meshRenderer = GetComponent<MeshRenderer>();

    }

    protected override void OnInteract() {

        if (fadeCoroutine != null)
            StopCoroutine(fadeCoroutine);

        elevator.CallElevator(buttonType);
        fadeCoroutine = StartCoroutine(HandleButtonClick(clickedColor));

    }

    private IEnumerator HandleButtonClick(Color targetColor) {

        float currentTime = 0f;
        Color startColor = meshRenderer.material.color;

        while (currentTime < fadeDuration) {

            currentTime += Time.deltaTime;
            meshRenderer.material.color = Color.Lerp(startColor, targetColor, currentTime / fadeDuration);
            yield return null;

        }

        meshRenderer.material.color = targetColor;

        yield return new WaitForSeconds(clickedDuration);

        currentTime = 0f;

        while (currentTime < fadeDuration) {

            currentTime += Time.deltaTime;
            meshRenderer.material.color = Color.Lerp(targetColor, startColor, currentTime / fadeDuration);
            yield return null;

        }

        meshRenderer.material.color = startColor;
        fadeCoroutine = null;

    }
}
