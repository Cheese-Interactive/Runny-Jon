using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Device;

public class Collectible : MonoBehaviour {

    [Header("References")]
    private GameManager gameManager;
    private Material material;
    private bool collected;

    [Header("Settings")]
    [SerializeField] private CollectibleType collectibleType;

    [Header("Animations")]
    [SerializeField] private float fadeDuration;

    private void Start() {

        gameManager = FindObjectOfType<GameManager>();
        material = GetComponent<MeshRenderer>().material;

    }

    private void OnTriggerEnter(Collider collider) {

        // player collided with object
        if (!collected && collider.transform.CompareTag("Player")) {

            gameManager.CollectCollectible(this);
            collected = true;
            StartCoroutine(FadeOutCollectible(fadeDuration));

        }
    }

    private IEnumerator FadeOutCollectible(float duration) {

        float currentTime = 0f;
        Color startColor = material.color;
        Color targetColor = new Color(material.color.r, material.color.g, material.color.b, 0f);

        while (currentTime < duration) {

            currentTime += Time.deltaTime;
            material.color = Color.Lerp(startColor, targetColor, currentTime / duration);
            yield return null;

        }

        material.color = targetColor;
        gameObject.SetActive(false);

    }

    public CollectibleType GetCollectibleType() {

        return collectibleType;

    }
}

public enum CollectibleType {

    Queso, Other

}