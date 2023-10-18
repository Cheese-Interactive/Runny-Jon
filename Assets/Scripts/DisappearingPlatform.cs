using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Device;

public class DisappearingPlatform : MonoBehaviour {

    [Header("References")]
    private new Collider collider;
    private MeshRenderer meshRenderer;

    [Header("Disappearing")]
    [SerializeField] private float disappearDuration;
    [SerializeField] private float resetDuration;
    [SerializeField] private float resetFadeDuration;
    [SerializeField] private string playerTag;

    private void Start() {

        collider = GetComponent<Collider>();
        meshRenderer = GetComponent<MeshRenderer>();

    }

    private void OnCollisionEnter(Collision collision) {

        if (collision.gameObject.CompareTag(playerTag))
            StartCoroutine(HandleDisappear());

    }

    private IEnumerator HandleDisappear() {

        float currentTime = 0f;
        Color startColor = meshRenderer.material.color;
        Color targetColor = new Color(startColor.r, startColor.g, startColor.b, 0f);

        while (currentTime < disappearDuration) {

            currentTime += Time.deltaTime;
            meshRenderer.material.color = Color.Lerp(startColor, targetColor, currentTime / disappearDuration);
            yield return null;

        }

        meshRenderer.material.color = targetColor;

        collider.enabled = false;
        meshRenderer.enabled = false;

        yield return new WaitForSeconds(resetDuration);

        meshRenderer.material.color = startColor;

        collider.enabled = true;
        meshRenderer.enabled = true;

        currentTime = 0f;

        while (currentTime < resetFadeDuration) {

            currentTime += Time.deltaTime;
            meshRenderer.material.color = Color.Lerp(targetColor, startColor, currentTime / resetFadeDuration);
            yield return null;

        }

        meshRenderer.material.color = startColor;

    }
}
