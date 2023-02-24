using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] List<WaveConfigSO> waveConfigs;
    [SerializeField] float timeBetweenWaves = 0f;
    [SerializeField] bool isLooping = false;
    WaveConfigSO currentWave;

    void Start()
    {
        StartCoroutine(SpawnEnemyWaves());
    }

    public WaveConfigSO GetCurrentWave()
    {
        return currentWave;
    }
    private IEnumerator SpawnEnemyWaves()
    {
        do
        {
            foreach (var wave in waveConfigs)
            {
                currentWave = wave;

                for (int i = 0; i < currentWave.GetEnemyCount(); i++)
                {
                    Instantiate(currentWave.GetEnemyPrefab(i), currentWave.GetStartingWaypoint().position, Quaternion.Euler(0, 0, 180), transform);
                    yield return new WaitForSeconds(currentWave.GetRandomSpawnTime());
                }

                yield return new WaitForSeconds(timeBetweenWaves -  Math.Min((Time.deltaTime * 0.002f), 0f));
            }
        }
        while (isLooping);
    }
}
