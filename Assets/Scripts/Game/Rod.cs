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

    // Update is called once per frame
    void Update()
    {
        Vector3 eulers = GyroManager.instance.GetRotation().eulerAngles;
        transform.rotation = camera.transform.rotation * Quaternion.Euler(- eulers.x, eulers.z, eulers.y);
        //return;
        //transform.rotation = Quaternion.Euler(
        //    Mathf.Clamp(transform.rotation.eulerAngles.x, -limit_pitch, limit_pitch),
        //    Mathf.Clamp(transform.rotation.eulerAngles.x, -limit_pitch, limit_pitch),
        //    Mathf.Clamp(transform.rotation.eulerAngles.x, -limit_pitch, limit_pitch));
    }

    public void Calibrate()
    {
        calibrationRotation = Quaternion.Inverse(GyroManager.instance.GetRotation());
    }
}
