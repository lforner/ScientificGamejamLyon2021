using System.Collections;
using UnityEngine;

public class AnimalDetailsUI : PanelBase
{
    public static AnimalDetailsUI S;

    private AnimalGenome _genome;

    private void Awake()
    {
        S = this;
    }

    public void ShowAnimalDetails(AnimalGenome genome)
    {
        _genome = genome;
        Show();
    }

    public void ModifyGenome(int gene)
    {
        if (MutationPointsUI.S.MutationPoints <= 1) return;

        MutationPointsUI.S.Increase(-1);

        switch (gene)
        {
            case -1:
            case 1:
                _genome.Speed.Increment(Mathf.Sign(gene));
                break;
            case -2:
            case 2:
                _genome.Visibility.Increment(Mathf.Sign(gene));
                break;
            case -3:
            case 3:
                _genome.EnergyEfficiency.Increment(Mathf.Sign(gene));
                break;
            default:
                _genome.Fertility.Increment(Mathf.Sign(gene));
                break;
        }
        //HideAnimalDetails();
    }

    public void HideAnimalDetails()
    {
        Hide();
        CamerasManager.S.UnfollowTarget();
    }
}