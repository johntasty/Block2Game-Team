using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeybaordPop : MonoBehaviour
{
   public void OnBtClick()
    {

            System.Diagnostics.Process.Start("OSK.exe");

    }
    public void OnClickExit()
    {
        Application.Quit();
    }
}
