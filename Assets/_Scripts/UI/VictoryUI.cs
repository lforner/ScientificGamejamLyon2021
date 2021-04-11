using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class VictoryUI : PanelBase
{
    void Start()
    {
        GameManager.S.VictoryUI = this;
    }

    public void WinGame()
    {
        Show();
    }
}
