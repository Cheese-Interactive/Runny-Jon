using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElevatorDoor : MonoBehaviour {

    [Header("References")]
    [SerializeField] private InnerElevator innerElevator;
    private Vector3 startPosition;

    [Header("Settings")]
    [SerializeField] private Vector3 movement;
    [SerializeField] private float movementDelay;
    [SerializeField] private float movementDuration;
    [SerializeField] private float doorCloseDelay;
    private Coroutine doorCoroutine;
    private Coroutine doorTimeoutCoroutine;
    private bool doorOpen;

    private void Start() {

        startPosition = transform.localPosition;

    }

    public void Open() {

        if (doorOpen)
            return;

        if (doorCoroutine != null) {

            StopCoroutine(doorCoroutine);
            doorCoroutine = StartCoroutine(MoveDoor(startPosition + movement, true, false));

        }

        if (doorTimeoutCoroutine != null)
            StopCoroutine(doorTimeoutCoroutine);

        doorCoroutine = StartCoroutine(MoveDoor(startPosition + movement, true, true));
        doorTimeoutCoroutine = StartCoroutine(HandleDoorClose());

    }

    public void Close() {

        if (!doorOpen)
            return;

        if (doorCoroutine != null) {

            //StopCoroutine(doorCoroutine);
            doorCoroutine = StartCoroutine(MoveDoor(startPosition, false, true));

        }

        if (doorTimeoutCoroutine != null)
            StopCoroutine(doorTimeoutCoroutine);

        doorCoroutine = StartCoroutine(MoveDoor(startPosition, false, true));

    }

    private IEnumerator MoveDoor(Vector3 targetPosition, bool openDoor, bool delay) {

        doorOpen = openDoor;

        if (delay)
            yield return new WaitForSeconds(movementDelay);

        float currentTime = 0f;
        Vector3 startPosition = transform.localPosition;

        while (currentTime < movementDuration) {

            currentTime += Time.deltaTime;
            transform.localPosition = Vector3.Lerp(startPosition, targetPosition, currentTime / movementDuration);
            yield return null;

        }

        transform.localPosition = targetPosition;
        doorCoroutine = null;

    }

    private IEnumerator HandleDoorClose() {

        while (innerElevator.IsPlayerInside())
            yield return null;

        yield return new WaitForSeconds(doorCloseDelay);

        Close();

    }
}
