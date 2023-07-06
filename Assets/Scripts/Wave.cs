using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Wave
{
    public List<Spawn> spawns;
    public float intervalBetweenTypes = 5f;
    public float intervalBetweenWaves = 10f;
}

[System.Serializable]
public class Spawn
{
    public GameObject minionPrefab;
    public Transform spawnPoint;
    public int count = 5;
    public float intervalBetweenSpawns = 1f;
}