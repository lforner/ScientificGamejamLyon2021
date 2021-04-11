using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefeatUI : MonoBehaviour
{
    public float FadeDuration = 1;

    // Start is called before the first frame update
    void Start()
    {
        GameManager.S.DefeatUI = this;
    }

    public void LoseGame()
    {
        GetComponent<CanvasGroup>().DOFade(1, FadeDuration);
    }
}
