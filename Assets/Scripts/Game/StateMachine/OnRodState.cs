
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
        hook_pos = hook.point.position;

        hook.fishManager = GameObject.Find("FishManager").GetComponent<FishManager>();

    }

    public override void Enter()
    {
        hook.transform.localPosition = hook_pos;
        rigid.useGravity = false; rigid.velocity = Vector3.zero;
        hook.transform.parent = fishing_rod.transform;
        hook.GetComponent<Collider>().enabled = false;

        // Solve glitch
        hook.fishManager.ResetAllTargets(null);
    }


    public override void Process()
    {
        hook_pos = hook.point.position;
        hook.transform.position = hook_pos;
        // Solve glitch
        hook.fishManager.ResetAllTargets(null);
    }

}
