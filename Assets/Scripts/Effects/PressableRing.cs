using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressableRing : MonoBehaviour {

    [Header("References")]
    [SerializeField] private EffectButton effectButton;
    [SerializeField] private Transform pressTarget;

    [Header("Pressing")]
    [SerializeField] private float pressDuration;
    [SerializeField] private float pressCooldown;
    [SerializeField] private float resetDuration;
    private Vector3 startPosition;
    private bool pressable;

    private void Start() {

        // store start position for pressing
        startPosition = transform.position;

        // make button pressable
        pressable = true;

    }

    private void OnCollisionEnter(Collision collision) {

        if (pressable && collision.transform.CompareTag("Player")) {

            effectButton.OnPress();

            // start press animation
            StartCoroutine(HandlePressAnimation(pressTarget.position, pressDuration, pressCooldown));

        }
    }

    private IEnumerator HandlePressAnimation(Vector3 targetPosition, float pressDuration, float cooldown) {

        pressable = false;

        float currentTime = 0f;

        while (currentTime < pressDuration) {

            currentTime += Time.unscaledDeltaTime;
            transform.position = Vector3.Lerp(startPosition, targetPosition, currentTime / pressDuration);
            yield return null;

        }

        transform.position = targetPosition;

        yield return new WaitForSeconds(cooldown);

        currentTime = 0f;

        while (currentTime < resetDuration) {

            currentTime += Time.unscaledDeltaTime;
            transform.position = Vector3.Lerp(targetPosition, startPosition, currentTime / resetDuration);
            yield return null;

        }

        transform.position = startPosition;

        pressable = true;

    }
}
