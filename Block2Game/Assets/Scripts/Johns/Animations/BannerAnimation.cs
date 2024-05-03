using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BannerAnimation : MonoBehaviour
{
    [SerializeField] RawImage imageBox;

    [SerializeField] float _Speed;
    private Rect uvRect;

    // Update is called once per frame
    private void Start()
    {
        uvRect = imageBox.uvRect;

    }
    void Update()
    {
        uvRect.x += _Speed * Time.deltaTime;
        imageBox.uvRect = uvRect;
    }
}
