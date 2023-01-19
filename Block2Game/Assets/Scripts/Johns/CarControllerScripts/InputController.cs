using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputController
{
    public SteeringUiRotation pedalsHolder { get; set; }
    private Joystick wheelRot;

    public Joystick Wheel
    {
        get => wheelRot;

        set => wheelRot = value;
    }
   
    public float RotationCar()
    {
        var rotWheel = wheelRot.Horizontal;
        return rotWheel;
    }
    public float CarPedalInput()
    {       
        float inputVer = pedalsHolder.PedalPushedPerecnt;
        return inputVer;       
    }
   
}
