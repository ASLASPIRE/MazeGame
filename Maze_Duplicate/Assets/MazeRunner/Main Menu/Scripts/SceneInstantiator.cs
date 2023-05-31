using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneInstantiator : MonoBehaviour
{
    private TextAsset _vocabJson;
    private string _vocabSet;
    private string _levelPath;
    private bool _loadReady = false;

    void Update() {
        if (_loadReady) {
            Load();
            _loadReady = false;
        }
    }

    public void setVocabPath(TextAsset p) {
        _vocabJson = p;
    }

    public void setVocabPath(string vocabSet)
    {
        _vocabSet = vocabSet;
    }

    public void setLevelPath(string p) {
        _levelPath = p;
    }

    public void TriggerLoad() {
        _loadReady = true;
    }

    private void Load() {
        Globals.vocabJson = _vocabJson;
        Globals.vocabSet = _vocabSet;
        DontDestroyOnLoad(gameObject);
        SceneManager.LoadScene(_levelPath);
    }
}
