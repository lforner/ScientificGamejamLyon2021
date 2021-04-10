using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimalSpawner : MonoBehaviour
{
    public GameObject AnimalPrefab;
    public int AnimalsCount;
    public float SpawnRadius;

    private List<GameObject> _animals = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < AnimalsCount; i++)
        {
            Vector2 randomPosition = Random.insideUnitCircle * SpawnRadius;
            GameObject animal = Instantiate(AnimalPrefab, transform);
            animal.transform.localPosition = new Vector3(randomPosition.x, animal.transform.position.y, randomPosition.y);
            _animals.Add(animal);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
