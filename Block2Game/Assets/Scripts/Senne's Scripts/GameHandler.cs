using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class GameHandler : MonoBehaviour
{
    [SerializeField] LapTimer lapTimer;
    [SerializeField] HighScoreHandler highScoreHandler;
    [SerializeField] TMP_InputField nameInput;



    public void StopGame()
    {
        //Pauses the gameplay
        Time.timeScale = 0f;
        //Adds the score to the list if it is possible 
        highScoreHandler.AddHighScoreIfPossible(new HighScoreElement(nameInput.text, lapTimer.rawTime, lapTimer.timeString));
        //Pull up ui 
        //update highscore list 
    }
}
