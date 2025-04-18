using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hook : MonoBehaviour
{
    private Rigidbody rigid;
    public Rod fishing_rod;
    private Collider colllider;

    public FishManager fishManager;

    public float launchVelocity = 3f;

    // Start is called before the first frame update
    void Awake()
    {
        fishing_rod = GetComponentInParent<Rod>();
        rigid = GetComponent<Rigidbody>();
        colllider = GetComponentInParent<Collider>();
    }

    bool bitten = false;
    bool deviating_direction = true;
    Vector3 perpendicularCounterClockwise;
    Vector3 perpendicularClockwise;

    private Direction direction = Direction.None;

    private float SPEED = 0.6f;
    public Fish currentFish;

    public HookStateMachine machine;

    public Transform point; // point where the hook is when on rod

    public Collider baitCollider;

    private void Start()
    {
        machine = GetComponent<HookStateMachine>();
        GyroManager.instance.pull_event.AddListener(ReelIn);
    }

    // Update is called once per frame
    void Update()
    {
        if (bitten && rigid != null)
        {
            Debug.DrawLine(transform.position, rigid.velocity * 10, Color.red);

            // 90 degrees counter-clockwise rotation around the global Y-axis
             
            //Debug.DrawLine(transform.position, perpendicularCounterClockwise * 10, Color.green);

            // 90 degrees clockwise rotation around the global Y-axis
            
            //Debug.DrawLine(transform.position, perpendicularClockwise * 10, Color.yellow);
        }



    }

    // Reel in the hook
    public void SetVelocityToRod()
    {
        rigid.velocity = (fishing_rod.transform.position - transform.position).normalized * SPEED;
        rigid.velocity = new Vector3(rigid.velocity.x, 0, rigid.velocity.z);
        // activate sparkle
    }

    public void StartLaunchRoutine()
    {
        StartCoroutine(ApplyLaunchForce());
    }

    private IEnumerator ApplyLaunchForce()
    {
        float launchDuration = 0.1f; // Adjust this duration as needed
        float elapsedTime = 0f;
        Vector3 forwardDirection = this.fishing_rod.transform.forward;
        Vector3 upwardDirection = Vector3.up;
        float upwardForceFactor = 0.5f;
        Vector3 launchDirection = (forwardDirection + upwardDirection * upwardForceFactor).normalized;
        float forceMagnitude = 1.5f; // Your original force magnitude

        while (elapsedTime < launchDuration)
        {
            rigid.AddForce(launchDirection * forceMagnitude, ForceMode.Force); // Use ForceMode.Force for continuous application
            elapsedTime += Time.fixedDeltaTime; // Use FixedDeltaTime for physics updates
            yield return new WaitForFixedUpdate(); // Wait for the next physics update
        }
    }


    public void ReelIn(float degrees)
    {
        if(machine.checkState != State.ON_ROD && machine.checkState != State.THROWN)
        {
            machine.OnChildTransitionEvent(State.THROWN);
          var distance = Vector3.Distance(transform.position, point.position);
          transform.DOMove(point.transform.position, 0.15f * distance).OnComplete(() => machine.OnChildTransitionEvent(State.ON_ROD));
        }
    }

}

public enum Direction
{
    Left,
    Right,
    None
}