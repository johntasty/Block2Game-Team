using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField]
    private float lerpTime = 3.5f;
    [SerializeField, Range(1f, 4f)]
    private float forwardDistance = 3f;
    [SerializeField]
    private float distance = 2f;

    private GameObject player;
    [SerializeField]
    private int locationIndicator = 1;
    private CarControllerV2 carControllerV2;

    private Vector3 newPos;
    private Transform target;
    private GameObject focusPoint;
    private float accelerationEffect;

    public Vector2[] cameraPos;

    private void Start()
    {
        

        player = GameObject.FindGameObjectWithTag("Player");
        focusPoint = player.transform.Find("Focus").gameObject;

        target = focusPoint.transform;
        carControllerV2 = player.GetComponent<CarControllerV2>();

    }

    private void FixedUpdate()
    {
        CamFollow();
        
    }
    private void CycleCamera()
    {
        if (locationIndicator >= cameraPos.Length -1 || locationIndicator < 0)
            locationIndicator = 0;
        else
            locationIndicator++;
    }

    public void CamFollow()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            CycleCamera();
        }

        //Inidicates the position of the camera and sets the offset based of the hardcoded list
        newPos = target.position - (target.forward * cameraPos[locationIndicator].x) + (target.up * cameraPos[locationIndicator].y);
        //Creates the value for the effect lerped 
        //accelerationEffect = Mathf.Lerp(accelerationEffect, carControllerV2.gForce * 3.5f, 2 * Time.deltaTime);
        //Changes the position of the CameraController object to the focus point object with a lerp
        transform.position = Vector3.Lerp(transform.position, focusPoint.transform.position, lerpTime * Time.deltaTime);
            //Lerp percentage wise instead of time based 

        //Gets the distance between current position and newPos then raises it to the power of forward distance which is 3
        distance = Mathf.Pow(Vector3.Distance(transform.position, newPos), forwardDistance);
        //Gets a point between current location and new with a max value of the distance var
        transform.position = Vector3.MoveTowards(transform.position, newPos, distance * Time.deltaTime);
        //Rotates the main camera object
        transform.GetChild(0).transform.localRotation = Quaternion.Lerp(transform.GetChild(0).transform.localRotation, Quaternion.Euler(-accelerationEffect, 0, 0), 5 * Time.deltaTime);
        //Rotates the camera controller object to the car object
        transform.LookAt(target.transform);
    }

}
