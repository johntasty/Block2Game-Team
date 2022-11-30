using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerExit : MonoBehaviour
{
    [SerializeField]
    [Tooltip("Amount of time in seconds for road blocks to despawn")]
    private float delay = 30f;

    public delegate void ExitAction();
    public static event ExitAction OnChunkExited;

    private bool exited = false;

    //When a collider leaves the trigger
    private void OnTriggerExit(Collider other)
    {
        //Cartag is an empty script attached to the car 
        CarTag carTag = other.GetComponent<CarTag>();
        if (carTag != null)
        {
            if(!exited)
            {
                exited = true;
                //Raise an event that can be detected by things that are subscribed to it
                OnChunkExited();
                StartCoroutine(WaitAndDeactivate());
            }
        }
    }

    IEnumerator WaitAndDeactivate()
    {
        //Waits for 30 seconds 
        yield return new WaitForSeconds(delay);
        //After the wait executes this below
        transform.root.gameObject.SetActive(false);
    }
}
