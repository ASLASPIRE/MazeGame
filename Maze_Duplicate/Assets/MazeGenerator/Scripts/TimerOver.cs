using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TimerOver : MonoBehaviour
{
    [SerializeField] private GameObject timerOverPanel;
    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] private TextMeshProUGUI restartText;
    private bool isGameOver;
    // Start is called before the first frame update
    void Start()
    {
        timerOverPanel.SetActive(false);
        restartText.gameObject.SetActive(false);
        isGameOver = false;
        Time.timeScale = 1.0f;
    }

    // Update is called once per frame
    void Update()
    {
        //If game is over
        if (isGameOver)
        {
            //If R is hit, restart the current scene
            if (Input.GetKeyDown(KeyCode.R))
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }

            //If Q is hit, quit the game
            if (Input.GetKeyDown(KeyCode.Q))
            {
                print("Application Quit");
                Application.Quit();
            }
        }
    }

    public void ShowTimerOverScreen()
    {
        isGameOver = true;
        timerOverPanel.SetActive(true);
        StartCoroutine(GameOverSequence());
    }

    IEnumerator GameOverSequence()
    {
        Time.timeScale = 0f;
        int score = ScoreIncrement.score;
        if (score == 1)
        {
            text.text = $"Great Job!\nYou collected {ScoreIncrement.score} coin";
        }
        else
        {
            text.text = $"Great Job!\nYou collected {ScoreIncrement.score} coins";
        }
        yield return new WaitForSecondsRealtime(4.0f);
        restartText.gameObject.SetActive(true);
    }
}
