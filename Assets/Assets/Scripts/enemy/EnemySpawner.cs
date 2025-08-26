using System.Collections;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab;
    public float timeBetweenSpawn = 1f;
    public int enemiesPerSpawn = 1;

    public void StartWave(int enemiesToSpawn)
    {
        StartCoroutine(SpawnEnemies(enemiesToSpawn));
    }

    private IEnumerator SpawnEnemies(int enemiesToSpawn)
    {
        int spawned = 0;
        while (spawned < enemiesToSpawn)
        {
            int spawnCount = Mathf.Min(enemiesPerSpawn, enemiesToSpawn - spawned);
            for (int i = 0; i < spawnCount; i++)
            {
                GameObject enemy = Instantiate(enemyPrefab, transform.position, Quaternion.identity);

                EnemyHealthSystem enemyHealth = enemy.GetComponent<EnemyHealthSystem>();
                if (enemyHealth != null)
                {
                    enemyHealth.OnDie += () =>
                    {
                        WaveSystem.Instance.EnemyDied();
                    };
                }

                spawned++;
            }

            yield return new WaitForSeconds(timeBetweenSpawn);
        }
    }
}
