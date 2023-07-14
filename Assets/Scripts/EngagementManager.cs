using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EngagementManager : MonoBehaviour
{
    public static EngagementManager instance;
    public BarracksTower barracksTower;

    public List<Minion> minions = new List<Minion>();
    public List<Guard> guards = new List<Guard>();

    private List<Minion> minionsToRemove = new List<Minion>();

    private void Awake()
    {
        if(instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
    }

    private void Update()
    {
        UpdateEngagements();
    }

    public void RegisterMinion(Minion minion)
    {
        minions.Add(minion);
    }

    public void RegisterGuard(Guard guard)
    {
        guards.Add(guard);
    }

    public void UnregisterMinion(Minion minion)
    {
        minions.Remove(minion);
    }

    public void UnregisterGuard(Guard guard)
    {
        guards.Remove(guard);
    }

    public void AddMinionToRemove(Minion minion)
    {
        minionsToRemove.Add(minion);
    }

    private void UpdateEngagements()
    {
        foreach (var minion in minions)
        {
            minion.Move();
        }

        foreach (var guard in guards)
        {
            // If the guard is engaging, check the state of the engagement
            if (guard.state == GuardState.Engaging)
            {
                if (guard.target == null || guard.target.health <= 0)
                {
                    guard.state = GuardState.Returning;
                    ReturnGuardToPosition(guard);
                }
                else
                {
                    float distance = Vector3.Distance(guard.transform.position, guard.target.transform.position);
                    if (distance > guard.attackRange)
                    {
                        MoveGuardTowardsMinion(guard, guard.target);
                    }
                    else if (distance <= guard.attackRange)
                    {
                        if (guard.attackCooldownCurrent <= 0)
                            guard.Attack(guard.target);
                    }
                    // Bullshit code to bring minions back to tower if they are too far...
                    // float distantToTower = Vector3.Distance(guard.transform.position, barracksTower.transform.position);
                    // if(distanceToTower > barracksTower.range && Vector3.Distance(guard.target.transform.position, barracksTower.transform.position) > barracksTower.range)
                    // {
                    //     guard.state = GuardState.Returning;

                    // }
                    // // Guard is out of range of their tower, return to idle
                    // if(barracksTower.range < Vector3.Distance(guard.transform.position, barracksTower.transform.position))
                    // {
                    //     guard.state = GuardState.Returning;
                    // }
                }
            }
            // If the guard is returning, continue returning
            else if (guard.state == GuardState.Returning)
            {
                guard.ReturnToSpawn();
            }
            // If the guard is idle, find the closest minion and engage if one is in range
            else
            {
                Minion closestMinion = null;
                float closestDistance = Mathf.Infinity;

                // Find the closest minion
                foreach (var minion in minions)
                {
                    float distance = Vector3.Distance(guard.transform.position, minion.transform.position);
                    if (distance < closestDistance)
                    {
                        closestDistance = distance;
                        closestMinion = minion;
                    }
                }

                if (closestMinion != null && closestDistance <= barracksTower.range)
                {
                    guard.state = GuardState.Engaging;
                    guard.target = closestMinion;
                    MoveGuardTowardsMinion(guard, closestMinion);
                }
            }
        }

        foreach(var minion in minionsToRemove)
        {
            minions.Remove(minion);
        }
        minionsToRemove.Clear();
    }


    private void MoveGuardTowardsMinion(Guard guard, Minion minion)
    {
        float distanceToMinion = Vector3.Distance(guard.transform.position, minion.transform.position);

        if(distanceToMinion > guard.attackRange)
        {
            // Move the guard
            Vector3 direction = (minion.transform.position - guard.transform.position).normalized;
            guard.transform.position += direction * guard.moveSpeed * Time.deltaTime;
        }
        else
        {
            // Guard is in range, attack the minion
            guard.Attack(minion); 
            guard.state = GuardState.Returning;
        }
    }

    private void AttackMinion(Guard guard, Minion minion)
    {
        minion.health -= guard.damage;
        if(minion.health <= 0)
        {
            // minion is dead, remove it from the list of minions
            minions.Remove(minion);
            Destroy(minion.gameObject);
            // Send guard back to its guarding position
            guard.state = GuardState.Returning;
        }
    }

    private void ReturnGuardToPosition(Guard guard)
    {
        guard.ReturnToSpawn();
    }

    public void RemoveMinionAsTarget(Minion minion)
    {
        foreach(var guard in guards)
        {
            if(guard.target == minion)
            {
                guard.target = null;
            }
        }
    }
}
