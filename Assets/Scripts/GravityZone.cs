using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityZone : MonoBehaviour {

    [Header("References")]
    [SerializeField] private EffectObject effectObject;

    private void OnTriggerEnter(Collider collider) {

        if (collider.CompareTag("Player"))
            effectObject.ZoneEntered();

    }

    private void OnTriggerExit(Collider collider) {

        if (collider.CompareTag("Player"))
            effectObject.ZoneExited();

    }
}
