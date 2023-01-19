using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HighScoreUI : MonoBehaviour
{
    [SerializeField] GameObject panel;
    [SerializeField] GameObject highScoreUIElementPrefab;
    [SerializeField] Transform elementWrapper;
    

    List<GameObject> uiElement = new List<GameObject> ();

    private void OnEnable()
    {
        HighScoreHandler.onHighScoreListChanged += UpdateUI;
    }

    private void OnDisable()
    {
        HighScoreHandler.onHighScoreListChanged -= UpdateUI;
    }
    private void UpdateUI(List <HighScoreElement> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            HighScoreElement el = list[i];

            if (el.rawTime > 0)
            {
                if(i >= uiElement.Count)
                {
                    //instantiate new object
                    var inst = Instantiate(highScoreUIElementPrefab, Vector3.zero, Quaternion.identity);
                    inst.transform.SetParent(elementWrapper, false);

                    uiElement.Add(inst);
                }
                //write or overwrite name and times
                var texts = uiElement[i].GetComponentsInChildren<Text>();
                texts[0].text = el.playerName;
                texts[1].text = el.timeString;
            }
        }
    }

    
}
