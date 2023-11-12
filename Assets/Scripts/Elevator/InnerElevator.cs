using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InnerElevator : MonoBehaviour {

    [Header("Elevator")]
    [SerializeField] private Elevator elevator;

    private void OnTriggerEnter(Collider collider) {

        if (collider.CompareTag("Player"))
            elevator.ElevatorEntered();

    }

    private void OnTriggerExit(Collider collider) {

        if (collider.CompareTag("Player"))
            elevator.ElevatorExited();

    }
}
