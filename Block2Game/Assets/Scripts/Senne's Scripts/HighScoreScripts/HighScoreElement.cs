using System;

[Serializable]
public class HighScoreElement
{
    public string playerName;
    public float rawTime;
    public string timeString;

    //Constructer to prepare the data for a list 
    public HighScoreElement(string name, float time, string timeString)
    {
        playerName = name;
        this.rawTime = time;
        this.timeString = timeString;
    }
}
