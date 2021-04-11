using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

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

public enum AnimalState {
    None,
    Walk,
    WalkPredator,
    WalkPrey,
    WalkMate,
    EatPrey,
    MakingLove,
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

    [Header("Readonly")]
    public int InViewCount = 0;
    public int CollidingWithCount = 0;
    public AnimalState State = AnimalState.None;

    public bool IsHungry => Energy <= FoodEnergy;
    public bool CanMakeLove => Energy > LoveEnergyConsumption;

    // Start is called before the first frame update
    void Start() {
        _species = speciesMap[SpeciesType];
    }

    // Update is called once per frame
    void Update() {

    }

    private void FixedUpdate() {
        // Move
        MoveToward(GetNextMove());

        // Death
        if (Energy < 0) {
            Die();
        }
    }

    private Vector3? GetNextMove() {
        // Has object in sight
        InViewCount = ViewCollider.CollidingWith.Count;
        CollidingWithCount = BodyCollider.CollidingWith.Count;
        if (InViewCount > 0) {
            // Check predator
            var firstPredator = ViewCollider.CollidingWith.FirstOrDefault((animal) => animal._species.IsPredatorOf(_species));
            if (firstPredator != null) {
                // Check FOV
                var predatorDirection = firstPredator.transform.position - transform.position;
                var angle = Vector3.Angle(transform.forward, predatorDirection);
                if (angle <= HalfFieldOfView) {     // TODO should do this for all CollidingWith
                    // Move away
                    State = AnimalState.WalkPredator;
                    return -predatorDirection;
                }
            }

            // Check prey
            var firstPrey = ViewCollider.CollidingWith.FirstOrDefault((animal) => _species.IsPredatorOf(animal._species));
            if (IsHungry && firstPrey != null) {
                var firstTouchingPrey = BodyCollider.CollidingWith.FirstOrDefault((animal) => _species.IsPredatorOf(animal._species));

                // Just Walk toward prey
                if (firstTouchingPrey == null) {
                    State = AnimalState.WalkPrey;
                    return firstPrey.transform.position - transform.position;
                } 
                
                // Eat prey
                else {
                    State = AnimalState.EatPrey;
                    EatPrey(firstTouchingPrey);
                    return null;
                }

            }

            // Check mate
            var firstMate = ViewCollider.CollidingWith.FirstOrDefault((animal) => SpeciesType == animal.SpeciesType);
            if (CanMakeLove && firstMate != null) {
                var firstTouchingMate = BodyCollider.CollidingWith.FirstOrDefault((animal) => SpeciesType == animal.SpeciesType);

                // Just Walk toward mate
                if (firstTouchingMate == null) {
                    State = AnimalState.WalkMate;
                    return firstMate.transform.position - transform.position;
                } 
                
                // Make love
                else {
                    var coupleFertility = Genome.Fertility + firstMate.Genome.Fertility;
                    if (coupleFertility >= Random.value) {
                        State = AnimalState.MakingLove;
                        MakeLove(firstMate);        // TODO only one of them should make love
                        return null;
                    }
                }
            }
        }

        // Nothing in sight
        State = AnimalState.Walk;
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

    private void EatPrey(AnimalBehaviour prey) {
        prey.Die();
        Energy += FoodEnergy;
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

    private void Die() {
        GetComponent<NavMeshAgent>().enabled = false;
        transform.position = new Vector3(0, 1000, 0);
        Destroy(gameObject, 1);
    }
}
