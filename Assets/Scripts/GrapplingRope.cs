using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrapplingRope : MonoBehaviour {

    [Header("References")]
    private PlayerController playerController;
    private LineRenderer lineRenderer;
    private Spring spring;

    [Header("Swinging")]
    [SerializeField] private int quality;
    [SerializeField] private float damper;
    [SerializeField] private float strength;
    [SerializeField] private float velocity;
    [SerializeField] private float waveCount;
    [SerializeField] private float waveHeight;
    [SerializeField] private AnimationCurve effectCurve;
    private Vector3 currentSwingPosition;

    private void Start() {

        playerController = GetComponent<PlayerController>();
        lineRenderer = GetComponent<LineRenderer>();
        spring = new Spring();
        spring.SetTarget(0);

    }

    private void LateUpdate() {

        DrawRope();

    }

    private void DrawRope() {

        if (!playerController.IsSwinging()) {

            currentSwingPosition = playerController.GetSwingMuzzle().position;
            spring.Reset();

            if (lineRenderer.positionCount > 0)
                lineRenderer.positionCount = 0;

            return;

        }

        if (lineRenderer.positionCount == 0) {

            spring.SetVelocity(velocity);
            lineRenderer.positionCount = quality + 1;

        }

        spring.SetDamper(damper);
        spring.SetStrength(strength);
        spring.Update(Time.deltaTime);

        Vector3 swingPoint = playerController.GetSwingPoint();
        Vector3 muzzlePos = playerController.GetSwingMuzzle().position;
        Vector3 up = Quaternion.LookRotation(swingPoint - muzzlePos).normalized * Vector3.up;

        // TODO: Make 12f a variable?
        currentSwingPosition = Vector3.Lerp(currentSwingPosition, swingPoint, Time.deltaTime * 12f);

        for (int i = 0; i < quality + 1; i++) {

            var delta = i / (float) quality;
            Vector3 offset = up * waveHeight * Mathf.Sin(delta * waveCount * Mathf.PI * spring.Value * effectCurve.Evaluate(delta));

            lineRenderer.SetPosition(i, Vector3.Lerp(muzzlePos, currentSwingPosition, delta) + offset);

        }
    }
}
