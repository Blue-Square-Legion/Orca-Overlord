using UnityEngine;

public class Follow : LookAt
{
    private Vector3 _direction;
    
    protected float ClosenessToPlayer;
    
    public Follow(GameObject thisEnemy, GameObject player, float closenessToPlayer, float moveSpeed, float neighbourDistance, float rotationSpeed, DolphinSpawner dolphinSpawner) : base(thisEnemy, player, neighbourDistance, moveSpeed, rotationSpeed, dolphinSpawner)
    {
        ClosenessToPlayer = closenessToPlayer;
    }
    
    public override void Enter()
    {
        
    }

    public override void Update()
    {
        base.Update();
        
        Vector3 direction = (Player.transform.position - ThisEnemy.transform.position).normalized;
        float distanceFromPlayer = Vector3.Distance(ThisEnemy.transform.position, Player.transform.position);
        
        if (distanceFromPlayer > ClosenessToPlayer)
        {
            ThisEnemy.transform.position += direction * MoveSpeed * Time.deltaTime;
        }
    }

    public override void Exit()
    {
        
    }
}