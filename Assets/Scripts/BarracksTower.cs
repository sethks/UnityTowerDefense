using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarracksTower : BaseTower
{
    public Guard guardPrefab;
    private List<Guard> spawnedGuards = new List<Guard>();
    private List<Transform> spawnPoints = new List<Transform>();
    public int maxGuards = 2;

    // relative spawn positions
    public Vector3 offset1;
    public Vector3 offset2;

    public BarracksTower()
    {
        range = 5;
    }

    private void Start()
    {
        CreateSpawnPoint(offset1);
        CreateSpawnPoint(offset2);
    }

    public override void Attack(Minion target)
    {
        if(spawnedGuards.Count < maxGuards)
        {
            SpawnGuard();
        }
    }

    private void CreateSpawnPoint(Vector3 offset)
    {
        GameObject spawnPoint = new GameObject("SpawnPoint");
        spawnPoint.transform.parent = transform; // Make the new GameObject a child of the tower.
        spawnPoint.transform.localPosition = offset; // Position it relative to the tower.
        spawnPoints.Add(spawnPoint.transform);
    }

    private void SpawnGuard()
    {
        Transform spawnPoint = spawnPoints[spawnedGuards.Count % spawnPoints.Count];
        Guard newGuard = Instantiate(guardPrefab, spawnPoint.position, Quaternion.identity);
        spawnedGuards.Add(newGuard);
    }

    private void OnDestroy()
    {
        foreach (Guard guard in spawnedGuards)
        {
            if(guard != null)
                Destroy(guard.gameObject);
        }
    }

    private void Update()
    {
        if(spawnedGuards.Count < maxGuards)
        {
            SpawnGuard();
        }
    }
}
