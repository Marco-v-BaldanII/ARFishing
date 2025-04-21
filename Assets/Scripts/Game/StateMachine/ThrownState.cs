
using UnityEngine;

public class ThrownState : IState
{
    Hook hook;
    Rigidbody rigid;
    BoxCollider collider;
    public ThrownState(Hook pablo)
    {
        hook = pablo;
        rigid = hook.GetComponent<Rigidbody>();
        collider = hook.GetComponentInChildren<BoxCollider>();

    }

    public override void Enter() {

        rigid.velocity = Vector3.zero;

        collider.enabled = true;

        rigid.useGravity = true;
        Vector3 launchDirection = hook.fishing_rod.transform.forward + hook.fishing_rod.transform.up * 0.5f;
        rigid.velocity = launchDirection * hook.launchVelocity;
        rigid.transform.parent = null;

        hook.currentFish = null;

    }

    public override void Process()
    {
        Debug.Log("Velocity is " + rigid.velocity);
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
