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
        lapTimesFloats.Sort((s1, s2) => s1._LapTime.CompareTo(s2._LapTime));

    }
    public void LogStats()
    {
        _Encrypt = new Encryption();
        LoadKey();
        string persistentPath = Application.persistentDataPath + "/Stats.json";
        if (File.Exists(persistentPath))
        {
            LoadStats();

        }
                
        var emailEncrypt = _Encrypt.EncryptEmail(email, key, iv);

        float timerCurrent = float.Parse(time);
        Score _pointAddCurrent = new Score { _Name = name, _LapTime = timerCurrent, _HashedEmail = emailEncrypt };
        lapTimesFloats.Add(_pointAddCurrent);

        SortTimes();

        Score[] _SaveScores = new Score[lapTimesFloats.Count];

        for (int i = 0; i < lapTimesFloats.Count; i++)
        {
            
            Score _pointAdd = new Score { _Name = lapTimesFloats[i]._Name, _LapTime = lapTimesFloats[i]._LapTime, _HashedEmail = lapTimesFloats[i]._HashedEmail };
            _SaveScores[i] = _pointAdd;
        }        

        string json = JsonHelper.ToJson(_SaveScores, true);
        File.WriteAllText(persistentPath, json);

        LoadStats();
    }

    public void LoadStats()
    {
        
        string persistentPath = Application.persistentDataPath + "/Stats.json";
        string jsonString = File.ReadAllText(persistentPath);

        Score[] _points = JsonHelper.FromJson<Score>(jsonString);

        lapTimesFloats = new List<Score>();
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
