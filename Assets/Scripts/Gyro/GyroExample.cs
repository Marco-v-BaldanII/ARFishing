using UnityEngine;

public class GyroExample : MonoBehaviour
{
    private Gyroscope gyro;

    void Start()
    {
        if (SystemInfo.supportsGyroscope)
        {
            gyro = Input.gyro;
            gyro.enabled = true;
        }
        else
        {
            print("Gyroscope not supported on this device.");
        }
    }

    void Update()
    {
        if (gyro != null)
        {
            Quaternion deviceRotation = gyro.attitude;
            transform.rotation = deviceRotation;
        }
    }
}

