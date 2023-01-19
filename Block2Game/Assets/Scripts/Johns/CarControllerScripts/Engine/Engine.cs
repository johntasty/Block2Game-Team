using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Engine : MonoBehaviour
{
    [SerializeField] float _MotorPower;
    [SerializeField] 
    private LayerMask Ground;
    private bool _Grounded = true;
    public void CarEngine(Transform car, Rigidbody carPhysics, float gas)
    {
        RaycastHit hit;
        _Grounded = Physics.Raycast(car.position, -transform.up, out hit, 10f, Ground);
        if (!_Grounded) return;

        Vector3 motor = car.position + car.forward;
        Vector3 dir = motor - transform.position;
        Vector3 targetVelocity = dir.normalized * 60f;
        Vector3 force = (targetVelocity - carPhysics.velocity);

        Vector3 accel = force / Time.deltaTime;
        carPhysics.AddForce(accel * (_MotorPower * gas));

        if (accel.sqrMagnitude > 60f * 60f)
            accel = accel.normalized * 60f;

    }
}
