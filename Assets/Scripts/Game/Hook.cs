using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hook : MonoBehaviour
{
    private Rigidbody rigid;
    public Rod fishing_rod;
    private Collider colllider;

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
    private Fish currentFish;

    private void Start()
    {
        GyroManager.instance.steer_left_event.AddListener(RedirectFishLeft);
        GyroManager.instance.steer_right_event.AddListener(RedirectFishRight);
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

    private void SetVelocityToRod()
    {
        rigid.velocity = (fishing_rod.transform.position - transform.position).normalized * SPEED;
        rigid.velocity = new Vector3(rigid.velocity.x, 0, rigid.velocity.z);
    }

    private IEnumerator AttemptBreakFree()
    {
        SetVelocityToRod();

        while (bitten)
        {
            yield return new WaitForSeconds(Random.Range(1.2f, 4.2f));

            int id = Random.Range(0, 2);
            Vector3 velocity;
            if(id == 0) { velocity = new Vector3(-rigid.velocity.z, rigid.velocity.y, rigid.velocity.x).normalized; direction = Direction.Left; }
            else { velocity =  new Vector3(rigid.velocity.z, rigid.velocity.y, -rigid.velocity.x).normalized; direction = Direction.Right; }

            if (currentFish) { currentFish.ShowExclamationMark(true); }

            rigid.velocity = velocity * 0.2f;

            break;
            // yield return null;
            // perpendicularCounterClockwise = new Vector3(-rigid.velocity.z, rigid.velocity.y, rigid.velocity.x).normalized;
            // rigid.velocity = perpendicularCounterClockwise.normalized * 0.2f;
            //// rigid.velocity  = Quaternion.AngleAxis(90, transform.up) * rigid.velocity;
            // break;

        }
    }


    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Water"))
        {
           rigid.velocity = Vector3.Lerp(rigid.velocity, Vector3.zero, Time.deltaTime);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Fish"))
        {
            other.transform.parent = this.transform;
            bitten = true;
            rigid.useGravity = false;
            colllider.enabled = false;
            StartCoroutine(AttemptBreakFree());
            Fish fish = other.gameObject.GetComponent<Fish>();
            if (fish) { currentFish = fish; }   
        }
    }

    public void RedirectFishLeft()
    {
        if(direction == Direction.Left)
        {
            print("succesfully redirected fisssh");
            if (currentFish) { currentFish.ShowExclamationMark(false); }
            SetVelocityToRod();
            StartCoroutine(AttemptBreakFree());
        }
    }

    public void RedirectFishRight()
    {
        if (direction == Direction.Right)
        {
            print("succesfully redirected fisssh");
            if (currentFish) { currentFish.ShowExclamationMark(false); }
            SetVelocityToRod();
            StartCoroutine(AttemptBreakFree());
        }
    }

}

public enum Direction
{
    Left,
    Right,
    None
}