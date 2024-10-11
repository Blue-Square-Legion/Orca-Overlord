using UnityEngine;

public class Dolphin : MonoBehaviour
{
    public Transform island;
    public float speed;
    public bool isConnected;
   
    void Update()
    {
        if (isConnected)
        {
            this.transform.position -= (Vector3.MoveTowards(this.transform.position, island.position, 5f) * speed * Time.deltaTime)
                / Vector3.Distance(transform.position,island.transform.position);
        }
    }

}
