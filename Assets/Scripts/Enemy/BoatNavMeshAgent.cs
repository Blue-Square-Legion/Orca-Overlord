using System;
using UnityEngine;
using UnityEngine.AI;

public class BoatNavMeshAgent : MonoBehaviour
{
    private NavMeshAgent _agent;
    public Transform goal;
    
    private void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();
    }

    private void Start()
    {
        _agent.SetDestination(goal.position);
    }
}
