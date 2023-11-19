using System.Collections;
using UnityEngine;

public class MovingPlatform : MonoBehaviour {

    [Header("Movement")]
    [SerializeField] private Vector3 movement;
    [SerializeField] private float movementDuration;
    private Transform prevParent;

    [Header("Resetting")]
    [SerializeField] private float resetWaitDuration;
    [SerializeField] private bool rotateEnabled;
    [SerializeField] private float rotateDuration;

    private void Start() {

        StartCoroutine(HandleMovement(transform.position, transform.position + movement));

    }

    private void OnCollisionEnter(Collision collision) {

        if (collision.gameObject.CompareTag("Player")) {

            prevParent = collision.transform.parent;
            collision.transform.parent = transform;

        }
    }

    private void OnTriggerEnter(Collider collider) {

        if (collider.gameObject.CompareTag("Player")) {

            prevParent = collider.transform.parent;
            collider.transform.parent = transform;

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

    private IEnumerator HandleMovement(Vector3 startPosition, Vector3 targetPosition) {

        float currentTime;

        while (true) {

            currentTime = 0f;

            while (currentTime < movementDuration) {

                currentTime += Time.deltaTime;
                transform.position = Vector3.Lerp(startPosition, targetPosition, currentTime / movementDuration);
                yield return null;

            }

            transform.position = targetPosition;

            if (rotateEnabled) {

                Vector3 startRotation = transform.rotation.eulerAngles;
                Vector3 targetRotation = new Vector3(startRotation.x, startRotation.y + 180f, startRotation.z);
                currentTime = 0f;

                while (currentTime < rotateDuration) {

                    currentTime += Time.deltaTime;
                    transform.rotation = Quaternion.Euler(Vector3.Lerp(startRotation, targetRotation, currentTime / rotateDuration));
                    yield return null;

                }

                transform.rotation = Quaternion.Euler(targetRotation);

            }

            Vector3 temp = startPosition;
            startPosition = targetPosition;
            targetPosition = temp;

            yield return new WaitForSeconds(resetWaitDuration);

        }
    }
}
