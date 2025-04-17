using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Rotation
{
    Roll,    // Z
    Pitch,   // Y
    Yaw      // X
}

public struct DetectedRotation
{
    public Rotation rotation;
    public float degrees;

    public DetectedRotation(Rotation rotation, float degrees)
    {
        this.rotation = rotation;
        this.degrees = degrees;
    }
}

public class CompoundGesture 
{
    public List<DetectedRotation> my_gesture;
}
