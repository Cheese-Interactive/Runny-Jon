using UnityEngine;

public class CameraFollow : MonoBehaviour {

    [Header("Follow")]
    private Transform target;

    private void Start() {

        target = FindObjectOfType<CameraPosition>().transform;

    }

    private void LateUpdate() {

        transform.position = target.position;
        transform.rotation = target.rotation;

    }
}
