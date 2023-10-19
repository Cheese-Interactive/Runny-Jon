using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zipline : MonoBehaviour {

    [Header("References")]
    [SerializeField] private Zipline targetZipline;
    [SerializeField] private Transform ziplineConnector;
    private PlayerController playerController;
    private LineRenderer lineRenderer;
    private GameObject currZipline;
    private Rigidbody ziplineRb;
    private Vector3 offset;

    [Header("Settings")]
    [SerializeField] private float ziplineSpeed;
    [SerializeField] private float ziplineScale;
    [SerializeField] private float arrivalThreshold; // distance from the end to get off
    private bool isZiplining;

    private void Awake() {

        lineRenderer = ziplineConnector.GetComponent<LineRenderer>();
        lineRenderer.SetPosition(0, ziplineConnector.position);
        lineRenderer.SetPosition(1, targetZipline.ziplineConnector.position);

    }

    private void Start() {

        playerController = FindObjectOfType<PlayerController>();

    }

    private void Update() {

        if (!isZiplining || currZipline == null)
            return;

        currZipline.GetComponent<Rigidbody>().AddForce((targetZipline.ziplineConnector.position - ziplineConnector.position).normalized * ziplineSpeed * Time.deltaTime, ForceMode.Acceleration);
        playerController.transform.position = currZipline.transform.position - offset;

        if (Vector3.Distance(currZipline.transform.position, targetZipline.ziplineConnector.position) <= arrivalThreshold)
            ResetZipline();

    }

    public void StartZipline() {

        if (isZiplining)
            return;

        currZipline = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        currZipline.transform.position = ziplineConnector.position;
        currZipline.transform.localScale = new Vector3(ziplineScale, ziplineScale, ziplineScale);
        ziplineRb = currZipline.gameObject.AddComponent<Rigidbody>();
        ziplineRb.useGravity = false;
        ziplineRb.velocity = (targetZipline.ziplineConnector.position - ziplineConnector.position).normalized * playerController.GetComponent<Rigidbody>().velocity.magnitude;
        currZipline.GetComponent<Collider>().isTrigger = true;
        offset = new Vector3(0f, playerController.GetPlayerHeight(), 0f);
        isZiplining = true;

    }

    public void ResetZipline() {

        if (!isZiplining)
            return;

        Destroy(currZipline);
        currZipline = null;
        isZiplining = false;
        playerController.ResetZipline();

    }
}
