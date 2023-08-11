using UnityEngine;

public class CameraFollow : MonoBehaviour {

    [Header("Follow")]
    [SerializeField] private Transform target;

    private void Update() {

        transform.position = target.position;
        transform.rotation = target.rotation;

    }
}
