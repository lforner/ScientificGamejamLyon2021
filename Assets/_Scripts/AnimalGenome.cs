using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class AnimalGenome {
    public float Speed = 1;

    [Tooltip("Fertility, from 0 to 0.5")]
    public float Fertility = 0.25f;
    public float Visibility = 1;
    public float EnergyEfficiency = 1;

    public AnimalGenome ChildGenome(AnimalGenome parent) {
        return new AnimalGenome() {
            Speed = getChildValue(Speed, parent.Speed),
            Fertility = getChildValue(Fertility, parent.Fertility),
            Visibility = getChildValue(Visibility, parent.Visibility),
            EnergyEfficiency = getChildValue(EnergyEfficiency, parent.EnergyEfficiency),
        };            
    }

    private float getChildValue(float parent1Value, float parent2Value) {
        return GaussianRandom.generateNormalRandom((parent1Value + parent2Value) / 2, Mathf.Abs(parent2Value - parent1Value) / 4);
    }
}
