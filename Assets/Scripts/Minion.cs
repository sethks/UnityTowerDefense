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
    public int attackRange;
    public bool isEngaged = false;

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
        EngagementManager.instance.RegisterMinion(this);
    }

    protected virtual void Start()
    {
        activeMinions++;
    }

    private void Update()
    {
        if(health <= 0 && gameObject != null)
        {
            Player.instance.AddGold(goldReward);
            EngagementManager.instance.AddMinionToRemove(this);
            EngagementManager.instance.RemoveMinionAsTarget(this);
            StartCoroutine(DestroyMinion());
        }
    }

    public virtual void Move()
    {
        if(isEngaged || Player.instance.health <= 0)
            return;

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
                EngagementManager.instance.AddMinionToRemove(this);
                Destroy(gameObject);
            }
        }
    }

    private IEnumerator DestroyMinion()
    {
        yield return new WaitForEndOfFrame();
        Destroy(gameObject);
    }
}
