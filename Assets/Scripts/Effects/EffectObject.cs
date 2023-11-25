using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectObject : MonoBehaviour {

    [Header("References")]
    [SerializeField] private List<Effect> effectCycle;
    [SerializeField] private EffectZone effectZone;
    private int currIndex;
    private PlayerController playerController;
    private Material platformMaterial;
    private Material zoneMaterial;

    [Header("Animations")]
    private Coroutine platformFadeCoroutine;
    private Coroutine zoneFadeCoroutine;

    private void Start() {

        playerController = FindObjectOfType<PlayerController>();
        platformMaterial = GetComponent<MeshRenderer>().material;
        zoneMaterial = effectZone.GetComponent<MeshRenderer>().material;

        // get effect color
        Color effectColor = effectCycle[currIndex].GetEffectColor();

        // fade platform color
        platformFadeCoroutine = StartCoroutine(FadeColor(platformMaterial, effectColor, effectCycle[currIndex].GetColorFadeDuration(), platformFadeCoroutine));

        // fade zone color
        zoneFadeCoroutine = StartCoroutine(FadeColor(zoneMaterial, new Color(effectColor.r, effectColor.g, effectColor.b, effectZone.GetOpacity()), effectCycle[currIndex].GetColorFadeDuration(), zoneFadeCoroutine));

    }

    public void ChangeEffect() {

        // get next effect
        currIndex++;

        // loop if needed
        if (currIndex >= effectCycle.Count)
            currIndex = 0;

        // stop any existing platform fade coroutine
        if (platformFadeCoroutine != null)
            StopCoroutine(platformFadeCoroutine);

        // stop any existing zone fade coroutine
        if (zoneFadeCoroutine != null)
            StopCoroutine(zoneFadeCoroutine);

        // get effect color
        Color effectColor = effectCycle[currIndex].GetEffectColor();

        // fade platform color
        platformFadeCoroutine = StartCoroutine(FadeColor(platformMaterial, effectColor, effectCycle[currIndex].GetColorFadeDuration(), platformFadeCoroutine));

        // fade zone color
        zoneFadeCoroutine = StartCoroutine(FadeColor(zoneMaterial, new Color(effectColor.r, effectColor.g, effectColor.b, effectZone.GetOpacity()), effectCycle[currIndex].GetColorFadeDuration(), zoneFadeCoroutine));

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

    private IEnumerator FadeColor(Material material, Color targetColor, float duration, Coroutine coroutine) {

        float currentTime = 0f;
        Color startColor = material.color;

        while (currentTime < duration) {

            currentTime += Time.deltaTime;
            material.color = Color.Lerp(startColor, targetColor, currentTime / duration);
            yield return null;

        }

        material.color = targetColor;
        coroutine = null;

    }
}
