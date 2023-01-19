using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Camera))]
public class FloatingOrigin : MonoBehaviour
{
    [SerializeField]
    [Tooltip("Threshold for when the track gets reset to world origin")]
    private float threshold = 100.0f;
    [SerializeField]
    LevelLayoutGenerator layoutGenerator;

    private void FixedUpdate()
    {
        Vector3 cameraPosition = gameObject.transform.position;
        //maybe not necessary 
        cameraPosition.y = 0;

        //Make a counter of chunks instead of magnitude
        //Checks if the camera distance is greater then threshold
        if (cameraPosition.magnitude > threshold)
        {
            //Checks for all the scenes 
            for (int i = 0; i < SceneManager.sceneCount; i++)
            {
                //Loops over all the gameobjects in the scene and grabs only the roots of the objects
                foreach (GameObject g in SceneManager.GetSceneAt(i).GetRootGameObjects())
                {
                    g.transform.position -= cameraPosition;
                }
            }

            //Stores the change 
            Vector3 originDelta = Vector3.zero - cameraPosition;
            //Passes the value from above to the layout generator
            layoutGenerator.UpdateSpawnOrigin(originDelta);
            Debug.Log("Recentering, the origin delta is" + originDelta);
        }
    }
}
