using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Device;

public class EffectButton : MonoBehaviour {

    [Header("Objects")]
    [SerializeField] private List<EffectObject> effectObjects;

    public void OnPress() {

        // change each object's color to current effect color
        foreach (EffectObject effectObj in effectObjects)
            effectObj.ChangeEffect();

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