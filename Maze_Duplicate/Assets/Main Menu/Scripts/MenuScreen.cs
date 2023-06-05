using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuScreen : MonoBehaviour
{
    public Student1Screen student1Screen;
    public TeacherScreen teacherScreen;

    public void Enable() {
        enabled = true;
        gameObject.GetComponent<Canvas>().enabled = true;
    }

    public void HandleStudentButton() {
        student1Screen.Enable();
        gameObject.GetComponent<Canvas>().enabled = false;
        enabled = false;
    }

    public void HandleTeacherButton() {
        gameObject.GetComponent<Canvas>().enabled = false;
        enabled = false;
    }
}
