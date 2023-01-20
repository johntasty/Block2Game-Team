using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LogScore : MonoBehaviour
{
    private string _LoggedTimed;

    public void LogTime()
    {
        GetComponent<LapTimerManager>().StopTimer();
        _LoggedTimed = GetComponent<LapTimerManager>()._TimeString;
    }
}
