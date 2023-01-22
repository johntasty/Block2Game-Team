using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpawnFacts : MonoBehaviour
{
    [SerializeField] List<Sprite> _Facts = new List<Sprite>();

    [SerializeField] Image _FactHolder;

    public void SwitchFacts()
    {
        int _randomFact = Random.Range(0, _Facts.Count);

        _FactHolder.sprite = _Facts[_randomFact];
    }
}
