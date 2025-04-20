using TMPro;
using UnityEngine;

public class TimerDisplay : MonoBehaviour
{
    public TextMeshProUGUI timerText;

    void Update()
    {
        if (GameManager.Instance == null)
        {
            Debug.LogError("TimerDisplay: GameManager.Instance is null!");
            return;
        }
        if (timerText == null)
        {
            Debug.LogError("TimerDisplay: timerText field is not assigned!");
            return;
        }
        float displayTime = Mathf.Max(0, GameManager.Instance.currentTime);
        int minutes = Mathf.FloorToInt(displayTime / 60f);
        int seconds = Mathf.FloorToInt(displayTime % 60f);
        timerText.text = $"Time: {minutes:00}:{seconds:00}";
    }
} 