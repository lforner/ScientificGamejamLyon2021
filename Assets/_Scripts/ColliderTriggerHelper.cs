using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class ColliderTriggerHelper : MonoBehaviour {
    [HideInInspector]
    public GameObject CollidingWith;

    private void OnTriggerEnter(Collider other) {
        CollidingWith = other.gameObject;
    }

    private void OnTriggerExit(Collider other) {
        CollidingWith = null;
    }
}
