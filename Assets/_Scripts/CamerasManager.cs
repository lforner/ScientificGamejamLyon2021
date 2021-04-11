using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamerasManager : MonoBehaviour
{
    public static CamerasManager S;

    public CinemachineFreeLook OrbitalCam;
    public CinemachineVirtualCamera SelectionCam;

    [HideInInspector] public bool IsFollowing;

    private void Awake()
    {
        S = this;
    }

    private void Update()
    {
        OrbitalCam.m_XAxis.m_MaxSpeed = Input.GetMouseButton(0) ? 100 : 0;
        OrbitalCam.m_YAxis.m_MaxSpeed = Input.GetMouseButton(0) ? 1 : 0;
    }

    public void FollowTarget(Transform target)
    {
        Debug.Log($"FollowTarget");
        SelectionCam.Follow = SelectionCam.LookAt = target;
        SelectionCam.Priority = 10;
        Invoke(nameof(SetIsFollow), .1f);
    }

    private void SetIsFollow()
    {
        Debug.Log($"SetIsFollow");
        IsFollowing = true;
    }

    public void UnfollowTarget()
    {
        SelectionCam.Priority = 0;
        IsFollowing = false;
    }
}
