using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LapTimerManager : MonoBehaviour
{
    public TextMeshProUGUI timeTxt;
    private string timeString;
    public string _TimeString
    {
        get => timeString;
    }
    private float startTime;
    private float rawTime;

    private bool started = false;

    private bool finished = false;

    public void StartTimer()
    {
        started = true;
        startTime = Time.time;
    }
    public void StopTimer()
    {
        finished = true;
    }
    public void RunTimer()
    {
        if (started)
            Timer();
    }
    void Timer()
    {
        if (finished)
            return;

        rawTime = Time.time - startTime;
        //Converts the time from an numer to actual minutes and seconds
        string minutes = ((int)rawTime / 60).ToString();
        string seconds = (rawTime % 60).ToString("f2");
        //need to save this text 
        timeTxt.text = minutes + "." + seconds;
        timeString = rawTime.ToString();
    }
}
