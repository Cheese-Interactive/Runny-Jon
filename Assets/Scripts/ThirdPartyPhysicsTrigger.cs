using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPartyPhysicsTrigger : MonoBehaviour {

    [Header("References")]
    [SerializeField] private PhysicsEffect physicsEffect;

    private void OnCollisionEnter(Collision collision) {

        if (collision.transform.CompareTag("Player")) {

            physicsEffect.OnThirdPartyCollision();

        }
    }
}
