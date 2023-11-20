using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Collider))]
public class PhysicsTrigger : MonoBehaviour {

    [Header("References")]
    private MeshRenderer meshRenderer;
    private GameAudioManager audioManager;
    private new Collider collider;
    private Rigidbody rb;
    private bool triggered;

    [Header("Mesh Settings")]
    [SerializeField] private bool meshVisible;
    [SerializeField] private bool flipMesh;
    [SerializeField] private bool enablePhysics;
    [SerializeField] private bool disableCollider;
    [SerializeField] private float colliderDisableDelay;

    private void Start() {

        meshRenderer = GetComponent<MeshRenderer>();
        audioManager = FindObjectOfType<GameAudioManager>();
        collider = GetComponent<Collider>();
        rb = GetComponent<Rigidbody>();

        rb.constraints = RigidbodyConstraints.FreezeAll;
        rb.freezeRotation = true;
        rb.useGravity = false;
        meshRenderer.enabled = meshVisible;
        collider.enabled = disableCollider;

    }

    private void OnCollisionEnter(Collision collision) {

        if (!triggered && collision.transform.CompareTag("Player")) {

            audioManager.PlaySound(GameAudioManager.GameSoundEffectType.Shatter);

            if (flipMesh)
                meshRenderer.enabled = !meshVisible;

            if (enablePhysics) {

                rb.constraints = RigidbodyConstraints.None;
                rb.freezeRotation = false;
                rb.useGravity = true;

            }

            if (disableCollider)
                StartCoroutine(HandleColliderDisable(colliderDisableDelay));

            triggered = true;

        }
    }

    public void OnThirdPartyCollision() {

        if (triggered)
            return;

        audioManager.PlaySound(GameAudioManager.GameSoundEffectType.Shatter);

        if (flipMesh)
            meshRenderer.enabled = !meshVisible;

        if (enablePhysics) {

            rb.constraints = RigidbodyConstraints.None;
            rb.freezeRotation = false;
            rb.useGravity = true;

        }

        if (disableCollider)
            StartCoroutine(HandleColliderDisable(colliderDisableDelay));

        triggered = true;
    }

    private IEnumerator HandleColliderDisable(float duration) {

        yield return new WaitForSeconds(duration);
        collider.enabled = false;

    }
}
