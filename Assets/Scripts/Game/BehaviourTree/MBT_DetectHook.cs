using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MBT;

[AddComponentMenu("")]
[MBTNode(name = "DetectTarget")]
public class MBT_DetectHook : Leaf
{

    Camera frustum;
    Blackboard board;
    public LayerMask mask;


    void Awake()
    {
        frustum = GetComponentInChildren<Camera>();
        board = GetComponent<Blackboard>();
    }


    public override NodeResult Execute()
    {
        bool detect = false;
        Transform detectTrasnform = null;
        TransformVariable trs = board.GetVariable<TransformVariable>("target");


        Collider[] colliders = Physics.OverlapSphere(transform.position, frustum.farClipPlane, mask);
        Plane[] planes = GeometryUtility.CalculateFrustumPlanes(frustum);
        foreach (Collider col in colliders)
        {
            if (col.CompareTag("Hook") && col.isTrigger == false && GeometryUtility.TestPlanesAABB(planes, col.bounds))
            {
                RaycastHit hit;
                Ray ray = new Ray();
                ray.origin = transform.position;
                ray.direction = (col.transform.position - transform.position).normalized;
                ray.origin = ray.GetPoint(frustum.nearClipPlane);

                Hook hook = col.gameObject.GetComponent<Hook>();
                if(hook != null && hook.currentFish == null)
                {

                }
                else { 
                    return NodeResult.failure ; 
                }


                if (Physics.Raycast(ray, out hit, frustum.farClipPlane, mask))
                {
                    print("Fish has target");

                    if(hit.collider.gameObject.GetComponent<Hook>() == null)
                    {
                        return NodeResult.failure ;
                    }

                    detectTrasnform = hit.collider.transform;
                    break;

                }
            }

        }


        trs.Value = detectTrasnform;
        return NodeResult.failure;
    }
}
