using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinishLine : MonoBehaviour
{
    [SerializeField] GameObject _FinishFlag;
    GameManagerBehaviour manager;
    public void AddFinishLine(Vector3 end, Vector3 look)
    {
        GameObject _FinishLine = Instantiate(_FinishFlag);
        _FinishLine.transform.position = end + (Vector3.up * 5f);

        Vector3 dir = look - end;

        _FinishLine.transform.rotation = Quaternion.LookRotation(dir, Vector3.up);

        manager = FindObjectOfType<GameManagerBehaviour>();
        _FinishLine.transform.GetComponent<FinishTrigger>()._Manager = manager;
    }

   
}
