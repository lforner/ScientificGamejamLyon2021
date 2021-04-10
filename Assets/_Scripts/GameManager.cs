using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager S;

    public List<List<GameObject>> AnimalsLists = new List<List<GameObject>>();


    // Start is called before the first frame update
    void Awake()
    {
        S = this;
        DontDestroyOnLoad(this);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
