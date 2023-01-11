using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
public class TrackSaving : MonoBehaviour
{
    private TrackGeneratorConvex _Generator;

    [System.Serializable]
    public struct Point
    {
        public float x;
        public float y;
        public float z;
    }

    public void SaveTrack()
    {
        _Generator = FindObjectOfType<TrackGeneratorConvex>();
        LineRenderer line = _Generator.GetComponent<LineRenderer>();
        Vector3[] _ListSave = new Vector3[line.positionCount];
        line.GetPositions(_ListSave);

        Point[] _SavePoint = new Point[_ListSave.Length];
        
        for(int i = 0; i < _ListSave.Length; i++)
        {
            Point _pointAdd = new Point { x = _ListSave[i].x, y = _ListSave[i].y, z = _ListSave[i].z };
            _SavePoint[i] = _pointAdd;
        }

        string json = JsonHelper.ToJson(_SavePoint,true);
       
        string persistentPath = Application.persistentDataPath + "/points.json";
        if (File.Exists(persistentPath))
        {

            File.Delete(persistentPath);
            
        }
        File.WriteAllText(persistentPath, json);
    }

    public Vector3[] LoadTrack()
    {
        string persistentPath = Application.persistentDataPath + "/points.json";
        string jsonString = File.ReadAllText(persistentPath);
        Point[] _points = JsonHelper.FromJson<Point>(jsonString);
        Vector3[] _RenderTrack = new Vector3[_points.Length];
       for(int i = 0; i < _points.Length; i++)
        {
            _RenderTrack[i] = new Vector3(_points[i].x, _points[i].y, _points[i].z);
        }
        return _RenderTrack;
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
