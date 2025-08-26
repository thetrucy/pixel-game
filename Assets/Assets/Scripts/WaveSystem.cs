using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[System.Serializable]
public class EnemySpawnData
{
    public EnemySpawner spawner; // reference đến spawner
    public int enemiesPerWave = 3; // số quái spawn mỗi wave từ spawner này
}

public class WaveSystem : MonoBehaviour
{
    public static WaveSystem Instance { get; private set; }

    [Header("Wave Settings")]
    public float timeBetweenWave = 5f;
    public int maxWaves = 7;

    [Header("Spawner Settings")]
    public List<EnemySpawnData> spawners = new List<EnemySpawnData>();

    [Header("UI")]
    public TextMeshProUGUI waveText;
    public TextMeshProUGUI waveAnnouncementText;
    public TextMeshProUGUI enemiesLeftText;
    public float waveAnnouncementDuration = 2f;

    private int currentWave = 0;
    [HideInInspector] public int totalEnemiesAlive = 0;

    private void Awake()
    {
        if (Instance != null && Instance != this) Destroy(gameObject);
        else Instance = this;
    }

    private void Start()
    {
        StartCoroutine(WaveLoop());
    }

    private IEnumerator WaveLoop()
    {
        while (currentWave < maxWaves)
        {
            if (currentWave > 0)
                yield return StartCoroutine(ShowWaveAnnouncement("Wave Clear"));

            currentWave++;

            yield return StartCoroutine(ShowWaveAnnouncement($"Wave {currentWave}"));

            float countdown = timeBetweenWave;
            while (countdown > 0)
            {
                countdown -= Time.deltaTime;
                yield return null;
            }

            totalEnemiesAlive = 0;
            foreach (var data in spawners)
            {
                int enemiesToSpawn = data.enemiesPerWave * currentWave;
                totalEnemiesAlive += enemiesToSpawn;

                data.spawner.StartWave(enemiesToSpawn);
            }

            while (totalEnemiesAlive > 0)
                yield return null;
        }

        GameManager.Instance.PlayerWins();
    }

    private IEnumerator ShowWaveAnnouncement(string text)
    {
        if (waveAnnouncementText == null) yield break;

        waveAnnouncementText.text = text;
        Color color = waveAnnouncementText.color;
        float halfDuration = waveAnnouncementDuration / 2f;

        // Fade in
        for (float t = 0; t < halfDuration; t += Time.deltaTime)
        {
            color.a = Mathf.Lerp(0, 1, t / halfDuration);
            waveAnnouncementText.color = color;
            yield return null;
        }
        color.a = 1;
        waveAnnouncementText.color = color;

        yield return new WaitForSeconds(halfDuration);

        // Fade out
        for (float t = 0; t < halfDuration; t += Time.deltaTime)
        {
            color.a = Mathf.Lerp(1, 0, t / halfDuration);
            waveAnnouncementText.color = color;
            yield return null;
        }
        color.a = 0;
        waveAnnouncementText.color = color;
    }

    private void Update()
    {
        if (PlayerDead())
        {
            StopAllCoroutines();           // dừng wave loop
            GameManager.Instance.PlayerLoses();
            enabled = false;               // tắt WaveSystem
        }

        if (waveText != null)
            waveText.text = $"Wave: {currentWave}";

        if (enemiesLeftText != null)
            enemiesLeftText.text = $"Enemies Left: {totalEnemiesAlive}";
    }

    private bool PlayerDead()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player == null) return true;

        HealthManager hp = player.GetComponent<HealthManager>();
        return hp != null && hp.currentHealth <= 0;
    }

    public void EnemyDied()
    {
        totalEnemiesAlive--;
    }
}
