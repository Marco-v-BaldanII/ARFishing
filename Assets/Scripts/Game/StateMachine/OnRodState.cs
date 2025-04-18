using log4net.Util;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnRodState : IState
{
    Hook hook;
    Rigidbody rigid;
    Rod fishing_rod;

    Vector3 hook_pos;
    public OnRodState(Hook pablo)
    {
        hook = pablo;
        fishing_rod = hook.fishing_rod;
        rigid = pablo.GetComponent<Rigidbody>();
        hook_pos = hook.transform.localPosition;
    }

    public override void Enter()
    {
        hook.transform.localPosition = hook_pos;
        rigid.useGravity = false; rigid.velocity = Vector3.zero;
        hook.transform.parent = fishing_rod.transform;
        hook.GetComponent<Collider>().enabled = false;
    }

}
