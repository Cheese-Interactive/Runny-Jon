using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityZone : MonoBehaviour {

    [Header("Settings")]
    [SerializeField] private float gravityMultiplier;

    private void OnTriggerStay(Collider collider) {

        if (collider.CompareTag("Player"))
            collider.GetComponent<Rigidbody>().AddForce(transform.up * Physics.gravity.y * gravityMultiplier);

    }
}
