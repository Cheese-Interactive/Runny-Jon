using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathZone : MonoBehaviour {

    [Header("References")]
    private GameManager gameManager;

    [Header("Deaths")]
    [SerializeField] private string playerTag;

    private void Start() {

        gameManager = FindObjectOfType<GameManager>();

    }

    private void OnTriggerEnter(Collider collider) {

        if (collider.CompareTag(playerTag)) {

            gameManager.KillPlayer();

        }
    }
}
