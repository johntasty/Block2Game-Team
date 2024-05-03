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
        foreach (Button pedal in pedals)
        {
            // Get the child object of the pedal that will be used for rotation
            Transform pedalVisualRotator = pedal.transform.GetChild(0);

            // Add an EventTrigger component to the pedal
            EventTrigger pedalFunc = pedal.gameObject.AddComponent<EventTrigger>();

            // Create two new EventTrigger entries for when the pedal is held down and released
            EventTrigger.Entry pedalHeld = new EventTrigger.Entry();
            EventTrigger.Entry pedalReleased = new EventTrigger.Entry();

            // Set the event type for the "pedalHeld" entry to pointer down
            pedalHeld.eventID = EventTriggerType.PointerDown;
            pedalReleased.eventID = EventTriggerType.PointerUp;
            // Add a listener to the "pedalHeld" entry that calls the "GasPedalHeld" function and passes in the "pedalVisualRotator" variable
            pedalHeld.callback.AddListener((pedalHeld) => { GasPedalHeld(pedalVisualRotator); });
            pedalReleased.callback.AddListener((pedalReleased) => { GasPedalReleased(pedalVisualRotator); });

            // Add the "pedalHeld" and "pedalReleased" entries to the EventTrigger component
            pedalFunc.triggers.Add(pedalHeld);
            pedalFunc.triggers.Add(pedalReleased);

        }
        // Start the "PedalInput" coroutine
        StartCoroutine(PedalInput());


    }

    // Update is called once per frame
    void Update()
    {
        RotationWheel();
        GasTesting();
    }
    public void GasTesting()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            percentPushed += 0.1f;
            percentPushed = Mathf.Clamp(percentPushed, 0, 1);
        }
        else { 
            percentPushed -= 0.1f;
            percentPushed = Mathf.Clamp(percentPushed, 0, 1);
        }
    }
    private void RotationWheel()
    {
        // Get the current position of the steering stick
        Vector2 stick = new Vector2(steeringStick.localPosition.x, steeringStick.localPosition.y);

        // Get the current position of the steering wheel
        Vector2 wheel = new Vector2(steeringWheel.rectTransform.localPosition.x, steeringWheel.rectTransform.localPosition.y);

        // Calculate the angle between the stick and the wheel
        float angle = Mathf.Atan2(stick.x - wheel.x, stick.y - wheel.y);

        // Convert the angle from radians to degrees
        float angleInRads = angle * 180 / Mathf.PI;

        // Reverse the sign of the angle for clockwise rotation
        Quaternion rotaion = Quaternion.AngleAxis(angleInRads * -1, Vector3.forward);

        // Apply the rotation to the steering wheel
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
        // Start an infinite loop
        while (true)
        {
            // Wait for the next frame
            yield return null;
            if (pedalHeldDown)
            {
                // Get the bounds of the button
                GetButtonBounds(pedalPushed.parent);

                // Calculate the push force as the distance between the mouse position and the pedal position
                float pushForce = Vector2.Distance(Input.mousePosition, pedalPosition);
                // Get the upward direction of the pedal
                Vector2 pointUp = transform.InverseTransformDirection(Vector2.up);
                // Get the direction of the push force
                Vector2 dir = (Input.mousePosition - pedalPosition).normalized;
                // Calculate the force direction as the dot product of the upward direction and the push force direction
                float forceDirection = Vector2.Dot(pointUp, dir);
                // Clamp the push force to the bounds of the button
                float posGasMouse = Mathf.Clamp(pushForce * forceDirection, 0f, (buttonBounds / 2));
                // Calculate the percentage of the button that is pushed
                percentPushed = posGasMouse / (buttonBounds / 2);
                // Get the rotation for when the pedal is fully pushed
                Quaternion pedalRotation = Quaternion.Euler(60f, 0f, 0f);
                // Apply the rotation to the pedal based on the percentage pushed
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
    public void ExitGame()
    {
        Debug.Log("exit");
        Application.Quit();
    }
}
