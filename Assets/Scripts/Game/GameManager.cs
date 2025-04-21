using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum GameState
{
    Playing,
    Paused,
    GameOver
}

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public int score = 0;
    public GameState gameState = GameState.Playing;
    public float gameDuration = 60.0f;
    [HideInInspector]
    public float currentTime = 0.0f;

    public GameObject steeringPannel;
    public GameObject throwPannel;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    private void Start()
    {
        currentTime = gameDuration;
        gameState = GameState.Playing;
    }

    private void Update()
    {
        if (gameState == GameState.Playing)
        {
            currentTime -= Time.deltaTime;
            if (currentTime <= 0f)
            {
                GameOver();
            }
        }
    }

    public void IncrementScore(int amount)
    {
        if (gameState == GameState.Playing)
        {
            score += amount;
            Debug.Log($"GameManager: Score incremented by {amount}. New score: {score}");
        }
    }

    bool shownTutorial = false;

    public void BlinkTutorial()
    {
        if (!shownTutorial)
        {
            StartCoroutine(BlinkRoutine());
            shownTutorial = true;
        }
    }

    private IEnumerator BlinkRoutine()
    {
        for (int i = 0; i < 4; ++i)
        {
            yield return new WaitForSeconds(0.2f);
            steeringPannel.SetActive(true);
            yield return new WaitForSeconds(0.2f);
            steeringPannel.SetActive(false);
        }
    }
    public void GameOver()
    {
        //currentTime = 0f;
        //gameState = GameState.GameOver;
        //Debug.Log("Game Over!");
    }

    public void ThrowPannel()
    {
        throwPannel.SetActive(true);
        StartCoroutine(ThrowRoutine());
    }

    private IEnumerator ThrowRoutine()
    {
        yield return new WaitForSeconds(3f);
        throwPannel.SetActive(false);
    }

    public void RestartGame()
    {
        //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
} 