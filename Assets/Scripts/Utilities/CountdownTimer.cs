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
    private float _timerDuration;
    private TextMeshProUGUI _timerText;

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
        _timeRemaining = _timerDuration;
    }

    void Countdown()
    {
        _timerText.text = (_timeRemaining).ToString("F2");
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
       _timerDuration = timerDuration;
    }

    public void SetTimerText(TextMeshProUGUI timerText)
    {
        _timerText = timerText;
    }
}
