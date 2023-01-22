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
        RaycastHit hit;
        if (Physics.Raycast(car.position, -car.up, out hit, _lenght, _ground))
        {            
            forceAmount = HooksLaw(hit.distance);
            physicsBody.AddForceAtPosition(car.up * forceAmount, car.position);
        }
        else
        {
            lastHitDis = _lenght * 1.1f;
        }
    }

    private float HooksLaw(float distance)
    {
        float forceAmount = _strenght * (_lenght - distance) + (_dampen * (lastHitDis - distance));
        forceAmount = Mathf.Max(0f, forceAmount);
        lastHitDis = distance;

        return forceAmount;
    }
   
}
