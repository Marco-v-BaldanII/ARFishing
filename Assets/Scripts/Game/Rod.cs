using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rod : MonoBehaviour
{
    [Header("RotationLimits")]
    public float limit_pitch;
    public float limit_yaw;
    public float limit_roll;

    Quaternion calibrationRotation;
    public Camera camera;
    public Transform cameraOffset;

    public Rigidbody hook_object;
    private Vector3 hook_pos;
    HookStateMachine machine;

    private Hook hook;


    private void Start()
    {
        Vector3 eulers = GyroManager.instance.GetRotation().eulerAngles;
        transform.rotation = GyroManager.instance.GetRodRotation();
        //  cameraOffset.transform.LookAt(transform.position);

        GyroManager.instance.throw_event.AddListener(ThrowRod);
        hook_pos = hook_object.transform.localPosition;
        machine = hook_object.GetComponent<HookStateMachine>();
        hook = hook_object.GetComponent<Hook>();
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.A)) // manual debug method for throwing hook
        {
            GyroManager.instance.throw_event?.Invoke(20);
        }


            


            Vector3 eulers = GyroManager.instance.GetRotation().eulerAngles;
        //print("Rod receives " + eulers + "from gyro");
        //transform.rotation = camera.transform.rotation * Quaternion.Euler(- eulers.x, - eulers.z, eulers.y);
        transform.rotation = GyroManager.instance.GetRodRotation();

        Debug.DrawLine(transform.position, transform.forward * 10, Color.blue);

        // If touch screen return hook
        if (Input.touchCount == 0)
            return;
        Touch a_touch = Input.GetTouch(0);
        if (a_touch.phase != TouchPhase.Ended) { return; }

        GyroManager.instance.Recalibrate();

        //machine.OnChildTransitionEvent(State.ON_ROD);

    }

    public void Calibrate()
    {
        calibrationRotation = Quaternion.Inverse(GyroManager.instance.GetRotation());
    }



    public void ThrowRod(float degrees)
    {

        hook.launchVelocity = degrees / 6.666f;
        print("Force of launch is " + hook.launchVelocity);

        if (machine.checkState == State.ON_ROD)
        {
            machine.OnChildTransitionEvent(State.THROWN);
        }
    }

}
