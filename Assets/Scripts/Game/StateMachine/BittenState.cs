using Codice.CM.Common;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;
using UnityEngine;

public class BittenState : IState
{
    Hook hook;
    Rigidbody rigid;
    Fish current_fish;
    Direction direction;
    public BittenState(Hook pablo)
    {
        hook = pablo;
        rigid = hook.GetComponent<Rigidbody>();
        current_fish = hook.currentFish;
        GyroManager.instance.steer_left_event.AddListener(RedirectFishLeft);
        GyroManager.instance.steer_right_event.AddListener(RedirectFishRight);
        _cts = new CancellationTokenSource();
        direction = Direction.None;
    }

    public override void Process()
    {
        
    }

    private CancellationTokenSource _cts;

    public async override void Enter()
    {
        current_fish = hook.currentFish;
        direction = Direction.None;
        hook.SetVelocityToRod();

        try
        {
            await AttemptBreakFree(_cts.Token);
        }
        catch (TaskCanceledException)
        {
            Debug.Log("failed to catch fiish");
        }
    }

    private async Task AttemptBreakFree(CancellationToken token)
    {

            await Task.Delay(Random.Range(1200, 4200));

            int id = Random.Range(0, 2);
            Vector3 velocity;
            if (id == 0) { velocity = new Vector3(-rigid.velocity.z, rigid.velocity.y, rigid.velocity.x).normalized; direction = Direction.Left; }
            else { velocity = new Vector3(rigid.velocity.z, rigid.velocity.y, -rigid.velocity.x).normalized; direction = Direction.Right; }

            if (current_fish) { current_fish.ShowExclamationMark(true); }
            rigid.velocity = velocity * 0.2f;

    }

    public async void RedirectFishLeft()
    {
        if (direction == Direction.Left)
        {
            Debug.Log("succesfully redirected fisssh");
            if (current_fish) { current_fish.ShowExclamationMark(false); }
            hook.SetVelocityToRod();
            try
            {
                await AttemptBreakFree(_cts.Token);
            }
            catch (TaskCanceledException)
            {
                Debug.Log("failed to catch fiish");
            }
        }
    }

    public async void RedirectFishRight()
    {
        if (direction == Direction.Right)
        {
            Debug.Log("succesfully redirected fisssh");
            if (current_fish) { current_fish.ShowExclamationMark(false); }
            hook.SetVelocityToRod();
            try
            {
                await AttemptBreakFree(_cts.Token);
            }
            catch (TaskCanceledException)
            {
                Debug.Log("failed to catch fiish");
            }
        }
    }


}
