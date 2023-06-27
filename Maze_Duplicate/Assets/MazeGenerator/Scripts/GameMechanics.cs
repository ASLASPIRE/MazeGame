using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMechanics : MonoBehaviour
{
    [Header("Score")]
    public static int Score;

    [Header("Timer")]
    public float TimeRemaining = 180;
    public bool IsTimerRunning;
    public bool IsGameOver;

    [Header("Gameplay")]
    public bool IsGameplayActive;

    [Header("Managers")]
    [SerializeField] private UIManager uiManager;

    // Start is called before the first frame update
    private void Start()
    {
        Score = 0;
        IsTimerRunning = true;
        IsGameOver = false;
        IsGameplayActive = true;
    }

    // Update is called once per frame
    private void Update()
    {
        if (IsTimerRunning)
        {
            if (TimeRemaining > 0)
            {
                TimeRemaining -= Time.deltaTime;
                uiManager.UpdateTimerToText(TimeRemaining);
            }
            else
            {
                Debug.Log("Time has run out!");
                uiManager.DestroyGamePanel();
                TimeRemaining = 0;
                IsTimerRunning = false;
                SetGameOver(true);
            }
        }
    }

    /// <summary>
    /// Adds to the overall score of the game and updates the UI text
    /// </summary>
    /// <param name="toAdd">The amount to add</param>
    /// <returns>Returns the new score's value after addition</returns>
    public int addScore(int toAdd)
    {
        Score += toAdd;
        uiManager.UpdateScoreToText();
        return Score;
    }

    /// <summary>
    /// Converts a float value in seconds to a minute:seconds string display
    /// </summary>
    /// <param name="timeToDisplay">Time in seconds</param>
    /// <returns>Returns the clock formatted time as a string</returns>
    public string DisplayTime(float timeToDisplay)
    {
        timeToDisplay += 1;

        float minutes = Mathf.FloorToInt(timeToDisplay / 60);
        float seconds = Mathf.FloorToInt(timeToDisplay % 60);

        return string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    /// <summary>
    /// Changes the game state IsGameOver, where if this is true events are triggered signaling the end of the game (such as showing the game over panel)
    /// </summary>
    /// <param name="isGameOver">Signals if the game is over</param>
    public void SetGameOver(bool isGameOver)
    {
        IsGameOver = isGameOver;
        if (IsGameOver)
        {
            uiManager.ShowGameOverScreen();
        }
    }
}
