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
    public Fish currentFish;

    private void Start()
    {

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
    }


}

public enum Direction
{
    Left,
    Right,
    None
}