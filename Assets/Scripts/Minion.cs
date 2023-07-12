using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Minion : MonoBehaviour
{
    // static counter
    public static int activeMinions = 0;

    // Basic minion attributes
    public int health;
    public int speed;
    public int damage;
    public int defense;
    public int goldReward;
    public bool isEngaged = false;
    public int attackRange;
    public float attackSpeed = 1f;

    private float timeSinceLastAttack = 0f;
    private Guard target;

    // Path attributes for minion
    protected Path path;
    protected int currentWaypointIndex = 0;

    public int CurrentWaypointIndex
    {
        get { return currentWaypointIndex; }
    }

    protected virtual void Awake()
    {
        path = PathManager.instance.path;
    }

    protected virtual void Start()
    {
        activeMinions++;
    }

    private void Update()
    {
        timeSinceLastAttack += Time.deltaTime;
        if (timeSinceLastAttack >= attackSpeed)
        {
            Move();
            timeSinceLastAttack = 0f;
        }
    }

    public virtual void Move()
    {
        if(!isEngaged)
        {
            Transform targetWaypoint = path.GetWaypoint(currentWaypointIndex);
            Vector3 direction = (targetWaypoint.position - transform.position).normalized;
            transform.position += direction * speed * Time.deltaTime;

            if(Vector3.Distance(transform.position, targetWaypoint.position) < 0.1f)
            {
                currentWaypointIndex++;

                if(currentWaypointIndex >= path.GetPathLength())
                {
                    Player.instance.TakeDamage(damage);
                    activeMinions--;
                    Destroy(gameObject);
                }
            }

            Collider[] inRange = Physics.OverlapSphere(transform.position, attackRange); // make sure to define attackRange
            Guard targetGuard = null;
            float closestGuardDistance = Mathf.Infinity;

            foreach(Collider c in inRange)
            {
                Guard g = c.gameObject.GetComponent<Guard>();
                if(g != null && Vector3.Distance(transform.position, g.transform.position) < closestGuardDistance)
                {
                    targetGuard = g;
                    closestGuardDistance = Vector3.Distance(transform.position, g.transform.position);
                }
            }
            target = targetGuard;
            if (target != null)
            {
                isEngaged = true;
            }
        }
        else
        {
            // Already engaged, just attack
            Attack(target);
        }

        if(health <= 0)
        {
            Player.instance.AddGold(goldReward);
            Destroy(gameObject);
        }
    }

    public virtual void Attack(Guard guard)
    {
        if(guard != null)
        {
            int damageDone = Mathf.Max(0, damage - guard.defense);
            if(damageDone < 1)
            {
                guard.health -= 1;
            }
            else
            {
                guard.health -= damageDone;
            }
            if(guard.health <= 0)
            {
                isEngaged = false;
                target = null;
            }
        }
        else
        {
            isEngaged = false;
        }
    }
}
