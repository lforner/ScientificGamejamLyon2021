using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelBase : MonoBehaviour
{
    public float FadeDuration = 1;
    private CanvasGroup _canvasGroup;

    private void Start()
    {
        _canvasGroup = GetComponent<CanvasGroup>();
    }

    public void Show()
    {
        _canvasGroup.DOFade(1, FadeDuration).timeScale = 1 / Time.timeScale;
        _canvasGroup.interactable = _canvasGroup.blocksRaycasts = true;
    }
    public void Hide()
    {
        _canvasGroup.DOFade(0, FadeDuration).timeScale = 1 / Time.timeScale;
        _canvasGroup.interactable = _canvasGroup.blocksRaycasts = false;
    }
}
