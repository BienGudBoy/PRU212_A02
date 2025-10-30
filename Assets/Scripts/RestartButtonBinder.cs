using UnityEngine;
using UnityEngine.UI;

public class RestartButtonBinder : MonoBehaviour
{
    void Start()
    {
        var button = GetComponent<Button>();
        if (button == null) return;
        button.onClick.AddListener(OnClick);
    }

    void OnClick()
    {
        Time.timeScale = 1f;
        if (GameManager.Instance != null)
        {
            GameManager.Instance.RestartGame();
        }
    }
}
