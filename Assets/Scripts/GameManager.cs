using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;
    private FishSpawner _fishSpawner;
    private float _waterLevel;
    
    [SerializeField] private GameObject mainCamera;
    [SerializeField] private CountdownTimer countdownTimer;
    
    public CountdownTimer CountdownTimer => countdownTimer; 
    public FishSpawner FishSpawner => _fishSpawner; 
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
        
        /*if (countdownTimers.Length > 0)
        {
            foreach (CountdownTimerContainer timer in countdownTimers)
            {
                timer.countdownTimer = gameObject.AddComponent<CountdownTimer>();
                timer.countdownTimer.SetTimerDuration(timer.timerDuration);
                timer.countdownTimer.SetTimerText(timerText);
            }
        }*/
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
    }
}
