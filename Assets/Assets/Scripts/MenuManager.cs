using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public GameObject mainMenuPanel;
    public GameObject settingsPanel;
    public GameObject playPanel;

    void Start() {
    // On game start, show main menu and hide others
    OpenMainMenu();
    }

    // UI Actions
    public void OpenMainMenu()
    {
        CloseAll();
        mainMenuPanel.SetActive(true);
    }

    public void OpenSettings()
    {
        CloseAll();
        settingsPanel.SetActive(true);
    }


    public void OpenPlayPanel()
    {
        CloseAll();
        playPanel.SetActive(true);
    }

    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Quit Game");
    }

    public void LoadMap(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    private void CloseAll()
    {
        settingsPanel.SetActive(false);
        playPanel.SetActive(false);
    }
}
