using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using UnityEngine;
using UnityEngine.Video;
using static Globals;

public class VideoPlayerController : MonoBehaviour
{
    [SerializeField] private VideoPlayer videoPlayer;
    
    public Dictionary<string, string> VocabWordToPathDict = new Dictionary<string, string>();

    private void Awake()
    {
        //Debug.Log($"Thing to print: {vocabList.ToString()}");
        string path = Application.streamingAssetsPath + "/" + vocabList.ToString();
        GenerateVocabListFromVideos(path);
        videoPlayer.url = Application.streamingAssetsPath + "/Chemistry/Beaker.mp4";
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void GenerateVocabListFromVideos(string folderPath)
    {
        DirectoryInfo directory = new DirectoryInfo(folderPath);
        FileInfo[] files = directory.GetFiles("*.mp4");
        foreach (FileInfo file in files)
        {
            string processedName = file.Name.Trim().Replace(".mp4","").Replace("_"," ");
            TextInfo textInfo = new CultureInfo("en-US", false).TextInfo;
            textInfo.ToTitleCase(processedName);

            VocabWordToPathDict[processedName] = file.FullName;
        }
    }

    public void PlayVideo(string vocabWord)
    {
        Debug.Log($"playing vid: {vocabWord}");
        videoPlayer.url = VocabWordToPathDict[vocabWord];
        videoPlayer.Prepare();
        videoPlayer.Play();
        Debug.Log($"vid URL: {VocabWordToPathDict[vocabWord]}");
    }
}
