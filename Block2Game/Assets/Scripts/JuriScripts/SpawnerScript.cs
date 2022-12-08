using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerScript : MonoBehaviour
{
    public GameObject enemy;
    public float xPos;
    public float zPos;
    public int enemyCount;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(EnemySpawn());
    }

  
    IEnumerator EnemySpawn()
    {
        while (enemyCount < 3)
        {
            xPos = Random.Range(-12f, 14f);
            zPos = Random.Range(15f, 50f);
            Instantiate(enemy, new Vector3(xPos, 0f, zPos), Quaternion.identity);
            yield return new WaitForSeconds(15f);
            enemyCount += 1;
        }
    }
}
