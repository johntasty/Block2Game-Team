using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    CameraController cameraController;
    CarController carController;
    // Start is called before the first frame update
    void Start()
    {
        cameraController = GameObject.Find("CameraController").GetComponent<CameraController>();
        carController = GameObject.Find("Car").GetComponent <CarController>();

    }

    // Update is called once per frame
    void Update()
    {
        carController.CarUpdate();
    }
    private void FixedUpdate()
    {
      
        carController.CarFixedUpdate();
        
    }
    private void LateUpdate()
    {
        cameraController.CamFollow();
    }
}
