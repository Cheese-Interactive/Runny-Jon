using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level : MonoBehaviour {

    [Header("Settings")]
    public int levelNumber;
    [Range(1, 3599)] public int timeLimit;
    public Transform spawn;
    public Transform[] checkpoints;
    public Transform objective;

}
