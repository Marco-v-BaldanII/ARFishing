using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GyroManager : MonoBehaviour
{
    private Gyroscope gyro;

    public static GyroManager instance;

    public GameObject noGyro;

    Quaternion device_rotation;

    public CompoundGesture throwGesture;
    public CompoundGesture PitchGesture;
    public CompoundGesture NegativePitchGesture;
    public CompoundGesture PullGesture;

    float timer_to_refresh = 0.4f;

    public Queue<DetectedRotation> detected_gestures;
    private float GESTURE_REFRESH_RATE = 0.3f;
    private float COMPOUND_DETECTION_RATE = 0.2f;

    public List<CompoundGesture> compoundGestures;


    Vector3 device_rotation_buffer = Vector3.zero;

    private float DEGREES_DETECTION_THRESHOLD = 15f;

    public UnityEvent throw_event;
    public UnityEvent steer_left_event;
    public UnityEvent steer_right_event;
    public UnityEvent pull_event;


    private Quaternion gyroOffset = Quaternion.identity;
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
        compoundGestures = new List<CompoundGesture>();
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
        throwGesture._event = throw_event;
        throwGesture.name = "throw";

        PitchGesture = new CompoundGesture();
        PitchGesture.my_gesture = new List<DetectedRotation>();
        PitchGesture.my_gesture.Add(new DetectedRotation(Rotation.Pitch, -20f));
        PitchGesture._event = steer_left_event;
        PitchGesture.name = "steer_left";

        NegativePitchGesture = new CompoundGesture();
        NegativePitchGesture.my_gesture= new List<DetectedRotation>();
        NegativePitchGesture.my_gesture.Add(new DetectedRotation(Rotation.Pitch, 20f));
        NegativePitchGesture._event = steer_right_event;
        NegativePitchGesture.name = "steer_right";

        PullGesture = new CompoundGesture();
        PullGesture.my_gesture = new List<DetectedRotation> { new DetectedRotation(Rotation.Yaw , -20f) };
        PullGesture._event = pull_event;
        PullGesture.name = "pull_event";

        compoundGestures.Add(throwGesture); compoundGestures.Add(PitchGesture); compoundGestures.Add(NegativePitchGesture); compoundGestures.Add(PullGesture);

        InvokeRepeating("DetectGestures", 0f, GESTURE_REFRESH_RATE);
       InvokeRepeating("NonSuspectingGestureCheck", 0f, COMPOUND_DETECTION_RATE);
    }

    void Update()
    {
        if (gyro != null)
        {
            device_rotation = gyroOffset * gyro.attitude ;
            if(gyro.enabled == false)
            {
                gyro.enabled = true;
            }
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
        //Vector3 current_rotation = gyro.attitude.eulerAngles;
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
        device_rotation_buffer = current_rotation;
    }


    public void DetectCompoundGestures()
    {
        for (int j = 0; j < compoundGestures.Count; j++)
        {

            int index = 0;
            while (detected_gestures.Count > 0)
            {
                DetectedRotation rotation = detected_gestures.Dequeue();

                // detected rotation == first rotation of the throw gesture
                if (rotation.rotation == compoundGestures[j].my_gesture[index].rotation)
                {
                    if (compoundGestures[j].my_gesture[index].degrees > 0 && rotation.degrees >= compoundGestures[j].my_gesture[index].degrees)
                    {
                        index++;
                    }
                    else if (compoundGestures[j].my_gesture[index].degrees < 0 && rotation.degrees <= compoundGestures[j].my_gesture[index].degrees)
                    {
                        index++;
                    }
                    if (index == compoundGestures[j].my_gesture.Count)
                    {
                        print ( compoundGestures[j].name + " gesture identified ");
                        compoundGestures[j]._event?.Invoke();
                        if(j == 0) detected_gestures.Clear();
                        break;
                    }
                }
            }
            if (index != 2)
            {
                print("NOT Throw");
            }
        }
    }

    public void NonSuspectingGestureCheck()
    {
        for (int j = 0; j < compoundGestures.Count; j++)
        {
            int index = 0;
            Queue<DetectedRotation> detected = new Queue<DetectedRotation>(detected_gestures);
            while (detected.Count > 0)
            {
                DetectedRotation rotation = detected.Dequeue();

                // detected rotation == first rotation of the throw gesture
                if (rotation.rotation == compoundGestures[j].my_gesture[index].rotation)
                {
                    if (compoundGestures[j].my_gesture[index].degrees > 0 && rotation.degrees >= compoundGestures[j].my_gesture[index].degrees)
                    {
                        index++;
                    }
                    else if (compoundGestures[j].my_gesture[index].degrees < 0 && rotation.degrees <= compoundGestures[j].my_gesture[index].degrees)
                    {
                        index++;
                    }
                    if (index == compoundGestures[j].my_gesture.Count)
                    {
                        print(compoundGestures[j].name + " gesture identified ");
                        compoundGestures[j]._event?.Invoke();
                        if (j == 0) detected_gestures.Clear();
                        break;
                    }
                }

            }
        }
    }

    public void Recalibrate()
    {
        gyroOffset = Quaternion.Inverse(gyro.attitude);
        detected_gestures.Clear();
    }

}

