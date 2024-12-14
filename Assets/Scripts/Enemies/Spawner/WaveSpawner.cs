using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]

public class Wave
{
    public GameObject[] enemies;
}
public class WaveSpawner : MonoBehaviour
{
    private enum WaveState
    {
        waitngForWaveStart,
        waitingForNextWave,
        spawningWave,
        AllWavesDone
    };

    private WaveState currentWaveState = WaveState.waitngForWaveStart;

    public Wave[] waves;
    public Vector2[] spawnPoints;
    public float timeBetweenWaves = 5f;
    public float timeBeforeFirstWave = 2f;
    public float timeBeforeLockedZoneEnds = 2f;

    private int currentWaveIndex = 0;
    private int enemiesRemaining = 0;

    private bool wavesStarted = false;
    private bool isWaitingForNextWave = false;

    public UnityEvent OnWavesEnd;

    public void StartWaves()
    {
        if(!wavesStarted)
        {
            wavesStarted = true;
            print("Starting waves!");
            Invoke(nameof(StartNextWave), timeBeforeFirstWave);
        }
    }
    private void SetWaveState(WaveState state)
    {
        currentWaveState = state;

        switch(currentWaveState)
        {
            case WaveState.spawningWave:
                StartNextWave();
                break;
            case WaveState.waitingForNextWave:
                Invoke(nameof(StartNextWave), timeBetweenWaves);
                break;
            case WaveState.AllWavesDone:
                Invoke(nameof(HandleAllWaveEnd), timeBeforeLockedZoneEnds);
                break;
        }
    }

    private void StartNextWave()
    {
        if (currentWaveIndex < waves.Length)
        {
            Wave currentWave = waves[currentWaveIndex];

            enemiesRemaining = currentWave.enemies.Length;
            for (int i= 0; i < currentWave.enemies.Length; i++)
            {
                SpawnEnemy(currentWave.enemies[i], spawnPoints[i]);
            }
            currentWaveIndex++;
        }
    }

    private void HandleAllWaveEnd()
    {
        print("All waves Complete!");
        wavesStarted = false;
        OnWavesEnd?.Invoke();
        Destroy(this);
    }

    private void SpawnEnemy(GameObject enemy, Vector2 spawnPoint)
    {
        GameObject enemySpawn = Instantiate(enemy, new Vector3(spawnPoint.x, spawnPoint.y, 0), Quaternion.identity);
        Entity entity = enemySpawn.GetComponent<Entity>();

        if(entity != null)
        {
            entity.OnDeath.AddListener(OnEnemyKilled);
        }
    }

    private void OnEnemyKilled()
    {
        enemiesRemaining--;

        if(enemiesRemaining <= 0)
        {
            print("Wave complete!");

            if (currentWaveIndex < waves.Length)
            {
                SetWaveState(WaveState.waitingForNextWave);
            }
            else SetWaveState(WaveState.AllWavesDone);

        }
    }

    IEnumerator NextWaveCountdown()
    {
        isWaitingForNextWave = true;
        yield return new WaitForSeconds(timeBetweenWaves);
        StartNextWave();
        isWaitingForNextWave = false;
    }

    private void OnDrawGizmos()
    {
        if (spawnPoints != null)
        {
            Gizmos.color = Color.green; 

            foreach (Vector2 spawnPoint in spawnPoints)
            {
                if (spawnPoint != null)
                {
                    Gizmos.DrawWireSphere(new Vector3(spawnPoint.x, spawnPoint.y, 0), 0.5f); 
                }
            }
        }
    }
}
