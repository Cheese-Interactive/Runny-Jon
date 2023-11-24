using System.Collections;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class Zipline : MonoBehaviour {

    [Header("References")]
    [SerializeField] private Zipline targetZipline;
    [SerializeField] private Transform ziplineConnector;
    private PlayerController playerController;
    private LineRenderer lineRenderer;
    private GameObject currZipline;
    private Rigidbody currZiplineRb;
    private Collider currZiplineCollider;
    private Rigidbody playerRb;
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

        currZiplineRb.AddForce((targetZipline.ziplineConnector.position - ziplineConnector.position).normalized * ziplineSpeed * Time.deltaTime, ForceMode.Acceleration);
        playerRb.velocity = currZiplineRb.velocity;
        playerRb.MovePosition(currZipline.transform.position - offset);

        if (Vector3.Distance(currZipline.transform.position, targetZipline.ziplineConnector.position) <= arrivalThreshold)
            ResetZipline();

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
        currZiplineRb = currZipline.GetComponent<Rigidbody>();

        lerpToZiplineCoroutine = StartCoroutine(LerpPlayerToZipline(playerController.transform, currZipline.transform.position - offset, lerpToZiplineDuration));

    }

    private IEnumerator LerpPlayerToZipline(Transform player, Vector3 targetPosition, float duration) {

        float currentTime = 0f;
        Vector3 startPosition = playerController.transform.position;
        playerRb = playerController.GetComponent<Rigidbody>();
        currZiplineCollider = currZipline.GetComponent<Collider>();

        while (currentTime < duration) {

            currentTime += Time.deltaTime;
            player.position = Vector3.Lerp(startPosition, targetPosition, currentTime / duration);
            yield return null;

        }

        player.position = targetPosition;
        lerpToZiplineCoroutine = null;

        currZiplineRb = currZipline.gameObject.AddComponent<Rigidbody>();
        currZiplineRb.useGravity = false;
        currZiplineRb.velocity = (targetZipline.ziplineConnector.position - ziplineConnector.position).normalized * playerRb.velocity.magnitude;
        currZiplineCollider.isTrigger = true;
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
