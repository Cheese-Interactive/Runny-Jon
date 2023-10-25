using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElevatorDoor : MonoBehaviour {

    [Header("References")]
    private Vector3 startPosition;

    [Header("Settings")]
    [SerializeField] private Vector3 movement;
    [SerializeField] private float movementDelay;
    [SerializeField] private float movementDuration;
    [SerializeField] private float doorCloseDelay;
    private Coroutine doorCoroutine;
    private Coroutine doorCloseCoroutine;
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

        if (doorCloseCoroutine != null)
            StopCoroutine(doorCloseCoroutine);

        doorCoroutine = StartCoroutine(MoveDoor(startPosition + movement, true, true));
        doorCloseCoroutine = StartCoroutine(HandleDoorClose());

    }

    public void Close() {

        if (!doorOpen)
            return;

        if (doorCoroutine != null) {

            StopCoroutine(doorCoroutine);
            doorCoroutine = StartCoroutine(MoveDoor(startPosition, false, false));

        }

        if (doorCloseCoroutine != null)
            StopCoroutine(doorCloseCoroutine);

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

        yield return new WaitForSeconds(doorCloseDelay);
        Close();

    }
}
