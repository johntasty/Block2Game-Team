using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class LapTimer : MonoBehaviour
{
    public TextMeshProUGUI timeTxt;
    public string timeString;
    private float startTime;
    public float rawTime;

    bool started = false;
    public bool finished = false;

    public GameObject startLine;
    public GameObject finishLine;
    public GameObject checkPoint;
    [SerializeField] GameHandler gameHandler;
    
    
    private void Start()
    {
        //Finds all the objects based on the tag
        startLine = GameObject.FindGameObjectWithTag("startLine");
        finishLine = GameObject.FindGameObjectWithTag("finishLine");
        checkPoint = GameObject.FindGameObjectWithTag("checkPoint");
        //Finish line needs to be active to be found so I turn it off after its found
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
            gameHandler.StopGame();
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

        rawTime = Time.time - startTime;
        //Converts the time from an numer to actual minutes and seconds
        string minutes = ((int)rawTime / 60).ToString();
        string seconds = (rawTime % 60).ToString("f2");
        //need to save this text 
        timeTxt.text = minutes + ":" + seconds;
        timeString = timeTxt.ToString();
        
    }
}
