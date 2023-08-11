using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

    [Header("References")]
    [SerializeField] private Transform player;

    [Header("Settings")]
    [SerializeField][Range(0f, 100f)] private float xSensitivity;
    [SerializeField][Range(0f, 100f)] private float ySensitivity;
    [SerializeField][Range(0f, 90f)] private float topCameraClamp;
    [SerializeField][Range(0f, 90f)] private float bottomCameraClamp;
    private float xRotation;
    private float yRotation;

    private void Start() {

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

    }

    private void Update() {

        float mouseX = Input.GetAxisRaw("Mouse X") * xSensitivity * 10f * Time.fixedDeltaTime;
        float mouseY = Input.GetAxisRaw("Mouse Y") * ySensitivity * 10f * Time.fixedDeltaTime;

        yRotation += mouseX;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -topCameraClamp, bottomCameraClamp);

        transform.rotation = Quaternion.Euler(xRotation, yRotation, 0);
        player.rotation = Quaternion.Euler(0, yRotation, 0);

    }
}
