using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoadSpeed : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] Material _RoadMat;
    [SerializeField] Rigidbody _PlayerBody;
    [SerializeField] Engine _PlayerEngine;

    public void SetSpeed()
    {
        // Get the maximum speed from the player's engine
        float maxSpeed = _PlayerEngine._maxSpeed;

        // Get the current velocity of the player's body
        float velocity = _PlayerBody.velocity.magnitude;

        // Calculate the current speed as a percentage of the maximum speed
        float currentSpeed = (velocity / maxSpeed) * 0.1f;

        // Pass the current speed to the "Vector1_351a13987685418985b0a1c5632550fd" property of the road material
        _RoadMat.SetFloat("Vector1_351a13987685418985b0a1c5632550fd", currentSpeed);
    }


}
