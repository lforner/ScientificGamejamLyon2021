using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct AnimalSpecies {
    public AnimalSpeciesType Type;
    public AnimalSpeciesType PredatorType;
    public AnimalSpeciesType PreyType;

    public AnimalSpecies(AnimalSpeciesType type, AnimalSpeciesType predator, AnimalSpeciesType prey) {
        Type = type;
        PredatorType = predator;
        PreyType = prey;
    }

    public bool IsPredatorOf(AnimalSpecies other) => other.PredatorType == Type;
}

public enum AnimalSpeciesType {
    Rock,
    Paper,
    Cisor,
}

public class AnimalBehaviour : MonoBehaviour {
    public static Dictionary<AnimalSpeciesType, AnimalSpecies> speciesMap = new Dictionary<AnimalSpeciesType, AnimalSpecies>() {
        { AnimalSpeciesType.Rock, new AnimalSpecies(AnimalSpeciesType.Rock, AnimalSpeciesType.Paper, AnimalSpeciesType.Cisor) },
        { AnimalSpeciesType.Paper, new AnimalSpecies(AnimalSpeciesType.Paper, AnimalSpeciesType.Cisor, AnimalSpeciesType.Rock) },
        { AnimalSpeciesType.Cisor, new AnimalSpecies(AnimalSpeciesType.Cisor, AnimalSpeciesType.Rock, AnimalSpeciesType.Paper) }
    };
    public static float FoodEnergy = 0.5f;
    public static float WalkEnergyConsumption = 0.001f;
    public static float LoveEnergyConsumption = 0.6f;

    public AnimalSpeciesType SpeciesType;
    private AnimalSpecies _species;

    public float Energy = 1.0f;

    public AnimalGenome Genome = new AnimalGenome();
    public AnimalGenome ChildGenome = new AnimalGenome();

    public ColliderTriggerHelper BodyCollider;
    public ColliderTriggerHelper ViewCollider;

    public AnimalBehaviour() {
        _species = speciesMap[SpeciesType];
    }

    public bool IsHungry => Energy <= FoodEnergy;
    public bool CanMakeLove => Energy >= LoveEnergyConsumption;

    // Start is called before the first frame update
    void Start() {
        //Time.timeScale = 0.1f;
    }

    // Update is called once per frame
    void Update() {

    }

    private void FixedUpdate() {
        // Move
        MoveToward(GetNextMove());

        // Death
        if (Energy < 0) {
            Destroy(gameObject);
        }
    }

    private Vector3? GetNextMove() {
        // Has object in sight
        if (ViewCollider.CollidingWith != null) {
            var collidingWith = ViewCollider.CollidingWith.gameObject.GetComponentInParent<AnimalBehaviour>();

            // Check predator
            if (collidingWith._species.IsPredatorOf(_species)) {
                Debug.Log("Predator");
                // Check FOV
                var predatorDirection = ViewCollider.CollidingWith.transform.position - transform.position;
                var angle = Vector3.Angle(transform.forward, predatorDirection);
                if (angle <= 80) {
                    // Move away
                    return -predatorDirection;
                }
            }

            // Check food
            if (IsHungry && _species.IsPredatorOf(collidingWith._species)) {
                Debug.Log("Eat");
            }

            // Check mate
            if (CanMakeLove && SpeciesType == collidingWith.SpeciesType) {
                Debug.Log("Love");
                var coupleFertility = Genome.Fertility + collidingWith.Genome.Fertility;
                if (coupleFertility >= Random.value) {
                    MakeLove();
                    return null;
                }
            }
        }

        // Nothing in sight  
        var rotation = Quaternion.Euler(0, Random.value * 30 - 15, 0);
        return rotation * transform.forward;
    }

    private void MoveToward(Vector3? direction) {
        if (direction == null) return;

        // Move
        var move = direction.GetValueOrDefault().normalized * Genome.Speed;
        transform.position += move;
        transform.forward = move;

        // Energy
        Energy -= WalkEnergyConsumption * Genome.EnergyEfficiency;
    }

    private void MakeLove() {
        var childGenome = new AnimalGenome();

    }
}
