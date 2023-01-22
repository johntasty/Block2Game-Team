using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.IO;

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
    [SerializeField] Transform _PlayerCar;
    [TextArea(5, 10)]
    [SerializeField] List<string> _Facts = new List<string>();
    public List<string> _facts
    {
        get => _Facts;
    }
    List<float> lapTimesNum = new List<float>();

    RoadSpeed _RoadText;
    Speedometer _SpeedometerFunc;

    GhostPosSaver _GhostLogger;
    GhostController _GhostManager;
    
    private SteeringUiRotation pedalsHolderUi;

    // Start is called before the first frame update
    void Start()
    {
        _ScoreLogging = new ScoresClass();
        _GhostLogger = new GhostPosSaver();
        _UiSpawner = GetComponent<CreateUiTable>();
        _RoadText = GetComponent<RoadSpeed>();
        _SpeedometerFunc = GetComponent<Speedometer>();
        _TimerFunctions = GetComponent<LapTimerManager>();
        
        _GhostManager = FindObjectOfType<GhostController>();
        _TimerFunctions.StartTimer();
        _GhostManager.StartGhost();
        pedalsHolderUi = FindObjectOfType<SteeringUiRotation>();

        StartCoroutine( _GhostLogger.PositionLogs(_PlayerCar, pedalsHolderUi));
    }

    // Update is called once per frame
    void Update()
    {
        _TimerFunctions.RunTimer();
        _SpeedometerFunc.SpeedometerBar();
     
    }
    private void FixedUpdate()
    {
        _RoadText.SetSpeed();
        if (_GhostManager.enabled)
        {
            _GhostManager.MoveGhost();
        }
       
    }
    public void SubmitActive()
    {
        _TimerFunctions.StopTimer();
        _GhostLogger._recording = false;
                
        _DriverUi.SetActive(false);
        _SubmitPanel.SetActive(true);
        _Player.enabled = false;
    }
    public void TestTimerLog()
    {
       
        _ScoreLogging._time = _TimerFunctions._TimeString;
        _ScoreLogging._name = _Username.text;
        _ScoreLogging._email = _Email.text;

        _ScoreLogging.LogStats();

        _LeaderBoard.SetActive(true);
        _SubmitPanel.SetActive(false);
        

        lapTimesNum = new List<float>();

        for (int i = 0; i < _ScoreLogging._LapTimesFloats.Count; i++)
        {
            string timerCurrent = ConvertRawTime(_ScoreLogging._LapTimesFloats[i]._LapTime);
            _UiSpawner.SpawnUi(_ScoreLogging._LapTimesFloats[i]._Name, timerCurrent);
            lapTimesNum.Add(_ScoreLogging._LapTimesFloats[i]._LapTime);
        }
       
        if (_ScoreLogging._LapTimesFloats.Count == 1)
        {
           
            _GhostLogger.SaveLogs();
        }
       
        float timerCurrentPlayer = float.Parse(_TimerFunctions._TimeString);
        
      if(timerCurrentPlayer > _ScoreLogging._LapTimesFloats[0]._LapTime)
        {
            Debug.Log("Slow");
            
        }
      else
        {
            _GhostLogger.SaveLogs();
          
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
