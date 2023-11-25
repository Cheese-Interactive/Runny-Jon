using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerColor : Cosmetic {

    [Header("References")]
    private PlayerController playerController;
    private Material playerMaterial;

    [Header("Settings")]
    [SerializeField] private Color color;

    private void Start() {

        playerController = GetComponent<PlayerController>();
        playerController.SetPlayerColor(color);

    }

    public void SetColor(Color color) {

        this.color = color;

    }

    public override void CopyTo(Cosmetic cosmetic) {

        PlayerColor playerColor = (PlayerColor) cosmetic;
        playerColor.SetColor(color);

    }
}

