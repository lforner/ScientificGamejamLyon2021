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
    public int RandomWalkHalfAngle = 30;

    public AnimalSpeciesType SpeciesType;
    private AnimalSpecies _species;   

    public float Energy = 1.0f;

    public AnimalGenome Genome = new AnimalGenome();
    public AnimalGenome ChildGenome = new AnimalGenome();

    public ColliderTriggerHelper BodyCollider;
    public ColliderTriggerHelper ViewCollider;

    private SpriteRenderer _emoticon;
    public Sprite EmoticonWalk;
    public Sprite EmoticonWalkPredator;
    public Sprite EmoticonWalkPrey;
    public Sprite EmoticonWalkMate;
    public Sprite EmoticonEatPrey;
    public Sprite EmoticonMakingLove;

    private AnimalState _state = AnimalState.None;
    public AnimalState State {
        get => _state;
        set {
            _state = value;
            switch (value) {
                //case AnimalState.Walk:
                //    _emoticon.sprite = EmoticonWalk;
                //    break;
                case AnimalState.WalkPredator:
                    _emoticon.sprite = EmoticonWalkPredator;
                    break;
                case AnimalState.WalkPrey:
                    _emoticon.sprite = EmoticonWalkPrey;
                    break;
                case AnimalState.WalkMate:
                    _emoticon.sprite = EmoticonWalkMate;
                    break;
                case AnimalState.EatPrey:
                    _emoticon.sprite = EmoticonEatPrey;
                    break;
                case AnimalState.MakingLove:
                    _emoticon.sprite = EmoticonMakingLove;
                    break;
                default:
                    _emoticon.sprite = null;
                    break;
            }
        }
    }

    private AudioSource _audioSource;
    public AudioClip AudioDie;
    public AudioClip AudioLove;
    public AudioClip AudioEat;

    [Header("Readonly")]
    public float DebugData = 0;
    public int InViewCount = 0;
    public int CollidingWithCount = 0;
    public bool IsDying = false;

    private NavMeshAgent _navMeshAgent;

    public bool IsHungry => Energy <= FoodEnergy;
    public bool CanMakeLove => Energy > LoveEnergyConsumption;

    // Start is called before the first frame update
    void Start() {
        _species = speciesMap[SpeciesType];
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _emoticon = GetComponentInChildren<SpriteRenderer>();
        _audioSource = GetComponentInChildren<AudioSource>();
    }

    // Update is called once per frame
    void Update() {

    }

    private void FixedUpdate() {
        if (IsDying) return;

        // Move
        MoveToward(GetNextMove());

        // Death
        if (Energy < 0) {
            Die(SpeciesType);
        }
    }

    private Vector3? GetNextMove() {
        // Has object in sight
        InViewCount = ViewCollider.CollidingWith.Count;
        CollidingWithCount = BodyCollider.CollidingWith.Count;
        if (InViewCount > 0) {
            // Check predator
            var predators = ViewCollider.CollidingWith.Where((animal) => animal._species.IsPredatorOf(_species) && !animal.IsDying);
            if (predators.Count() > 0) {
                foreach (var predator in predators) {
                    // Search for a predator in sight
                    var runDirection = GetPredatorInViewRunDirection(predator);
                    if (runDirection != null) {
                        // Move away
                        State = AnimalState.WalkPredator;
                        return runDirection;
                    }
                }
            }

            // Check prey
            var firstPrey = ViewCollider.CollidingWith.FirstOrDefault((animal) => _species.IsPredatorOf(animal._species) && !animal.IsDying);
            if (IsHungry && firstPrey != null) {
                var firstTouchingPrey = BodyCollider.CollidingWith.FirstOrDefault((animal) => _species.IsPredatorOf(animal._species) && !animal.IsDying);

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
            var firstMate = ViewCollider.CollidingWith.FirstOrDefault((animal) => SpeciesType == animal.SpeciesType && !animal.IsDying);
            if (CanMakeLove && firstMate != null) {
                var firstTouchingMate = BodyCollider.CollidingWith.FirstOrDefault((animal) => SpeciesType == animal.SpeciesType && !animal.IsDying);

                // Just Walk toward mate
                if (firstTouchingMate == null) {
                    State = AnimalState.WalkMate;
                    return firstMate.transform.position - transform.position;
                } 
                
                // Make love
                else {
                    var coupleFertility = Genome.Fertility.Value + firstMate.Genome.Fertility.Value;
                    if (coupleFertility >= Random.value) {
                        State = AnimalState.MakingLove;
                        MakeLove(firstMate);
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

        // Normalize direction
        var moveDirection = direction.GetValueOrDefault();

        // Check boundaries
        if (_navMeshAgent.isOnNavMesh) {
            NavMeshHit hit;
            if (_navMeshAgent.FindClosestEdge(out hit)) {
                DebugData = hit.distance;
                // If it hits a border, bounce
                if (hit.distance <= 0) {
                    // Move toward center
                    moveDirection = -transform.position;

                    // Bounce like a mirror
                    /*var angleToBorder = Vector3.Angle(transform.forward, hit.normal) - 90;
                    moveDirection = Quaternion.Euler(0, angleToBorder * 2, 0) * transform.forward;*/
                }
            }
        } else {
            Debug.LogWarning($"Animal not on nav mesh : {transform.position}");
        }

        // Move
        var move = moveDirection.normalized * Genome.Speed.Value;
        //transform.position += move;    
        _navMeshAgent.SetDestination(transform.position + move);
        //transform.forward = moveDirection;

        // Energy
        Energy -= WalkEnergyConsumption / Genome.EnergyEfficiency.Value;
    }

    private Vector3? GetPredatorInViewRunDirection(AnimalBehaviour predator) {
        var predatorDirection = predator.transform.position - transform.position;
        if (predatorDirection == Vector3.zero) predatorDirection = Random.insideUnitSphere;
        var angle = Vector3.Angle(transform.forward, predatorDirection);
        if (angle <= Genome.HalfFieldOfView) return -predatorDirection;
        return null;
    }

    private void EatPrey(AnimalBehaviour prey) {
        prey.Die(prey.SpeciesType);
        PlayAudio(AudioEat);
        Energy += FoodEnergy;
    }

    private void MakeLove(AnimalBehaviour parent2) {
        // Get child genome
        var childGenome = ChildGenome.ChildGenome(parent2.ChildGenome);

        // Create child
        var child = Instantiate(gameObject, (transform.position + parent2.transform.position) / 2, Quaternion.Lerp(transform.rotation, parent2.transform.rotation, 0.5f), transform.parent);
        //Debug.Log($"Child : {child.transform.position} | {IsDying} | {parent2.IsDying}");
        var animal = child.GetComponent<AnimalBehaviour>();
        animal.Genome = childGenome;
        animal.Energy = LoveEnergyConsumption;

        AnimalsCountUI.S.IncrementsAnimalsCount(animal.SpeciesType, 1);

        var viewCollider = child.GetComponentsInChildren<ColliderTriggerHelper>().FirstOrDefault((c) => c.name == ColliderTriggerHelper.ViewName);
        viewCollider.transform.localScale = Vector3.one * childGenome.ViewDistance;

        PlayAudio(AudioLove);

        // AfterLoveTask
        AfterLoveTask(this);
        AfterLoveTask(parent2);
    }

    private void AfterLoveTask(AnimalBehaviour lover) {
        lover.Energy -= LoveEnergyConsumption / Genome.EnergyEfficiency.Value;
    }

    private void Die(AnimalSpeciesType speciesType) {
        IsDying = true;
        _navMeshAgent.enabled = false;
        transform.position = new Vector3(0, 1000, 0);
        PlayAudio(AudioDie);
        AnimalsCountUI.S.IncrementsAnimalsCount(speciesType, -1);
        Destroy(gameObject, 1);
    }

    private void PlayAudio(AudioClip clip) {
        _audioSource.clip = clip;
        _audioSource.Play();
    }
}
