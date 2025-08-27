using UnityEngine;
using TMPro;

public class HighestScoreUpdate : MonoBehaviour
{
    public TextMeshProUGUI highScoreText;

    private void Start()
    {
        int highestScore = PlayerPrefs.GetInt("HighScore", 0);
        if (highScoreText != null)
        {
            highScoreText.text = $"Highest Score: {highestScore}";
        }
    }
}
