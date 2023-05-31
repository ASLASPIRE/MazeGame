using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CountDownBar : MonoBehaviour
{
    public Transform fgBar;
    public float defaultInitialTime = 30.0f;
    public bool runOnStart = true;
    float initialTime;
    float timeRemaining;
    bool isRunning;
    // Start is called before the first frame update
    void Start()
    {
        initialTime = defaultInitialTime;
        timeRemaining = initialTime;
        isRunning = runOnStart;
    }

    // Update is called once per frame
    void Update()
    {
        if (isRunning) {
            timeRemaining = timeRemaining - Time.deltaTime;
            if (timeRemaining <= 0.0f) {
                timeRemaining = 0.0f;
                isRunning = false;
            }
            Resize();
        }
    }

    private void Resize() {
        float fullness = timeRemaining / initialTime;
        fgBar.localScale = new Vector3(fullness, fgBar.localScale.y, fgBar.localScale.z);
        fgBar.localPosition = new Vector3(-0.5f * (1.0f - fullness), fgBar.localPosition.y, fgBar.localPosition.z);
    }

    void SetTime(float remainingT) {
        if (remainingT >= 0.0f && remainingT <= initialTime) {
            timeRemaining = remainingT;
        }
    }

    void StartTimer() {
        isRunning = true;
    }

    void StopTimer() {
        isRunning = false;
    }

    void ResetTimer() {
        isRunning = false;
        timeRemaining = initialTime;
    }

    void ResetTimer(float initTime) {
        initialTime = initTime;
        ResetTimer();
    }
}
