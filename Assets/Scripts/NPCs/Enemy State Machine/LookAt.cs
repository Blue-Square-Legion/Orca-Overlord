using UnityEngine;

public class LookAt : State
{
    private GameObject _thisEnemy;
    private GameObject _player;
    
    public LookAt(GameObject thisEnemy, GameObject player)
    {
        _thisEnemy = thisEnemy;
        _player = player;
    }
    
    public override void Enter()
    {
        Debug.Log("LookAt State Enter.");
    }

    public override void Update()
    {
        _thisEnemy.transform.LookAt(_player.transform);
    }

    public override void Exit()
    {
        
    }
}