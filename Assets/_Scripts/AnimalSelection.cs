using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimalSelection : MonoBehaviour
{
    public void OnSelect()
    {
        CamerasManager.S.FollowTarget(transform);

    }

    private void OnDestroy()
    {
        CamerasManager.S.UnfollowTarget();
    }
}
