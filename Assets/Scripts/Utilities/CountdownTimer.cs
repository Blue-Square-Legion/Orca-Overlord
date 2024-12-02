using System;
using TMPro;
using UnityEngine;


/// <summary>
/// Countdown Timer Implementation.
/// </summary>

public class CountdownTimer : MonoBehaviour
{
    private bool _isCountingDown = false;
    private float _timeRemaining;
    
    [SerializeField] private float timerDuration;
    [SerializeField] private TextMeshProUGUI timerText;

    public bool IsCountingDown => _isCountingDown;
    
    /// <summary>
    /// Update is called once per frame
    /// </summary>
    void Update()
    {
        if (_isCountingDown)
        {
            Countdown();
        }
    }

    public void StartTimer()
    {
        _isCountingDown = true;
        _timeRemaining = timerDuration;
    }

    void Countdown()
    {
        timerText.text = (_timeRemaining).ToString("F2");
        _timeRemaining -= Time.deltaTime;
        
        if (_timeRemaining <= 0.0f)
        {
            TimerEnd();
        }
    }
    
    void TimerEnd()
    {
        _isCountingDown = false;
        Debug.Log("Timer has ended.");
    }

    public void SetTimerDuration(float timerDuration)
    {
       this.timerDuration = timerDuration;
    }

    public void SetTimerText(TextMeshProUGUI timerText)
    {
        this.timerText = timerText;
    }
}
