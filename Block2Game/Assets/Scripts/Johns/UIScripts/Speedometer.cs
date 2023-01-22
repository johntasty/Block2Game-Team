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
        float _speed = CarPlayer.velocity.magnitude;
        float maxSpeed = _PlayerEngine._maxSpeed;
        float _current = (_speed / maxSpeed) / 2;

        _SpeedBar.fillAmount = _current + 0.1f;
        _SpeedText.text = ((int)_speed * 3).ToString();
       
        _SpeedText.color = Color.Lerp(Color.cyan, Color.red, _current * 2);
        _Vfx.SetFloat("spawnrate", _speed);

    }
}
