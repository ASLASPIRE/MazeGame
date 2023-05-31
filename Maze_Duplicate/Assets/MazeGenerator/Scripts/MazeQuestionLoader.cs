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


public class MazeQuestionLoader : MonoBehaviour
{
    // UI Components
    public VideoPlayer VideoPlayer;
    public Animator anim;
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

    [SerializeField] private GIFController gifController;
    private string vocabSet;
    private List<RuntimeAnimatorController> vocabSetList;
    private string _currentWord;
    private RuntimeAnimatorController _currentController;

    // Start is called before the first frame update
    void Start()
    {
        if (Globals.vocabJson)
        {
            jsonFile = Globals.vocabJson;
        }
        if (Globals.vocabSet != null)
        {
            vocabSet = Globals.vocabSet;
        }
        gifController.UpdateVocabSet(vocabSet);
        vocabSetList = gifController.currentVocabList;
        Debug.Log($"vocabSetList Size = {vocabSetList.Count}");
        _questions = ReadFromFileJSON(jsonFile);
        _unencounteredQs = new List<int>();
        for (int i = 0; i < vocabSetList.Count; i++)
        {
            _unencounteredQs.Add(i);
        }
        LoadRandomQuestion();
    }

    public void LoadRandomQuestion()
    {
        int randomIndex = GetRandomQuestionIndex();
        //LoadQuestion(_questions[randomIndex]);
        LoadWord(vocabSetList[randomIndex]);
    }

    public int GetRandomQuestionIndex()
    {
        var questionListIndex = _unencounteredQs[Random.Range(0, _unencounteredQs.Count)];
        _unencounteredQs.Remove(questionListIndex);

        if (_unencounteredQs.Count == 0)
        {
            for (int i = 0; i < vocabSetList.Count; i++)
            {
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

    public void LoadWord(RuntimeAnimatorController word)
    {
        _currentWord = word.name;
        _currentController = word;
        RenderButtonText(word);
        RenderQuestionText(word);
        RenderGIF(word);

    }

    public void RenderQuestionText(Question question)
    {
        VideoQuestionText.text = "What sign does the video show?";
    }

    public void RenderQuestionText(RuntimeAnimatorController controller)
    {
        VideoQuestionText.text = "What sign does the video show?";
    }

    public void RenderButtonText(Question question)
    {
        List<Question> answersShuffled = GetRandomAnswers(_questions, _currentQuestion);
        Debug.Log($"toReturn = {answersShuffled.Count}");
        Debug.Log($"first word = {answersShuffled[0].Word}");
        Answer1.gameObject.GetComponent<MazeButtonHandler>().SetText(answersShuffled[0].Word);
        Answer2.gameObject.GetComponent<MazeButtonHandler>().SetText(answersShuffled[1].Word);
        Answer3.gameObject.GetComponent<MazeButtonHandler>().SetText(answersShuffled[2].Word);
        Answer4.gameObject.GetComponent<MazeButtonHandler>().SetText(answersShuffled[3].Word);
    }

    public void RenderButtonText(RuntimeAnimatorController controller)
    {
        List<string> answersShuffled = GetRandomAnswers(vocabSetList, _currentWord);
        Answer1.gameObject.GetComponent<MazeButtonHandler>().SetText(answersShuffled[0]);
        Answer2.gameObject.GetComponent<MazeButtonHandler>().SetText(answersShuffled[1]);
        Answer3.gameObject.GetComponent<MazeButtonHandler>().SetText(answersShuffled[2]);
        Answer4.gameObject.GetComponent<MazeButtonHandler>().SetText(answersShuffled[3]);
    }

    public void RenderVideo(Question question)
    {
        VideoPlayer.url = question.Link;
        VideoPlayer.Play();
    }

    public void RenderGIF(RuntimeAnimatorController controller)
    {
        Debug.Log($"_currentWord = {_currentWord}");
        gifController.ChangeAnimationState(controller);
    }

    public async void VideoLoadDelay()
    {
        // whatever you need to do before delay goes here         

        await Task.Delay(2000);

        // whatever you need to do after delay.
    }

    public bool AnswerIsCorrect(string answerText)
    {
        //return _currentQuestion.Word.Equals(answerText);
        return _currentWord.Equals(answerText);
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

        Debug.Log($"toReturn = {toReturn.Count}");
        // Randomize list
        for (int i = 0; i < toReturn.Count; i++)
        {
            Question temp = toReturn[i];
            int randomIndex = Random.Range(i, toReturn.Count);
            toReturn[i] = toReturn[randomIndex];
            toReturn[randomIndex] = temp;
        }

        Debug.Log($"toReturn2 = {toReturn.Count}");
        return toReturn;
    }

    public List<string> GetRandomAnswers(List<RuntimeAnimatorController> controllerList, string correctAns)
    {
        List<string> toReturn = new List<string>();

        // Make list of 4 random Questions, including current one
        List<string> inputList = new List<string>();
        Debug.Log($"controllerList size = {controllerList.Count}");
        for (int i = 0; i < controllerList.Count; i++)
        {
            inputList.Add(controllerList[i].name);
        }
        toReturn.Add(correctAns);
        inputList.Remove(correctAns);

        for (int i = 0; i < 3; i++)
        {
            int rndNum = Random.Range(0, inputList.Count);
            toReturn.Add(inputList[rndNum]);
            inputList.Remove(inputList[rndNum]);
        }

        Debug.Log($"toReturn = {toReturn.Count}");
        // Randomize list
        for (int i = 0; i < toReturn.Count; i++)
        {
            string temp = toReturn[i];
            int randomIndex = Random.Range(i, toReturn.Count);
            toReturn[i] = toReturn[randomIndex];
            toReturn[randomIndex] = temp;
        }

        Debug.Log($"toReturn2 = {toReturn.Count}");
        return toReturn;
    }

    public List<Question> ReadFromFileJSON(TextAsset jsonFile)
    {
        // Each JSON file has a list of Questions -> read that list and place into a List<>
        //Debug.Log(jsonFile);
        Questions questionsjson = JsonUtility.FromJson<Questions>(jsonFile.text);
        //Debug.Log($"thing = {questionsjson.questions.Length}");
        List<Question> toReturn = new List<Question>();
        foreach (Question q in questionsjson.questions)
        {
            toReturn.Add(q);
        }
        return toReturn;
    }
}
