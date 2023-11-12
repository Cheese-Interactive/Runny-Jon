using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PhysicsEffect : MonoBehaviour {

    [Header("References")]
    private MeshRenderer meshRenderer;
    private Rigidbody rb;
    private bool physicsEnabled;

    [Header("Mesh Settings")]
    [SerializeField] private bool meshVisible;
    [SerializeField] private bool flipOnCollision;

    private void Start() {

        meshRenderer = GetComponent<MeshRenderer>();
        rb = GetComponent<Rigidbody>();

        meshRenderer.enabled = meshVisible;
        rb.isKinematic = true;

    }

    private void OnCollisionEnter(Collision collision) {

        if (!physicsEnabled && collision.transform.CompareTag("Player")) {

            if (flipOnCollision)
                meshRenderer.enabled = !meshVisible;

            rb.isKinematic = false;
            physicsEnabled = true;

        }
    }
}
