using System.Collections;
using UnityEngine;

public class MovingPlatform : MonoBehaviour {

    [Header("Falling")]
    [SerializeField] private Vector3 movement;
    [SerializeField] private float movementDelay;
    [SerializeField] private float movementDuration;
    private Transform prevParent;
    private Coroutine fallCoroutine;

    private void OnCollisionEnter(Collision collision) {

        if (fallCoroutine != null)
            return;

        if (collision.gameObject.CompareTag("Player")) {

            prevParent = collision.transform.parent;
            collision.transform.parent = transform;
            fallCoroutine = StartCoroutine(HandleMovement());

        }
    }

    private void OnTriggerEnter(Collider collider) {

        if (fallCoroutine != null)
            return;

        if (collider.gameObject.CompareTag("Player")) {

            prevParent = collider.transform.parent;
            collider.transform.parent = transform;
            fallCoroutine = StartCoroutine(HandleMovement());

        }
    }

    private void OnCollisionExit(Collision collision) {

        if (collision.gameObject.CompareTag("Player"))
            collision.transform.parent = prevParent;

    }

    private void OnTriggerExit(Collider collider) {

        if (collider.gameObject.CompareTag("Player"))
            collider.transform.parent = prevParent;

    }

    private IEnumerator HandleMovement() {

        yield return new WaitForSeconds(movementDelay);

        float currentTime = 0f;
        Vector3 startPosition = transform.position;
        Vector3 targetPosition = startPosition + movement;

        while (currentTime < movementDuration) {

            currentTime += Time.deltaTime;
            transform.position = Vector3.Lerp(startPosition, targetPosition, currentTime / movementDuration);
            yield return null;

        }

        transform.position = targetPosition;
        fallCoroutine = null;

    }
}
