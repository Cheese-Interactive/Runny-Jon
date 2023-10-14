using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu]
public class Level : ScriptableObject {

    [Header("Settings")]
    public string levelName;
    public int levelNumber;
    public Object scene;
    public Sprite icon;
    [Range(1, 3599)] public int timeLimit;
    public Transform objective;
    public bool isTutorial;

}