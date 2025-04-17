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

    float timer_to_refresh = 0.4f;

    public Queue<DetectedRotation> detected_gestures;
    private float GESTURE_REFRESH_RATE = 0.3f;
    private float COMPOUND_DETECTION_RATE = 0.9f;


    Vector3 device_rotation_buffer = Vector3.zero;

    private float DEGREES_DETECTION_THRESHOLD = 15f;

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

        detected_gestures = new Queue<DetectedRotation>();

        // Throw rod gesture configuration
        throwGesture = new CompoundGesture();
        throwGesture.my_gesture = new List<DetectedRotation>();
        throwGesture.my_gesture.Add(new DetectedRotation( Rotation.Yaw , -15f));
        throwGesture.my_gesture.Add(new DetectedRotation( Rotation.Yaw,   15f));

        InvokeRepeating("DetectGestures", 0f, GESTURE_REFRESH_RATE);
      //  InvokeRepeating("DetectCompoundGestures", 0f, COMPOUND_DETECTION_RATE);


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

        timer_to_refresh -= Time.deltaTime;
        if(timer_to_refresh <= 0)
        {
            DetectCompoundGestures();
            timer_to_refresh = 999f;
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
        Vector3 subtraction = new Vector3( /* DeltaAngle returns the shortest distance between 2 angles. ex: DeltaAngle(355 , 5) = 10 */
            Mathf.DeltaAngle(device_rotation_buffer.x, current_rotation.x),
            Mathf.DeltaAngle(device_rotation_buffer.y, current_rotation.y),
            Mathf.DeltaAngle(device_rotation_buffer.z, current_rotation.z)
        );
        bool refresh_timer = false;

        if (Mathf.Abs( subtraction.x  )  > DEGREES_DETECTION_THRESHOLD)
        {
            detected_gestures.Enqueue(new DetectedRotation(Rotation.Yaw , subtraction.x ));
            print("Detected Yaw of" + subtraction.x);
            refresh_timer = true;
        }

        if (Mathf.Abs( subtraction.y) > DEGREES_DETECTION_THRESHOLD)
        {
            detected_gestures.Enqueue(new DetectedRotation( Rotation.Pitch, subtraction.y));
            print("Detected Pitch of " + subtraction.y);
            refresh_timer = true;
        }

        if (Mathf.Abs(subtraction.z ) > DEGREES_DETECTION_THRESHOLD)
        {
            detected_gestures.Enqueue(new DetectedRotation( Rotation.Roll, subtraction.z));
            print("Detected Roll of " + subtraction.z);
            refresh_timer = true;
        }
        if (refresh_timer) { timer_to_refresh = 0.9f; }
        device_rotation_buffer = GetRodRotation().eulerAngles;
    }


    public void DetectCompoundGestures()
    {
        int index = 0;
        while (detected_gestures.Count > 0)
        {
            DetectedRotation rotation = detected_gestures.Dequeue();

            // detected rotation == first rotation of the throw gesture
            if(rotation.rotation == throwGesture.my_gesture[index].rotation)
            {
                if(throwGesture.my_gesture[index].degrees > 0 && rotation.degrees >= throwGesture.my_gesture[index].degrees)
                {
                    index++;
                }
                else if (throwGesture.my_gesture[index].degrees < 0 && rotation.degrees <= throwGesture.my_gesture[index].degrees)
                {
                    index++;
                }

                if (index == throwGesture.my_gesture.Count)
                {
                    print("Throw gesture identified ");
                    break;
                }
            }
            

        }

        if(index != 2)
        {
            print("NOT Throw");
        }
       

    }
}

