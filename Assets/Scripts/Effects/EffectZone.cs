using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectZone : MonoBehaviour {

    [Header("References")]
    [SerializeField] private EffectObject effectObject;

    [Header("Color")]
    [SerializeField][Range(0f, 1f)] private float opacity;

    private void OnTriggerEnter(Collider collider) {

        // check if player collided
        if (collider.CompareTag("Player"))
            // tell effect object that player entered zone
            effectObject.ZoneEntered();

    }

    private void OnTriggerExit(Collider collider) {

        // check if player collided
        if (collider.CompareTag("Player"))
            // tell effect object that player exited zone
            effectObject.ZoneExited();

    }

    public float GetOpacity() {

        return opacity;

    }
}
