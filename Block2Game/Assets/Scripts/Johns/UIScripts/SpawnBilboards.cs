using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class SpawnBilboards : MonoBehaviour
{   
    [SerializeField] GameObject _BillBoard;

    public void SpawnBoards(LineRenderer lineRenderer, List<string> facts)
    {
        List<string> tempFacts = facts;
        Vector3[] pos = new Vector3[lineRenderer.positionCount];
         
        lineRenderer.GetPositions(pos);
        int randomPos = pos.Length - 5;
        for (int i = 3; i < randomPos; i += 6)
        {
            string fact = string.Empty;
            int randomFact = Random.Range(0, tempFacts.Count - 1);
            if(randomFact < tempFacts.Count)
            {
                fact = tempFacts[randomFact];
                tempFacts.Remove(tempFacts[randomFact]);
            }
            
            
            Vector3 dir = pos[i] - pos[i - 1];
            GameObject boardSpawn = Instantiate(_BillBoard);
            boardSpawn.transform.GetChild(1).GetComponent<TMP_Text>().text = fact;            

            boardSpawn.transform.position = pos[i] + (Vector3.up * 30);
            boardSpawn.transform.rotation = Quaternion.FromToRotation(boardSpawn.transform.forward, dir);
        }
        
    }
    public void SpawnBoardsAtPlayer(Transform _Player, List<string> facts)
    {
        List<string> tempFacts = facts;        

       

    }
    //public void BlinkBillOut()
    //{
    //    _BillBoardObject.SetActive(false);
    //}
}
