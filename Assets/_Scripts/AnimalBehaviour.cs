using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AnimalSpecies {
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

    public float FoodEnergy = 0.5f;
    public float WalkEnergyConsumption = 0.001f;
    public float LoveEnergyConsumption = 0.6f;
    public int HalfFieldOfView = 80;
    public int RandomWalkHalfAngle = 30;

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
    public bool CanMakeLove => Energy > LoveEnergyConsumption;

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
        if (ViewCollider.CollidingWith.Count > 0) {
            var firstPredator = ViewCollider.CollidingWith.FirstOrDefault((animal) => animal._species.IsPredatorOf(_species));
            // Check predator
            if (firstPredator != null) {
                Debug.Log("Predator");
                // Check FOV
                var predatorDirection = firstPredator.transform.position - transform.position;
                var angle = Vector3.Angle(transform.forward, predatorDirection);
                if (angle <= HalfFieldOfView) {
                    // Move away
                    return -predatorDirection;
                }
            }

            // Check food
            var firstPrey = ViewCollider.CollidingWith.FirstOrDefault((animal) => _species.IsPredatorOf(animal._species));
            if (IsHungry && firstPrey != null) {
                Debug.Log("Eat");
            }

            // Check mate
            var firstMate = ViewCollider.CollidingWith.FirstOrDefault((animal) => SpeciesType == animal.SpeciesType);
            if (CanMakeLove && firstMate != null) {
                if (BodyCollider.CollidingWith == null) {
                    return firstMate.transform.position - transform.position;
                } else { 
                    var coupleFertility = Genome.Fertility + firstMate.Genome.Fertility;
                    if (coupleFertility >= Random.value) {
                        MakeLove(firstMate);
                        return null;
                    }
                }
            }
        }

        // Nothing in sight  
        var rotation = Quaternion.Euler(0, Random.value * RandomWalkHalfAngle - RandomWalkHalfAngle / 2, 0);
        return rotation * transform.forward;
    }

    private void MoveToward(Vector3? direction) {
        if (direction == null) return;

        // Move
        var move = direction.GetValueOrDefault().normalized * Genome.Speed;
        transform.position += move;
        //transform.DOMove(transform.position + move, Time.fixedDeltaTime).SetEase(Ease.Linear);
        transform.forward = move;

        // Energy
        Energy -= WalkEnergyConsumption * Genome.EnergyEfficiency;
    }

    private void MakeLove(AnimalBehaviour parent2) {
        var childGenome = ChildGenome.ChildGenome(parent2.ChildGenome);
        var child = Instantiate(gameObject, (transform.position + parent2.transform.position) / 2, Quaternion.Lerp(transform.rotation, parent2.transform.rotation, 0.5f), transform.parent);
        var animal = child.GetComponent<AnimalBehaviour>();
        animal.Genome = childGenome;
        animal.Energy = LoveEnergyConsumption;
        AfterLoveTask(this);
        AfterLoveTask(parent2);
    }

    private void AfterLoveTask(AnimalBehaviour lover) {
        lover.Energy -= LoveEnergyConsumption;
    }
}
