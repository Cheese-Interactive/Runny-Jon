using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPartyPhysicsTrigger : MonoBehaviour {

    [Header("References")]
    [SerializeField] private PhysicsTrigger[] physicsTriggers;

    private void OnCollisionEnter(Collision collision) {

        if (collision.transform.CompareTag("Player")) {

            foreach (PhysicsTrigger physicsEffect in physicsTriggers) {

                physicsEffect.OnThirdPartyCollision();

            }
        }
    }
}
