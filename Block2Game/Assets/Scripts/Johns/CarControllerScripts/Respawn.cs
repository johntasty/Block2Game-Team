using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class Respawn : MonoBehaviour
{
    [SerializeField] GameObject _DeadMenu;
    [SerializeField] GameObject _FactsHolder;
    [SerializeField] SpawnFacts _FactSpawner;
    public void RespawnFunction()
    {
        if(transform.position.y < -5f)
        {
            if (_DeadMenu.activeInHierarchy) { return; }
            _DeadMenu.SetActive(true);
            
            _FactsHolder.SetActive(true);
            _FactSpawner.SwitchFacts();
        }
        
    }
}
