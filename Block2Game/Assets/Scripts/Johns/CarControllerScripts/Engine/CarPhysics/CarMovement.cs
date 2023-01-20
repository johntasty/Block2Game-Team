using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent (typeof(Engine))]
[RequireComponent (typeof(CarSteering))]
public class CarMovement : MonoBehaviour
{
    private InputController inputSystem;
    private Engine _Engine;
    private CarSteering _SteeringWheel;
    private HoverCarController _HoverSystem;
    private Rigidbody carBody;
    private SteeringUiRotation pedalsHolderUi;
    [SerializeField] Joystick wheelRot;

    [SerializeField] RenderTrack _TrackPos;

    private Respawn _Spawner;
    //[SerializeField] Transform[] frontWheels;

    //[SerializeField] float turnPower;
    [SerializeField] bool _NPC;
    
    void Start()
    {
        _TrackPos = FindObjectOfType<RenderTrack>();
        LineRenderer _TrackFirstPos = _TrackPos.GetComponent<LineRenderer>();

        Vector3 _CarPosStart = _TrackFirstPos.GetPosition(0);
        Vector3 _CarStartRot = _TrackFirstPos.GetPosition(1);

        transform.position = _CarPosStart + (Vector3.up * 4);
        Vector3 dir = _CarStartRot - transform.position;
        
        Quaternion _RotationStart = Quaternion.LookRotation(dir, Vector3.up);
        transform.rotation = _RotationStart;

        carBody = transform.GetComponent<Rigidbody>();
        pedalsHolderUi = FindObjectOfType<SteeringUiRotation>();

        _Engine = transform.GetComponent<Engine>();
        _SteeringWheel = transform.GetComponent<CarSteering>();
        _HoverSystem = transform.GetComponent<HoverCarController>();
        _Spawner = GetComponent<Respawn>();

        inputSystem = new InputController();
        inputSystem.Wheel = wheelRot;
        inputSystem.pedalsHolder = pedalsHolderUi;
    }


    void FixedUpdate()
    {
        _Spawner.RespawnFunction();
        _HoverSystem.HoverCar(transform, carBody);
        _SteeringWheel.RotationCar(transform, inputSystem.RotationCar());
        _Engine.CarEngine(transform,carBody,inputSystem.CarPedalInput());
        //WheelsRotation();
        
    }
         
    //private void WheelsRotation()
    //{
    //    foreach(Transform wheel in frontWheels)
    //    {           
    //        var rotWheel = wheelRot.Horizontal * 100;
    //        rotWheel = Mathf.Clamp(rotWheel, -60, 60);
            
    //        var rot = Quaternion.Euler(0,rotWheel,0);
    //        wheel.localRotation = Quaternion.Slerp(Quaternion.identity, rot, roattionSpeed * Time.deltaTime);
    //    }       
    //}
}
