using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Engine : MonoBehaviour
{
    [SerializeField] float _MotorPower;
    [SerializeField] float _MaxSpeed;
    public float _maxSpeed
    {
        get => _MaxSpeed;
    }
    [SerializeField] 
    private LayerMask Ground;
    private bool _Grounded = true;
    public void CarEngine(Transform car, Rigidbody carPhysics, float gas, bool Npc)
    {
        // Check if the car is grounded
        RaycastHit hit;
        if (!Npc)
        {
            _Grounded = Physics.Raycast(car.position, -transform.up, out hit, 20f, Ground);
            // if the car is not grounded, exit the function
            if (!_Grounded) return;
        }

        // Calculate the motor point of the car
        Vector3 motor = car.position + car.forward;

        // Get the direction of the motor point
        Vector3 dir = motor - transform.position;

        // Calculate the target velocity
        Vector3 targetVelocity = dir.normalized * _MaxSpeed;

        // Calculate the force needed to reach the target velocity
        Vector3 force = (targetVelocity - carPhysics.velocity);

        // Calculate the acceleration
        Vector3 accel = force / Time.deltaTime;

        // Apply the acceleration force to the car's rigidbody
        carPhysics.AddForce(accel * (_MotorPower * gas));

        // Limit the acceleration to the max speed
        if (accel.sqrMagnitude > _MaxSpeed * _MaxSpeed)
            accel = accel.normalized * _MaxSpeed;

    }

}
