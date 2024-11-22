using UnityEngine;

public class Follow : State
{
    private GameObject _thisEnemy;
    private GameObject _player;
    private Vector3 _direction;
    private float _moveSpeed;
    
    public Follow(GameObject thisEnemy, GameObject player, float moveSpeed)
    {
        _thisEnemy = thisEnemy;
        _player = player;
        _moveSpeed = moveSpeed;
    }
    
    public override void Enter()
    {
        Debug.Log("Follow State Enter.");
    }

    public override void Update()
    {
        _thisEnemy.transform.LookAt(_player.transform);
        
        Vector3 direction = (_player.transform.position - _thisEnemy.transform.position).normalized;
        _thisEnemy.transform.position += direction * _moveSpeed * Time.deltaTime;
    }

    public override void Exit()
    {
        
    }
}