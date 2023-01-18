using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Timer : MonoBehaviour
{
    
    public AnimationClip Countdown3;
    public AnimationClip Countdown2;
    public AnimationClip Countdown1;
    private int CurrentCountdown;
    private TMP_Text Text;
    Animation anim;
    // Start is called before the first frame update
    private void Start()
    {
        anim = GetComponent<Animation>();
        GetComponent<Animator>().enabled = false;
        Text = GetComponent<TMP_Text>();
        CurrentCountdown = 3;
        StartCoroutine(startcountdown());

    }


    public IEnumerator startcountdown()
    {
        yield return new WaitForSeconds(5);
        while (CurrentCountdown > 0)
        {
            GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);

            GetComponent<Animator>().enabled = true;

            yield return new WaitForSeconds(1);
            CurrentCountdown -= 1;
            Text.text = CurrentCountdown.ToString();
            Debug.Log(Text.text);
            GetComponent<Animator>().enabled = false;
            Debug.Log(GetComponent<RectTransform>().localScale);

            GetComponent<RectTransform>().localScale = new Vector3(1,1,1);
            Debug.Log(GetComponent<RectTransform>().localScale);
        }
        //Text.text = (string) (int) CurrentCountdown;
        //ani.Play("countdown text", 0);

    }
}
