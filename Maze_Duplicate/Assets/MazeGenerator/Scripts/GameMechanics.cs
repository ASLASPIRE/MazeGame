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
                DisplayTime(TimeRemaining);
            }
            else
            {
                Debug.Log("Time has run out!");
                uiManager.DestroyGamePanel();
                //Destroy(player.currentPanel);
                TimeRemaining = 0;
                IsTimerRunning = false;
                // TODO: Show time over screen: game over
            }
        }
    }

    public int addScore(int toAdd)
    {
        Score += toAdd;
        uiManager.UpdateScoreToText();
        // TODO: update UI text
        return Score;
    }

    public string DisplayTime(float timeToDisplay)
    {
        timeToDisplay += 1;

        float minutes = Mathf.FloorToInt(timeToDisplay / 60);
        float seconds = Mathf.FloorToInt(timeToDisplay % 60);

        return string.Format("{0:00}:{1:00}", minutes, seconds);

        // TODO: update UI text
        //timeText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    public void SetGameOver(bool isGameOver)
    {
        IsGameOver = isGameOver;
        if (IsGameOver)
        {
            uiManager.ShowGameOverScreen();
        }
    }
}
