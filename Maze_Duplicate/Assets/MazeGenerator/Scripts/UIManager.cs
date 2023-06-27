using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    [Header("Main UI Display")]
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI timerText;

    [Header("Various Panels or UI Elements")]
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private TextMeshProUGUI gameOverText;
    [SerializeField] private TextMeshProUGUI restartGameText;

    [Header("Prefabs")]
    [SerializeField] private GameObject aslMCGamePanel;
    private GameObject spawnedGamePanel;

    [Header("Managers")]
    [SerializeField] private GameMechanics gameMechanics;
    [SerializeField] private Player player;

    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 1.0f;

        gameOverPanel.SetActive(false);
        //gameOverText.gameObject.SetActive(false);
        restartGameText.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        // Check for game over
        if (gameMechanics.IsGameOver)
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

    public void ShowGameOverScreen()
    {
        StartCoroutine(StartGameOverSequence());
    }

    private IEnumerator StartGameOverSequence()
    {
        Debug.Log("Showing game over screen");
        gameOverPanel.SetActive(true);
        Time.timeScale = 0.0f;
        int score = GameMechanics.Score;
        if (score == 1)
        {
            gameOverText.text = $"Great Job!\nYou collected 1 coin";
        }
        else
        {
            gameOverText.text = $"Great Job!\nYou collected {score} coins";
        }
        yield return new WaitForSecondsRealtime(4.0f);
        restartGameText.gameObject.SetActive(true);
    }

    public void PlayRemoveTimeAnimation(float timeToRemove)
    {
        StartCoroutine(RemoveTime(timeToRemove));
    }

    public void PlayAddTimeAnimation(float timeToAdd)
    {
        StartCoroutine(AddTime(timeToAdd));
    }

    private IEnumerator RemoveTime(float timeToRemove)
	{
		gameMechanics.TimeRemaining -= timeToRemove;
		Color origColor = timerText.color;
		float origSize = timerText.fontSize;
		timerText.color = new Color32(245, 25, 25, 255);
		timerText.fontSize += 10.0f;
		yield return new WaitForSecondsRealtime(0.5f);
		timerText.color = origColor;
		timerText.fontSize = origSize;
	}

    private IEnumerator AddTime(float timeToAdd)
	{
		gameMechanics.TimeRemaining += timeToAdd;
		Color origColor = timerText.color;
		float origSize = timerText.fontSize;
		timerText.color = new Color32(45, 220, 45, 255);
		timerText.fontSize += 10.0f;
		yield return new WaitForSecondsRealtime(0.5f);
		timerText.color = origColor;
		timerText.fontSize = origSize;
	}

    public void InstantiateGamePanel()
    {
        spawnedGamePanel = Instantiate(aslMCGamePanel);
        spawnedGamePanel.SetActive(true);
    }

    public void DestroyGamePanel()
    {
        Destroy(spawnedGamePanel);
    }

    public void UpdateScoreToText()
    {
        scoreText.text = GameMechanics.Score.ToString();
    }

    public void UpdateTimerToText(float timeToDisplaySeconds)
    {
        timeToDisplaySeconds += 1;

        float minutes = Mathf.FloorToInt(timeToDisplaySeconds / 60);
        float seconds = Mathf.FloorToInt(timeToDisplaySeconds % 60);

        // Update UI text
        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }
}
