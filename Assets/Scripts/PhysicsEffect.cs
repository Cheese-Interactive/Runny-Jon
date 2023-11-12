using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PhysicsEffect : MonoBehaviour {

    [Header("References")]
    private Rigidbody rb;
    private bool physicsEnabled;

    private void Start() {

        rb = GetComponent<Rigidbody>();
        rb.isKinematic = true;

    }

    private void OnCollisionEnter(Collision collision) {

        if (!physicsEnabled && collision.transform.CompareTag("Player")) {

            rb.isKinematic = false;
            physicsEnabled = true;

        }
    }
}
