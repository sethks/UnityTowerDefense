using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public bool isSetupStage = true;
    public bool isWaveActive = false;
    public float waveTimer = 0f;
    public int activeWaveCount = 0;
    public Button startWaveButton;

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
        //StartCoroutine(SpawnWave(waves[currentWaveIndex]));
    }

    private void Update()
    {
        if(Minion.activeMinions == 0 && !isWaveSpawning && isWaveActive)
            NextWave();

            if(isWaveActive)
                UpdateWaveTimer();
    }

    public void StartWave()
    {
        if(!isWaveActive)
        {
            isWaveActive = true;
            activeWaveCount++;
            StartCoroutine(SpawnWave(waves[currentWaveIndex]));

            waveTimer = waves[currentWaveIndex].intervalBetweenWaves;
            startWaveButton.interactable = true;
        }
        else
        {
            Player.instance.AddGold(GetEarlyStartReward());
            NextWave();
        }
    }

    private void UpdateWaveTimer()
    {
        waveTimer -= Time.deltaTime;
        if(waveTimer <= 0)
        {
            NextWave();
        }
    }

    private int GetEarlyStartReward()
    {
        // Reward the player based on the remaining wave timer, e.g., for every second remaining, give 5 gold
        int reward = (int) (waveTimer * 5);
        reward = Mathf.RoundToInt(reward / 10) * 10;
        Debug.Log("ESR: " + reward + " Gold");
        return reward;
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
        {
            startWaveButton.interactable = false;
            StartCoroutine(SpawnWave(waves[currentWaveIndex]));
        }
        else
        {
            Debug.Log(" Done... Starting Wave");
            isWaveActive = false;
        }
    }
}
