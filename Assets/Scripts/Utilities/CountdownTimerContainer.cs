using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Container Class for Timer Information.
/// </summary>
[CreateAssetMenu(fileName = "NewCountdownTimerContainer", menuName = "CountdownTimerContainer")]
public class CountdownTimerContainer : ScriptableObject
{
    public string timerName;
    public float timerDuration;
    [HideInInspector] public CountdownTimer countdownTimer;
}
