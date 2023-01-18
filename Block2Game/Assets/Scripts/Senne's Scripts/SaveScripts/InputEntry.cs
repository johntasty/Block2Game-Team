using System;

[Serializable]
public class InputEntry
{
    public string playerName;
    public int time;

    public InputEntry (string name, int time)
    {
        playerName = name;
        this.time = time;
    }
}
