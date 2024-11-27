using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

/// <summary>
/// Spawns Dolphins according to assigned prefab and properties.
/// </summary>
public class DolphinSpawner : MonoBehaviour
{
    [Header("Setup")]
    
    [Tooltip("Reference to Dolphin Prefab to create a school.")]
    public GameObject dolphinPrefab;
    
    [Tooltip("Reference to the spawn center game object. This acts as the center for the School of Dolphin System.")]
    public GameObject spawnPoint;
    
    [Tooltip("The range of randomness for the distance the dolphins will spawn from the SpawnPoint.")]
    public int dolphinSpawnOffset = 5;
    
    [Tooltip("Maximum radius for a pod of dolphins.")]
    public int effectiveRadius = 30;
   
    [Tooltip("Downward vertical distance from the water surface the dolphins will spawn at.")]
    public int waterSurfaceOffset = 10;
    
    [Tooltip("Range of randomness for the GoalPosition.")]
    public int goalPositionRange = 20;
    
    [FormerlySerializedAs("numFish")] [Tooltip("Number of dolphins in the pod.")]
    public int numDolphins = 10;
    
    
    [HideInInspector] public GameObject[] allDolphins;
    [HideInInspector] public Vector3 goalPosition;

    private Vector3 _position;
    private float _maxHeight;
    
    
    private void Awake()
    {
        allDolphins = new GameObject[numDolphins];
    }

    // Start is called before the first frame update
    void Start()
    {
        _position = spawnPoint.transform.position;
        goalPosition = spawnPoint.transform.position;
        _maxHeight = (int) GameManager.Instance.WaterLevel - waterSurfaceOffset;
        
        for (int i = 0; i < numDolphins; i++)
        {
            _position.x += Random.Range(-dolphinSpawnOffset, dolphinSpawnOffset);
            _position.y += Random.Range(-dolphinSpawnOffset, dolphinSpawnOffset);
            
            if (_position.y > _maxHeight)
            {
                _position.y = _maxHeight;
            }

            if (_position.y < 0)
            {
                _position.y = 5;
            }
        
            _position.z += Random.Range(-dolphinSpawnOffset, dolphinSpawnOffset);
            
            allDolphins[i] = Instantiate(dolphinPrefab, _position, quaternion.identity);
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
