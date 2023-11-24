using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InnerElevator : MonoBehaviour {

    [Header("Elevator")]
    [SerializeField] private Elevator elevator;
    private PlayerController playerController;
    private Transform prevParent;
    private bool playerInside;

    private void Start() {

        playerController = FindObjectOfType<PlayerController>();

    }

    private void OnTriggerEnter(Collider collider) {

        if (collider.CompareTag("Player")) {

            playerController.SetInElevator(true);
            prevParent = collider.transform.parent;
            collider.transform.parent = transform;
            elevator.ElevatorEntered();
            playerInside = true;

        }
    }

    private void OnTriggerExit(Collider collider) {

        if (collider.CompareTag("Player")) {

            playerController.SetInElevator(false);
            collider.transform.parent = prevParent;
            elevator.ElevatorExited();
            playerInside = false;

        }
    }

    public bool IsPlayerInside() {

        return playerInside;

    }
}
