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
    public int range;
    public int attackRange;
    public float moveSpeed = 1.0f;

    private Vector3 spawnPoint;

    // Interaction with enemy minion
    private Minion target;

    protected virtual void Awake()
    {
        spawnPoint = transform.position;
        state = GuardState.Idle;
    }
    
    private void Update()
    {
        switch(state)
        {
            case GuardState.Idle:
                FindTarget();
                break;
            case GuardState.Engaging:
                EngagTarget();
                break;
            case GuardState.Returning:
                ReturnToSpawn();
                break;
        }

        if(health <= 0)
        {
            if(target != null)
            {
                target.isEngaged = false;
            }
            Destroy(gameObject);
        }
    }

    private void FindTarget()
    {
        Collider[] inRange = Physics.OverlapSphere(transform.position, range);
        Minion targetMinion = null;
        int targetWaypoint = -1;

        foreach(Collider c in inRange)
        {
            Minion m = c.gameObject.GetComponent<Minion>();
            if(m != null && m.CurrentWaypointIndex > targetWaypoint)
            {
                if(targetMinion != null)
                    targetMinion.isEngaged = false;
                targetMinion = m;
                targetMinion.isEngaged = true;
                targetWaypoint = m.CurrentWaypointIndex;
            }
        }
        target = targetMinion;
        if(target != null)
        {
            state = GuardState.Engaging;
        }
    }

    private void EngagTarget()
    {
        if(target == null || target.health <= 0)
        {
            FindTarget();

            if(target == null)
                state = GuardState.Returning;
        }
        else
        {
            state = GuardState.Engaging;
            float distanceToTarget = Vector3.Distance(transform.position, target.transform.position);
            if(distanceToTarget > attackRange)
            {
                // Move towards minion
                Vector3 direction = (target.transform.position - transform.position).normalized;
                transform.position += direction * moveSpeed * Time.deltaTime;
            }
            else
            {
                // Attack minion
                //Debug.Log("Attacking enemy");
                Attack(target);
            }
        }
    }

    private void ReturnToSpawn()
    {
        Vector3 direction = (spawnPoint - transform.position).normalized;
        transform.position += direction * moveSpeed * Time.deltaTime;

        if (Vector3.Distance(transform.position, spawnPoint) < 0.1f)
        {
            state = GuardState.Idle;
        }
    }

    public virtual void Attack(Minion enemy)
    {
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
            enemy.isEngaged = false;
            target = null;
        }
    }
}
