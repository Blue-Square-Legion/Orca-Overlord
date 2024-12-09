using TMPro;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI scoreText;

    private int _score;
    
    public static ScoreManager Instance;
    
    
    private void Awake()
    {
        if (!Instance)
        {
            Instance = this;
        }
    }

    public void AddScore(int score)
    {
        _score += score;
        scoreText.text = _score.ToString();
    }

    
    
    public void SubtractScore(int score)
    {
        _score -= score;
    }
    
    
    
    public void ResetScore()
    {
        _score = 0;
    }
}