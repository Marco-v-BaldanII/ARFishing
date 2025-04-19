using TMPro;
using UnityEngine;

public class ScoreDisplay : MonoBehaviour
{
    public TextMeshProUGUI scoreText;

    void Update()
    {
        if (GameManager.Instance != null && scoreText != null)
            scoreText.text = "Score: " + GameManager.Instance.score;
    }
} 