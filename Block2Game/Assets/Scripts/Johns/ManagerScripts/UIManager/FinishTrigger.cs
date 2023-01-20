using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinishTrigger : MonoBehaviour
{
    GameManagerBehaviour manager;

    public GameManagerBehaviour _Manager
    {
        set => manager = value;
    }
    private void OnTriggerEnter(Collider other)
    {

        manager.SubmitActive();
    }
}
