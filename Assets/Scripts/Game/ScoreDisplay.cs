using TMPro;
using UnityEngine;

public class ScoreDisplay : MonoBehaviour
{
    public TextMeshProUGUI scoreText;

    void Update()
    {
        if (GameManager.Instance == null)
        {
            Debug.LogError("ScoreDisplay: GameManager.Instance is null!");
            return;
        }
        if (scoreText == null)
        {
            Debug.LogError("ScoreDisplay: scoreText field is not assigned!");
            return;
        }
        scoreText.text = "Score: " + GameManager.Instance.score;
    }
} 