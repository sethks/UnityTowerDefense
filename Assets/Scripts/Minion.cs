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
    public int goldReward;

    // Path attributes for basic minion
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
        Move();
    }

    public virtual void Move()
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
        if(health <= 0)
        {
            Player.instance.AddGold(goldReward);
            Destroy(gameObject);
        }
    }
}
