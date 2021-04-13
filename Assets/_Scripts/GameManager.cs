using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq;

public class GameManager : MonoBehaviour
{
    public static GameManager S;

    //public List<List<GameObject>> AnimalsLists = new List<List<GameObject>>();
    [HideInInspector] public VictoryUI VictoryUI;
    [HideInInspector] public DefeatUI DefeatUI;
    [HideInInspector] public GameObject AnimalContainer;

    [Header("Dynamic")]
    [Tooltip("Scaled time between two perturbations in seconds")]
    public float TimeBetweenPerturbations = 1;
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
        Application.targetFrameRate = 30;
    }

    private void Start()
    {
    }

    private void Update()
    {
        if (HasGameStarted && Input.GetMouseButtonDown(0) && CamerasManager.S.IsFollowing)
        {
            AnimalDetailsUI.S.HideAnimalDetails();
        }
    }

    private IEnumerator TriggerDisruptions(AsyncOperation loading)
    {
        yield return loading;

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

        var animal = AnimalContainer.GetComponentsInChildren<AnimalBehaviour>().FirstOrDefault((a) => a.SpeciesType != SelectedSpecies);
        if (animal != null) {
            animal.ChildGenome.Speed.Increment(1);
        }
    }

    public void StartGame()
    {
        AsyncOperation loading = SceneManager.LoadSceneAsync(1, LoadSceneMode.Additive);
        Invoke(nameof(UnloadMenuScene), 1);
        HasGameStarted = true;
        StartCoroutine(TriggerDisruptions(loading));
    }

    private void UnloadMenuScene()
    {
        SceneManager.UnloadSceneAsync(0);
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
