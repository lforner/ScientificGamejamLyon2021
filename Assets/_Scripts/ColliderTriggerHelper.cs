using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class ColliderTriggerHelper : MonoBehaviour {
    [HideInInspector]
    public HashSet<AnimalBehaviour> CollidingWith = new HashSet<AnimalBehaviour>();

    private void OnTriggerEnter(Collider other) {
        CollidingWith.Add(other.gameObject.GetComponentInParent<AnimalBehaviour>());
    }

    private void OnTriggerExit(Collider other) {
        CollidingWith.Remove(other.gameObject.GetComponentInParent<AnimalBehaviour>());
    }
}
