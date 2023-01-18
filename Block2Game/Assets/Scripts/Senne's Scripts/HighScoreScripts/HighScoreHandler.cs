using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HighScoreHandler : MonoBehaviour
{
    List<HighScoreElement> highScoresList = new List<HighScoreElement>();
    [SerializeField] int maxCount = 6;
    [SerializeField] string filename;

    public delegate void OnHighScoreListChanged(List<HighScoreElement> list);
    public static event OnHighScoreListChanged onHighScoreListChanged;

    private void Start()
    {
        LoadHighScores();
    }
    private void LoadHighScores()
    {
        highScoresList = FileHandler.ReadFromJSON<HighScoreElement>(filename);

        while (highScoresList.Count > maxCount)
        {
            highScoresList.RemoveAt(maxCount);
        }

        if (onHighScoreListChanged != null)
        {
            onHighScoreListChanged.Invoke(highScoresList);
        }
    }
    private void SaveHighScore()
    {
        FileHandler.SaveToJSON<HighScoreElement>(highScoresList, filename);
    }

    //Prob need to change the parameter to InputEntry
    public void AddHighScoreIfPossible(HighScoreElement element)
    {
        for (int i = 0; i < maxCount; i++)
        {
            if (i >= highScoresList.Count || element.rawTime > highScoresList[i].rawTime) 
            {
                highScoresList.Insert(i, element);
                while (highScoresList.Count > maxCount)
                {
                    highScoresList.RemoveAt(maxCount);
                }
                SaveHighScore();
                Debug.Log("Score saved");

                if (onHighScoreListChanged != null)
                {
                    onHighScoreListChanged.Invoke(highScoresList);
                }

                break;
            }
        }
    }
}
