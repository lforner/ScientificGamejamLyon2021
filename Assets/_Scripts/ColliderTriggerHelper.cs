using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class ColliderTriggerHelper : MonoBehaviour {
    public static readonly string BodyName = "Body";
    public static readonly string ViewName = "View";

    [HideInInspector]
    public HashSet<AnimalBehaviour> CollidingWith = new HashSet<AnimalBehaviour>();

    private void OnTriggerEnter(Collider other) {
        if (IsColliderCompatible(other))
            CollidingWith.Add(other.gameObject.GetComponentInParent<AnimalBehaviour>());        
    }

    private void OnTriggerExit(Collider other) {
        if (IsColliderCompatible(other))
            CollidingWith.Remove(other.gameObject.GetComponentInParent<AnimalBehaviour>());
    }

    private bool IsColliderCompatible(Collider other) => name == BodyName && other.name == BodyName || name == ViewName && other.name == BodyName;
}
