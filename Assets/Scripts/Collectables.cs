using UnityEngine;


/// <summary>
/// 
/// </summary>
public class Collectables : MonoBehaviour
{
    /// <summary>
    /// Start is called before the first frame update
    /// </summary>
    void Start()
    {
        
    }
    public void OnTriggerEnter(Collider other)
    {
        Destroy(gameObject);
    }
    /// <summary>
    /// Update is called once per frame
    /// </summary>
    void Update()
    {
        
    }
}
