using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostController : MonoBehaviour
{
    GhostPosSaver _LoadData = new GhostPosSaver();
    Engine GhostEngine;
    Rigidbody GhostBody;

    List<GhostSturcts> _Replay = new List<GhostSturcts>();

    Vector3 _Target;
    float _Speed;

    public float _RotSpeed;

    int _index = 1;
   
    public void StartGhost()
    {
        PopulateList();
        if (_Replay == null) { enabled = false;  return; }
        transform.position = _Replay[0]._PlayerPos - Vector3.up;
        GhostEngine = GetComponent<Engine>();
        GhostBody = GetComponent<Rigidbody>();
       
    }
    public void MoveGhost()
    {
        // Check if the current index is within the bounds of the replay list
        if (_index < _Replay.Count)
        {
            // Calculate the direction to the target position
            Vector3 direction = (_Target - transform.position).normalized;
            // Calculate the dot product of the forward vector and the direction vector
            float dot = Vector3.Dot(transform.forward, direction);

            // Set the target position and speed from the replay data
            _Target = _Replay[_index]._PlayerPos;
            _Speed = _Replay[_index]._GasInput;
            // Rotate the ghost car towards the target rotation using Slerp
            transform.rotation = Quaternion.Slerp(transform.rotation, _Replay[_index]._PlayerRot, Time.fixedDeltaTime * _RotSpeed);

            // Move the ghost car using the CarEngine method, passing in the transform, rigidbody, speed and a flag for ghosting
            GhostEngine.CarEngine(transform, GhostBody, _Speed, true);
            // Check if the dot product is less than -0.3
            if (dot < -0.3f)
            {
                // If so, increment the index to move to the next replay data point
                _index += 1;
            }
        }

    }


    void PopulateList()
    {
        _Replay = _LoadData.LoadLogs();
       
    }

}
