using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpdateMinimap : MonoBehaviour
{
    [SerializeField] Image _Player;
    [SerializeField] Vector2 TrackDimensions;
  
    Vector2 TestRatio;
    [SerializeField] RawImage _Ui;

    //Matrix4x4 translationMatrix;
    public void UpdateMinimapPos(Transform _PlayerPos)
    {
        Vector2 pos = new Vector2(_PlayerPos.position.x , _PlayerPos.position.z );
        
        _Player.rectTransform.anchoredPosition = pos * TestRatio;
    }

    public void CalcualteDimensions()
    {
        var size = _Ui.rectTransform.rect.size;
        var TrackDimension = TrackDimensions;

        var scaleRatio = size / TrackDimension;
        TestRatio = scaleRatio;
        //var _Translation = -size / 2;

        //translationMatrix = Matrix4x4.TRS(_Translation, Quaternion.identity, scaleRatio);
    }
    
}
