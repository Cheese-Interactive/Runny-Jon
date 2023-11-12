using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisappearingPlatform : MonoBehaviour {

    [Header("References")]
    private new Collider collider;
    private MeshRenderer meshRenderer;

    [Header("Disappearing")]
    [SerializeField] private float disappearWaitDuration;
    [SerializeField] private float disappearAnimationsDuration;
    [SerializeField] private float resetWaitDuration;
    [SerializeField] private float resetAnimationsDuration;
    private Coroutine disappearCoroutine;

    [Header("Falling")]
    [SerializeField] private float fallDistance;

    private void Start() {

        collider = GetComponent<Collider>();
        meshRenderer = GetComponent<MeshRenderer>();

    }

    private void OnCollisionEnter(Collision collision) {

        if (disappearCoroutine != null)
            return;

        if (collision.gameObject.CompareTag("Player"))
            disappearCoroutine = StartCoroutine(HandleFade());

    }

    private IEnumerator HandleFade() {

        yield return new WaitForSeconds(disappearWaitDuration);

        float currentTime = 0f;
        Color startColor = meshRenderer.material.color;
        Color targetColor = new Color(startColor.r, startColor.g, startColor.b, 0f);
        Vector3 startPosition = transform.position;
        Vector3 targetPosition = new Vector3(startPosition.x, startPosition.y - fallDistance, startPosition.z);

        while (currentTime < disappearAnimationsDuration) {

            currentTime += Time.deltaTime;
            meshRenderer.material.color = Color.Lerp(startColor, targetColor, currentTime / disappearAnimationsDuration);
            transform.position = Vector3.Lerp(startPosition, targetPosition, currentTime / disappearAnimationsDuration);
            yield return null;

        }

        meshRenderer.material.color = targetColor;
        transform.position = targetPosition;

        collider.enabled = false;
        meshRenderer.enabled = false;

        yield return new WaitForSeconds(resetWaitDuration);

        meshRenderer.material.color = startColor;

        meshRenderer.enabled = true;

        currentTime = 0f;

        while (currentTime < resetAnimationsDuration) {

            currentTime += Time.deltaTime;
            meshRenderer.material.color = Color.Lerp(targetColor, startColor, currentTime / resetAnimationsDuration);
            transform.position = Vector3.Lerp(targetPosition, startPosition, currentTime / resetAnimationsDuration);
            yield return null;

        }

        meshRenderer.material.color = startColor;
        transform.position = startPosition;
        collider.enabled = true;

        disappearCoroutine = null;

    }
}
