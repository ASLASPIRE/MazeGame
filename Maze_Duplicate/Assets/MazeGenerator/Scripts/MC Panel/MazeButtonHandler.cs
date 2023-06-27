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
    private ScoreIncrement _incrementScore; 

    private string _txt;
    public float _animationTime = 0.1f;

    [Header("Managers")]
    [SerializeField] private UIManager uiManager;
    [SerializeField] private GameMechanics gameMechanics;


    void Awake(){
       _button = GetComponent<Button>();
       _player = FindObjectOfType<Player>();
       _incrementScore = FindObjectOfType<ScoreIncrement>();
       uiManager = FindObjectOfType<UIManager>();
       gameMechanics = FindObjectOfType<GameMechanics>();
    }
    //deal with keypresses
    
    public void SetText(string txt) {
        var child_text = GetComponentInChildren<TextMeshProUGUI>();
        child_text.text = txt;
        _txt = txt;
    }

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

    public void SetColor(Color c) {
        var bg = GetComponent<Image>();
        bg.color = c;
    }

    IEnumerator CorrectAnswer()
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

    IEnumerator WrongAnswer()
    {
        SetColor(Color.red);
        yield return new WaitForSeconds(0.25f);
        //_player.EndPanel(false);
        HandleEndOfPanelLogic();
    }

    public void HandleEndOfPanelLogic()
    {
        uiManager.DestroyGamePanel();
        Destroy(_player.CurrentCoinCollectible);
        _player.IsCoinTouched = false;
        gameMechanics.IsGameplayActive = true;
    }
}
