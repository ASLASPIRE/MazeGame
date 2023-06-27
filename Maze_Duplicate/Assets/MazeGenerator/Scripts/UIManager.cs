using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [Header("Main UI Display")]
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI timerText;

    [Header("Various Panels or UI Elements")]
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private TextMeshProUGUI gameOverText;

    [Header("Game over Panels")]
    [SerializeField] private GameObject loadingBarPanel;
    [SerializeField] private Image loadingBarPanelMask;

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
        loadingBarPanel.SetActive(false);
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
                OnRestartButtonPress();
            }

            //If Q is hit, quit the game
            if (Input.GetKeyDown(KeyCode.Q))
            {
                OnQuitButtonPress();
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
        // Display something else if needed here
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

    public void OnRestartButtonPress()
    {
        StartCoroutine(LoadSceneAsync(1));
    }

    public void OnMainMenuButtonPress()
    {
        StartCoroutine(LoadSceneAsync(0));
    }

    public void OnQuitButtonPress()
    {
        print("Application Quit");
        Application.Quit();
    }

    private IEnumerator LoadSceneAsync(int sceneId)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneId);
        loadingBarPanel.SetActive(true);

        while (!operation.isDone)
        {
            float progressValue = Mathf.Clamp01(operation.progress / 0.95f);
            loadingBarPanelMask.fillAmount = progressValue;
            yield return null;
        }
    }
}
