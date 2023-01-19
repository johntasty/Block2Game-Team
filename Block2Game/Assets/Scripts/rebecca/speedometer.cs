using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class speedometer : MonoBehaviour
{
    public Rigidbody target;

    public float maxSpeed = 0.0f; //the maximum speed of the target in km/h

    public float minSpeedArrowAngle; //the angle of the arrow when the speed of the target is 0
    public float maxSpeedArrowAngle; //the angle of the arrow when the speed of the target is max

    [Header("UI")]
    //public Text speedLabel; //the label that displays the speed,between the arrow and frame
    public RectTransform arrow; //the arrow in the speedometer

    private float speed = 0.0f;

    private void Update()
    {
        speed = target.velocity.magnitude * 2f; //3.6 to convert to km
        //changed to 2f cuz with 3.6 it seemed as if the max speed was reached instantly
        //speed must be clamped by car controller

        //if(speedLabel != null)
           // speedLabel.test = ((int)speed) + "km/h";
        
        if(arrow != null)
        {
            arrow.localEulerAngles = 
                new Vector3(0, 0, Mathf.Lerp(minSpeedArrowAngle, maxSpeedArrowAngle, speed / maxSpeed));
        }
    }
}
