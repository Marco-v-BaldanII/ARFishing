using System.Threading.Tasks;
using System.Threading;
using UnityEngine;

public class BittenState : IState
{
    private Hook hook;
    private Rigidbody rigid;
    private Fish current_fish;
    private Direction direction;
    private CancellationTokenSource _cts;
    private bool escaping = false;
    private float escape_time = 4f;

    public BittenState(Hook pablo)
    {
        hook = pablo;
        rigid = hook.GetComponent<Rigidbody>();

        GyroManager.instance.steer_left_event.AddListener(RedirectFishLeft);
        GyroManager.instance.steer_right_event.AddListener(RedirectFishRight);

        _cts = new CancellationTokenSource();
        direction = Direction.None;
    }

    public override void Process()
    {
        if (current_fish == null)
        {
            CallTransition(State.IN_WATER, this);
            return;
        }

        if (escaping)
        {
            current_fish.ShowExclamationMark(true);
            escape_time -= Time.deltaTime;

            if (escape_time < 0)
            {
                CancelCurrentTask();

                if (hook.currentFish != null)
                {
                    GameObject.Destroy(hook.currentFish.gameObject);
                    hook.currentFish = null;
                }

                escape_time = 4f;
                CallTransition(State.IN_WATER, this);
            }
        }
    }

    public override void Enter()
    {
        current_fish = hook.currentFish;

        if (current_fish == null)
        {
            CallTransition(State.IN_WATER, this);
            return;
        }

        direction = Direction.None;
        hook.SetVelocityToRod();
        escaping = false;
        GameManager.Instance.BlinkTutorial();

        CancelCurrentTask();
        _cts = new CancellationTokenSource();

        StartBreakFreeAttempt();
    }

    private void StartBreakFreeAttempt()
    {
        if (_cts == null || _cts.IsCancellationRequested)
        {
            _cts = new CancellationTokenSource();
        }

        AttemptBreakFreeAsync(_cts.Token);
    }

    private async void AttemptBreakFreeAsync(CancellationToken token)
    {
        try
        {
            await Task.Delay(Random.Range(1200, 4200), token);

            if (token.IsCancellationRequested || current_fish == null)
                return;

            int id = Random.Range(0, 2);
            Vector3 velocity;

            if (id == 0)
            {
                velocity = new Vector3(-rigid.velocity.z, rigid.velocity.y, rigid.velocity.x).normalized;
                direction = Direction.Left;
            }
            else
            {
                velocity = new Vector3(rigid.velocity.z, rigid.velocity.y, -rigid.velocity.x).normalized;
                direction = Direction.Right;
            }

            if (current_fish != null)
            {
                current_fish.ShowExclamationMark(true);
            }

            rigid.AddForce(velocity * 2, ForceMode.Impulse);
            escaping = true;
        }
        catch (TaskCanceledException)
        {
            Debug.Log("PABLO PAGA LA COCA");
        }
        catch (System.Exception e)
        {
            Debug.LogError($"PABLO DONDE ESTAN MIS HIJOS: {e.Message}");
        }
    }

    public void RedirectFishLeft(float degrees)
    {
        if (direction == Direction.Left)
        {
            HandleSuccessfulRedirect();
        }
    }

    public void RedirectFishRight(float degrees)
    {
        if (direction == Direction.Right)
        {
            HandleSuccessfulRedirect();
        }
    }

    private void HandleSuccessfulRedirect()
    {
        Debug.Log("pez redirect");

        escape_time = 3f;
        escaping = false;
        direction = Direction.None;

        if (current_fish != null)
        {
            current_fish.ShowExclamationMark(false);
            current_fish.ShowParticles();
        }

        hook.SetVelocityToRod();
        rigid.AddForce(rigid.velocity * 2, ForceMode.Impulse);

        CancelCurrentTask();
        _cts = new CancellationTokenSource();
        StartBreakFreeAttempt();
    }

    private void CancelCurrentTask()
    {
        if (_cts != null && !_cts.IsCancellationRequested)
        {
            _cts.Cancel();
            _cts.Dispose();
        }
    }

    public override void OnAreaEnter(Collider collision)
    {
        if (collision.CompareTag("Rod"))
        {
            CancelCurrentTask();

            if (hook.currentFish != null)
            {
                GameManager.Instance.IncrementScore(10);
                CatchPannel.instance.Show(hook.currentFish);
                GameObject.Destroy(hook.currentFish.gameObject);
                hook.currentFish = null;
            }

            CallTransition(State.ON_ROD, this);
        }
    }

    public override void Exit()
    {
        CancelCurrentTask();
        escaping = false;
    }

    private void OnDestroy()
    {
        if (GyroManager.instance != null)
        {
            GyroManager.instance.steer_left_event.RemoveListener(RedirectFishLeft);
            GyroManager.instance.steer_right_event.RemoveListener(RedirectFishRight);
        }

        CancelCurrentTask();
    }
}