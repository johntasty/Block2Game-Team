using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class Respawn : MonoBehaviour
{
    [SerializeField] GameObject _DeadMenu;
    public void RespawnFunction()
    {
        if(transform.position.y < -5f)
        {
            _DeadMenu.SetActive(true);
        }
        
    }
}
