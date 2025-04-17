using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using MBT;

[AddComponentMenu("")]
[MBTNode(name = "Pursuit")]
public class MBT_Pursuit : Leaf
{

    public Rigidbody rigid;
    public float slowDownRadius;
    public Vector3 targetPos;

    public string targetVarName = "target";

    private Blackboard board;

    public float speed = 5f;


    void Awake()
    {
        board = GetComponent<Blackboard>();
    }


    public override NodeResult Execute()
    {

        if (board.GetVariable<TransformVariable>(targetVarName).Value != null)
        {
            var trans = board.GetVariable<TransformVariable>(targetVarName).Value;
            targetPos = board.GetVariable<TransformVariable>(targetVarName).Value.position;

            Vector3 direction = (targetPos - rigid.transform.position).normalized;

            rigid.velocity = new Vector3(direction.x, direction.y, direction.z) * speed;
            // ARRIVE
            var distance = Vector3.Distance(targetPos, transform.position);

            if (distance < slowDownRadius) /*Slow down when get closer than "slowDownRadius" */
            {
                rigid.velocity *= distance / slowDownRadius;
                if (distance < slowDownRadius / 2)
                {
                    rigid.velocity = Vector3.zero;
                    return NodeResult.failure; /* this way catched state can execute */

                }
            }
        }
        return NodeResult.success;
    }
}