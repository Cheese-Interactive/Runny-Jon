using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Objective : MonoBehaviour {

    [Header("References")]
    private GameManager gameManager;

    private void Start() {

        gameManager = FindObjectOfType<GameManager>();

    }
    private void OnTriggerEnter(Collider collision) {

        if (collision.transform.CompareTag("Player")) {

            gameManager.CompleteLevel();

        }
    }
}
