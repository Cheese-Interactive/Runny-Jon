using UnityEngine;

public class Vent : MonoBehaviour {

    [Header("Settings")]
    [SerializeField] private float ventForce;

    private void OnCollisionEnter(Collision collision) {

        Transform player = collision.transform;

        if (player.CompareTag("Player")) {

            player.GetComponent<Rigidbody>().AddForce(player.up * ventForce + player.forward * ventForce);

        }
    }
}
