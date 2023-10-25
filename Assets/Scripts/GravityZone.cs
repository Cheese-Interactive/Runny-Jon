using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityZone : MonoBehaviour {

    [Header("References")]
    [SerializeField] private string playerTag;

    [Header("Settings")]
    [SerializeField] private float gravityMultiplier;

    private void OnTriggerStay(Collider collider) {

        if (collider.CompareTag(playerTag))
            collider.GetComponent<Rigidbody>().AddForce(transform.up * Physics.gravity.y * gravityMultiplier);

    }
}
