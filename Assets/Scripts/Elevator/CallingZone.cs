using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CallingZone : MonoBehaviour {

    [Header("References")]
    [SerializeField] private Elevator elevator;
    [SerializeField] private string playerTag;
    [SerializeField] private ZoneType zoneType;

    public enum ZoneType {

        Top, Bottom

    }

    private void OnTriggerEnter(Collider collider) {


    }
}
