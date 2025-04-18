using MBT;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

[AddComponentMenu("")]
[MBTNode(name = "Wander")]
public class MBT_Wander : Leaf
{
    public float wanderRadius;
    public float wanderDistance;
    public float wanderJitter;
    public float movementSpeed = 5;
    public GameObject wander_shpere;

    private Rigidbody rigid;
    private float angle = 0;

    private float startingY;
    public float yLerpSpeed = 2f;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody>();
    }

    public override void OnEnter()
    {
        base.OnEnter();
        startingY = transform.position.y;
    }

    public override NodeResult Execute()
    {
        float angleOffset = Random.Range(-0.10f, 0.10f);

        Vector3 projection = new Vector3
        {
            x = Mathf.Cos(angle),
            z = Mathf.Sin(angle)
        };

        projection *= wanderRadius;
        angle += angleOffset;

        Vector3 endPoint = wander_shpere.transform.position + projection;

        Debug.DrawLine(wander_shpere.transform.position, endPoint, Color.red);

        // Smooth Y position
        float currentY = transform.position.y;
        float targetY = Mathf.Lerp(currentY, startingY, yLerpSpeed * Time.deltaTime);

        Vector3 direction = new Vector3(endPoint.x, targetY, endPoint.z) - transform.position;
        Vector3 velocity = direction.normalized * movementSpeed;

        rigid.velocity = new Vector3(velocity.x, rigid.velocity.y, velocity.z);

        if (rigid.velocity != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(new Vector3(rigid.velocity.x, 0, rigid.velocity.z), Vector3.up);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, 50 * Time.deltaTime);
        }

        return NodeResult.success;
    }
}
