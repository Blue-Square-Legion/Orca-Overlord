using System;
using UnityEngine;

public class Dolphin : MonoBehaviour
{
    [SerializeField] private float moveSpeed;
    [SerializeField] private float distanceThreshold;

    private GameObject _player;
    private void Awake()
    {
        _player = GameObject.FindGameObjectWithTag("Player");
        if (!_player)
        {
            Debug.LogError("Player Not Found!");
        }
    }

    private void Update()
    {
        transform.LookAt(_player.transform);
        
        float distance = Vector3.Distance(transform.position, _player.transform.position);

        // Check if the object should move towards the player
        if (distance > distanceThreshold)
        {
            // Move towards the player's position
            Vector3 direction = (_player.transform.position - transform.position).normalized;
            transform.position += direction * moveSpeed * Time.deltaTime;
        }
    }
}
