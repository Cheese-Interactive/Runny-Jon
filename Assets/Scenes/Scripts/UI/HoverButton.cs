using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class HoverButton : MonoBehaviour {

    [Header("Hovering")]
    [SerializeField] private Color hoverColor;
    [SerializeField] private float hoverFadeDuration;
    private TMP_Text text;
    private Color startColor;
    private Coroutine textColorFadeCoroutine;

    private void Start() {

        text = GetComponentInChildren<TMP_Text>();
        startColor = text.color;

        EventTrigger trigger = gameObject.AddComponent<EventTrigger>();

        EventTrigger.Entry entry1 = new EventTrigger.Entry();
        entry1.eventID = EventTriggerType.PointerEnter;
        entry1.callback.AddListener((data) => StartHover());
        trigger.triggers.Add(entry1);

        EventTrigger.Entry entry2 = new EventTrigger.Entry();
        entry2.eventID = EventTriggerType.PointerExit;
        entry2.callback.AddListener((data) => StopHover());
        trigger.triggers.Add(entry2);

    }

    private void OnDisable() {

        text.color = startColor;

    }

    private void StartHover() {

        if (textColorFadeCoroutine != null)
            StopCoroutine(textColorFadeCoroutine);

        textColorFadeCoroutine = StartCoroutine(FadeTextColor(text, hoverColor, hoverFadeDuration));

    }

    private void StopHover() {

        if (textColorFadeCoroutine != null)
            StopCoroutine(textColorFadeCoroutine);

        textColorFadeCoroutine = StartCoroutine(FadeTextColor(text, startColor, hoverFadeDuration));

    }

    private IEnumerator FadeTextColor(TMP_Text text, Color targetColor, float duration) {

        float currentTime = 0f;
        Color startColor = text.color;
        text.gameObject.SetActive(true);

        while (currentTime < duration) {

            currentTime += Time.deltaTime;
            text.color = Color.Lerp(startColor, targetColor, currentTime / duration);
            yield return null;

        }

        text.color = targetColor;
        textColorFadeCoroutine = null;

    }
}
