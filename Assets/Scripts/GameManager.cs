using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;
    private static PlayerManager _playerManager;

    public static PlayerManager PlayerManager => _playerManager;
    
    
    [SerializeField] private CountdownTimerContainer[] countdownTimers;

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

        if (!TryGetComponent<PlayerManager>(out  _playerManager))
        {
            Debug.LogError("Player Manager not found.");
        }

        if (!TryGetComponent<FishSchoolManager>(out FishSchoolManager fishSchoolManager))
        {
            Debug.LogError("Fish School Manager is Missing.");
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
