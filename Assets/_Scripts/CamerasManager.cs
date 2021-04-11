using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamerasManager : MonoBehaviour
{
    public CinemachineVirtualCamera OrbitalCam;
    public CinemachineVirtualCamera SelectionCam;

    public void FollowTarget(Transform target)
    {
        SelectionCam.Follow = SelectionCam.LookAt = target;
        SelectionCam.Priority = 10;
    }

    public void UnfollowTarget()
    {
        SelectionCam.Priority = 0;
    }
}
