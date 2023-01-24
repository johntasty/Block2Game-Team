using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;
using UnityEngine.UI;
using TMPro;
public class Speedometer : MonoBehaviour
{
    [SerializeField] Rigidbody CarPlayer;
    [SerializeField] Image _SpeedBar;
    [SerializeField] TMP_Text _SpeedText;
    [SerializeField] Engine _PlayerEngine;
    [SerializeField] VisualEffect _Vfx;
    public void SpeedometerBar()
    {
        // Get the current speed of the car
        float _speed = CarPlayer.velocity.magnitude;

        // Get the maximum speed of the car
        float maxSpeed = _PlayerEngine._maxSpeed;

        // Calculate the current speed as a percentage of the maximum speed
        float _current = (_speed / maxSpeed) / 2;

        // Update the fill amount of the speedometer bar 
        _SpeedBar.fillAmount = _current + 0.1f;

        // Update the speed text to display the current speed
        _SpeedText.text = ((int)_speed * 3).ToString();

        // Lerp the color of the text between cyan and red based on the current speed
        _SpeedText.color = Color.Lerp(Color.cyan, Color.red, _current * 2);

        // Set the spawnrate of the VFX to the current speed of the car
        _Vfx.SetFloat("spawnrate", _speed);
    }

}
