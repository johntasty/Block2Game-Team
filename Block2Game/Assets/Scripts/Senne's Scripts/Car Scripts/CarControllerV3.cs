using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

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

    public TMP_Text wheelRPMText;
    public TMP_Text KPHText;
    public TMP_Text SlipText0;
    public TMP_Text SlipText1;
    public TMP_Text SlipText2;
    public TMP_Text SlipText3;

    private float verticalInput;
    private float horizontalInput;
    private bool handbrake;
    private bool brake;

    [Header("Game objects")]
    [SerializeField]
    private WheelCollider[] wheels = new WheelCollider[4];
    [SerializeField]
    private GameObject[] wheelMesh = new GameObject[4];
    [SerializeField]
    private GameObject centerMass;

    [Header("Current Power")]
    [SerializeField]
    private float KPH;
    [SerializeField]
    private float wheelsRPM;
    [SerializeField]
    private float totalPower;
    [SerializeField]
    private float engineRPM;

    [Header("Gear stuff")]
    [SerializeField]
    private float[] gears;
    [SerializeField]
    private int gearNum = 0;
    [SerializeField]
    private float maxRPM = 5000;
    [SerializeField]
    private float minRPM = 3000;
    [Tooltip("Smoothing time for engine to get transfer power")]
    [SerializeField]
    private float smoothTime = 0.01f;

    [Header("Steering")]
    [SerializeField]
    private float steeringMax;
    [SerializeField]
    private float radius = 6;

    [SerializeField]
    private float downForce = 50;
    [SerializeField]
    private float brakePower;
    [SerializeField]
    private float handbrakePower;
    [Header("Engine curve")]
    [SerializeField]
    private AnimationCurve enginePower;

    public float[] slip = new float[4];

    private WheelFrictionCurve forwardFriction, sidewaysFriction;
    [SerializeField]
    private float frictionMultiplier;
    [SerializeField]
    private float handbrakeFriction;
    [SerializeField]
    private float driftFactor;
    [SerializeField]
    private float sideStiffness;
    [SerializeField]
    private float forStiffness;
    [SerializeField]
    private float forwheelFriction;
    [SerializeField]
    float driftSmoothFactor = .7f;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        centerMass = GameObject.Find("Mass");
        rb.centerOfMass = centerMass.transform.localPosition;

        //KPHText = GetComponent<TextMeshProUGUI>();
        //SlipText = GetComponent<TextMeshProUGUI>();
    }
    private void Update()
    {
        CarInput();
        CalEnginePower();

        wheelRPMText.text = "wheel RPM " + wheelsRPM.ToString();
        KPHText.text = "KPH: " + KPH.ToString();
        SlipText0.text = "Slip 1: " + slip[0].ToString();
        SlipText1.text = "Slip 2: " + slip[1].ToString();
        SlipText2.text = "Slip 3: " + slip[2].ToString();
        SlipText3.text = "Slip 4: " + slip[3].ToString();
    }
    private void FixedUpdate()
    {
        MoveCar();
        Steering();
        AddDownForce();
        AnimateWheels();
        GetFriction();
        AdjustFriction();
        Shifter();

        //AdjustSteering();
    }

    private void CarInput()
    {
        verticalInput = Input.GetAxisRaw("Vertical");
        horizontalInput = Input.GetAxisRaw("Horizontal");
        if (Input.GetKey(KeyCode.LeftShift))
        {
            handbrake = true;
        }
        else
        {
            handbrake = false;
        }
        if (Input.GetKey(KeyCode.Space))
        {
            brake = true;
        }
        else
        {
            brake= false;
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
            //Starts counting at the third wheel in the list (rearwheel drive)
            for (int i = 2; i < wheels.Length; i++)
            {
                wheels[i].motorTorque = verticalInput * (totalPower / 2);
            }
        }
        else
        {
            //Gets the first two wheels in the list (forwheel drive)
            for (int i = 0; i < wheels.Length -2; i++)
            {
                wheels[i].motorTorque = verticalInput * (totalPower / 2);
                
            }
        }

        KPH = rb.velocity.magnitude * 3.6f;
        if (brake)
        {
            for (int i = 0; i < wheels.Length; i++)
                wheels[i].brakeTorque = brakePower;
        }
        else
        {
            for (int i = 0; i < wheels.Length; i++)
                wheels[i].brakeTorque = 0f;
        }
        if(handbrake)
        {
            for (int i = 2; i < wheels.Length; i++)
            {
                wheels[i].brakeTorque = handbrakePower;
            }
        }
        else
        {
            for (int i = 2; i < wheels.Length; i++)
            {
                wheels[i].brakeTorque = 0f;
            }
        }

        //for (int i = 0; i < wheels.Length -2; i++)
        //{
        //    wheels[i].sidewaysFriction.extremumSlip = 1+(downForce/ rb.mass);
        //}

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
        //reduces wheel radius based on the speed to make it easier to corner while driving at high speeds
        radius = 4 + KPH / 40;
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

            //slip[i] = wheelHit.sidewaysSlip;
            slip[i] = wheelHit.sidewaysSlip * 10;
        }
    }

    private void AdjustFriction()
    {
        

        if (handbrake)
        {
            forwardFriction = wheels[0].forwardFriction;
            sidewaysFriction = wheels[0].sidewaysFriction;

            

            float velocity = 0;
            sidewaysFriction.extremumValue = sidewaysFriction.asymptoteValue = forwardFriction.extremumValue = forwardFriction.asymptoteValue =
                Mathf.SmoothDamp(forwardFriction.asymptoteValue, driftFactor * handbrakeFriction, ref velocity, driftSmoothFactor);

            //sidewaysFriction.extremumSlip = sidewaysFriction.asymptoteSlip = forwardFriction.extremumSlip = forwardFriction.asymptoteSlip =
            //   Mathf.SmoothDamp(forwardFriction.asymptoteSlip, driftFactor * frictionMultiplier, ref velocity, driftSmoothFactor);

            for (int i = 2; i < 4; i++)
            {
                wheels[i].sidewaysFriction = sidewaysFriction;
                wheels[i].forwardFriction = forwardFriction;
            }

            //extra grip for front wheels
            sidewaysFriction.extremumValue = sidewaysFriction.asymptoteValue = forwardFriction.extremumValue = forwardFriction.asymptoteValue = forwheelFriction;
            for (int i = 0; i < 2; i++)
            {
                wheels[i].sidewaysFriction = sidewaysFriction;
                wheels[i].forwardFriction = forwardFriction;
            }

            

            for (int i = 2; i < 4; i++)
            {
               
                sidewaysFriction.stiffness = sideStiffness - KPH/ 600;

                wheels[i].sidewaysFriction = sidewaysFriction;
                wheels[i].forwardFriction = forwardFriction;
            }

            //extra velocity while using the handbrake
            rb.AddForce(transform.forward * (KPH / 400) * 1000);
        }
        else
        {
            forwardFriction = wheels[0].forwardFriction;
            sidewaysFriction = wheels[0].sidewaysFriction;

            forwardFriction.stiffness = 1;
            sidewaysFriction.stiffness = 1;



            forwardFriction.extremumValue = forwardFriction.asymptoteValue = sidewaysFriction.extremumValue = sidewaysFriction.asymptoteValue =
                ((KPH * frictionMultiplier) / 300) + 1f;

            for (int i = 0; i < 4; i++)
            {
                wheels[i].forwardFriction = forwardFriction;
                wheels[i].sidewaysFriction = sidewaysFriction;
            }
        }

        for (int i = 2; i < 4; i++)
        {
            WheelHit wheelHit; 

            wheels[i].GetGroundHit(out wheelHit);

            if (wheelHit.sidewaysSlip < 0)
                driftFactor = (1 + -horizontalInput) * Mathf.Abs(wheelHit.sidewaysSlip);
            if (wheelHit.sidewaysSlip > 0)
                driftFactor = (1 + horizontalInput) * Mathf.Abs(wheelHit.sidewaysSlip);
        }
    }

    private IEnumerator AdjustSteering()
    {
        while (true)
        {
            yield return new WaitForSeconds(.7f);
            radius = 6 + KPH / 20;
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
