using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseTower : MonoBehaviour
{
    public int damage;
    public int range;
    public float fireRate;
    public int cost;

    private float lastShotTime;

    protected virtual void Awake()
    {

    }
    
    private void Start()
    {
        LayerMask.NameToLayer("Tower");
        lastShotTime = -fireRate; // Allows us to fire right away
    }

    public void Build()
    {
        if(Player.instance.gold >= cost)
        {
            Player.instance.SpendGold(cost);
        }
    }

    private void Update()
    {
        if(Time.time >= lastShotTime + fireRate)
        {
            Minion target = FindTarget();
            if(target != null)
            {
                Attack(target);
                lastShotTime = Time.time;
            }
        }
    }

    public virtual void Attack(Minion target)
    {
        target.health -= damage;
    }

    private Minion FindTarget()
    {
        Collider[] inRange = Physics.OverlapSphere(transform.position, range);
        Minion targetMinion = null;

        int targetWaypoint = -1;

        foreach(Collider c in inRange)
        {
            Minion m = c.gameObject.GetComponent<Minion>();
            if(m != null && m.CurrentWaypointIndex > targetWaypoint)
            {
                targetMinion = m;
                targetWaypoint = m.CurrentWaypointIndex;
            }
        }
        return targetMinion;
    }
}
