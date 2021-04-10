using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager S;

    public MutationPointsUI MutationPointsUI;
    public List<List<GameObject>> AnimalsLists = new List<List<GameObject>>();


    void Awake()
    {
        S = this;
        DontDestroyOnLoad(this);
    }
}
