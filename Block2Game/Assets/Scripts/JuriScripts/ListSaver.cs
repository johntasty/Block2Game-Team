using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
public class ListSaver : MonoBehaviour
{
    //I dont really understand the saving stuff so Ill try to convey what I want to do here 
    //OnTriggerEnter(FinishLine) -> if ghost.timeStamp.length < savedTimeStamp.length, call list saving function
    //list saving function: copy list from ListMaker to Saved Lists and save them 
    //list loading function, put saved lists into lists in this script on awake. idk if thats needed tho
    public ListMaker ghost;
    public ListFiller positions = null;
    public List<float> savedTimeStamp;
    public List<Vector3> savedPosition;
    public List<Vector3> savedRotation;
    public List<float> loadTimeStamp;
    public List<Vector3> loadPosition;
    public List<Vector3> loadRotation;

    private void OnTriggerEnter(Collider other)
    {
        positions =  FindObjectOfType<ListFiller>();

        savedPosition = positions.ghost.position;
        savedRotation = positions.ghost.rotation;
        savedTimeStamp = positions.ghost.timeStamp;
    }

    [System.Serializable]
    public struct Positions
    {
        public float savedTime;
        public Vector3 savedPosition;
        public Vector3 savedRotation;
    }

    public void SaveGhost()
    {
        positions = FindObjectOfType<ListFiller>();

        savedPosition = positions.ghost.position;
        savedRotation = positions.ghost.rotation;
        savedTimeStamp = positions.ghost.timeStamp;

        Positions[] _SavePoint = new Positions[savedTimeStamp.Count];

        for (int i = 0; i < savedTimeStamp.Count; i++)
        {
            Positions _pointAdd = new Positions { savedTime = savedTimeStamp[i], savedPosition = savedPosition[i], savedRotation = savedRotation[i] };
            _SavePoint[i] = _pointAdd;
        }

        string json = JsonHelper.ToJson(_SavePoint, true);

        string persistentPath = Application.persistentDataPath + "/GhostPositions.json";
        if (File.Exists(persistentPath))
        {

            File.Delete(persistentPath);

        }
        File.WriteAllText(persistentPath, json);
    }

    public void LoadPositionsGhost()
    {
        string persistentPath = Application.persistentDataPath + "/GhostPositions.json";
        if (!File.Exists(persistentPath)) return;
        
        string jsonString = File.ReadAllText(persistentPath);
        Positions[] _points = JsonHelper.FromJson<Positions>(jsonString);
        Vector3[] LoadSavedPosition = new Vector3[_points.Length];
        Vector3[] LoadSavedRotation = new Vector3[_points.Length];
        float[] LoadSavedTime = new float[_points.Length];
        for (int i = 0; i < _points.Length; i++)
        {
            LoadSavedRotation[i] = new Vector3(_points[i].savedRotation.x, _points[i].savedRotation.y, _points[i].savedRotation.z);
            LoadSavedPosition[i] = new Vector3(_points[i].savedPosition.x, _points[i].savedPosition.y, _points[i].savedPosition.z) ;
            LoadSavedTime[i] = _points[i].savedTime;
        }
        for(int y = 0; y < _points.Length; y++)
        {
            loadPosition.Add(LoadSavedPosition[y]);
            loadRotation.Add(LoadSavedRotation[y]);
            loadTimeStamp.Add(LoadSavedTime[y]);
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
    private void Awake()
    {
        LoadPositionsGhost();
    }
}
