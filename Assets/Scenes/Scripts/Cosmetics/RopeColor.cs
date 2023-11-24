using UnityEngine;

public class RopeColor : Cosmetic {

    [Header("References")]
    private LineRenderer lineRenderer;

    [Header("Settings")]
    [SerializeField] private Color startColor;
    [SerializeField] private Color endColor;

    private void Start() {

        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.startColor = startColor;
        lineRenderer.endColor = endColor;

    }

    public void SetStartColor(Color startColor) {

        this.startColor = startColor;

    }

    public void SetEndColor(Color endColor) {

        this.endColor = endColor;

    }

    public override void CopyTo(Cosmetic cosmetic) {

        RopeColor ropeColor = (RopeColor) cosmetic;
        ropeColor.SetStartColor(startColor);
        ropeColor.SetEndColor(endColor);

    }
}
