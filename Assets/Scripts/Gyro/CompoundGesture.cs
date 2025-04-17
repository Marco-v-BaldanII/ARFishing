using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Rotation
{
    Roll,    // Z
    Pitch,   // Y
    Yaw      // X
}

public class CompoundGesture 
{
    public Queue<Rotation> my_gesture;
}
