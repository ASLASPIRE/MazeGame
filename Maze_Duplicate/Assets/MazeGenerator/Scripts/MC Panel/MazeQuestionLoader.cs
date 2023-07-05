using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.UI;
using TMPro;
using System.Threading.Tasks;

public class MazeQuestionLoader : MonoBehaviour
{
    // UI Components
    [Header("UI components")]
    public TextMeshProUGUI VideoQuestionText;
    public VideoPlayerController VideoPlayerController;
    public Button Button1;
    public Button Button2;
    public Button Button3;
    public Button Button4;
    //public Animator animator;

    // Datasets
    [Header("Datasets")]
    [SerializeField] private TextAsset jsonFile;
    [SerializeField] private GIFController gifController;

    // JSON file reading
    //private List<Question> _questions = new List<Question>();
    //private Question _currentQuestion;
    private List<int> _unencounteredQs;

    // Private vars
    private string vocabSet; // name of vocab set we're using
    private List<RuntimeAnimatorController> vocabSetList; // list of animations for each vocab word
    private string _currentWord; // the current word that's being asked
    private RuntimeAnimatorController _currentController; // the current animation for the word being asked
    private List<string> vidVocabList;

    // Start is called before the first frame update
    void Start()
    {
        if (Globals.vocabJson)
        {
            jsonFile = Globals.vocabJson;
        }

        // Update the vocab set name and contents depending on value set by player in Globals
        // gifController.UpdateVocabSet(vocabSet);
        // vocabSetList = gifController.currentVocabList;
        // Debug.Log($"vocabSetList Size = {vocabSetList.Count}");
        // _questions = ReadFromFileJSON(jsonFile);
        // _unencounteredQs = new List<int>();
        // for (int i = 0; i < vocabSetList.Count; i++)
        // {
        //     _unencounteredQs.Add(i);
        // }
        // LoadRandomQuestion();



        vidVocabList = VideoPlayerController.VocabWordToPathDict.Keys.ToList();
        Debug.Log($"vidVocabList Size = {vidVocabList.Count}");
        LoadRandomQuestion();
    }

    /// <summary>
    /// Loads a question and 4 answers into the panel's contents
    /// </summary>
    public void LoadRandomQuestion()
    {
        int randomIndex = GetRandomQuestionIndex();
        //LoadQuestion(_questions[randomIndex]);
        //LoadWord(vocabSetList[randomIndex]);
        LoadWord(vidVocabList[randomIndex]);
    }

    /// <summary>
    /// Get a random question index
    /// </summary>
    /// <returns>Returns an int corresponding to an array index</returns>
    public int GetRandomQuestionIndex()
    {
        // var questionListIndex = _unencounteredQs[Random.Range(0, _unencounteredQs.Count)];
        // _unencounteredQs.Remove(questionListIndex);

        // if (_unencounteredQs.Count == 0)
        // {
        //     for (int i = 0; i < vocabSetList.Count; i++)
        //     {
        //         _unencounteredQs.Add(i);
        //     }
        // }
        // return questionListIndex;

        return Random.Range(0, vidVocabList.Count);
    }

    // public void LoadQuestion(Question question)
    // {
    //     _currentQuestion = question;
    //     RenderButtonText(question);
    //     RenderQuestionText(question);
    //     RenderVideo(question);
    // }

    public void LoadWord(RuntimeAnimatorController word)
    {
        _currentWord = word.name;
        _currentController = word;
        RenderButtonText(word);
        RenderQuestionText(word);
        RenderGIF(word);
    }

    /// <summary>
    /// Loads the question/word into the videoplayer and buttons
    /// </summary>
    /// <param name="word">Word the panel is asking about</param>
    public void LoadWord(string word)
    {
        _currentWord = word;
        RenderButtonText(word);
        RenderQuestionText(word);
        RenderVideo(word);
    }

    // public void RenderQuestionText(Question question)
    // {
    //     VideoQuestionText.text = "What sign does the video show?";
    // }

    /// <summary>
    /// Renders question text
    /// </summary>
    /// <param name="question">Question we're asking</param>
    public void RenderQuestionText(string question)
    {
        VideoQuestionText.text = "What sign does the video show?";
    }

    public void RenderQuestionText(RuntimeAnimatorController controller)
    {
        VideoQuestionText.text = "What sign does the video show?";
    }

    // public void RenderButtonText(Question question)
    // {
    //     List<Question> answersShuffled = GetRandomAnswers(_questions, _currentQuestion);
    //     Debug.Log($"toReturn = {answersShuffled.Count}");
    //     Debug.Log($"first word = {answersShuffled[0].Word}");
    //     Button1.gameObject.GetComponent<MazeButtonHandler>().SetText(answersShuffled[0].Word);
    //     Button2.gameObject.GetComponent<MazeButtonHandler>().SetText(answersShuffled[1].Word);
    //     Button3.gameObject.GetComponent<MazeButtonHandler>().SetText(answersShuffled[2].Word);
    //     Button4.gameObject.GetComponent<MazeButtonHandler>().SetText(answersShuffled[3].Word);
    // }

    /// <summary>
    /// Creates 4 random answers, one of them being correct, and assigns them to random buttons
    /// </summary>
    /// <param name="question">Word we're asking about</param>
    public void RenderButtonText(string question)
    {
        List<string> answersShuffled = GetRandomAnswers(vidVocabList, _currentWord);
        Debug.Log($"toReturn = {answersShuffled.Count}");
        Debug.Log($"first word = {answersShuffled[0]}");
        Button1.gameObject.GetComponent<MazeButtonHandler>().SetText(answersShuffled[0]);
        Button2.gameObject.GetComponent<MazeButtonHandler>().SetText(answersShuffled[1]);
        Button3.gameObject.GetComponent<MazeButtonHandler>().SetText(answersShuffled[2]);
        Button4.gameObject.GetComponent<MazeButtonHandler>().SetText(answersShuffled[3]);
    }

    public void RenderButtonText(RuntimeAnimatorController controller)
    {
        List<string> answersShuffled = GetRandomAnswers(vocabSetList, _currentWord);
        Button1.gameObject.GetComponent<MazeButtonHandler>().SetText(answersShuffled[0]);
        Button2.gameObject.GetComponent<MazeButtonHandler>().SetText(answersShuffled[1]);
        Button3.gameObject.GetComponent<MazeButtonHandler>().SetText(answersShuffled[2]);
        Button4.gameObject.GetComponent<MazeButtonHandler>().SetText(answersShuffled[3]);
    }

    // public void RenderVideo(Question question)
    // {
    //     //VideoPlayer.url = question.Link;
    //     //VideoPlayer.Play();
    // }

    /// <summary>
    /// Render the video associated with the question we're asking
    /// </summary>
    /// <param name="word">Word corresponding to title/sign of video</param>
    public void RenderVideo(string word)
    {
        //VideoPlayer.url = question.Link;
        //VideoPlayer.Play();
        VideoPlayerController.PlayVideo(word);
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

    /// <summary>
    /// Checks if the button selection matches the correct answer shown in the video
    /// </summary>
    /// <param name="answerText">The text of the button selected</param>
    /// <returns>Returns a bool corresponding to whether that selection was correct</returns>
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

    // public List<Question> GetRandomAnswers(List<Question> inputList, Question correctAns)
    // {
    //     List<Question> toReturn = new List<Question>();

    //     // Make list of 4 random Questions, including current one
    //     toReturn.Add(correctAns);
    //     inputList.Remove(correctAns);

    //     for (int i = 0; i < 3; i++)
    //     {
    //         int rndNum = Random.Range(0, inputList.Count);
    //         toReturn.Add(inputList[rndNum]);
    //         inputList.Remove(inputList[rndNum]);
    //     }

    //     Debug.Log($"toReturn = {toReturn.Count}");
    //     // Randomize list
    //     for (int i = 0; i < toReturn.Count; i++)
    //     {
    //         Question temp = toReturn[i];
    //         int randomIndex = Random.Range(i, toReturn.Count);
    //         toReturn[i] = toReturn[randomIndex];
    //         toReturn[randomIndex] = temp;
    //     }

    //     Debug.Log($"toReturn2 = {toReturn.Count}");
    //     return toReturn;
    // }

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

    /// <summary>
    /// Takes a list of vocab words and randomly selects 4 words, one of them being the correct answer
    /// </summary>
    /// <param name="vocabList">List of vocab words</param>
    /// <param name="correctAns">The correct vocab word</param>
    /// <returns>Returns a size 4 array of vocab words, one of them being correct</returns>
    public List<string> GetRandomAnswers(List<string> vocabList, string correctAns)
    {
        List<string> toReturn = new List<string>();

        // Make list of 4 random words, including current one
        List<string> inputList = new List<string>();
        Debug.Log($"controllerList size = {vocabList.Count}");
        for (int i = 0; i < vocabList.Count; i++)
        {
            inputList.Add(vocabList[i]);
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

    // public List<Question> ReadFromFileJSON(TextAsset jsonFile)
    // {
    //     // Each JSON file has a list of Questions -> read that list and place into a List<>
    //     //Debug.Log(jsonFile);
    //     Questions questionsjson = JsonUtility.FromJson<Questions>(jsonFile.text);
    //     //Debug.Log($"thing = {questionsjson.questions.Length}");
    //     List<Question> toReturn = new List<Question>();
    //     foreach (Question q in questionsjson.questions)
    //     {
    //         toReturn.Add(q);
    //     }
    //     return toReturn;
    // }
}
