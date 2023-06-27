using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class MazeButtonHandler : MonoBehaviour
{   
    //variables for RunnerGame button presses
    public KeyCode _key; 
    public GameObject confetti_;
    public MazeQuestionLoader _ql;

    private Button _button;
    private Player _player;

    private string _txt;
    public float _animationTime = 0.1f;

    [Header("Managers")]
    [SerializeField] private UIManager uiManager;
    [SerializeField] private GameMechanics gameMechanics;


    void Awake(){
       _button = GetComponent<Button>();
       _player = FindObjectOfType<Player>();
       uiManager = FindObjectOfType<UIManager>();
       gameMechanics = FindObjectOfType<GameMechanics>();
    }

    /// <summary>
    /// Sets the text of the button
    /// </summary>
    /// <param name="txt">Text to set</param>
    public void SetText(string txt) {
        var child_text = GetComponentInChildren<TextMeshProUGUI>();
        child_text.text = txt;
        _txt = txt;
    }

    /// <summary>
    /// Handles a MC button click and handles logic according to whether that was the correct answer
    /// </summary>
    public void HandleClick() {
        if (_ql.AnswerIsCorrect(_txt)) {
            StartCoroutine(CorrectAnswer());
        } else {
            StartCoroutine(WrongAnswer());
        }

        Invoke("ResetToWhite", _animationTime);
    }

    public void ResetToWhite() {
        SetColor(Color.white);
    }

    /// <summary>
    /// Sets the color of the button
    /// </summary>
    /// <param name="c">Color to set to</param>
    public void SetColor(Color c) {
        var bg = GetComponent<Image>();
        bg.color = c;
    }

    private IEnumerator CorrectAnswer()
    {
        SetColor(Color.green);
        yield return new WaitForSeconds(0.25f);

        HandleEndOfPanelLogic();

        gameMechanics.addScore(1);

        //_player.EndPanel(true);
        //_incrementScore.addScore();
        GameObject burst = Instantiate(confetti_, _player.transform.position, Quaternion.identity);
        burst.GetComponent<ParticleSystem>().Play();
        //_ql.LoadRandomQuestion();
    }

    private IEnumerator WrongAnswer()
    {
        SetColor(Color.red);
        yield return new WaitForSeconds(0.25f);
        //_player.EndPanel(false);
        HandleEndOfPanelLogic();
    }

    /// <summary>
    /// Upon end of MC panel's duration, destroys the panel, destroys the player's collectible, and resumes gameplay
    /// </summary>
    public void HandleEndOfPanelLogic()
    {
        uiManager.DestroyGamePanel();
        Destroy(_player.CurrentCoinCollectible);
        _player.IsCoinTouched = false;
        gameMechanics.IsGameplayActive = true;
    }
}
