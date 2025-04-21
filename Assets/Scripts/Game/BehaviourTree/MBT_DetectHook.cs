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
            if (col.CompareTag("Bait")  && GeometryUtility.TestPlanesAABB(planes, col.bounds))
            {
                RaycastHit hit;
                Ray ray = new Ray();
                ray.origin = transform.position;
                ray.direction = (col.transform.position - transform.position).normalized;
                ray.origin = ray.GetPoint(frustum.nearClipPlane);



                if (Physics.Raycast(ray, out hit, frustum.farClipPlane, mask))
                {
                    print("Fish has target");

 

                    detectTrasnform = hit.collider.transform;
                    break;

                }
            }

        }

        if ( detectTrasnform != null && detectTrasnform.CompareTag("Bait") && detectTrasnform.gameObject.name != "FishingRod")
        {
            Hook.instance.DeactivateBait(); // multpile fishes dont go for the same
            trs.Value = detectTrasnform;
        }
        return NodeResult.failure;
    }
}
