using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPartyPhysicsTrigger : MonoBehaviour {

    [Header("References")]
    [SerializeField] private PhysicsTrigger[] physicsEffects;

    private void OnCollisionEnter(Collision collision) {

        if (collision.transform.CompareTag("Player")) {

            foreach (PhysicsTrigger physicsEffect in physicsEffects) {

                physicsEffect.OnThirdPartyCollision();

            }
        }
    }
}
