using System;
using UnityEngine;

public class GyroManager : MonoBehaviour
{
    private Gyroscope gyro;

    public static GyroManager instance;

    public GameObject noGyro;

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
        Input.gyro.enabled = true;
        gyro = Input.gyro;
        if (gyro == null || ! SystemInfo.supportsGyroscope) { print("Gyro not supported on this device");
            noGyro.gameObject.SetActive(true);
            Destroy(this.gameObject); }
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

