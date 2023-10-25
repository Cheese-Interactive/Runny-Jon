using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InnerElevator : MonoBehaviour {

    [Header("Elevator")]
    [SerializeField] private Elevator elevator;
    [SerializeField] private string playerTag;

    private void OnTriggerEnter(Collider collider) {

        if (collider.CompareTag(playerTag))
            elevator.ElevatorEntered();

    }

    private void OnTriggerExit(Collider collider) {

        if (collider.CompareTag(playerTag))
            elevator.ElevatorExited();

    }
}
