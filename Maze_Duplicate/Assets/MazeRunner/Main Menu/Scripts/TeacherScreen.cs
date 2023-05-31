using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeacherScreen : MonoBehaviour
{
    void Enable() {
        enabled = true;
        gameObject.GetComponent<Canvas>().enabled = true;
    }
}
