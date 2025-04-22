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
    Fish fish;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody>();
        fish = GetComponent<Fish>();
    }

    public override void OnEnter()
    {
        base.OnEnter();
        transform.parent = Hook.instance.transform;
        Hook.instance.currentFish = fish;
        Hook.instance.Bitten();


        //   // bitten = true;
        //    rigid.useGravity = false;
        //    collider.enabled = false;
        //   // StartCoroutine(AttemptBreakFree());
        //    Fish fish = other.gameObject.GetComponent<Fish>();
        //    if (fish) { 
        //        hook.currentFish = fish; 
        //    }

        //    // Bit by fish -> transition to bitten state
        //    CallTransition(State.BITTEN, this);
        //    bitten = true;
    }
    public override NodeResult Execute()
    {

        //transform.position = Hook.instance.transform.position;
        rigid.constraints = RigidbodyConstraints.FreezeAll;



        //    other.transform.parent = hook.transform;
        //   // bitten = true;
        //    rigid.useGravity = false;
        //    collider.enabled = false;
        //   // StartCoroutine(AttemptBreakFree());
        //    Fish fish = other.gameObject.GetComponent<Fish>();
        //    if (fish) { 
        //        hook.currentFish = fish; 
        //    }

        //    // Bit by fish -> transition to bitten state
        //    CallTransition(State.BITTEN, this);
        //    bitten = true;
        return NodeResult.success;
    }
}
