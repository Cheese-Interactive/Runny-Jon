using System.Collections;
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
    private Coroutine lerpToZiplineCoroutine;

    [Header("Settings")]
    [SerializeField] private float ziplineSpeed;
    [SerializeField] private float ziplineScale;
    [SerializeField] private float lerpToZiplineDuration;
    [SerializeField] private float arrivalThreshold; // distance from the end to get off
    private bool isZiplining;

    private void Awake() {

        if (targetZipline != null) {

            lineRenderer = ziplineConnector.GetComponent<LineRenderer>();
            lineRenderer.SetPosition(0, ziplineConnector.position);
            lineRenderer.SetPosition(1, targetZipline.ziplineConnector.position);

        }
    }

    private void Start() {

        playerController = FindObjectOfType<PlayerController>();
        offset = new Vector3(0f, playerController.GetPlayerHeight(), 0f);

    }

    private void Update() {

        if (!isZiplining || currZipline == null)
            return;

        currZipline.GetComponent<Rigidbody>().AddForce((targetZipline.ziplineConnector.position - ziplineConnector.position).normalized * ziplineSpeed * Time.deltaTime, ForceMode.Acceleration);
        playerController.transform.position = currZipline.transform.position - offset;

        if (Vector3.Distance(currZipline.transform.position, targetZipline.ziplineConnector.position) <= arrivalThreshold)
            ResetZipline(true);

    }

    public bool CanZipline() {

        return targetZipline != null;

    }

    public void StartZipline() {

        if (isZiplining || targetZipline == null)
            return;

        if (lerpToZiplineCoroutine != null)
            StopCoroutine(lerpToZiplineCoroutine);

        currZipline = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        currZipline.transform.position = ziplineConnector.position;
        currZipline.transform.localScale = new Vector3(ziplineScale, ziplineScale, ziplineScale);

        lerpToZiplineCoroutine = StartCoroutine(LerpPlayerToZipline(playerController.transform, currZipline.transform.position - offset, lerpToZiplineDuration));

    }

    private IEnumerator LerpPlayerToZipline(Transform player, Vector3 targetPosition, float duration) {

        float currentTime = 0f;
        Vector3 startPosition = playerController.transform.position;

        while (currentTime < duration) {

            currentTime += Time.deltaTime;
            player.position = Vector3.Lerp(startPosition, targetPosition, currentTime / duration);
            yield return null;

        }

        player.position = targetPosition;
        lerpToZiplineCoroutine = null;

        ziplineRb = currZipline.gameObject.AddComponent<Rigidbody>();
        ziplineRb.useGravity = false;
        ziplineRb.velocity = (targetZipline.ziplineConnector.position - ziplineConnector.position).normalized * playerController.GetComponent<Rigidbody>().velocity.magnitude;
        currZipline.GetComponent<Collider>().isTrigger = true;
        isZiplining = true;

    }

    public void ResetZipline(bool jump) {

        if (!isZiplining)
            return;

        Destroy(currZipline);
        currZipline = null;
        isZiplining = false;
        playerController.ResetZipline(jump);

    }
}
