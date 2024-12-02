using TMPro;
using UnityEngine;
using UnityEngine.Rendering;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;

    private PlayerController _playerController;
    private FishSpawner _fishSpawner;
    private DolphinSpawner _dolphinSpawner;
    private float _waterLevel;
    
    [SerializeField] private CountdownTimer countdownTimer;
    [SerializeField] private GameObject mainCamera;

    public CountdownTimer CountdownTimer => countdownTimer;
    public FishSpawner FishSpawner => _fishSpawner;
    public DolphinSpawner DolphinSpawner => _dolphinSpawner;
    public float WaterLevel => _waterLevel;
    
    public static GameManager Instance
    {
        get
        {
            if (_instance == null)
            {
                Debug.LogError("Game Manager Instance is Null!");
            }
            
            return _instance;
        }
    }
    

    private void Awake()
    {
        _instance = this;

        if (!GameObject.FindGameObjectWithTag("Player").TryGetComponent(out _playerController))
        {
            Debug.LogError("Player Controller Component not found!");
        }
        
        GameObject water = GameObject.FindGameObjectWithTag("Water");
        if (water)
        {
            _waterLevel = water.transform.position.y;
        }
        else
        {
            Debug.LogError("Water not found.");
        }

        if (!GameObject.FindGameObjectWithTag("FishSpawner").TryGetComponent(out _fishSpawner))
        {
            Debug.LogError("Fish Manager is Missing.");
        }
        
        if (!GameObject.FindGameObjectWithTag("DolphinSpawner").TryGetComponent(out _dolphinSpawner))
        {
            Debug.LogError("Dolphin Spawner is Missing.");
        }
    }

    private void Update()
    {
        
        
        if (_playerController.IsInWater)
        {
            mainCamera.GetComponent<Volume>().enabled = true;
        }
        
        if(!_playerController.IsInWater || _playerController.IsAtSurface)
        {
            mainCamera.GetComponent<Volume>().enabled = false;
        }
    }
}
