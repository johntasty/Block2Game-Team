using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoverCarController : MonoBehaviour
{

    [SerializeField] private float _strenght;

    [SerializeField] private float _lenght;

    [SerializeField] private float _dampen;

    [SerializeField] LayerMask _ground;

    public float lastHitDis;
    public float forceAmount;

    public void HoverCar(Transform car, Rigidbody physicsBody)
    {
        // Perform a raycast to check if the car is hovering over the ground
        RaycastHit hit;
        if (Physics.Raycast(car.position, -car.up, out hit, _lenght, _ground))
        {
            // If the car is hovering over the ground, calculate the force using Hooks Law
            forceAmount = HooksLaw(hit.distance);
            // Apply the force to the car's physics body
            physicsBody.AddForceAtPosition(car.up * forceAmount, car.position);
        }
        else
        {
            lastHitDis = _lenght * 1.1f;
        }
    }

    private float HooksLaw(float distance)
    {
        // Calculate the force using Hooks Law
        float forceAmount = _strenght * (_lenght - distance) + (_dampen * (lastHitDis - distance));
        // Clamp the force to a minimum value of 0
        forceAmount = Mathf.Max(0f, forceAmount);
        // Update the last hit distance
        lastHitDis = distance;

        return forceAmount;
    }

}
