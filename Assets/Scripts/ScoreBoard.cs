using TMPro;
using UnityEngine;


/// <summary>
/// 
/// </summary>
public class ScoreBoard : MonoBehaviour
{
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI timerText;
    public GameObject patrolBoatPrefab;
    public GameObject motorBoatPrefab;
    public GameObject fishingBoatPrefab;
    private float score = 0;
    private float maxPatroly = 57;
    private float maxMotory = 49;
    private float maxFishingy = 42;
    private float timer = 100f;
    bool _five=false;
    bool _ten=false;
    bool _fifteen = false;
    /// <summary>
    /// Start is called before the first frame update
    /// </summary>
    void Start()
    {
       // patrolBoatPrefab = GetComponent<GameObject>();
      //  motorBoatPrefab= GetComponent<GameObject>();
      //  fishingBoatPrefab=GetComponent<GameObject>();
        score = 0;
     
    }

    /// <summary>
    /// Update is called once per frame
    /// </summary>p
    /// 
    public void checkOne()
    {
        if (patrolBoatPrefab.transform.rotation.z != 0 && !_ten)
        {
            score += 10;
            _ten = true;
        }
        if (motorBoatPrefab.transform.rotation.z != 0 && !_five)
        {
            score += 5;
            _five = true;
        }
        if (fishingBoatPrefab.transform.rotation.z != 0 && !_fifteen)
        {
            score += 15;
            _fifteen = true;
        }

      
      
    }
    void FixedUpdate()
    {
      

        if (timer > 0)
            timer-=Time.deltaTime;
        checkOne();
        timerText.text = timer.ToString();//$"{timer:02}";
        scoreText.text = Mathf.RoundToInt(score).ToString();
    }
}
