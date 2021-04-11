using System.Collections;
using UnityEngine;

public class AnimalDetailsUI : PanelBase
{
    public static AnimalDetailsUI S;

    private AnimalGenome _genome;

    public float SpeedGeneStep = 1;
    public float VisibilityGeneStep = 1;
    public float EnergyGeneStep = 1;
    public float FertilityGeneStep = 1;

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
        switch (gene)
        {
            case -1:
            case 1:
                _genome.Speed += gene * SpeedGeneStep;
                break;
            case -2:
            case 2:
                _genome.Visibility += gene * VisibilityGeneStep;
                break;
            case -3:
            case 3:
                _genome.EnergyEfficiency += gene * EnergyGeneStep;
                break;
            default:
                _genome.Fertility += gene * FertilityGeneStep;
                break;
        }
        HideAnimalDetails();
    }

    public void HideAnimalDetails()
    {
        Hide();
    }
}