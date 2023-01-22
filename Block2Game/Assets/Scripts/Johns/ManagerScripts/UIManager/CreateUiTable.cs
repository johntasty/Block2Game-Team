using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class CreateUiTable : MonoBehaviour
{
    [SerializeField] GameObject _PrefabName;
    [SerializeField] Transform _ListHolder;

    public void SpawnUi(string name, string time)
    {
        GameObject spawned = Instantiate(_PrefabName, _ListHolder);
        spawned.transform.GetChild(1).GetComponent<Text>().text = name;
        spawned.transform.GetChild(2).GetComponent<Text>().text = time;
    }
}
