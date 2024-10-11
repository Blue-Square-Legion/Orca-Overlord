using System.Collections;
using UnityEngine;


/// <summary>
/// 
/// </summary>
public class Spear : MonoBehaviour
{
    public GameObject player;
    public GameObject enemy;
    public Transform spearSpawnPt;
    public GameObject spearPrefab;
    public float spearSpeed = 100;
    private float duration = 200;
    private float speed = .02f;
    private void Start()
    {
        enemy.transform.LookAt(player.transform.position);
       
    }

           
        
    
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            GameObject spear = Instantiate(spearPrefab, spearSpawnPt.position, spearSpawnPt.rotation);
            spear.GetComponent<Rigidbody>().velocity = spearSpawnPt.forward * spearSpeed;
        }
    enemy.transform.position = Vector3.MoveTowards(enemy.transform.position, player.transform.position, speed);
}
}
    