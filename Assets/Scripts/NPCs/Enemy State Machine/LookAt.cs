using UnityEngine;

public class LookAt : Idle
{
    protected GameObject ThisEnemy;
    protected GameObject Player;
    
    public LookAt(GameObject thisEnemy, GameObject player)
    {
        ThisEnemy = thisEnemy;
        Player = player;
    }
    
    public override void Enter()
    {
        
    }

    public override void Update()
    {
        ThisEnemy.transform.LookAt(Player.transform);
    }

    public override void Exit()
    {
        
    }
}