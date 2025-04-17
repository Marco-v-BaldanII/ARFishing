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

    private void Start()
    {
        Vector3 eulers = GyroManager.instance.GetRotation().eulerAngles;
        transform.rotation = GyroManager.instance.GetRodRotation();
        //  cameraOffset.transform.LookAt(transform.position);

        GyroManager.instance.throw_event.AddListener(ThrowRod);
        hook_pos = hook_object.transform.localPosition;
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.A))
        {
            GyroManager.instance.throw_event?.Invoke();
        }


        Vector3 eulers = GyroManager.instance.GetRotation().eulerAngles;
        //print("Rod receives " + eulers + "from gyro");
        transform.rotation = camera.transform.rotation * Quaternion.Euler(- eulers.x, - eulers.z, eulers.y);

        Debug.DrawLine(transform.position, transform.forward * 10, Color.blue);

        // If touch screen return hook
        if (Input.touchCount == 0)
            return;
        Touch a_touch = Input.GetTouch(0);
        if (a_touch.phase != TouchPhase.Ended) { return; }

        hook_object.transform.localPosition = hook_pos;
        hook_object.useGravity = false; hook_object.velocity = Vector3.zero;
        hook_object.transform.parent = transform;
        hook_object.GetComponent<Collider>().enabled = false;

    }

    public void Calibrate()
    {
        calibrationRotation = Quaternion.Inverse(GyroManager.instance.GetRotation());
    }

    public void ThrowRod()
    {
        hook_object.GetComponent<Collider>().enabled = true;

        hook_object.useGravity = true;
        hook_object.AddForce(transform.forward  * 1, ForceMode.Impulse);
        hook_object.transform.parent = null;
    }

}
