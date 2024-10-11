using UnityEngine;


/// <summary>
/// 
/// </summary>
public class EnemyAI : MonoBehaviour
{
    public GameObject enemy;
    public GameObject player;
    public float speed =.2f;
    /// <summary>
    /// Start is called before the first frame update
    /// </summary>
    void Start()
    {
        
    }

    /// <summary>
    /// Update is called once per frame
    /// </summary>
    void Update()
    {
        player.transform.LookAt(enemy.transform);
        enemy.transform.position = Vector3.MoveTowards(enemy.transform.position, player.transform.position, speed);
    }
}
