using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro; // Needed for TextMeshPro

public class GameManager : MonoBehaviour
{
    // A single UI Panel to show/hide for the win/loss screen
    public GameObject winLossUI;

    // The TextMeshPro objects for the win and lose messages
    public TextMeshProUGUI winText;
    public TextMeshProUGUI loseText;

    // The name of your main menu scene
    public string mainMenuSceneName = "MainMenu";

   void Start()
    {
        // Check if the variables are null. If so, try to find them in the scene.
        if (winLossUI == null)
        {
            winLossUI = GameObject.Find("WinLossPanel");
        }

        if (winText == null)
        {
            winText = GameObject.Find("WinText").GetComponent<TextMeshProUGUI>();
        }

        if (loseText == null)
        {
            loseText = GameObject.Find("LoseText").GetComponent<TextMeshProUGUI>();
        }

        // Make sure the UI is hidden at the start of the game
        if (winLossUI != null)
        {
            winLossUI.SetActive(false);
        }
    }

    // Call this function when the player wins
    public void PlayerWins()
    {
        winLossUI.SetActive(true);
        winText.gameObject.SetActive(true);
        loseText.gameObject.SetActive(false);
    }

    // Call this function when the player loses
    public void PlayerLoses()
    {
        winLossUI.SetActive(true);
        winText.gameObject.SetActive(false);
        loseText.gameObject.SetActive(true);
    }

    // This function can be called by the Restart button
    public void RestartGame()
    {
        SceneManager.LoadScene(mainMenuSceneName);
    }

    // This function can be called by the Quit button
    public void QuitGame()
    {
        // This will only work in a built application, not in the Unity Editor
        Application.Quit();
    }

}