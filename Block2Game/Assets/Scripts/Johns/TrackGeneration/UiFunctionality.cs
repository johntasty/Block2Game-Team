using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UiFunctionality : MonoBehaviour
{
    Text percentageText;
    TrackGeneratorConvex _Generator;

    private float _Value;

    void Start()
    {
        percentageText = GetComponent<Text>();
        _Generator = FindObjectOfType<TrackGeneratorConvex>();
    }
    public void DisplayValue(float value)
    {
        percentageText.text = value.ToString();
        _Value = value;
    }
    public void SetValue(string ValueName)
    {
        var num = _Generator.GetType().GetProperty(ValueName);
        num.SetValue(_Generator, _Value, null);
    }

}
