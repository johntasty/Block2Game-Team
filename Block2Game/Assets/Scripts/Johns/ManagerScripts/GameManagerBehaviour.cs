using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class GameManagerBehaviour : MonoBehaviour
{
    LapTimerManager _TimerFunctions;
    ScoresClass _ScoreLogging;
    CreateUiTable _UiSpawner;
    [SerializeField] TMP_InputField _Username;
    [SerializeField] TMP_InputField _Email;

    [SerializeField] GameObject _LeaderBoard;
    [SerializeField] GameObject _DriverUi;
    [SerializeField] GameObject _SubmitPanel;

    [SerializeField] CarMovement _Player;
    List<float> lapTimesNum = new List<float>();

    RoadSpeed _RoadText;

    // Start is called before the first frame update
    void Start()
    {
        _ScoreLogging = new ScoresClass();
        _UiSpawner = GetComponent<CreateUiTable>();
        _RoadText = GetComponent<RoadSpeed>();
        _TimerFunctions = GetComponent<LapTimerManager>();
        _TimerFunctions.StartTimer();
    }

    // Update is called once per frame
    void Update()
    {
        _TimerFunctions.RunTimer();
    }
    private void FixedUpdate()
    {
        _RoadText.SetSpeed();
    }
    public void SubmitActive()
    {
        _TimerFunctions.StopTimer();
        
        _DriverUi.SetActive(false);
        _SubmitPanel.SetActive(true);
    }
    public void TestTimerLog()
    {
        _ScoreLogging._time = _TimerFunctions._TimeString;
        _ScoreLogging._name = _Username.text;
        _ScoreLogging._email = _Email.text;
        _ScoreLogging.LogStats();
       
        _LeaderBoard.SetActive(true);
        _SubmitPanel.SetActive(false);

        _Player.enabled = false;

        lapTimesNum = new List<float>();

        for (int i = 0; i < _ScoreLogging._LapTimesFloats.Count; i++)
        {
            string timerCurrent = ConvertRawTime(_ScoreLogging._LapTimesFloats[i]._LapTime);
            _UiSpawner.SpawnUi(_ScoreLogging._LapTimesFloats[i]._Name, timerCurrent);
            lapTimesNum.Add(_ScoreLogging._LapTimesFloats[i]._LapTime);
        }
       
    }

    string ConvertRawTime(float time)
    {
        string minutes = ((int)time / 60).ToString();
        string seconds = (time % 60).ToString("f2");
        //need to save this text 

        string timer = minutes + ":" + seconds;
        return timer;
    }
}
