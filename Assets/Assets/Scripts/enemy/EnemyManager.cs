using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EnemyManager : MonoBehaviour
{
    public static EnemyManager Instance { get; private set; }

    [Header("UI")]
    public TMP_Text monsterRemainingText;

    private int monsterCount = 0;

    public int GetmonsterCount() => monsterCount;

    private void Awake()
    {
        // Singleton setup
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject); // giữ lại qua scene (nếu cần)
    }

    public void RegisterEnemy()
    {
        monsterCount++;
        UpdateUI();
    }

    public void UnregisterEnemy()
    {
        monsterCount--;
        if (monsterCount < 0) monsterCount = 0;
        UpdateUI();
    }

    private void UpdateUI()
    {
        if (monsterRemainingText != null)
        {
            monsterRemainingText.text = $"Monster remaining: {monsterCount}";
        }
    }
}
