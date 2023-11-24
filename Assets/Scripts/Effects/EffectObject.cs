using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectObject : MonoBehaviour {

    [Header("References")]
    [SerializeField] private GravityZone gravityZone;
    [SerializeField] private List<Effect> effectCycle;
    private int currIndex;
    private PlayerController playerController;
    private Material material;

    [Header("Animations")]
    private Coroutine fadeCoroutine;

    private void Start() {

        playerController = FindObjectOfType<PlayerController>();
        material = GetComponent<MeshRenderer>().material;

        // check if gravity zone isn't null
        if (gravityZone)
            // disable gravity zone
            gravityZone.gameObject.SetActive(false);

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

        // make sure gravity zone isn't null
        if (gravityZone)
            // check if current effect is gravity
            if (effectCycle[currIndex].GetEffectType() == EffectType.Gravity)
                // enable gravity zone
                gravityZone.gameObject.SetActive(true);
            else
                // disable gravity zone
                gravityZone.gameObject.SetActive(false);

    }

    public Effect GetEffect() {

        return effectCycle[currIndex];

    }

    public void GravityEntered() {

        // add effect
        playerController.AddEffect(effectCycle[currIndex]);

    }

    public void GravityExited() {

        // remove effect
        playerController.RemoveEffect(effectCycle[currIndex]);

    }

    private void OnCollisionEnter(Collision collision) {

        // check if player collided
        if (collision.transform.CompareTag("Player"))
            // make sure effect isn't gravity (handled elsewhere)
            if (effectCycle[currIndex].GetEffectType() != EffectType.Gravity)
                // add effect
                playerController.AddEffect(effectCycle[currIndex]);

    }

    private void OnTriggerEnter(Collider collider) {

        // check if player collided
        if (collider.transform.CompareTag("Player"))
            // make sure effect isn't gravity (handled elsewhere)
            if (effectCycle[currIndex].GetEffectType() != EffectType.Gravity)
                // add effect
                playerController.AddEffect(effectCycle[currIndex]);

    }

    private void OnCollisionExit(Collision collision) {

        // check if player collided
        if (collision.transform.CompareTag("Player"))
            // make sure effect isn't gravity (handled elsewhere)
            if (effectCycle[currIndex].GetEffectType() != EffectType.Gravity)
                // remove effect
                playerController.RemoveEffect(effectCycle[currIndex]);

    }

    private void OnTriggerExit(Collider collider) {

        // check if player collided
        if (collider.transform.CompareTag("Player"))
            // make sure effect isn't gravity (handled elsewhere)
            if (effectCycle[currIndex].GetEffectType() != EffectType.Gravity)
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
