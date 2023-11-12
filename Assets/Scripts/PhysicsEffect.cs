using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PhysicsEffect : MonoBehaviour {

    [Header("References")]
    private MeshRenderer meshRenderer;
    private Rigidbody rb;
    private bool triggered;

    [Header("Mesh Settings")]
    [SerializeField] private bool meshVisible;
    [SerializeField] private bool flipOnCollision;

    [Header("Physics Settings")]
    [SerializeField] private bool physicsEnabled;

    private void Start() {

        meshRenderer = GetComponent<MeshRenderer>();
        rb = GetComponent<Rigidbody>();

        meshRenderer.enabled = meshVisible;
        rb.isKinematic = true;

    }

    private void OnCollisionEnter(Collision collision) {

        if (!triggered && collision.transform.CompareTag("Player")) {

            if (flipOnCollision)
                meshRenderer.enabled = !meshVisible;

            if (physicsEnabled) {

                rb.isKinematic = false;
                physicsEnabled = true;

            }

            triggered = true;

        }
    }
}
