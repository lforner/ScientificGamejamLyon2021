using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimalSelection : MonoBehaviour
{
    public void OnSelect()
    {
        Debug.Log($"Selected animal: {gameObject.name}");
        CamerasManager.S.FollowTarget(transform);
    }

    private void OnDestroy()
    {
        CamerasManager.S.UnfollowTarget();
    }
}
