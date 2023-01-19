using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Respawn : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] Transform respawnPoint;

    public void RespawnFunction()
    {
        if(transform.position.y < -5f)
        {
            transform.position = respawnPoint.position;
            transform.rotation = Quaternion.identity;
        }
        
    }
}
