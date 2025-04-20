using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameOverUI : MonoBehaviour
{
    public GameObject panel;
    public TextMeshProUGUI finalScoreText;
    public Button restartButton;

    void Start()
    {
        if (panel != null) panel.SetActive(false); // Ensure disabled at start
        if (restartButton != null) restartButton.onClick.AddListener(OnRestartClicked);
    }

    void Update()
    {
        if (GameManager.Instance != null && GameManager.Instance.gameState == GameState.GameOver)
        {
            if (panel != null && !panel.activeSelf)
            {
                panel.SetActive(true);
                if (finalScoreText != null)
                {
                    finalScoreText.text = "Final Score: " + GameManager.Instance.score;
                }
            }
        }
    }

    void OnRestartClicked()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.RestartGame();
        }
    }
} 