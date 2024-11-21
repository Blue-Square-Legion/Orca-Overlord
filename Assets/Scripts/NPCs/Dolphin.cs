using System;
using UnityEngine;

public class Dolphin : MonoBehaviour
{
    [SerializeField] private GameObject player;
    [SerializeField] private float moveSpeed;
    [SerializeField] private float distanceThreshold;
    
    private void Update()
    {
        transform.LookAt(player.transform);
        
        float distance = Vector3.Distance(transform.position, player.transform.position);

        // Check if the object should move towards the player
        if (distance > distanceThreshold)
        {
            // Move towards the player's position
            Vector3 direction = (player.transform.position - transform.position).normalized;
            transform.position += direction * moveSpeed * Time.deltaTime;
        }
    }
}
