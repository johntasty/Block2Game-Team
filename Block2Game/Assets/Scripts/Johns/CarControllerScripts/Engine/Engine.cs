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
        RaycastHit hit;
        if (!Npc)
        {
            _Grounded = Physics.Raycast(car.position, -transform.up, out hit, 20f, Ground);
            if (!_Grounded) return;
        }       

        Vector3 motor = car.position + car.forward;
        Vector3 dir = motor - transform.position;
        Vector3 targetVelocity = dir.normalized * _MaxSpeed;
        Vector3 force = (targetVelocity - carPhysics.velocity);

        Vector3 accel = force / Time.deltaTime;
        carPhysics.AddForce(accel * (_MotorPower * gas));

        if (accel.sqrMagnitude > _MaxSpeed * _MaxSpeed)
            accel = accel.normalized * _MaxSpeed;

    }
}
