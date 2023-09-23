using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour {

    [Header("References")]

    [Header("Checkpoint")]
    private CheckpointType checkpointType;

    public enum CheckpointType {

        Normal, Sprint, Jump, Slide, WallRun, Swing

    }

    private void OnCollisionEnter(Collision collision) {

        
    }
}
