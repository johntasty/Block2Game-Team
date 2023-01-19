using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class ListMaker : ScriptableObject
{
    public float frequency = 1;
    //creates the lists needed to make the ghost
    public List<float> timeStamp;
    public List<Vector3> position;
    public List<Vector3> rotation;

    //function to reset the lists
    public void ResetData()
    {
        timeStamp.Clear();
        position.Clear();
        rotation.Clear();
    }


}
