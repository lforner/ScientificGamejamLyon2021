using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefeatUI : PanelBase
{
    void Start()
    {
        GameManager.S.DefeatUI = this;
    }

    public void LoseGame()
    {
        Show();
    }
}
