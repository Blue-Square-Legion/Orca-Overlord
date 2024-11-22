using System.Collections;
using System.Threading;
using UnityEngine;

public class Attack : State
{
    private GameObject _thisEnemy;
    private GameObject _player;
    private CharacterController _characterController;
    private float _attackPower;
    private float _timeBetweenAttacks;
    
    public Attack(GameObject thisEnemy, GameObject player, float attackPower, float timeBetweenAttacks)
    {
        _thisEnemy = thisEnemy;
        _player = player;
        _attackPower = attackPower;
        _timeBetweenAttacks = timeBetweenAttacks;
    }
    
    public override void Enter()
    {
        Debug.Log("Attack State Enter.");
        
        if(!_player.TryGetComponent(out _characterController))
        {
            Debug.LogError("Player Character Controller not found!");
        }
    }

    public override void Update()
    {
        
    }

    public override void Exit()
    {
        
    }
}