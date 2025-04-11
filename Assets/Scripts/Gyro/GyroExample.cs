using UnityEngine;

public class GyroManager : MonoBehaviour
{
    private Gyroscope gyro;

    public static GyroManager instance;

    private void Awake()
    {
        if(instance != null)
        {

        }
    }

    public Material mat;

    void Start()
    {
        gyro = Input.gyro;
        gyro.enabled = true;
        if (SystemInfo.supportsGyroscope)
        {
            gyro = Input.gyro;
            gyro.enabled = true;
            mat.color = Color.blue;
        }
        else
        {
            print("Gyroscope not supported on this device.");
            mat.color = Color.red;
        }
    }

    void Update()
    {
        if (gyro != null)
        {
            mat.color = Color.green;
            Quaternion deviceRotation = gyro.attitude;
            transform.rotation = deviceRotation;
        }
    }
}

