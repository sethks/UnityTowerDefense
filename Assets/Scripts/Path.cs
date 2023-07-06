using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Path : MonoBehaviour
{
    public List<Transform> waypoints;

    private void Awake()
    {
        waypoints = new List<Transform>();

        foreach (Transform child in transform)
        {
            waypoints.Add(child);
        }
    }
    
    public Transform GetWaypoint(int index)
    {
        if(index < waypoints.Count)
            return waypoints[index];

        return null;
    }

    public int GetPathLength()
    {
        return waypoints.Count;
    }
}
