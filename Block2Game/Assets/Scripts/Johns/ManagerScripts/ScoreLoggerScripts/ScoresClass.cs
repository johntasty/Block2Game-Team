using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Globalization;
using System;

public class ScoresClass 
{
    private Encryption _Encrypt;
    List<Score> lapTimesFloats = new List<Score>();

    byte[] key;
    byte[] iv;
    public List<Score> _LapTimesFloats
    {
        get => lapTimesFloats;
    }
    private string email;
    public string _email
    {
        get => email;

        set => email = value;
    }

    private string name;
    public string _name
    {
        get => name;

        set => name = value;
    }
    private string time;
    public string _time
    {
        get => time;

        set => time = value;
    }
    [System.Serializable]
    public struct Score
    {        
        public string _Name;
        public float _LapTime;
        public string _HashedEmail;

    }

    private void SortTimes()
    {
        // Sort the lap times list in ascending order by lap time
        lapTimesFloats.Sort((s1, s2) => s1._LapTime.CompareTo(s2._LapTime));
    }

    public void LogStats()
    {
        // Initialize the encryption class
        _Encrypt = new Encryption();
        // Load the encryption key
        LoadKey();
        // Define the path for the stats file
        string persistentPath = Application.persistentDataPath + "/Stats.json";
        // Check if the file exists
        if (File.Exists(persistentPath))
        {
            // If so, load the existing stats
            LoadStats();
        }
        // Encrypt the email
        var emailEncrypt = _Encrypt.EncryptEmail(email, key, iv);
        // Convert the time string to a float
        float timerCurrent = float.Parse(time);
        // Create a new Score object with the current name, time, and encrypted email
        Score _pointAddCurrent = new Score { _Name = name, _LapTime = timerCurrent, _HashedEmail = emailEncrypt };
        // Add the new Score object to the lap times list
        lapTimesFloats.Add(_pointAddCurrent);
        // Sort the lap times list
        SortTimes();        
        // Create an array to hold the sorted lap times
        Score[] _SaveScores = new Score[lapTimesFloats.Count];
        // Copy the lap times from the list to the array
        for (int i = 0; i < lapTimesFloats.Count; i++)
        {
            Score _pointAdd = new Score { _Name = lapTimesFloats[i]._Name, _LapTime = lapTimesFloats[i]._LapTime, _HashedEmail = lapTimesFloats[i]._HashedEmail };
            _SaveScores[i] = _pointAdd;
        }
        // Convert the array to a JSON string
        string json = JsonHelper.ToJson(_SaveScores, true);
        // Write the JSON string to the stats file
        File.WriteAllText(persistentPath, json);
        // Reload the stats from the file
        LoadStats();
    }

    public void LoadStats()
    {
        // Define the path for the stats file
        string persistentPath = Application.persistentDataPath + "/Stats.json";
        // Read the contents of the file into a string
        string jsonString = File.ReadAllText(persistentPath);
        // Convert the JSON string to an array of Score objects
        Score[] _points = JsonHelper.FromJson<Score>(jsonString);
        // Initialize the lap times list
        lapTimesFloats = new List<Score>();
        // Copy the data from the array to the list
        for (int i = 0; i < _points.Length; i++)
        {
            lapTimesFloats.Add(_points[i]);
        }
    }

    public static class JsonHelper
    {
        public static T[] FromJson<T>(string json)
        {
            Wrapper<T> wrapper = JsonUtility.FromJson<Wrapper<T>>(json);
            return wrapper.Items;
        }

        public static string ToJson<T>(T[] array)
        {
            Wrapper<T> wrapper = new Wrapper<T>();
            wrapper.Items = array;
            return JsonUtility.ToJson(wrapper);
        }

        public static string ToJson<T>(T[] array, bool prettyPrint)
        {
            Wrapper<T> wrapper = new Wrapper<T>();
            wrapper.Items = array;
            return JsonUtility.ToJson(wrapper, prettyPrint);
        }

        [System.Serializable]
        private class Wrapper<T>
        {
            public T[] Items;
        }
    }

    private void LoadKey()
    {
        string json = File.ReadAllText(Application.persistentDataPath + "/admin.json");

        // Convert the JSON string to an object
        var passwordData = JsonUtility.FromJson<PasswordData>(json);
               
        key = Convert.FromBase64String(passwordData.key);
        iv = Convert.FromBase64String(passwordData.iv);
    }

    [Serializable]
    public class PasswordData
    {
        public string username;
        public string hashedPassword;
        public string salt;

        public string key;
        public string iv;
    }
}
