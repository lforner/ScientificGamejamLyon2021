using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager S;

    public List<List<GameObject>> AnimalsLists = new List<List<GameObject>>();

    [Header("Dynamic")]
    [Tooltip("Scaled time between two perturbations in seconds")]
    public float TimeBetweenPerturbations = 10;

    void Awake()
    {
        if (S != null)
        {
            Destroy(gameObject);
            return;
        }
        S = this;
        DontDestroyOnLoad(this);
    }

    private void Start()
    {
        StartCoroutine(TriggerDisruptions());
    }

    private IEnumerator TriggerDisruptions()
    {
        yield return new WaitForSeconds(TimeBetweenPerturbations);

        Disrupt();
    }

    private void Disrupt()
    {
        Debug.Log("Disrupt");
    }

    public static void StartGame()
    {
        SceneManager.LoadScene(1);
    }
}
