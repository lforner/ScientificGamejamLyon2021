using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class AnimalGenome {
    [Tooltip("Speed, from 1 to 3")]
    public RangedValue Speed = new RangedValue(1, 3);

    [Tooltip("Fertility, from 0.25 to 0.5")]
    public RangedValue Fertility = new RangedValue(0.25f, 0.5f);

    [Tooltip("Visibility, from 1 to 2")]
    public RangedValue Visibility = new RangedValue(1, 2);
    public float HalfFieldOfView => Visibility.ToScale(40, 150);
    public float ViewDistance => Visibility.ToScale(1, 2);

    [Tooltip("EnergyEfficiency, from 1 to 2")]
    public RangedValue EnergyEfficiency = new RangedValue(1, 2);

    public AnimalGenome ChildGenome(AnimalGenome parent) {
        return new AnimalGenome() {
            Speed = getChildValue(Speed, parent.Speed),
            Fertility = getChildValue(Fertility, parent.Fertility),
            Visibility = getChildValue(Visibility, parent.Visibility),
            EnergyEfficiency = getChildValue(EnergyEfficiency, parent.EnergyEfficiency),
        };            
    }

    private RangedValue getChildValue(RangedValue parent1Value, RangedValue parent2Value) {
        var childValue = GaussianRandom.generateNormalRandom((parent1Value.Value + parent2Value.Value) / 2, Mathf.Abs(parent2Value.Value - parent1Value.Value) / 4);
        return new RangedValue(parent1Value.Min, parent1Value.Max) { Value = childValue };
    }
}

[System.Serializable]
public class RangedValue {
    public readonly float Min;
    public readonly float Max;
    public float Value;

    public RangedValue(float min, float max) {
        Min = min;
        Max = max;
        Value = Min;
    }

    public void Increment(float sign) => Value = Mathf.Clamp(Value + sign * (Max - Min) / 10, Min, Max);

    public float ToScale(float newScaleMin, float newScaleMax) {
        var b = (newScaleMax - newScaleMin) / (Max - Min);
        var a = newScaleMax - b * Max;
        return a + b * Value;
    }
}