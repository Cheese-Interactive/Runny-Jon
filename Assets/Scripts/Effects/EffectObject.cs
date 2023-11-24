using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectObject : MonoBehaviour {

    [Header("References")]
    [SerializeField] private List<Effect> effectCycle;
    private int currIndex;
    private PlayerController playerController;
    private Material material;

    [Header("Animations")]
    private Coroutine fadeCoroutine;

    private void Start() {

        playerController = FindObjectOfType<PlayerController>();
        material = GetComponent<MeshRenderer>().material;

        // fade color
        fadeCoroutine = StartCoroutine(FadeColor(effectCycle[currIndex].GetEffectColor(), effectCycle[currIndex].GetColorFadeDuration()));

    }

    public void ChangeEffect() {

        // get next effect
        currIndex++;

        // loop if needed
        if (currIndex >= effectCycle.Count)
            currIndex = 0;

        // stop any existing fade coroutine
        if (fadeCoroutine != null)
            StopCoroutine(fadeCoroutine);

        // fade color
        fadeCoroutine = StartCoroutine(FadeColor(effectCycle[currIndex].GetEffectColor(), effectCycle[currIndex].GetColorFadeDuration()));

    }

    public Effect GetEffect() {

        return effectCycle[currIndex];

    }

    public void ZoneEntered() {

        // add effect
        playerController.AddEffect(effectCycle[currIndex]);

    }

    public void ZoneExited() {

        // remove effect
        playerController.RemoveEffect(effectCycle[currIndex]);

    }

    private IEnumerator FadeColor(Color targetColor, float duration) {

        float currentTime = 0f;
        Color startColor = material.color;

        while (currentTime < duration) {

            currentTime += Time.deltaTime;
            material.color = Color.Lerp(startColor, targetColor, currentTime / duration);
            yield return null;

        }

        material.color = targetColor;
        fadeCoroutine = null;

    }
}
