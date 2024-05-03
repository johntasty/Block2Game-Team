using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class GhostPosSaver 
{
    SteeringUiRotation pedalsHolderUi;

    [System.Serializable]
    public class GhostLog
    {
        public Vector3 _PlayerPos;
        public Quaternion _PlayerRot;
        public float _GasInput;
    }

    List<GhostSturcts> _GhostLogList = new List<GhostSturcts>();
    bool _Recording = true;

    public bool _recording
    {
        set => _Recording = value;
    }

    float _SaveStep = 0.1f;

    public float _saveStep
    {
        set => _SaveStep = value;
    }

    public IEnumerator PositionLogs(Transform _Player, SteeringUiRotation pedalsHolderUi)
    {
        InputController inputSystem = new InputController();
        inputSystem.pedalsHolder = pedalsHolderUi;
        do
        {
            GhostSturcts tempLog = new GhostSturcts { _PlayerPos = _Player.position, _PlayerRot = _Player.rotation, _GasInput = inputSystem.CarPedalInput()};
            _GhostLogList.Add(tempLog);
            yield return new WaitForSeconds(_SaveStep);


        } while (_Recording);

    }

    public void SaveLogs()
    {
        GhostSturcts[] _SaveGhostReplay = new GhostSturcts[_GhostLogList.Count];

        for(int i = 0; i < _GhostLogList.Count; i++)
        {
            _SaveGhostReplay[i] = _GhostLogList[i];
        }

        string persistentPath = Application.persistentDataPath + "/GhostLog.json";
        string json = JsonHelper.ToJson(_SaveGhostReplay, true);
        File.WriteAllText(persistentPath, json);
    }

    public List<GhostSturcts> LoadLogs()
    {
        string persistentPath = Application.persistentDataPath + "/GhostLog.json";
        if (!File.Exists(persistentPath)) { return null; }
        List<GhostSturcts> _Replay = new List<GhostSturcts>();

        
        string jsonString = File.ReadAllText(persistentPath);

        GhostSturcts[] _ReplayPoints = JsonHelper.FromJson<GhostSturcts>(jsonString);

        foreach(GhostSturcts point in _ReplayPoints)
        {
            _Replay.Add(point);
        }

        return _Replay;
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
}
