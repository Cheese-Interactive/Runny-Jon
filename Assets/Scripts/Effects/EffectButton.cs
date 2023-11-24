using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectButton : MonoBehaviour {

    [Header("Objects")]
    [SerializeField] private List<EffectObject> effectObjects;

    [Header("Effects")]
    [SerializeField] private float pressCooldown;
    private bool pressable;

    private void Start() {

        // make button pressable
        pressable = true;

    }

    private void OnCollisionEnter(Collision collision) {

        // check if button is pressable & player collided
        if (pressable && collision.transform.CompareTag("Player")) {

            // change each object's color to current effect color
            foreach (EffectObject effectObj in effectObjects)
                effectObj.ChangeEffect();

            // start press cooldown
            StartCoroutine(HandlePressCooldown(pressCooldown));

        }
    }

    private IEnumerator HandlePressCooldown(float duration) {

        pressable = false;
        yield return new WaitForSeconds(duration);
        pressable = true;

    }
}

[Serializable]
public class Effect {

    [Header("Settings")]
    [SerializeField] private EffectType effectType;
    [SerializeField] private float multiplier;
    [SerializeField] private Color effectColor;
    [SerializeField] private float colorFadeDuration;

    public bool Equals(Effect other) {

        return effectType == other.GetEffectType() && multiplier == other.GetMultiplier() && effectColor == other.GetEffectColor() && colorFadeDuration == other.GetColorFadeDuration();

    }

    public EffectType GetEffectType() {

        return effectType;

    }

    public float GetMultiplier() {

        return multiplier;

    }

    public Color GetEffectColor() {

        return effectColor;

    }

    public float GetColorFadeDuration() {

        return colorFadeDuration;

    }
}

public enum EffectType {

    None, Speed, Jump, Gravity

}