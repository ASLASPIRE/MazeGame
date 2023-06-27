using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.VFX;
using static Globals;

public class MenuScreenManager : MonoBehaviour
{
    [SerializeField] private GameObject loadingPanel;
    [SerializeField] private Image loadingBarMask;
    [SerializeField] private ToggleGroup difficultyToggleGroup;
    [SerializeField] private ToggleGroup vocabToggleGroup;

    // Start is called before the first frame update
    private void Start()
    {
        loadingPanel.SetActive(false);
        Time.timeScale = 1.0f;
    }

    // Update is called once per frame
    private void Update()
    {

    }

    public void OnStartButtonClick(int sceneId)
    {
        string difficulty = difficultyToggleGroup.ActiveToggles().FirstOrDefault().ToString();
        if (CaseInsensitiveContains(difficulty, "easy"))
        {
            Globals.difficulty = Difficulty.Easy;
        } else if (CaseInsensitiveContains(difficulty, "medium"))
        {
            Globals.difficulty = Difficulty.Medium;
        } else if (CaseInsensitiveContains(difficulty, "hard"))
        {
            Globals.difficulty = Difficulty.Hard;
        } else
        {
            throw new System.Exception("Unknown difficulty selection, ensure name of toggle has difficulty written in it.");
        }

        string vocabSet = vocabToggleGroup.ActiveToggles().FirstOrDefault().ToString();
        if (CaseInsensitiveContains(vocabSet, "biology"))
        {
            Globals.vocabList = Vocab.Biology;
        } else if (CaseInsensitiveContains(vocabSet, "chemistry"))
        {
            Globals.vocabList = Vocab.Chemistry;
        } else if (CaseInsensitiveContains(vocabSet, "foodweb") || CaseInsensitiveContains(vocabSet, "food web"))
        {
            Globals.vocabList = Vocab.FoodWeb;
        } else if (CaseInsensitiveContains(vocabSet, "partsofthecell"))
        {
            Globals.vocabList = Vocab.PartsOfTheCell;
            Debug.Log("we did it joe");
        } else
        {
            throw new Exception("Unknown vocab set selection, ensure name of toggle has vocab set written in it.");
        }

        StartCoroutine(LoadSceneAsync(sceneId));
    }

    private IEnumerator LoadSceneAsync(int sceneId)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneId);
        loadingPanel.SetActive(true);

        while (!operation.isDone)
        {
            float progressValue = Mathf.Clamp01(operation.progress / 0.95f);
            loadingBarMask.fillAmount = progressValue;
            yield return null;
        }
    }

    private bool CaseInsensitiveContains(string source, string toCompare)
    {
        return source.IndexOf(toCompare, System.StringComparison.OrdinalIgnoreCase) >= 0;
    }
}
