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

    private void Start()
    {
        Vector3 eulers = GyroManager.instance.GetRotation().eulerAngles;
        transform.rotation = camera.transform.rotation * Quaternion.Euler(-eulers.x, -eulers.z, eulers.y);
      //  cameraOffset.transform.LookAt(transform.position);
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 eulers = GyroManager.instance.GetRotation().eulerAngles;
      //  print("Rod receives " + eulers + "from gyro");
        transform.rotation = camera.transform.rotation * Quaternion.Euler(- eulers.x, - eulers.z, eulers.y);

    }

    public void Calibrate()
    {
        calibrationRotation = Quaternion.Inverse(GyroManager.instance.GetRotation());
    }
}
