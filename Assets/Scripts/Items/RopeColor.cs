using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RopeColor : MonoBehaviour {

    [Header("References")]
    private LineRenderer lineRenderer;

    [Header("Settings")]
    private Color startColor;
    private Color endColor;

    private void Start() {

        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.startColor = startColor;
        lineRenderer.endColor = endColor;

    }
}
