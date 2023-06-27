using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Student1Screen : MonoBehaviour
{
    public SceneInstantiator sceneInstantiator;
    public Student2Screen student2Screen;

    [SerializeField] private TextAsset biologyJson;
    [SerializeField] private TextAsset chemistryJson;

    public void Enable() {
        enabled = true;
        gameObject.GetComponent<Canvas>().enabled = true;
    }

    public void HandleBioButton() {
        sceneInstantiator.setVocabPath(biologyJson);
        sceneInstantiator.setVocabPath("Heredity");
        student2Screen.Enable();
        gameObject.GetComponent<Canvas>().enabled = false;
        enabled = false;
    }

    public void HandleChemButton() {
        sceneInstantiator.setVocabPath(chemistryJson);
        sceneInstantiator.setVocabPath("Chemistry");
        student2Screen.Enable();
        gameObject.GetComponent<Canvas>().enabled = false;
        enabled = false;
    }

    public void HandleFoodWebButton()
    {
        sceneInstantiator.setVocabPath("FoodWeb");
        student2Screen.Enable();
        gameObject.GetComponent<Canvas>().enabled = false;
        enabled = false;
    }

    public void HandlePartsOfTheCellButton()
    {
        sceneInstantiator.setVocabPath(chemistryJson);
        sceneInstantiator.setVocabPath("PartsOfTheCell");
        student2Screen.Enable();
        gameObject.GetComponent<Canvas>().enabled = false;
        enabled = false;
    }
}
