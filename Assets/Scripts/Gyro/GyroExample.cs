using System;
using System.Collections.Generic;
using UnityEngine;

public class GyroManager : MonoBehaviour
{
    private Gyroscope gyro;

    public static GyroManager instance;

    public GameObject noGyro;

    Quaternion device_rotation;

    public CompoundGesture throwGesture;

    public Queue<Rotation> detected_gestures;
    public float gesture_refresh_rate = 0.4f;
    Vector3 device_rotation_buffer = Vector3.zero;

    private void Awake()
    {
        if(instance != null)
        {
            Destroy(instance );
        }
        instance = this;
        DontDestroyOnLoad( instance );
    }

    void Start()
    {
        Input.gyro.enabled = true;
        gyro = Input.gyro;
        if (gyro == null || ! SystemInfo.supportsGyroscope) { print("Gyro not supported on this device");
            noGyro.gameObject.SetActive(true);
            Destroy(this.gameObject); }
        else
        {
            gyro.enabled = true;
        }

        detected_gestures = new Queue<Rotation>();

        // Throw rod gesture configuration
        throwGesture = new CompoundGesture();
        throwGesture.my_gesture = new Queue<Rotation>();
        throwGesture.my_gesture.Enqueue(Rotation.Roll);

        InvokeRepeating("DetectGestures", 0f, gesture_refresh_rate);


        //if (SystemInfo.supportsGyroscope)
        //{
        //    gyro = Input.gyro;
        //    gyro.enabled = true;
        //    mat.color = Color.blue;
        //}
        //else
        //{
        //    print("Gyroscope not supported on this device.");
        //    mat.color = Color.red;
        //}
    }

    void Update()
    {
        if (gyro != null)
        {
            device_rotation = gyro.attitude;
        }
    }

    public Quaternion GetRotation()
    {
        return device_rotation;
    }

    public Quaternion GetRodRotation()
    {
        Vector3 eulers = device_rotation.eulerAngles;
        return Camera.main.transform.rotation * Quaternion.Euler(-eulers.x, -eulers.z, eulers.y);
    }

    public void DetectGestures()
    {
        Vector3 current_rotation = GetRodRotation().eulerAngles;

        if(Mathf.Abs( current_rotation.x - device_rotation_buffer.x  )  > 20)
        {
            detected_gestures.Enqueue(Rotation.Yaw);
            print("Detected Yaw");
        }

        if (Mathf.Abs(current_rotation.y - device_rotation_buffer.y) > 20)
        {
            detected_gestures.Enqueue(Rotation.Pitch);
            print("Detected Pitch");
        }

        if (Mathf.Abs(current_rotation.z - device_rotation_buffer.z) > 20)
        {
            detected_gestures.Enqueue(Rotation.Roll);
            print("Detected Roll");
        }

        device_rotation_buffer = GetRodRotation().eulerAngles;
    }

}

