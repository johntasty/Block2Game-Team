using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    public List<Vector3> _positions = new List<Vector3>();
    public GameObject player;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        _positions.Add(transform.position);
        //Debug.Log(_positions[0]);
        if(_positions.Count > 3)
        {
            _positions.RemoveAt(0);
            //print ("swagswagswag");
        }
    }
}
