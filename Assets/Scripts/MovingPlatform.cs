using System.Collections;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{

    [Header("References")]
    [SerializeField] private string playerTag;

    [Header("Falling")]
    [SerializeField] private Vector3 movement;
    [SerializeField] private float movementDelay;
    [SerializeField] private float movementDuration;
    private Coroutine fallCoroutine;

    private void OnCollisionEnter(Collision collision)
    {

        if (fallCoroutine != null)
            return;

        if (collision.gameObject.CompareTag(playerTag))
            fallCoroutine = StartCoroutine(HandleMovement());

    }

    private void OnTriggerEnter(Collider collider)
    {

        if (fallCoroutine != null)
            return;

        if (collider.gameObject.CompareTag(playerTag))
            fallCoroutine = StartCoroutine(HandleMovement());

    }

    private IEnumerator HandleMovement()
    {

        yield return new WaitForSeconds(movementDelay);

        float currentTime = 0f;
        Vector3 startPosition = transform.position;
        Vector3 targetPosition = startPosition + movement;

        while (currentTime < movementDuration)
        {

            currentTime += Time.deltaTime;
            transform.position = Vector3.Lerp(startPosition, targetPosition, currentTime / movementDuration);
            yield return null;

        }

        transform.position = targetPosition;
        fallCoroutine = null;

    }
}
