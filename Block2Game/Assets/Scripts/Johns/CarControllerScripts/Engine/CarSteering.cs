using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarSteering : MonoBehaviour
{
    [SerializeField] float _TurnSpeed;
    [SerializeField] float _RotationSpeed;
    public void RotationCar(Transform car,float steeringWheel)
    {        
        var rot = car.rotation * Quaternion.Euler(0, _TurnSpeed * steeringWheel, 0);
        car.rotation = Quaternion.Slerp(car.rotation, rot, _RotationSpeed * Time.fixedDeltaTime);

    }
}
