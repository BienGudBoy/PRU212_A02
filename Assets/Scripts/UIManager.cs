using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    [Header("HUD Elements")]
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI speedText;
    public TextMeshProUGUI livesText;
    public TextMeshProUGUI comboText;
    public TextMeshProUGUI multiplierText;
    
    [Header("Trick Display")]
    public TextMeshProUGUI trickText;
    public float trickDisplayTime = 2f;
    
    [Header("Game Over")]
    public GameObject gameOverPanel;
    public TextMeshProUGUI finalScoreText;
    
    [Header("Pause Menu")]
    public GameObject pausePanel;
    
    private bool isPaused = false;
    
    void Update()
    {
        // Update speed display
        PlayerController player = FindObjectOfType<PlayerController>();
        if (player != null && speedText != null)
        {
            speedText.text = "Speed: " + player.GetCurrentSpeed().ToString("F1");
        }
        
        // Handle pause input
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause();
        }
    }
    
    public void UpdateScore(float score)
    {
        if (scoreText != null)
            scoreText.text = "Score: " + score.ToString("F0");
    }
    
    public void UpdateLives(int lives)
    {
        if (livesText != null)
            livesText.text = "Lives: " + lives.ToString();
    }
    
    public void UpdateCombo(int combo, float multiplier)
    {
        if (comboText != null)
        {
            if (combo > 0)
                comboText.text = "Combo: " + combo.ToString();
            else
                comboText.text = "";
        }
        
        if (multiplierText != null)
        {
            if (multiplier > 1f)
                multiplierText.text = "x" + multiplier.ToString("F1");
            else
                multiplierText.text = "";
        }
    }
    
    public void ShowTrickText(string trickName, int comboCount)
    {
        if (trickText != null)
        {
            string displayText = trickName;
            if (comboCount > 1)
                displayText += " (Combo x" + comboCount + ")";
                
            trickText.text = displayText;
            trickText.gameObject.SetActive(true);
            
            StopCoroutine(HideTrickText());
            StartCoroutine(HideTrickText());
        }
    }
    
    private IEnumerator HideTrickText()
    {
        yield return new WaitForSeconds(trickDisplayTime);
        if (trickText != null)
            trickText.gameObject.SetActive(false);
    }
    
public void ShowGameOver()
{
    Time.timeScale = 0f;
    if (gameOverPanel != null)
    {
        gameOverPanel.SetActive(true);
        if (finalScoreText != null && GameManager.Instance != null)
            finalScoreText.text = "Final Score: " + GameManager.Instance.currentScore.ToString("F0");
    }
}
    
    public void TogglePause()
    {
        isPaused = !isPaused;
        
        if (pausePanel != null)
            pausePanel.SetActive(isPaused);
            
        Time.timeScale = isPaused ? 0f : 1f;
    }
    
    // Button methods
    public void RestartGame()
    {
        Time.timeScale = 1f;
        if (GameManager.Instance != null)
            GameManager.Instance.RestartGame();
    }
    
    public void ResumeGame()
    {
        TogglePause();
    }
    
    public void MainMenu()
    {
        Time.timeScale = 1f;
        if (GameManager.Instance != null)
            GameManager.Instance.LoadMainMenu();
    }
}