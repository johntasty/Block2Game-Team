using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ListFiller : MonoBehaviour
{
    public ListMaker ghost;
    public ListSaver manager;
    private float timer;
    private float timeValue;

    //resets recording lists
    private void Awake()
    {
        ghost.ResetData();
        timeValue = 0;
        timer = 0;
    }
    //Update adds 1 position every second
    private void Update()
    {
        timer += Time.unscaledDeltaTime;
        timeValue += Time.unscaledDeltaTime;

        if (timer >= 1 / ghost.frequency)
        {
            ghost.timeStamp.Add(timeValue);
            ghost.position.Add(this.transform.position);
            ghost.rotation.Add(this.transform.eulerAngles);

            timer = 0;
        }
    }
    private void PositionsToManager()
    {
        manager.SaveGhost();
    }

    private void OnTriggerEnter(Collider other)
    {
        PositionsToManager();
    }
}
