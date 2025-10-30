using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    public void StartGame()
    {
        // Reset GameManager state for a fresh start
        if (GameManager.Instance != null)
        {
            GameManager.Instance.isGameActive = true;
            GameManager.Instance.playerLives = 3;
            GameManager.Instance.currentScore = 0f;
            GameManager.Instance.scoreMultiplier = 1f;
            GameManager.Instance.comboCount = 0;
            GameManager.Instance.comboTimer = 0f;
            GameManager.Instance.currentLevel = 1;
            GameManager.Instance.nextBaseSpeed = 20f;
        }
        
        Time.timeScale = 1f;
        SceneManager.LoadScene("Level1");
    }
    
    public void QuitGame()
    {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }
}
