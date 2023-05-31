using System.Collections;
using static System.Array;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.UI;
using UnityEngine.UIElements;
using TMPro;
using System.Threading.Tasks;


/*
 * @TODO: 
 * Display corresponding question text (in white panel on top of scene)
 * Display corresponding image/video for question (only if image or video based), also in white panel on top of scene
 * Randomly shuffle to next question on click of correct answer
 * */


public class QuestionLoader : MonoBehaviour
{
    // UI Components
    public VideoPlayer VideoPlayer;
    public UnityEngine.UI.Button Answer1;
    public UnityEngine.UI.Button Answer2;
    public UnityEngine.UI.Button Answer3;
    public UnityEngine.UI.Button Answer4;
    public TextMeshProUGUI VideoQuestionText;

    // JSON file reading
    [SerializeField] private TextAsset jsonFile;
    private List<Question> _questions = new List<Question>();
    private Question _currentQuestion;
    private List<int> _unencounteredQs;

    // Start is called before the first frame update
    void Start()
    {
        if (Globals.vocabJson)
        {
            jsonFile = Globals.vocabJson;
        }
        _questions = ReadFromFileJSON(jsonFile);
        _unencounteredQs = new List<int>();
        for (int i = 0; i < _questions.Count; i++) {
            _unencounteredQs.Add(i);
        }
        LoadRandomQuestion();
    }

    public void LoadRandomQuestion() {
        int randomIndex = GetRandomQuestionIndex();
        LoadQuestion(_questions[randomIndex]);
    }

    public int GetRandomQuestionIndex()
    {
        var questionListIndex = _unencounteredQs[Random.Range(0, _unencounteredQs.Count)];
        _unencounteredQs.Remove(questionListIndex);

        if (_unencounteredQs.Count == 0) {
            for (int i = 0; i < _questions.Count; i++) {
                _unencounteredQs.Add(i);
            }
        }
        return questionListIndex;
    }

    public void LoadQuestion(Question question)
    {
        _currentQuestion = question;
        RenderButtonText(question);
        RenderQuestionText(question);
        RenderVideo(question);
    }

    public void RenderQuestionText(Question question)
    {
        VideoQuestionText.text = "What sign does the video show?";
    }

    public void RenderButtonText(Question question)
    {
        List<Question> answersShuffled = GetRandomAnswers(_questions, _currentQuestion);
        Answer1.gameObject.GetComponent<ButtonHandler>().SetText(answersShuffled[0].Word);
        Answer2.gameObject.GetComponent<ButtonHandler>().SetText(answersShuffled[1].Word);
        Answer3.gameObject.GetComponent<ButtonHandler>().SetText(answersShuffled[2].Word);
        Answer4.gameObject.GetComponent<ButtonHandler>().SetText(answersShuffled[3].Word);
    }

    public void RenderVideo(Question question)
    {
        VideoPlayer.url = question.Link;
        VideoPlayer.Play();
    }

    public async void VideoLoadDelay()
    {
        // whatever you need to do before delay goes here         

        await Task.Delay(2000);

        // whatever you need to do after delay.
    }

    public bool AnswerIsCorrect(string answerText)
    {
        return _currentQuestion.Word.Equals(answerText);
    }

    //public static List<Question> shuffleAnswerList(List<Question> input) {
    //    string[] ret = new string[input.Length];
    //    System.Array.Copy(input, ret, input.Length);
    //    int n = input.Length;
    //    while (n > 1) {
    //        n --;
    //        int k = Random.Range(0, n);
    //        var tmp = ret[k];
    //        ret[k] = ret[n];
    //        ret[n] = tmp;
    //    }

    //    return ret;
    //}

    public List<Question> GetRandomAnswers(List<Question> inputList, Question correctAns)
    {
        List<Question> toReturn = new List<Question>();

        // Make list of 4 random Questions, including current one
        toReturn.Add(correctAns);
        inputList.Remove(correctAns);

        for (int i = 0; i < 3; i++)
        {
            int rndNum = Random.Range(0, inputList.Count);
            toReturn.Add(inputList[rndNum]);
            inputList.Remove(inputList[rndNum]);
        }

        // Randomize list
        for (int i = 0; i < toReturn.Count; i++)
        {
            Question temp = toReturn[i];
            int randomIndex = Random.Range(i, toReturn.Count);
            toReturn[i] = toReturn[randomIndex];
            toReturn[randomIndex] = temp;
        }

        return toReturn;
    }

    public List<Question> ReadFromFileJSON(TextAsset jsonFile)
    {
        // Each JSON file has a list of Questions -> read that list and place into a List<>
        Questions questionsjson = JsonUtility.FromJson<Questions>(jsonFile.text);
        List<Question> toReturn = new List<Question>();
        foreach (Question q in questionsjson.questions)
        {
            toReturn.Add(q);
        }
        return toReturn;
    }
}
