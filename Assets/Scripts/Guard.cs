using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GuardState
{
    Idle,
    Engaging,
    Returning
}

public class Guard : MonoBehaviour
{
    public GuardState state;
    public int health;
    public int speed;
    public int damage;
    public int defense;
    public int attackRange;
    public float attackCooldown;
    public float attackCooldownCurrent;
    public float moveSpeed = 1.0f;
    public Minion target;

    private Vector3 spawnPoint;

    protected virtual void Awake()
    {
        spawnPoint = transform.position;
        state = GuardState.Idle;
        EngagementManager.instance.RegisterGuard(this);
        attackCooldownCurrent = attackCooldown;
    }
    
    private void Update()
    {
        if(attackCooldownCurrent > 0)
        {
            attackCooldownCurrent -= Time.deltaTime;
        }

        if(health <= 0)
        {
            EngagementManager.instance.UnregisterGuard(this);
            Destroy(gameObject);
        }
    }

    public void ReturnToSpawn()
    {
        if(target != null)
            target.isEngaged = false;
            
        Vector3 direction = (spawnPoint - transform.position).normalized;
        transform.position += direction * moveSpeed * Time.deltaTime;

        if (Vector3.Distance(transform.position, spawnPoint) < 0.1f)
        {
            state = GuardState.Idle;
        }
    }

    public void Attack(Minion enemy)
    {
        if(attackCooldownCurrent <= 0)
        {
            attackCooldownCurrent = attackCooldown;
            enemy.isEngaged = true;
            int damageDone = Mathf.Max(0, damage - enemy.defense);

            if(damageDone < 1)
            {
                enemy.health -= 1;
            }
            else
            {
                enemy.health -= damageDone;
            }

            if(enemy.health <= 0)
            {
                target = null;
            }
        }
    }
}
