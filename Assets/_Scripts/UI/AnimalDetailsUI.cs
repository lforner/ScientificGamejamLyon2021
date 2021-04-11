using System.Collections;
using UnityEngine;

public class AnimalDetailsUI : PanelBase
{
    private AnimalGenome _genome;

    public void ShowAnimalDetails(AnimalGenome genome)
    {
        _genome = genome;
        Show();
    }

    public void HideAnimalDetails()
    {
        Hide();
    }
}