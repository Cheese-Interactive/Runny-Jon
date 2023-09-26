using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointArrow : MonoBehaviour {

    [Header("References")]
    private Transform player;
    private Material material;

    [Header("Animations")]
    private Coroutine fadeOutCoroutine;

    private void Start() {

        player = FindObjectOfType<PlayerController>().transform;
        material = GetComponent<MeshRenderer>().material;

    }

    private void Update() {

        Vector3 adjustedVec = player.transform.position;
        adjustedVec.y = transform.position.y;
        transform.LookAt(adjustedVec);
        transform.rotation = Quaternion.LookRotation(player.forward);

    }

    public void StartFadeOutArrow(float duration) {

        if (fadeOutCoroutine != null)
            fadeOutCoroutine = null;

        fadeOutCoroutine = StartCoroutine(FadeOutArrow(duration));

    }

    private IEnumerator FadeOutArrow(float duration) {

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
}
