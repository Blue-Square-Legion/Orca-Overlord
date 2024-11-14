using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;
    private static PlayerManager _playerManager;
    private static FishSpawner _fishSpawner;

    private float _waterLevel;
    
    [SerializeField] private CountdownTimerContainer[] countdownTimers;
    [SerializeField] private GameObject mainCamera;
    
    public static PlayerManager PlayerManager => _playerManager;
    public static FishSpawner FishSpawner => _fishSpawner;
    public float WaterLevel => _waterLevel;
    
    [Header("UI Components")] 
    [SerializeField]public TextMeshProUGUI timerText;
    
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

        GameObject water = GameObject.FindGameObjectWithTag("Water");
        if (water)
        {
            _waterLevel = water.transform.position.y;
        }
        else
        {
            Debug.LogError("Water not found.");
        }
        
        if (!GameObject.FindGameObjectWithTag("Player").TryGetComponent(out _playerManager))
        {
            Debug.LogError("Player Manager not Found.");
        }

        if (!GameObject.FindGameObjectWithTag("FishSpawner").TryGetComponent(out _fishSpawner))
        {
            Debug.LogError("Fish Manager is Missing.");
        }
        
        if (countdownTimers.Length > 0)
        {
            foreach (CountdownTimerContainer timer in countdownTimers)
            {
                timer.countdownTimer = gameObject.AddComponent<CountdownTimer>();
                timer.countdownTimer.SetTimerDuration(timer.timerDuration);
                timer.countdownTimer.SetTimerText(timerText);
            }
        }
    }

    private void Update()
    {
        if (PlayerManager.IsPlayerInWater())
        {
            mainCamera.GetComponent<Volume>().enabled = true;
        }
        
        if(!PlayerManager.IsPlayerInWater() || PlayerManager.IsPlayerAtSurface())
        {
            mainCamera.GetComponent<Volume>().enabled = false;
        }
        
        if (countdownTimers.Length> 0 && !countdownTimers[0].countdownTimer.IsCountingDown)
        {
            countdownTimers[0].countdownTimer.StartTimer();
        }
    }
}
