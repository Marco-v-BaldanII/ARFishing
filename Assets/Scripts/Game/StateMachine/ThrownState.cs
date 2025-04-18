using log4net.Util;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrownState : IState
{
    Hook hook;
    Rigidbody rigid;
    Collider collider;
    public ThrownState(Hook pablo)
    {
        hook = pablo;
        rigid = hook.GetComponent<Rigidbody>();
        collider = hook.GetComponent<Collider>();
    }

    public override void Enter() {

        collider.enabled = true;

        rigid.useGravity = true;
        rigid.AddForce(hook.fishing_rod.transform.forward * 1.5f, ForceMode.Impulse);
        rigid.transform.parent = null;

    }

    public override void OnBodyEnter(Collider collision) 
    {
        if (collision.CompareTag("Water"))
        {
            rigid.velocity = Vector3.Lerp(rigid.velocity, Vector3.zero, Time.deltaTime);
            CallTransition(State.IN_WATER, this);
        }
    }

}
