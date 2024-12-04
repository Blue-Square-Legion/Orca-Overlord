using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

/// <summary>
/// Spawns Fish according to assigned prefab and properties.
/// </summary>
public class FishSpawner : MonoBehaviour
{
    [Header("Setup")]
    
    [Tooltip("Reference to Fish Prefab to create a school.")]
    public GameObject fishPrefab;
    
    [Tooltip("Reference to the spawn center game object. This acts as the center for the School of Fish System.")]
    public GameObject spawnPoint;
    
    [Tooltip("The range of randomness for the distance the fish will spawn from the SpawnPoint.")]
    public int fishSpawnOffset = 5;
    
    [Tooltip("Maximum radius for a school of fish.")]
    public int effectiveRadius = 30;
   
    [Tooltip("Downward vertical distance from the water surface the fish will spawn at.")]
    public int waterSurfaceOffset = 10;
    
    [Tooltip("Range of randomness for the GoalPosition.")]
    public int goalPositionRange = 20;
    
    [Tooltip("Number of fish in the school.")]
    public int numFish = 10;
    
    
    [HideInInspector] public GameObject[] allFish;
    [HideInInspector] public Vector3 goalPosition;

    private Vector3 _position;
    private float _maxHeight;
    private GameObject _spawnedFishHolder;
    
    
    private void Awake()
    {
        allFish = new GameObject[numFish];
        _spawnedFishHolder = new GameObject("Spawned Fish Holder");
    }

    // Start is called before the first frame update
    void Start()
    {
        _position = spawnPoint.transform.position;
        goalPosition = spawnPoint.transform.position;
        _maxHeight = (int) GameManager.Instance.WaterLevel - waterSurfaceOffset;
        
        for (int i = 0; i < numFish; i++)
        {
            _position.x += Random.Range(-fishSpawnOffset, fishSpawnOffset);
            _position.y += Random.Range(-fishSpawnOffset, fishSpawnOffset);
        
            if (_position.y > _maxHeight)
            {
                _position.y = _maxHeight;
            }

            if (_position.y < 0)
            {
                _position.y = 5;
            }
        
            _position.z += Random.Range(-fishSpawnOffset, fishSpawnOffset);
            
            allFish[i] = Instantiate(fishPrefab, _position, quaternion.identity);
            allFish[i].transform.parent = _spawnedFishHolder.transform;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Random.Range(0, 10000) < 50)
        {
            goalPosition.x += Random.Range(-goalPositionRange, goalPositionRange);
            goalPosition.y += Random.Range(-goalPositionRange, goalPositionRange);
            
            if (goalPosition.y > _maxHeight)
            {
                goalPosition.y = _maxHeight;
            }

            if (goalPosition.y < 0)
            {
                goalPosition.y = 5;
            }
            goalPosition.z += Random.Range(-goalPositionRange, goalPositionRange);
            
            
        }
        
        spawnPoint.transform.position = goalPosition;
    }
    
    
}
