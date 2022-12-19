using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarMovement : MonoBehaviour
{
    private Rigidbody carBody;
    private SteeringUiRotation pedalsHolder;
    [SerializeField] Joystick wheelRot;
    [SerializeField] float roattionSpeed;
    [SerializeField] Transform targetMotor;
    [SerializeField] Transform[] frontWheels;
    public float velo;

    public float inputVer;
    [SerializeField] float motorPower;
    // Start is called before the first frame update
    void Start()
    {
        carBody = transform.GetComponent<Rigidbody>();
        pedalsHolder = FindObjectOfType<SteeringUiRotation>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        CarFlliped();
        CarInputs();
        WheelsRotation();
        
    }

    private void CarInputs()
    {
        Vector3 dir = targetMotor.position - transform.position;
        Vector3 targetVelocity = dir.normalized * 60f;
        Vector3 force = (targetVelocity - carBody.velocity);
        inputVer = pedalsHolder.PedalPushedPerecnt;
        Vector3 accel = force / Time.deltaTime;
        
        carBody.AddForce(accel * (motorPower * inputVer));
       
        if (accel.sqrMagnitude > 60f * 60f)
            accel = accel.normalized * 60f;
        velo = carBody.velocity.magnitude;
    }
    private void RotationCar()
    {
        var rotWheel = wheelRot.Horizontal * 100;
      
        var rot = Quaternion.Euler(0, rotWheel, 0);
        targetMotor.rotation = Quaternion.Slerp(targetMotor.rotation, rot, roattionSpeed * Time.fixedDeltaTime);
                
    }
    private void CarFlliped()
    {
        if(carBody.rotation.eulerAngles.z > 2)
        {
            carBody.rotation = Quaternion.Euler(carBody.rotation.eulerAngles.x, carBody.rotation.eulerAngles.y, 0);
        }
    }
    private void WheelsRotation()
    {
        foreach(Transform wheel in frontWheels)
        {           
            var rotWheel = wheelRot.Horizontal * 100;
            rotWheel = Mathf.Clamp(rotWheel, -60, 60);
            
            var rot = Quaternion.Euler(0,rotWheel,0);
            wheel.localRotation = Quaternion.Slerp(Quaternion.identity, rot, roattionSpeed * Time.deltaTime);
        }
        RotationCar();
    }
}
