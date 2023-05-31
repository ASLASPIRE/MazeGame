using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ButtonHandler : MonoBehaviour
{   //variables for RunnerGame button presses
    public KeyCode _key; 
    private Button _button;
    


    public QuestionLoader _ql;
    private string _txt;
    public float _animationTime = 0.1f;


    void Awake(){
        _button = GetComponent<Button>();
    }
    //deal with keypresses
    
    public void SetText(string txt) {
        var child_text = GetComponentInChildren<TextMeshProUGUI>();
        child_text.text = txt;
        _txt = txt;
    }

    public void HandleClick() {
        if (_ql.AnswerIsCorrect(_txt)) {
            SetColor(Color.green);
            _ql.LoadRandomQuestion();
        } else {
            SetColor(Color.red);
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
}
