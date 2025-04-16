using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class WaterPlacer : MonoBehaviour
{
    public GameObject WaterPrefab;

    [SerializeField]
    ARRaycastManager m_RaycastManager;

    List<ARRaycastHit> m_Hits = new List<ARRaycastHit>();

    void Update()
    {
        if (Input.touchCount == 0)
            return;

        Touch a_touch = Input.GetTouch(0);

        if (a_touch.phase != TouchPhase.Ended) { return; }

        if (m_RaycastManager.Raycast(Input.GetTouch(0).position, m_Hits))
        {
            print("RayCast HitSomething");
            foreach (var hit in m_Hits) 
            {
                HandleRaycast(hit); 
            }
        }
        print("RayCast Hit Nothing");

    }

    void HandleRaycast(ARRaycastHit hit)
    {
        if (hit.trackable is ARPlane plane)
        {
            print($"Hit a plane with alignment {plane.alignment}");

            if (plane.alignment == UnityEngine.XR.ARSubsystems.PlaneAlignment.HorizontalUp)
            {
                print("Hit a Floor");
                Vector3 hitPosition = hit.pose.position;
                Instantiate(WaterPrefab, hitPosition, Quaternion.identity);
            }
        }
        else
        {
            print($"Raycast hit a {hit.hitType}");
        }
    }
}
