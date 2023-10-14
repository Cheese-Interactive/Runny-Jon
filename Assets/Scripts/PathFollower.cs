using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFollower : MonoBehaviour {

    [Header("Paths")]
    [SerializeField] private Transform pathSpawn;
    [SerializeField] private List<Path> pathTargets;

    [Header("Headbob")]
    [SerializeField] private bool bobEnabled;
    [SerializeField] private float bobSpeed;
    [SerializeField] private float bobAmount;
    private Vector3 startPos;
    private float timer;

    private void Start() {

        startPos = transform.position;

        transform.position = pathSpawn.position;
        transform.rotation = pathSpawn.rotation;

        StartCoroutine(StartPathFollow());

    }

    private void Update() {

        timer += bobSpeed * Time.deltaTime;
        transform.position = new Vector3(transform.position.x, startPos.y + Mathf.Sin(timer) * bobAmount, transform.position.z);

    }

    private IEnumerator StartPathFollow() {

        foreach (Path targetObj in pathTargets) {

            yield return StartCoroutine(FollowTarget(targetObj));

        }
    }

    private IEnumerator FollowTarget(Path targetObj) {

        float currentTime = 0f;
        Vector3 startPosition = transform.position;
        Quaternion startRotation = transform.rotation;
        Vector3 targetPosition = targetObj.target.position;
        Quaternion targetRotation = targetObj.target.rotation;
        float duration = (targetObj.positionDuration > targetObj.rotationDuration ? targetObj.positionDuration : targetObj.rotationDuration);

        while (currentTime < duration) {

            currentTime += Time.deltaTime;
            transform.position = new Vector3(Mathf.Lerp(startPosition.x, targetPosition.x, currentTime / targetObj.positionDuration), transform.position.y, Mathf.Lerp(startPosition.z, targetPosition.z, currentTime / targetObj.positionDuration));
            transform.rotation = Quaternion.Lerp(startRotation, targetRotation, currentTime / targetObj.rotationDuration);
            yield return null;
        }

        transform.position = targetPosition;
        transform.rotation = targetRotation;

    }
}

[System.Serializable]
public class Path {

    public Transform target;
    public float positionDuration;
    public float rotationDuration;

}
