using MBT;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

[AddComponentMenu("")]
[MBTNode(name = "Bite")]
public class MBT_Bite : Leaf
{
    private Rigidbody rigid;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody>();
    }
    public override NodeResult Execute()
    {
        return NodeResult.success;
    }
}
