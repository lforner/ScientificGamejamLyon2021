using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MutationPointsUI : MonoBehaviour
{
    public static MutationPointsUI S;

    [HideInInspector] public float MutationPoints;

    [SerializeField] private Slider _slider;
    [SerializeField] private TextMeshProUGUI _text;
    [Header("Dynamic")]
    [SerializeField] private float _mutationIncreaseSpeed = 1;
    [SerializeField] private readonly float _maxValue = 5;

    private void Awake()
    {
        S = this;
        _slider.maxValue = _maxValue;
    }

    void FixedUpdate()
    {
        float newMutationPoints = MutationPoints + Time.fixedDeltaTime * _mutationIncreaseSpeed;
        if (newMutationPoints < _maxValue)
        {
            MutationPoints = Mathf.Min(_maxValue, newMutationPoints);
            UpdateUI(MutationPoints);
        }
    }

    public void UpdateUI(float mutationPoints)
    {
        _slider.value = mutationPoints;
        //_text.text = $"Mutation points: {mutationPoints:F1}";
        _text.text = $"{mutationPoints:F0}";
    }

    public void Increase(float diff)
    {
        MutationPoints = Mathf.Clamp(MutationPoints + diff, 0, _maxValue);
        UpdateUI(MutationPoints);
    }
}
