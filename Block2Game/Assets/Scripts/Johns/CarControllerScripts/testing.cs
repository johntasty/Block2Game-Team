using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testing : MonoBehaviour
{
    [SerializeField] Joystick rotationStick;
    private SteeringUiRotation pedalsHolderUi;
    [SerializeField] float turnPower;
    Rigidbody rigidPhysics;

    private InputController inputSystem;

    private float inputVer;

    public float motorPower;
    // Start is called before the first frame update
    private void Start()
    {
        rigidPhysics = transform.GetComponent<Rigidbody>();
        pedalsHolderUi = FindObjectOfType<SteeringUiRotation>();

        inputSystem = new InputController();
        inputSystem.pedalsHolder = pedalsHolderUi;
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        var rotWheel = rotationStick.Horizontal;
        Quaternion deltaRotation = Quaternion.Euler(transform.up * turnPower * rotWheel);
        rigidPhysics.MoveRotation(rigidPhysics.rotation * deltaRotation);
        CarInputs();
    }
    private void CarInputs()
    {
        Vector3 dir = (transform.position + transform.forward) - transform.position;
        Vector3 targetVelocity = dir.normalized * 60f;
        Vector3 force = (targetVelocity - rigidPhysics.velocity);
        inputVer = inputSystem.CarPedalInput();
        Vector3 accel = force / Time.deltaTime;

        rigidPhysics.AddForce(accel * (motorPower * inputVer));

        if (accel.sqrMagnitude > 60f * 60f)
            accel = accel.normalized * 60f;

    }
}
