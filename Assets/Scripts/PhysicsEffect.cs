using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Collider))]
public class PhysicsEffect : MonoBehaviour {

    [Header("References")]
    private MeshRenderer meshRenderer;
    private new Collider collider;
    private Rigidbody rb;
    private bool triggered;

    [Header("Mesh Settings")]
    [SerializeField] private bool meshVisible;
    [SerializeField] private bool flipMeshOnCollision;
    [SerializeField] private bool flipPhysicsOnCollision;

    private void Start() {

        meshRenderer = GetComponent<MeshRenderer>();
        collider = GetComponent<Collider>();
        rb = GetComponent<Rigidbody>();

        rb.constraints = RigidbodyConstraints.FreezeAll;
        rb.freezeRotation = true;
        rb.useGravity = false;
        meshRenderer.enabled = meshVisible;
        collider.enabled = meshVisible;

    }

    private void OnCollisionEnter(Collision collision) {

        if (!triggered && collision.transform.CompareTag("Player")) {

            if (flipMeshOnCollision) {

                meshRenderer.enabled = !meshVisible;
                collider.enabled = !meshVisible;

            }

            if (flipPhysicsOnCollision) {

                rb.constraints = RigidbodyConstraints.None;
                rb.freezeRotation = false;
                rb.useGravity = true;

            }

            triggered = true;

        }
    }

    public void OnThirdPartyCollision() {

        if (triggered)
            return;

        if (flipMeshOnCollision) {

            meshRenderer.enabled = !meshVisible;
            collider.enabled = !meshVisible;

        }

        if (flipPhysicsOnCollision) {

            rb.constraints = RigidbodyConstraints.None;
            rb.freezeRotation = false;
            rb.useGravity = true;

        }

        triggered = true;
    }
}
