using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AnimalsCountUI : MonoBehaviour
{
    public static AnimalsCountUI S;

    public Dictionary<AnimalSpeciesType, int> AnimalsCount = new Dictionary<AnimalSpeciesType, int>()
    {
        { AnimalSpeciesType.Rock, 0 },
        { AnimalSpeciesType.Paper, 0 },
        { AnimalSpeciesType.Cisor, 0 }
    };

    private TextMeshProUGUI _text;

    void Awake()
    {
        S = this;
        _text = GetComponentInChildren<TextMeshProUGUI>();
    }

    public void IncrementsAnimalsCount(AnimalSpeciesType type, int number = 1)
    {
        AnimalsCount[type] += number;
        UpdateAnimalsCount();
    }

    public void UpdateAnimalsCount()
    {
        int rocksNb = AnimalsCount[AnimalSpeciesType.Rock];
        int papersNb = AnimalsCount[AnimalSpeciesType.Paper];
        int scisorsNb = AnimalsCount[AnimalSpeciesType.Cisor];

        _text.text = $"Pierres : {rocksNb}\n" +
            $"Feuilles : {papersNb}\n" +
            $"Ciseaux : {scisorsNb}";

        if (rocksNb + papersNb + scisorsNb <= 0)
        {
            GameManager.S.LoseGame();
        }
    }

    //public void SetAnimalsCount(int rocksNb, int papersNb, int cisorsNb)
    //{
    //    AnimalsCount[AnimalSpeciesType.Rock] = rocksNb;
    //    AnimalsCount[AnimalSpeciesType.Paper] = papersNb;
    //    AnimalsCount[AnimalSpeciesType.Cisor] = cisorsNb;

    //    UpdateAnimalsCount();
    //}
}
