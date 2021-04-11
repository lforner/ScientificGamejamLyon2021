using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class VictoryUI : MonoBehaviour
{
    public float FadeDuration = 1;

    // Start is called before the first frame update
    void Start()
    {
        GameManager.S.VictoryUI = this;
    }

    public void WinGame()
    {
        GetComponent<CanvasGroup>().DOFade(1, FadeDuration);
    }
}
