using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameOverUI : MonoBehaviour
{
    public GameObject panel;
    public TextMeshProUGUI finalScoreText;
    public Button restartButton;
    private bool hasActivated = false;

    void Start()
    {
        Debug.Log("GameOverUI Start - Panel reference: " + (panel != null ? "Valid" : "NULL") + 
                  ", Initial active state: " + (panel != null ? panel.activeSelf.ToString() : "N/A"));
        
        // Make sure the panel is initially hidden
        if (panel != null)
        {
            panel.SetActive(false);
            Debug.Log("GameOverUI: Panel hidden in Start");
        }
        
        if (restartButton != null) 
        {
            restartButton.onClick.AddListener(OnRestartClicked);
            Debug.Log("GameOverUI: Restart button listener added");
        }
    }

    void Update()
    {
        // Only try to activate the panel once
        if (!hasActivated && GameManager.Instance != null && GameManager.Instance.gameState == GameState.GameOver)
        {
            Debug.Log("GameOverUI: Game is in GameOver state. Panel reference: " + (panel != null ? "Valid" : "NULL") + 
                      ", Panel active: " + (panel != null ? panel.activeSelf.ToString() : "N/A"));
            
            if (panel != null)
            {
                Debug.Log("GameOverUI: Attempting to activate panel");
                panel.SetActive(true);
                Debug.Log("GameOverUI: Panel active state after SetActive(true): " + panel.activeSelf);
                
                if (finalScoreText != null)
                {
                    finalScoreText.text = "Final Score: " + GameManager.Instance.score;
                    Debug.Log("GameOverUI: Set final score text to: " + finalScoreText.text);
                }
                else
                {
                    Debug.LogError("GameOverUI: finalScoreText reference is NULL!");
                }
                hasActivated = true;
            }
        }
    }

    void OnRestartClicked()
    {
        Debug.Log("GameOverUI: Restart button clicked");
        if (GameManager.Instance != null)
        {
            GameManager.Instance.RestartGame();
        }
    }
} 