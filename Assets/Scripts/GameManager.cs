using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public HUDManager hudManager;
    public int currentLevel = 1;
    public float nextBaseSpeed = 20f;
    
    [Header("Game State")]
    public bool isGameActive = true;
    public int playerLives = 3;
    public float currentScore = 0f;
    public float scoreMultiplier = 1f;
    public int comboCount = 0;
    public float comboTimer = 0f;
    public float comboTimeWindow = 3f;
    
    [Header("Score Settings")]
    public float speedScoreMultiplier = 0.1f;
    public float trickBaseScore = 100f;
    public float comboMultiplierIncrease = 0.5f;
    public float maxComboMultiplier = 5f;
    
    [Header("UI References")]
    public UIManager uiManager;
    
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void OnDestroy()
    {
        if (Instance == this) {
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        hudManager = FindObjectOfType<HUDManager>();
        uiManager = FindObjectOfType<UIManager>();
        if (hudManager != null)
        {
            hudManager.UpdateLevel(currentLevel);
            hudManager.UpdateScore(currentScore);
        }
    }
    
    void Start()
    {
        if (uiManager == null)
            uiManager = FindObjectOfType<UIManager>();
    }
    
    void Update()
    {
        if (isGameActive)
        {
            UpdateScore();
            UpdateComboTimer();
        }
    }
    
    void UpdateScore()
    {
        // Add score based on speed
        PlayerController player = FindObjectOfType<PlayerController>();
        if (player != null)
        {
            float speedScore = player.GetCurrentSpeed() * speedScoreMultiplier * Time.deltaTime;
            AddScore(speedScore);
        }
    }
    
    void UpdateComboTimer()
    {
        if (comboCount > 0)
        {
            comboTimer -= Time.deltaTime;
            if (comboTimer <= 0)
            {
                ResetCombo();
            }
        }
    }
    
    public void AddScore(float points)
    {
        currentScore += points * scoreMultiplier;
        if (hudManager != null)
    hudManager.UpdateScore(currentScore);
if (uiManager != null)
    uiManager.UpdateScore(currentScore);
    }
    
    public void AddTrickScore(string trickName)
    {
        float trickScore = trickBaseScore;
        comboCount++;
        comboTimer = comboTimeWindow;
        
        // Increase multiplier based on combo
        scoreMultiplier = 1f + (comboCount - 1) * comboMultiplierIncrease;
        scoreMultiplier = Mathf.Clamp(scoreMultiplier, 1f, maxComboMultiplier);
        
        AddScore(trickScore);
        
        if (uiManager != null)
        {
            uiManager.ShowTrickText(trickName, comboCount);
            uiManager.UpdateCombo(comboCount, scoreMultiplier);
        }
    }
    
    public void ResetCombo()
    {
        comboCount = 0;
        scoreMultiplier = 1f;
        comboTimer = 0f;
        
        if (uiManager != null)
            uiManager.UpdateCombo(comboCount, scoreMultiplier);
    }
    
    public void LoseLife()
    {
        playerLives--;
        if (uiManager != null)
            uiManager.UpdateLives(playerLives);
            
        if (playerLives <= 0)
        {
            GameOver();
        }
        else
        {
            // Reset combo on crash
            ResetCombo();
        }
    }
    
    public void GameOver()
    {
        isGameActive = false;
        var player = FindObjectOfType<PlayerController>();
        if (player != null)
            player.DisableControls();
        if (uiManager == null)
            uiManager = FindObjectOfType<UIManager>();
        if (uiManager != null)
            uiManager.ShowGameOver();
    }
    
public void RestartGame()
{
    Time.timeScale = 1f;
    isGameActive = true;
    playerLives = 3;
    currentScore = 0f;
    scoreMultiplier = 1f;
    comboCount = 0;
    comboTimer = 0f;
    currentLevel = 1;
    nextBaseSpeed = 20f;
    SceneManager.LoadScene(SceneManager.GetActiveScene().name);
}
    
public void LoadMainMenu()
{
    Time.timeScale = 1f;
    SceneManager.LoadScene("MainMenu");
}


public void UpdateHUDLevel()
{
    if (hudManager != null)
        hudManager.UpdateLevel(currentLevel);
}


public void LevelUp()
{
    currentLevel++;
    if (hudManager != null)
    {
        hudManager.UpdateLevel(currentLevel);
    }
    // Increase base speed for next round
    nextBaseSpeed += 3f;
    // Optionally reset score, lives, combo, etc. for next round
}
}