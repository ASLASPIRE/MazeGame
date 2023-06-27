using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HomeScreen : MonoBehaviour
{
    public MenuScreen menuScreen;

    // Update is called once per frame
    void Update()
    {
        //var keyPressed = Keyboard.current.anyKey.wasPressedThisFrame;
        var keyPressed = Input.anyKey;

        if (keyPressed)
        {
            menuScreen.Enable();
            gameObject.GetComponent<Canvas>().enabled = false;
            enabled = false;
        }
    }
}
