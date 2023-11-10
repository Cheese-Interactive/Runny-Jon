using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelLayout : MonoBehaviour {

    [Header("References")]
    [SerializeField] private Transform levelsParent;

    public Transform GetLevelsParent() {

        return levelsParent;

    }
}
