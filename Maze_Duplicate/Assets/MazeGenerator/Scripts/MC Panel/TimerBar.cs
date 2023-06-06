using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.Experimental.GraphView.GraphView;

public class TimerBar : MonoBehaviour
{
    [Header("Progress bar parameters")]
    public Image mask;
    private Player player;

    [Header("Timer parameters")]
    public float timeRemaining = 10;
    public bool timerIsRunning = false;

    private float initialTime;

    // Start is called before the first frame update
    private void Start()
    {
        // Starts the timer automatically
        timerIsRunning = true;

        // Remembers initial time
        initialTime = timeRemaining;

        // Find player
        player = FindObjectOfType<Player>();
    }

    // Update is called once per frame
    private void Update()
    {
        if (timerIsRunning)
        {
            if (timeRemaining > 0)
            {
                timeRemaining -= Time.deltaTime;
            }
            else
            {
                Debug.Log("Panel time has run out!");
                player.EndPanel(false);
            }
        }

        GetCurrentFill();
    }

    private void GetCurrentFill()
    {
        float fillAmount = timeRemaining / initialTime;
        mask.fillAmount = fillAmount;
    }
}
