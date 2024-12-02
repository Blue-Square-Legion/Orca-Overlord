using TMPro;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;


/// <summary>
/// 
/// </summary>
public class TimerScore : MonoBehaviour
{
    public GameObject motorBoatPrefab;
    public GameObject patrolBoatPrefab;
    public GameObject fishingBoatPrefab;
    float score = 0f;
    bool active = true;
    bool patrol = true;
    bool motor = true;
    bool fishing = true;
    [SerializeField] TextMeshProUGUI scoreText;
    /// <summary>
    /// Start is called before the first frame update
    /// </summary>
    void Start()
    {
        score = 0f;
    }

    /// <summary>
    /// Update is called once per frame
    /// </summary>
    void FixedUpdate()
    {
        if (motorBoatPrefab.transform.rotation.z != 0 && patrol)
        {
            score += 5f;
            patrol = false;
        }
        
        if (patrolBoatPrefab.transform.rotation.z != 0 && motor)
        {
           score += 10f;
           motor=false;  
        } 
       
        if (fishingBoatPrefab.transform.rotation.z != 0 && fishing) 
        {
            score += 15f;
            fishing=false;  
        }
       
        scoreText.text = Mathf.RoundToInt(score).ToString();

        if (!GameManager.Instance.CountdownTimer.IsCountingDown)
        { 
           GameManager.Instance.CountdownTimer.StartTimer();
        }
    }
}
