using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Timer : MonoBehaviour
{

    Animator m_animator;
    [SerializeField]TMP_Text m_text;
    [SerializeField]GameObject canvasAnimation;

    GameManagerBehaviour _Manager;
    [SerializeField]SteeringUiRotation _Inputs;

    private int m_current_number;
    private int m_initial_waittime;

    void Start()
    {
        // Set the initial countdown number to 3
        m_current_number = 3;

        // Before starting the countdown, wait this many seconds. 
        m_initial_waittime = 2;

        // Get all the required components once during start. 
        m_animator = GetComponent<Animator>();
        _Manager = GetComponent<GameManagerBehaviour>();
        // Disable the animator to not autostart the animation. 
        m_animator.enabled = false;

        // Start a coroutine.
        StartCoroutine(delayAnimation());
    }


    // Does the actual hard work; waits m_initial_waittime seconds, 
    // and then counts down from 3 to 0. 
    IEnumerator delayAnimation()
    {
       
        yield return new WaitForSeconds(m_initial_waittime);
        // Can do other things to, such as changing color
        m_text.color = Color.red;
        m_animator.enabled = true;
        while (m_current_number > 0)
        {
            yield return new WaitForSeconds(1);
            m_current_number -= 1;
            
            m_text.text = m_current_number.ToString();
        }

        // Stop playing the animations. 
        m_animator.enabled = false;

        // Optionally destroy the gameobject when no longer needed.
        canvasAnimation.SetActive(false);
        _Manager.enabled = true;
        _Inputs.enabled = true;
    }
}

