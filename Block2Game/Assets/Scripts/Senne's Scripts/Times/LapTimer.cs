using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class LapTimer : MonoBehaviour
{
    public TextMeshProUGUI timerText;
    private float startTime;

    bool started = false;
    bool finished = false;

    public GameObject startLine;
    public GameObject finishLine;
    public GameObject checkPoint;

    
    private void Start()
    {
        startLine = GameObject.FindGameObjectWithTag("startLine");
        finishLine = GameObject.FindGameObjectWithTag("finishLine");
        checkPoint = GameObject.FindGameObjectWithTag("checkPoint");
        finishLine.SetActive(false);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "startLine")
        {
            startTime = Time.time;
            started = true;
        }
        if (other.gameObject.tag == "checkPoint")
        {
            startLine.SetActive(false);
            finishLine.SetActive(true);
        }
        if (other.gameObject.tag == "finishLine")
        {
            finished = true;
            
            //stop race
            //save time
        }
        
    }

    private void Update()
    {
        if (started)
            Timer();
    }
    void Timer()
    {
        if (finished)
            return;
        
        float t = Time.time - startTime;
        string minutes = ((int)t / 60).ToString();
        string seconds = (t % 60).ToString("f2");
        timerText.text = minutes + ":" + seconds;
    }
}
