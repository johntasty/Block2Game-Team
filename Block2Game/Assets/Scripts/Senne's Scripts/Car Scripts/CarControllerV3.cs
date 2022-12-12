using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarControllerV3 : MonoBehaviour
{
    Rigidbody rb;
    internal enum driveType
    {
        frontWheelDrive,
        rearWheelDrive,
        allWheelDrive
    }
    [SerializeField]
    private driveType drive;



    private float verticalInput;
    private float horizontalInput;
    private bool handBrake;

    [SerializeField]
    private WheelCollider[] wheels = new WheelCollider[4];
    [SerializeField]
    private GameObject[] wheelMesh = new GameObject[4];
    [SerializeField]
    private GameObject centerMass;
   
    [SerializeField]
    private float KPH;
    [SerializeField]
    private float wheelsRPM;
    [SerializeField]
    private float totalPower;
    [SerializeField]
    private float engineRPM;
    [SerializeField]
    private float[] gears;
    [SerializeField]
    private int gearNum = 0;
    [SerializeField]
    private float smoothTime = 0.01f;
    [SerializeField]
    private float maxRPM = 5000;
    [SerializeField]
    private float minRPM = 3000;
    [SerializeField]
    private float torque;
    [SerializeField]
    private float steeringMax;
    [SerializeField]
    private float radius = 6;
    [SerializeField]
    private float downForce = 50;
    [SerializeField]
    private float brakePower;
    [SerializeField]
    private AnimationCurve enginePower;

    public float[] slip = new float[4];

    

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        centerMass = GameObject.Find("Mass");
        rb.centerOfMass = centerMass.transform.localPosition;
    }
    private void Update()
    {
        CarInput();
        
    }
    private void FixedUpdate()
    {
        MoveCar();
        Steering();
        AddDownForce();
        AnimateWheels();
        //GetFriction();
        CalEnginePower();
        Shifter();
    }

    private void CarInput()
    {
        verticalInput = Input.GetAxisRaw("Vertical");
        horizontalInput = Input.GetAxisRaw("Horizontal");
        if (Input.GetKey(KeyCode.LeftShift))
        {
            handBrake = true;
        }
        else
        {
            handBrake = false;
        }
    }

    private void MoveCar()
    {

        if (drive == driveType.allWheelDrive)
        {
            for (int i = 0; i < wheels.Length; i++)
            {
                wheels[i].motorTorque = verticalInput * (totalPower/4);
            }
        }
        else if (drive == driveType.rearWheelDrive)
        {
            for (int i = 2; i < wheels.Length; i++)
            {
                wheels[i].motorTorque = verticalInput * (totalPower / 2);
            }
        }
        else
        {
            for (int i = 0; i < wheels.Length -2; i++)
            {
                wheels[i].motorTorque = verticalInput * (totalPower / 2);
            }
        }

        KPH = rb.velocity.magnitude * 3.6f;
        if (handBrake)
        {
            wheels[3].brakeTorque = wheels[2].brakeTorque = brakePower;
        }
        else
        {
            wheels[3].brakeTorque = wheels[2].brakeTorque = 0f;
        }
    }
    private void Steering()
    {
        //Ackermann steering formula: Mathf.Rad2Deg * Mathf.Atan(2.55f / (radius + (1.5 / 2))) * horizontalInput;


        //rear track size is 1.5f and wheel base is 2.55f
        if (horizontalInput > 0)
        {
            wheels[0].steerAngle = Mathf.Rad2Deg * Mathf.Atan(2.55f / (radius + (1.5f / 2f))) * horizontalInput;
            wheels[1].steerAngle = Mathf.Rad2Deg * Mathf.Atan(2.55f / (radius + (1.5f / 2f))) * horizontalInput;
        }
        else if(horizontalInput < 0)
        {
            wheels[0].steerAngle = Mathf.Rad2Deg * Mathf.Atan(2.55f / (radius + (1.5f / 2f))) * horizontalInput;
            wheels[1].steerAngle = Mathf.Rad2Deg * Mathf.Atan(2.55f / (radius + (1.5f / 2f))) * horizontalInput;
        }
        else
        {
            wheels[0].steerAngle = 0;
            wheels[1].steerAngle = 0;
        }
    }
    private void CalEnginePower()
    {
        WheelRPM();
        totalPower = enginePower.Evaluate(engineRPM) * (gears[gearNum]) * verticalInput;
        float velocity = 0.0f;
        engineRPM = Mathf.SmoothDamp(engineRPM, 1000 + (Mathf.Abs(wheelsRPM) * (gears[gearNum])), ref velocity, smoothTime);
    }

    private void Shifter()
    {
        //automatic 
        if (engineRPM > maxRPM && gearNum < gears.Length-1)
        {
            gearNum++;
        }
        else if (engineRPM < minRPM && gearNum > 0)
        {
            gearNum--;

        }
    }
    private void WheelRPM()
    {
        float sum = 0;
        int R = 0;

        for (int i = 0; i < 4; i++)
        {
            sum += wheels[i].rpm;
            R++;
        }
        wheelsRPM = (R != 0) ? sum / R : 0;
    }

    private void GetFriction()
    {
        for (int i = 0; i < wheels.Length; i++)
        {
            WheelHit wheelHit;
            wheels[i].GetGroundHit(out wheelHit);

            slip[i] = wheelHit.sidewaysSlip;
        }
    }
    private void AddDownForce()
    {
        rb.AddForce(-transform.up * downForce * rb.velocity.magnitude);
    }

    private void AnimateWheels()
    {
        Vector3 wheelPos = Vector3.zero;
        Quaternion wheelRot = Quaternion.identity;

        for (int i = 0; i < wheels.Length; i++)
        {
            wheels [i].GetWorldPose(out wheelPos, out wheelRot);
            wheelMesh [i].transform.position = wheelPos;
            wheelMesh[i].transform.rotation = wheelRot;
        }
    }


    
}
