using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Student2Screen : MonoBehaviour
{
    public SceneInstantiator _sceneInstantiator;
    //public const string tetrisPath = "Tetris Clone/Pre-Game MC/Pre-Game MC";
    //public const string mazePath = "MazeGameFolder/MazeGame";
    public const string tetrisSceneName = "TetrisClone";
    public const string mazeSceneName = "MazeGame";

    public void Enable() {
        enabled = true;
        gameObject.GetComponent<Canvas>().enabled = true;
    }

    public void HandleTetrisButton() {
        _sceneInstantiator.setLevelPath(tetrisSceneName);
        _sceneInstantiator.TriggerLoad();
    }

    public void HandleMazeButton() {
        _sceneInstantiator.setLevelPath(mazeSceneName);
        _sceneInstantiator.TriggerLoad();
    }
}
