using System.Collections;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;
using System.Timers;
using System;
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
    private float time = 200000f;
    private float time2 = 2f;
    bool _On = true;
    GameObject spear;
    int x = 10;
    float count1 = 0f;
    private void Start()
    {
        enemy.transform.LookAt(player.transform.position);

    }
    public void ShootMe()
    {
        enemy.transform.LookAt(player.transform.position);

        spear = Instantiate(spearPrefab, spearSpawnPt.position, spearSpawnPt.rotation);

        spear.GetComponent<Rigidbody>();
        spear.GetComponent<Rigidbody>().velocity = spearSpawnPt.forward * spearSpeed;
        _On = false;
        Destroy( spear ,4);
    }

    private void FixedUpdate()
    {
        if (time % 2 < .01 && _On)

        {

            Invoke(nameof(ShootMe), 4);

        }
        _On = true;
        time -= Time.deltaTime;

        enemy.transform.position = Vector3.MoveTowards(enemy.transform.position, player.transform.position, speed);
    }
}
