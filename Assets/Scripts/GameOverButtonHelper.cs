using UnityEngine;
using UnityEngine.UI;

public class GameOverButtonHelper : MonoBehaviour
{
    [SerializeField] Button restartButton;
    [SerializeField] Button mainMenuButton;
    
    void Start()
    {
        UIManager uiManager = FindObjectOfType<UIManager>();
        if (uiManager == null) return;
        
        if (restartButton != null)
        {
            restartButton.onClick.RemoveAllListeners();
            restartButton.onClick.AddListener(() => uiManager.RestartGame());
        }
        
        if (mainMenuButton != null)
        {
            mainMenuButton.onClick.RemoveAllListeners();
            mainMenuButton.onClick.AddListener(() => uiManager.MainMenu());
        }
    }
}

