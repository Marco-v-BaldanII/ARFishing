
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

    float escape_time = 4f; // TODO : replace or involve fish's particular characteristics like strength
    public override void Process()
    {
        if (escaping)
        {
            current_fish.ShowExclamationMark(true);
            escape_time -= Time.deltaTime;
            if(escape_time < 0)
            {
                _cts.Cancel();
                GameObject.Destroy(hook.currentFish.gameObject); // Destroy fish , "it escapes"
                escape_time = 4f;
                CallTransition(State.IN_WATER, this);
            }
        }
    }

    private CancellationTokenSource _cts;

    public async override void Enter()
    {
        current_fish = hook.currentFish;
        direction = Direction.None;
        hook.SetVelocityToRod();
        escaping = false;

        try
        {
            _cts.Cancel();
            await AttemptBreakFree(_cts.Token);
        }
        catch (TaskCanceledException)
        {
            Debug.Log("failed to catch fiish");
        }
    }

    bool escaping = false;

    private async Task AttemptBreakFree(CancellationToken token)
    {

            await Task.Delay(Random.Range(1200, 4200));

            int id = Random.Range(0, 2);
            Vector3 velocity;
            if (id == 0) { velocity = new Vector3(-rigid.velocity.z, rigid.velocity.y, rigid.velocity.x).normalized; direction = Direction.Left; }
            else { velocity = new Vector3(rigid.velocity.z, rigid.velocity.y, -rigid.velocity.x).normalized; direction = Direction.Right; }

            if (current_fish) { current_fish.ShowExclamationMark(true); }
            rigid.AddForce(velocity * 2, ForceMode.Impulse);
        escaping = true;

    }

    public async void RedirectFishLeft(float degrees)
    {
        if (direction == Direction.Left)
        {
            Debug.Log("succesfully redirected fisssh");
            escape_time = 3f;
            escaping = false;
            if (current_fish) { current_fish.ShowExclamationMark(false); current_fish.ShowParticles(); }
            hook.SetVelocityToRod();
            rigid.AddForce(rigid.velocity * 2, ForceMode.Impulse);
            direction = Direction.None;
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

    public async void RedirectFishRight(float degrees)
    {
        if (direction == Direction.Right)
        {
            Debug.Log("succesfully redirected fisssh");
            escape_time = 3f;
            escaping = false;
            if (current_fish) { current_fish.ShowExclamationMark(false); current_fish.ShowParticles(); }
            hook.SetVelocityToRod();
            rigid.AddForce(rigid.velocity * 2, ForceMode.Impulse);
            direction = Direction.None;
            try
            {
                _cts.Cancel();
                await AttemptBreakFree(_cts.Token);
            }
            catch (TaskCanceledException)
            {
                Debug.Log("failed to catch fiish");
            }
        }
    }

    public override void OnAreaEnter(Collider collision)
    {
        if (collision.CompareTag("Rod"))
        {
            _cts.Cancel();
            // Fish catched
            CatchPannel.instance.Show();
            GameObject.Destroy(hook.currentFish.gameObject);
            CallTransition(State.ON_ROD, this);
        }
    }

}
