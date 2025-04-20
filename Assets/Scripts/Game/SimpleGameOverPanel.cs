using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SimpleGameOverPanel : MonoBehaviour
{
    public TextMeshProUGUI finalScoreText;
    public Button restartButton;
    private bool hasActivated = false;
    
    // Canvas group to control visibility (or use any child panel)
    public CanvasGroup canvasGroup;
    public GameObject visualPanel; // Optional: child panel to toggle if not using CanvasGroup

    void Awake()
    {
        // Find components if not assigned
        if (canvasGroup == null)
            canvasGroup = GetComponent<CanvasGroup>();
        
        if (visualPanel == null && transform.childCount > 0)
            visualPanel = transform.GetChild(0).gameObject;
            
        HidePanel();
        Debug.Log("SimpleGameOverPanel: Initialized. References - finalScoreText: " + 
                  (finalScoreText != null ? "Valid" : "NULL") + ", restartButton: " + 
                  (restartButton != null ? "Valid" : "NULL") + 
                  ", canvasGroup: " + (canvasGroup != null ? "Valid" : "NULL") +
                  ", visualPanel: " + (visualPanel != null ? "Valid" : "NULL"));
    }
    
    void Start()
    {
        if (restartButton != null)
        {
            restartButton.onClick.AddListener(() => {
                if (GameManager.Instance != null)
                {
                    Debug.Log("SimpleGameOverPanel: Restart button clicked");
                    GameManager.Instance.RestartGame();
                }
            });
        }
    }

    void Update()
    {
        // Important: we use !hasActivated to ensure we only do this once
        if (!hasActivated && GameManager.Instance != null && 
            GameManager.Instance.gameState == GameState.GameOver)
        {
            Debug.Log("SimpleGameOverPanel: Game state is GameOver, showing panel");
            
            // Show the panel
            ShowPanel();
            
            // Update score text
            if (finalScoreText != null)
            {
                finalScoreText.text = "Final Score: " + GameManager.Instance.score;
                Debug.Log("SimpleGameOverPanel: Set final score to " + GameManager.Instance.score);
            }
            
            hasActivated = true;
        }
    }
    
    void HidePanel()
    {
        // Use CanvasGroup if available (preferred method)
        if (canvasGroup != null)
        {
            canvasGroup.alpha = 0;
            canvasGroup.interactable = false;
            canvasGroup.blocksRaycasts = false;
            Debug.Log("SimpleGameOverPanel: Panel hidden via CanvasGroup");
        }
        // Otherwise use child panel if available
        else if (visualPanel != null)
        {
            visualPanel.SetActive(false);
            Debug.Log("SimpleGameOverPanel: Panel hidden via child GameObject");
        }
        // Fallback if neither is available
        else
        {
            // Just hide the immediate children instead of the gameObject itself
            for (int i = 0; i < transform.childCount; i++)
            {
                transform.GetChild(i).gameObject.SetActive(false);
            }
            Debug.Log("SimpleGameOverPanel: Panel hidden by disabling all children");
        }
    }
    
    void ShowPanel()
    {
        // Use CanvasGroup if available
        if (canvasGroup != null)
        {
            canvasGroup.alpha = 1;
            canvasGroup.interactable = true;
            canvasGroup.blocksRaycasts = true;
            Debug.Log("SimpleGameOverPanel: Panel shown via CanvasGroup");
        }
        // Otherwise use child panel if available
        else if (visualPanel != null)
        {
            visualPanel.SetActive(true);
            Debug.Log("SimpleGameOverPanel: Panel shown via child GameObject");
        }
        // Fallback if neither is available
        else
        {
            // Show all the immediate children
            for (int i = 0; i < transform.childCount; i++)
            {
                transform.GetChild(i).gameObject.SetActive(true);
            }
            Debug.Log("SimpleGameOverPanel: Panel shown by enabling all children");
        }
    }
} 