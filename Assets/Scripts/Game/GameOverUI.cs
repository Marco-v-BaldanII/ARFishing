using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class GameOverUI : MonoBehaviour
{
    public GameObject panel;
    public TextMeshProUGUI finalScoreText;
    public Button restartButton;
    private bool hasActivated = false;

    void Start()
    {
        Debug.Log("GameOverUI Start - Panel reference: " + (panel != null ? "Valid" : "NULL") + 
                  ", Initial active state: " + (panel != null ? panel.activeSelf.ToString() : "N/A") +
                  ", RestartButton: " + (restartButton != null ? "Valid" : "NULL"));
        
        // Make sure the panel is initially hidden
        if (panel != null)
        {
            panel.SetActive(false);
            Debug.Log("GameOverUI: Panel hidden in Start");
        }
        
        if (restartButton != null) 
        {
            // Standard onClick listener
            restartButton.onClick.AddListener(OnRestartClicked);
            
            // Make sure the button is interactable
            restartButton.interactable = true;
            
            // Get or add EventTrigger for additional events
            EventTrigger trigger = restartButton.gameObject.GetComponent<EventTrigger>();
            if (trigger == null)
            {
                trigger = restartButton.gameObject.AddComponent<EventTrigger>();
            }
            
            // Add pointer down event
            EventTrigger.Entry pointerDownEntry = new EventTrigger.Entry();
            pointerDownEntry.eventID = EventTriggerType.PointerDown;
            pointerDownEntry.callback.AddListener((data) => { 
                Debug.Log("GameOverUI: Button PointerDown detected"); 
                OnRestartClicked();
            });
            trigger.triggers.Add(pointerDownEntry);
            
            Debug.Log("GameOverUI: Restart button configured with multiple listeners");
        }
        else
        {
            Debug.LogError("GameOverUI: RestartButton reference is NULL!");
        }
    }

    void Update()
    {
        // Debug any touch input
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began)
            {
                Debug.Log("GameOverUI: Touch detected at position " + touch.position);
            }
        }
        
        // Only try to activate the panel once
        if (!hasActivated && GameManager.Instance != null && GameManager.Instance.gameState == GameState.GameOver)
        {
            Debug.Log("GameOverUI: Game is in GameOver state. Panel reference: " + (panel != null ? "Valid" : "NULL") + 
                      ", Panel active: " + (panel != null ? panel.activeSelf.ToString() : "N/A"));
            
            if (panel != null)
            {
                Debug.Log("GameOverUI: Attempting to activate panel");
                panel.SetActive(true);
                
                // Set panel to front in case it's being blocked
                if (panel.GetComponent<RectTransform>() != null)
                {
                    panel.transform.SetAsLastSibling();
                }
                
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
                
                // Make sure button is active and interactable
                if (restartButton != null)
                {
                    restartButton.gameObject.SetActive(true);
                    restartButton.interactable = true;
                    Debug.Log("GameOverUI: RestartButton activated and set interactable");
                }
                
                hasActivated = true;
            }
        }
    }

    void OnRestartClicked()
    {
        Debug.Log("GameOverUI: Restart button clicked - Reloading scene now!");
        if (GameManager.Instance != null)
        {
            // Try directly reloading scene as a fallback
            try
            {
                Debug.Log("GameOverUI: Calling GameManager.RestartGame()");
                GameManager.Instance.RestartGame();
            }
            catch (System.Exception e)
            {
                Debug.LogError("GameOverUI: Error in RestartGame: " + e.Message);
                // Direct scene reload as fallback
                UnityEngine.SceneManagement.SceneManager.LoadScene(
                    UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex);
            }
        }
        else
        {
            Debug.LogError("GameOverUI: GameManager.Instance is null, trying direct scene reload");
            // Direct fallback if GameManager is null
            UnityEngine.SceneManagement.SceneManager.LoadScene(
                UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex);
        }
    }
} 