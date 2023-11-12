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
    [SerializeField] private bool flipMeshOnCollision;
    [SerializeField] private bool flipPhysicsOnCollision;

    private void Start() {

        meshRenderer = GetComponent<MeshRenderer>();
        rb = GetComponent<Rigidbody>();

        rb.isKinematic = true;
        rb.useGravity = false;
        meshRenderer.enabled = meshVisible;

    }

    private void OnCollisionEnter(Collision collision) {

        if (!triggered && collision.transform.CompareTag("Player")) {

            if (flipMeshOnCollision)
                meshRenderer.enabled = !meshVisible;

            if (flipPhysicsOnCollision) {

                rb.isKinematic = false;
                rb.useGravity = true;

            }

            triggered = true;

        }
    }
}
