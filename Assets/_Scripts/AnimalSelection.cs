using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimalSelection : MonoBehaviour
{
    public void OnSelect()
    {
        CamerasManager.S.FollowTarget(transform);
        AnimalDetailsUI.S.ShowAnimalDetails(GetComponent<AnimalBehaviour>().ChildGenome);
    }

    private void OnDestroy()
    {
        CamerasManager.S.UnfollowTarget();
        AnimalDetailsUI.S.HideAnimalDetails();
    }
}
