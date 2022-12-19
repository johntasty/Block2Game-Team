using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SteeringUiRotation : MonoBehaviour
{
    [SerializeField] RectTransform steeringStick;

    [SerializeField] Image steeringWheel;
    [SerializeField] Button gasPedal;
    [SerializeField] Transform gasPedalVisual;
    [SerializeField] Button[] pedals;

    //pedal variables 
    Transform pedalPushed;
    Vector3 pedalPosition;
    private float buttonBounds;
    private bool pedalHeldDown = false;
    private bool breakPedal = false;
    private float percentPushed;

    public float PedalPushedPerecnt
    {
        get { return percentPushed; }
    }
    public bool BreakPedalPushed
    {
        get { return breakPedal; }
    }
    //break pedal

    // Start is called before the first frame update
    void Start()
    {
        foreach(Button pedal in pedals)
        {
            Transform pedalVisualRotator = pedal.transform.GetChild(0);
            EventTrigger pedalFunc = pedal.gameObject.AddComponent<EventTrigger>();
            EventTrigger.Entry pedalHeld = new EventTrigger.Entry();
            EventTrigger.Entry pedalReleased = new EventTrigger.Entry();
            pedalHeld.eventID = EventTriggerType.PointerDown;
            pedalReleased.eventID = EventTriggerType.PointerUp;           
            pedalHeld.callback.AddListener((pedalHeld) => { GasPedalHeld(pedalVisualRotator); } );
            pedalReleased.callback.AddListener((pedalReleased) => { GasPedalReleased(pedalVisualRotator); } );

            pedalFunc.triggers.Add(pedalHeld);
            pedalFunc.triggers.Add(pedalReleased);

        }       
        StartCoroutine(PedalInput());       
        
    }

    // Update is called once per frame
    void Update()
    {
        RotationWheel();
    }
    private void RotationWheel()
    {
        Vector2 stick = new Vector2(steeringStick.localPosition.x, steeringStick.localPosition.y);
        Vector2 wheel = new Vector2(steeringWheel.rectTransform.localPosition.x, steeringWheel.rectTransform.localPosition.y);
       
        float angle = Mathf.Atan2(stick.x - wheel.x, stick.y - wheel.y);
        float angleInRads = angle * 180 / Mathf.PI;
       
        //reverse sign for clockwise rotation
        Quaternion rotaion = Quaternion.AngleAxis(angleInRads * - 1, Vector3.forward);
        steeringWheel.rectTransform.localRotation = rotaion;
    }

    public void GasPedalHeld(Transform pedalToRotate)
    {
        pedalHeldDown = true;
        pedalPushed = pedalToRotate;
    }
    public void GasPedalReleased(Transform pedalToRotate)
    {
        pedalHeldDown = false;
        pedalPushed = pedalToRotate;
        percentPushed = 0;
        pedalPushed.localRotation = Quaternion.identity;
    }

    IEnumerator PedalInput()
    {
        while (true)
        {
            yield return null;
            if (pedalHeldDown)
            {
               
                GetButtonBounds(pedalPushed.parent);
                
                float pushForce = Vector2.Distance(Input.mousePosition, pedalPosition);
                Vector2 pointUp = transform.InverseTransformDirection(Vector2.up);
                Vector2 dir = (Input.mousePosition - pedalPosition).normalized;
                float forceDirection = Vector2.Dot(pointUp, dir);
                float posGasMouse = Mathf.Clamp(pushForce * forceDirection, 0f, (buttonBounds/2));
                percentPushed = posGasMouse / (buttonBounds/2);
                
                Quaternion pedalRotation = Quaternion.Euler(60f, 0f, 0f);
                pedalPushed.localRotation = Quaternion.Slerp(Quaternion.identity, pedalRotation, percentPushed);
               
            }
        }
    }
    public void GetButtonBounds(Transform pedal)
    {
        pedalPosition = new Vector3(pedal.position.x, pedal.position.y, 0f);
        pedalPosition.y = pedalPosition.y - (pedalPosition.y / 2);
        buttonBounds = gasPedal.GetComponent<RectTransform>().rect.height;
    }
    public void BreakPedal()
    {
        breakPedal = !breakPedal;
    }
}
