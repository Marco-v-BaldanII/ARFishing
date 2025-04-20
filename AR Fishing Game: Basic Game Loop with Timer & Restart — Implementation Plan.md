# AR Fishing Game: Basic Game Loop with Timer & Restart — Implementation Plan

## Objective

Implement a time-based game loop where the game ends after a set duration, displays a "Game Over" screen with the final score, and allows the player to restart.

---

## 1. GameManager Enhancement (State & Timer)

- **Purpose:** Manage the game state (`Playing`, `GameOver`) and track game time.
- **Implementation (`GameManager.cs`):**
  - Add `using UnityEngine.SceneManagement;` at the top.
  - Define a public `GameState` enum: `public enum GameState { Playing, GameOver }`.
  - Add a public variable: `public GameState currentState = GameState.Playing;`.
  - Add public variables for timing:
    - `public float gameDuration = 60.0f; // Duration in seconds`
    - `private float currentTime = 0.0f;`
  - In `Awake()` or `Start()`, initialize `currentTime = gameDuration` and `currentState = GameState.Playing`.
  - In `Update()`:
    - Check if `currentState == GameState.Playing`.
    - If playing, decrement `currentTime`: `currentTime -= Time.deltaTime;`.
    - If `currentTime <= 0`, call a new `GameOver()` method.
  - Create a public `GameOver()` method:
    - Set `currentTime = 0;`.
    - Set `currentState = GameState.GameOver;`.
    - (Optional: Trigger a `GameOver` event if using events).
    - Debug log "Game Over!".
  - Create a public `RestartGame()` method:
    - Reload the current scene: `SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);`.
  - Modify `IncrementScore(int amount)`:
    - Only increment score if `currentState == GameState.Playing`.

---

## 2. Gameplay Control based on State

- **Purpose:** Pause game actions like fishing and fish movement when the game is over.
- **Implementation:**
  - **`HookStateMachine.cs`:**
    - In methods that initiate actions (like casting the hook, reeling in), add a check at the beginning: `if (GameManager.Instance.currentState != GameManager.GameState.Playing) return;`. Apply this to input handling or state transitions that shouldn't happen when the game is over.
  - **`FishBehavior.cs` (or similar fish script):**
    - In `Update()` or FixedUpdate where movement/AI logic occurs, add a check: `if (GameManager.Instance.currentState != GameManager.GameState.Playing) { /* Optional: Disable components like Rigidbody/Animator or just return */ return; }`.
  - **`FishSpawner.cs` (if exists):**
    - In the spawning logic (likely in `Update` or a coroutine), add a check: `if (GameManager.Instance.currentState != GameManager.GameState.Playing) return;`.

---

## 3. Timer Display

- **Purpose:** Show the remaining game time to the player.
- **Implementation:**
  - Create `TimerDisplay.cs`.
  - Attach to a new `TextMeshProUGUI` object in the UI Canvas.
  - Reference the `TextMeshProUGUI` component.
  - In `Update()`:
    - Check if `GameManager.Instance` exists.
    - Get `currentTime` from `GameManager.Instance`.
    - Format the time (e.g., as minutes:seconds) and update the text. Ensure time doesn't go below zero for display purposes: `float displayTime = Mathf.Max(0, GameManager.Instance.currentTime);`
    - Example formatting: `int minutes = Mathf.FloorToInt(displayTime / 60F); int seconds = Mathf.FloorToInt(displayTime % 60F); timerText.text = string.Format("Time: {0:00}:{1:00}", minutes, seconds);`

---

## 4. Game Over UI

- **Purpose:** Display the game over message, final score, and restart button.
- **Implementation:**
  - Create a new UI Panel in the Canvas (e.g., `GameOverPanel`). Make it inactive by default.
  - Add `TextMeshProUGUI` elements to the panel for:
    - "Game Over!" title.
    - Final score display (e.g., "Final Score: [Score]").
  - Add a UI Button to the panel with text "Restart".
  - Create `GameOverUI.cs`.
  - Attach `GameOverUI.cs` to the `GameOverPanel` GameObject.
  - In `GameOverUI.cs`:
    - Add references: `public GameObject panel;`, `public TextMeshProUGUI finalScoreText;`, `public Button restartButton;`.
    - In `Start()` or `Awake()`:
      - Deactivate the panel: `panel.SetActive(false);`.
      - Add a listener to the button: `restartButton.onClick.AddListener(OnRestartClicked);`.
    - In `Update()`:
      - Check if `GameManager.Instance.currentState == GameManager.GameState.GameOver`.
      - If true and the panel is not active, activate it and display the final score:
        - `panel.SetActive(true);`
        - `finalScoreText.text = "Final Score: " + GameManager.Instance.score;`
    - Create the `OnRestartClicked()` method:
      - Call `GameManager.Instance.RestartGame();`.

---

## 5. Scene Setup

- **Steps:**
  - Ensure the `GameManager` GameObject with `GameManager.cs` exists in the scene.
  - Add a `TextMeshPro - Text (UI)` element for the Timer display.
  - Attach `TimerDisplay.cs` to the Timer Text object and assign the reference.
  - Create the `GameOverPanel` with its child Text and Button elements as described above.
  - Attach `GameOverUI.cs` to the `GameOverPanel` and assign the references (Panel itself, Final Score Text, Restart Button).
  - Ensure the `GameOverPanel` is initially disabled in the Inspector.

---

## Minimal Changes Principle

- Modify `GameManager.cs`, `HookStateMachine.cs`, relevant fish scripts (`FishBehavior.cs`), and potentially `FishSpawner.cs`.
- Add new scripts: `TimerDisplay.cs`, `GameOverUI.cs`.
- Add new UI elements for Timer and Game Over screen.

---

## Example Code Snippets (Additions/Modifications)

### GameManager Additions

```csharp
using UnityEngine.SceneManagement; // Add this

public class GameManager : MonoBehaviour
{
    // ... existing code (Instance, score) ...

    public enum GameState { Playing, GameOver }
    public GameState currentState = GameState.Playing;
    public float gameDuration = 60.0f;
    private float currentTime = 0.0f;

    private void Start() // Or Awake, ensure Singleton is set first
    {
        currentTime = gameDuration;
        currentState = GameState.Playing;
    }

    void Update()
    {
        if (currentState == GameState.Playing)
        {
            currentTime -= Time.deltaTime;
            if (currentTime <= 0f)
            {
                GameOver();
            }
        }
    }

    public void IncrementScore(int amount) {
        if (currentState == GameState.Playing) {
             score += amount;
             // Optionally: trigger a score changed event
        }
    }

    public void GameOver() {
        currentTime = 0f;
        currentState = GameState.GameOver;
        Debug.Log("Game Over!");
        // Optional: Trigger event
    }

    public void RestartGame() {
        // Reset any static variables if necessary before reloading
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
```

### TimerDisplay

```csharp
using UnityEngine;
using TMPro;

public class TimerDisplay : MonoBehaviour
{
    public TextMeshProUGUI timerText;

    void Update()
    {
        if (GameManager.Instance != null && timerText != null)
        {
            float displayTime = Mathf.Max(0, GameManager.Instance.currentTime);
            int minutes = Mathf.FloorToInt(displayTime / 60F);
            int seconds = Mathf.FloorToInt(displayTime % 60F);
            timerText.text = string.Format("Time: {0:00}:{1:00}", minutes, seconds);
        }
    }
}
```

### GameOverUI

```csharp
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
        if (GameManager.Instance != null && GameManager.Instance.currentState == GameManager.GameState.GameOver)
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
        // Optional: Hide panel if game restarts and state changes back
        // else if (panel != null && panel.activeSelf && GameManager.Instance != null && GameManager.Instance.currentState == GameManager.GameState.Playing) {
        //     panel.SetActive(false);
        // }
    }

    void OnRestartClicked()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.RestartGame();
        }
    }

    // Optional: Clean up listener if the object persists between scenes
    // void OnDestroy() {
    //    if (restartButton != null) restartButton.onClick.RemoveListener(OnRestartClicked);
    // }
}
```

---