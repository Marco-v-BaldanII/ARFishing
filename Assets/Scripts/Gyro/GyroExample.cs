using System;
using UnityEngine;

public class GyroManager : MonoBehaviour
{
    private Gyroscope gyro;

    public static GyroManager instance;

    Quaternion device_rotation;

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
        gyro = Input.gyro;
        if (gyro == null) { print("Gyro not supported on this device"); Destroy(this.gameObject); }
        else
        {
            gyro.enabled = true;
        }
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

}

