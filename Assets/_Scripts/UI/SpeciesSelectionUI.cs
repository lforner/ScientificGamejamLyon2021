using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeciesSelectionUI : PanelBase
{
    public void StartGame(int speciesIndex)
    {
        switch (speciesIndex)
        {
            case 0:
                GameManager.S.SelectedSpecies = AnimalSpeciesType.Rock;
                break;
            case 1:
                GameManager.S.SelectedSpecies = AnimalSpeciesType.Paper;
                break;
            default:
                GameManager.S.SelectedSpecies = AnimalSpeciesType.Cisor;
                break;
        }

        Hide();
        GameManager.S.StartGame();
    }
}
