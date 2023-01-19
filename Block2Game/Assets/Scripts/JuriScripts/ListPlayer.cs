using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ListPlayer : MonoBehaviour
{
    //instead of taking the data from the list maker the list player should take it from the list saver
    public ListSaver ghost;
    private float timeValue;
    private int index1;
    private int index2;

    private void Awake()
    {
        timeValue = 0;

    }

    private void Update()
    {
        timeValue += Time.unscaledDeltaTime;

        GetIndex();
        SetTransform();
    }

    private void GetIndex()
    {
        for (int i = 0; i < ghost.loadTimeStamp.Count - 2; i++)
        {
            if (i > ghost.loadTimeStamp.Count) return;
            if (ghost.loadTimeStamp[i] == timeValue)
            {
                index1 = i;
                index2 = i;
                return;
            }
            else if (ghost.loadTimeStamp[i] < timeValue & timeValue < ghost.loadTimeStamp[i + 1])
            {
                index1 = i;
                index2 = i + 1;
                return;
            }
        }
        index1 = ghost.savedTimeStamp.Count - 1;
        index2 = ghost.savedTimeStamp.Count - 1;
    }

    private void SetTransform()
    {
        if (index1 < 0) return;
        if (index1 == index2)
        {
            Debug.Log(index1);
            this.transform.position = ghost.loadPosition[index1];
            this.transform.eulerAngles = ghost.loadRotation[index1];
        }
        else
        {
            float interpolationFactor = (timeValue - ghost.loadTimeStamp[index1]) / (ghost.loadTimeStamp[index2] - ghost.loadTimeStamp[index1]);
            Vector3 direction = (ghost.loadRotation[index2] - ghost.loadRotation[index1]);
            Quaternion rotation = Quaternion.LookRotation(direction, Vector3.up);
            this.transform.position = Vector3.Lerp(ghost.loadPosition[index1], ghost.loadPosition[index2], interpolationFactor);
            this.transform.rotation = Quaternion.Slerp(this.transform.rotation, Quaternion.Euler(0, rotation.y, rotation.z), interpolationFactor);
        }
    }


}
