// EnemySpawner.cs
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [Header("Enemy to Spawn")]
    public GameObject enemyPrefab;

    [Header("Spawn Settings")]
    public float timeBetweenWaves = 5f;
    public int enemiesPerWave = 5;

    private float timeUntilNextWave;
    private int currentWave = 0;

    void Start()
    {
        // Set the timer for the first wave
        timeUntilNextWave = timeBetweenWaves;
    }

    void Update()
    {
        // Check if all waves have been spawned
        if (currentWave >= 3)
        {
            return;
        }

        // Countdown the timer
        timeUntilNextWave -= Time.deltaTime;

        // If the timer runs out, spawn a wave
        if (timeUntilNextWave <= 0)
        {
            SpawnWave();
            timeUntilNextWave = timeBetweenWaves; // Reset the timer
        }
    }

    void SpawnWave()
    {
        for (int i = 0; i < enemiesPerWave; i++)
        {
            // Spawn an enemy at the spawner's position
            Instantiate(enemyPrefab, transform.position, Quaternion.identity);
        }
        
        currentWave++; // Increment the wave counter
        Debug.Log($"Wave {currentWave} spawned!");
    }
}