using UnityEngine;

public class Vent : MonoBehaviour {

    [Header("Settings")]
    [SerializeField] private float ventForce;

    [Header("Collisions")]
    [SerializeField] private string playerTag;

    private void OnCollisionEnter(Collision collision) {

        Transform player = collision.transform;

        if (player.CompareTag(playerTag)) {

            player.GetComponent<Rigidbody>().AddForce(player.up * ventForce + player.forward * ventForce);

        }
    }
}
