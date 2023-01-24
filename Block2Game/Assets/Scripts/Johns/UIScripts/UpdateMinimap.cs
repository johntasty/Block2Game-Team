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
        // Create a new Vector2 object with the player's x and z position
        Vector2 pos = new Vector2(_PlayerPos.position.x, _PlayerPos.position.z);

        // Update the player's position on the minimap using the "TestRatio" variable
        _Player.rectTransform.anchoredPosition = pos * TestRatio;
    }

    public void CalcualteDimensions()
    {
        // Get the size of the UI element
        var size = _Ui.rectTransform.rect.size;
        var TrackDimension = TrackDimensions;

        // Calculate the ratio between the UI element size and the track dimensions
        var scaleRatio = size / TrackDimension;
        TestRatio = scaleRatio;
        //var _Translation = -size / 2;

        //translationMatrix = Matrix4x4.TRS(_Translation, Quaternion.identity, scaleRatio);
    }


}
