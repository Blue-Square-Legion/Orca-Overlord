using UnityEngine;

public class LookAt : Idle
{
    private Vector3 _direction;
    
    public LookAt(GameObject thisEnemy, GameObject player, float neighborDistance, float moveSpeed, float rotationSpeed, DolphinSpawner dolphinSpawner) : base(thisEnemy, player, neighborDistance, moveSpeed, rotationSpeed, dolphinSpawner)
    {
        
    }
    
    public override void Enter()
    {
        
    }

    public override void Update()
    {
        _direction = (Player.transform.position - ThisEnemy.transform.position).normalized;

        Quaternion lookRotation = Quaternion.LookRotation(_direction);
        
        ThisEnemy.transform.rotation = Quaternion.Slerp(ThisEnemy.transform.rotation, lookRotation, RotationSpeed * Time.deltaTime);
    }

    public override void Exit()
    {
        
    }
}