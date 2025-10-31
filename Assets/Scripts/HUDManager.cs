using UnityEngine;
using TMPro;

public class HUDManager : MonoBehaviour
{
    public TMP_Text scoreText;
    public TMP_Text levelText;
    public TMP_Text livesText;
    
    public void UpdateScore(float score)
    {
        if (scoreText != null)
            scoreText.text = $"Score: {score:0}";
    }

    public void UpdateLevel(int level)
    {
        if (levelText != null)
            levelText.text = $"Level: {level}";
    }

    public void UpdateLives(int lives)
    {
        if (livesText != null)
            livesText.text = $"Lives: {lives}";
    }
}
