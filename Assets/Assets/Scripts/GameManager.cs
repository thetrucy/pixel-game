using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; } // Singleton

    [Header("UI")]
    public GameObject winLossUI;
    public TextMeshProUGUI winText;
    public TextMeshProUGUI loseText;

    [Header("Scene")]
    public string mainMenuSceneName = "MainMenu";

    private void Awake()
    {
        // Thiết lập Singleton
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    private void Start()
    {
        // Nếu chưa gán UI, tìm trong scene
        if (winLossUI == null) winLossUI = GameObject.Find("WinLossPanel");
        if (winText == null) winText = GameObject.Find("WinText").GetComponent<TextMeshProUGUI>();
        if (loseText == null) loseText = GameObject.Find("LoseText").GetComponent<TextMeshProUGUI>();

        // Ẩn UI khi bắt đầu
        if (winLossUI != null) winLossUI.SetActive(false);
    }

    public void PlayerWins()
    {
        winLossUI.SetActive(true);
        if (winText != null) winText.gameObject.SetActive(true);
        if (loseText != null) loseText.gameObject.SetActive(false);
        int finalScore = WaveSystem.Instance != null ? WaveSystem.Instance.currentScore : 0;
        int highestScore = PlayerPrefs.GetInt("HighScore", 0);
        if (finalScore > highestScore)
        {
            PlayerPrefs.SetInt("HighScore", finalScore);
            PlayerPrefs.Save();
        }
    }

    public void PlayerLoses()
    {
        winLossUI.SetActive(true);
        if (winText != null) winText.gameObject.SetActive(false);
        if (loseText != null) loseText.gameObject.SetActive(true);
        int finalScore = WaveSystem.Instance != null ? WaveSystem.Instance.currentScore : 0;
        int highestScore = PlayerPrefs.GetInt("HighScore", 0);
        if (finalScore > highestScore)
        {
            PlayerPrefs.SetInt("HighScore", finalScore);
            PlayerPrefs.Save();
        }
    }

    public void RestartGame()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.name);
    }

    public void QuitToMainMenu()
    {
        SceneManager.LoadScene(mainMenuSceneName);
    }
}
