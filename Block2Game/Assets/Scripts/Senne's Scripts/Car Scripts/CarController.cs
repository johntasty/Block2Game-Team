using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarController : MonoBehaviour
{
    //Notes:
    //All things that need to go in update are seperated into its own functions for usage in a game manager later 
    public Rigidbody sphereRB;
    private float moveInput;

    [Header("Acceleration")]
    [SerializeField]
    [Tooltip("Amount of forward accelarion (~200 is nice)")]
    private float topSpeed;
    [SerializeField]
    private float acceleration;
    private float forwardSpeed;
    [SerializeField]
    [Tooltip("Amount of backwards accelarion (best to halve the forwardspeed)")]
    private float reverseSpeed;
    [Header ("Turnspeed")]
    [SerializeField]
    [Tooltip("The speed at which the car turns (~150 is nice)")]
    private float turnSpeedGrounded;
    [SerializeField]
    [Tooltip("Turning speed while airborn")]
    private float turnSpeedAir;
    
    private float turnSpeed;
    private float turnInput;

    [SerializeField]
    private float slerpRot = 1f;

    [Header("Drag")]
    [SerializeField]
    [Tooltip("Amount of drag while in the air (~.1 is nice)")]
    private float airDrag;
    [SerializeField]
    [Tooltip("Amount of drag while grounded (~4 is nice)")]
    private float groundDrag;
    [SerializeField]
    [Tooltip("Amount of gravity applied while in the air NEEDS TO BE MINUS(~-30 is nice)")]
    private float gravity = -30f;
    
    private bool isGrounded;
    [SerializeField]
    [Tooltip("Select the layer that the ground enviroment has ramps included")]
    private LayerMask Ground;

    [SerializeField]
    private float currentVelocity;
    public float gForce;
    private float lastFrameVelocity;


    

    private void Start()
    {
        //Detaches the sphere from the car
        sphereRB.transform.parent = null;
    }
    private void Update()
    {
        //Pivot point from the car empty needs to be at the same location as the pivot point from the sphere
        //Set the positon of the car empty to the sphere
        //transform.position = sphereRB.transform.position;

        //Slopes();

        CarUpdate();
    }
    private void FixedUpdate()
    {
       CarFixedUpdate();
    }

  

    public void CarFixedUpdate()
    {
        MotorForce();
        GForce();
        transform.position = sphereRB.transform.position;
    }
    public void CarUpdate()
    {
        
        Slopes();
        
    }
    
    private void MotorForce()
    {
        //Get vertical and horizontal input 
        //Maximum values are -1 to 1
        moveInput = Input.GetAxisRaw("Vertical");
        turnInput = Input.GetAxisRaw("Horizontal");

        //when the input value is higher then 1 it applies forward speed otherwise reverse speed

        forwardSpeed = Mathf.Lerp(currentVelocity, topSpeed, acceleration);

        moveInput *= moveInput > 0 ? forwardSpeed : reverseSpeed;

        //Get braking over time 
        //Add a bit of up and down 
        //IEnumator for bouncy effect
            //Loops for when acces isn't needed 

        //use wheels instead of sphere


        

        
        //Translates the horizontal input into a turn and gets vertical input to prevent 
        //the player from turning while standing still, also gives inverse rotation while reversing 
        float newRotation = turnInput * turnSpeed * Time.deltaTime * Input.GetAxisRaw("Vertical");
        //Applies the rotation value to the empty car game object 
        
        transform.Rotate(0, newRotation, 0, Space.World);
        //transform.rotation = Quaternion.Slerp(transform.rotation, )
       

        //Use the sphere for turning and let the car follow the sphere
        //Slerping

        //When the car is grounded it performs the defualt forward force and sets the drag of the rigidbody to the ground drag
        //When the car isn't grounded it applies the extra gravity and sets the drag to the airdrag value
        if (isGrounded)
        {
            //Applies force to the rigid body of the sphere 
            sphereRB.AddForce(transform.forward * moveInput, ForceMode.Acceleration);
            sphereRB.drag = groundDrag;
            turnSpeed = turnSpeedGrounded;
        }
        else
        {
            sphereRB.AddForce(transform.up * gravity);
            sphereRB.drag = airDrag;
            turnSpeed = turnSpeedAir;
        }

    }

    private void Turning()
    {
        //Translates the horizontal input into a turn and gets vertical input to prevent 
        //the player from turning while standing still, also gives inverse rotation while reversing 
        float newRotation = turnInput * turnSpeed * Time.deltaTime * Input.GetAxisRaw("Vertical");
        //Applies the rotation value to the empty car game object 
        transform.Rotate(0, newRotation, 0, Space.World);
    }
    private void Slopes()
    {
        //Performs a raycast to check if the car is on the ground 
        RaycastHit hit;
        isGrounded = Physics.Raycast(transform.position, -transform.up, out hit, 1f, Ground);
        //Rotates the car upwards for slopes 
        transform.rotation = Quaternion.FromToRotation(transform.up, hit.normal) * transform.rotation;

        //Get raycasts for all wheels to take the normal and slerp the rotation 
        //Look at unity forum
    }

    private void GForce()
    {
        currentVelocity = sphereRB.velocity.magnitude;
        gForce = (currentVelocity - lastFrameVelocity) / (Time.deltaTime * Physics.gravity.magnitude);
        lastFrameVelocity = currentVelocity;
    }


}
