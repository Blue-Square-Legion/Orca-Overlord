using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;
    private static PlayerManager _playerManager;
    private static FishManager _fishManager;
    
    [SerializeField] private CountdownTimerContainer[] countdownTimers;
    
    public static PlayerManager PlayerManager => _playerManager;
    public static FishManager FishManager => _fishManager;

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

        if (!GameObject.FindGameObjectWithTag("Player").TryGetComponent(out _playerManager))
        {
            Debug.LogError("Player Manager not Found.");
        }

        if (!GameObject.FindGameObjectWithTag("FishManager").TryGetComponent(out _fishManager))
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
        if (countdownTimers.Length> 0 && !countdownTimers[0].countdownTimer.IsCountingDown)
        {
            countdownTimers[0].countdownTimer.StartTimer();
        }
    }
}
