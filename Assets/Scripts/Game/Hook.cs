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
    Vector3 perpendicularCounterClockwise;
    Vector3 perpendicularClockwise;

    // Update is called once per frame
    void Update()
    {
        if (bitten && rigid != null)
        {
            Debug.DrawLine(transform.position, rigid.velocity * 10, Color.red);

            // 90 degrees counter-clockwise rotation around the global Y-axis
             
            Debug.DrawLine(transform.position, perpendicularCounterClockwise * 10, Color.green);

            // 90 degrees clockwise rotation around the global Y-axis
            
            //Debug.DrawLine(transform.position, perpendicularClockwise * 10, Color.yellow);
        }
    }

    private IEnumerator AttemptBreakFree()
    {
        rigid.velocity = (fishing_rod.transform.position - transform.position).normalized * 0.2f;
        rigid.velocity = new Vector3(rigid.velocity.x, 0, rigid.velocity.z);

        while (bitten)
        {
            yield return null;
            perpendicularCounterClockwise = new Vector3(-rigid.velocity.z, rigid.velocity.y, rigid.velocity.x).normalized;
            rigid.velocity = perpendicularCounterClockwise.normalized * 0.2f;
           // rigid.velocity  = Quaternion.AngleAxis(90, transform.up) * rigid.velocity;
            break;

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
        }
    }



}
