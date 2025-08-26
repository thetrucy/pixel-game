using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EnemySpawner : MonoBehaviour
{
    public static EnemySpawner Instance { get; private set; } // Singleton

    [Header("Enemy Settings")]
    public GameObject enemyPrefab;

    [Header("Spawn Settings")]
    public float timeBetweenSpawn = 1f; // thời gian giữa mỗi lần spawn
    public int enemiesPerSpawn = 1;     // số quái spawn mỗi lần
    public float timeBetweenWave = 5f;  // delay trước wave tiếp theo

    [Header("UI Settings")]
    public TextMeshProUGUI waveText;
    public TextMeshProUGUI enemiesLeftText;
    public TextMeshProUGUI countdownText;

    private int waveNumber = 1;
    private int enemiesToSpawn = 0;
    private int enemiesSpawned = 0;
    [HideInInspector] public int enemiesAlive = 0;

    private bool isSpawning = false;

    private void Awake()
    {
        if (Instance != null && Instance != this) Destroy(gameObject);
        else Instance = this;
    }

    private void Start()
    {
        UpdateUI();
        StartCoroutine(SpawnWave());
    }

    private void Update()
    {
        // Kiểm tra player chết
        if (PlayerDead())
        {
            GameManager.Instance.PlayerLoses();
            StopAllCoroutines();
            enabled = false;
        }

        // Kiểm tra Win game: qua wave > 10
        if (waveNumber > 10 && enemiesAlive <= 0)
        {
            GameManager.Instance.PlayerWins();
            StopAllCoroutines();
            enabled = false;
        }

        UpdateUI();

        // Nếu wave đã spawn xong và không còn enemy → chuẩn bị wave tiếp
        if (!isSpawning && enemiesAlive <= 0)
        {
            waveNumber++;
            StartCoroutine(SpawnWave());
        }
    }

    private IEnumerator SpawnWave()
    {
        isSpawning = true;

        // Tính tổng số quái trong wave: 3 * 2^(waveNumber-1)
        enemiesToSpawn = 3 * (int)Mathf.Pow(2, waveNumber - 1);
        enemiesSpawned = 0;

        int spawnAmount = Mathf.Min(enemiesPerSpawn + waveNumber - 1, enemiesToSpawn);

        // Countdown trước wave mới
        if (countdownText != null)
        {
            float countdown = timeBetweenWave;
            while (countdown > 0)
            {
                countdownText.text = $"Next Wave: {Mathf.Ceil(countdown)}";
                countdown -= Time.deltaTime;
                yield return null;
            }
            countdownText.text = "";
        }

        // Spawn enemy từng đợt
        while (enemiesSpawned < enemiesToSpawn)
        {
            for (int i = 0; i < spawnAmount && enemiesSpawned < enemiesToSpawn; i++)
            {
                GameObject enemy = Instantiate(enemyPrefab, transform.position, Quaternion.identity);
                enemiesAlive++;
                enemiesSpawned++;

                // Callback khi enemy chết
                EnemyHealthSystem enemyHealth = enemy.GetComponent<EnemyHealthSystem>();
                if (enemyHealth != null)
                    enemyHealth.OnDie += () => { enemiesAlive--; };
            }

            yield return new WaitForSeconds(timeBetweenSpawn);
        }

        isSpawning = false;
        Debug.Log($"Wave {waveNumber} spawned: {enemiesToSpawn} enemies!");
    }

    private void UpdateUI()
    {
        if (waveText != null) waveText.text = $"Wave: {waveNumber}";
        if (enemiesLeftText != null) enemiesLeftText.text = $"Enemies Left: {enemiesAlive}";
    }

    private bool PlayerDead()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player == null) return true;

        HealthManager hp = player.GetComponent<HealthManager>();
        return hp != null && hp.currentHealth <= 0;
    }
}
