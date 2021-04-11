using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager S;

    public List<List<GameObject>> AnimalsLists = new List<List<GameObject>>();
    [HideInInspector] public VictoryUI VictoryUI;
    [HideInInspector] public DefeatUI DefeatUI;

    [Header("Dynamic")]
    [Tooltip("Scaled time between two perturbations in seconds")]
    public float TimeBetweenPerturbations = 10;
    public int NumberOfPerturbationsToWin = 10;
    public bool EndlessMode;

    public int WaveNumber { get; private set; }
    public AnimalSpeciesType SelectedSpecies { get; internal set; }
    public bool HasGameStarted { get; private set; }

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

    private void Update()
    {
        if (HasGameStarted && Input.GetMouseButtonDown(0) && CamerasManager.S.IsFollowing)
        {
            CamerasManager.S.UnfollowTarget();
        }
    }

    private IEnumerator TriggerDisruptions()
    {
        while (EndlessMode || WaveNumber < NumberOfPerturbationsToWin)
        {
            Disrupt();

            yield return new WaitForSeconds(TimeBetweenPerturbations);

            WaveNumber++;
        }

        WinGame();
    }

    private void Disrupt()
    {
        Debug.Log($"Disrupt wave#{WaveNumber}");
    }

    public void StartGame()
    {
        SceneManager.LoadScene(1);
        HasGameStarted = true;
    }

    public void LoseGame()
    {
        Debug.Log("LoseGame");
        DefeatUI.LoseGame();
    }

    public void WinGame()
    {
        Debug.Log("WinGame");
        VictoryUI.WinGame();
    }
}
