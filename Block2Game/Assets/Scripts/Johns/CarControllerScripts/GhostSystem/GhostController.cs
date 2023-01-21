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
        transform.position = _Replay[0]._PlayerPos;
        GhostEngine = GetComponent<Engine>();
        GhostBody = GetComponent<Rigidbody>();
       
    }
    public void MoveGhost()
    {
        
        if (_index < _Replay.Count)
        {
           
            Vector3 direction = (_Target - transform.position).normalized;
            float dot = Vector3.Dot(transform.forward, direction);
            
            _Target = _Replay[_index]._PlayerPos;
            _Speed = _Replay[_index]._GasInput;
            transform.rotation = Quaternion.Slerp(transform.rotation, _Replay[_index]._PlayerRot, Time.fixedDeltaTime * _RotSpeed);
            
            GhostEngine.CarEngine(transform, GhostBody, _Speed,true);
            if (dot < -0.3f)
            {
                _index += 1;
            }
        }
     
    }
   
    void PopulateList()
    {
        _Replay = _LoadData.LoadLogs();
       
    }

}
