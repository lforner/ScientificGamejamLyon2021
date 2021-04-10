using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct AnimalSpecies {
    public AnimalSpeciesType MateType;
    public AnimalSpeciesType PredatorType;
    public AnimalSpeciesType PreyType;

    public AnimalSpecies(AnimalSpeciesType type, AnimalSpeciesType predator, AnimalSpeciesType prey) {
        MateType = type;
        PredatorType = predator;
        PreyType = prey;
    }

    public bool IsPredatorOf(AnimalSpeciesType other) => PredatorType == other;
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

    public static float FoodStamina = 0.5f;

    public AnimalSpecies Species;
    public float Energy = 1.0f;

    public AnimalGenome Genome;
    public AnimalGenome ChildGenome;

    public ColliderTriggerHelper BodyCollider;
    public ColliderTriggerHelper ViewCollider;

    public bool IsHungry => Energy <= FoodStamina;

    // Start is called before the first frame update
    void Start() {
        Time.timeScale = 0.1f;
    }

    // Update is called once per frame
    void Update() {

    }

    private void FixedUpdate() {
        // Check environment
        /*if (ViewCollider.CollidingWith != null) {
            var collidingWith = ViewCollider.CollidingWith.gameObject.GetComponent<AnimalBehaviour>();

            // Check predator
            if (collidingWith.Species.IsPredatorOf(Species.MateType)) {
                // Check FOV
                var predatorDirection = ViewCollider.CollidingWith.transform.position - transform.position;
                var angle = Vector3.Angle(transform.forward, predatorDirection);
                if (angle <= 80) {
                    // Move away
                    MoveToward(-predatorDirection);
                }
            }

            // Check food
            if (IsHungry && Species.IsPredatorOf(collidingWith.Species.MateType)) { 

            }

            // Check mate


        }*/


        // New position
        var direction = Random.insideUnitCircle;
        MoveToward(new Vector3(direction.x, 0, direction.y));


        //Time.timeScale
        //Debug.Log(Time.fixedDeltaTime);
    }

    private void MoveToward(Vector3 direction) {
        var move = direction.normalized * Genome.Speed;
        transform.position += move;
        transform.forward = move;
    }
}
