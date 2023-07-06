using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public List<Wave> waves;
    private int currentWaveIndex = 0;
    private bool isWaveSpawning = false;

    private void Awake()
    {
        // Singleton setup
        if(instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    private void Start()
    {
        StartCoroutine(SpawnWave(waves[currentWaveIndex]));
    }

    private void Update()
    {
        if(Minion.activeMinions == 0 && !isWaveSpawning)
            NextWave();
    }

    private IEnumerator SpawnWave(Wave wave)
    {
        isWaveSpawning = true;
        foreach(Spawn spawn in wave.spawns)
        {
            for(int i = 0; i < spawn.count; i++)
            {
                Instantiate(spawn.minionPrefab, spawn.spawnPoint.position, Quaternion.identity);
                //Debug.Log("Spawned a minion " + (Minion.activeMinions + 1) + " Minions.");
                
                // Wait between each spawn
                yield return new WaitForSeconds(spawn.intervalBetweenSpawns);
            }
            //wait before next type of minions spawn
            yield return new WaitForSeconds(wave.intervalBetweenTypes);
        }
        // Init next wave AFTER all spawns in the current wave are dead
        isWaveSpawning = false;
    }

    private void NextWave()
    {
        currentWaveIndex++;

        //check for extra waves
        if(currentWaveIndex < waves.Count)
            StartCoroutine(SpawnWave(waves[currentWaveIndex]));
        else
        {
            Debug.Log(" Done... Starting Wave");
        }
    }
}
