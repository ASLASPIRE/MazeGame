using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ScoreIncrement : MonoBehaviour
{
    public static int score = 0;
    public TextMeshProUGUI scoreText;

    // Start is called before the first frame update
    private void Start()
    {
        score = 0;
    }

    public void addScore()
    {
        score += 1;
        scoreText.text = score.ToString();
    }
}