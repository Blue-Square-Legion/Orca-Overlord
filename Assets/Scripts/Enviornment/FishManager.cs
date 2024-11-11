using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

public class FishManager : MonoBehaviour
{
    [Tooltip("Reference to Fish Prefab to create a school.")]
    public GameObject fishPrefab;
    public int fishSpawnOffset = 5;
    public int effectiveRadius = 30;
    public int goalPositionOffset = 20;
    public int numFish = 10;
    [HideInInspector] public GameObject[] allFish;
    [HideInInspector] public Vector3 GoalPosition;

    private Vector3 _position;
    private GameObject _water;
    
    private void Awake()
    {
        allFish = new GameObject[numFish];
        _water = GameObject.FindGameObjectWithTag("Water");
    }

    // Start is called before the first frame update
    void Start()
    {
        _position = PlayerManager.GetPlayerPosition();
        GoalPosition = PlayerManager.GetPlayerPosition();
        
        for (int i = 0; i < numFish; i++)
        {
            RandomizePosition(ref _position, fishSpawnOffset);
            allFish[i] = Instantiate(fishPrefab, _position, quaternion.identity);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Random.Range(0, 10000) < 50)
        {
            RandomizePosition(ref GoalPosition, goalPositionOffset);
        }
    }

    private void RandomizePosition(ref Vector3 pos, int range)
    {
        pos.x += Random.Range(-range, range);
        pos.y += Random.Range(-range, range);
        pos.z += Random.Range(-range, range);
    }
}
