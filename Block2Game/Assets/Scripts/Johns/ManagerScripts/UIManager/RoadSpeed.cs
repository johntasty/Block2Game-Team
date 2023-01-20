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
        float maxSpeed = _PlayerEngine._maxSpeed;
        float velocity = _PlayerBody.velocity.magnitude;
        float currentSpeed = (velocity / maxSpeed) * 0.1f;
        
        _RoadMat.SetFloat("Vector1_351a13987685418985b0a1c5632550fd", currentSpeed);
    }

}
