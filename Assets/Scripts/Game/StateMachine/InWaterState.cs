using Codice.CM.Common;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InWaterState : IState
{
    Hook hook;
    Rigidbody rigid;
    Collider collider;
    public InWaterState(Hook pablo)
    {
        hook = pablo;
        rigid = hook.GetComponent<Rigidbody>();
        collider = hook.GetComponent <Collider>();
    }

    public override void Process()
    {
        rigid.velocity = Vector3.Lerp(rigid.velocity, Vector3.zero, Time.deltaTime);
    }

    public override void OnAreaEnter(Collider other)
    {
        if (other.CompareTag("Fish"))
        {
            other.transform.parent = hook.transform;
           // bitten = true;
            rigid.useGravity = false;
            collider.enabled = false;
           // StartCoroutine(AttemptBreakFree());
            Fish fish = other.gameObject.GetComponent<Fish>();
            if (fish) { hook.currentFish = fish; }

            // Bit by fish -> transition to bitten state
            CallTransition(State.BITTEN, this);

        }
    }
}

