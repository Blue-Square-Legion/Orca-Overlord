using UnityEngine;

public class Attack : Follow
{
    private float _attackCooldown; // Time between attacks
    private float _attackRange; // Distance to trigger attack
    private float _knockbackPower = 10f; // Strength of knockback
    private int _damage;
    private float _nextAttackTime;

    public Attack(GameObject thisEnemy, GameObject player, float knockbackPower, int damage, float attackRange, float attackCooldown, float closenessToPlayer, float moveSpeed, float neighbourDistance, float rotationSpeed, DolphinSpawner dolphinSpawner) :
        base(thisEnemy, player, closenessToPlayer, moveSpeed, neighbourDistance, rotationSpeed, dolphinSpawner)
    {
        _knockbackPower = knockbackPower;
        _attackRange = attackRange;
        _attackCooldown = attackCooldown;
        _damage = damage;
    }
    
    public override void Enter()
    {
    }

    public override void Update() 
    {
        base.Update();
        
        // Calculate distance to the player
        float distance = Vector3.Distance(ThisEnemy.transform.position, Player.transform.position);

        if (distance <= _attackRange && Time.time >= _nextAttackTime) 
        {
            PerformAttack();
            _nextAttackTime = Time.time + _attackCooldown;
        }
    }

    public override void Exit() 
    {
    }

    private void PerformAttack() 
    {
        // Apply knockback to the player
        Vector3 knockbackDirection = (Player.transform.position - ThisEnemy.transform.position).normalized;
        knockbackDirection.y = 0; // Ensure knockback is only horizontal

        Player.GetComponent<PlayerController>()?.ApplyKnockback(knockbackDirection, _knockbackPower, _damage);
    }
}